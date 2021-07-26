using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QBFC13Lib;
using TAMS.BO;
using TAMS.MAIL;

namespace TAMS
{
    public class QuickBooks
    {
        public IQBSession iQb;
        private string qbFile;
        private string appName;
        private List<Order> ordersCollection;
        private string sheetName;

        public QuickBooks(string qbfilepath, string appname, List<Order> ordercollection, string sheetname)
        {
            sheetName = sheetname;
            qbFile = qbfilepath;
            appName = appname;
            ordersCollection = ordercollection;
        }

        public List<Order> CreateOrder()
        {
            //File.AppendAllText("log.txt", " Creating quickbooks session "+DateTime.Now.ToString());

            try {

                File.AppendAllText("log.txt", " Importing orders "+DateTime.Now.ToString());
                return  CreateSalesOrder( ordersCollection);
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }   

        }

        private string getItemCode(string lineitemsku)
        {

            Item item = new Item(qbFile, appName);
            List<string> itemsList = item.GetItems(lineitemsku);

            string itemCode = "";

            if (itemsList.Count > 0)
            {

                itemCode = itemsList[0];
            }

            return itemCode;
        }
        private bool searchForSalesOrder(string refnum)
        {
            try
            {

           
            iQb = new QBSession(qbFile, appName);

            IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
            //ISalesOrderAdd salesOrderAdd = requestSet.AppendSalesOrderAddRq();

            requestSet.Attributes.OnError = ENRqOnError.roeContinue;
            ISalesOrderQuery SalesOrderQueryRq = requestSet.AppendSalesOrderQueryRq();
            //SalesOrderQueryRq.ORTxnNoAccountQuery.
            SalesOrderQueryRq.ORTxnNoAccountQuery.RefNumberList.Add(refnum);
             
            IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);
            IResponseList responseList = responseSet.ResponseList;
            if (responseList == null)
            {
                return false;
            }
            //else 
             //   return
              //  responseList.Count >= 1;
            //};
            IResponse response = responseSet.ResponseList.GetAt(0);
            int statusCode = response.StatusCode;

            if (response.StatusCode >= 0)
            {
                //the request-specific response is in the details, make sure we have some
                if (response.Detail != null)
                {
                    //make sure the response is the type we're expecting
                    ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                    if (responseType == ENResponseType.rtSalesOrderQueryRs)
                    {
                        //upcast to more specific type here, this is safe because we checked with response.Type check above
                        ISalesOrderRetList SalesOrderRet = (ISalesOrderRetList)response.Detail;
                        
                        //WalkSalesOrderRet(SalesOrderRet);

                    }
                }
            }

                return (response.StatusCode == 0);
            }
            catch (Exception ex)
            {
               
                string error = ex.Message;
                StringBuilder errMessage = new StringBuilder();
                errMessage.AppendLine(error);
                Mailer.SendMessage(errMessage);
                //return errMessage.ToString();
            }
            finally
            {
                if (iQb != null)
                {
                    iQb.CloseSession();
                }
            }

            return false;
        }

