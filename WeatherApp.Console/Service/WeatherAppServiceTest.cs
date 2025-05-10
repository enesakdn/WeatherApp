using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Client.Accu;
using Weather.Client.Accu.Wind;

namespace WeatherApp.Console.Service
{
    public static class WeatherAppServiceTest
    {
        public static void Execute()
        {

            // Test AccuWebService
            var result = AcuWeatherWebService.GetWind(new AcuWindRequest()
            {
                Country = "Turkey",
                StartDate = DateTime.Now.Date.AddDays(-5),
                EndDate = DateTime.Now.Date.AddDays(1)
            });



            // Test MGMWebService


        }
    }
}
