using System;
using System.Linq;
using Weather.Client.Accu.Wind;

namespace Weather.Client.Accu
{
    public class AcuWindWebServiceManager : WeatherWindWebServiceManager
    {
        public override (double Speed, double Direction, string Trend) Get(DateTime startDate)
        {
            var request = new AcuWindRequest();
            request.StartDate = DateTime.Now;

            var response = AcuWeatherWebService.GetWind(request);
            var item = response.Items.FirstOrDefault();
            return (FormatWindSpeed(item.SpeedKmh), item.DirectionDegrees, item.Trend);
        }

        public override string FormatWindOutput(double speed, double direction, string trend)
        { 
            string status = speed switch
            {
                < 10 => "Calm",
                < 30 => "Breezy",
                _ => "Stormy"
            };

            return $"{speed} km/h, {FormatWindDirection(direction)} - Status: {status}, Trend: {trend}";
        }

        public override double FormatWindSpeed(double speed)
        {
            return Math.Round(speed * 2) / 2;
        }

       
        public override string FormatWindDirection(double degrees)
        {
            // burada kısa adlandırma gibi mantıklar ekleyebiliriz. 
           return FormatWindDirection(degrees);

        }
    }
}