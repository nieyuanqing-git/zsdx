using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewNMPEquipmentBLL : BLLBase<ViewNMPEquipment>, IViewNMPEquipmentBLL
    {
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewNMPEquipment> ViewNMPEquipmentList, bool isSupressLazyLoad)
        {
            NMPPrivilige NMPPrivilige = null;
            if (userId.HasValue) NMPPrivilige = PriviligeFactory.GetNMPPrivilige(userId.Value);
            if (ViewNMPEquipmentList == null || ViewNMPEquipmentList.Count == 0) return;
            foreach (var ViewNMPEquipment in ViewNMPEquipmentList)
            {
                ViewNMPEquipment.IsSupressLazyLoad = false;
                ViewNMPEquipment.InitOperate();
                ViewNMPEquipment.InitRemoteOperate();
                OperateDecorate(userId, ViewNMPEquipment, NMPPrivilige);
                ViewNMPEquipment.BuildRemoteOperate();
                ViewNMPEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewNMPEquipment ViewNMPEquipment, NMPPrivilige NMPPrivilige)
        {
            if (ViewNMPEquipment == null) throw new ArgumentException("工位关联仪器信息为空");
            if (NMPPrivilige == null)
            {
                ViewNMPEquipment.EditOperate = "";
                return;
            }
            if (!NMPPrivilige.IsEnableEdit)
            {
                ViewNMPEquipment.EditOperate = "";
            }
            if (!NMPPrivilige.IsEnableRemoteOpen)
                ViewNMPEquipment.RemoteOpenOperate = "";
            if (!NMPPrivilige.IsEnableRemoteClose)
                ViewNMPEquipment.RemoteCloseOperate = "";
            if (!NMPPrivilige.IsEnableRemoteReloadControllerStatus)
                ViewNMPEquipment.RemoteReloadOperate = "";
        }
        public IList<ViewNMPEquipment> GetViewNMPEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewNMPEquipmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPEquipmentList, isSupressLazyLoad);
            return ViewNMPEquipmentList;
        }
        public IList<ViewNMPEquipment> GetViewNMPEquipmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewNMPEquipmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPEquipmentList, isSupressLazyLoad);
            return ViewNMPEquipmentList;
        }
        public IList<ViewNMPEquipment> GetViewNMPEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewNMPEquipmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPEquipmentList, isSupressLazyLoad);
            return ViewNMPEquipmentList;
        }
        public GridData<ViewNMPEquipment> GetGridViewNMPEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewNMPEquipmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPEquipmentList == null ? null : ViewNMPEquipmentList.Data, isSupressLazyLoad);
            return ViewNMPEquipmentList;
        }
        public GridData<ViewNMPEquipment> GetGridViewNMPEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewNMPEquipmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPEquipmentList == null ? null : ViewNMPEquipmentList.Data, isSupressLazyLoad);
            return ViewNMPEquipmentList;
        }
        public IList<ViewNMPEquipment> GetUserManageViewNMPEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            if (!userId.HasValue) return null;
            var userBLL = BLLFactory.CreateUserBLL();
            var nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
            var workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            var user = userBLL.GetEntityById(userId.Value);
            if (user == null) return null;
            if (!user.IsSuperAdmin)
            {
                var nmpAdminList = nmpAdminBLL.GetEntitiesByExpression(string.Format("AdminId=\"{0}\"", user.Id));
                var queryExpression = "";
                if (nmpAdminList != null && nmpAdminList.Count() > 0)
                    queryExpression = nmpAdminList.Select(p => p.AdminId).ToFormatStr();
                var queryOrgId = workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, null, new ManagerNMPOrgId(ModuleBusinessType.NMP, "", "", "OrganizationId"));
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
        }
    }
}
