﻿using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDS_Module2_D365Client.Services;

namespace UDS_Module2_D365Client
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string connectionString = ConfigurationManager.ConnectionStrings["MyCRM"].ConnectionString;
            var service = new CrmServiceClient(connectionString);

            cr9d3_rent rent = new cr9d3_rent();
            rent.cr9d3_reserved_pickup = RadmonValues.GetRandomPickUpDay();

            service.Create(rent);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
