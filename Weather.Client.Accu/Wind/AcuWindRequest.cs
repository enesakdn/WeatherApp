using System;
using System.Collections.Generic;

namespace Weather.Client.Accu.Wind
{
    public class AcuWindRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
    }

    public class AcuWindResponse
    {
        public List<AcuWindResponseItem> Items { get; set; }
    }

    public class AcuWindResponseItem
    {
        public DateTime Date { get; set; }
        public double SpeedKmh { get; set; } 
        public double DirectionDegrees { get; set; }
        public string Trend { get; set; } 
    }
}