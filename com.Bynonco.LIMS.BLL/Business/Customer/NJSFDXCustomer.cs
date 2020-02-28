
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //南京师范大学
    public class NJSFDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "NJSFDX";
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetEquipmentShowBasic()
        {
            return __CODE + "ShowBasic";
        }
        public override string GetSampleInnerApplyViewName()
        {
            return __CODE + "SampleApplyInner";
        }
        public override string GetSampleInnerOrOuterApplyViewName()
        {
            return __CODE + "SampleApplyInnerOrOuter";
        }
        public override string GetSubjectProjectShowName()
        {
            return "课题名称";
        }
        public override string GetSampleInfoViewName()
        {
            return __CODE + "ViewSampleApplyInfo";
        }
        public override bool JudgeIsEnableEditSampleAppyNumber()
        {
            return false;
        }
        public override bool GetIsAdminEnableChangeAppointmentOfOtherUser()
        {
            return false;
        }
        public override bool GetIsAdminEnableCancelAppointmentOfOtherUser()
        {
            return false;
        }
        public override bool GetIsDepositTesterRequired()
        {
            return true;
        }
        public override string GetDepositSamplyNoDisplayName()
        {
            return "送样单号";
        }
        public override bool GetIsSendMessageToTutorSameTime()
        {
            return true;
        }
    }
}
