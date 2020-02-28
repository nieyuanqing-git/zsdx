using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class BussinessIdManager
    {
        public static IDictCodeTypeBLL __dictCodeTypeBLL;
        public static IDictCodeBLL __dictCodeBLL;
        private static object __objLock = new object();
        private static BussinessIdManager __bussinessIdManager;
        private BussinessIdManager() { }
        public static BussinessIdManager GetInstance()
        {
            lock (__objLock)
            {
                return __bussinessIdManager == null ? new BussinessIdManager() : __bussinessIdManager;
            }
        }
        static BussinessIdManager()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public string GenerateSampleApplySerialNo()
        {
            var sampleSerialNoLength = 5;
            var sampleNoFormatStr = "yyyy-MM-dd";
            var sampleNoFormatDictCodeTyeps = __dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleNoFormat*IsDelete=false*IsStop=false");
            if (sampleNoFormatDictCodeTyeps != null && sampleNoFormatDictCodeTyeps.Count > 0)
            {
                var sampleNoFormatDictCodeTyep = sampleNoFormatDictCodeTyeps.First();
                if (!string.IsNullOrWhiteSpace(sampleNoFormatDictCodeTyep.Value))
                    sampleNoFormatStr = sampleNoFormatDictCodeTyep.Value;
            }
            var sampleApplySerilaNoLengthDictCodeTyeps = __dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleApplySerilaNoLength*IsDelete=false*IsStop=false");
            if(sampleApplySerilaNoLengthDictCodeTyeps!= null && sampleApplySerilaNoLengthDictCodeTyeps.Count>0)
            {
                var sampleApplySerilaNoLengthDictCodeTyep = sampleApplySerilaNoLengthDictCodeTyeps.First();
                if(!string.IsNullOrWhiteSpace(sampleApplySerilaNoLengthDictCodeTyep.Value) && int.TryParse(sampleApplySerilaNoLengthDictCodeTyep.Value,out sampleSerialNoLength))
                {
                    var no = GenerateBusinessIdBLL.Generate(string.Format("SampleSerialNumber-{0}", DateTime.Now.ToString(sampleNoFormatStr))).ToString();
                    if (sampleSerialNoLength == -1) return string.Format("{0}-{1}", DateTime.Now.ToString(sampleNoFormatStr), no.ToString());
                    if (no.Length < sampleSerialNoLength)
                        no = string.Format("{0}-{1}", DateTime.Now.ToString(sampleNoFormatStr), no.PadLeft(sampleSerialNoLength, '0'));
                    return no;
                }
            }
            throw new Exception("SampleApplySerilaNoLength配置错误");
        }
        public string GenerateOpenFundApplySerialNo()
        {
            return GenerateSerialNo("OpenFundApply", "ApplyNumFormat", "ApplySerilaNoLength", "OpenFundApplySerialNo");
        }
        public string GenerateJudgeEquipmentRecordSerialNo()
        {
            return GenerateSerialNo("JudgeEquipmentRecord", "RecordNumFormat", "RecordNumLength", "JudgeEquipmentRecordSerialNo");
        }
        public string GenerateMaterialPurchaseSerialNo()
        {
            return GenerateSerialNo("MaterialPurchase", "PurchaseNumFormat", "PurchaseNumLength", "PurchaseNumSerilaNo");
        }
        private string GenerateSerialNo(string dictCodeTypeCode, string formatCode, string lengthCode, string serialHeader)
        {
            var serialNoLength = -1;
            var serialNoFormat = __dictCodeBLL.GetDictCodeValueByCode(dictCodeTypeCode, formatCode);
            if (string.IsNullOrWhiteSpace(serialNoFormat)) serialNoFormat = "yyyy-MM-dd";

            var serilaNoLengthStr = __dictCodeBLL.GetDictCodeValueByCode(dictCodeTypeCode, lengthCode);
            if (!string.IsNullOrWhiteSpace(serilaNoLengthStr) && serilaNoLengthStr.IsInt()) serialNoLength = int.Parse(serilaNoLengthStr);

            var no = GenerateBusinessIdBLL.Generate(string.Format("{0}-{1}", serialHeader, DateTime.Now.ToString(serialNoFormat))).ToString();
            if (serialNoLength == -1) return string.Format("{0}-{1}", DateTime.Now.ToString(serialNoFormat), no.ToString());
            if (no.Length < serialNoLength)
                no = string.Format("{0}-{1}", DateTime.Now.ToString(serialNoFormat), no.PadLeft(serialNoLength, '0'));
            return no;
        }
    }
}
