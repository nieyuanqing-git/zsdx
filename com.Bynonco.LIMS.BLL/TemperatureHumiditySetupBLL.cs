using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Business.CERS.InstrusAndGroups;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class TemperatureHumiditySetupBLL : BLLBase<TemperatureHumiditySetup>, ITemperatureHumiditySetupBLL
    {
        public object SendTemperatureHumidity(string url, string methodname, object[] args)
        {
            return CallWebService.InvokeWebService(url, methodname, args);
        }
    }
}