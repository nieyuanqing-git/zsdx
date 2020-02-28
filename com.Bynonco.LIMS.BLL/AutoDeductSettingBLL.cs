using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using com.august.DataLink;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class AutoDeductSettingBLL : IAutoDeductSettingBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IDictCodeBLL __dictCodeBLL;
        public AutoDeductSettingBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }

        public Model.Business.AutoDeductSetting GetAutoDeductSetting()
        {
            var timeDiff = 60;
            var days = 0;
            int isValidateNum = 0;
            bool isValidate = false;
            IList<Guid> equipmentIds = null;
            var autoDeductSettingDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=AutoDeductSetting*IsDelete=false");
            if (autoDeductSettingDictCodeType != null && autoDeductSettingDictCodeType.DictCodes != null && autoDeductSettingDictCodeType.DictCodes.Count > 0)
            {
                var timeDiffDictCode = autoDeductSettingDictCodeType.DictCodes.FirstOrDefault(p => p.Code == "TimeDiff");
                if (timeDiffDictCode != null && !string.IsNullOrWhiteSpace(timeDiffDictCode.Value) && int.TryParse(timeDiffDictCode.Value.Trim(),out timeDiff))
                {
                    timeDiff = int.Parse(timeDiffDictCode.Value.Trim());
                }
                var daysDictCode = autoDeductSettingDictCodeType.DictCodes.FirstOrDefault(p => p.Code == "Days");
                if (daysDictCode != null && !string.IsNullOrWhiteSpace(daysDictCode.Value) && int.TryParse(daysDictCode.Value.Trim(), out days))
                {
                    days = int.Parse(daysDictCode.Value.Trim());
                }
                var isValidateDictCode = autoDeductSettingDictCodeType.DictCodes.FirstOrDefault(p => p.Code == "IsValidate");
                if (isValidateDictCode != null && !string.IsNullOrWhiteSpace(isValidateDictCode.Value) && int.TryParse(isValidateDictCode.Value.Trim(), out isValidateNum))
                {
                    isValidate = isValidateNum > 0;
                }
                var equipmentIdDictCode = autoDeductSettingDictCodeType.DictCodes.FirstOrDefault(p => p.Code == "EquipmentIds");
                if (equipmentIdDictCode != null && !string.IsNullOrWhiteSpace(equipmentIdDictCode.Value))
                {
                    equipmentIds = equipmentIdDictCode.Value.Trim().Split(',').Select(p => new Guid(p)).ToList();
                }
            }
            AutoDeductSetting autoDeductSetting = new AutoDeductSetting(timeDiff, days, equipmentIds, isValidate);
            return autoDeductSetting;
        }

        public bool SaveAutoDeductSetting(Model.Business.AutoDeductSetting autoDeductSetting, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                bool isNew = true;
                IList<DictCode> dictCodes = new List<DictCode>();
                var autoDeductSettingDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=AutoDeductSetting*IsDelete=false");
                isNew = autoDeductSettingDictCodeType == null;
                if (isNew)
                {
                    autoDeductSettingDictCodeType = new Model.DictCodeType() { Id = Guid.NewGuid(), IsStop = false, IsDelete = false };
                }
                else if (autoDeductSettingDictCodeType.DictCodes != null && autoDeductSettingDictCodeType.DictCodes.Count > 0)
                {
                    __dictCodeBLL.Delete(autoDeductSettingDictCodeType.DictCodes.Select(p => p.Id), ref tran, true);
                }
                autoDeductSettingDictCodeType.Code = "AutoDeductSetting";
                autoDeductSettingDictCodeType.Name = "自动扣费设置";
                if (isNew) __dictCodeTypeBLL.Add(new DictCodeType[] { autoDeductSettingDictCodeType }, ref tran, true);
                else __dictCodeTypeBLL.Save(new DictCodeType[] { autoDeductSettingDictCodeType }, ref tran, true);
                dictCodes.Add(new DictCode()
                    {
                        Id = Guid.NewGuid(),
                        DictCodeTypeId = autoDeductSettingDictCodeType.Id,
                        Value = autoDeductSetting.TimeDiff.ToString(),
                        Code = "TimeDiff",
                        Name = "扣费时间间隔"
                    });
                dictCodes.Add(new DictCode()
                {
                    Id = Guid.NewGuid(),
                    DictCodeTypeId = autoDeductSettingDictCodeType.Id,
                    Value = autoDeductSetting.IsValidate?"1":"0",
                    Code = "IsValidate",
                    Name = "是否开启服务"
                });
                dictCodes.Add(new DictCode()
                {
                    Id = Guid.NewGuid(),
                    DictCodeTypeId = autoDeductSettingDictCodeType.Id,
                    Value = autoDeductSetting.Days.ToString(),
                    Code = "Days",
                    Name ="多少天前的预约记录"
                });
                dictCodes.Add(new DictCode()
                {
                    Id = Guid.NewGuid(),
                    DictCodeTypeId = autoDeductSettingDictCodeType.Id,
                    Value = autoDeductSetting.EquipmentIds != null && autoDeductSetting.EquipmentIds.Count() > 0 ? 
                            string.Join(",", autoDeductSetting.EquipmentIds.Select(p => p.ToString())) : "",
                    Name ="需要自动扣费的设备",
                    Code = "EquipmentIds"
                });
                if (dictCodes != null && dictCodes.Count > 0) __dictCodeBLL.Add(dictCodes, ref tran, true);
                if (tran != null && tran.HasTransaction) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                errorMessage = ex.Message;
                return false;
            }
            finally { if (tran != null) tran.RollbackTransaction(); }
        }
    }
}
