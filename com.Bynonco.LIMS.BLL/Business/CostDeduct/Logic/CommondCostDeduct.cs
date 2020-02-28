using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>成本扣费指令</summary>
    public abstract class CommondCostDeduct : CostDeductBase
    {
        /// <summary> 课题组业务逻辑 </summary>
        private ISubjectBLL __subjectBLL;
        /// <summary> 用户账号业务逻辑 </summary>
        protected IUsedConfirmBLL _usedConfirmBLL;
        /// <summary> 是否扣费课题组 </summary>
        protected bool _isDeductSubjectHook = true;
        /// <summary> 是否扣费课题组 </summary>
        protected bool _isDeductSubject = true;
        /// <summary> 是否保存开放基金设备 </summary>
        protected bool _isSaveEquipmentOpenFund = true;
        /// <summary> 开放基金扣费设备 </summary>
        protected IList<CostDeductEquipmentOpenFund> _preCostDeductEquipmentOpenFunds;
        /// <summary> 开放基金明细 </summary>
        protected IList<ViewOpenFundDetail> _viewOpenFundDetails;
        public CommondCostDeduct(object[] businessObjects,Guid? operatorId,string operateIP)
            : base(businessObjects, operatorId, operateIP)
        {
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            _usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
        }
        /// <summary> 创建成本扣费 </summary>
        /// <returns></returns>
        protected abstract Model.CostDeduct CreateCostDeduct();
        /// <summary> 获取并处理扣费 </summary>
        /// <param name="subjectId"></param>
        /// <param name="userId"></param>
        /// <param name="paymentMethod"></param>
        /// <returns></returns>
        protected abstract CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod);
        protected virtual void BusinessHandleAfterDeduct(ref august.DataLink.XTransaction tran) { }
        protected virtual void DeductHandle(ref august.DataLink.XTransaction tran, CostDeduct costDeduct, CostDeduct originalCostDeduct, bool isNew)
        {
            string errorMessage = "";
            UserAccount userAccount = null;
            Guid userId = Guid.Empty; ;
            Guid subjectId = Guid.Empty;
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            BusinessHandle(out subjectId, out userId, out paymentMethod);
            double realCurrency = 0d, virtualCurrency = 0d;
            if (isNew)
            {
                userAccount = _userAccountBLL.Deduct(costDeduct.IsOpenFundCostDeduct,userId, subjectId, paymentMethod, costDeduct.Fee, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false, _isDeductSubject);
            }
            else
            {
                userAccount = originalCostDeduct.UserAccount;
                var fee = costDeduct.Fee - originalCostDeduct.Fee;
                if(_isDeductSubject) __subjectBLL.Deduct(userId, subjectId, paymentMethod, fee, ref tran);
                userAccount.RealCurrency +=originalCostDeduct.RealCurrency.Value;
                userAccount.VirtualCurrency += originalCostDeduct.VirtualCurrency.Value;
                userAccount.Deduct(costDeduct.IsOpenFundCostDeduct, costDeduct.Fee, out realCurrency, out virtualCurrency);
            }
            _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, tran != null);
        }
        /// <summary> 取消扣费 </summary>
        /// <param name="tran"></param>
        /// <param name="userAccount"></param>
        protected virtual void CancelDeductHandle(ref august.DataLink.XTransaction tran, UserAccount userAccount)
        {
            _userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, tran != null);
        }
        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            string errorMessage = "";
            // 如果存在事务则挂起
            bool isSupress = tran != null;
            var costDeduct = CreateCostDeduct();
            if (costDeduct.HasClossingAccount) throw new Exception ("已结算");
            try
            {
                if (!CheckMoney()) throw new CheckMoneyFailException(errorMessage);
                if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
                if (costDeduct.ManualCostDeduct != null)
                {
                    var manualCostDeductTemp = _manualCostDeductBLL.GetEntityById(costDeduct.ManualCostDeduct.Id);
                    if (manualCostDeductTemp != null)
                        _manualCostDeductBLL.Save(new ManualCostDeduct[] { costDeduct.ManualCostDeduct }, ref tran, true);
                    else
                        _manualCostDeductBLL.Add(new ManualCostDeduct[] { costDeduct.ManualCostDeduct }, ref tran, true);
                }
                if (costDeduct.AnimalCostDeduct != null)
                {
                    var animalCostDeductTemp = _animalCostDeductBLL.GetEntityById(costDeduct.AnimalCostDeduct.Id);
                    if (animalCostDeductTemp != null)
                        _animalCostDeductBLL.Save(new AnimalCostDeduct[] { costDeduct.AnimalCostDeduct }, ref tran, true);
                    else
                        _animalCostDeductBLL.Add(new AnimalCostDeduct[] { costDeduct.AnimalCostDeduct }, ref tran, true);
                }
                if (costDeduct.MaterialOutput != null)
                {
                    var materialOutputTemp = _materialOutputBLL.GetEntityById(costDeduct.MaterialOutput.Id);
                    if (materialOutputTemp != null)
                    {
                        costDeduct.MaterialOutput.Status = (int)MaterialOutputStatus.Deduct;
                        _materialOutputBLL.Save(new MaterialOutput[] { costDeduct.MaterialOutput }, ref tran, true);
                    }
                    else
                        _materialOutputBLL.Add(new MaterialOutput[] { costDeduct.MaterialOutput }, ref tran, true);
                }
                if (costDeduct.WaterControlCostDeduct != null)
                {
                    var waterControlCostDeductTemp = _waterControlCostDeductBLL.GetEntityById(costDeduct.WaterControlCostDeduct.Id);
                    if (waterControlCostDeductTemp != null)
                        _waterControlCostDeductBLL.Save(new WaterControlCostDeduct[] { costDeduct.WaterControlCostDeduct }, ref tran, true);
                    else
                        _waterControlCostDeductBLL.Add(new WaterControlCostDeduct[] { costDeduct.WaterControlCostDeduct }, ref tran, true);
                }
                var costDeductTemp = _costDeductBLL.GetEntityById(costDeduct.Id);
                if (costDeductTemp != null) _costDeductBLL.Save(new CostDeduct[] { costDeduct }, ref tran, true);
                else _costDeductBLL.Add(_viewOpenFundDetails, new CostDeduct[] { costDeduct }, _preCostDeductEquipmentOpenFunds, ref tran, true, _isSaveEquipmentOpenFund);

                DeductHandle(ref tran, costDeduct, costDeductTemp, costDeductTemp == null);
                BusinessHandleAfterDeduct(ref tran);
                if (!isSupress) tran.CommitTransaction();
                SendDeductMessage(costDeduct.UserAccount, out errorMessage);
                return new object[] { costDeduct };
            }
            catch(Exception ex)
            {
                if (!isSupress && tran!= null ) tran.RollbackTransaction();
                if (ex is CheckMoneyFailException) throw new CheckMoneyFailException(ex.Message);
                throw;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
        }
        /// <summary> 撤销扣费
        /// 彻底删除扣费记录
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        public override object[] CancelDeduct(ref XTransaction tran)
        {
            CostDeduct costDeduct = null;
            UserAccount userAccount = null;
            string errorMessage = "";
            Guid userId = Guid.Empty;
            Guid subjectId = Guid.Empty;
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            bool isSupress = tran != null;
            try
            {
                costDeduct = BusinessHandle(out subjectId, out userId, out paymentMethod);
                if (costDeduct == null) throw new Exception("出错,找不到扣费记录信息");
                if (costDeduct.HasClossingAccount) throw new Exception("已结算");
                _costDeductBLL.Delete(new Guid[] { costDeduct.Id }, ref tran,_operatorId,_operateIP, true, _isSaveEquipmentOpenFund);
                if (costDeduct.ManualCostDeduct != null)
                {
                    _manualCostDeductBLL.Delete(new Guid[] { costDeduct.ManualCostDeduct.Id }, ref tran, true);
                }
               
                if (costDeduct.AnimalCostDeduct != null)
                {
                    _animalCostDeductBLL.Delete(new Guid[] { costDeduct.AnimalCostDeduct.Id }, ref tran, true);
                }

                if (costDeduct.WaterControlCostDeduct != null)
                {
                    _waterControlCostDeductBLL.Delete(new Guid[] { costDeduct.WaterControlCostDeduct.Id }, ref tran, true);
                }
                userAccount = _userAccountBLL.CancelDeduct(userId, subjectId, paymentMethod, costDeduct.RealCurrency.Value, costDeduct.VirtualCurrency.Value, ref tran, out errorMessage, !isSupress, _isDeductSubjectHook);
                CancelDeductHandle(ref tran, userAccount);
                if (!isSupress) tran.CommitTransaction();
                return new object[] { userAccount };
            }
            catch(Exception ex)
            {
                if (!isSupress && tran != null) tran.RollbackTransaction();
                throw;
            }
            finally { if (tran != null && !isSupress) tran.Dispose(); }
        }
    }
}
