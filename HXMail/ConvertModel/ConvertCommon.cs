using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMConvert
{
     public class ConvertCommon
    {
        public static DateTime ConvertFromMailDate(string mailDate)
        {
            if (string.IsNullOrWhiteSpace(mailDate))
                return DateTime.MinValue;
            try
            {
                return Convert.ToDateTime((System.Text.RegularExpressions.Regex.Match(mailDate, ".*[0-9]{4} [0-9]{2}:[0-9]{2}:[0-9]{2}").Value));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
