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
    public class ViewMaterialRecipientBLL : BLLBase<ViewMaterialRecipient>, IViewMaterialRecipientBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialRecipientItemBLL __materialRecipientItemBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;

        public ViewMaterialRecipientBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialRecipientItemBLL = BLLFactory.CreateMaterialRecipientItemBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialRecipient> viewMaterialRecipientList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialRecipientList == null || viewMaterialRecipientList.Count == 0) return;
            foreach (var viewMaterialRecipient in viewMaterialRecipientList)
            {
                viewMaterialRecipient.IsSupressLazyLoad = false;
                viewMaterialRecipient.InitOperate();
                OperateDecorate(userId, viewMaterialRecipient);
                viewMaterialRecipient.BuildOperate();
                viewMaterialRecipient.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialRecipient viewMaterialRecipient)
        {
            if (viewMaterialRecipient == null) throw new ArgumentException("耗材领用单为空");
            var materialRecipientPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialRecipientPrivilige(userId.Value, viewMaterialRecipient.Id) : null;
            if (materialRecipientPrivilige == null)
            {
                viewMaterialRecipient.ModifyOperate = "";
                viewMaterialRecipient.DeleteOperate = "";
                viewMaterialRecipient.AuditOperate = "";
                viewMaterialRecipient.RecipientOutputOperate = "";
                viewMaterialRecipient.NoteOperate = "";
                return;
            }
            if (!materialRecipientPrivilige.IsEnableEdit) viewMaterialRecipient.ModifyOperate = "";
            if (!materialRecipientPrivilige.IsEnableDelete) viewMaterialRecipient.DeleteOperate = "";
            if (!materialRecipientPrivilige.IsEnableRecipientOutput) viewMaterialRecipient.RecipientOutputOperate = "";
            if (!materialRecipientPrivilige.IsEnableAudit) viewMaterialRecipient.AuditOperate = "";
            if (!materialRecipientPrivilige.IsEnableNote) viewMaterialRecipient.NoteOperate = "";
            if (viewMaterialRecipient.StatusEnum != MaterialRecipientStatus.WaittingAudit) viewMaterialRecipient.ModifyOperate = "";
            if (viewMaterialRecipient.StatusEnum != MaterialRecipientStatus.WaittingAudit 
                && viewMaterialRecipient.StatusEnum != MaterialRecipientStatus.AuditPass 
                && viewMaterialRecipient.StatusEnum != MaterialRecipientStatus.AuditReject)
            {
                viewMaterialRecipient.ModifyOperate = "";
                viewMaterialRecipient.DeleteOperate = "";
                viewMaterialRecipient.AuditOperate = "";
            }
            if (!materialRecipientPrivilige.IsEnableAudit && (viewMaterialRecipient.StatusEnum == MaterialRecipientStatus.AuditPass || viewMaterialRecipient.StatusEnum == MaterialRecipientStatus.AuditReject))
            {
                viewMaterialRecipient.ModifyOperate = "";
                viewMaterialRecipient.DeleteOperate = "";
            }
            if (viewMaterialRecipient.StatusEnum != MaterialRecipientStatus.AuditPass)
            {
                viewMaterialRecipient.RecipientOutputOperate = "";
            }

        }
        public IList<ViewMaterialRecipient> GetViewMaterialRecipientListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialRecipientList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialRecipientList, isSupressLazyLoad);
            return viewMaterialRecipientList;
        }
        public IList<ViewMaterialRecipient> GetViewMaterialRecipientListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialRecipientList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialRecipientList, isSupressLazyLoad);
            return viewMaterialRecipientList;
        }
        public IList<ViewMaterialRecipient> GetViewMaterialRecipientListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialRecipientList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialRecipientList, isSupressLazyLoad);
            return viewMaterialRecipientList;
        }
        public GridData<ViewMaterialRecipient> GetGridViewMaterialRecipientListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialRecipientList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialRecipientList == null ? null : viewMaterialRecipientList.Data, isSupressLazyLoad);
            return viewMaterialRecipientList;
        }
        public GridData<ViewMaterialRecipient> GetGridViewMaterialRecipientListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialRecipientList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialRecipientList == null ? null : viewMaterialRecipientList.Data, isSupressLazyLoad);
            return viewMaterialRecipientList;
        }
      
        public IList<ViewMaterialRecipient> GetManageViewMaterialRecipientList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialRecipient(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        public int GetManageViewMaterialRecipientCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialRecipient(userId, ref dataGridSettings)) return 0;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }

        private bool IsManageMaterialRecipient(Guid? userId, ref DataGridSettings dataGridSettings)
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
                    var materialRecipientItemList = __materialRecipientItemBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", materialInfoIds.ToFormatStr("MaterialInfoId")), null, "", true);
                    if (materialRecipientItemList != null && materialRecipientItemList.Count() > 0)
                        queryExpression = materialRecipientItemList.Select(p => p.MaterialRecipientId).Distinct().ToFormatStr();
                    else return false;
                }
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }

        
    }
}
