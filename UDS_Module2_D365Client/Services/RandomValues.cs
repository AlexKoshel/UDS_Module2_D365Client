using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace UDS_Module2_D365Client.Services
{
    public class RandomValues
    {
        public static EntityReference GetCarClass(CrmServiceClient service)
        {
            var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='cr9d3_carclass'>
                <attribute name='cr9d3_classcode' />
                <attribute name='createdon' />
                <attribute name='cr9d3_price' />
                <attribute name='cr9d3_classdescription' />
                <attribute name='cr9d3_carclassid' />
                <order attribute='cr9d3_classcode' descending='false' />
                <filter type='and'>
                  <condition attribute='statecode' operator='eq' value='0' />
                </filter>
              </entity>
            </fetch>";

            var carClassesFromClassList = service.RetrieveMultiple(new FetchExpression(query));
            var randomClassFromClassList = carClassesFromClassList[new Random().Next(0, carClassesFromClassList.Entities.Count)];
            var randomClassFromClassListReference = new EntityReference(randomClassFromClassList.LogicalName, randomClassFromClassList.Id);
            randomClassFromClassListReference.Name = randomClassFromClassList.GetAttributeValue<string>("cr9d3_classcode"); 
            return randomClassFromClassListReference;
        }

        public static DateTime GetPickUpDate()
        {
            Random random = new Random();
            var startDate = new DateTime(2019, 1, 1, 0,0,0);
            var endDate = new DateTime(2020, 12, 31, 0, 0, 0 );
            TimeSpan range = startDate - endDate;
            var randts = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return startDate + randts;
        }

        public static DateTime GetHandoverDay(DateTime pickUpDate)
        {
            Random random = new Random();
            double range = random.Next(1,30);
            return pickUpDate.AddDays(range);
        }

        public static EntityReference GetRandomCar(CrmServiceClient service, string clasName, string carClasId)
        {            
            var query = $"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                $"<entity name='cr9d3_car'>" +
                    $"<attribute name='cr9d3_vinnumber' />" +
                    $"<attribute name='cr9d3_purchasedate' /> " +
                    $"<attribute name='cr9d3_productiondate' />" +
                    $"<attribute name='cr9d3_carmodel' />" +
                    $"<attribute name='cr9d3_carmanufacturer' />" +
                    $"<attribute name='cr9d3_carclass' />" +
                    $"<attribute name='cr9d3_carid' />" +
                    $"<order attribute='cr9d3_vinnumber' descending='false' />" +
                    $"<filter type='and'>" +
                        $"<condition attribute='cr9d3_carclass' operator='eq' uiname='{clasName}' uitype='cr9d3_carclass' value='{carClasId}' />" +
                    $"</filter>" +
                $"</entity>" +
            $"</fetch>";

            var carsFromClass = service.RetrieveMultiple(new FetchExpression(query));
            var randomCarFromClass = carsFromClass[new Random().Next(0, carsFromClass.Entities.Count)];
            var randomCar = new EntityReference(randomCarFromClass.LogicalName, randomCarFromClass.Id);
            randomCar.Name = randomCarFromClass.GetAttributeValue<string>("cr9d3_vinnumber");
            return randomCar;
        }

        public static EntityReference GetRandomCustomer(CrmServiceClient service)
        {
            var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='contact'>
                <attribute name='fullname' />
                <attribute name='parentcustomerid' />
                <attribute name='telephone1' />
                <attribute name='emailaddress1' />
                <attribute name='contactid' />
                <order attribute='fullname' descending='false' />
                <filter type='and'>
                  <condition attribute='statecode' operator='eq' value='0' />
                </filter>
              </entity>
            </fetch>";

            var customers = service.RetrieveMultiple(new FetchExpression(query));
            var randomCustomer = customers[new Random().Next(0, customers.Entities.Count)];
            return new EntityReference(randomCustomer.LogicalName, randomCustomer.Id);
        }

        public static int GetRandomStatusReasonValue(int recordNumber)
        {
            if (recordNumber <= 2000)
            {
                return 1;
            }

            if ((recordNumber >= 2001 ) && (recordNumber <= 4000))
            {
                return 970300000;
            }

            if ((recordNumber >= 4001 ) && (recordNumber <= 6000))
            {
                return 970300001;
            }

            if ((recordNumber >= 6001 ) && (recordNumber <= 10000))
            {
                return 970300002;
            }
            return 2;
        }
    }
}