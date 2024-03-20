// See https://aka.ms/new-console-template for more information
using RuleEngineSaple;

//Sample rule
//RuleEvaluator.ExecuteDiscount();

//Eligiblity
RuleEvaluator.ExecuteEligibility();

//Eligiblity Plus
RuleEvaluator.ExecuteEligibilityPlus();

//Chaining rule
RuleEvaluator.ExecuteEligibilityChain();

//Rule with actions
RuleEvaluator.ExecuteRuleWithAction();