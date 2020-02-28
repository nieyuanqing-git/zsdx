using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewActivityApplyBLL : BLLBase<ViewActivityApply>, IViewActivityApplyBLL
    {
       private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewActivityApplyBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewActivityApply> viewActivityApplyList, bool isSupressLazyLoad)
        {
            if (!userId.HasValue || viewActivityApplyList == null || viewActivityApplyList.Count == 0) return;
            var equipmentSemesterCostPrivilige = PriviligeFactory.GetActivityApplyPrivilige(userId.Value);
            foreach (var viewActivityApply in viewActivityApplyList)
            {
                viewActivityApply.IsSupressLazyLoad = false;
                viewActivityApply.InitOperate();
                OperateDecorate(userId, viewActivityApply, equipmentSemesterCostPrivilige);
                viewActivityApply.BuildOperate();
                viewActivityApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewActivityApply viewActivityApply, ActivityApplyPrivilige equipmentSemesterCostPrivilige)
        {
            if (viewActivityApply == null) throw new ArgumentException("业务活动申请表信息为空");
            if (equipmentSemesterCostPrivilige == null)
            {
                viewActivityApply.ViewOperate = "";
                viewActivityApply.ModifyOperate = "";
                viewActivityApply.GroupAdminAuditOperate = "";
                viewActivityApply.DirectorAuditOperate = "";
                viewActivityApply.OperatorAuditOperate = "";
                viewActivityApply.DeleteOperate = "";
                viewActivityApply.ExportDocOperate = "";
                return;
            }
            if (!equipmentSemesterCostPrivilige.IsEnableView) viewActivityApply.ViewOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableEdit) viewActivityApply.ModifyOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableGroupAdminAudit) viewActivityApply.GroupAdminAuditOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableDirectorAudit) viewActivityApply.DirectorAuditOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableOperatorAudit) viewActivityApply.OperatorAuditOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableDelete) viewActivityApply.DeleteOperate = "";
            if (!equipmentSemesterCostPrivilige.IsEnableExportDoc) viewActivityApply.ExportDocOperate = "";
            switch(viewActivityApply.StatusEnum)
            {
                case ActivityApplyStatus.WaittingAudit:
                case ActivityApplyStatus.GroupAdminAuditReject:
                    viewActivityApply.DirectorAuditOperate = "";
                    viewActivityApply.OperatorAuditOperate = "";
                    break;
                case ActivityApplyStatus.GroupAdminAuditPass:
                    viewActivityApply.OperatorAuditOperate = "";
                    break;
                case ActivityApplyStatus.DirectorAuditReject:
                    viewActivityApply.GroupAdminAuditOperate = "";
                    viewActivityApply.OperatorAuditOperate = "";
                    break;
                case ActivityApplyStatus.DirectorAuditPass:
                    viewActivityApply.GroupAdminAuditOperate = "";
                    break;
                case ActivityApplyStatus.OperatorAuditPass:
                case ActivityApplyStatus.OperatorAuditReject:
                    viewActivityApply.GroupAdminAuditOperate = "";
                    viewActivityApply.DirectorAuditOperate = "";
                    break;
                           
            }
            if (viewActivityApply.StatusEnum != ActivityApplyStatus.WaittingAudit)
            {
                viewActivityApply.ModifyOperate = "";
                viewActivityApply.DeleteOperate = "";
            }
            if (viewActivityApply.GroupAdminAuditOperate != "")
            {
                if (viewActivityApply.EquipmentGroupAdminList == null
                    || viewActivityApply.EquipmentGroupAdminList.Count() == 0
                    || viewActivityApply.EquipmentGroupAdminList.Where(p => p.AdminId == userId.Value).Count() == 0)
                {
                    viewActivityApply.GroupAdminAuditOperate = "";
                }
            }
        }

        public IList<ViewActivityApply> GetViewActivityApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewActivityApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewActivityApplyList, isSupressLazyLoad);
            return viewActivityApplyList;
        }
        public IList<ViewActivityApply> GetViewActivityApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewActivityApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewActivityApplyList, isSupressLazyLoad);
            return viewActivityApplyList;
        }
        public IList<ViewActivityApply> GetViewActivityApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewActivityApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewActivityApplyList, isSupressLazyLoad);
            return viewActivityApplyList;
        }
        public GridData<ViewActivityApply> GetGridViewActivityApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewActivityApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewActivityApplyList == null ? null : viewActivityApplyList.Data, isSupressLazyLoad);
            return viewActivityApplyList;
        }
        public GridData<ViewActivityApply> GetGridViewActivityApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewActivityApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewActivityApplyList == null ? null : viewActivityApplyList.Data, isSupressLazyLoad);
            return viewActivityApplyList;
        }
    }
}
