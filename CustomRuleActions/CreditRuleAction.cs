using RuleEngineSample.RuleInputs;
using RulesEngine.Actions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineSample.CustomRuleActions
{
    public class CreditRuleAction : ActionBase
    {
        public async override ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            var customInput = context.GetContext<string>("customContextInput");
            Console.WriteLine("The custom action is executed - the input provided is {0}", customInput);

            if (ruleParameters != null && ruleParameters.Length > 0)
            {
                MemberInput memberParamValue = (MemberInput)ruleParameters[0].Value;
                Console.WriteLine("Member parameter name value is {0}.", memberParamValue.name);
            }

            return true;
        }
    }
}
