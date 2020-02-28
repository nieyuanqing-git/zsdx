using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class MaterialUserAccountBLL : BLLBase<MaterialUserAccount>, IMaterialUserAccountBLL
    {
        private IMaterialDepositRecordBLL __materialDepositRecordBLL;
        private IUserAccountBLL __userAccountBLL;
        public MaterialUserAccountBLL()
        {
            __materialDepositRecordBLL = BLLFactory.CreateMaterialDepositRecordBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
        }
        private bool DelMaterialUserAccount(Guid userId, Guid materialUserAccountId, ref XTransaction tran, out string errorMessage)
        {
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                 errorMessage = "";
                MaterialUserAccount materialUserAccount = GetEntityById(materialUserAccountId);;
                if (materialUserAccount == null)  throw new Exception("出错,找不到经费存款记录");
                IList<MaterialDepositRecord> materialDepositRecordList = __materialDepositRecordBLL.GetEntitiesByExpression(string.Format("MaterialUserAccountId=\"{0}\"*IsDelete=false", materialUserAccount.Id)); ;
                if (materialDepositRecordList != null 
                    && materialDepositRecordList.Count() > 0 
                    && materialDepositRecordList.Where(p => p.OperateType == (int)MaterialDepositRecordType.Output).Count() > 0)
                {
                    throw new Exception("出错,该经费账户存在经费转入用户账户记录,无法删除");
                }
                materialUserAccount.IsDelete = true;
                if (materialDepositRecordList != null && materialDepositRecordList.Count() > 0)
                {
                    foreach (var item in materialDepositRecordList)
                    {
                        item.IsDelete = true;
                    }
                    __materialDepositRecordBLL.Save(materialDepositRecordList, ref tran, true);
                }
                Save(new MaterialUserAccount[] { materialUserAccount }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool DelMaterialUserAccount(Guid userId, Guid materialUserAccountId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialUserAccount(userId, materialUserAccountId, ref tran, out errorMessage);
        }
        public bool DelMaterialUserAccounts(Guid userId, IList<MaterialUserAccount> materialUserAccounts, out  string errorMessage)
        {
            errorMessage = "";
            if (materialUserAccounts != null && materialUserAccounts.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialUserAccount in materialUserAccounts)
                    {
                        var result = DelMaterialUserAccount(userId, materialUserAccount.Id, ref tran, out errorMessage);
                        if (!result)
                            throw new Exception(errorMessage);
                    }
                    if (tran.HasTransaction) tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (tran.HasTransaction) tran.CommitTransaction();
                    errorMessage = ex.Message;
                    return false;
                }
                finally { tran.Dispose(); }
            }
            return true;
        }
        public bool InputMoney(Guid userId, Guid materialUserAccountId, double current, string remark, out string errorMessage, Guid? businessId = null, MaterialDepositRecordBusinessType? businessType = null)
        {
            errorMessage = "";
            if (remark == null) remark = "";
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                errorMessage = "";
                MaterialUserAccount materialUserAccount = GetEntityById(materialUserAccountId); ;
                if (materialUserAccount == null) throw new Exception("出错,找不到经费存款记录");
                materialUserAccount.Currency += current;
                var materialDepositRecord = new MaterialDepositRecord();
                materialDepositRecord.Id = Guid.NewGuid();
                materialDepositRecord.MaterialUserAccountId = materialUserAccount.id;
                materialDepositRecord.DepositSum = current;
                materialDepositRecord.Remark = string.Format("经费账户存入金额【{0}】.{1}", current, remark);
                materialDepositRecord.OperatorId = userId;
                materialDepositRecord.OperateTime = DateTime.Now;
                materialDepositRecord.OperateType = (int)MaterialDepositRecordType.Input;
                materialDepositRecord.IsDelete = false;
                if (businessId.HasValue && businessType.HasValue)
                {
                    materialDepositRecord.BusinessId = businessId.Value;
                    materialDepositRecord.BusinessType = (int)businessType.Value;
                    switch (businessType.Value)
                    {
                        case MaterialDepositRecordBusinessType.MaterialPurchaseBalance:
                            var materialPurchaseBLL = BLLFactory.CreateMaterialPurchaseBLL();
                            var materialPurchase = materialPurchaseBLL.GetEntityById(businessId.Value);
                            if (materialPurchase != null)
                            {
                                remark += (remark == "" ? "" : "  ")
                                    + string.Format("经费账户【{0}】结算金额【{1}】", materialUserAccount.Name, current);
                                materialPurchase.Status = (int)MaterialPurchaseStatus.BalancePass;
                                materialPurchase.AuditBalanceId = userId;
                                materialPurchase.AuditBalanceTime = DateTime.Now;
                                materialPurchase.AuditBalanceRemark = remark;
                                materialPurchase.UpdateUserId = userId;
                                materialPurchase.UpdateTime = DateTime.Now;
                                materialPurchaseBLL.Save(new MaterialPurchase[] { materialPurchase }, ref tran, true);
                            }
                            break;
                    }
                }
                __materialDepositRecordBLL.Add(new MaterialDepositRecord[] { materialDepositRecord }, ref tran, true);
                Save(new MaterialUserAccount[] { materialUserAccount }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
        public bool OutputMoney(Guid userId, Guid materialUserAccountId, double current, out string errorMessage)
        {
            errorMessage = ""; 
            var tran = SessionManager.Instance.BeginTransaction();
            try
            {
                errorMessage = "";
                MaterialUserAccount materialUserAccount = GetEntityById(materialUserAccountId); ;
                if (materialUserAccount == null) throw new Exception("出错,找不到经费存款记录");
                if(current <= 0) throw new Exception("出错,经费账户转入用户账户金额必须大于0");
                if (materialUserAccount.Currency < current) throw new Exception(string.Format("出错,经费账户余款小于【{0}】,无法转入用户账户",Math.Round(current,2).ToString()));
                var userAccount = __userAccountBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"", materialUserAccount.UserId));
                if (userAccount == null) throw new Exception("出错,找不到要转入的用户账户,请联系管理员.");
                materialUserAccount.Currency -= current;
                var materialDepositRecord = new MaterialDepositRecord();
                materialDepositRecord.Id = Guid.NewGuid();
                materialDepositRecord.MaterialUserAccountId = materialUserAccount.id;
                materialDepositRecord.DepositSum = 0 - current;
	            materialDepositRecord.Remark = string.Format("经费账户转入用户账户金额【{0}】",current);
	            materialDepositRecord.OperatorId = userId;
	            materialDepositRecord.OperateTime = DateTime.Now;
	            materialDepositRecord.OperateType = (int)MaterialDepositRecordType.Output;
                materialDepositRecord.IsDelete = false;
                __materialDepositRecordBLL.Add(new MaterialDepositRecord[] { materialDepositRecord }, ref tran, true);
                Save(new MaterialUserAccount[] { materialUserAccount }, ref tran, true);
                userAccount.RealCurrency += current;
                __userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
    }
}