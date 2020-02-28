using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentRepairFundsApplyBLL : BLLBase<ViewEquipmentRepairFundsApply>, IViewEquipmentRepairFundsApplyBLL
    {
        public bool JudgeIsEnableDelete(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableDelete(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableDelete(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableDelete)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != EquipmentRepairFundsApplyStatus.Applied || viewEquipmentRepairFundsApply.StatusEnum != EquipmentRepairFundsApplyStatus.Canceled)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行删除操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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
        public bool JudgeIsEnableEdit(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableEdit(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEdit(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableEdit)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != EquipmentRepairFundsApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行编辑操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableApply(equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableApply(object equipmentApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentApplyPrivilige != null ? equipmentApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableApply)
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
        public bool JudgeIsEnableCancel(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableCancel(userId.Value,viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancel(Guid userId ,ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableCancel)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != EquipmentRepairFundsApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行取消操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "不是申请人本人,不能进行取消操作";
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
        public bool JudgeIsEnableFinishRepairRegister(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableFinishRepairRegister(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableFinishRepairRegister(Guid userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableFinishRepairRegister)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.IsFinishRepair)
                {
                    errorMessage = "已经完成维修登记操作";
                    return false;
                }
                if ((viewEquipmentRepairFundsApply.IsBigAmount && viewEquipmentRepairFundsApply.StatusEnum!= EquipmentRepairFundsApplyStatus.SharEAuditPassed) ||
                    (!viewEquipmentRepairFundsApply.IsBigAmount && viewEquipmentRepairFundsApply.StatusEnum!= EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed))
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行完成维修登记操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "不是申请人本人,不能进行完成维修登记操作";
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


        public bool JudgeIsEnableFundsProvide(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableFundsProvide(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableFundsProvide(Guid userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableFundsProvide)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.IsFundsProvide)
                {
                    errorMessage = "已经完成基金已发放操作";
                    return false;
                }
                if (!viewEquipmentRepairFundsApply.IsFinishRepair)
                {
                    errorMessage = "未来完成维修，不能进行基金已发放操作";
                    return false;
                }
                if (userId != viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "不是申请人本人,不能进行基金已发放操作";
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

        public bool JudgeIsEnableLabRoomAuditPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableLabRoomAuditPass(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableLabRoomAuditPass(Guid userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableCollegeAuditAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.Applied &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.LabRoomAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId == viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "自己不能审核通过自己的申请单";
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
        public bool JudgeIsEnableLabRoomAuditNoPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableLabRoomAuditNoPass(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableLabRoomAuditNoPass(Guid userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableCollegeAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.Applied &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.LabRoomAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核不通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId == viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "自己不能审核不通过自己的申请单";
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


        public bool JudgeIsEnableCollegeAuditPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableCollegeAuditPass(userId.Value,viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCollegeAuditPass(Guid userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableCollegeAuditAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.LabRoomAuditPassed && 
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.CollegeAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId == viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "自己不能审核通过自己的申请单";
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
        public bool JudgeIsEnableCollegeAuditNoPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableCollegeAuditNoPass(userId.Value,viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCollegeAuditNoPass(Guid userId,ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableCollegeAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.LabRoomAuditPassed && 
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.CollegeAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核不通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId == viewEquipmentRepairFundsApply.ApplicantId)
                {
                    errorMessage = "自己不能审核不通过自己的申请单";
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

        public bool JudgeIsEnableEquipmentDeptAuditPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableEquipmentDeptAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEquipmentDeptAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableEquipmentDeptAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.CollegeAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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
        public bool JudgeIsEnableEquipmentDeptAuditNoPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableEquipmentDeptAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEquipmentDeptAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableEquipmentDeptAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.CollegeAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核不通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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

        public bool JudgeIsEnableShareEAuditPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableShareEAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableShareEAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableShareEDirectorAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.SharEAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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
        public bool JudgeIsEnableShareEAuditNoPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableShareEAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableShareEAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableShareEDirectorAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.SharEAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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

        public bool JudgeIsEnableSchoolAuditPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableSchoolAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableSchoolAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableSchoolAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.SchoolAuditNoPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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
        public bool JudgeIsEnableSchoolAuditNoPass(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableSchoolAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableSchoolAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, object equipmentRepairFundsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var equipmentRepairFundsApplyPriviligeTemp = equipmentRepairFundsApplyPrivilige != null ? equipmentRepairFundsApplyPrivilige as EquipmentRepairFundsApplyPrivilige : null;
                if (equipmentRepairFundsApplyPriviligeTemp == null || !equipmentRepairFundsApplyPriviligeTemp.IsEnableSchoolAuditNoPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed &&
                    viewEquipmentRepairFundsApply.StatusEnum != Model.Enum.EquipmentRepairFundsApplyStatus.SchoolAuditPassed)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行审核通过操作", viewEquipmentRepairFundsApply.StatusEnum.ToCnName());
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

        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentRepairFundsApply> viewEquipmentRepairFundsApplyList, bool isSupressLazyLoad)
        {
            if (viewEquipmentRepairFundsApplyList == null || viewEquipmentRepairFundsApplyList.Count == 0) return;
            EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige = PriviligeFactory.GetEquipmentRepairFundsApplyPrivilige(userId.Value);
            foreach (var viewEquipmentApply in viewEquipmentRepairFundsApplyList)
            {

                viewEquipmentApply.IsSupressLazyLoad = false;
                viewEquipmentApply.InitOperate();
                OperateDecorate(userId, viewEquipmentApply, equipmentRepairFundsApplyPrivilige);
                viewEquipmentApply.BuildOperate();
                viewEquipmentApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, EquipmentRepairFundsApplyPrivilige equipmentRepairFundsApplyPrivilige)
        {
            string errorMessage = "";
            if (viewEquipmentRepairFundsApply == null) throw new ArgumentException("设备信息为空");
            if (equipmentRepairFundsApplyPrivilige == null)
            {
                viewEquipmentRepairFundsApply.LabRoomAuditPassOperate = "";
                viewEquipmentRepairFundsApply.LabRoomAuditNoPassOperate = "";
                viewEquipmentRepairFundsApply.CollegeAuditPassOperate = "";
                viewEquipmentRepairFundsApply.EquipmentDeptAuditPassOperate = "";
                viewEquipmentRepairFundsApply.ShareEAuditPassOperate = "";
                viewEquipmentRepairFundsApply.CollegeAuditNoPassOperate = "";
                viewEquipmentRepairFundsApply.EquipmentDeptAuditNoPassOperate = "";
                viewEquipmentRepairFundsApply.ShareEAuditNoPassOperate = "";
                viewEquipmentRepairFundsApply.SchoolAuditPassOperate = "";
                viewEquipmentRepairFundsApply.SchoolAuditNoPassOperate = "";
                viewEquipmentRepairFundsApply.PrintOperate = "";
                viewEquipmentRepairFundsApply.ViewOperate = "";
                viewEquipmentRepairFundsApply.EditOperate = "";
                viewEquipmentRepairFundsApply.DeleteOperate = "";
                viewEquipmentRepairFundsApply.FundsProvideOperate = "";
                viewEquipmentRepairFundsApply.FinishRepairRegisterOperate = "";
                return;
            }
            if (!JudgeIsEnableDelete(userId, viewEquipmentRepairFundsApply, out errorMessage))
                viewEquipmentRepairFundsApply.DeleteOperate = "";
            if (!JudgeIsEnableEdit(userId, viewEquipmentRepairFundsApply, out errorMessage))
                viewEquipmentRepairFundsApply.EditOperate = "";

            if (!equipmentRepairFundsApplyPrivilige.IsEnablePrint)
                viewEquipmentRepairFundsApply.PrintOperate = "";

            if (!JudgeIsEnableLabRoomAuditNoPass(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.LabRoomAuditNoPassOperate = "";

            if (!JudgeIsEnableLabRoomAuditPass(userId.Value, viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.LabRoomAuditPassOperate = "";

            if (!JudgeIsEnableCollegeAuditNoPass(userId.Value,viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.CollegeAuditNoPassOperate = "";

            if (!JudgeIsEnableCollegeAuditPass(userId.Value,viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.CollegeAuditPassOperate = "";

            if (!JudgeIsEnableEquipmentDeptAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.EquipmentDeptAuditNoPassOperate = "";

            if (!JudgeIsEnableEquipmentDeptAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.EquipmentDeptAuditPassOperate = "";

            if (!JudgeIsEnableShareEAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.ShareEAuditNoPassOperate = "";


            if (!JudgeIsEnableShareEAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.ShareEAuditPassOperate = "";

            if (!JudgeIsEnableSchoolAuditNoPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.SchoolAuditNoPassOperate = "";


            if (!JudgeIsEnableSchoolAuditPass(viewEquipmentRepairFundsApply, equipmentRepairFundsApplyPrivilige, out errorMessage))
                viewEquipmentRepairFundsApply.SchoolAuditPassOperate = "";

            if (!equipmentRepairFundsApplyPrivilige.IsEnableView)
                viewEquipmentRepairFundsApply.ViewOperate = "";

            if (!JudgeIsEnableCancel(userId,viewEquipmentRepairFundsApply,out errorMessage))
                viewEquipmentRepairFundsApply.CancelOperate = "";

            if (!JudgeIsEnableFinishRepairRegister(userId, viewEquipmentRepairFundsApply, out errorMessage))
                viewEquipmentRepairFundsApply.FinishRepairRegisterOperate = "";

            if (!JudgeIsEnableFundsProvide(userId, viewEquipmentRepairFundsApply, out errorMessage))
                viewEquipmentRepairFundsApply.FundsProvideOperate = "";
        }
        public IList<ViewEquipmentRepairFundsApply> GetViewEquipmentRepairFundsApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentRepairFundsApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentRepairFundsApplyList, isSupressLazyLoad);
            return viewEquipmentRepairFundsApplyList;
        }

        public IList<ViewEquipmentRepairFundsApply> GetViewEquipmentRepairFundsApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentRepairFundsApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentRepairFundsApplyList, isSupressLazyLoad);
            return viewEquipmentRepairFundsApplyList;
        }

        public IList<ViewEquipmentRepairFundsApply> GetViewEquipmentRepairFundsApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentRepairFundsApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentRepairFundsApplyList, isSupressLazyLoad);
            return viewEquipmentRepairFundsApplyList;
        }

        public Logic.Model.GridData<ViewEquipmentRepairFundsApply> GetGridViewEquipmentRepairFundsApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            expression = dataGridSettings.QueryExpression;
            var viewEquipmentRepairFundsApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentRepairFundsApplyList == null ? null : viewEquipmentRepairFundsApplyList.Data, isSupressLazyLoad);
            return viewEquipmentRepairFundsApplyList;
        }

        public Logic.Model.GridData<ViewEquipmentRepairFundsApply> GetGridViewEquipmentRepairFundsApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEquipmentApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEquipmentApplyList == null ? null : viewEquipmentApplyList.Data, isSupressLazyLoad);
            return viewEquipmentApplyList;
        }
    }
}
