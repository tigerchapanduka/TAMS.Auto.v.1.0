using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAMS
{
    public class Item
    {
        public IQBSession iQb;
        private string qbFile;
        private string appName;
        private List<string> QBItems;

        public Item(string qbfilepath, string appname)
        {
            qbFile = qbfilepath;
            appName = appname;
        }
        public List<string> GetItems(string filter)
        {
            QBItems = new List<string>();

                try
                {
                    iQb = new QBSession(qbFile, appName);
                    IMsgSetRequest requestSet = iQb.getLatestMsgSetRequest();
                    requestSet.Attributes.OnError = ENRqOnError.roeStop;
                    IItemQuery ItemQ = requestSet.AppendItemQueryRq();
                    ItemQ.ORListQuery.ListFilter.MaxReturned.SetValue(5000);

                    if (!string.IsNullOrEmpty(filter))
                    {
                        ItemQ.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(filter);

                        ItemQ.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
                    }
                    IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                    IResponse response = responseSet.ResponseList.GetAt(0);
                    IORItemRetList orItemRetList = response.Detail as IORItemRetList;

                    if (orItemRetList != null)
                    {
                        if (!(orItemRetList.Count == 0))
                        {
                            for (int ndx = 0; ndx <= (orItemRetList.Count - 1); ndx++)
                            {
                                IORItemRet orItemRet = orItemRetList.GetAt(ndx);

                                switch (orItemRet.ortype)
                                {
                                    case ENORItemRet.orirItemServiceRet:
                                        {

                                            IItemServiceRet ItemServiceRet = orItemRet.ItemServiceRet;
                                            if (!string.IsNullOrEmpty(ItemServiceRet.FullName.GetValue()) )   //&& (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.Desc != null))
                                            {
                                                QBItems.Add(ItemServiceRet.FullName.GetValue());
                                            }
                                        }
                                        break;
                                    case ENORItemRet.orirItemInventoryRet:
                                        {
                                            IItemInventoryRet ItemInventoryRet = orItemRet.ItemInventoryRet;
       
                                            if (!string.IsNullOrEmpty(ItemInventoryRet.FullName.GetValue()) && (ItemInventoryRet.PurchaseDesc != null))
                                            {
                                                QBItems.Add(ItemInventoryRet.FullName.GetValue());
                                            }

                                        }
                                        break;
                                    case ENORItemRet.orirItemNonInventoryRet:
                                        {
                                            IItemNonInventoryRet ItemNonInventoryRet = orItemRet.ItemNonInventoryRet;

                                            if (string.IsNullOrEmpty(ItemNonInventoryRet.FullName.GetValue()) && (ItemNonInventoryRet.ORSalesPurchase.SalesOrPurchase.Desc != null))
                                            {
                                                QBItems.Add(ItemNonInventoryRet.FullName.GetValue());
                                            }
                                        }
                                        break;
                                }
                            } // for loop

                        }

                    }

                    iQb.CloseSession();
                    return QBItems;
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
       
        void WalkItemServiceAddRs(IMsgSetResponse responseMsgSet)
        {
            if (responseMsgSet == null) return;

            IResponseList responseList = responseMsgSet.ResponseList;
            if (responseList == null) return;

            //if we sent only one request, there is only one response, we'll walk the list for this sample
            for (int i = 0; i < responseList.Count; i++)
            {
                IResponse response = responseList.GetAt(i);
                //check the status code of the response, 0=ok, >0 is warning
                var statusCode = response.StatusCode;

                if (response.StatusCode == 0)
                {
                    //the request-specific response is in the details, make sure we have some
                    if (response.Detail != null)
                    {
                        //make sure the response is the type we're expecting
                        ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                        if (responseType == ENResponseType.rtItemServiceAddRs)
                        {
                            //upcast to more specific type here, this is safe because we checked with response.Type check above
                            IItemServiceRet ItemServiceRet = (IItemServiceRet)response.Detail;
                            WalkItemServiceRet(ItemServiceRet);
                        }
                    }
                }
            }
        }

        void WalkItemServiceRet(IItemServiceRet ItemServiceRet)
        {
            if (ItemServiceRet == null) return;

            //Go through all the elements of IItemServiceRet
            //Get value of ListID
            string ListID3 = (string)ItemServiceRet.ListID.GetValue();
            //Get value of TimeCreated
            DateTime TimeCreated4 = (DateTime)ItemServiceRet.TimeCreated.GetValue();
            //Get value of TimeModified
            DateTime TimeModified5 = (DateTime)ItemServiceRet.TimeModified.GetValue();
            //Get value of EditSequence
            string EditSequence6 = (string)ItemServiceRet.EditSequence.GetValue();
            //Get value of Name
            string Name7 = (string)ItemServiceRet.Name.GetValue();
            //Get value of FullName
            string FullName8 = (string)ItemServiceRet.FullName.GetValue();
            //Get value of BarCodeValue
            if (ItemServiceRet.BarCodeValue != null)
            {
                string BarCodeValue9 = (string)ItemServiceRet.BarCodeValue.GetValue();
            }
            //Get value of IsActive
            if (ItemServiceRet.IsActive != null)
            {
                bool IsActive10 = (bool)ItemServiceRet.IsActive.GetValue();
            }
            if (ItemServiceRet.ClassRef != null)
            {
                //Get value of ListID
                if (ItemServiceRet.ClassRef.ListID != null)
                {
                    string ListID11 = (string)ItemServiceRet.ClassRef.ListID.GetValue();
                }
                //Get value of FullName
                if (ItemServiceRet.ClassRef.FullName != null)
                {
                    string FullName12 = (string)ItemServiceRet.ClassRef.FullName.GetValue();
                }
            }
            if (ItemServiceRet.ParentRef != null)
            {
                //Get value of ListID
                if (ItemServiceRet.ParentRef.ListID != null)
                {
                    string ListID13 = (string)ItemServiceRet.ParentRef.ListID.GetValue();
                }
                //Get value of FullName
                if (ItemServiceRet.ParentRef.FullName != null)
                {
                    string FullName14 = (string)ItemServiceRet.ParentRef.FullName.GetValue();
                }
            }
            //Get value of Sublevel
            int Sublevel15 = (int)ItemServiceRet.Sublevel.GetValue();
            if (ItemServiceRet.UnitOfMeasureSetRef != null)
            {
                //Get value of ListID
                if (ItemServiceRet.UnitOfMeasureSetRef.ListID != null)
                {
                    string ListID16 = (string)ItemServiceRet.UnitOfMeasureSetRef.ListID.GetValue();
                }
                //Get value of FullName
                if (ItemServiceRet.UnitOfMeasureSetRef.FullName != null)
                {
                    string FullName17 = (string)ItemServiceRet.UnitOfMeasureSetRef.FullName.GetValue();
                }
            }
            //Get value of IsTaxIncluded
            if (ItemServiceRet.IsTaxIncluded != null)
            {
                bool IsTaxIncluded18 = (bool)ItemServiceRet.IsTaxIncluded.GetValue();
            }
            if (ItemServiceRet.SalesTaxCodeRef != null)
            {
                //Get value of ListID
                if (ItemServiceRet.SalesTaxCodeRef.ListID != null)
                {
                    string ListID19 = (string)ItemServiceRet.SalesTaxCodeRef.ListID.GetValue();
                }
                //Get value of FullName
                if (ItemServiceRet.SalesTaxCodeRef.FullName != null)
                {
                    string FullName20 = (string)ItemServiceRet.SalesTaxCodeRef.FullName.GetValue();
                }
            }
            if (ItemServiceRet.ORSalesPurchase != null)
            {
                if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase != null)
                {
                    if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase != null)
                    {
                        //Get value of Desc
                        if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.Desc != null)
                        {
                            string Desc21 = (string)ItemServiceRet.ORSalesPurchase.SalesOrPurchase.Desc.GetValue();
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice != null)
                        {
                            if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null)
                            {
                                //Get value of Price
                                if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price != null)
                                {
                                    double Price22 = (double)ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.GetValue();
                                }
                            }
                            if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null)
                            {
                                //Get value of PricePercent
                                if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent != null)
                                {
                                    double PricePercent23 = (double)ItemServiceRet.ORSalesPurchase.SalesOrPurchase.ORPrice.PricePercent.GetValue();
                                }
                            }
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.AccountRef != null)
                        {
                            //Get value of ListID
                            if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID != null)
                            {
                                string ListID24 = (string)ItemServiceRet.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ItemServiceRet.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName != null)
                            {
                                string FullName25 = (string)ItemServiceRet.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.GetValue();
                            }
                        }
                    }
                }
                if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase != null)
                {
                    if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase != null)
                    {
                        //Get value of SalesDesc
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.SalesDesc != null)
                        {
                            string SalesDesc26 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.SalesDesc.GetValue();
                        }
                        //Get value of SalesPrice
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.SalesPrice != null)
                        {
                            double SalesPrice27 = (double)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.SalesPrice.GetValue();
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef != null)
                        {
                            //Get value of ListID
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID != null)
                            {
                                string ListID28 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName != null)
                            {
                                string FullName29 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.IncomeAccountRef.FullName.GetValue();
                            }
                        }
                        //Get value of PurchaseDesc
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseDesc != null)
                        {
                            string PurchaseDesc30 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseDesc.GetValue();
                        }
                        //Get value of PurchaseCost
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseCost != null)
                        {
                            double PurchaseCost31 = (double)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseCost.GetValue();
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef != null)
                        {
                            //Get value of ListID
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID != null)
                            {
                                string ListID32 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName != null)
                            {
                                string FullName33 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PurchaseTaxCodeRef.FullName.GetValue();
                            }
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef != null)
                        {
                            //Get value of ListID
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID != null)
                            {
                                string ListID34 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName != null)
                            {
                                string FullName35 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.ExpenseAccountRef.FullName.GetValue();
                            }
                        }
                        if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef != null)
                        {
                            //Get value of ListID
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID != null)
                            {
                                string ListID36 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName != null)
                            {
                                string FullName37 = (string)ItemServiceRet.ORSalesPurchase.SalesAndPurchase.PrefVendorRef.FullName.GetValue();
                            }
                        }
                    }
                }
            }
            //Get value of ExternalGUID
            if (ItemServiceRet.ExternalGUID != null)
            {
                string ExternalGUID38 = (string)ItemServiceRet.ExternalGUID.GetValue();
            }
            if (ItemServiceRet.DataExtRetList != null)
            {
                for (int i39 = 0; i39 < ItemServiceRet.DataExtRetList.Count; i39++)
                {
                    IDataExtRet DataExtRet = ItemServiceRet.DataExtRetList.GetAt(i39);
                    //Get value of OwnerID
                    if (DataExtRet.OwnerID != null)
                    {
                        string OwnerID40 = (string)DataExtRet.OwnerID.GetValue();
                    }
                    //Get value of DataExtName
                    string DataExtName41 = (string)DataExtRet.DataExtName.GetValue();
                    //Get value of DataExtType
                    ENDataExtType DataExtType42 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                    //Get value of DataExtValue
                    string DataExtValue43 = (string)DataExtRet.DataExtValue.GetValue();
                }
            }
        }


    }
}
