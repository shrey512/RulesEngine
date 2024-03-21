using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using RulesEngine.Models;
using RulesEngine.Extensions;
using System.Diagnostics.Metrics;
using RulesEngine.HelperFunctions;
using RulesEngine.Actions;
using RuleEngineSample.CustomRuleActions;


namespace RuleEngineSaple
{
    internal static class RuleEvaluator
    {
        public static void ExecuteDiscount()
        {
            Console.WriteLine("Executing the discount rule.........");

            //read the json rule from the repository 
            string jsonRuleExpression = File.ReadAllText(@"./rules/discount.json");

            //instantiate the rules engine 
            List<Workflow> workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);
            RulesEngine.RulesEngine bre = new RulesEngine.RulesEngine(workflow.ToArray(), null);

            //determine the input -- this can be constants, LOVs, nested lists, output from SPs / Funcs, Scalar values, aggregate values etc. 
            var input1 = new
            {
                country = "india",
                loyaltyFactor = 4,
                totalPurchasesToDate = 15500
            };


            List<RuleResultTree> resultList = bre.ExecuteAllRulesAsync("Discount", input1).Result;
            string discountOffered = "No discount offered.";

            resultList.OnSuccess((eventName) =>
            {
                discountOffered = eventName;
            });

            resultList.OnFail(() =>
            {
                discountOffered = "The user is not eligible for any discount.";
            });

            Console.WriteLine(discountOffered);
        }

        public static void ExecuteEligibility()
        {
            Console.WriteLine("Executing the eligibility rule.........");

            //read the json rule
            string jsonRuleExpression = File.ReadAllText(@"./rules/eligibility.json");

            //instantiate the rules engine 
            List<Workflow> workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);
            RulesEngine.RulesEngine bre = new RulesEngine.RulesEngine(workflow.ToArray(), null);

            //determine the input -- this can be constants, LOVs, nested lists, output from SPs / Funcs, Scalar values, aggregate values etc. 
            var input1 = new
            {
                name = "Jane Doe",
                age = 48
            };
            RuleParameter member = new RuleParameter("member", input1);


            var input2 = new
            {
                name = "John Doe",
                gender = "M",
                age = 42
            };
            RuleParameter dependent = new RuleParameter("dependent", input2);


            List<RuleResultTree> resultList = bre.ExecuteAllRulesAsync("Eligibility", member, dependent).Result;
            string memberEligibleFor = string.Empty;

            resultList.ForEach(evalItem =>
            {
                if (evalItem.IsSuccess)
                {
                    memberEligibleFor += evalItem.Rule.SuccessEvent + ",";
                }
            });

