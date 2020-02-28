using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleApplyCostDeductMessageTemplate : MessageTemplateBase
    {
        private SampleApply __sampleApply;
        public SampleApplyCostDeductMessageTemplate(MessageTemplateType messageTemplateType, SampleApply sampleApply, User sender)
            : base(messageTemplateType, sender)
        {
            this.__sampleApply = sampleApply;
        }
        protected override MessageContext GenerateMessageContext()
        {
            string title = string.Format("$ReceiverInfo$发生送样扣费，送样单号:{0}", __sampleApply.SampleNo + "-" + __sampleApply.RowNo);
            string content = string.Format("{0}送样扣费,样品数:{1},检测项目:{2},扣费:{3}元", __sampleApply.SampleNo + "-" + __sampleApply.RowNo, __sampleApply.RealQuatity.HasValue ? __sampleApply.RealQuatity.Value : __sampleApply.Quatity, __sampleApply.SampleItem.Name, __sampleApply.RealCurrency + __sampleApply.VirtualCurrency);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", title, content, null, _sender, __sampleApply.Applicant, new User[] { __sampleApply.Applicant }, __sampleApply.Applicant, __sampleApply.Id, null);
        }
    }
}
