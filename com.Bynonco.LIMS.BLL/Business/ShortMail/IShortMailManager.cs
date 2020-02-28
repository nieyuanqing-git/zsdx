using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.Leams.GsmComm.GsmCommunication;
namespace com.Bynonco.LIMS.BLL
{
    public interface IShortMailManager
    {
        void SendShortMail(Guid? operatorId, GsmCommMain comm, out int successCount, out int failCount, out string errorMessage);
        void SendShortMailWebRequest(Guid? operatorId, string userName, string passWord, string url, out int successCount, out int failCount, out string errorMessage);
        void SendShortMailWebRequest(int bm,Guid? operatorId, string userName, string passWord, string url, out int successCount, out int failCount, out string errorMessage);
    }
}
