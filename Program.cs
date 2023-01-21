using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System.ServiceModel;

namespace update_data_from_xml
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connectionString = @"AuthType = Office365; Url = https://org8b2a8c21.api.crm8.dynamics.com/XRMServices/2011/Organization.svc;Username=aliahmed@1107admin.onmicrosoft.com;Password=Roman@1107$";
                CrmServiceClient conn = new CrmServiceClient(connectionString);

                IOrganizationService service = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

                //XML for all account
                string fetchquery = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                        <entity name='contact'>
                                        <attribute name='fullname'/>
                                        <attribute name='emailaddress1'/>
                                        <attribute name='parentcustomerid'/>
                                        <attribute name='telephone1'/>
                                        <attribute name='statecode'/>
                                        <attribute name='contactid'/>
                                        </entity>
                                        </fetch>";

                // xml for inacitve account
                /* string fetchquery = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                     <entity name='contact'>
                                     <attribute name='fullname'/>
                                     <attribute name='parentcustomerid'/>
                                     <attribute name='telephone1'/>
                                     <attribute name='contactid'/>
                                     <order attribute='fullname' descending='false'/>
                                     <filter type='and'>
                                     <condition attribute='statecode' operator='eq' value='1'/>
                                     </filter>
                                     </entity>
                                     </fetch>";*/

                EntityCollection Contact = service.RetrieveMultiple(new FetchExpression(fetchquery));
                foreach (var c in Contact.Entities)
                {

                    //assign value in string field and lookup field
                    c["emailaddress1"] = "test@gmail.com";
                    c["parentcustomerid"] = new EntityReference("contact", new Guid("8894c5c4-bc98-ed11-aad0-000d3af2358c"));
                    //update remove the parentid and email id from all contact
                    /* c["emailaddress1"] = string.Empty;
                     c.Attributes["parentcustomerid"] = null;*/
                    service.Update(c);
                    //Console.WriteLine("Name: {0}", c.Attributes["fullname"]);


                }

                Console.WriteLine("Updated contact");
                Console.ReadLine();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }
    }
}