            if (!string.IsNullOrEmpty(memberEligibleFor))
            {
                Console.WriteLine(memberEligibleFor);
            }
            else
            {
                Console.WriteLine("Member is not eligible");
            }
        }


        public static void ExecuteEligibilityPlus()
        {
            Console.WriteLine("Executing the eligibility plus rule.........");

            //read the json rule
            string jsonRuleExpression = File.ReadAllText(@"./rules/eligibilityplus.json");

            //instantiate the rules engine 
            List<Workflow> workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);

            //define the custom setting for the rule where we need the external input functions to be registered
            var reSettings = new ReSettings
            {
                CustomTypes = new Type[] { typeof(ExternalInputFunctions) }
            };

            RulesEngine.RulesEngine bre = new RulesEngine.RulesEngine(workflow.ToArray(), reSettings);

            //determine the input -- this can be constants, LOVs, nested lists, output from SPs / Funcs, Scalar values, aggregate values etc. 
            var input1 = new
            {
                name = "Jane Doe",
                age = 48,
                extraCode5 = "A2",
                annualSalary = 312600,
                extraInt1 = 3
            };
            RuleParameter member = new RuleParameter("member", input1);

            List<RuleResultTree> resultList = bre.ExecuteAllRulesAsync("EligibilityPlus", member).Result;
            string memberEligibleFor = string.Empty;

            resultList.ForEach(evalItem =>
            {
                if (evalItem.IsSuccess)
                {
                    memberEligibleFor += evalItem.Rule.SuccessEvent + ",";
                }
            });

            if (!string.IsNullOrEmpty(memberEligibleFor))
            {
                Console.WriteLine(memberEligibleFor);
            }
            else
            {
                Console.WriteLine("Member is not eligible");
            }
        }

        public async static void ExecuteEligibilityChain()
        {
            Console.WriteLine("Executing the eligibility chain rule.........");

            //read the json rule
            string jsonRuleExpression = File.ReadAllText(@"./rules/eligibilitychain.json");

            //instantiate the rules engine 
            List<Workflow> workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);

            //define the custom setting for the rule where we need the external input functions to be registered
            var reSettings = new ReSettings
            {
                CustomTypes = new Type[] { typeof(ExternalInputFunctions) }
            };

            RulesEngine.RulesEngine bre = new RulesEngine.RulesEngine(workflow.ToArray(), reSettings);

            //determine the input -- this can be constants, LOVs, nested lists, output from SPs / Funcs, Scalar values, aggregate values etc. 
            var input1 = new
            {
                name = "John Doe",
                age = 52,
                stateCode = "IL",
                extraCode5 = "A2",
                annualSalary = 300000,
                extraInt1 = 3
            };
            RuleParameter member = new RuleParameter("member", input1);

            var input2 = new
            {
                name = "Jane Doe",
                gender = "F",
                age = 42
            };
            RuleParameter dependent = new RuleParameter("dependent", input2);
            List<RuleParameter> ruleParams = new List<RuleParameter> { member, dependent };

            var workflowResult = await bre.ExecuteActionWorkflowAsync("GeneralEligibilityChain", "GeneralEligibility", ruleParams.ToArray());
            string memberEligibleFor = string.Empty;

            foreach (var result in workflowResult.Results)
            {
                if (result.IsSuccess)
                {
                    memberEligibleFor += result.Rule.SuccessEvent + ",";
                }
            }

            if (!string.IsNullOrEmpty(memberEligibleFor))
            {
                Console.WriteLine(memberEligibleFor);
            }
            else
            {
                Console.WriteLine("Member is not eligible");
            }
        }

        public async static void ExecuteRuleWithAction()
        {
            Console.WriteLine("Executing the rule with action.........");

            //read the json rule
            string jsonRuleExpression = File.ReadAllText(@"./rules/rulewithaction.json");

            //instantiate the rules engine 
            List<Workflow> workflow = JsonConvert.DeserializeObject<List<Workflow>>(jsonRuleExpression);

            //define the custom setting for the rule where we need the external input functions to be registered
            var reSettings = new ReSettings
            {
                CustomTypes = new Type[] { typeof(ExternalInputFunctions) },
                CustomActions = new Dictionary<string, Func<ActionBase>>{
                                          {"CreditRuleAction", () => new CreditRuleAction() }
                                      }
            };

            RulesEngine.RulesEngine bre = new RulesEngine.RulesEngine(workflow.ToArray(), reSettings);

            //determine the input -- this can be constants, LOVs, nested lists, output from SPs / Funcs, Scalar values, aggregate values etc. 
            var input1 = new
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
            };
            RuleParameter member = new RuleParameter("member", input1);

            List<RuleParameter> ruleParams = new List<RuleParameter> { member };

            var workflowResult = await bre.ExecuteActionWorkflowAsync("RuleWithAction", "ComplexRuleWithAction", ruleParams.ToArray());
            string memberEligibleFor = string.Empty;

            foreach (var result in workflowResult.Results)
            {
                if (result.IsSuccess)
                {
                    memberEligibleFor += result.Rule.SuccessEvent + ",";
                }
            }

            if (!string.IsNullOrEmpty(memberEligibleFor))
            {
                Console.WriteLine(memberEligibleFor);
            }
            else
            {
                Console.WriteLine("Member is not eligible");
            }
        }
    }
}
