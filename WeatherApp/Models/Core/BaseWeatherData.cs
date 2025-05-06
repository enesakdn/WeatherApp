using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WeatherApp.Models.Core
{
    public class BaseWeatherData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime? UtcDateTime { get; set; } 
        public string Sehir { get; set; } 
        public string Ulke { get; set; }
        public double? Enlem { get; set; } 
        public double? Boylam { get; set; } 
        public double? Ortalama_Sicaklik { get; set; }
        public double? Max_Sicaklik { get; set; }
        public double? Min_Sicaklik { get; set; }
        public string HavaKodu { get; set; } 
        public string HavaAciklamasi { get; set; } 
        public double? Ruzgar_hizi { get; set; } 
        public int? Nem { get; set; } 
     
    }
}