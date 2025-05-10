using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Client
{
    public abstract class WeatherPrecipitationWebServiceManager
    {
        
        public abstract double Get(DateTime startDate);

        // Yağış miktarını formatlamak için
        public virtual double FormatPrecipitation(double precipitation)
        {
            if (precipitation < 0)
                throw new ArgumentException("Precipitation cannot be negative.");

            return Math.Round(precipitation, 1); 
        }

        public virtual string FormatPrecipitationOutput(double precipitation)
        {
            return $"{precipitation} mm";
        }
    }
}
