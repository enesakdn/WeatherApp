using MongoDB.Driver;
using WeatherApp.Models.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace WeatherApp.DataAccess
{
    public class WeatherDataRepository
    {
        private readonly IMongoCollection<BaseWeatherData> _weatherDataCollection;

        public WeatherDataRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("WeatherAppDb");
            _weatherDataCollection = database.GetCollection<BaseWeatherData>("WeatherData");
        }

        public async Task SaveWeatherDataAsync(BaseWeatherData weatherData)
        {
            await _weatherDataCollection.InsertOneAsync(weatherData);
        }

        public async Task SaveWeatherDataBatchAsync(IEnumerable<BaseWeatherData> weatherDataList)
        {
            await _weatherDataCollection.InsertManyAsync(weatherDataList);
        }

        public async Task<List<BaseWeatherData>> GetWeatherDataAsync(string city, DateTime? startDate, DateTime? endDate)
        {
            var filterBuilder = Builders<BaseWeatherData>.Filter;
            var filter = filterBuilder.Eq(x => x.Sehir, city);

            if (startDate.HasValue && endDate.HasValue)
            {
                filter = filter & filterBuilder.Gte(x => x.UtcDateTime, startDate.Value)
                                    & filterBuilder.Lte(x => x.UtcDateTime, endDate.Value);
            }

            return await _weatherDataCollection.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateWeatherDataAsync(string city, BaseWeatherData weatherData)
        {
            var filterBuilder = Builders<BaseWeatherData>.Filter;
            var filter = filterBuilder.Eq(x => x.Sehir, city);

            var existingData = await _weatherDataCollection.Find(filter)
                                                .SortByDescending(x => x.UtcDateTime)
                                                .FirstOrDefaultAsync();

            if (existingData != null)
            {
        
                weatherData.Id = existingData.Id;
                var replaceResult = await _weatherDataCollection.ReplaceOneAsync(
                    filterBuilder.Eq(x => x.Id, existingData.Id),
                    weatherData);
                return replaceResult.ModifiedCount > 0;
            }
            else
            {
                await _weatherDataCollection.InsertOneAsync(weatherData);
                return true;
            }
        }
    }
}