using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewAnimalBLL : BLLBase<ViewAnimal>, IViewAnimalBLL
    {
        public bool JudgeIsEnableIn(Guid? userId, ViewAnimal viewAnimal, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;
            return JudgeIsEnableIn(userId.Value, viewAnimal, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableIn(Guid userId, ViewAnimal viewAnimal, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableIn)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimal.StatusEnum != Model.Enum.AnimalStatus.Live ||
                   (viewAnimal.StoreStatusEnum != Model.Enum.AnimalStoreStatus.In &&
                    viewAnimal.StoreStatusEnum != Model.Enum.AnimalStoreStatus.Input))
                {
                    errorMessage = string.Format("当前状态【{0}】,储存状态【{1}】入库操作", viewAnimal.StatusEnum.ToCnName(), viewAnimal.StoreStatusEnum.ToCnName());
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
        public bool JudgeIsEnableRegisterDeath(Guid? userId, ViewAnimal viewAnimal, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;
            return JudgeIsEnableRegisterDeath(userId.Value, viewAnimal, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableRegisterDeath(Guid userId, ViewAnimal viewAnimal, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableRegisterDeath)
                {
                    errorMessage = "无操作权限";
                    return false;
                }
                if (viewAnimal.StatusEnum != Model.Enum.AnimalStatus.Live ||
                   (viewAnimal.StoreStatusEnum != Model.Enum.AnimalStoreStatus.In &&
                    viewAnimal.StoreStatusEnum != Model.Enum.AnimalStoreStatus.RegisterDeath))
                {
                    errorMessage = string.Format("当前状态【{0}】,储存状态【{1}】不能进行登记死亡操作", viewAnimal.StatusEnum.ToCnName(), viewAnimal.StoreStatusEnum.ToCnName());
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
        public bool JudgeIsEnableConfirmRegistingDeath(bool isAuto, Guid? userId, ViewAnimal viewAnimal, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;

            return JudgeIsEnableConfirmRegistingDeath(isAuto,userId.HasValue ? userId.Value : Guid.Empty, viewAnimal, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableConfirmRegistingDeath(bool isAuto, Guid userId, ViewAnimal viewAnimal, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!isAuto)
                {
                    var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                    if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableConfirmRegistingDeath)
                    {
                        errorMessage = "无操作权限";
                        return false;
                    }
                    if (viewAnimal.OwnerId.HasValue && viewAnimal.OwnerId.Value != userId)
                    {
                        errorMessage = "非本人，不可以进行确认登记死亡操作";
                        return false;
                    }
                }
                if (viewAnimal.StoreStatusEnum != Model.Enum.AnimalStoreStatus.RegisterDeath)
                {
                    errorMessage = string.Format("当前状态【{0}】,储存状态【{1}】不能进行确认登记死亡操作", viewAnimal.StatusEnum.ToCnName(), viewAnimal.StoreStatusEnum.ToCnName());
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
        public bool JudgeIsEnableCostDeduct(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;
            return JudgeIsEnableCostDeduct(userId.Value, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableCostDeduct(Guid userId, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableCostDeduct)
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
        public bool JudgeIsEnableBatchCostDeduct(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;
            return JudgeIsEnableBatchCostDeduct(userId.Value, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableBatchCostDeduct(Guid userId, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableBatchCostDeduct)
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
        public bool JudgeIsEnableAutoCostDeduct(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            AnimalPrivilige animalPrivilige = userId.HasValue ? PriviligeFactory.GetAnimalPrivilige(userId.Value) : null;
            return JudgeIsEnableAutoCostDeduct(userId.Value, animalPrivilige, out errorMessage);
        }
        public bool JudgeIsEnableAutoCostDeduct(Guid userId, object animalPrivilige, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var animalPriviligeTemp = animalPrivilige != null ? animalPrivilige as AnimalPrivilige : null;
                if (animalPriviligeTemp == null || !animalPriviligeTemp.IsEnableAutoCostDeduct)
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
        private void ExcuteOperateDecorate(Guid? userId, bool isForCostDeduct, IList<ViewAnimal> ViewAnimalList, bool isSupressLazyLoad)
        {
            AnimalPrivilige AnimalPrivilige = null;
            if (userId.HasValue) AnimalPrivilige = PriviligeFactory.GetAnimalPrivilige(userId.Value);
            if (ViewAnimalList != null && ViewAnimalList.Count > 0)
            {
                foreach (var ViewAnimal in ViewAnimalList)
                {
                    ViewAnimal.IsForCostDeduct = isForCostDeduct;
                    ViewAnimal.IsSupressLazyLoad = false;
                    ViewAnimal.InitOperate();
                    OperateDecorate(userId, ViewAnimal, AnimalPrivilige);
                    ViewAnimal.BuildOperate();
                    ViewAnimal.IsSupressLazyLoad = isSupressLazyLoad;
                }
            }
        }
        private void OperateDecorate(Guid? userId, ViewAnimal ViewAnimal, AnimalPrivilige AnimalPrivilige)
        {
            string errorMessage = "";
            if (ViewAnimal == null) throw new ArgumentException("动物信息为空");
            if (AnimalPrivilige == null)
            {
                ViewAnimal.EditOperate = "";
                ViewAnimal.RegisterDeathOperate = "";
                ViewAnimal.ConfirmRegistingDeathOperate = "";
                ViewAnimal.ViewOperate = "";
                ViewAnimal.InOperate = "";
                return;
            }
            if (!AnimalPrivilige.IsEnableEdit)
            {
                ViewAnimal.EditOperate = "";
            }
            if (!AnimalPrivilige.IsEnableView)
            {
                ViewAnimal.ViewOperate = "";
            }
            if (!JudgeIsEnableRegisterDeath(userId, ViewAnimal, out errorMessage))
            {
                ViewAnimal.RegisterDeathOperate = "";
            }
            if (!JudgeIsEnableConfirmRegistingDeath(false, userId, ViewAnimal, out errorMessage))
            {
                ViewAnimal.ConfirmRegistingDeathOperate = "";
            }
            if (!JudgeIsEnableIn(userId, ViewAnimal, out errorMessage))
            {
                ViewAnimal.InOperate = "";
            }
            if (!AnimalPrivilige.IsEnableCostDeduct)
            {
                ViewAnimal.CostDeductOperate = "";
            }
        }
        public IList<ViewAnimal> GetViewAnimalListByExpression(Guid? userId, bool isForCostDeduct,string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId,isForCostDeduct, ViewAnimalList, isSupressLazyLoad);
            return ViewAnimalList;
        }
        public IList<ViewAnimal> GetViewAnimalListByExpression(Guid? userId, bool isForCostDeduct, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, isForCostDeduct, ViewAnimalList, isSupressLazyLoad);
            return ViewAnimalList;
        }
        public IList<ViewAnimal> GetViewAnimalListByExpression(Guid? userId, bool isForCostDeduct, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, isForCostDeduct, ViewAnimalList, isSupressLazyLoad);
            return ViewAnimalList;
        }
        public GridData<ViewAnimal> GetGridViewAnimalListByExpression(Guid? userId, bool isForCostDeduct, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, isForCostDeduct, ViewAnimalList == null ? null : ViewAnimalList.Data, isSupressLazyLoad);
            return ViewAnimalList;
        }
        public GridData<ViewAnimal> GetGridViewAnimalListByExpression(Guid? userId, bool isForCostDeduct, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            var ViewAnimalList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, isForCostDeduct, ViewAnimalList == null ? null : ViewAnimalList.Data, isSupressLazyLoad);
            return ViewAnimalList;
        }
    }
}
