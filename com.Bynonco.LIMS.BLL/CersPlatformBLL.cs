using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using System.Reflection;
using com.Bynonco.LIMS.Model.Business.CERS.Platform;

namespace com.Bynonco.LIMS.BLL
{
    public class CersPlatformBLL : BLLBase<CersPlatform>, ICersPlatformBLL
    {
        private IDictCodeBLL __dictCodeBLL;
        public CersPlatformBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public Platform FillPlatform()
        {
            CersPlatform cersPlatform = GetFirstOrDefaultEntityByExpression("");
            cersPlatform.SchoolPropertyCode = cersPlatform.SchoolPropertyCode.Trim().Substring(0, 1) == "0" ? cersPlatform.SchoolPropertyCode.Trim().Substring(1, 1) : cersPlatform.SchoolPropertyCode.Trim();
            if (cersPlatform == null) return null;
            Platform platform = new Platform();
            platform.SchoolCode = __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolCode");
            com.august.Core.Utility.Tools.DeeplyCopy(cersPlatform, platform);
            return platform;
        }
    }
}