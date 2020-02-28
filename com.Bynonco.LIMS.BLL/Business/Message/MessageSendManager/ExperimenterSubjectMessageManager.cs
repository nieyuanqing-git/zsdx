using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class ExperimenterSubjectMessageManager : GlobalMessageManager
    {
        public ExperimenterSubjectMessageManager(object[] businessObjs, User sender)
            : base(businessObjs, sender) { }
        private bool __isHTMLTemplate = false;
        protected override MessageTemplateBase CreateMessageTemplate()
        {
            __isHTMLTemplate = true;
            return new ExperimenterSubjectMessageTemplate(Model.Enum.MessageTemplateType.HTML, this._businessObjects[0] as ExperimenterSubject, _sender);
        }
        protected override bool SendMessage()
        {
            try
            {
                if (null == this._businessObjects) return false;
                var subjectExperimenter = this._businessObjects[0] as ExperimenterSubject;
                if (subjectExperimenter == null) return false;
                //查找课题的合作导师
                var ownerSubjectExperimenter =
                  subjectExperimenter.GetSubject().Experiments.FirstOrDefault(p => p.Experimenter.Id == subjectExperimenter.Owner.Id);
                if (ownerSubjectExperimenter != null && ownerSubjectExperimenter.UseMoneyTypeEnum == Model.Enum.ExperimenterSubjectUseMoneyType.Assign)
                {
                    ExperimenterSubjectMessageTemplate ownerExperimenterSubjectMessageTemplate = new ExperimenterSubjectMessageTemplate(__isHTMLTemplate ? MessageTemplateType.HTML : MessageTemplateType.NoHTML, ownerSubjectExperimenter, _sender);
                    if (ownerExperimenterSubjectMessageTemplate == null) return false;
                    if (ownerExperimenterSubjectMessageTemplate.MessageContext.Receivers != null && ownerExperimenterSubjectMessageTemplate.MessageContext.Receivers.Count() > 0)
                    {
                        if (JudgeHasNoticeRecently(ownerExperimenterSubjectMessageTemplate.MessageContext.Title, ownerExperimenterSubjectMessageTemplate.MessageContext.Receivers.First()))
                        {
                            return true;
                        }
                    }
                    foreach (var messageSender in _messageSenders)
                        messageSender.SendMessage(ownerExperimenterSubjectMessageTemplate);
                }
                if (subjectExperimenter.UseMoneyTypeEnum == Model.Enum.ExperimenterSubjectUseMoneyType.Assign)
                {
                    ExperimenterSubjectMessageTemplate experimenterSubjectMessageTemplate = new ExperimenterSubjectMessageTemplate(__isHTMLTemplate ? MessageTemplateType.HTML : MessageTemplateType.NoHTML, subjectExperimenter, _sender);
                    if (experimenterSubjectMessageTemplate != null)
                    {
                        if (experimenterSubjectMessageTemplate.MessageContext.Receivers != null && experimenterSubjectMessageTemplate.MessageContext.Receivers.Count() > 0)
                        {
                            if (JudgeHasNoticeRecently(experimenterSubjectMessageTemplate.MessageContext.Title, experimenterSubjectMessageTemplate.MessageContext.Receivers.First()))
                            {
                                return true;
                            }
                        }
                        foreach (var messageSender in _messageSenders)
                            messageSender.SendMessageSeparately(experimenterSubjectMessageTemplate);
                    }
                }
                return true;
            }
            catch { return false; }
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            __isHTMLTemplate = false;
            return new ExperimenterSubjectMessageTemplate(Model.Enum.MessageTemplateType.NoHTML, this._businessObjects[0] as ExperimenterSubject, _sender);
        }
    }
}
