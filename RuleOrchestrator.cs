// See https://aka.ms/new-console-template for more information
using Microsoft.IdentityModel.Tokens;
using RuleEngineSample;
using RuleEngineSample.Data;
using RuleEngineSaple;

////Sample rule
//RuleEvaluator.ExecuteDiscount();

////Eligiblity
RuleEvaluator.ExecuteEligibility();

////Eligiblity Plus
//RuleEvaluator.ExecuteEligibilityPlus();

////Chaining rule
//RuleEvaluator.ExecuteEligibilityChain();

////Rule with actions
//RuleEvaluator.ExecuteRuleWithAction();

namespace RuleEngineSample
{
    class RuleOrchestrator
    {
        private readonly RulesEngineDbContext _context;
        public RuleOrchestrator(RulesEngineDbContext rulesEngineDbContext)
        {
           _context = rulesEngineDbContext;
        }

        static void Main(string[] args)
        {
            //Fetching Data Using ADO.NET i.e RuleEngineDBManager
            string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=RulesEngineEditorDB;Integrated Security=true;";
            RuleEngineDBManager dBManager = new RuleEngineDBManager(connectionstring);
            RulesEngineDbContext rulesEngineDbContext = new RulesEngineDbContext();
            //dBManager.displayworkflowtable();
            //dBManager.displayruletable();
            //dBManager.displaydemographic();
            dBManager.Getdemographic();


            //Fetching Data Using EF i.e. RulesEngineDbContext
            if(rulesEngineDbContext != null)
            {
                var demographic = rulesEngineDbContext.GetdemographicEF();
                foreach (var item in demographic)
                {
                    Console.WriteLine(item.First_Name);
                }
            }
            if(rulesEngineDbContext != null)
            {
                var jsonrule = rulesEngineDbContext.GetjsonDataModels();
                foreach (var item in jsonrule)
                {
                    Console.WriteLine(item.JsonData);
                }
            }

            //Rules Evaluation----------------------------------------------------------------------------------------
            RuleEvaluationService service1 = new RuleEvaluationService(RuleLibrary.DiscountRule);
            service1.EvaluateRule();

            RuleEvaluationService service2 = new RuleEvaluationService(RuleLibrary.ElgibilityRule);
            service2.EvaluateRule();

            RuleEvaluationService service3 = new RuleEvaluationService(RuleLibrary.ElgibilityPlusRule);
            service3.EvaluateRule();

            RuleEvaluationService service4 = new RuleEvaluationService(RuleLibrary.EligibilityChain);
            service4.EvaluateRuleWithActionFlow("GeneralEligibility");

            RuleEvaluationService service5 = new RuleEvaluationService(RuleLibrary.RuleWithAction);
            service5.EvaluateRuleWithActionFlow("ComplexRuleWithAction");
        }
    }
}