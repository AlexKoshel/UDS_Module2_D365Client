using Microsoft.Xrm.Sdk;
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
            var maxRecords = 100;

            for (int i = 0; i < maxRecords; i++)
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

                rent.cr9d3_customer = RandomValues.GetRdmCustomer(service);
                rent.cr9d3_pickup_location = new OptionSetValue(new Random().Next(0, 2));
                rent.cr9d3_return_location = new OptionSetValue(new Random().Next(0, 2));

                if (!StatusCodeIsActive(i))
                {
                    rent.statecode = cr9d3_rentState.Inactive;
                }

                rent.statuscode = new OptionSetValue( RandomValues.GetRdmStatusReasonValue(i));

                service.Create(rent);

            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static bool StatusCodeIsActive(int recordNumber)
        {
            return (recordNumber <= 14);            //-1    
        }
        
        

    }
}
