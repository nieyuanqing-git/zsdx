using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public partial class SampleManager
    {
        private SendMailMode SendMailMode
        {
            get
            {
                var dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=SampleSendMailMode");
                if (dictCodeTypes != null && dictCodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictCodeTypes.First().Value))
                {
                    var sendMode = dictCodeTypes.First().Value.Trim();
                    return (SendMailMode)System.Enum.ToObject(typeof(SendMailMode), int.Parse(sendMode));
                }
                return com.Bynonco.LIMS.Model.Enum.SendMailMode.Auto;
            }
        }
        public bool SendMessage(IEnumerable<SampleApply> sampleApplies, string address, string title, string content, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                foreach (var sampleApply in sampleApplies)
                {
                    if (JudgeIsCostDeduct(sampleApply)) _messageHandler.SendSampleApplyCostDeductNotice(sampleApply, null);
                    if (SendMailMode == Model.Enum.SendMailMode.Auto)
                    {
                        _messageHandler.SendSampleNotice(sampleApply, sampleApply.ApplicantName, null, address, title, content);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "出错," + ex.Message;
                return false;
            }
        }
    }
}
