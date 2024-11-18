using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherOutlook.Utilities
{
    public class GeoDataByZip
    {
        //ZIP,LAT,LNG
        [Name("ZIP")]
        public string Zip { get; set; }
        [Name("LAT")]
        public float Lat { get; set; }
        [Name("LNG")]
        public float Lng { get; set; }
    }
}
