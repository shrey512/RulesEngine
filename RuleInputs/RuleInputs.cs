using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineSample.RuleInputs
{
    public class DiscountInputs : IRuleInput
    {
        public string country = "india";
        public int loyaltyFactor = 4;
        public decimal totalPurchasesToDate = 15500;
    }

    public class MemberInput : IRuleInput
    {
        public string name = "Jane Doe";
        public int age = 48;
        public string stateCode = "IL";
        public string extraCode5 = "A2";
        public decimal annualSalary = 312600;
        public int extraInt1 = 3;
        public decimal forzenSalary = 2500;
        public string managerEmployeeNo = "3A";
        public string fullOrPartTimeCode = "FT";
    }

    public class DependentInput : IRuleInput
    {
        public string name = "John Doe";
        public string gender = "M";
        public int age = 42;
    }

}
