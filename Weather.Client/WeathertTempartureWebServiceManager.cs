using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Client
{
    public abstract class WeathertTempartureWebServiceManager
    {
        public abstract double Get(DateTime startDate);
        
        public double FormatTemperatureSabit(double temperature)
        {
            return temperature / 1.8 - 32;
        }

        public virtual string FormatLocaleDurumaGore(double temperature)
        {
            return $"{temperature} °F";
        }
    }
}
