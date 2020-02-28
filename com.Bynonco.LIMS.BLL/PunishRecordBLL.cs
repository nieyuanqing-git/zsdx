using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class PunishRecordBLL : BLLBase<PunishRecord>, IPunishRecordBLL
    {
        
        public PunishRecord GetUserPubishRecord(Guid userId)
        {
            return GetFirstOrDefaultEntityByExpression(string.Format("PunisherId=\"{0}\"*HasFinished=false", userId.ToString()));
        }
    }
}