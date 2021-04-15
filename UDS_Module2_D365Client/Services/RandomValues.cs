using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var carClass = service.RetrieveMultiple(new FetchExpression(query));
            var rdmClass = carClass[new Random().Next(0, carClass.Entities.Count)];
            var Clas = new EntityReference(rdmClass.LogicalName, rdmClass.Id);
            Clas.Name = rdmClass.GetAttributeValue<string>("cr9d3_classcode"); 
            return Clas;
        }

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

        public static EntityReference GetRdmCar(CrmServiceClient service, string clasName, string carClasId)
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
            var cars = service.RetrieveMultiple(new FetchExpression(query));
            var rdmCar = cars[new Random().Next(0, cars.Entities.Count)];
           return new EntityReference(rdmCar.LogicalName, rdmCar.Id);
        }
    }
}
