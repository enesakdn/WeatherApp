using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }; // Example temperature
        }

    }


}
