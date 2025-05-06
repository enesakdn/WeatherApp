using Microsoft.AspNetCore.Mvc;
using WeatherApp.Managers;
using WeatherApp.Models.Core;
using WeatherApp.Services;
using System.Threading.Tasks;
namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherManager _weatherManager;
        private readonly WeatherValidationService _validationService;

        public WeatherForecastController(
            WeatherManager weatherManager,
            WeatherValidationService validationService)
        {
            _weatherManager = weatherManager;
            _validationService = validationService;
        }

        [HttpGet("current/{city}")]
        public async Task<ActionResult<BaseWeatherData>> GetCurrentWeather(string city)
        {
            var weatherData = await _weatherManager.GetCurrentWeatherData(city);
            if (weatherData == null)
            {
                return NotFound("Hava durumu verisi bulunamadı.");
            }
            return Ok(weatherData);
        }

        [HttpGet("history/{city}")]
        public async Task<ActionResult<List<BaseWeatherData>>> GetPastFiveDaysWeather(string city)
        {
            var weatherDataList = await _weatherManager.GetPastFiveDaysWeatherData(city);
            if (weatherDataList == null || weatherDataList.Count == 0)
            {
                return NotFound("Geçmiş hava durumu verisi bulunamadı.");
            }
            return Ok(weatherDataList);
        }

        [HttpGet("validate/{city}")]
        public async Task<ActionResult<ValidationResult>> ValidateWeatherData(string city)
        {
            var validationResult = await _validationService.ValidateCurrentWeather(city);
            return Ok(validationResult);
        }
    }
}