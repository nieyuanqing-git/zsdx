using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentAdditionChargeItemBLL : BLLBase<EquipmentAdditionChargeItem>, IEquipmentAdditionChargeItemBLL
    {
        private IEquipmentLabelBLL __equipmentLabelBLL;
        private IEquipmentLabelChargeStandardBLL __equipmentLabelChargeStandardBLL;
        private IEquipmentLabelAddtionChargeItemBLL __equipmentLabelAddtionChargeItemBLL;
        public EquipmentAdditionChargeItemBLL()
        {
            __equipmentLabelBLL = BLLFactory.CreateEquipmentLabelBLL();
            __equipmentLabelChargeStandardBLL = BLLFactory.CreateEquipmentLabelChargeStandardBLL();
            __equipmentLabelAddtionChargeItemBLL = BLLFactory.CreateEquipmentLabelAddtionChargeItemBLL();
        }
        private IList<EquipmentAdditionChargeItem> GeEquipmentAdditionChargeItemList(Guid equipmentId, Guid? userId, Guid? subjectId)
        {
            if (!userId.HasValue) return null;
            var equipmentLable = __equipmentLabelBLL.GetEquipmentLabelByUserId(equipmentId, userId.Value,subjectId);
            if (equipmentLable != null)
            {
                var equipmentLabelChargeStandardList = __equipmentLabelChargeStandardBLL.GetEntitiesByExpression(string.Format("EquipmentLabelId=\"{0}\"", equipmentLable.Id.ToString()));
                if (equipmentLabelChargeStandardList != null && equipmentLabelChargeStandardList.Count > 0)
                {
                    var equipmentLabelAddtionChargeItemList = __equipmentLabelAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("EquipmentLabelId=\"{0}\"", equipmentLable.Id.ToString()));
                    if (equipmentLabelAddtionChargeItemList == null || equipmentLabelAddtionChargeItemList.Count == 0) return null;
                    var equipmentAdditionChargeItemList = GetEntitiesByExpression(equipmentLabelAddtionChargeItemList.Select(p => p.EquipmentAdditionChargeItemId).ToFormatStr() + "*IsStop=false*IsDelete=false");
                    if (equipmentAdditionChargeItemList != null && equipmentAdditionChargeItemList.Count > 0)
                    {
                        foreach (var equipmentAdditionChargeItem in equipmentAdditionChargeItemList)
                            equipmentAdditionChargeItem.StandardPrice = equipmentLabelAddtionChargeItemList.First(p => p.EquipmentAdditionChargeItemId == equipmentAdditionChargeItem.Id).StandardPrice;
                    }
                    return equipmentAdditionChargeItemList;
                }
            }
            return GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*IsStop=false*IsDelete=false", equipmentId.ToString()));
        }
        public IList<IAdditionChargeItem> GetEquipmentAdditionChargeItemListByEquipmentIdAndUserId(Guid equipmentId, Guid userId, Guid? subjectId)
        {
            var equipmentAdditionChargeItemList = GeEquipmentAdditionChargeItemList(equipmentId, userId, subjectId);
            if (equipmentAdditionChargeItemList == null || equipmentAdditionChargeItemList.Count == 0) return null;
            IList<IAdditionChargeItem> lstAdditionChargeItem = new List<IAdditionChargeItem>();
            foreach (var equipmentAdditionChargeItem in equipmentAdditionChargeItemList)
            {
                lstAdditionChargeItem.Add(equipmentAdditionChargeItem);
            }
            return lstAdditionChargeItem;
        }
        public IList<EquipmentAdditionChargeItem> GetUserEquipmentAdditionChargeItemList(Guid equipmentId, Guid? userId, Guid? subjectId)
        {
            return GeEquipmentAdditionChargeItemList(equipmentId, userId,  subjectId);
        }

    }
}
