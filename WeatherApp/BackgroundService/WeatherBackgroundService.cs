using WeatherApp.DataAccess;
using WeatherApp.Models.Core;
using WeatherApp.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class WeatherBackgroundService : BackgroundService
{
    private readonly ApiWeatherWebService _weatherService;
    private readonly WeatherDataRepository _weatherDataRepository;
    private readonly ILogger<WeatherBackgroundService> _logger;
    private readonly string[] _monitoredCities = { "Istanbul", "Ankara", "Izmir", "Antalya", "Bursa", "Zonguldak" };

    public WeatherBackgroundService(
        ApiWeatherWebService weatherService,
        WeatherDataRepository weatherDataRepository,
        ILogger<WeatherBackgroundService> logger)
    {
        _weatherService = weatherService;
        _weatherDataRepository = weatherDataRepository;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WeatherBackgroundService başlatıldı.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateAllCitiesWeatherData();
                _logger.LogInformation("Hava durumu verileri güncellendi.");

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Arkaplan servisinde hata: {Message}", ex.Message);

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }

    private async Task UpdateAllCitiesWeatherData()
    {
        foreach (var city in _monitoredCities)
        {
            try
            {
                var currentWeather = await _weatherService.GetCurrentWeatherAsync(city);

                var baseWeatherData = new BaseWeatherData
                {
                    UtcDateTime = DateTimeOffset.FromUnixTimeSeconds(currentWeather.Current.last_updated_epoch).UtcDateTime,
                    Sehir = city,
                    Ulke = currentWeather.Location.country,
                    Enlem = currentWeather.Location.lat,
                    Boylam = currentWeather.Location.lon,
                    Ortalama_Sicaklik = currentWeather.Current.temp_c,
                    Max_Sicaklik = currentWeather.Current.feelslike_c,
                    Min_Sicaklik = currentWeather.Current.dewpoint_c,
                    HavaKodu = currentWeather.Current.condition.code.ToString(),
                    HavaAciklamasi = currentWeather.Current.condition.text,
                    Ruzgar_hizi = currentWeather.Current.wind_kph,
                    Nem = currentWeather.Current.humidity
                };


                bool success = await _weatherDataRepository.UpdateWeatherDataAsync(city, baseWeatherData);

                if (success)
                {
                    _logger.LogInformation($"{city} şehri için hava durumu verileri güncellendi.");
                }
                else
                {
                    _logger.LogWarning($"{city} şehri için hava durumu verileri güncellenemedi.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{city} şehri için hava durumu verileri güncellenemedi: {ex.Message}");
            }
        }
    }
}