        private List<Order> CreateSalesOrder( List<Order> ls)
        {
            try
            {
                List<Order> importedOrders = new List<Order>();

                foreach (Order order in ls)
                {
                    if (order.order_number=="6099")
                    {
                        Console.WriteLine();
                    }

                    bool exists = searchForSalesOrder(order.order_number+"S" + DateTime.Now.Year.ToString());
                    Customer cust = new Customer(qbFile, appName);
                    List<string> customerList = cust.GetItems(order.Customer.first_name + " " + order.Customer.last_name);

                    string customer = "";

                    if (customerList.Count == 0)
                    {
                        int qbResponseCode = cust.CreateCustomer(order);
                        if (qbResponseCode == 0 || qbResponseCode == 3100)
                        {
                            customer = order.Customer.first_name + " " + order.Customer.last_name;
                        }

                    }
                    else
                    {
                        customer = customerList[0];

                    }

                    if (!exists)
                    {
               
                        Class qbClass = new Class(qbFile, appName);
                        string qbclass = qbClass.GetItems("Stockist - Online Store")[0];


                        iQb = new QBSession(qbFile, appName);

                        IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
                        ISalesOrderAdd salesOrderAdd = requestSet.AppendSalesOrderAddRq();

                        requestSet.Attributes.OnError = ENRqOnError.roeContinue;

                        if (!string.IsNullOrEmpty(customer))
                        {

                            salesOrderAdd.TxnDate.SetValue(Convert.ToDateTime(DateTime.Now));
                            salesOrderAdd.CustomerRef.FullName.SetValue(customer);
                            salesOrderAdd.RefNumber.SetValue(order.order_number + "S"+DateTime.Now.Year.ToString());
                            salesOrderAdd.IsTaxIncluded.SetValue(true);
                            salesOrderAdd.IsToBeEmailed.SetValue(false);
                            salesOrderAdd.IsToBePrinted.SetValue(false);
                            salesOrderAdd.DueDate.SetValue(DateTime.Now);
                            salesOrderAdd.TermsRef.FullName.SetValue("Due on receipt");
                            salesOrderAdd.ShipDate.SetValue(DateTime.Now);

                            salesOrderAdd.ClassRef.FullName.SetValue(qbclass);
                            string notes = "";
                            foreach (Note note in order.note_attributes)
                            {
                                notes = notes + " " + note.name + " " + note.value;
                            }

                            salesOrderAdd.Memo.SetValue(notes);

                            string excessAddress = "";
                            string prependedAddress = "";

                            if (!string.IsNullOrEmpty(order.BillingStreet))
                            {
                                if (order.BillingStreet.Length > 40)
                                {
                                    salesOrderAdd.BillAddress.Addr1.SetValue(order.billing_address.address1.Substring(0, 40));
                                    excessAddress = order.billing_address.address1.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.BillAddress.Addr1.SetValue(order.billing_address.address1);
                                }
                            }

                            prependedAddress = excessAddress + " " + order.billing_address.address2;
                            if (!string.IsNullOrEmpty(prependedAddress.Trim()))
                            {

                                if (prependedAddress.Length > 40)
                                {
                                    salesOrderAdd.BillAddress.Addr2.SetValue(prependedAddress.Substring(0, 40));
                                    excessAddress = prependedAddress.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.BillAddress.Addr2.SetValue(prependedAddress);
                                }

                            }

                            prependedAddress = excessAddress + " " + order.billing_address.address2;
                            if (!string.IsNullOrEmpty(prependedAddress.Trim()))
                            {

                                if (prependedAddress.Length > 40)
                                {
                                    salesOrderAdd.BillAddress.Addr3.SetValue(prependedAddress.Substring(0, 40));
                                    excessAddress = prependedAddress.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.BillAddress.Addr3.SetValue(prependedAddress);
                                }

                            }

                            salesOrderAdd.BillAddress.City.SetValue(order.billing_address.city);
                            salesOrderAdd.BillAddress.State.SetValue(order.billing_address.province);
                            salesOrderAdd.BillAddress.Country.SetValue(order.billing_address.country);
                            salesOrderAdd.BillAddress.PostalCode.SetValue(order.billing_address.zip);

                            excessAddress = "";

                            if (!string.IsNullOrEmpty(order.shipping_address.address1))
                            {
                                if (order.shipping_address.address1.Length > 40)
                                {
                                    salesOrderAdd.ShipAddress.Addr1.SetValue(order.shipping_address.address1.Substring(0, 40));
                                    excessAddress = order.shipping_address.address1.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.ShipAddress.Addr1.SetValue(order.shipping_address.address1);
                                }
                            }



                            prependedAddress = excessAddress + " " + order.shipping_address.address1;
                            if (!string.IsNullOrEmpty(prependedAddress.Trim()))
                            {

                                if (prependedAddress.Length > 40)
                                {
                                    salesOrderAdd.ShipAddress.Addr2.SetValue(prependedAddress.Substring(0, 40));
                                    excessAddress = prependedAddress.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.ShipAddress.Addr2.SetValue(prependedAddress);
                                }

                            }

                            prependedAddress = excessAddress + " " + order.shipping_address.address2;
                            if (!string.IsNullOrEmpty(prependedAddress.Trim()))
                            {

                                if (prependedAddress.Length > 40)
                                {
                                    salesOrderAdd.ShipAddress.Addr3.SetValue(prependedAddress.Substring(0, 40));
                                    excessAddress = prependedAddress.Substring(41);
                                }
                                else
                                {
                                    salesOrderAdd.ShipAddress.Addr3.SetValue(prependedAddress);
                                }

                            }

                            salesOrderAdd.ShipAddress.City.SetValue(order.shipping_address.city);
                            salesOrderAdd.ShipAddress.State.SetValue(order.shipping_address.province);
                            salesOrderAdd.ShipAddress.Country.SetValue(order.shipping_address.country);
                            salesOrderAdd.ShipAddress.PostalCode.SetValue(order.shipping_address.zip);

                            int countOrderWithValidItemCode = 0;

                            /*
                            string itemCode = getItemCode(order.LineItemSKU);
                            if (!string.IsNullOrEmpty(itemCode))
                            {
                                ISalesOrderLineAdd salesOrderLineAdd = salesOrderAdd.ORSalesOrderLineAddList.Append().SalesOrderLineAdd;
                                salesOrderLineAdd.ItemRef.FullName.SetValue(itemCode);
                                salesOrderLineAdd.Quantity.SetValue(order.LineItemQuantity);
                                salesOrderLineAdd.ClassRef.FullName.SetValue(qbclass);
                                salesOrderLineAdd.Amount.SetValue(order.LineItemPrice);
                                order.Imported = true;
                                countOrderWithValidItemCode += 1;
                            }
                            else
                            {
                                order.ImportException = "Item code does not exist";
                                order.Imported = false;
                            }
                            */
                            foreach (LineItem childOrder in order.line_items)
                            {
                                string childItemCode = getItemCode(childOrder.sku);
                                if (!string.IsNullOrEmpty(childItemCode))
                                {
                                    ISalesOrderLineAdd salesOrderLineAdd = salesOrderAdd.ORSalesOrderLineAddList.Append().SalesOrderLineAdd;
                                    salesOrderLineAdd.ItemRef.FullName.SetValue(childItemCode);
                                    salesOrderLineAdd.Quantity.SetValue(Convert.ToDouble(childOrder.quantity));
                                    salesOrderLineAdd.ClassRef.FullName.SetValue(qbclass);
                                    salesOrderLineAdd.Amount.SetValue(childOrder.price);
                                    StringBuilder notesBuilder = new StringBuilder();
                                    foreach (Note note in childOrder.properties)
                                    {
                                        notesBuilder.AppendLine(note.ToString()); 
                                    }
                                    salesOrderLineAdd.Desc.SetValue(notesBuilder.ToString());
                                    childOrder.Imported = true;
                                    countOrderWithValidItemCode += 1;
                                }
                                else
                                {
                                    childOrder.ImportException = "Item code does not exist";
                                    childOrder.Imported = false;
                                }
                            }


                            if (Convert.ToDouble(order.total_discounts_set.shop_money.amount) > 0)
                            {
                                ISalesOrderLineAdd salesOrderLineAddDiscount = salesOrderAdd.ORSalesOrderLineAddList.Append().SalesOrderLineAdd;
                                //QB Item code
                                salesOrderLineAddDiscount.ItemRef.FullName.SetValue("Sales Discounts");
                                //salesOrderLineAddDiscount.Quantity.SetValue(1);
                                salesOrderLineAddDiscount.ClassRef.FullName.SetValue(qbclass);
                                salesOrderLineAddDiscount.Amount.SetValue(Convert.ToDouble(order.total_discounts_set.shop_money.amount));

                            }

                            if (Convert.ToDouble(order.total_shipping_price_set.shop_money.amount) > 0)
                            {
                                ISalesOrderLineAdd salesOrderLineAddShppingAmount = salesOrderAdd.ORSalesOrderLineAddList.Append().SalesOrderLineAdd;
                                salesOrderLineAddShppingAmount.ItemRef.FullName.SetValue("Delivery & Transport");
                                //salesOrderLineAddShppingAmount.Quantity.SetValue(1);
                                salesOrderLineAddShppingAmount.ClassRef.FullName.SetValue(qbclass);
                                salesOrderLineAddShppingAmount.Amount.SetValue(Convert.ToDouble(order.total_shipping_price_set.shop_money.amount));
                            }

                            //TO DO Check item codes exist before importing


                            //string requestXML = requestSet.ToXMLString();
                            try
                            {
                                if (countOrderWithValidItemCode > 0)
                                {
                                    IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                                    IResponse response = responseSet.ResponseList.GetAt(0);

                                    string responseStatusMessage = response.StatusMessage;

                                    int statusCode = response.StatusCode;

                                    if (statusCode == 0)
                                    {

                                        ISalesOrderRet soDetail = (ISalesOrderRet)response.Detail;
                                        string transactionID = soDetail.TxnID.GetValue();
                                        order.Imported = true;

                                    }
                                    else
                                    {
                                        order.ImportException = responseStatusMessage;
                                    }
                                }
                                else
                                {
                                    
                                    order.ImportException = "Line items do not have valid sku code";
                                }
                            }
                            catch (Exception ex)
                            {
                                order.Imported = false;
                                order.ImportException = ex.Message;
                                //MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                //if (iQb != null)
                                {
                                    //    iQb.CloseSession();
                                }
                            }
                        }
                        else
                        {
                            order.Imported = false;
                            order.ImportException = "Customer could not be created";
                        }


                    }
                    else {

                        foreach (LineItem childOrder in order.line_items)
                        {
                            string childItemCode = getItemCode(childOrder.sku);
                            if (string.IsNullOrEmpty(childItemCode))
                            {
                                childOrder.ImportException = "Item code does not exist";
                                childOrder.Imported = false;
                                order.ImportException = order.ImportException + "Item code does not exist ";
                            }
                          
                        }
                        order.AlreadyImported = true;
                    }
                    importedOrders.Add(order);

                }
                return importedOrders;
            }

            catch (Exception ex)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(ex.Message);

                File.AppendAllText("log.txt", sb.ToString());
                sb.Clear();
                MAIL.Mailer.SendMessage(sb);
                //MessageBox.Show(ex.Message);
                //Console.WriteLine(ex.Message);

                return new List<Order>();
            }
            finally
            {

                if (iQb != null)
                {
                    iQb.CloseSession();
                }

            }

            
}
    }

}
    

