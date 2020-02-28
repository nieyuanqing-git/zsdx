using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewShareFundApplyBLL : BLLBase<ViewShareFundApply>, IViewShareFundApplyBLL
    {
        private IUserBLL __userBLL;
        private IShareFundApplyEquipmentBLL __shareFundApplyEquipmentBLL;

        public ViewShareFundApplyBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __shareFundApplyEquipmentBLL = BLLFactory.CreateShareFundApplyEquipmentBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewShareFundApply> viewShareFundApplyList, bool isSupressLazyLoad = false)
        {            
            if (viewShareFundApplyList == null || viewShareFundApplyList.Count == 0) return;
            var shareFundApplyPrivilige = userId.HasValue ? PriviligeFactory.GetShareFundApplyPrivilige(userId.Value) : null;
            foreach (var viewShareFundApply in viewShareFundApplyList)
            {
                viewShareFundApply.IsSupressLazyLoad = false;
                viewShareFundApply.InitOperate();
                OperateDecorate(userId, viewShareFundApply, shareFundApplyPrivilige);
                viewShareFundApply.BuildOperate();
                viewShareFundApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewShareFundApply viewShareFundApply, ShareFundApplyPrivilige shareFundApplyPrivilige)
        {
            if (viewShareFundApply == null) throw new ArgumentException("共享基金为空");
            if (shareFundApplyPrivilige == null)
            {
                viewShareFundApply.ModifyOperate = "";
                viewShareFundApply.DeleteOperate = "";
                viewShareFundApply.AuditOperate = "";
                    
                return;
            }
            if (!shareFundApplyPrivilige.IsEnableEdit) viewShareFundApply.ModifyOperate = "";
            if (!shareFundApplyPrivilige.IsEnableDelete) viewShareFundApply.DeleteOperate = "";
            if (!shareFundApplyPrivilige.IsEnableAudit) viewShareFundApply.AuditOperate = "";
            if (viewShareFundApply.StatusEnum != ShareFundApplyStatus.WaitingAudit && !shareFundApplyPrivilige.IsEnableAudit)
            {
                viewShareFundApply.ModifyOperate = "";
                viewShareFundApply.DeleteOperate = "";
            }
        }
        public IList<ViewShareFundApply> GetViewShareFundApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if(!userId.HasValue) return null;
            var shareFundApplyPrivilige = PriviligeFactory.GetShareFundApplyPrivilige(userId.Value);
            if (shareFundApplyPrivilige == null || !shareFundApplyPrivilige.IsEnableAudit)
                expression += (expression == "" ? "" : "*") + string.Format("ApplyUserId=\"{0}\"", userId.Value);
            var viewShareFundApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewShareFundApplyList, isSupressLazyLoad);
            return viewShareFundApplyList;
        }
        public IList<ViewShareFundApply> GetViewShareFundApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            var shareFundApplyPrivilige = PriviligeFactory.GetShareFundApplyPrivilige(userId.Value);
            if (shareFundApplyPrivilige == null || !shareFundApplyPrivilige.IsEnableAudit)
                expression += (expression == "" ? "" : "*") + string.Format("ApplyUserId=\"{0}\"", userId.Value);
            var viewShareFundApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewShareFundApplyList, isSupressLazyLoad);
            return viewShareFundApplyList;
        }
        public IList<ViewShareFundApply> GetViewShareFundApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            var shareFundApplyPrivilige = PriviligeFactory.GetShareFundApplyPrivilige(userId.Value);
            if (shareFundApplyPrivilige == null || !shareFundApplyPrivilige.IsEnableAudit)
                dataGridSettings.AppendAndQueryExpression(string.Format("ApplyUserId=\"{0}\"", userId.Value));
            var viewShareFundApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewShareFundApplyList, isSupressLazyLoad);
            return viewShareFundApplyList;
        }
        public GridData<ViewShareFundApply> GetGridViewShareFundApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            var shareFundApplyPrivilige = PriviligeFactory.GetShareFundApplyPrivilige(userId.Value);
            if (shareFundApplyPrivilige == null || !shareFundApplyPrivilige.IsEnableAudit)
                expression += (expression == "" ? "" : "*") + string.Format("ApplyUserId=\"{0}\"", userId.Value);
            var viewShareFundApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewShareFundApplyList == null ? null : viewShareFundApplyList.Data, isSupressLazyLoad);
            return viewShareFundApplyList;
        }
        public GridData<ViewShareFundApply> GetGridViewShareFundApplyListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            var shareFundApplyPrivilige = PriviligeFactory.GetShareFundApplyPrivilige(userId.Value);
            if (shareFundApplyPrivilige == null || !shareFundApplyPrivilige.IsEnableAudit)
                dataGridSettings.AppendAndQueryExpression(string.Format("ApplyUserId=\"{0}\"", userId.Value));
            var viewShareFundApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewShareFundApplyList == null ? null : viewShareFundApplyList.Data, isSupressLazyLoad);
            return viewShareFundApplyList;
        }
    }
}
