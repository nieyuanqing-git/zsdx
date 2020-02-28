using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.DAL;
using System.Data;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewSampleApplyBLL : BLLBase<ViewSampleApply>, IViewSampleApplyBLL
    {
        private IUserEquipmentPriviligeBLL __userEquipmentPriviligeBLL;
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private IUserBLL __userBLL;
        private static DictCodeType __whichSampleStausDisableEditDictCodeType;
        private static string __whichSampleStausDisableEdit;
        private static DictCodeType __whenCanModifySampleAmountDictCodeType;
        private static string __whenCanModifySampleAmount=((int)SampleApplyStatus.FinishTest).ToString();
        public ViewSampleApplyBLL()
        {
            __userEquipmentPriviligeBLL = BLLFactory.CreateUserEquipmentPriviligeBLL();
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            if (__whichSampleStausDisableEditDictCodeType == null)
                __whichSampleStausDisableEditDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=WhichSampleStausDisableEdit");
            if (__whichSampleStausDisableEditDictCodeType != null)
                __whichSampleStausDisableEdit = __whichSampleStausDisableEditDictCodeType.Value.Trim();
            if(__whenCanModifySampleAmountDictCodeType==null)
                __whenCanModifySampleAmountDictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=WhenCanModifySampleAmount");
            if (__whenCanModifySampleAmountDictCodeType != null && !string.IsNullOrWhiteSpace(__whenCanModifySampleAmountDictCodeType.Value))
                __whenCanModifySampleAmount = __whenCanModifySampleAmountDictCodeType.Value.Trim();
        }
        //public bool JudgeIsEnableModifyAmount(Guid userId, SampleApplyStatus sampleApplyStatus,object samplePriviligeObj, out string errorMessage)
        //{
        //    errorMessage = "";
        //    try
        //    {
        //        if (!__whenCanModifySampleAmount.Replace("，", ",").Split(',').Contains(((int)sampleApplyStatus).ToString()))
        //        {
        //            errorMessage = string.Format("出错,当前状态【{0}】,不能修改样品数量",sampleApplyStatus.ToCnName());
        //            return false;
        //        }
        //        SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
        //        if (samplePrivilige == null || !samplePrivilige.IsEnableEdit)
        //        {
        //            errorMessage = "无操作权限";
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message;
        //        return false;
        //    }
        //    return true;
        //}
        public bool JudgeIsEnableOperate(Guid[] ids, Guid operatorId, bool hook, SampleOperate sampleOperate, out string errorMessage)
        {
            errorMessage = "";
            ISampleApplyBLL sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
            IList<SampleApply> models = sampleApplyBLL.GetEntitiesByExpression(ids.ToFormatStr());
            return JudgeIsEnableOperate(operatorId,models,hook, sampleOperate, out errorMessage);
        }
        public bool JudgeIsEnableOperate(Guid operatorId, IList<SampleApply> models, bool hook, SampleOperate sampleOperate, out string errorMessage)
        {
            errorMessage = "";
            IList<Guid> ids = new List<Guid>();
            foreach (var model in models)
            {
                var userEquipmentPrivilige = __userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserItemId(operatorId, model.SampleItemId);
                errorMessage = string.Format("出错,项目【{0}】无操作权限", model.SampleItem.Name);
                switch (sampleOperate)
                {
                    case SampleOperate.Apply:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableApply) return false;
                        break;
                    case SampleOperate.Accept:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableAccept) return false;
                        break;
                    case SampleOperate.Analysis:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableAnalysis) return false;
                        break;
                    case SampleOperate.AuditDoublt:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableAuditDoubts) return false;
                        break;

                    case SampleOperate.Cancel:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableCancel) return false;
                        break;
                    case SampleOperate.Charge:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableCharge) return false;
                        break;
                    case SampleOperate.Copy:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableCopy) return false;
                        break;
                    case SampleOperate.Doubts:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableDoubts) return false;
                        break;
                    case SampleOperate.DownLoad:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableDownLoad) return false;
                        break;
                    case SampleOperate.Edit:
                        if (hook) return true;
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableEdit) return false;
                        break;
                    case SampleOperate.Pass:
                    case SampleOperate.Refuse:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableAudit) return false;
                        break;
                    case SampleOperate.RefuseAccept:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableRefuse) return false;
                        break;
                    case SampleOperate.Register:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableRegister) return false;
                        break;
                    case SampleOperate.Release:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableRelease) return false;
                        break;
                    case SampleOperate.Test:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableTest) return false;
                        break;
                    case SampleOperate.ReportConfirm:
                        if (userEquipmentPrivilige == null || !userEquipmentPrivilige.IsEnableReportConfirm) return false;
                        break;
                }
            }
            errorMessage = "";
            return true;
        }
        public bool JudgeIsEnableTutorAudit(Guid userId, Guid auditTutorId,Guid? sampleApplyAuditTutorId,Guid applicantTutorId,string sampleNo, int rowNo, bool isNeedTutorAudit, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            try
            {
                errorMessage = "";
                IUserBLL userBLL = BLLFactory.CreateUserBLL();
                var tutor = userBLL.GetEntityById(auditTutorId);
                if (tutor == null)
                {
                    errorMessage = "出错,找不到导师信息";
                    return false;
                }
                if (tutor.TutorId.HasValue)
                {
                    errorMessage = string.Format("出错,用户【{0}】不是导师", tutor.UserName);
                    return false;
                }
                if (!isNeedTutorAudit)
                {
                    errorMessage = string.Format("出错,编号为【{0}】的送样申请单无需导师审核", sampleNo + "~" + rowNo);
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit)
                {
                    errorMessage = string.Format("出错,编号为【{0}】的送样申请单,当前状态【{1}】,不能进行导师审核操作", sampleNo + "~" + rowNo, sampleApplyStatus.ToCnName());
                    return false;
                }
                if (sampleApplyAuditTutorId.HasValue)
                {
                    errorMessage = string.Format("出错,编号为【{0}】的送样申请单,已经审核过,不能重复操作", sampleNo + "~" + rowNo);
                    return false;
                }
                if (applicantTutorId != auditTutorId)
                {
                    errorMessage = string.Format("出错,您不能审核编号为【{0}】的送样申请单,您不是该申请人的导师", sampleNo + "~" + rowNo);
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
        public bool JudgeIsEnableApply(Guid userId, bool hook, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!__userBLL.CheckUserStatus(userId, out errorMessage))
                {
                    errorMessage = string.Format("当前用户状态为:{0},不允许添加送样申请单.",  errorMessage);
                    return false;
                }
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableApply)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableEdit(Guid userId,Guid applicantId,Guid creatorId,SampleApplyStatus sampleApplyStatus,bool hook, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (applicantId != userId && userId != creatorId)
                {
                    errorMessage = "非本人,不允许操作";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit || (!string.IsNullOrWhiteSpace(__whichSampleStausDisableEdit) && __whichSampleStausDisableEdit.Trim().Split(',').Contains(((int)sampleApplyStatus).ToString())))
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许编辑操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                if(!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableEdit)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableView(Guid userId, bool hook, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableView)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableReportConfirm(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (sampleApplyStatus != SampleApplyStatus.Released && sampleApplyStatus != SampleApplyStatus.Obtained)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许确认报告操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableReportConfirm)
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
        public bool JudgeIsEnableAccept(Guid userId,bool isNeedConfirmAcceptSample,bool isNeedTutorAudit,AuditStatus tutorAuditStatus, SampleApplyStatus sampleApplyStatus,object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!isNeedConfirmAcceptSample)
                {
                    errorMessage = "无需确认收样";
                    return false;
                }
                if (isNeedTutorAudit && tutorAuditStatus != (int)AuditStatus.Passed)
                {
                    errorMessage = "该申请单需要导师审核通过才能进行确认收样操作";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许确认收样操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableAccept)
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
        public bool JudgeIsEnableRefuseAccept(Guid userId, bool isNeedConfirmAcceptSample,bool isNeedTutorAudit,AuditStatus tutorAuditStatus, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!isNeedConfirmAcceptSample)
                {
                    errorMessage = "无需拒绝收样";
                    return false;
                }
                if (isNeedTutorAudit && tutorAuditStatus != (int)AuditStatus.Passed)
                {
                    errorMessage = "该申请单需要导师审核通过才能进行拒绝收样操作";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许拒绝收样操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableRefuse)
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
        public bool JudgeIsEnableAnalysis(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (sampleApplyStatus != SampleApplyStatus.Registed)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许分析操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableAnalysis)
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
        public bool JudgeIsEnableAuditDoubts(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (sampleApplyStatus != SampleApplyStatus.Doubt)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许质疑审核操作",sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableAuditDoubts)
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
        public bool JudgeIsEnableReleaseResult(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (sampleApplyStatus != SampleApplyStatus.Registed)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许发布结果操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableReleaseResult)
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
        public bool JudgeIsEnableCharge(Guid userId, SampleChargeStatus sampleChargeStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!(sampleChargeStatus == SampleChargeStatus.Charged))
                {
                    errorMessage = string.Format("申请单当前的扣费状态【{0}】,不允许进行扣费操作", sampleChargeStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableCharge)
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
        public bool JudgeIsEnableCancel(Guid userId,Guid applicantId,Guid creatorId, SampleApplyStatus sampleApplyStatus,bool hook ,object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (sampleApplyStatus != SampleApplyStatus.UnAudit)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许撤消操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                if (applicantId != userId && creatorId != userId)
                {
                    errorMessage = "非本人或者创建人,不能进行撤消操作";
                    return false;
                }
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableCancel)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableCopy(Guid userId,Guid applicantId,bool hook, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (applicantId != userId)
                {
                    errorMessage = "不能复制他人的申请单";
                    return false;
                }
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableCopy)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableDownLoad(Guid userId,Guid?reportObtainerId,Guid applicantId, SampleApplyStatus sampleApplyStatus, bool hook, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!reportObtainerId.HasValue)
                {
                    errorMessage = "未发布结果,不允许下载结果操作";
                    return false;
                }
                if (reportObtainerId.Value != applicantId)
                {
                    errorMessage = "不是结果获取人,不允许下载结果操作";
                    return false;
                }
                if((sampleApplyStatus != SampleApplyStatus.Released && sampleApplyStatus != SampleApplyStatus.AuditDoubted &&
                    sampleApplyStatus != SampleApplyStatus.Obtained))
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许下载结果操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableDownLoad)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableDoubts(Guid userId,Guid applicantId, SampleApplyStatus sampleApplyStatus, bool hook,object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
              
                if (applicantId != userId)
                {
                    errorMessage = "不能质疑他人的申请单";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.Obtained)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许质疑操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                if (!hook)
                {
                    SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                    if (samplePrivilige == null || !samplePrivilige.IsEnableDoubts)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableAudit(Guid userId, bool isNeedConfirmAcceptSample,bool isNeedTutorAudit,AuditStatus tutorAuditStatus,SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                return JudgeIsEnablePass(userId, isNeedConfirmAcceptSample, isNeedTutorAudit, tutorAuditStatus,sampleApplyStatus,samplePriviligeObj,out errorMessage) 
                    || JudgeIsEnableRefuse(userId, isNeedConfirmAcceptSample, isNeedTutorAudit, tutorAuditStatus,sampleApplyStatus,samplePriviligeObj,out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        public bool JudgeIsEnablePass(Guid userId, bool isNeedConfirmAcceptSample,bool isNeedTutorAudit,AuditStatus tutorAuditStatus,SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (isNeedConfirmAcceptSample)
                {
                    if (sampleApplyStatus != SampleApplyStatus.Accepted)
                    {
                        errorMessage = string.Format("当前状态【{0}】,不允许同意操作", sampleApplyStatus.ToCnName());
                        return false;
                    }
                }
                if (isNeedTutorAudit && tutorAuditStatus != AuditStatus.Passed)
                {
                    errorMessage = "该申请单需要导师审核通过才能进行同意操作";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit && sampleApplyStatus != SampleApplyStatus.ApplyCancel && sampleApplyStatus != SampleApplyStatus.Accepted && sampleApplyStatus != SampleApplyStatus.RefuseCancel)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许同意操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableAudit)
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
        public bool JudgeIsEnableRefuse(Guid userId, bool isNeedConfirmAcceptSample,bool isNeedTutorAudit,AuditStatus tutorAuditStatus,SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (isNeedConfirmAcceptSample)
                {
                    if (sampleApplyStatus != SampleApplyStatus.Accepted)
                    {
                        errorMessage = string.Format("当前状态【{0}】,不允许否决操作", sampleApplyStatus.ToCnName());
                        return false;
                    }
                }
                if (isNeedTutorAudit && tutorAuditStatus != AuditStatus.Passed)
                {
                    errorMessage = "该申请单需要导师审核通过才能进行否决操作";
                    return false;
                }
                if (sampleApplyStatus != SampleApplyStatus.UnAudit && 
                    sampleApplyStatus != SampleApplyStatus.Audited && 
                    sampleApplyStatus != SampleApplyStatus.Accepted && 
                    sampleApplyStatus != SampleApplyStatus.ApplyCancel &&
                    sampleApplyStatus != SampleApplyStatus.Canceled)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许否决操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableAudit)
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
        public bool JudgeIsEnableRegister(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {

                if (sampleApplyStatus != SampleApplyStatus.FinishTest)
                {
                    errorMessage = string.Format("当前状态【{0}】,不允许结果上传操作", sampleApplyStatus.ToCnName());
                    return false;
                }
                SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                if (samplePrivilige == null || !samplePrivilige.IsEnableRegister)
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
        public bool JudgeIsEnableInputFinishedQuatity(Guid userId, SampleApplyStatus sampleApplyStatus, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                //if (sampleApplyStatus != Model.Enum.SampleApplyStatus.FinishTest)
                //{
                //    errorMessage = string.Format("出错,送样单当前的状态不是【{0}】,不能进行操作", Model.Enum.SampleApplyStatus.FinishTest.ToCnName());
                //    return false;
                //}  
                //if (samplePrivilige == null || !samplePrivilige.IsEnableRegister)
                //{
                //    errorMessage = "无操作权限";
                //    return false;
                //}
                if (!__whenCanModifySampleAmount.Replace("，", ",").Split(',').Contains(((int)sampleApplyStatus).ToString()))
                {
                    errorMessage = string.Format("出错,当前状态【{0}】,不能修改样品数量", sampleApplyStatus.ToCnName());
                    return false;
                }
                //SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
                //if (!samplePrivilige.IsEnableInputFinishedQuatity)
                //{
                //    errorMessage = "出错,无操作权限";
                //    return false;
                //}
                
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool JudgeIsEnableFeedBack(Guid userId, bool isNeedFeedBack, Guid applicantId, SampleApplyStatus sampleApplyStatus, out string errorMessage)
        {
            errorMessage = "";
            if (!isNeedFeedBack)
            {
                errorMessage = "无需反馈";
                return false;
            }
            if (applicantId != userId)
            {
                errorMessage = "非送样人本人,不能进行反馈";
                return false;
            }
            if (sampleApplyStatus != Model.Enum.SampleApplyStatus.Obtained)
            {
                errorMessage = string.Format("非【{0}】状态,不能填写反馈", Model.Enum.SampleApplyStatus.Obtained.ToCnName());
                return false;
            }
           
            return true;
        }
        public bool JudgeIsEnableRelativeTestOperate(Guid operatorId, ViewSampleApply viewSampleApply, IList<SampleTester> sampleTesters, IList<SampleTestRecord> sampleTestRecords, SampleTestOption sampleTestOption, object samplePriviligeObj, out string errorMessage)
        {
            errorMessage = "";

            SamplePrivilige samplePrivilige = samplePriviligeObj != null ? samplePriviligeObj as SamplePrivilige : null;
            if (samplePrivilige == null || !samplePrivilige.IsEnableTest)
            {
                errorMessage = "无操作权限";
                return false;
            }
            if (sampleTesters != null && sampleTesters.Count > 0 && !sampleTesters.Any(p => p.TesterId == operatorId))
            {
                viewSampleApply.BeginHandleOperate = "";
                viewSampleApply.EndHandleOperate = "";
                viewSampleApply.BeginTestOperate = "";
                viewSampleApply.EndTestOperate = "";
                errorMessage = "非指定的测试人,不能进行操作";
                return false;
            }
            var statusEnum = (SampleApplyStatus)viewSampleApply.SampleStatus;
            if ((sampleTesters == null || sampleTesters.Count == 0 || sampleTesters.Count == 1) && sampleTestRecords != null && sampleTestRecords.Count > 0 && sampleTestRecords.Any(p => p.TesterId != operatorId))
            {
                errorMessage = string.Format("其他人正在进行{0}操作,不能进行{1}操作", statusEnum.ToCnName(), sampleTestOption.ToCnName());
                return false;
            }
            switch (sampleTestOption)
            {
                case SampleTestOption.BeginHandle:
                    if (viewSampleApply.SampleStatus != SampleApplyStatus.Audited)
                    {
                        errorMessage = string.Format("申请单当前的状态【{0}】不能进行【{1}】操作", viewSampleApply.SampleStatus.ToCnName(), sampleTestOption.ToCnName());
                        return false;
                    }
                    if (!viewSampleApply.IsNeedHandle)
                    {
                        errorMessage = "无需前处理";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginHandle)
                    {
                        errorMessage = "当前正在开始前处理,不能重复操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishHandle)
                    {
                        if (sampleTestRecords.Any(p => p.TesterId == operatorId))
                        {
                            errorMessage = "已经进行过开始前处理,不能重复进行操作";
                            return false;
                        }
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginTest)
                    {
                        errorMessage = "正在检测,不能进行开始前处理操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishTest)
                    {
                        errorMessage = "已完成检测,不能进行开始前处理操作";
                        return false;
                    }
                    break;
                case SampleTestOption.EndHandle:
                    if (!viewSampleApply.IsNeedHandle)
                    {
                        errorMessage = "无需前处理";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginHandle)
                    {
                        if (!sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Handle && p.EndTime == null))
                        {
                            errorMessage = "其他人正在进行开始前处理";
                            return false;
                        }

                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishHandle)
                    {
                        errorMessage = "结束前处理,不能进行结束前处理操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginTest)
                    {
                        errorMessage = "正在检测,不能进行结束前处理操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishTest)
                    {
                        errorMessage = "完成测试,不能进行结束前处理操作";
                        return false;
                    }
                    else
                    {
                        errorMessage = "当前状态不能进行结束前处理操作";
                        return false;
                    }
                    break;
                case SampleTestOption.BeginTest:
                    if (!viewSampleApply.IsNeedHandle && viewSampleApply.SampleStatus != SampleApplyStatus.Audited)
                    {
                        errorMessage = string.Format("申请单当前的状态【{0}】不能进行【{1}】操作", viewSampleApply.SampleStatus.ToCnName(), sampleTestOption.ToCnName());
                        return false;
                    }
                    if (viewSampleApply.Status == (int)SampleApplyStatus.BeginHandle)
                    {
                        errorMessage = "开始前处理,不能进行开始测试操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishHandle)
                    {
                        if (sampleTesters != null && sampleTesters.Count > 0)
                        {
                            foreach (var sampleTester in sampleTesters)
                            {
                                if (!sampleTestRecords.Any(p => p.TesterId == sampleTester.TesterId && p.TestCategoryEnum == SampleTestCategory.Handle && p.EndTime.HasValue))
                                {
                                    errorMessage = "有人尚未完成前处理，不能进行开始测试操作";
                                    return false;
                                }
                            }
                        }
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginTest)
                    {
                        if (viewSampleApply.IsNeedHandle && !sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Handle && p.EndTime.HasValue))
                        {
                            errorMessage = "未完成前处理,不能进行开始测试操作";
                            return false;
                        }
                        if (sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Test))
                        {
                            errorMessage = "已经进行过测试，不能重复进行";
                            return false;
                        }
                        if (sampleTestRecords.Any(p => p.TesterId != operatorId && p.TestCategoryEnum == SampleTestCategory.Test && p.EndTime == null))
                        {
                            errorMessage = "其他人正在进行开始测试，不能进行开始测试操作";
                            return false;
                        }
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishTest)
                    {
                        if (sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Test && p.EndTime.HasValue))
                        {
                            errorMessage = "已经完成检测,不能重复进行操作";
                            return false;
                        }
                    }
                    else
                    {
                        if (viewSampleApply.IsNeedHandle)
                        {
                            errorMessage = "尚未完成前处理,不能进行开始测试操作";
                            return false;
                        }
                    }
                    break;
                case SampleTestOption.EndTest:
                    if (viewSampleApply.Status == (int)SampleApplyStatus.BeginHandle)
                    {
                        errorMessage = "开始前处理,不能进行结束测试操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishHandle)
                    {
                        errorMessage = "完成前处理,不能进行结束测试操作";
                        return false;
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.BeginTest)
                    {
                        if (sampleTestRecords != null && sampleTestRecords.Count > 0)
                        {
                            if (sampleTestRecords.Any(p => p.TesterId != operatorId && p.TestCategoryEnum == SampleTestCategory.Test && p.EndTime == null))
                            {
                                errorMessage = "其他人正在进行检测,不能进行完成检测操作";
                                return false;
                            }
                            if (sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Test && p.EndTime.HasValue))
                            {
                                errorMessage = "已经完成检测,不能重复进行完成检测操作";
                                return false;
                            }
                            if (!sampleTestRecords.Any(p => p.TesterId == operatorId && p.TestCategoryEnum == SampleTestCategory.Test && p.EndTime == null))
                            {
                                errorMessage = "未开始检测,不能进行完成检测操作";
                                return false;
                            }
                        }
                    }
                    else if (viewSampleApply.Status == (int)SampleApplyStatus.FinishTest)
                    {
                        errorMessage = "已经完成检测,不能进行完成检测操作";
                        return false;
                    }
                    else
                    {
                        errorMessage = "当前状态不能进行结束前处理操作";
                        return false;
                    }
                    break;
            }
            return true;
        }
        public string[] GetViewSampleApplyOutMappings()
        {
            return new string[] { "EnableOperatorId", "EquipmentAdminId" };
        }
        public IList<ViewSampleApply> GetUserWillPaySampleApplyList(Guid payerId)
        {
            return GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*ChargeStatus={1}", payerId, (int)SampleChargeStatus.UnCharge), null, "", true, false, true, GetViewSampleApplyOutMappings());
        }
        public IList<ViewSampleApply> GetUserWillPaySampleApplyList(Guid userId, Guid subjectId)
        {
            return GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*ChargeStatus={1}*ApplicantId=\"{2}\"", subjectId, (int)SampleChargeStatus.UnCharge, userId), null, "", true, false, true, GetViewSampleApplyOutMappings());
        }
        public int GetUnAuditSampleApplyCount(Guid userId)
        {
            var queryExpression = "(Status=0)*(IsNeedTutorAudit=false+(IsNeedTutorAudit=true*TutorAuditStatus=0))*(IsNeedConfirmAcceptSample=false+(IsNeedConfirmAcceptSample=true*Status=5))";
            //var dictcodeTypes = __dictCodeTypeBLL.GetEntitiesByExpression("Code=UnAuditSampleQueryExpression");
            //if (dictcodeTypes != null && dictcodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(dictcodeTypes.First().Value))
            //    queryExpression = dictcodeTypes.First().Value.Trim();
            queryExpression += "*" + "EnableOperatorId=\"" + userId + "\"";
            return CountModelListByExpression(queryExpression, null, true, GetViewSampleApplyOutMappings());
        }
        public override GridData<ViewSampleApply> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (mapping == null) mapping = new Dictionary<string, string>();
            mapping["Id"] = "SampleApplyId";
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, mapping);
            GridData<ViewSampleApply> gridData = new GridData<ViewSampleApply>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var pageIndexDataParam = DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex);
            var pageCountDataParam = DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);

            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 pageIndexDataParam,
                 pageCountDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetSampleApplyList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewSampleApply> viewSampleApplies = new List<ViewSampleApply>();
                foreach (var result in results)
                {
                    ViewSampleApply viewSampleApply = new ViewSampleApply();
                    viewSampleApply.SampleNo = result[0].ToString();
                    viewSampleApply.RowNo = int.Parse(result[1].ToString());
                    viewSampleApply.Id = new Guid(result[2].ToString());
                    viewSampleApply.ApplicantId = new Guid(result[3].ToString());
                    viewSampleApply.ApplicantName = result[4] != null ? result[4].ToString() : null;
                    viewSampleApply.ApplyDate = DateTime.Parse(result[5].ToString());
                    viewSampleApply.GetResultWay = new Guid(result[6].ToString()); ;
                    viewSampleApply.GetResultWayName = result[7] != null ? result[7].ToString() : null;
                    viewSampleApply.ExpectSendDate = null;
                    if (result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString())) viewSampleApply.ExpectSendDate = DateTime.Parse(result[8].ToString());
                    viewSampleApply.ExpectResultDate = null;
                    if (result[9] != null && !string.IsNullOrWhiteSpace(result[9].ToString())) viewSampleApply.ExpectResultDate = DateTime.Parse(result[9].ToString());
                    viewSampleApply.UseEquipmentTarget = null;
                    if (result[10] != null && !string.IsNullOrWhiteSpace(result[10].ToString())) viewSampleApply.UseEquipmentTarget = new Guid(result[10].ToString());
                    viewSampleApply.UseEquipmentTargetName = result[11] != null ? result[11].ToString() : null;
                    viewSampleApply.SmapleStatus = null;
                    if (result[12] != null && !string.IsNullOrWhiteSpace(result[12].ToString())) viewSampleApply.SmapleStatus = new Guid(result[12].ToString());
                    viewSampleApply.SmapleStatusName = result[13] != null ? result[13].ToString() : null;
                    viewSampleApply.SampleStatusReamrk = result[14] != null ? result[14].ToString() : null;
                    viewSampleApply.TestCondition = null;
                    if (result[15] != null && !string.IsNullOrWhiteSpace(result[15].ToString())) viewSampleApply.TestCondition = new Guid(result[15].ToString());
                    viewSampleApply.TestConditionName = result[16] != null ? result[16].ToString() : null;
                    viewSampleApply.Status = int.Parse(result[17].ToString());
                    viewSampleApply.PasserId = null;
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString())) viewSampleApply.PasserId = new Guid(result[18].ToString());
                    viewSampleApply.PasserName = result[19] != null ? result[19].ToString() : null;
                    viewSampleApply.PassDate = null;
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString())) viewSampleApply.PassDate = DateTime.Parse(result[20].ToString());
                    viewSampleApply.PassRemark = result[21] != null ? result[21].ToString() : null;
                    viewSampleApply.RefuseRemark = result[22] != null ? result[22].ToString() : null;
                    viewSampleApply.Price = null;
                    if (result[23] != null && !string.IsNullOrWhiteSpace(result[23].ToString())) viewSampleApply.Price = double.Parse(result[23].ToString());
                    viewSampleApply.IsNeedHandle = bool.Parse(result[24].ToString());
                    viewSampleApply.IsUrgent = bool.Parse(result[25].ToString());
                    viewSampleApply.IsSecret = bool.Parse(result[26].ToString());
                    viewSampleApply.Remark = result[27] != null ? result[27].ToString() : null;
                    viewSampleApply.PredictResultTime = null;
                    if (result[28] != null && !string.IsNullOrWhiteSpace(result[28].ToString())) viewSampleApply.PredictResultTime = DateTime.Parse(result[28].ToString());
                    viewSampleApply.RegisterTime = null;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString())) viewSampleApply.RegisterTime = DateTime.Parse(result[29].ToString());
                    viewSampleApply.RegisterId = null;
                    if (result[30] != null && !string.IsNullOrWhiteSpace(result[30].ToString())) viewSampleApply.RegisterId = new Guid(result[30].ToString());
                    viewSampleApply.RegisterName = result[31] != null ? result[31].ToString() : null;
                    viewSampleApply.RegisterRemark = result[32] != null ? result[32].ToString() : null;
                    viewSampleApply.Quatity = int.Parse(result[33].ToString());
                    viewSampleApply.SampleItemId = new Guid(result[34].ToString());
                    viewSampleApply.SampleItemName = result[35] != null ? result[35].ToString() : null;
                    viewSampleApply.EquipmentId = new Guid(result[36].ToString());
                    viewSampleApply.EquipmentName = result[37] != null ? result[37].ToString() : null;
                    viewSampleApply.EquipmentLinkTelNo = result[38] != null ? result[38].ToString() : null;
                    viewSampleApply.FinishDate = null;
                    if (result[39] != null && !string.IsNullOrWhiteSpace(result[39].ToString())) viewSampleApply.FinishDate = DateTime.Parse(result[39].ToString());
                    viewSampleApply.SubjectId = new Guid(result[40].ToString());
                    viewSampleApply.SubjectName = result[41] != null ? result[41].ToString() : null;
                    viewSampleApply.PaymentMethod = int.Parse(result[42].ToString());
                    viewSampleApply.PayerId = new Guid(result[43].ToString());
                    viewSampleApply.PayerName = result[44] != null ? result[44].ToString() : null;
                    viewSampleApply.RelateId = new Guid(result[45].ToString());
                    viewSampleApply.GetResultTime = null;
                    if (result[46] != null && !string.IsNullOrWhiteSpace(result[46].ToString())) viewSampleApply.GetResultTime = DateTime.Parse(result[46].ToString());
                    viewSampleApply.ChargeStatus = int.Parse(result[47].ToString());
                    viewSampleApply.RefuserId = null;
                    if (result[48] != null && !string.IsNullOrWhiteSpace(result[48].ToString())) viewSampleApply.RefuserId = new Guid(result[48].ToString());
                    viewSampleApply.RefuseDate = null;
                    if (result[49] != null && !string.IsNullOrWhiteSpace(result[49].ToString())) viewSampleApply.RefuseDate = DateTime.Parse(result[49].ToString());
                    viewSampleApply.RefuserName = result[50] != null ? result[50].ToString() : null;
                    viewSampleApply.ChargeCategory = int.Parse(result[51].ToString());
                    viewSampleApply.AnalisyserId = null;
                    if (result[52] != null && !string.IsNullOrWhiteSpace(result[52].ToString())) viewSampleApply.AnalisyserId = new Guid(result[52].ToString());
                    viewSampleApply.AnalisyserName = result[53] != null ? result[53].ToString() : null;
                    viewSampleApply.AnalisysTime = null;
                    if (result[54] != null && !string.IsNullOrWhiteSpace(result[54].ToString())) viewSampleApply.AnalisysTime = DateTime.Parse(result[54].ToString());
                    viewSampleApply.AnalisysRemark = result[55] != null ? result[55].ToString() : null;
                    viewSampleApply.ReleaserId = null;
                    if (result[56] != null && !string.IsNullOrWhiteSpace(result[56].ToString())) viewSampleApply.ReleaserId = new Guid(result[56].ToString());
                    viewSampleApply.ReleaserName = result[57] != null ? result[57].ToString() : null;
                    viewSampleApply.ReleaseTime = null;
                    if (result[58] != null && !string.IsNullOrWhiteSpace(result[58].ToString())) viewSampleApply.ReleaseTime = DateTime.Parse(result[58].ToString());
                    viewSampleApply.ReleaseRemark = result[59] != null ? result[59].ToString() : null;
                    viewSampleApply.ReportObtainerId = null;
                    if (result[60] != null && !string.IsNullOrWhiteSpace(result[60].ToString())) viewSampleApply.ReportObtainerId = new Guid(result[60].ToString());
                    viewSampleApply.ReportObtainerName = result[61] != null ? result[61].ToString() : null;
                    viewSampleApply.ReportObtainTime = null;
                    if (result[62] != null && !string.IsNullOrWhiteSpace(result[62].ToString())) viewSampleApply.ReportObtainTime = DateTime.Parse(result[62].ToString());
                    viewSampleApply.IsDoubt = bool.Parse(result[63].ToString());
                    viewSampleApply.DoubterId = null;
                    if (result[64] != null && !string.IsNullOrWhiteSpace(result[64].ToString())) viewSampleApply.DoubterId = new Guid(result[64].ToString());
                    viewSampleApply.DoubterName = result[65] != null ? result[65].ToString() : null;
                    viewSampleApply.DoubtTime = null;
                    if (result[66] != null && !string.IsNullOrWhiteSpace(result[66].ToString())) viewSampleApply.DoubtTime = DateTime.Parse(result[66].ToString());
                    viewSampleApply.DoubtRemark = result[67] != null ? result[67].ToString() : null;
                    viewSampleApply.DoubtAuditorId = null;
                    if (result[68] != null && !string.IsNullOrWhiteSpace(result[68].ToString())) viewSampleApply.DoubtAuditorId = new Guid(result[68].ToString());
                    viewSampleApply.DoubtAuditorName = result[69] != null ? result[69].ToString() : null;
                    viewSampleApply.DoubtAuditTime = null;
                    if (result[70] != null && !string.IsNullOrWhiteSpace(result[70].ToString())) viewSampleApply.DoubtAuditTime = DateTime.Parse(result[70].ToString());
                    viewSampleApply.DoubtAuditRemark = result[71] != null ? result[71].ToString() : null;
                    viewSampleApply.OperateDate = DateTime.Parse(result[72].ToString());
                    viewSampleApply.RealCurrency = double.Parse(result[73].ToString());
                    viewSampleApply.VirtualCurrency = double.Parse(result[74].ToString());
                    viewSampleApply.TutorName = result[75] != null ? result[75].ToString() : null;
                    viewSampleApply.TestDemand = result[76] != null ? result[76].ToString() : null;
                    viewSampleApply.IsOuter = bool.Parse(result[77].ToString());
                    viewSampleApply.PaymentTypeId = null;
                    if (result[78] != null && !string.IsNullOrWhiteSpace(result[78].ToString())) viewSampleApply.PaymentTypeId = new Guid(result[78].ToString());
                    viewSampleApply.PaymentTypeName = result[79] != null ? result[79].ToString() : null;
                    viewSampleApply.SubjectDirectorId = null;
                    if (result[80] != null && !string.IsNullOrWhiteSpace(result[80].ToString())) viewSampleApply.SubjectDirectorId = new Guid(result[80].ToString());
                    viewSampleApply.SubjectDirectorName = result[81] != null ? result[81].ToString() : null;
                    viewSampleApply.ApplicantRelativePic = result[82] != null ? result[82].ToString() : null;
                    viewSampleApply.IsEnableSelectSampleSendTime = bool.Parse(result[83].ToString());
                    viewSampleApply.SubjectProjectId = null;
                    if (result[84] != null && !string.IsNullOrWhiteSpace(result[84].ToString())) viewSampleApply.SubjectProjectId = new Guid(result[84].ToString());
                    viewSampleApply.SubjectProjectName = result[85] != null ? result[85].ToString() : null;
                    viewSampleApply.ReportCategoryId = null;
                    if (result[86] != null && !string.IsNullOrWhiteSpace(result[86].ToString())) viewSampleApply.ReportCategoryId = new Guid(result[86].ToString());
                    viewSampleApply.ReportCategoryName = result[87] != null ? result[87].ToString() : null;
                    viewSampleApply.IsPayFor = bool.Parse(result[88].ToString());
                    viewSampleApply.PayForFee = double.Parse(result[89].ToString());
                    viewSampleApply.InvoiceNo = result[90] != null ? result[90].ToString() : null;
                    viewSampleApply.DepositRecordId = null;
                    if (result[91] != null && !string.IsNullOrWhiteSpace(result[91].ToString())) viewSampleApply.DepositRecordId = new Guid(result[91].ToString());
                    viewSampleApply.RealQuatity = null;
                    if (result[92] != null && !string.IsNullOrWhiteSpace(result[92].ToString())) viewSampleApply.RealQuatity = int.Parse(result[92].ToString());
                    viewSampleApply.EquipmentOrgId = null;
                    if (result[93] != null && !string.IsNullOrWhiteSpace(result[93].ToString())) viewSampleApply.EquipmentOrgId = new Guid(result[93].ToString());
                    viewSampleApply.UserOrgId = null;
                    if (result[94] != null && !string.IsNullOrWhiteSpace(result[94].ToString())) viewSampleApply.UserOrgId = new Guid(result[94].ToString());
                    viewSampleApply.IsNeedFeedBack = bool.Parse(result[95].ToString());
                    viewSampleApply.FeedbackTime = null;
                    if (result[96] != null && !string.IsNullOrWhiteSpace(result[96].ToString())) viewSampleApply.FeedbackTime = DateTime.Parse(result[96].ToString());
                    viewSampleApply.Description = result[97] != null ? result[97].ToString() : null;
                    viewSampleApply.CreatorId = null;
                    if (result[98] != null && !string.IsNullOrWhiteSpace(result[98].ToString())) viewSampleApply.CreatorId = new Guid(result[98].ToString());
                    viewSampleApply.Email = result[99] != null ? result[99].ToString() : null;
                    viewSampleApply.ContactAddress = result[100] != null ? result[100].ToString() : null;
                    viewSampleApply.OrganizationName = result[101] != null ? result[101].ToString() : null;
                    viewSampleApply.TelephoneNo = result[102] != null ? result[102].ToString() : null;
                    viewSampleApply.MobileNo = result[103] != null ? result[103].ToString() : null;
                    //viewSampleApply.UseNature = int.Parse(result[104].ToString());
                    viewSampleApply.IsNeedTutorAudit = bool.Parse(result[105].ToString());
                    viewSampleApply.TutorAuditStatus = int.Parse(result[106].ToString());
                    if (result[107] != null && !string.IsNullOrWhiteSpace(result[107].ToString())) viewSampleApply.AuditTutorId = new Guid(result[107].ToString());
                    viewSampleApply.IsNeedConfirmAcceptSample = bool.Parse(result[108].ToString());
                    if (result[109] != null && !string.IsNullOrWhiteSpace(result[109].ToString())) viewSampleApply.AccepterId = new Guid(result[109].ToString());
                    viewSampleApply.AccepterName = result[110] != null ? result[110].ToString() : null;
                    if (result[111] != null && !string.IsNullOrWhiteSpace(result[111].ToString())) viewSampleApply.AcceptDate = DateTime.Parse(result[111].ToString());
                    viewSampleApply.AcceptRemark = result[112] != null ? result[112].ToString() : null;

                    if (result[113] != null && !string.IsNullOrWhiteSpace(result[113].ToString())) viewSampleApply.RefuseAcceptOperaterId = new Guid(result[113].ToString());
                    viewSampleApply.RefuseAcceptOperaterName = result[114] != null ? result[114].ToString() : null;
                    if (result[115] != null && !string.IsNullOrWhiteSpace(result[115].ToString())) viewSampleApply.RefuseAcceptDate = DateTime.Parse(result[115].ToString());
                    viewSampleApply.RefuseAcceptRemark = result[116] != null ? result[116].ToString() : null;
                    viewSampleApply.Duration = null;
                    if (result[117] != null && !string.IsNullOrWhiteSpace(result[117].ToString())) viewSampleApply.Duration = double.Parse(result[117].ToString());

                    viewSampleApply.IsSupressLazyLoad = isSupressLazyLoad;
                    viewSampleApplies.Add(viewSampleApply);
                }
                gridData.Data = viewSampleApplies;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return gridData;
        }
        
    }
}
