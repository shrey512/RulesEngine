// See https://aka.ms/new-console-template for more information
using RuleEngineSample;
using RuleEngineSaple;

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

////Sample rule
//RuleEvaluator.ExecuteDiscount();

////Eligiblity
//RuleEvaluator.ExecuteEligibility();

////Eligiblity Plus
//RuleEvaluator.ExecuteEligibilityPlus();

////Chaining rule
//RuleEvaluator.ExecuteEligibilityChain();

////Rule with actions
//RuleEvaluator.ExecuteRuleWithAction();