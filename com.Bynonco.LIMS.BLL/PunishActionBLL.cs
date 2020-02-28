using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class PunishActionBLL : BLLBase<PunishAction>, IPunishActionBLL
    {
        private IPunishRecordBLL __punishRecordBLL;
        public PunishActionBLL()
        {
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
        }
        public bool Validates(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
            var punishRecordList = __punishRecordBLL.GetEntitiesByExpression(string.Format("PunisherId=\"{0}\"", userId.Value.ToString()));
            if (punishRecordList == null || punishRecordList.Count == 0) return true;
            var punishActionList = GetEntitiesByExpression(string.Format("HasRepeal=false*HasStoped=false*((EndAt=-null*EndAt>\"{0}\")+EndAt=null)*PunishType>{1}", DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss"),(int)PunishType.Unappointment) + "*" + punishRecordList.Select(p => p.Id).ToFormatStr("PunishRecordId"));
            if (punishActionList == null || punishActionList.Count == 0) return true;
            foreach (var punishAction in punishActionList)
            {
                if (punishAction.PunishTypeEnum >= PunishType.Unappointment)
                {
                    errorMessage = string.Format("用户被禁止预约");
                    return false;
                }
            }
            return true;
        }
    }
}