using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentApplyBLL : BLLBase<ViewEquipmentApply>, IViewEquipmentApplyBLL
    {
        public bool JudgeIsEnableDelete(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableDelete(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableDelete(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableDelete)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != EquipmentApplyStatus.Applied || viewEquipmentApply.StatusEnum != EquipmentApplyStatus.Canceled)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行删除操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableEdit(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableEdit(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEdit(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableEdit)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != EquipmentApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行编辑操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableApply(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableApply(equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableApply(object equipmentApplyPrivilige,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableApply)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableCancel(Guid? userId,ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableCancel(userId.Value,viewEquipmentApply,equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancel(Guid userId,ViewEquipmentApply viewEquipmentApply,object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableCancel)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != EquipmentApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行取消操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewEquipmentApply.ApplicantId)
                {
                    errorMessage = "只有申请人才能取消自己的申请单";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableLabRoomDirectorAuditPass(Guid? userId,ViewEquipmentApply viewEquipmentApply ,out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableLabRoomDirectorAuditPass(userId.Value, viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableLabRoomDirectorAuditPass(Guid userId,ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableLabRoomDirectorAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.Applied &&
                    viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.LabRoomDirectotAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
                if (viewEquipmentApply.ApplicantId == userId)
                {
                    errorMessage = "不能审核通过自己的申请单";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableLabRoomDirectorAuditNoPass(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableLabRoomDirectorAuditNoPass(userId.Value,viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableLabRoomDirectorAuditNoPass(Guid userId, ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableLabRoomDirectorAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.Applied &&
                    viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.LabRoomDirectotAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核不通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
                if (viewEquipmentApply.ApplicantId == userId)
                {
                    errorMessage = "不能审核不通过自己的申请单";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public bool JudgeIsEnableShoolDirectorAuditPass(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableShoolDirectorAuditPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableShoolDirectorAuditPass(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableSchoolDirectorAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.LabRoomDirectotAuditPassed &&
                    viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SchoolDirectorAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableSchoolDirectorAuditNoPass(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableSchoolDirectorAuditNoPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableSchoolDirectorAuditNoPass(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableSchoolDirectorAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.LabRoomDirectotAuditPassed && 
                   viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SchoolDirectorAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核不通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public bool JudgeIsEnableShareEDirectorAuditPass(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableShareEDirectorAuditPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableShareEDirectorAuditPass(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableShareEDirectorAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SchoolDirectorAuditPassed &&
                    viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SharEDirectorAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableShareEDirectorAuditNoPass(Guid? userId, ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentApplyPrivilige equipmentApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableShareEDirectorAuditNoPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableShareEDirectorAuditNoPass(ViewEquipmentApply viewEquipmentApply, object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentApplyPrivilige : null;
                if (equipmentPriviligeTemp == null || !equipmentPriviligeTemp.IsEnableShareEDirectorAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SchoolDirectorAuditPassed &&
                    viewEquipmentApply.StatusEnum != Model.Enum.EquipmentApplyStatus.SharEDirectorAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentApply.StatusEnum.ToCnName());
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentApply> viewEquipmentApplyList, bool isSupressLazyLoad)
        {
            if (viewEquipmentApplyList == null || viewEquipmentApplyList.Count == 0) return;
            EquipmentApplyPrivilige equipmentApplyPrivilige = PriviligeFactory.GetEquipmentApplyPrivilige(userId.Value);
            foreach (var viewEquipmentApply in viewEquipmentApplyList)
            {

                viewEquipmentApply.IsSupressLazyLoad = false;
                viewEquipmentApply.InitOperate();
                OperateDecorate(userId, viewEquipmentApply, equipmentApplyPrivilige);
                viewEquipmentApply.BuildOperate();
                viewEquipmentApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentApply viewEquipmentApply, EquipmentApplyPrivilige equipmentApplyPrivilige)
        {
            string errorMessage = "";
            if (viewEquipmentApply == null) throw new ArgumentException("设备信息为空");
            if (equipmentApplyPrivilige == null)
            {
                viewEquipmentApply.LabRoomDirectorAuditPassOperate = "";
                viewEquipmentApply.SchoolDirectorAuditPassOperate = "";
                viewEquipmentApply.ShareEDirectorAuditPassOperate = "";
                viewEquipmentApply.LabRoomDirectorAuditNoPassOperate = "";
                viewEquipmentApply.SchoolDirectorAuditNoPassOperate = "";
                viewEquipmentApply.ShareEDirectorAuditNoPassOperate = "";
                viewEquipmentApply.ExpertOperate = "";
                viewEquipmentApply.ViewOperate = "";
                viewEquipmentApply.EditOperate = "";
                viewEquipmentApply.DeleteOperate = "";
                return;
            }
            if (!JudgeIsEnableDelete(userId, viewEquipmentApply, out errorMessage))
                viewEquipmentApply.DeleteOperate = "";
            if (!JudgeIsEnableEdit(userId, viewEquipmentApply, out errorMessage))
                viewEquipmentApply.EditOperate = "";

            if (!equipmentApplyPrivilige.IsEnableExpert)
                viewEquipmentApply.ExpertOperate = "";

            if (!JudgeIsEnableLabRoomDirectorAuditNoPass(userId.Value,viewEquipmentApply,equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.LabRoomDirectorAuditNoPassOperate = "";

            if (!JudgeIsEnableLabRoomDirectorAuditPass(userId.Value,viewEquipmentApply, equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.LabRoomDirectorAuditPassOperate = "";

            if (!JudgeIsEnableSchoolDirectorAuditNoPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.SchoolDirectorAuditNoPassOperate = "";

            if (!JudgeIsEnableShoolDirectorAuditPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.SchoolDirectorAuditPassOperate = "";

            if (!JudgeIsEnableShareEDirectorAuditNoPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.ShareEDirectorAuditNoPassOperate = "";

            if (!JudgeIsEnableShareEDirectorAuditPass(viewEquipmentApply, equipmentApplyPrivilige, out errorMessage))
                viewEquipmentApply.ShareEDirectorAuditPassOperate = "";

            if (!equipmentApplyPrivilige.IsEnableView)
                viewEquipmentApply.ViewOperate = "";

            if (!JudgeIsEnableCancel(userId.Value,viewEquipmentApply,out errorMessage))
                viewEquipmentApply.CancelOperate = "";

        }
        public IList<ViewEquipmentApply> GetViewEquipmentApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }

        public IList<ViewEquipmentApply> GetViewEquipmentApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }

        public IList<ViewEquipmentApply> GetViewEquipmentApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }

        public Logic.Model.GridData<ViewEquipmentApply> GetGridViewEquipmentApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList == null ? null : viewEquipmentApplyList.Data, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }

        public Logic.Model.GridData<ViewEquipmentApply> GetGridViewEquipmentApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList == null ? null : viewEquipmentApplyList.Data, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }
    }
}
