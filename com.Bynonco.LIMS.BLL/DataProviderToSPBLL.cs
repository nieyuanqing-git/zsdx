using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class DataProviderToSPBLL : IDataProviderToSPBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private static DataProviderToSPSetting __dataProviderToSPSetting;
        public DataProviderToSPBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        public DataProviderToSPSetting GetDataProviderToSPSetting()
        {
            if (__dataProviderToSPSetting == null)
            {
                var authrizeURL = ".*";
                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=DataProviderInterface");
                if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                {
                    var autherizaURLDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "AutherizeURL" && !string.IsNullOrWhiteSpace(p.Value));
                    if (autherizaURLDictCode != null) authrizeURL = autherizaURLDictCode.Value;
                }
                __dataProviderToSPSetting = new DataProviderToSPSetting(authrizeURL);
            }
            return __dataProviderToSPSetting;

        }
    }
}
