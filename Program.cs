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

    public static async Task<string> GetOutlookReportByZipCode(string zipcode)
    {

        var restClient = new APIClient(Constants.WeatherGovAPIBaseURL);
        var reseponse =  restClient.GetRequest("/gridpoints/IND/59,29/forecast").Result;
        
        ForecastResponse forecastResponse = JsonConvert.DeserializeObject<ForecastResponse>(reseponse);
        
        var sb = new StringBuilder();
        if (forecastResponse != null)
        {
            foreach (var period in forecastResponse.properties.periods.Where(period => period.number < 4))
            {
                sb.Append($"For {period.name}, The forecast is: {period.detailedForecast}\r\n\r\n");
            }
        }
        return sb.ToString();
    }

}
