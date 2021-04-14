using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS_Module2_D365Client.Services
{
    public class RadmonValues
    {     
        public static DateTime GetPickUpDate()
        {
            Random gen = new Random();
            var start = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2020, 12, 31);
            int range = (endDate - start).Days;
            return start.AddDays(gen.Next(range));
        }

        public static DateTime GetHandoverDay(DateTime pickUpDate)
        {
            Random gen = new Random();
            double range = gen.Next(1,30);
            return pickUpDate.AddDays(range);
        }
    }
}
