using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherOutlook.APIClient
{
    public class APIClient
    {
        private readonly RestClient _client;
        private readonly string _token;
        private bool _UseBearerToken;

        public APIClient(string baseUrl, string? token)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
            }

            _client = new RestClient(baseUrl);

            if (!String.IsNullOrEmpty(token))
            {
                _UseBearerToken = true;
                _token = token;
            }
        }

        public async Task<string> GetRequest(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);

            if (_UseBearerToken)
            {
                AddTokenHeader(request);
            }

            var response = await ExecuteRequest(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                Console.WriteLine($"GET Error: {response.StatusCode}");
                return null;
            }
        }

        public async Task<string> PostRequest(string endpoint, object data)
        {
            var request = new RestRequest(endpoint, Method.Post);
            if (_UseBearerToken)
            {
                AddTokenHeader(request);
            }
            request.AddJsonBody(data);

            var response = await ExecuteRequest(request);

            if (response.IsSuccessful)
            {
                return response.Content;
            }
            else
            {
                Console.WriteLine($"POST Error: {response.StatusCode}");
                return null;
            }
        }

        internal async Task<RestResponse> ExecuteRequest(RestRequest request)
        {
            return await _client.ExecuteAsync(request);
        }


        public void AddTokenHeader(RestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {_token}");
        }
    }
}
