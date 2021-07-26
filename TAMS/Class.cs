using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAMS
{
    public class Class 
    {
        public IQBSession iQb;
        private string qbFile;
        private string appName;
        private List<string> Items;

        public Class(string qbfilepath, string appname)
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

                    IClassQuery classQ = requestSet.AppendClassQueryRq();

                    if (!string.IsNullOrEmpty(filter))
                    {
                        classQ.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(filter);

                        classQ.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
                    }

                    IMsgSetResponse responseSet = iQb.GetSessionManager().DoRequests(requestSet);

                    IResponse response = responseSet.ResponseList.GetAt(0);

                    string errorMessage = response.StatusMessage;

                    IClassRetList classRetList = response.Detail as IClassRetList;
                    if (classRetList != null)
                    {
                        if (!(classRetList.Count == 0))
                        {
                            for (int ndx = 0; ndx <= (classRetList.Count - 1); ndx++)
                            {
                                IClassRet iclassRet = classRetList.GetAt(ndx);
                                Items.Add(iclassRet.FullName.GetValue());
                            }
                        }
                    }
                    else
                    {

                        Items.Add("Class Does not exist:None");
                    }

                    //iQb.CloseSession();
                    return Items;
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
    }
}
