using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewMaterialInfoBLL : BLLBase<ViewMaterialInfo>, IViewMaterialInfoBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;

        public ViewMaterialInfoBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialInfo> viewMaterialInfoList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialInfoList == null || viewMaterialInfoList.Count == 0) return;
            foreach (var viewMaterialInfo in viewMaterialInfoList)
            {
                viewMaterialInfo.IsSupressLazyLoad = false;
                viewMaterialInfo.InitOperate();
                OperateDecorate(userId, viewMaterialInfo);
                viewMaterialInfo.BuildOperate();
                viewMaterialInfo.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialInfo viewMaterialInfo)
        {
            if (viewMaterialInfo == null) throw new ArgumentException("耗材信息为空");
            var materialInfoPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialInfoPrivilige(userId.Value, viewMaterialInfo.Id) : null;
            if (materialInfoPrivilige == null)
            {
                viewMaterialInfo.ModifyOperate = "";
                viewMaterialInfo.DeleteOperate = "";
                return;
            }
            if (!materialInfoPrivilige.IsEnableEdit) viewMaterialInfo.ModifyOperate = "";
            if (!materialInfoPrivilige.IsEnableDelete) viewMaterialInfo.DeleteOperate = "";
            if (!materialInfoPrivilige.IsEnableNote) viewMaterialInfo.NoteOperate = "";
        }
        public IList<ViewMaterialInfo> GetViewMaterialInfoListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInfoList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInfoList, isSupressLazyLoad);
            return viewMaterialInfoList;
        }
        public IList<ViewMaterialInfo> GetViewMaterialInfoListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInfoList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInfoList, isSupressLazyLoad);
            return viewMaterialInfoList;
        }
        public IList<ViewMaterialInfo> GetViewMaterialInfoListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInfoList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInfoList, isSupressLazyLoad);
            return viewMaterialInfoList;
        }
        public GridData<ViewMaterialInfo> GetGridViewMaterialInfoListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInfoList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInfoList == null ? null : viewMaterialInfoList.Data, isSupressLazyLoad);
            return viewMaterialInfoList;
        }
        public GridData<ViewMaterialInfo> GetGridViewMaterialInfoListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInfoList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInfoList == null ? null : viewMaterialInfoList.Data, isSupressLazyLoad);
            return viewMaterialInfoList;
        }
      
        public IList<ViewMaterialInfo> GetManageViewMaterialInfoList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialInfo(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        public int GetManageViewMaterialInfoCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialInfo(userId, ref dataGridSettings)) return 0;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }

        private bool IsManageMaterialInfo(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var materialAdminList = __materialAdminBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id), null, "", true);
                var queryExpression = "";
                if (materialAdminList != null && materialAdminList.Count() > 0) queryExpression = materialAdminList.Select(p => p.MaterialInfoId).ToFormatStr();
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Material, "", "", "OrganizationId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "" && queryExpression != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
        public IEnumerable<Guid> GetManageMaterialInfoIds(Guid? userId, DataGridSettings dataGridSettings)
        {
            IsManageMaterialInfo(userId, ref dataGridSettings);
            var ManageMaterialInfoList = GetEntitiesByExpression(dataGridSettings, null, "", true);
            if (ManageMaterialInfoList != null && ManageMaterialInfoList.Count() > 0)
                return ManageMaterialInfoList.Select(p => p.Id).Distinct();
            else return null;
        }
        
    }
}
