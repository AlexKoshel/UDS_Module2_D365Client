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
    public class CrmRequests
    {
        public static EntityReference GetPickupReport(CrmServiceClient service, string carName, Guid carValue, DateTime? pickUpDate)
        {
            var carPickUpDate = pickUpDate.ToString();
            
            var query = $"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>" +
                            $"<entity name='cr9d3_cartransferreport'>" +
                                $"<attribute name='cr9d3_type' />" +
                                $"<attribute name='cr9d3_date' />" +
                                $"<attribute name='cr9d3_damages' />" +
                                $"<attribute name='cr9d3_damagedescription' />" +
                                $"<attribute name='cr9d3_car' />" +
                                $"<attribute name='cr9d3_cartransferreportid' />" +
                                $"<order attribute='cr9d3_type' descending='false' />" +
                                    $"<filter type='and'>" +
                                        $"<condition attribute='cr9d3_car' operator='eq' uiname='{carName}' uitype='cr9d3_car' value='{carValue}' />" +
                                    $"</filter>" +
                            $"</entity>" +
                        $"</fetch>";
            var pickupReports = service.RetrieveMultiple(new FetchExpression(query));
            foreach (var pickupReport in pickupReports.Entities)
            {
                var reportDate = pickupReport.Attributes["cr9d3_date"].ToString(); 
                if (reportDate == carPickUpDate) return new EntityReference(pickupReport.LogicalName, pickupReport.Id);
            }

            return null;
        }
    }
}
