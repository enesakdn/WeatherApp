using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Client.Accu.Precipitation;
using Weather.Client.Accu.Temprature;
using Weather.Client.Accu.Wind;

namespace Weather.Client.Accu
{
    public static class AcuWeatherWebService
    {
        public static AcuTempratureResponse GetTemprature(AcuTempratureRequest request)
        {
            // Validate (RestResponse ve dönen datayı doğrulamak)
            return new AcuTempratureResponse()
            {
                items = new List<AcuTempratureResponseItem>()
                {
                    new AcuTempratureResponseItem()
                    {
                        Fehranayt = 120,
                        Hour = 12
                    },
                    new AcuTempratureResponseItem()
                    {
                        Fehranayt = 130,
                        Hour = 13
                    },
                    new AcuTempratureResponseItem()
                    {
                        Fehranayt = 140,
                        Hour = 14
                    },
                }
            };
        }

        public static AcuPrecipitationResponse GetPrecipitation(AcuPrecipitationRequest request)
        {
            return new AcuPrecipitationResponse
            {
                Items = new List<AcuPrecipitationResponseItem>
                {
                    new AcuPrecipitationResponseItem
                    {
                        PrecipitationMm = 2.5,
                        Hour = 12
                    },
                    new AcuPrecipitationResponseItem
                    {
                        PrecipitationMm = 5.0,
                        Hour = 13
                    },
                    new AcuPrecipitationResponseItem
                    {
                        PrecipitationMm = 0.0,
                        Hour = 14
                    }
                }
            };
        }
        public static AcuWindResponse GetWind(AcuWindRequest request)
        {
            var hours = Enumerable.Range(0, 23); // 0, 1 , 2, ..., 23
            var response = new AcuWindResponse();
            var items = hours.Select(a => new AcuWindResponseItem()
            {
                Date = DateTime.Now.Date.AddHours(a),
                SpeedKmh = new Random().Next(2, 50),
                DirectionDegrees = 90 + new Random().Next(120, 130),
                Trend = "Stable"
            });
            response.Items = items.Where(a => a.Date >= request.StartDate && a.Date <= request.EndDate).ToList();

            if (response.Items.Count == 0)
                throw new NoNullAllowedException("No data found for the given date range.");

            return response;
        }
    }
}