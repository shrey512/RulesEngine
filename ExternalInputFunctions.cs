using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineSaple
{
    public static class ExternalInputFunctions
    {
        public static bool SalaryCodeCheck(string extraCode5Value, string extraCodeAllowedList)
        {
            if (string.IsNullOrEmpty(extraCode5Value)) return false;

            if(string.IsNullOrEmpty(extraCodeAllowedList)) return false;

            string[] extraCodeAllowedListSplit = extraCodeAllowedList.Split(',');

            return extraCodeAllowedListSplit.Contains(extraCode5Value);

        }

        public static bool ExtraIntCheck(int extraInt1Value, string extraInt1AllowedList)
        {
            if (string.IsNullOrEmpty(extraInt1AllowedList)) return false;

            string[] extraInt1AllowedListSplit = extraInt1AllowedList.Split(',');

            bool result = false;
            int value = 0;

            foreach(string x in extraInt1AllowedListSplit)
            {
                if (int.TryParse(x, out value) && value == extraInt1Value)
                {
                    result = true;
                    break;
                }
            };

            return result;

        }

        public static bool ResidenceStateCheck(string stateCode, string allowedStateCodes)
        {
            if (string.IsNullOrEmpty(stateCode)) return false;

            if (string.IsNullOrEmpty(allowedStateCodes)) return false;

            string[] allowedStateCodeList = allowedStateCodes.Split(',');

            return allowedStateCodeList.Contains(stateCode);
        }

    }
}
