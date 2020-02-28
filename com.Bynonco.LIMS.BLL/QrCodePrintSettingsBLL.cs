using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class QrCodePrintSettingsBLL : IQrCodePrintSettingsBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public QrCodePrintSettingsBLL() 
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        public Model.Business.QrCodePrintSettings GetSampleQrCodePrintSettings()
        {
            string defaultPrinterName = "";
            int width = 50, height = 50, leftMargin = 0, topMargin = 0;
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=SampleQrCodePrintSettings*IsDelete=false");
            if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
            {
                var defaultPrinterDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "SampleDefaultQrCodePrinter");
                if (defaultPrinterDictCode != null && !string.IsNullOrWhiteSpace(defaultPrinterDictCode.Value))
                {
                    defaultPrinterName = defaultPrinterDictCode.Value;
                }
                var topMarginDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "SampleQrCodeTopMargin");
                if (topMarginDictCode != null && !string.IsNullOrWhiteSpace(topMarginDictCode.Value) && int.TryParse(topMarginDictCode.Value.Trim(), out topMargin))
                {
                    topMargin = int.Parse(topMarginDictCode.Value.Trim());
                }
                var heightDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "SampleQrCodeHeight");
                if (heightDictCode != null && !string.IsNullOrWhiteSpace(heightDictCode.Value) && int.TryParse(heightDictCode.Value.Trim(), out height))
                {
                    height = int.Parse(heightDictCode.Value.Trim());
                }
                var leftMarginDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "SampleQrCodeLeftMargin");
                if (leftMarginDictCode != null && !string.IsNullOrWhiteSpace(leftMarginDictCode.Value) && int.TryParse(leftMarginDictCode.Value.Trim(), out leftMargin))
                {
                    leftMargin = int.Parse(leftMarginDictCode.Value.Trim());
                }
                var widthMarginDictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "SampleQrcodeWidth");
                if (widthMarginDictCode != null && !string.IsNullOrWhiteSpace(widthMarginDictCode.Value) && int.TryParse(widthMarginDictCode.Value.Trim(), out width))
                {
                    width = int.Parse(widthMarginDictCode.Value.Trim());
                }
            }
            return new Model.Business.QrCodePrintSettings(defaultPrinterName, width, height, leftMargin, topMargin);
        }
    }
}
