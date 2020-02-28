using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Business;
using System.Configuration;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class SystemRunSettingBLL : ISystemRunSettingBLL
    {
        public Model.Business.SystemRunSetting GetSystemRunSetting()
        {
             SystemRunSetting systemRunSetting = new SystemRunSetting();
             var isRunAsTouchScreenModeStr = ConfigurationManager.AppSettings["IsRunAsTouchScreenMode"];
             systemRunSetting.IsRunAsTouchScreenMode = !string.IsNullOrWhiteSpace(isRunAsTouchScreenModeStr) && isRunAsTouchScreenModeStr.Trim() == "1";
             return systemRunSetting;
        }

        public bool SaveSystemRunSetting(SystemRunSetting systemRunSetting, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                WebConfigUtility.AddOrSaveAppSettings("IsRunAsTouchScreenMode",systemRunSetting.IsRunAsTouchScreenMode ? "1" : "0");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
    }
}
