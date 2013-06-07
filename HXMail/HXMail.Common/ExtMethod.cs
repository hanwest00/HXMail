using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HXMail.Common
{
    public static class ExtMethod
    {
        public static DateTime ConvertFromMailDate(this DateTime date,string mailDate)
        {
            if (string.IsNullOrWhiteSpace(mailDate))
                return DateTime.MinValue;
            try
            {
                return DateTime.Parse(System.Text.RegularExpressions.Regex.Match(mailDate, ".*[0-9]{4} [0-9]{2}:[0-9]{2}:[0-9]{2}").Value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
