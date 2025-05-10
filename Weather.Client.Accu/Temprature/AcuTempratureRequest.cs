using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Client.Accu.Temprature
{
    public class AcuTempratureRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Country { get; set; }
    }

    public class AcuTempratureResponse
    {
        public List<AcuTempratureResponseItem> items { get; set; }
    }

    public class AcuTempratureResponseItem
    {
        public double Fehranayt { get; set; }
        public int Hour { get; set; }
    }

}
