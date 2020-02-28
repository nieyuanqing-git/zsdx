
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //浙江大学纳米学院
    public class ZJDXNMXYCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZJDXNMXY";
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
        //public override string GetHomeFooter(string xpath)
        //{
        //    return __CODE + "Footer";
        //}
        public override bool GetIsRegisterDoc()
        {
            return true;
        }
        public override string GetSampleInnerApplyViewName()
        {
            return __CODE + "SampleApplyInner";
        }
        public override string GetSampleInnerOrOuterApplyViewName()
        {
            return __CODE + "SampleApplyInnerOrOuter";
        }
        public override string GetSampleInfoViewName()
        {
            return __CODE + "ViewSampleApplyInfo";
        }
        public override bool GetIsShowAppointmentSampleStuff()
        {
            return true;
        }

        public override bool GetIsShowAppointmentSampleSize()
        {
            return true;
        }

        public override bool GetIsShowAppointmentSampleTarget()
        {
            return true;
        }

        public override bool GetIsAppointmentEqAdminNeedSelectSampleApply()
        {
            return true;
        }
        public override bool GetIsAppointmentSampleAmountRequired()
        {
            return true;
        }
    }
}
