using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Client.Accu;
using Weather.Client.Accu.Wind;

namespace WeatherApp.Tests.Accu
{
    public class AccuWebServiceTests
    {

        [Test]
        public void EndDateStartDate_GreaterOrEqual()
        {
           var result = AcuWeatherWebService.GetWind(new AcuWindRequest()
            {
                Country = "Turkey",
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date.AddDays(1)
           });

            Assert.GreaterOrEqual(result.Items.Count, 1);
        }


        [Test]
        public void EndDateStartDate_Throw_If_Empty_Result()
        {
            Assert.Throws<NoNullAllowedException>(new TestDelegate(() =>
            {
            var result = AcuWeatherWebService.GetWind(new AcuWindRequest()
            {
                Country = "Turkey",
                StartDate = DateTime.Now.Date.AddDays(1),
                EndDate = DateTime.Now.Date.AddDays(2)
            });
            }));
        }

    }
}
