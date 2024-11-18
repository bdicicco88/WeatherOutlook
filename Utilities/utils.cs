using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;

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

        public static string GetLongLatFromZip(string zipCode)
        {
            //data from https://gist.github.com/erichurst/7882666
            using var sr = new StreamReader(@".\Utilities\Data\USZipCodeData.csv");
            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<GeoDataByZip>().ToList();

            var data = records.Where(s => s.Zip == zipCode).First();
            
            return $"{data.Lat},{data.Lng}";
        }
    }
}
