using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEthicsApplyBLL : BLLBase<ViewEthicsApply>, IViewEthicsApplyBLL
    {
        public bool JudgeIsPassTranning(Guid? userId, out string errorMessage)
        {
            IEthicsSettingBLL ethicsSettingBLL = new EthicsSettingBLL();
            IUserExaminationBLL userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
            errorMessage = "";
            if (!userId.HasValue)
            {
                errorMessage = "用户信息为空";
                return false;
            }
            
            var ethicsSetting = ethicsSettingBLL.GetEthicsSetting();
            if (ethicsSetting.IsNeedTrainningBeforeApply && ethicsSetting.NeedPassTrainningTypes != null && ethicsSetting.NeedPassTrainningTypes.Count > 0)
            {
                var userExaminationList = userExaminationBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsPass=true", userId.Value));
                if (userExaminationList == null || userExaminationList.Count == 0)
                {
                    errorMessage = "未通过任何考试";
                    return false;
                }
                bool isPass = true;
                foreach (var needPassTrainningType in ethicsSetting.NeedPassTrainningTypes)
                {
                    if (!userExaminationList.Any(p => p.TrainningExamination != null && p.TrainningExamination.TrainningTypeId == needPassTrainningType.Id))
                    {
                        string msg = string.Format("未通过考试类型为:{0}的考试", needPassTrainningType.Name);
                        errorMessage += string.IsNullOrWhiteSpace(errorMessage) ? msg : "\n\r" + msg;
                        isPass = false;
                    }
                }
                return isPass;
            }
            return true;
        }
        public bool JudgeIsEnableEdit(Guid? userId, ViewEthicsApply viewEthicsApply, out string errorMessage)
        {
            errorMessage = "";
            EthicsApplyPrivilige ethicsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEthicsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableEdit(userId.Value,viewEthicsApply, ethicsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEdit(Guid userId,ViewEthicsApply viewEthicsApply, object EthicsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var ethicsApplyPriviligeTemp = EthicsApplyPrivilige != null ? EthicsApplyPrivilige as EthicsApplyPrivilige : null;
                if (ethicsApplyPriviligeTemp == null || !ethicsApplyPriviligeTemp.IsEnableEdit)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEthicsApply.StatusEnum != EthicsApplyStatus.Applied || viewEthicsApply.ApplicantId != userId)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行编辑操作", viewEthicsApply.StatusEnum.ToCnName());
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
            EthicsApplyPrivilige ethicsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEthicsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableApply(ethicsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableApply(object ethicsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var ethicsApplyPriviligeTemp = ethicsApplyPrivilige != null ? ethicsApplyPrivilige as EthicsApplyPrivilige : null;
                if (ethicsApplyPriviligeTemp == null || !ethicsApplyPriviligeTemp.IsEnableApply)
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
        public bool JudgeIsEnableCancel(Guid? userId, ViewEthicsApply viewEthicsApply, out string errorMessage)
        {
            errorMessage = "";
            EthicsApplyPrivilige ethicsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEthicsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableCancel(userId.Value, viewEthicsApply, ethicsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancel(Guid userId, ViewEthicsApply viewEthicsApply, object ethicsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var ethicsApplyPriviligeTemp = ethicsApplyPrivilige != null ? ethicsApplyPrivilige as EthicsApplyPrivilige : null;
                if (ethicsApplyPriviligeTemp == null || !ethicsApplyPriviligeTemp.IsEnableCancel)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEthicsApply.StatusEnum != EthicsApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行取消操作", viewEthicsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewEthicsApply.ApplicantId)
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
        public bool JudgeIsEnableAuditPass(Guid? userId, ViewEthicsApply viewEthicsApply, out string errorMessage)
        {
            errorMessage = "";
            EthicsApplyPrivilige ethicsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEthicsApplyPrivilige(userId.Value) : null;
            return JudgeIsEnableAuditPass(userId.Value, viewEthicsApply, ethicsApplyPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAuditPass(Guid userId, ViewEthicsApply viewEthicsApply, object ethicsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var ethicsApplyPriviligeTemp = ethicsApplyPrivilige != null ? ethicsApplyPrivilige as EthicsApplyPrivilige : null;
                if (ethicsApplyPriviligeTemp == null || !ethicsApplyPriviligeTemp.IsEnableAuditPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEthicsApply.StatusEnum != Model.Enum.EthicsApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行同意操作", viewEthicsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (viewEthicsApply.ApplicantId == userId)
                {
                    errorMessage = "不能同意自己的申请单";
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
        public bool JudgeIsEnableAuditNoPass(Guid? userId, ViewEthicsApply viewEthicsApply, out string errorMessage)
        {
            errorMessage = "";
            EthicsApplyPrivilige ethicsApplyPrivilige = userId.HasValue ? PriviligeFactory.GetEthicsApplyPrivilige(userId.Value) : null;
            return JudgeAuditNoPass(userId.Value, viewEthicsApply, ethicsApplyPrivilige, out errorMessage);
        }
        public bool JudgeAuditNoPass(Guid userId, ViewEthicsApply viewEthicsApply, object ethicsApplyPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var ethicsApplyPriviligeTemp = ethicsApplyPrivilige != null ? ethicsApplyPrivilige as EthicsApplyPrivilige : null;
                if (ethicsApplyPriviligeTemp == null || !ethicsApplyPriviligeTemp.IsEnableAuditNotPass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewEthicsApply.StatusEnum != Model.Enum.EthicsApplyStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行否决操作", viewEthicsApply.StatusEnum.ToCnName());
                    return false;
                }
                if (viewEthicsApply.ApplicantId == userId)
                {
                    errorMessage = "不能否决自己的申请单";
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
       

        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEthicsApply> viewEthicsApplyList, bool isSupressLazyLoad)
        {
            if (viewEthicsApplyList == null || viewEthicsApplyList.Count == 0) return;
            EthicsApplyPrivilige EthicsApplyPrivilige = PriviligeFactory.GetEthicsApplyPrivilige(userId.Value);
            foreach (var viewEthicsApply in viewEthicsApplyList)
            {

                viewEthicsApply.IsSupressLazyLoad = false;
                viewEthicsApply.InitOperate();
                OperateDecorate(userId, viewEthicsApply, EthicsApplyPrivilige);
                viewEthicsApply.BuildOperate();
                viewEthicsApply.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewEthicsApply viewEthicsApply, EthicsApplyPrivilige EthicsApplyPrivilige)
        {
            string errorMessage = "";
            if (viewEthicsApply == null) throw new ArgumentException("伦理申请信息为空");
            if (EthicsApplyPrivilige == null)
            {
                viewEthicsApply.EditOperate = "";
                viewEthicsApply.ViewOperate = "";
                viewEthicsApply.PrintOperate = "";
                viewEthicsApply.CancelOperate = "";
                viewEthicsApply.AuditPassOperate = "";
                viewEthicsApply.AuditNoPassOperate = "";
                return;
            }
            if (!JudgeIsEnableCancel(userId, viewEthicsApply, out errorMessage))
                viewEthicsApply.CancelOperate = "";
            if (!JudgeIsEnableEdit(userId, viewEthicsApply, out errorMessage))
                viewEthicsApply.EditOperate = "";

            if (!EthicsApplyPrivilige.IsEnablePrint)
                viewEthicsApply.PrintOperate = "";

            if (!JudgeAuditNoPass(userId.Value, viewEthicsApply, EthicsApplyPrivilige, out errorMessage))
                viewEthicsApply.AuditNoPassOperate = "";

            if (!JudgeIsEnableAuditPass(userId.Value, viewEthicsApply, EthicsApplyPrivilige, out errorMessage))
                viewEthicsApply.AuditPassOperate = "";

            if (!EthicsApplyPrivilige.IsEnableView)
                viewEthicsApply.ViewOperate = "";

            if (!JudgeIsEnableCancel(userId.Value, viewEthicsApply, out errorMessage))
                viewEthicsApply.CancelOperate = "";

        }
        public IList<ViewEthicsApply> GetViewEthicsApplyListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEthicsApplyList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEthicsApplyList, isSupressLazyLoad);
            return viewEthicsApplyList;
        }

        public IList<ViewEthicsApply> GetViewEthicsApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEthicsApplyList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEthicsApplyList, isSupressLazyLoad);
            return viewEthicsApplyList;
        }

        public IList<ViewEthicsApply> GetViewEthicsApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEthicsApplyList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEthicsApplyList, isSupressLazyLoad);
            return viewEthicsApplyList;
        }

        public Logic.Model.GridData<ViewEthicsApply> GetGridViewEthicsApplyListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            expression = dataGridSettings.QueryExpression;
            var viewEthicsApplyList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEthicsApplyList == null ? null : viewEthicsApplyList.Data, isSupressLazyLoad);
            return viewEthicsApplyList;
        }

        public Logic.Model.GridData<ViewEthicsApply> GetGridViewEthicsApplyListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewEthicsApplyList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewEthicsApplyList == null ? null : viewEthicsApplyList.Data, isSupressLazyLoad);
            return viewEthicsApplyList;
        }
    }
}
