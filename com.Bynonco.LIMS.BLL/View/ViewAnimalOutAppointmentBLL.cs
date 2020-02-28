using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewAnimalOutAppointmentBLL : BLLBase<ViewAnimalOutAppointment>, IViewAnimalOutAppointmentBLL
    {
        public void GetEnableGetOutAnimalAmount(Guid animalAppointmentOutId, string ethicsNo, Guid animalCategoryId, out int totalCount, out int femaleCount, out int maleCount, out int leftTotalCount, out int leftFemaleCount, out int leftMaleCount, out Guid animalSourceId, out string animalSourceName, out string animalSpecifications)
        {
            IEthicsApplyBLL ethicsApplyBLL = BLLFactory.CreateEthicsApplyBLL();
            IAnimalBLL animalBLL = BLLFactory.CreateAnimalBLL();
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
                var animalList = animalBLL.GetEntitiesByExpression(string.Format("EthicsNo=\"{0}\"*AnimalCategoryId=\"{1}\"*IsDelete=false*(StoreStatus={2}+AnimalOutAppointmentId=\"{3}\")", ethicsNo.Trim(), animalCategoryId, (int)AnimalStoreStatus.In, animalAppointmentOutId));
                if (animalList != null && animalList.Count > 0)
                {
                    var ethicsApply = ethicsApplyBLL.GetFirstOrDefaultEntityByExpression(string.Format("EthicsNo=\"{0}\"", ethicsNo.Trim()));
                    var ethicsApplyAnimalData = ethicsApply.EthicsApplyAnimalDatas.First(p => p.AnimalCategoryId == animalCategoryId);
                    animalSourceId = ethicsApplyAnimalData.AnimalSourceId;
                    animalSourceName = ethicsApplyAnimalData.AnimalSource.Name;
                    animalSpecifications = ethicsApplyAnimalData.AnimalSpecifications;
                    totalCount = animalList.Count;
                    femaleCount = animalList.Count(p=>!p.Sex);
                    maleCount = animalList.Count(p => p.Sex);
                    leftTotalCount = totalCount;
                    leftMaleCount = maleCount;
                    leftFemaleCount = femaleCount;
                    //var exceptAnimalList = animalList.Where(p => (((p.StoreStatus == (int)AnimalStoreStatus.Die || p.StoreStatus == (int)AnimalStoreStatus.RegisterGettingOut ||
                    //    p.StoreStatus == (int)AnimalStoreStatus.RegisterDeath || p.StoreStatus == (int)AnimalStoreStatus.GetOut) && p.AnimalOutAppointmentId != animalAppointmentOutId)));
                    //if (exceptAnimalList != null && exceptAnimalList.Count() > 0)
                    //{
                    //    leftTotalCount -= exceptAnimalList.Count();
                    //    leftMaleCount -= exceptAnimalList.Count(p=>p.Sex);
                    //    leftFemaleCount -= exceptAnimalList.Count(p => !p.Sex);
                    //}
                }
                //var animalAppointmentOutList = GetEntitiesByExpression(string.Format("Id=-\"{0}\"*EthicsNo=\"{1}\"*(Status={2}+Status={3})", animalAppointmentOutId, ethicsNo.Trim(), (int)AnimalOutAppointmentStatus.Apply, (int)AnimalOutAppointmentStatus.Pass));
                //if (animalAppointmentOutList != null && animalAppointmentOutList.Count>0)
                //{
                //    leftTotalCount -= animalAppointmentOutList.Sum(p=>p.Amount);
                //    leftMaleCount -= animalAppointmentOutList.Sum(p => p.MaleQuatity);
                //    leftFemaleCount -= animalAppointmentOutList.Sum(p => p.FemaleQuatity);
                //}

            }
        }
        public bool JudgeIsBeyondTheMaxAllowAmount(AnimalAppointmentSex animalAppointmentSex,Guid animalCategoryId, int amount, int maleAmount, int femaleAmount, string idStr, EthicsApply ethicsApply, out  string errorMessage)
        {
            errorMessage = "";
            Guid animalSourceId = Guid.Empty;
            string animalSpecifications = "", animalSourceName = "";
            int totalCount = 0, femaleCount = 0, maleCount = 0, leftTotalCount = 0, leftMaleCount = 0, leftFemaleCount = 0;
            GetEnableGetOutAnimalAmount(string.IsNullOrWhiteSpace(idStr) ? Guid.Empty : new Guid(idStr), ethicsApply.EthicsNo, animalCategoryId, out totalCount, out femaleCount, out maleCount, out leftTotalCount, out leftFemaleCount, out leftMaleCount, out animalSourceId, out animalSourceName, out animalSpecifications);
            if (amount > leftTotalCount)
            {
                errorMessage = string.Format("本次申请数量【{0}】大于该伦理号该品系所剩余允许申请的数量【{1}】", amount, leftTotalCount);
                return false;
            }
            if ((animalAppointmentSex == AnimalAppointmentSex.Female || animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale) && femaleAmount > leftFemaleCount)
            {
                errorMessage = string.Format("本次申请数量雌性动物的【{0}】大于该伦理号该品系所剩余允许申请的雌性动物数量【{1}】", femaleAmount, leftFemaleCount);
                return false;
            }
            if ((animalAppointmentSex == AnimalAppointmentSex.Male || animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale) && maleAmount > leftMaleCount)
            {
                errorMessage = string.Format("本次申请数量雄性动物的【{0}】大于该伦理号该品系所剩余允许申请的雄性动物数量【{1}】", maleAmount, leftMaleCount);
                return false;
            }
            if (animalAppointmentSex == AnimalAppointmentSex.MaleAndFemale && amount != femaleAmount + maleAmount)
            {
                errorMessage = "动物总数量！=雌性动物数量+雄性动物数量";
                return false;

            }
            return true;
        }


        public bool JudgeIsEnableEdit(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableEdit(userId.Value, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableEdit(Guid userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, object AnimalOutAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var AnimalOutAppointmentPriviligeTemp = AnimalOutAppointmentPrivilige != null ? AnimalOutAppointmentPrivilige as AnimalOutAppointmentPrivilige : null;
                if (AnimalOutAppointmentPriviligeTemp == null || !AnimalOutAppointmentPriviligeTemp.IsEnableEdit)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (ViewAnimalOutAppointment.StatusEnum != Model.Enum.AnimalOutAppointmentStatus.Apply)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行编辑操作", ViewAnimalOutAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (ViewAnimalOutAppointment.ApplicantId != userId)
                {
                    errorMessage = "不是本人,不能进行编辑操作";
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

        public bool JudgeIsEnableCancel(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableCancel(userId.Value, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCancel(Guid userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, object AnimalOutAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var AnimalOutAppointmentPriviligeTemp = AnimalOutAppointmentPrivilige != null ? AnimalOutAppointmentPrivilige as AnimalOutAppointmentPrivilige : null;
                if (AnimalOutAppointmentPriviligeTemp == null || !AnimalOutAppointmentPriviligeTemp.IsEnableCancel)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (ViewAnimalOutAppointment.StatusEnum != Model.Enum.AnimalOutAppointmentStatus.Apply)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行撤销操作", ViewAnimalOutAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (ViewAnimalOutAppointment.ApplicantId != userId)
                {
                    errorMessage = "不是本人,不能进行撤销操作";
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

        public bool JudgeIsEnablePass(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnablePass(userId.Value, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnablePass(Guid userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, object AnimalOutAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var AnimalOutAppointmentPriviligeTemp = AnimalOutAppointmentPrivilige != null ? AnimalOutAppointmentPrivilige as AnimalOutAppointmentPrivilige : null;
                if (AnimalOutAppointmentPriviligeTemp == null || !AnimalOutAppointmentPriviligeTemp.IsEnablePass)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (ViewAnimalOutAppointment.StatusEnum != Model.Enum.AnimalOutAppointmentStatus.Apply)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行同意操作", ViewAnimalOutAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (ViewAnimalOutAppointment.IsFeedBack)
                {
                    errorMessage = "已反馈,不能进行同意操作";
                    return false;
                }
                if (ViewAnimalOutAppointment.ApplicantId == userId)
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

        public bool JudgeIsEnableRefuse(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableRefuse(userId.Value, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableRefuse(Guid userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, object AnimalOutAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var AnimalOutAppointmentPriviligeTemp = AnimalOutAppointmentPrivilige != null ? AnimalOutAppointmentPrivilige as AnimalOutAppointmentPrivilige : null;
                if (AnimalOutAppointmentPriviligeTemp == null || !AnimalOutAppointmentPriviligeTemp.IsEnableRefuse)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (ViewAnimalOutAppointment.StatusEnum != Model.Enum.AnimalOutAppointmentStatus.Apply)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行否决操作", ViewAnimalOutAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (ViewAnimalOutAppointment.IsFeedBack)
                {
                    errorMessage = "已反馈,不能进行否决操作";
                    return false;
                }
                if (ViewAnimalOutAppointment.ApplicantId == userId)
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

        public bool JudgeIsEnableFeedBack(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, out string errorMessage)
        {
            errorMessage = "";
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value) : null;
            return JudgeIsEnableFeedBack(userId.Value, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableFeedBack(Guid userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, object AnimalOutAppointmentPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var AnimalOutAppointmentPriviligeTemp = AnimalOutAppointmentPrivilige != null ? AnimalOutAppointmentPrivilige as AnimalOutAppointmentPrivilige : null;
                if (AnimalOutAppointmentPriviligeTemp == null || !AnimalOutAppointmentPriviligeTemp.IsEnableFeedBack)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (!ViewAnimalOutAppointment.IsNeedFeedBack)
                {
                    errorMessage = "无需反馈";
                    return false;
                }
                if (ViewAnimalOutAppointment.StatusEnum != Model.Enum.AnimalOutAppointmentStatus.Pass)
                {
                    errorMessage = string.Format("当前状态【{0}】不能进行反馈操作", ViewAnimalOutAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (ViewAnimalOutAppointment.ApplicantId != userId)
                {
                    errorMessage = "不是本人,不能进行反馈操作";
                    return false;
                }
                if (ViewAnimalOutAppointment.IsFeedBack)
                {
                    errorMessage = "已反馈,不能进行重复操作";
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

        private void ExcuteOperateDecorate(Guid? userId, IList<ViewAnimalOutAppointment> ViewAnimalOutAppointmentList, bool isSupressLazyLoad)
        {
            AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige = null;
            if (userId.HasValue) AnimalOutAppointmentPrivilige = PriviligeFactory.GetAnimalOutAppointmentPrivilige(userId.Value);
            if (ViewAnimalOutAppointmentList != null && ViewAnimalOutAppointmentList.Count > 0)
            {
                foreach (var ViewAnimalOutAppointment in ViewAnimalOutAppointmentList)
                {
                    ViewAnimalOutAppointment.IsSupressLazyLoad = false;
                    ViewAnimalOutAppointment.InitOperate();
                    OperateDecorate(userId, ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige);
                    ViewAnimalOutAppointment.BuildOperate();
                    ViewAnimalOutAppointment.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, ViewAnimalOutAppointment ViewAnimalOutAppointment, AnimalOutAppointmentPrivilige AnimalOutAppointmentPrivilige)
        {
            string errorMessage = "";
            if (ViewAnimalOutAppointment == null) throw new ArgumentException("取材申请信息为空");
            if (AnimalOutAppointmentPrivilige == null)
            {
                ViewAnimalOutAppointment.CancelOperate = "";
                ViewAnimalOutAppointment.EditOperate = "";
                ViewAnimalOutAppointment.FeedBackOperate = "";
                ViewAnimalOutAppointment.PassOperate = "";
                ViewAnimalOutAppointment.RefuseOperate = "";
                ViewAnimalOutAppointment.ViewOperate = "";
                ViewAnimalOutAppointment.PrintOperate = "";
                return;
            }
            if (!AnimalOutAppointmentPrivilige.IsEnablePrint)
            {
                ViewAnimalOutAppointment.PrintOperate = "";
            }
            if (!JudgeIsEnableCancel(userId, ViewAnimalOutAppointment, out errorMessage))
            {
                ViewAnimalOutAppointment.CancelOperate = "";
            }
            if (!JudgeIsEnableEdit(userId, ViewAnimalOutAppointment, out errorMessage))
            {
                ViewAnimalOutAppointment.EditOperate = "";
            }
            if (!JudgeIsEnableFeedBack(userId, ViewAnimalOutAppointment, out errorMessage))
            {
                ViewAnimalOutAppointment.FeedBackOperate = "";
            }
            if (!JudgeIsEnablePass(userId, ViewAnimalOutAppointment, out errorMessage))
            {
                ViewAnimalOutAppointment.PassOperate = "";
            }
            if (!JudgeIsEnableRefuse(userId, ViewAnimalOutAppointment, out errorMessage))
            {
                ViewAnimalOutAppointment.RefuseOperate = "";
            }
            if (!AnimalOutAppointmentPrivilige.IsEnableView)
            {
                ViewAnimalOutAppointment.ViewOperate = "";
            }
        }
        public IList<ViewAnimalOutAppointment> GetViewAnimalOutAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalOutAppointmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewAnimalOutAppointmentList, isSupressLazyLoad);
            return ViewAnimalOutAppointmentList;
        }
        public IList<ViewAnimalOutAppointment> GetViewAnimalOutAppointmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalOutAppointmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewAnimalOutAppointmentList, isSupressLazyLoad);
            return ViewAnimalOutAppointmentList;
        }
        public IList<ViewAnimalOutAppointment> GetViewAnimalOutAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalOutAppointmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewAnimalOutAppointmentList, isSupressLazyLoad);
            return ViewAnimalOutAppointmentList;
        }
        public GridData<ViewAnimalOutAppointment> GetGridViewAnimalOutAppointmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalOutAppointmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewAnimalOutAppointmentList == null ? null : ViewAnimalOutAppointmentList.Data, isSupressLazyLoad);
            return ViewAnimalOutAppointmentList;
        }
        public GridData<ViewAnimalOutAppointment> GetGridViewAnimalOutAppointmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalOutAppointmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewAnimalOutAppointmentList == null ? null : ViewAnimalOutAppointmentList.Data, isSupressLazyLoad);
            return ViewAnimalOutAppointmentList;
        }
    }
}
