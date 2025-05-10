using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Client.Accu.Precipitation
{
    public class AcuPrecipitationWebServiceManager : WeatherPrecipitationWebServiceManager
    {
        public override double Get(DateTime startDate)
        {
      //request
        AcuPrecipitationRequest request = new AcuPrecipitationRequest();
            request.StartDate = DateTime.Now;
      //response
            var response = AcuWeatherWebService.GetPrecipitation(request);
            var precipitation = response.Items.FirstOrDefault().PrecipitationMm;
            return precipitation;
        }
        public override string FormatPrecipitationOutput(double precipitation)
        {
            string status = precipitation switch
            {
                0 => "No Rain",
                < 3 => "Light Rain",
                < 7 => "Moderate Rain",
                _ => "Heavy Rain"
            };

            return $"{precipitation} mm - {status}";
        }
        public override double FormatPrecipitation(double precipitation)
        {
            return Math.Round(precipitation * 2) / 2;
        }
    }
    
    
}
