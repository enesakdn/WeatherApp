using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WeatherApp.DataAccess;
using WeatherApp.Models.Core;
using WeatherApp.Services;

namespace WeatherApp.Services
{
    public class WeatherValidationService
    {
        private readonly WeatherDataRepository _weatherDataRepository;
        private readonly ApiWeatherWebService _weatherService;
        private readonly ILogger<WeatherValidationService> _logger;

        public WeatherValidationService(
            WeatherDataRepository weatherDataRepository,
            ApiWeatherWebService weatherService,
            ILogger<WeatherValidationService> logger)
        {
            _weatherDataRepository = weatherDataRepository;
            _weatherService = weatherService;
            _logger = logger;
        }


        public async Task<ValidationResult> ValidateCurrentWeather(string city)
        {
            try
            {
                var apiWeatherData = await _weatherService.GetCurrentWeatherAsync(city);
                var utcDateTime = DateTimeOffset.FromUnixTimeSeconds(apiWeatherData.Current.last_updated_epoch).UtcDateTime;
                var startTime = utcDateTime.AddMinutes(-50);
                var endTime = utcDateTime.AddMinutes(+50);

                var dbWeatherData = await _weatherDataRepository.GetWeatherDataAsync(
                    city, startTime, endTime);

                if (dbWeatherData == null || dbWeatherData.Count == 0)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Message = $"{city} şehri için veritabanında {utcDateTime} tarihine ait veri bulunamadı.",
                        ApiData = apiWeatherData
                    };
                }
                var dbData = dbWeatherData[0];
                bool isTemperatureValid = Math.Abs(dbData.Ortalama_Sicaklik.GetValueOrDefault() - apiWeatherData.Current.temp_c) < 1.0;
                bool isHumidityValid = dbData.Nem == apiWeatherData.Current.humidity;
                bool isWindSpeedValid = Math.Abs(dbData.Ruzgar_hizi.GetValueOrDefault() - apiWeatherData.Current.wind_kph) < 2.0;

                if (isTemperatureValid && isHumidityValid && isWindSpeedValid)
                {
                    return new ValidationResult
                    {
                        IsValid = true,
                        Message = $"{city} şehri için API ve veritabanı verileri uyumlu.",
                        ApiData = apiWeatherData,
                        DbData = dbData
                    };
                }
                else
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        Message = $"{city} şehri için veri tutarsızlığı: " +
                                    $"Sıcaklık: DB={dbData.Ortalama_Sicaklik}°C, API={apiWeatherData.Current.temp_c}°C, " +
                                    $"Nem: DB={dbData.Nem}%, API={apiWeatherData.Current.humidity}%, " +
                                    $"Rüzgar: DB={dbData.Ruzgar_hizi}kph, API={apiWeatherData.Current.wind_kph}kph",
                        ApiData = apiWeatherData,
                        DbData = dbData
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{city} şehri için hava durumu doğrulama hatası: {ex.Message}");
                return new ValidationResult
                {
                    IsValid = false,
                    Message = $"Doğrulama sırasında hata: {ex.Message}"
                };
            }
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public CurrentWeatherResponse ApiData { get; set; }
        public BaseWeatherData DbData { get; set; }
    }
}