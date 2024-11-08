using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherOutlook.Utilities
{
    public static class utils
    {
        public const string _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
        public static bool IsValidZipCode(string zipCode)
        {
            var validZipCode = true;
            if ((!Regex.Match(zipCode, _usZipRegEx).Success))
            {
                validZipCode = false;
            }
            return validZipCode;
        }
    }
}
