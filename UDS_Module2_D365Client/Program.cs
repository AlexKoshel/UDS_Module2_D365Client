using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
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
            var maxRecords = 15;

            for (int recordNumber = 0; recordNumber < maxRecords; recordNumber++)
            {
                var service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRM"].ConnectionString);

                cr9d3_rent rent = new cr9d3_rent();

                rent.cr9d3_reserved_pickup = RandomValues.GetPickUpDate();
                var pickupDate = rent.cr9d3_reserved_pickup;
                rent.cr9d3_reserved_handover = RandomValues.GetHandoverDay((DateTime)pickupDate);

                rent.cr9d3_car_class = RandomValues.GetCarClass(service);
                var carClas = rent.cr9d3_car_class.Name.ToString();
                var carClasId = rent.cr9d3_car_class.Id.ToString();
                rent.cr9d3_car = RandomValues.GetRandomCar(service, carClas, carClasId);
                var carName = rent.cr9d3_car.Name;
                var carValue = rent.cr9d3_car.Id;
                rent.cr9d3_customer = RandomValues.GetRandomCustomer(service);
                rent.cr9d3_pickup_location = new OptionSetValue(new Random().Next(0, 2));
                rent.cr9d3_return_location = new OptionSetValue(new Random().Next(0, 2));

                if (!StatusCodeIsActive(recordNumber))
                {
                    rent.statecode = cr9d3_rentState.Inactive;
                }

                rent.statuscode = new OptionSetValue(RandomValues.GetRandomStatusReasonValue(recordNumber));
                var statusReason = rent.statuscode;

                if (statusReason.Value == 970300001) CreatePickUpReport(service, rent, recordNumber); 
                




                CrmRequests.GetPickupReport(service,carName, carValue, (DateTime)pickupDate); //need tests

                //service.Create(rent);          




            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static bool StatusCodeIsActive(int recordNumber)
        {
            return (recordNumber <= 14);         
        }

        private static void CreatePickUpReport(CrmServiceClient cervice, cr9d3_rent rent, int recordNumber)
        {
            var pickupReport = new cr9d3_cartransferreport();

            pickupReport.cr9d3_car = rent.cr9d3_car;
            pickupReport.cr9d3_date = rent.cr9d3_reserved_pickup;
            if ((recordNumber >= 10) && (recordNumber <= 12))
            {
                pickupReport.cr9d3_damages = true;
                pickupReport.cr9d3_damagedescription = "damage";
            }
            cervice.Create(pickupReport);
        }
        
        

    }
}
