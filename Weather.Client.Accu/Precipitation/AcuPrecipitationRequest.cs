using System;
using System.Collections.Generic;

namespace Weather.Client.Accu.Precipitation
{
    public class AcuPrecipitationRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
    }

    public class AcuPrecipitationResponse
    {
        public List<AcuPrecipitationResponseItem> Items { get; set; }
    }

    public class AcuPrecipitationResponseItem
    {
        public double PrecipitationMm { get; set; } 
        public int Hour { get; set; }
    }
}