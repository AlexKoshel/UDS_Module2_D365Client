using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;

using UDS_Module2_D365Client.Services;

namespace UDS_Module2_D365Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRM"].ConnectionString);
            var carClassList = CrmRequests.GetCarClassList(service);
            var carsList = CrmRequests.GetCarslist(service);
            var customerList = CrmRequests.GetCustomer(service);

            var maxRecords = 40000;
            var statusReasonValue = 0;
           
            for (int recordNumber = 31248; recordNumber <= maxRecords; recordNumber++)
            {              
                cr9d3_rent rent = new cr9d3_rent();

                rent.cr9d3_reserved_pickup = RandomValues.GetPickUpDate();
                var pickupDate = rent.cr9d3_reserved_pickup;
                rent.cr9d3_reserved_handover = RandomValues.GetHandoverDay((DateTime)pickupDate);
               
                rent.cr9d3_car_class = RandomValues.GetCarClass(carClassList);
                var clasName = rent.cr9d3_car_class.Name.ToString();
                
                rent.cr9d3_car = RandomValues.GetRandomCar(carsList, clasName);                            
                rent.cr9d3_customer = RandomValues.GetRandomCustomer(customerList);             
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
                    rent.cr9d3_pickup_report = CreatePickUpReport(service, rent, recordNumber);
                }


                if (statusReason.Value == 2)
                {
                    rent.cr9d3_pickup_report = CreatePickUpReport(service, rent, recordNumber);
                    rent.cr9d3_return_report = CreateReturnReport(service, rent, recordNumber);
                }

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

        private static EntityReference CreatePickUpReport(CrmServiceClient service, cr9d3_rent rent, int recordNumber)
        {
            var pickupReport = new cr9d3_cartransferreport
            {
                cr9d3_type = new OptionSetValue(970300001),
                cr9d3_car = rent.cr9d3_car,
                cr9d3_date = rent.cr9d3_reserved_pickup
            };
            var reportId = service.Create(pickupReport);
            return new EntityReference("cr9d3_cartransferreport", reportId);        }
        

        private static EntityReference CreateReturnReport(CrmServiceClient service, cr9d3_rent rent, int recordNumber)
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
            var reportId = service.Create(returnReport);
            return new EntityReference("cr9d3_cartransferreport", reportId);
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
