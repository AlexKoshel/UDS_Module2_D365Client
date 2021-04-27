using Microsoft.Xrm.Sdk;
using System;

namespace UDS_Module2_D365Client.Services
{
    public class RandomValues
    {
        public static EntityReference GetCarClass(EntityCollection carClassList)
        {            
            var randomClassFromClassList = carClassList[new Random().Next(0, carClassList.Entities.Count)];
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

        public static EntityReference GetRandomCar(EntityCollection carsList, string clasName)
        {
            var carsCollection = new EntityCollection();

            foreach (var car in carsList.Entities)
            {
                var carClas = car.GetAttributeValue<EntityReference>("cr9d3_carclass").Name;
                if (carClas == clasName)
                {
                    carsCollection.Entities.Add(car);
                }
            }
            
            var randomCarFromCollection = carsCollection[new Random().Next(0, carsCollection.Entities.Count)];                          
            var randomCar = new EntityReference(randomCarFromCollection.LogicalName, randomCarFromCollection.Id);
            randomCar.Name = randomCarFromCollection.GetAttributeValue<string>("cr9d3_vinnumber");
            return randomCar;
        }

        public static EntityReference GetRandomCustomer(EntityCollection customersList)
        {           
            var randomCustomer = customersList[new Random().Next(0, customersList.Entities.Count)];
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