using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentLabelChargeStandardBLL : BLLBase<EquipmentLabelChargeStandard>, IEquipmentLabelChargeStandardBLL
    {
       
        public int GetEquipmentLabelChargeStandardCount(Guid equipmentId, Guid labelId)
        {
            var count = CountModelListByExpression(string.Format("EquipmentId=\"{0}\"*EquipmentLabelId=\"{1}\"",equipmentId, labelId));
            return count;
        }
        public IList<EquipmentLabelChargeStandard> GetEquipmentLabelChargeStandardList(Guid equipmentId, Guid labelId)
        {
            var equipmentLabelChargeStandardList = GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*EquipmentLabelId=\"{1}\"", equipmentId, labelId));
            return equipmentLabelChargeStandardList;
        }
        public EquipmentLabelChargeStandard GetEquipmentLabelChargeStandard(Guid equipmentId, Guid relationId)
        {
            return GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*RelationId=\"{1}\"", equipmentId, relationId));
        }
        public IList<ILabelChargeStandard> GetEquipmentLabelChargeStandardListByEquipmentIdAndLableId(Guid equipmentId, Guid labelId)
        {
            IList<EquipmentLabelChargeStandard> equipmentLabelChargeStandardList = GetEquipmentLabelChargeStandardList(equipmentId, labelId);
            if (equipmentLabelChargeStandardList == null || equipmentLabelChargeStandardList.Count == 0) return null;
            IList<ILabelChargeStandard> lstLabelChargeStandard = new List<ILabelChargeStandard>();
            foreach (var equipmentLabelChargeStandard in equipmentLabelChargeStandardList) lstLabelChargeStandard.Add(equipmentLabelChargeStandard);
            return lstLabelChargeStandard;

        }
    }
}
