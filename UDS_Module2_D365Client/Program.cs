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
            var maxRecords = 40000;
            var statusReasonValue = 0;
            var service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRM"].ConnectionString);

            for (int recordNumber = 10001; recordNumber <= maxRecords; recordNumber++)
            {              

                cr9d3_rent rent = new cr9d3_rent();

                rent.cr9d3_reserved_pickup = RandomValues.GetPickUpDate();
                var pickupDate = rent.cr9d3_reserved_pickup;
                rent.cr9d3_reserved_handover = RandomValues.GetHandoverDay((DateTime)pickupDate);
                var handoverDate = rent.cr9d3_reserved_handover;

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
                statusReasonValue = RandomValues.GetRandomStatusReasonValue(recordNumber);
                rent.statuscode = new OptionSetValue(statusReasonValue);
                var statusReason = rent.statuscode;

                if (statusReason.Value == 970300001)
                {
                    CreatePickUpReport(service, rent, recordNumber);
                }

                if (statusReason.Value == 2) 
                {
                    CreatePickUpReport(service, rent, recordNumber);
                    CreateReturnReport(service, rent, recordNumber);
                }

                rent.cr9d3_pickup_report = CrmRequests.GetPickupReport(service, carName, carValue, (DateTime)pickupDate, "Pickup");
                rent.cr9d3_return_report = CrmRequests.GetPickupReport(service, carName, carValue, (DateTime)handoverDate, "Return");
                rent.cr9d3_paid = IsNotPaid(statusReason,recordNumber);

                service.Create(rent);
                Console.WriteLine($"Create record № {recordNumber}");
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static bool StatusCodeIsActive(int recordNumber)
        {
            return (recordNumber <= 6000);         
        }

        private static void CreatePickUpReport(CrmServiceClient service, cr9d3_rent rent, int recordNumber)
        {
            var pickupReport = new cr9d3_cartransferreport
            {
                cr9d3_type = new OptionSetValue(970300001),
                cr9d3_car = rent.cr9d3_car,
                cr9d3_date = rent.cr9d3_reserved_pickup
            };
            service.Create(pickupReport);
        }

        private static void CreateReturnReport(CrmServiceClient service, cr9d3_rent rent, int recordNumber)
        {
            var returnReport = new cr9d3_cartransferreport
            {
                cr9d3_type = new OptionSetValue(970300000),
                cr9d3_car = rent.cr9d3_car,
                cr9d3_date = rent.cr9d3_reserved_handover
            };

            if ((recordNumber >= 20001) && (recordNumber <= 21500))
            {
                returnReport.cr9d3_damages = true;
                returnReport.cr9d3_damagedescription = "damage";
            }
            service.Create(returnReport);
        }

        private static bool IsNotPaid(OptionSetValue statusReasonValue, int recordNumber)
        {
            if((statusReasonValue.Value == 970300000) && (recordNumber >= 2010) &&(recordNumber <= 2209))
            {
                return false;
            }

            if ((statusReasonValue.Value == 970300001) && (recordNumber >= 4001) && (recordNumber <= 4002))
            {
                return false;
            }

            if ((statusReasonValue.Value == 2) && (recordNumber >= 10001) && (recordNumber <= 10006))
            {
                return false;
            }

            return true;
        }        
    }
}
