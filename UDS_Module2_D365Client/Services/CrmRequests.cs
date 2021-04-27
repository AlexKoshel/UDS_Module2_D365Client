using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace UDS_Module2_D365Client.Services
{
    public class CrmRequests
    {
        public static EntityCollection GetCarClassList(CrmServiceClient service)
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

            return service.RetrieveMultiple(new FetchExpression(query));
        }

        public static EntityCollection GetCarslist(CrmServiceClient service)
        {
            var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                <entity name='cr9d3_car'>
                <attribute name='cr9d3_vinnumber' />
                <attribute name='cr9d3_purchasedate' />
                <attribute name='cr9d3_productiondate' />
                <attribute name='cr9d3_carmodel' />
                <attribute name='cr9d3_carmanufacturer' />
                <attribute name='cr9d3_carclass' />
                <attribute name='cr9d3_carid' />
                <order attribute='cr9d3_vinnumber' descending='false' />
                <filter type='and'>
                    <condition attribute='statecode' operator='eq' value='0' />
                </filter>
                </entity>
            </fetch>";

            return service.RetrieveMultiple(new FetchExpression(query));
        }

        public static EntityCollection GetCustomer(CrmServiceClient service)
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

            return service.RetrieveMultiple(new FetchExpression(query));
        }
    }
}