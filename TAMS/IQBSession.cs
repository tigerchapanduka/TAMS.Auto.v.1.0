using QBFC13Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAMS
{
    public interface IQBSession
    {
        //IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager, string filename);
        IMsgSetRequest getLatestMsgSetRequest();
        void CloseSession();
        QBSessionManager GetSessionManager();

    }
}
