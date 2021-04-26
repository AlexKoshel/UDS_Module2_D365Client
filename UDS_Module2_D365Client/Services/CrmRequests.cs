using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace UDS_Module2_D365Client.Services
{
    public class CrmRequests
    {
        public static EntityReference GetPickupReport(CrmServiceClient service, string carName, Guid carValue, DateTime requiredDate, string reportType)
        {
            var reportTypeValue = 0;
            var reportRequiredDate = requiredDate.ToString();

            switch (reportType)
            {
                case "Return":
                    reportTypeValue = 970300000;
                    break;
                default:
                    reportTypeValue = 970300001;
                    break;
            }            
                        
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
                                        $"<condition attribute='cr9d3_type' operator='eq' value='{reportTypeValue}' />" +
                                    $"</filter>" +
                            $"</entity>" +
                        $"</fetch>";

            var requiredReports = service.RetrieveMultiple(new FetchExpression(query));

            foreach (var report in requiredReports.Entities)
            {
                var reportDate = report.GetAttributeValue<DateTime>("cr9d3_date").ToString();
                if (reportDate == reportRequiredDate)
                {
                    return new EntityReference(report.LogicalName, report.Id);
                }
            }
            return null;
        }
    }
}