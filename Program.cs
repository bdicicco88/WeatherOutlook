using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using WeatherOutlook;
using WeatherOutlook.APIClient;
using WeatherOutlook.APIClient.Response;
using System.Linq;
using WeatherOutlook.Utilities;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = Constants.title;

        Console.WriteLine(Constants.titleArt);
        RunMenu();
    }

    public static async void RunMenu()
    {
        while (true)
        {
            Console.Write("Check Weather? y|n: ");
            var option = Console.ReadLine();

            if (option.ToLower() != "n")
            {
             await GetOutlook();
            }
            else
            {
                break;
            }
        }
    }

    public static async Task GetOutlook()
    {
        Console.Write("Enter ZipCode: ");
        // Get Users Zip
        var zipcode = Console.ReadLine();

        if (utils.IsValidZipCode(zipcode))
        {
            Console.WriteLine("Lets get the Outlook for zipcode: " + zipcode);
            Console.WriteLine();
            // Display Information:
            var response = await GetOutlookReportByZipCode(zipcode);
            Console.WriteLine(response);
        }
        else
        {
            Console.WriteLine("Invalid Zip Please Enter Aagin...");
        }
    }

    public static Task<string> GetOutlookReportByZipCode(string zipcode)
    {
        var longLatPoints = utils.GetLongLatFromZip(zipcode);

        var sb = new StringBuilder();
        var restClient = new APIClient(Constants.WeatherGovAPIBaseURL);

        //Get response based on long/lat
       var restPointsResponse =  restClient.GetRequest($"/points/{longLatPoints}").Result;
        string forcastsub = null;
        if (restPointsResponse != null)
        {
            PointsResponse points = JsonConvert.DeserializeObject<PointsResponse>(restPointsResponse);

            var getForcast = points.properties.forecast;
            string pattern = @"/gridpoints/[^/]+/\d+,\d+/forecast";

            Match match = Regex.Match(getForcast, pattern);
            if (match.Success)
            {
               forcastsub = match.Value; // Outputs: /gridpoints/IND/57,78/forecast
            }

        }
        var reseponse =  restClient.GetRequest(forcastsub).Result;
        
        if (reseponse != null)
        {
            ForecastResponse forecastResponse = JsonConvert.DeserializeObject<ForecastResponse>(reseponse); 
            if (forecastResponse != null)
            {
                foreach (var period in forecastResponse.properties.periods.Where(period => period.number < 4))
                {
                    sb.Append($"For {period.name}, The forecast is: {period.detailedForecast}\r\n\r\n");
                }
            }
        }
        return Task.FromResult(sb.ToString());
    }

}
