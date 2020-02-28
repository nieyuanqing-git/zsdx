using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //中大眼科中心
    public class ZDYKZXCustomer : DefaultCustomer
    {
        private readonly string __CODE = "ZDYKZX";
        public override string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public override bool GetIsRegistUserUploadRelativePic()
        {
            return true;
        }
        public override bool GetIsRegistUserAnimalExperimentInfo()
        {
            return true;
        }
        public override bool GetIsOpenAnimalModules()
        {
            return true;
        }
        public override bool GetIsStartAnimalModule()
        {
            return true;
        }
        public override bool GetIsUserManageSubjectProject()
        {
            return true;
        }

        public override string GetDefaultAnimalEnviromentQuatityNo()
        {
            return "SYXK(粤)2010-0058";
        }
    }
}
