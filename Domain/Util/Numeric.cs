using System;

namespace Domain.Util
{
    public static class Numeric
    {
        /* Verify string is numeric*/
        public static bool IsNumeric(object Expression)
        {
            double retNum;
            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        /* Verify string is an Integer*/
        public static bool IsInteger(object Expression)
        {
            long retNum;
            bool isNum = long.TryParse(Convert.ToString(Expression), out retNum);
            return isNum;
        }
    }
}
