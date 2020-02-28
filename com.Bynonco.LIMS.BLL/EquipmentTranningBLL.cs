using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentTranningBLL : BLLBase<EquipmentTrainning>, IEquipmentTranningBLL
    {
        private IEquipmentBLL __equipmentBLL;
        public EquipmentTranningBLL()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
        }
        public object GetEquipmentTrainningPriviliges(Guid? userId, IList<ViewEquipmentTrainning> viewEquipmentTrainningList)
        {
            IList<EquipmentTrainningPrivilige> lstEquipmentTrainningPriviliges = new List<EquipmentTrainningPrivilige>();
            if (userId.HasValue && viewEquipmentTrainningList != null && viewEquipmentTrainningList.Count > 0)
            {
                foreach (ViewEquipmentTrainning viewEquipmentTrainning in viewEquipmentTrainningList)
                {
                    EquipmentTrainningPrivilige equipmentTrainningPrivilige = lstEquipmentTrainningPriviliges.FirstOrDefault(p => p.EquipmentTrainningId.HasValue && p.EquipmentTrainningId.Value == viewEquipmentTrainning.Id);
                    if (equipmentTrainningPrivilige == null)
                    {
                        equipmentTrainningPrivilige = PriviligeFactory.GetEquipmentTrainningPrivilige(userId.Value, viewEquipmentTrainning.Id);
                        lstEquipmentTrainningPriviliges.Add(equipmentTrainningPrivilige);
                    }
                }
            }
            return lstEquipmentTrainningPriviliges;
        }
        public bool JudgeIsEnableApplyTranning(Guid equipmentId, Guid userId)
        {
            var equipmentTrainningList = GetEntitiesByExpression(string.Format("CreatorId=\"{0}\"*EquipmentId=\"{1}\"", userId.ToString(), equipmentId.ToString()));
            return equipmentTrainningList == null || equipmentTrainningList.Count == 0;
        }
        public bool Validates(Guid equipmentId, Guid? userId,out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
            var equipment = __equipmentBLL.GetEntityById(equipmentId);
            if ((!equipment.IsNeedTranningBeforeAppointment.HasValue || !equipment.IsNeedTranningBeforeAppointment.Value) && (!equipment.IsNeedTranningBeforeUse.HasValue || !equipment.IsNeedTranningBeforeUse.Value)) return true;
            var equipmentTranningList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*CreatorId=\"{1}\"",
                                        equipmentId.ToString(),
                                        userId.Value.ToString()));
            if (equipmentTranningList == null || equipmentTranningList.Count == 0)
            {
                errorMessage = "未提交培训申请";
                return false;
            }
            var equipmentTrainning = equipmentTranningList.FirstOrDefault(p => p.StatusEnum == EquipmentTrainningStatus.Finished);
            if (equipmentTrainning == null)
            {
                errorMessage = string.Format("[{0}]培训申请尚未完成", equipmentTranningList.FirstOrDefault().CreateTime.ToString("yyyy-MM-dd"));
                return false;
            }
            errorMessage = "培训已完成";
            return true;
        }
    }
}