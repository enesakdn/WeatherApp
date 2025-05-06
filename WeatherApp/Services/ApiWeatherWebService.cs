using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using WeatherApp.Models.Core;

namespace WeatherApp.Services
{
    public class ApiWeatherWebService
    {
        private readonly RestClient _restClient;
        private const string _apiKey = "1b05c607ddf74362a61230548250505";
        private const string _historyBaseUrl = "http://api.weatherapi.com/v1/history.json";
        private const string _currentBaseUrl = "http://api.weatherapi.com/v1/current.json";

        public ApiWeatherWebService()
        {
            _restClient = new RestClient();
        }

        private async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"API request failed: {response.ErrorMessage ?? response.Content}");
            }
            return response;
        }

        public async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty", nameof(city));

            try
            {
                var request = new RestRequest(_currentBaseUrl);
                request.AddQueryParameter("key", _apiKey);
                request.AddQueryParameter("q", city);
                request.AddQueryParameter("aqi", "no");

                var response = await ExecuteAsync(request);

                return JsonSerializer.Deserialize<CurrentWeatherResponse>(response.Content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get current weather data: {ex.Message}", ex);
            }
        }

        public async Task<List<WeatherHistoryResponse>> GetPastFiveDaysWeatherAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty", nameof(city));

            var result = new List<WeatherHistoryResponse>();
            var today = DateTime.Today;

            for (int i = 1; i <= 5; i++)
            {
                var date = today.AddDays(-i);

                try
                {
                    var request = new RestRequest(_historyBaseUrl);
                    request.AddQueryParameter("key", _apiKey);
                    request.AddQueryParameter("q", city);
                    request.AddQueryParameter("dt", date.ToString("yyyy-MM-dd"));

                    var response = await ExecuteAsync(request);

                    var weatherData = JsonSerializer.Deserialize<WeatherHistoryResponse>(response.Content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    result.Add(weatherData);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to get weather data for date {date:yyyy-MM-dd}: {ex.Message}", ex);
                }
            }

            return result;
        }
    }
}