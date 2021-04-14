using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS_Module2_D365Client.Services
{
    public class RadmonValues
    {       

        public static DateTime GetRandomPickUpDay()
        {
            Random gen = new Random();
            var start = new DateTime(2019, 1, 1);
            var endDate = new DateTime(2020, 12, 31);
            int range = (endDate - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
