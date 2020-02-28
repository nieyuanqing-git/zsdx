using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Business;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class DictCodeBLL : BLLBase<DictCode>, IDictCodeBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public DictCodeBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }

        public DictCode GetAndSaveDictCode(string code, string name, string value, Guid dictCodeTypeId, ref XTransaction tran, bool isSupress = false)
        {
            var item = GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*DictCodeTypeId=\"{1}\"*IsDelete=false", code, dictCodeTypeId));
            if (item == null)
            {
                item = new DictCode() { Id = Guid.NewGuid(), Code = code, Name = name, Value = value, DictCodeTypeId = dictCodeTypeId };
                Add(new DictCode[] { item }, ref tran, isSupress);
            }
            else
            {
                item.Value = value;
                Save(new DictCode[] { item }, ref tran, isSupress);
            }
            return item;
        }
        public DictCode GetDictCodeByCode(string dictcodeTypeCode, string dictCodeCode)
        {
            var dictCodeTypes = __dictCodeTypeBLL.GetEntitiesByExpression(string.Format("Code=\"{0}\"", dictcodeTypeCode));
            if (dictCodeTypes != null)
            {
                var dictCodeType = dictCodeTypes.First();
                if (dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                {
                    var dictCodes = dictCodeType.DictCodes;
                    return dictCodes.FirstOrDefault(p => p.Code.ToLower() == dictCodeCode.ToLower());
                }
            }
            return null;
        }
        public string GetDictCodeValueByCode(string dictcodeTypeCode, string dictCodeCode)
        {
            var dictCode = GetDictCodeByCode(dictcodeTypeCode, dictCodeCode);
            if (dictCode != null) return dictCode.Value;
            return "";
        }
        public bool? GetDictCodeBoolValueByCode(string dictcodeTypeCode, string dictCodeCode)
        {
            bool? isTrue = null;
            var str = GetDictCodeValueByCode(dictcodeTypeCode, dictCodeCode);
            if(!string.IsNullOrWhiteSpace(str))
            {
                if(str.Trim() == "1") isTrue = true;
                else if(str.Trim() =="0") isTrue = false;
            }
            return isTrue;
        }

        public bool JudgeIsSelectEquipmentDependsOnTutor(out UserSelectEquipmentType userSelectEquipmentType)
        {
            var dictCode = GetDictCodeByCode("SelectEquipment", "SelectEquipmentMethod");
            userSelectEquipmentType = UserSelectEquipmentType.UseAll;
            bool isDependsOnTutor = false;
            if (dictCode != null && dictCode.Value.IsInt())
            {
                try
                {
                    userSelectEquipmentType = (UserSelectEquipmentType)(int.Parse(dictCode.Value));
                    isDependsOnTutor = userSelectEquipmentType == UserSelectEquipmentType.DependsOnTutor;
                }
                catch { };
            }
            return isDependsOnTutor;
        }
        public int GetDefaultMinAppointmentCancelMinute()
        {
            var minCancelAppointmentMinute = 30;
            var dictcodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=EquipmentDefaultSetting");
            if (dictcodeType != null && dictcodeType.DictCodes != null && dictcodeType.DictCodes.Any(p => p.Code == "MinAppointmentCancelTime"))
            {
                minCancelAppointmentMinute = int.Parse(dictcodeType.DictCodes.First(p => p.Code == "MinAppointmentCancelTime").Value);
            }
            return minCancelAppointmentMinute;
        }
       
        public string GetLoginJumpTypeHref()
        {
            var dictCode = GetDictCodeByCode("System", "LoginJumpType");
            if (dictCode == null || dictCode.Value == "") return "";
            LoginJumpType loginJumpType = LoginJumpType.Location;
            try
            {
                loginJumpType = (LoginJumpType)System.Enum.ToObject(typeof(LoginJumpType), int.Parse(dictCode.Value));
            }
            catch{}
            var href = loginJumpType.ToAction();
            if (href == "" && loginJumpType == LoginJumpType.Other)
                href = dictCode.Ext1;
            return href;
        }
        
        public double GetEquipmentRepairFundsBigAmountDefinition()
        {
            double bigAmount = 20000d;
            var bigAmountStr = GetDictCodeValueByCode("EquipmentRepairFunds", "EquipmentRepairFundsBigAmount");
            if (!string.IsNullOrWhiteSpace(bigAmountStr) && double.TryParse(bigAmountStr, out bigAmount))
            {
                return bigAmount;
            }
            return bigAmount;
        }
        public DictCode GenerateDefaultCertificateType(string certificateTypeName, ref XTransaction tran)
        {
            certificateTypeName = certificateTypeName == null ? "" : certificateTypeName.Trim();
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=\"CertificateType\"*IsDelete=false");
            if (dictCodeType == null) throw new Exception("找不到证件类型的定义");
            var dictCode = dictCodeType.DictCodes != null ? dictCodeType.DictCodes.FirstOrDefault(p => string.Equals(p.Name == null ? "" : p.Name.Trim(), certificateTypeName, StringComparison.OrdinalIgnoreCase)) : null;
            if (dictCode == null)
            {
                dictCode = new DictCode() { Id = Guid.NewGuid(), Name = certificateTypeName, Code = certificateTypeName, Value = certificateTypeName, DictCodeTypeId = dictCodeType.Id, IsDelete = false, IsStop = false };
                Add(new DictCode[] { dictCode }, ref tran, true);
            }
            return dictCode;
        }
        public DictCode GenerateDictCode(string dictCodeTypeCode, string code, string name, string value, ref XTransaction tran)
        {
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", dictCodeTypeCode.Trim()));
            if (dictCodeType == null) throw new Exception(string.Format("找不到编码为【{0}】的DictCodeType定义", dictCodeTypeCode.Trim()));
            var dictCode = dictCodeType.DictCodes != null ? dictCodeType.DictCodes.FirstOrDefault(p => p.Name == name.Trim()) : null;
            if (dictCode == null)
            {
                dictCode = new DictCode() { Id = Guid.NewGuid(), Name = name, Code = code.Trim(), Value = value, DictCodeTypeId = dictCodeType.Id, IsDelete = false, IsStop = false };
                Add(new DictCode[] { dictCode }, ref tran, true);
            }
            return dictCode;
        }
        public DictCode GetDictCodeByCodeTypeCodeAndExt1(string dictCodeTypeCode, string ext1)
        {
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", dictCodeTypeCode.Trim()));
            if (dictCodeType == null) 
                return null;
            var dictCode = dictCodeType.DictCodes != null ? dictCodeType.DictCodes.FirstOrDefault(p => p.Ext1 == ext1.Trim()) : null;            
            return dictCode;
        }
    }
}
