using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAMS.BO;

namespace TAMS
{

    public class Customer
    {
        public IQBSession iQb;
        private string qbFile;
        private string appName;
        private List<string> Items;
        
        public Customer(string qbfilepath, string appname)
        {
            qbFile = qbfilepath;
            appName = appname;
        }

        public List<string> GetItems(string filter)
        {
            Items = new List<string>();

                iQb = new QBSession(qbFile, appName);
                try
                {
                    IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
                    requestSet.Attributes.OnError = ENRqOnError.roeStop;

                    ICustomerQuery customerQ = requestSet.AppendCustomerQueryRq();

                    if (!string.IsNullOrEmpty(filter))
                    {
                        customerQ.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameFilter.Name.SetValue(filter);

                        customerQ.ORCustomerListQuery.CustomerListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
                    }

                    IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                    IResponse response = responseSet.ResponseList.GetAt(0);

                    ICustomerRetList customerRetList = response.Detail as ICustomerRetList;


                    if (customerRetList != null)
                    {
                        if (!(customerRetList.Count == 0))
                        {
                            for (int ndx = 0; ndx <= (customerRetList.Count - 1); ndx++)
                            {
                                ICustomerRet customerRet = customerRetList.GetAt(ndx);
                                Items.Add(customerRet.Name.GetValue());
                            }
                        }
                    }

                    //iQb.CloseSession();

                }
                catch (Exception ex)
                {

                    //iQb.CloseSession();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    iQb.CloseSession();
                }

            return Items;
        }

        public int CreateCustomer(Order customerorder)
        {
           
            iQb = new QBSession(qbFile, appName);
            try
            {
                IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
                requestSet.Attributes.OnError = ENRqOnError.roeContinue;

                ICustomerAdd CustomerAddRq = requestSet.AppendCustomerAddRq();

                if (customerorder!=null)
                {
                    //Set field value for Name

                    CustomerAddRq.Name.SetValue(customerorder.Customer.first_name +" "+customerorder.Customer.last_name);
                    //Set field value for IsActive
                    //CustomerAddRq.IsActive.SetValue(true);
                   // CustomerAddRq.ClassRef.FullName.SetValue(customerorder.Customer.first_name);
                    //CustomerAddRq.ShipAddress.Addr1.SetValue(customerorder.shipping_address.address1);
                    //Set field value for Addr2
                   // CustomerAddRq.ShipAddress.Addr2.SetValue(customerorder.ShippingAddress1);
                    //Set field value for Addr3
                   // CustomerAddRq.ShipAddress.Addr3.SetValue(customerorder.ShippingAddress2);
                   // CustomerAddRq.ShipAddress.City.SetValue(customerorder.ShippingCity);
                    //Set field value for State
                   // CustomerAddRq.ShipAddress.State.SetValue(customerorder.ShippingCity);
                    //Set field value for PostalCode
                   // CustomerAddRq.ShipAddress.PostalCode.SetValue(customerorder.ShippingZip);
                    
                   // CustomerAddRq.ShipAddress.Country.SetValue(customerorder.ShippingCountry);


                }

                string requestXML = requestSet.ToXMLString();
            
                IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                IResponse response = responseSet.ResponseList.GetAt(0);

                string responseMessage = response.StatusMessage;

                int returnCode = response.StatusCode;


                //iQb.CloseSession();

                return returnCode;

            }
            catch (Exception ex)
            {

                //iQb.CloseSession();
                throw new Exception(ex.Message);
            }
            finally
            {
                iQb.CloseSession();
            }
                       
        }

        public int TestCreateCustomer(string customername)
        {

            iQb = new QBSession(qbFile, appName);
            try
            {
                IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
                requestSet.Attributes.OnError = ENRqOnError.roeStop;

                ICustomerAdd CustomerAddRq = requestSet.AppendCustomerAddRq();

                if (customername != null)
                {
                    CustomerAddRq.Name.SetValue(customername);
                }

                string requestXML = requestSet.ToXMLString();

                IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                IResponse response = responseSet.ResponseList.GetAt(0);

                string responseMessage = response.StatusMessage;

                int returnCode = response.StatusCode;

                return returnCode;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                iQb.CloseSession();
            }

        }
    }
}
