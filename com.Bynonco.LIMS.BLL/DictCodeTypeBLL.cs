using System;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class DictCodeTypeBLL : BLLBase<DictCodeType>, IDictCodeTypeBLL
    {
        public DictCodeType GetAndSaveDictCodeType(string code, string name, string value, Guid? parentId, ref XTransaction tran, bool isSupress = false)
        {
            var item = GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", code) + (parentId.HasValue ? string.Format("*ParentId=\"{0}\"", parentId.Value) : ""));
            if (item == null)
            {
                item = new DictCodeType() { Id = Guid.NewGuid(), Code = code, Name = name, Value = value, ParentId = parentId };
                Add(new DictCodeType[] { item }, ref tran, isSupress);
            }
            else
            {
                item.Value = value;
                Save(new DictCodeType[] { item }, ref tran, isSupress);
            }
            return item;
        }
        public string[] GetWhenSampleCharge()
        {
            var dictCodeType = GetFirstOrDefaultEntityByExpression("Code=WhenSampleCharge");
            if (dictCodeType == null) throw new Exception("出错,找不到WhenSampleCharge配置");
            if (string.IsNullOrWhiteSpace(dictCodeType.Value)) throw new Exception("出错,WhenSampleCharge为空");
            return dictCodeType.Value.Trim().Replace("，", ",").Split(',');
        }
        public string[] GetWhenSampleCancelCharge()
        {
            var dictCodeType = GetFirstOrDefaultEntityByExpression("Code=WhenSampleCancelCharge");
            if (dictCodeType == null) throw new Exception("出错,找不到WhenSampleCancelCharge配置");
            if (string.IsNullOrWhiteSpace(dictCodeType.Value)) throw new Exception("出错,WhenSampleCancelCharge为空");
            return dictCodeType.Value.Trim().Replace("，", ",").Split(',');
        }
        public EquipmentTimeAppointmemtMode GetEquipmentTimeAppointmemtMode()
        {
            EquipmentTimeAppointmemtMode equipmentTimeAppointmemtMode = EquipmentTimeAppointmemtMode.CommondCalendar;
            var equipmentTimeAppointmemtModeDictCodeType = GetFirstOrDefaultEntityByExpression("Code=EquipmentTimeAppointmemtMode*IsDelete=false");
            if (equipmentTimeAppointmemtModeDictCodeType != null &&
                !string.IsNullOrWhiteSpace(equipmentTimeAppointmemtModeDictCodeType.Value) &&
                equipmentTimeAppointmemtModeDictCodeType.Value.Trim().IsInt() &&
                Enum.TryParse<EquipmentTimeAppointmemtMode>(equipmentTimeAppointmemtModeDictCodeType.Value.Trim(), out equipmentTimeAppointmemtMode)
             )
            {
                equipmentTimeAppointmemtMode = (EquipmentTimeAppointmemtMode)int.Parse(equipmentTimeAppointmemtModeDictCodeType.Value.Trim());
            }
            return equipmentTimeAppointmemtMode;
        }
    }
}
