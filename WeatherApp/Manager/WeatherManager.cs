using WeatherApp.Models.Core;
using WeatherApp.Services;
using WeatherApp.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WeatherApp.Managers
{
    public class WeatherManager
    {
        private readonly ApiWeatherWebService _weatherService;
        private readonly WeatherDataRepository _weatherDataRepository;
        private const int _cacheExpirationMinutes = 5; 

        public WeatherManager(ApiWeatherWebService weatherService, WeatherDataRepository weatherDataRepository)
        {
            _weatherService = weatherService;
            _weatherDataRepository = weatherDataRepository;
        }

        public async Task<BaseWeatherData> GetCurrentWeatherDataWithCache(string city)
        {
            var lastWeatherData = await _weatherDataRepository.GetWeatherDataAsync(city, DateTime.UtcNow.AddMinutes(-_cacheExpirationMinutes), DateTime.UtcNow);

            if (lastWeatherData != null && lastWeatherData.Count > 0)
            {
                return lastWeatherData[0]; 
            }
            else
            {
              
                var currentWeatherResponse = await _weatherService.GetCurrentWeatherAsync(city);
                var baseWeatherData = GenerateBaseWeatherDataFromCurrent(currentWeatherResponse, city);

                if (baseWeatherData != null)
                {
                  
                    await _weatherDataRepository.UpdateWeatherDataAsync(city, baseWeatherData);
                    return baseWeatherData;
                }

                return null;
            }
        }

        public async Task<BaseWeatherData> GetCurrentWeatherData(string city)
        {
            var currentWeatherResponse = await _weatherService.GetCurrentWeatherAsync(city);
            return GenerateBaseWeatherDataFromCurrent(currentWeatherResponse, city);
        }

        public async Task<List<BaseWeatherData>> GetPastFiveDaysWeatherData(string city)
        {
            var historyResponses = await _weatherService.GetPastFiveDaysWeatherAsync(city);
            var pastWeatherDataList = new List<BaseWeatherData>();

            foreach (var historyResponse in historyResponses)
            {
                var baseWeatherData = GenerateBaseWeatherDataFromHistory(historyResponse, city);
                if (baseWeatherData != null)
                {
                    pastWeatherDataList.Add(baseWeatherData);
                }
            }

            return pastWeatherDataList;
        }

        private BaseWeatherData GenerateBaseWeatherDataFromCurrent(CurrentWeatherResponse response, string city)
        {
            if (response?.Location == null || response?.Current == null)
            {
                return null;
            }

            return new BaseWeatherData
            {
                UtcDateTime = DateTimeOffset.FromUnixTimeSeconds(response.Current.last_updated_epoch).UtcDateTime,
                Sehir = city,
                Ulke = response.Location.country,
                Enlem = response.Location.lat,
                Boylam = response.Location.lon,
                Ortalama_Sicaklik = response.Current.temp_c,
                Max_Sicaklik = response.Current.feelslike_c,
                Min_Sicaklik = response.Current.dewpoint_c,
                HavaKodu = response.Current.condition.code.ToString(),
                HavaAciklamasi = response.Current.condition.text,
                Ruzgar_hizi = response.Current.wind_kph,
                Nem = response.Current.humidity,
            };
        }

        private BaseWeatherData GenerateBaseWeatherDataFromHistory(WeatherHistoryResponse response, string city)
        {
            if (response?.location == null || response?.forecast?.forecastday?.FirstOrDefault()?.day == null)
            {
                return null;
            }

            var forecastDay = response.forecast.forecastday.First().day;

            return new BaseWeatherData
            {
                UtcDateTime = DateTimeOffset.FromUnixTimeSeconds(response.forecast.forecastday.First().date_epoch).UtcDateTime,
                Sehir = city,
                Ulke = response.location.country,
                Enlem = response.location.lat,
                Boylam = response.location.lon,
                Ortalama_Sicaklik = forecastDay.avgtemp_c,
                Max_Sicaklik = forecastDay.maxtemp_c,
                Min_Sicaklik = forecastDay.mintemp_c,
                HavaKodu = forecastDay.condition.code.ToString(),
                HavaAciklamasi = forecastDay.condition.text,
                Ruzgar_hizi = forecastDay.maxwind_kph,
                Nem = forecastDay.avghumidity,
            };
        }
    }
}