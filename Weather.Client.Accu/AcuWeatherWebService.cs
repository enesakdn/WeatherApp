using System;
using System.Collections.Generic;
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
            return new AcuWindResponse
            {
                Items = new List<AcuWindResponseItem>
                {
                    new AcuWindResponseItem
                    {
                        SpeedKmh = 15.5,
                        DirectionDegrees = 90,
                        Trend = "Stable",
                        Hour = 12
                    },
                    new AcuWindResponseItem
                    {
                        SpeedKmh = 20.2,
                        DirectionDegrees = 180,
                        Trend = "Increasing",
                        Hour = 13
                    },
                    new AcuWindResponseItem
                    {
                        SpeedKmh = 18.9,
                        DirectionDegrees = 270,
                        Trend = "Decreasing",
                        Hour = 14
                    }
                }
            };
        }
    }
}