using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentNoticeBLL : BLLBase<EquipmentNotice>, IEquipmentNoticeBLL
    {
        public bool Validates(Guid equipmentId, Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            IEquipmentNoticeReadBLL equipmentNoticeReadBLL = BLLFactory.CreateEquipmentNoticeReadBLL();
            if (!userId.HasValue) return true;
            var equipmentNoticeList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*IsMustReadBeforeAppointment=true", equipmentId.ToString()));
            if (equipmentNoticeList == null || equipmentNoticeList.Count == 0) return true;
            var equipmentNoticeReadList = equipmentNoticeReadBLL.GetEntitiesByExpression(equipmentNoticeList.Select(p => p.Id).ToFormatStr("EquipmentNoticeId") + "*UserId=\"" + userId.Value.ToString() + "\"");
            errorMessage = string.Format("{0}|{1} (已阅读|需阅读)", (equipmentNoticeReadList == null ? 0 : equipmentNoticeReadList.Count),equipmentNoticeList.Count);
            if (equipmentNoticeReadList == null || equipmentNoticeReadList.Count < equipmentNoticeList.Count)
            {
                return false;
            }
            return true;
        }
    }
}