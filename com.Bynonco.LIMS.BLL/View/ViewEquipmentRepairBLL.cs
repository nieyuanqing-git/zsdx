using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewEquipmentRepairBLL : BLLBase<ViewEquipmentRepair>, IViewEquipmentRepairBLL
    {
        private IUserWorkScopeBLL __userWorkScopeBLL;
        public ViewEquipmentRepairBLL()
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
        }
        public IList<ViewEquipmentRepair> GetUserManageViewEquipmentRepair(Guid? userId, DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return null;
            var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.Value.ToString()));
            if (userWorkScopeList == null || userWorkScopeList.Count == 0) return null;
            return GetEntitiesByExpression(dataGridSettings.AppendAndQueryExpression(userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr()));
        }
    }
}
