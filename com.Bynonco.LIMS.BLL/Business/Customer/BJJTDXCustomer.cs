using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class BJJTDXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJJTDX";
        public override bool GetIsShowDutyFreeEquipment()
        {
            return true;
        }
        public override bool GetIsLoadRepariUnit()
        {
            return true;
        }
        public override bool GetIsShowBJJTDXCol()
        {
            return true;
        }
        public override string GetEquipmentShowBasic()
        {
            return __CODE + "ShowBasic";
        }
        public override string GetEquipmentShowListConditionSeach()
        {
            return __CODE + "ShowListConditionSeach";
        }
        public override bool GetIsAppointmentSubjectProjectRequired()
        {
            return true;
        }
        public override bool GetIsBindEquipmentLinkUser()
        {
            return false;
        }
        public override string GetLabelShowName()
        {
            return "设备编号";
        }
        public override string GetArticleCategoryShowList()
        {
            return __CODE + "HomeShowList";
        }
        public override bool GetIsUseNatureFromDictcode()
        {
            return true;
        }
        public override bool GetIsShowMessageShareFunApply()
        {
            return true;
        }
        public override bool GetIsToSystemSJ3Index()
        {
            return true;
        }
    }
}
