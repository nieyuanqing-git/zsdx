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
    public class ViewMaterialBrokenBLL : BLLBase<ViewMaterialBroken>, IViewMaterialBrokenBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialBrokenItemBLL __materialBrokenItemBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;

        public ViewMaterialBrokenBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialBrokenItemBLL = BLLFactory.CreateMaterialBrokenItemBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialBroken> viewMaterialBrokenList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialBrokenList == null || viewMaterialBrokenList.Count == 0) return;
            foreach (var viewMaterialBroken in viewMaterialBrokenList)
            {
                viewMaterialBroken.IsSupressLazyLoad = false;
                viewMaterialBroken.InitOperate();
                OperateDecorate(userId, viewMaterialBroken);
                viewMaterialBroken.BuildOperate();
                viewMaterialBroken.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialBroken viewMaterialBroken)
        {
            if (viewMaterialBroken == null) throw new ArgumentException("耗材报废单为空");
            var materialBrokenPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialBrokenPrivilige(userId.Value, viewMaterialBroken.Id) : null;
            if (materialBrokenPrivilige == null)
            {
                viewMaterialBroken.ModifyOperate = "";
                viewMaterialBroken.DeleteOperate = "";
                viewMaterialBroken.AuditOperate = "";
                viewMaterialBroken.OutputOperate = "";
                viewMaterialBroken.NoteOperate = "";
                return;
            }
            if (!materialBrokenPrivilige.IsEnableEdit) viewMaterialBroken.ModifyOperate = "";
            if (!materialBrokenPrivilige.IsEnableDelete) viewMaterialBroken.DeleteOperate = "";
            if (!materialBrokenPrivilige.IsEnableOutput) viewMaterialBroken.OutputOperate = "";
            if (!materialBrokenPrivilige.IsEnableNote) viewMaterialBroken.NoteOperate = "";
            if (!materialBrokenPrivilige.IsEnableAudit) viewMaterialBroken.AuditOperate = "";
            if (viewMaterialBroken.StatusEnum != MaterialBrokenStatus.WaittingAudit) viewMaterialBroken.ModifyOperate = "";
            if (viewMaterialBroken.StatusEnum != MaterialBrokenStatus.WaittingAudit
                && viewMaterialBroken.StatusEnum != MaterialBrokenStatus.AuditPass
                && viewMaterialBroken.StatusEnum != MaterialBrokenStatus.AuditReject)
            {
                viewMaterialBroken.ModifyOperate = "";
                viewMaterialBroken.DeleteOperate = "";
                viewMaterialBroken.AuditOperate = "";
            }
            if(viewMaterialBroken.StatusEnum != MaterialBrokenStatus.AuditPass) viewMaterialBroken.OutputOperate = "";
        }
        public IList<ViewMaterialBroken> GetViewMaterialBrokenListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialBrokenList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialBrokenList, isSupressLazyLoad);
            return viewMaterialBrokenList;
        }
        public IList<ViewMaterialBroken> GetViewMaterialBrokenListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialBrokenList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialBrokenList, isSupressLazyLoad);
            return viewMaterialBrokenList;
        }
        public IList<ViewMaterialBroken> GetViewMaterialBrokenListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialBrokenList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialBrokenList, isSupressLazyLoad);
            return viewMaterialBrokenList;
        }
        public GridData<ViewMaterialBroken> GetGridViewMaterialBrokenListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialBrokenList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialBrokenList == null ? null : viewMaterialBrokenList.Data, isSupressLazyLoad);
            return viewMaterialBrokenList;
        }
        public GridData<ViewMaterialBroken> GetGridViewMaterialBrokenListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialBrokenList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialBrokenList == null ? null : viewMaterialBrokenList.Data, isSupressLazyLoad);
            return viewMaterialBrokenList;
        }
      
        public IList<ViewMaterialBroken> GetManageViewMaterialBrokenList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialBroken(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        private bool IsManageMaterialBroken(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = "";
                var materialInfoIds = __viewMaterialInfoBLL.GetManageMaterialInfoIds(userId, new DataGridSettings() { QueryExpression = "" });
                if (materialInfoIds != null && materialInfoIds.Count() > 0)
                {
                    var materialBrokenItemList = __materialBrokenItemBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", materialInfoIds.ToFormatStr("MaterialInfoId")), null, "", true);
                    if (materialBrokenItemList != null && materialBrokenItemList.Count() > 0)
                        queryExpression = materialBrokenItemList.Select(p => p.MaterialBrokenId).Distinct().ToFormatStr();
                    else return false;
                }
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }

        
    }
}
