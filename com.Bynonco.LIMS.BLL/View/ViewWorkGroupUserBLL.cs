using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewWorkGroupUserBLL : BLLBase<ViewWorkGroupUser>, IViewWorkGroupUserBLL
    {
        //public GridData<ViewWorkGroupUser> GetGridViewWorkGroupUserListByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "")
        //{
        //    DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
        //    //if (isManageList && !IsManageAppointment(userId, ref dataGridSettings)) return null;
        //    expression = dataGridSettings.QueryExpression;
        //    var viewWorkGroupUserList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression);
        //    //if (isIniOperate) ExcuteOperateDecorate(userId, viewAppointmentList, isSupressLazyLoad);
        //    return viewWorkGroupUserList;
        //}
    }
}
