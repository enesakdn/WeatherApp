using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Client.Accu.Temprature
{
    public class AcuTempratureWebServiceManager : WeathertTempartureWebServiceManager
    {
        public override double Get(DateTime startDate)
        {
            // Request
            AcuTempratureRequest request = new AcuTempratureRequest();
            request.StartDate = DateTime.Now;

            // Response
            AcuTempratureResponse response = AcuWeatherWebService.GetTemprature(request);

            var fahrenayt = response.items.FirstOrDefault().Fehranayt;

            var celcius = FormatTemperatureSabit(fahrenayt);
            return celcius;

        }

        public override string FormatLocaleDurumaGore(double temperature)
        {
            return $"{temperature} °X" + base.FormatLocaleDurumaGore(temperature); // 120 °F  120 °X
        }
    }
}
