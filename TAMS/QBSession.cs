using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAMS
{
    public class QBSession : IQBSession
    {
        private QBSessionManager sessionManager;
        public QBSessionManager GetSessionManager()
        {
            return this.sessionManager;
        }
        public QBSession(string qbfilename, string appname)
        {
            try
            {
                QBFileName = qbfilename;
                sessionManager = new QBSessionManager();
                sessionManager.OpenConnection("appname", appname);
                sessionManager.BeginSession(qbfilename, ENOpenMode.omDontCare);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IMsgSetRequest getLatestMsgSetRequest()
        {

            // IY: Find and adapt to supported version of QuickBooks
            double supportedVersion = QBFCLatestVersion();
            // MessageBox.Show("supportedVersion = " + supportedVersion.ToString());

            short qbXMLMajorVer = 0;
            short qbXMLMinorVer = 0;
            if (supportedVersion >= 6.0)
            {
                qbXMLMajorVer = 6;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 5.0)
            {
                qbXMLMajorVer = 5;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 4.0)
            {
                qbXMLMajorVer = 4;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 3.0)
            {
                qbXMLMajorVer = 3;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 2.0)
            {
                qbXMLMajorVer = 2;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 1.1)
            {
                qbXMLMajorVer = 1;
                qbXMLMinorVer = 1;
            }
            else
            {
                qbXMLMajorVer = 1;
                qbXMLMinorVer = 0;
            }
            IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("UK", qbXMLMajorVer, qbXMLMinorVer);
            return requestMsgSet;
        }
        private double QBFCLatestVersion()
        {

            IMsgSetRequest msgset = this.sessionManager.CreateMsgSetRequest("UK", 13, 0);
            msgset.AppendHostQueryRq();

            IMsgSetResponse QueryResponse = sessionManager.DoRequests(msgset);
            IResponse response = QueryResponse.ResponseList.GetAt(0);
            IHostRet HostResponse = response.Detail as IHostRet;
            IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

            int i;
            double vers;
            double LastVers = 0;
            string svers = null;

            for (i = 0; i <= supportedVersions.Count - 1; i++)
            {
                svers = supportedVersions.GetAt(i);
                vers = Convert.ToDouble(svers);
                if (vers > LastVers)
                {
                    LastVers = vers;
                    //svers = supportedVersions.GetAt(i);
                }
            }

            // IY: Close the session and connection with QuickBooks

            return LastVers;
        }
        public string QBFileName { get; set; }
        public void CloseSession()
        {
            sessionManager.EndSession();
            sessionManager.CloseConnection();
        }
    }
}
