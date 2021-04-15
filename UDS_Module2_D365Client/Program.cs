using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
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
            var service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRM"].ConnectionString);

            cr9d3_rent rent = new cr9d3_rent();
            rent.cr9d3_reserved_pickup = RandomValues.GetPickUpDate();
            var pickupDate = rent.cr9d3_reserved_pickup;
            rent.cr9d3_reserved_handover = RandomValues.GetHandoverDay((DateTime)pickupDate);

            rent.cr9d3_car_class = RandomValues.GetCarClass(service);
            var carClas = rent.cr9d3_car_class.Name.ToString();
            var carClasId = rent.cr9d3_car_class.Id.ToString();
            rent.cr9d3_car = RandomValues.GetRdmCar(service, carClas, carClasId);

            service.Create(rent);

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
