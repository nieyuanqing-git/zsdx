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
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewAnimalAppointmentBLL : BLLBase<ViewAnimalAppointment>, IViewAnimalAppointmentBLL
    {
        public void GenerateEnableAppointmentAnimalAmount(Guid animalAppointmentId, Guid animacategoryId,string ethicsNo, out int totalCount, out int femaleCount, out int maleCount, out int leftTotalCount, out int leftFemaleCount, out int leftMaleCount,out Guid animalSourceId,out string animalSourceName,out string animalSpecifications)
        {
            IEthicsApplyBLL ethicsApplyBLL = BLLFactory.CreateEthicsApplyBLL();
            IAnimalAppointmentBLL animalAppointmentBLL = BLLFactory.CreateAnimalAppointmentBLL();
            totalCount = 0;
            femaleCount = 0;
            maleCount = 0;
            leftTotalCount = 0;
            leftMaleCount = 0;
            leftFemaleCount = 0;
            animalSourceId = Guid.Empty;
            animalSourceName = "";
            animalSpecifications = "";
            if (!string.IsNullOrWhiteSpace(ethicsNo))
            {
                var ethicsApply = ethicsApplyBLL.GetFirstOrDefaultEntityByExpression(string.Format("EthicsNo=\"{0}\"", ethicsNo.Trim()));
                if (ethicsApply != null)
                {
                    var ethicsApplyAnimalData = ethicsApply.EthicsApplyAnimalDatas.First(p => p.AnimalCategoryId == animacategoryId);
                    animalSourceId = ethicsApplyAnimalData.AnimalSourceId;
                    animalSourceName = ethicsApplyAnimalData.AnimalSource.Name;
                    animalSpecifications = ethicsApplyAnimalData.AnimalSpecifications;
                    totalCount = ethicsApplyAnimalData.AllowQuatity;
                    femaleCount = ethicsApplyAnimalData.AllowFemaleQuatity;
                    maleCount = ethicsApplyAnimalData.AllowMaleQuatity;
                    leftTotalCount = ethicsApplyAnimalData.AllowQuatity;
                    leftFemaleCount = ethicsApplyAnimalData.AllowFemaleQuatity;
                    leftMaleCount = ethicsApplyAnimalData.AllowMaleQuatity;
                    var animalAppointmentList = animalAppointmentBLL.GetEntitiesByExpression(string.Format("EthicsNo=\"{0}\"*Status=-{1}*Status=-{2}*Id=-\"{3}\"*AnimalCategoryId=\"{4}\"", ethicsNo.Trim(), (int)AnimalAppointmentStatus.AuditedNotPass, (int)AnimalAppointmentStatus.Canceled, animalAppointmentId, animacategoryId));
                    if (animalAppointmentList != null && animalAppointmentList.Count > 0)
                    {
                        foreach(var animalAppointment in animalAppointmentList)
                        {
                            leftTotalCount -= animalAppointment.Status != (int)AnimalAppointmentStatus.Purchased ? animalAppointment.Quantity : animalAppointment.RealQuatity;
                        }
                        leftFemaleCount = leftFemaleCount - animalAppointmentList.Sum(p => p.FemaleQuatity);
                        leftMaleCount = leftMaleCount - animalAppointmentList.Sum(p => p.MaleQuatity);
                    }
                }
            }
        }
        public bool JudgeIsEnableEditDelegateAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableEditDelegateAppointment(userId.Value,viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEditDelegateAppointment(Guid userId,ViewAnimalAppointment viewAnimalAppointment, object AnimalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.DelegateBuy)
                {
                    errorMessage = "非代购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = AnimalAppointmentPrivilige != null ? AnimalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableEditDelegateAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行编辑操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewAnimalAppointment.ApplicantId)
                {
                    errorMessage = "只有申请人才能编辑申请单";
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
        public bool JudgeIsEnableAddDelegateAppointment(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAddDelegateAppointment(animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAddDelegateAppointment(object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableAddDelegateAppointment)
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
        public bool JudgeIsBeyondTheMaxAllowAmountOfEthicsApply(AnimalAppointmentSex animalAppointmentSex, Guid animalCategoryId,int amount, int maleAmount, int femaleAmount, string idStr, EthicsApply ethicsApply, out  string errorMessage)
        {
            errorMessage = "";
            Guid animalSourceId = Guid.Empty;
            int totalCount = 0, femaleCount = 0, maleCount = 0, leftTotalCount = 0, leftMaleCount = 0, leftFemaleCount = 0;
            string animalSpecifications = "", animalSourceName = "";
            GenerateEnableAppointmentAnimalAmount(string.IsNullOrWhiteSpace(idStr) ? Guid.Empty : new Guid(idStr), animalCategoryId, ethicsApply.EthicsNo, out totalCount, out femaleCount, out maleCount, out leftTotalCount, out leftFemaleCount, out leftMaleCount, out animalSourceId, out animalSourceName, out animalSpecifications);
            if (amount > leftTotalCount)
            {
                errorMessage = string.Format("本次申请数量【{0}】大于该伦理号所剩余允许申请的数量【{1}】", amount, leftTotalCount);
                return false;
            }
            if ((animalAppointmentSex == AnimalAppointmentSex.Female || animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale) && femaleAmount > leftFemaleCount)
            {
                errorMessage = string.Format("本次申请数量雌性动物的【{0}】大于该伦理号所剩余允许申请的雌性动物数量【{1}】", femaleAmount, leftFemaleCount);
                return false;
            }
            if ((animalAppointmentSex == AnimalAppointmentSex.Male || animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale) && maleAmount > leftMaleCount)
            {
                errorMessage = string.Format("本次申请数量雄性动物的【{0}】大于该伦理号所剩余允许申请的雄性动物数量【{1}】", maleAmount, leftMaleCount);
                return false;
            }
            if (animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale && amount != femaleAmount + maleAmount)
            {
                errorMessage = "动物总数量！=雌性动物数量+雄性动物数量";
                return false;

            }
            return true;
        }
        public bool JudgeIsEnableCancelDelegateAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableCancelDelegateAppointment(userId.Value, viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancelDelegateAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.DelegateBuy)
                {
                    errorMessage = "非代购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableCancelDelegateAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行取消操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewAnimalAppointment.ApplicantId)
                {
                    errorMessage = "只有申请人才能取消申请单";
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
        public bool JudgeIsEnableAuditPassDelegateAppointment(Guid? userId, ViewAnimalAppointment ViewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAuditPassDelegateAppointment(userId.Value, ViewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAuditPassDelegateAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.DelegateBuy)
                {
                    errorMessage = "非代购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableAuditPassDelegateAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行同意操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAnimalAppointment.ApplicantId == userId)
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
        public bool JudgeIsEnableAuditNotPassDelegateAppointment(Guid? userId, ViewAnimalAppointment ViewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige AnimalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAuditNotPassDelegateAppointment(userId.Value, ViewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAuditNotPassDelegateAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object AnimalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.DelegateBuy)
                {
                    errorMessage = "非代购类型的申请单";
                    return false;
                }
                var AnimalAppointmentPriviligeTemp = AnimalAppointmentPrivilige != null ? AnimalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (AnimalAppointmentPriviligeTemp == null || !AnimalAppointmentPriviligeTemp.IsEnableAuditNotPassDelegateAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行否决操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAnimalAppointment.ApplicantId == userId)
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
        public bool JudgeIsEnablePurchaseDelegateAppointment(Guid? userId, ViewAnimalAppointment ViewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige AnimalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnablePurchaseDelegateAppointment(userId.Value, ViewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnablePurchaseDelegateAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object AnimalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.DelegateBuy)
                {
                    errorMessage = "非代购类型的申请单";
                    return false;
                }
                var AnimalAppointmentPriviligeTemp = AnimalAppointmentPrivilige != null ? AnimalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (AnimalAppointmentPriviligeTemp == null || !AnimalAppointmentPriviligeTemp.IsEnablePurchaseDelegateAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.AuditedPass)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行采购操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAnimalAppointment.ApplicantId == userId)
                {
                    errorMessage = "不能采购自己的申请单";
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
        public bool JudgeIsEnableEditSelfAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableEditSelfAppointment(userId.Value,viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEditSelfAppointment(Guid userId,ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.SelfBuy)
                {
                    errorMessage = "非自购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableEditSelfAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行编辑操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewAnimalAppointment.ApplicantId)
                {
                    errorMessage = "只有本人才可以编辑申请单";
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
        public bool JudgeIsEnableAddSelfAppointment(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAddSelfAppointment(animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAddSelfAppointment(object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableAddSelfAppointment)
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
        public bool JudgeIsEnableCancelSelfAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableCancelSelfAppointment(userId.Value, viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancelSelfAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.SelfBuy)
                {
                    errorMessage = "非自购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableCancelSelfAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不行进行取消操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (userId != viewAnimalAppointment.ApplicantId)
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
        public bool JudgeIsEnableAuditPassSelfAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAuditPassSelfAppointment(userId.Value, viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAuditPassSelfAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.SelfBuy)
                {
                    errorMessage = "非自购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableAuditPassSelfAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行同意操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAnimalAppointment.ApplicantId == userId)
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
        public bool JudgeIsEnableAuditNotPassSelfAppointment(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableAuditNotPassSelfAppointment(userId.Value, viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAuditNotPassSelfAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.SelfBuy)
                {
                    errorMessage = "非自购类型的申请单";
                    return false;
                }
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null || !animalAppointmentPriviligeTemp.IsEnableAuditNotPassSelfAppointment)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.Applied)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行否决操作", viewAnimalAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAnimalAppointment.ApplicantId == userId)
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
        public bool JudgeIsEnablePurchaseSelfAppointment(Guid? userId, ViewAnimalAppointment ViewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige AnimalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnablePurchaseSelfAppointment(userId.Value, ViewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnablePurchaseSelfAppointment(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object AnimalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (viewAnimalAppointment.BuyTypeEnum != AnimalBuyType.SelfBuy)
                {
                    errorMessage = "非自购类型的申请单";
                    return false;
                }
                errorMessage = "自购类型,不能进行采购操作";
                return false;
                //var AnimalAppointmentPriviligeTemp = AnimalAppointmentPrivilige != null ? AnimalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                //if (AnimalAppointmentPriviligeTemp == null || !AnimalAppointmentPriviligeTemp.IsEnableAuditNotPassSelfAppointment)
                //{
                //    errorMessage = "无操作权限";
                //    return false;
                //}
                //if (viewAnimalAppointment.StatusEnum != Model.Enum.AnimalAppointmentStatus.AuditedPass)
                //{
                //    errorMessage = string.Format("当前状态【{0}】不能进行采购操作", viewAnimalAppointment.StatusEnum.ToCnName());
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        public bool JudgeIsEnableInputAppointmentAnimalQualifiedNo(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalAppointmentPrivilige animalAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableInputAppointmentAnimalQualifiedNo(userId.Value, viewAnimalAppointment, animalAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableInputAppointmentAnimalQualifiedNo(Guid userId, ViewAnimalAppointment viewAnimalAppointment, object animalAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalAppointmentPriviligeTemp = animalAppointmentPrivilige != null ? animalAppointmentPrivilige as AnimalAppointmentPrivilige : null;
                if (animalAppointmentPriviligeTemp == null)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.BuyTypeEnum == AnimalBuyType.DelegateBuy && !animalAppointmentPriviligeTemp.IsEnableInputDelegateAppointmentAnimalQualifiedNo)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.BuyTypeEnum == AnimalBuyType.SelfBuy && !animalAppointmentPriviligeTemp.IsEnableInputSelfAppointmentAnimalQualifiedNo)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimalAppointment.StatusEnum!= AnimalAppointmentStatus.Purchased)
                {
                    errorMessage = string.Format("当前状态【{0}】不能录入合格证编号", viewAnimalAppointment.StatusEnum.ToCnName());
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
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewAnimalAppointment> ViewAnimalAppointmentList, bool isSupressLazyLoad)
        {
            if (ViewAnimalAppointmentList == null || ViewAnimalAppointmentList.Count == 0) return;
            AnimalAppointmentPrivilige AnimalAppointmentPrivilige = PriviligeFactory.GetAnimalAppointmentPrivilige(userId.Value);
            foreach (var ViewAnimalAppointment in ViewAnimalAppointmentList)
            {

                ViewAnimalAppointment.IsSupressLazyLoad = false;
                ViewAnimalAppointment.InitOperate();
                OperateDecorate(userId, ViewAnimalAppointment, AnimalAppointmentPrivilige);
                ViewAnimalAppointment.BuildOperate();
                ViewAnimalAppointment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewAnimalAppointment viewAnimalAppointment, AnimalAppointmentPrivilige AnimalAppointmentPrivilige)
        {
            string errorMessage = "";
            if (viewAnimalAppointment == null) throw new ArgumentException("实验动物购买申请信息为空");
            if (AnimalAppointmentPrivilige == null)
            {
                viewAnimalAppointment.EditDelegateAppointmentOperate = "";
                viewAnimalAppointment.ViewDelegateAppointmentOperate = "";
                viewAnimalAppointment.PrintDelegateAppointmentOperate = "";
                viewAnimalAppointment.CancelDelegateAppointmentOperate = "";
                viewAnimalAppointment.AuditPassDelegateAppointmentOperate = "";
                viewAnimalAppointment.AuditNotPassDelegateAppointmentOperate = "";
                viewAnimalAppointment.PurchaseDelegateAppointmentOperate = "";

                viewAnimalAppointment.EditSelfAppointmentOperate = "";
                viewAnimalAppointment.ViewSelfAppointmentOperate = "";
                viewAnimalAppointment.PrintSelfAppointmentOperate = "";
                viewAnimalAppointment.CancelSelfAppointmentOperate = "";
                viewAnimalAppointment.AuditPassSelfAppointmentOperate = "";
                viewAnimalAppointment.AuditNotPassSelfAppointmentOperate = "";
                viewAnimalAppointment.PurchaseSelfAppointmentOperate = "";
                viewAnimalAppointment.InputAnimalQualifiedNoOperate = "";
                return;
            }
            if (viewAnimalAppointment.BuyTypeEnum == AnimalBuyType.DelegateBuy)
            {
                viewAnimalAppointment.EditSelfAppointmentOperate = "";
                viewAnimalAppointment.ViewSelfAppointmentOperate = "";
                viewAnimalAppointment.PrintSelfAppointmentOperate = "";
                viewAnimalAppointment.CancelSelfAppointmentOperate = "";
                viewAnimalAppointment.AuditPassSelfAppointmentOperate = "";
                viewAnimalAppointment.AuditNotPassSelfAppointmentOperate = "";
                viewAnimalAppointment.PurchaseSelfAppointmentOperate = "";
            }
            if (viewAnimalAppointment.BuyTypeEnum == AnimalBuyType.SelfBuy)
            {
                viewAnimalAppointment.EditDelegateAppointmentOperate = "";
                viewAnimalAppointment.ViewDelegateAppointmentOperate = "";
                viewAnimalAppointment.PrintDelegateAppointmentOperate = "";
                viewAnimalAppointment.CancelDelegateAppointmentOperate = "";
                viewAnimalAppointment.AuditPassDelegateAppointmentOperate = "";
                viewAnimalAppointment.AuditNotPassDelegateAppointmentOperate = "";
                viewAnimalAppointment.PurchaseDelegateAppointmentOperate = "";
            }
            if (!JudgeIsEnableCancelDelegateAppointment(userId, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.CancelDelegateAppointmentOperate = "";
            if (!JudgeIsEnableEditDelegateAppointment(userId, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.EditDelegateAppointmentOperate = "";

            if (!AnimalAppointmentPrivilige.IsEnablePrintDelegateAppointment)
                viewAnimalAppointment.PrintDelegateAppointmentOperate = "";

            if (!JudgeIsEnableAuditNotPassDelegateAppointment(userId.Value, viewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage))
                viewAnimalAppointment.AuditNotPassDelegateAppointmentOperate = "";

            if (!JudgeIsEnableAuditPassDelegateAppointment(userId.Value, viewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage))
                viewAnimalAppointment.AuditPassDelegateAppointmentOperate = "";

            if (!AnimalAppointmentPrivilige.IsEnableViewDelegateAppointment)
                viewAnimalAppointment.ViewDelegateAppointmentOperate = "";

            if (!JudgeIsEnableCancelDelegateAppointment(userId.Value, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.CancelDelegateAppointmentOperate = "";

            if (!JudgeIsEnablePurchaseDelegateAppointment(userId.Value, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.PurchaseDelegateAppointmentOperate = "";

            if (!JudgeIsEnableCancelSelfAppointment(userId, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.CancelSelfAppointmentOperate = "";
            if (!JudgeIsEnableEditSelfAppointment(userId, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.EditSelfAppointmentOperate = "";

            if (!AnimalAppointmentPrivilige.IsEnablePrintSelfAppointment)
                viewAnimalAppointment.PrintSelfAppointmentOperate = "";

            if (!JudgeIsEnableAuditNotPassSelfAppointment(userId.Value, viewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage))
                viewAnimalAppointment.AuditNotPassSelfAppointmentOperate = "";

            if (!JudgeIsEnableAuditPassSelfAppointment(userId.Value, viewAnimalAppointment, AnimalAppointmentPrivilige, out errorMessage))
                viewAnimalAppointment.AuditPassSelfAppointmentOperate = "";

            if (!AnimalAppointmentPrivilige.IsEnableViewSelfAppointment)
                viewAnimalAppointment.ViewSelfAppointmentOperate = "";

            if (!JudgeIsEnableCancelSelfAppointment(userId.Value, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.CancelSelfAppointmentOperate = "";

            if (!JudgeIsEnablePurchaseSelfAppointment(userId.Value, viewAnimalAppointment, out errorMessage))
                viewAnimalAppointment.PurchaseSelfAppointmentOperate = "";

            if (!JudgeIsEnableInputAppointmentAnimalQualifiedNo(userId.Value, viewAnimalAppointment, out errorMessage)) 
                viewAnimalAppointment.InputAnimalQualifiedNoOperate = "";

        }
        public IList<ViewAnimalAppointment> GetViewAnimalAppointmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var ViewAnimalAppointmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, ViewAnimalAppointmentList, isSupressLazyLoad);
            return ViewAnimalAppointmentList;
        }

        public IList<ViewAnimalAppointment> GetViewAnimalAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var ViewAnimalAppointmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, ViewAnimalAppointmentList, isSupressLazyLoad);
            return ViewAnimalAppointmentList;
        }

        public IList<ViewAnimalAppointment> GetViewAnimalAppointmentListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var ViewAnimalAppointmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, ViewAnimalAppointmentList, isSupressLazyLoad);
            return ViewAnimalAppointmentList;
        }

        public Logic.Model.GridData<ViewAnimalAppointment> GetGridViewAnimalAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            expression = dataGridSettings.QueryExpression;
            var ViewAnimalAppointmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, ViewAnimalAppointmentList == null ? null : ViewAnimalAppointmentList.Data, isSupressLazyLoad);
            return ViewAnimalAppointmentList;
        }

        public Logic.Model.GridData<ViewAnimalAppointment> GetGridViewAnimalAppointmentListByExpression(Guid? userId, JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var ViewAnimalAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, ViewAnimalAppointmentList == null ? null : ViewAnimalAppointmentList.Data, isSupressLazyLoad);
            return ViewAnimalAppointmentList;
        }
    }
}
