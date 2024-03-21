using Newtonsoft.Json;
using RuleEngineSample.CustomRuleActions;
using RuleEngineSaple;
using RulesEngine.Actions;
using RulesEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuleEngineSample.RuleInputs;

namespace RuleEngineSample
{
    public enum RuleLibrary
    {
        [StringValue(@"./rules/discount.json")]
        DiscountRule = 1,

        [StringValue(@"./rules/eligibility.json")]
        ElgibilityRule = 2,

        [StringValue(@"./rules/eligibilityplus.json")]
        ElgibilityPlusRule = 3,

        [StringValue(@"./rules/eligibilitychain.json")]
        EligibilityChain = 4,

        [StringValue(@"./rules/rulewithaction.json")]
        RuleWithAction = 5
    }

    public class RuleEvaluationService
    {
        private ReSettings _reSettings;
        private RuleLibrary _ruleLibrary;
        private string _ruleSource;
        List<Workflow> _ruleWorkflow;
        RulesEngine.RulesEngine _businesRuleEngine;
        List<RuleParameter> _ruleParameters;
        public RuleEvaluationService(RuleLibrary ruleLibrary)
        {
            this._ruleLibrary = ruleLibrary;
            _reSettings = new ReSettings()
            {
                CustomTypes = new Type[] { typeof(ExternalInputFunctions) },
                CustomActions = new Dictionary<string, Func<ActionBase>>
                {
                    {
                        "CreditRuleAction", () => new CreditRuleAction()
                    }
                }
            };

            _ruleSource = ruleLibrary.GetStringValue();

            this.InitilizeRulesEngine();
            this.InitializeInputs();
        }

        private void InitializeInputs()
        {
            _ruleParameters = new List<RuleParameter>();
            switch (this._ruleLibrary)
            {
                case RuleLibrary.DiscountRule:
                    RuleParameter parameter = new RuleParameter("input1", new DiscountInputs());
                    _ruleParameters.Add(parameter);
                    break;
                case RuleLibrary.ElgibilityRule:
                    RuleParameter memberParameter1 = new RuleParameter("member", new MemberInput());
                    RuleParameter dependentParameter1 = new RuleParameter("dependent", new DependentInput());
                    _ruleParameters.Add(memberParameter1);
                    _ruleParameters.Add(dependentParameter1);
                    break;
                case RuleLibrary.ElgibilityPlusRule:
                    RuleParameter memberParameter2 = new RuleParameter("member", new MemberInput());
                    _ruleParameters.Add(memberParameter2);
                    break;
                case RuleLibrary.EligibilityChain:
                    RuleParameter memberParameter3 = new RuleParameter("member", new MemberInput()
                    {
                        name = "John Doe",
                        age = 52,
                        stateCode = "IL",
                        extraCode5 = "A2",
                        annualSalary = 300000,
                        extraInt1 = 3
                    });
                    RuleParameter dependentParameter3 = new RuleParameter("dependent", new DependentInput()
                    {
                        name = "Jane Doe",
                        gender = "F",
                        age = 42
                    });
                    _ruleParameters.Add(memberParameter3);
                    _ruleParameters.Add(dependentParameter3); break;
                case RuleLibrary.RuleWithAction:
                    RuleParameter memberParameter4 = new RuleParameter("member", new MemberInput()
                    {
                        name = "John Doe",
                        age = 52,
                        stateCode = "IL",
                        extraCode5 = "A2",
                        annualSalary = 300000,
                        extraInt1 = 3,
                        forzenSalary = 2500,
                        managerEmployeeNo = "3A",
                        fullOrPartTimeCode = "FT"
                    });
                    _ruleParameters.Add(memberParameter4);
                    break;
            }
        }

        private void InitilizeRulesEngine()
        {
            if (!File.Exists(_ruleSource)) throw new Exception("The rule source is not found");

            string jsonRuleExpression = File.ReadAllText(_ruleSource);
            if (string.IsNullOrEmpty(jsonRuleExpression)) throw new Exception("Invalid rule definition in the rule file");

            _ruleWorkflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);
            if (_ruleWorkflow == null) throw new Exception("Invalid rule json object");

            _businesRuleEngine = new RulesEngine.RulesEngine(_ruleWorkflow.ToArray(), this._reSettings);
        }

        public void EvaluateRule()
        {
            foreach (var item in _ruleWorkflow)
            {
                List<RuleResultTree> resultList = _businesRuleEngine.ExecuteAllRulesAsync(item.WorkflowName, _ruleParameters.ToArray()).Result;
                resultList.ForEach(evalItem =>
                {
                    if (evalItem.IsSuccess)
                    {
                        Console.WriteLine(evalItem.Rule.SuccessEvent);
                    }
                });
            }
        }

        public async void EvaluateRuleWithActionFlow(string ruleName)
        {
            foreach (var item in _ruleWorkflow)
            {
                var workflowResult = await _businesRuleEngine.ExecuteActionWorkflowAsync(item.WorkflowName, ruleName, _ruleParameters.ToArray());
                foreach (var result in workflowResult.Results)
                {
                    if (result.IsSuccess)
                    {
                        Console.WriteLine(result.Rule.SuccessEvent);
                    }
                }
            }
        }
    }
}
