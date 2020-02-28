using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL.View;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class UserAccountBLL : BLLBase<UserAccount>, IUserAccountBLL
    {
        private ISubjectBLL __subjectBLL;
        private IUserBLL __userBLL;
        private IExperimenterSubjectBLL __experimenterSujectBLL;
        public UserAccountBLL()
        {
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __experimenterSujectBLL = BLLFactory.CreateExperimenterSubjectBLL();
        }
        public UserAccount GetUserAccountByUserId(Guid userId)
        {
            var userAccountList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsDelete=false", userId.ToString()));
            return userAccountList != null && userAccountList.Count > 0 ? userAccountList.First() : null;
        }
        public bool GetUserAccountBalanceByLabel(string label,string tutorName ,out string retrunMsg)
        {
            retrunMsg = "";
            var userList = __userBLL.GetEntitiesByExpression(string.Format("Label=\"{0}\"*IsDel=false", label));
            if (userList == null || userList.Count==0)
            {
                retrunMsg = string.Format("找不到Label为【{0}】的用户信息", label);
                return false;
            }
            if (userList.Count > 1)
            {
                retrunMsg = string.Format("存在【{0}】个Label为【{1}】的用户信息", userList.Count, label);
                return false;
            }
            var user = userList.First();
            if (user.TutorId.HasValue)//导师
            {
                 var experimenterSubject = __experimenterSujectBLL.GetFirstOrDefaultEntityByExpression(string.Format("UseMoneyType={0}*ExperimenterId=\"{1}\"*OwnerId=\"{2}\"*IsDelete=false", 
                    (int)ExperimenterSubjectUseMoneyType.Assign, user.Id, user.TutorId.Value));
                if (experimenterSubject != null)
                {
                    retrunMsg = string.Format("分配经费余额:{0}", experimenterSubject.OddSum.ToString("0.00"));
                }
            }
            var tutorId = user.TutorId.HasValue ? user.TutorId.Value : user.Id;
            var userAccountList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsDelete=false", tutorId));
            if (userAccountList == null || userAccountList.Count == 0)
            {
                retrunMsg = string.Format("找不到用户编码为【{0}】的账户信息", tutorId);
                return false;
            }
            if (userAccountList.Count > 1)
            {
                retrunMsg = string.Format("存在【{0}】个用户编码为【{1}】的账户信息", userAccountList.Count, tutorId);
                return false;
            }
            var userAccount = userAccountList.First();
            retrunMsg = string.Format("账户总金额:{0},其中真实币:{1},虚拟币{2}", userAccount.TotalCurrency.ToString("0.00"), userAccount.RealCurrency.ToString("0.00"), userAccount.VirtualCurrency.ToString("0.00"));
            return true;
        }
        /// <summary> 扣费 </summary>
        /// <param name="isEquipmentOpenFundDeduct"></param>
        /// <param name="userId"></param>
        /// <param name="subjectId"></param>
        /// <param name="paymentMethod"></param>
        /// <param name="fee"></param>
        /// <param name="deductRealCurrency"></param>
        /// <param name="deductVirtualCurrency"></param>
        /// <param name="tran"></param>
        /// <param name="errorMessage"></param>
        /// <param name="saveImmediately"></param>
        /// <param name="isDeductSubject"></param>
        /// <returns></returns>
        public UserAccount Deduct(bool isEquipmentOpenFundDeduct,Guid userId, Guid subjectId, PaymentMethod paymentMethod, double fee, out double deductRealCurrency, out double deductVirtualCurrency, ref XTransaction tran, out string errorMessage, bool saveImmediately = true, bool isDeductSubject = true)
        {
            errorMessage = "";
            deductVirtualCurrency = 0d;
            deductRealCurrency = 0d;
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                UserAccount userAccount;
                if (isDeductSubject) userAccount = __subjectBLL.Deduct(userId, subjectId, paymentMethod, fee, ref tran);
                else userAccount = __subjectBLL.GetEntityById(subjectId).Director.UserAccount;
                if (!isEquipmentOpenFundDeduct)
                {
                    deductVirtualCurrency = fee - userAccount.VirtualCurrency;
                    var curVirtualCurrecy = userAccount.VirtualCurrency;
                    if (deductVirtualCurrency > 0)
                    {
                        deductRealCurrency = deductVirtualCurrency;
                        userAccount.RealCurrency -= deductVirtualCurrency;
                        deductVirtualCurrency = curVirtualCurrecy;
                        userAccount.VirtualCurrency = 0d;
                    }
                    else
                    {
                        deductVirtualCurrency = fee;
                        userAccount.VirtualCurrency -= fee;
                    }
                }
                else
                {
                    deductVirtualCurrency = 0d;
                    deductRealCurrency = fee;
                    userAccount.RealCurrency -= fee;
                }
                if (saveImmediately) Save(new UserAccount[] { userAccount }, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
                return userAccount;
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                return null;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public UserAccount CancelDeduct(Guid userId, Guid subjectId, PaymentMethod paymentMethod,double cancelRealCurrency, double cancelVirtualCurrency, ref XTransaction tran, out string errorMessage, bool saveImmediately = true,bool isDeductSubject = true)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            UserAccount userAccount = null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (isDeductSubject) userAccount = __subjectBLL.Deduct(userId, subjectId, paymentMethod, (cancelRealCurrency + cancelVirtualCurrency) * -1, ref tran);
                else userAccount = __subjectBLL.GetEntityById(subjectId).Director.UserAccount;
                userAccount.RealCurrency += cancelRealCurrency;
                userAccount.VirtualCurrency += cancelVirtualCurrency;
                if (saveImmediately) Save(new UserAccount[] { userAccount }, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
                return userAccount;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress) tran.RollbackTransaction();
                return null;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public IList<UserAccountRecord> GetUserAccountRecordList(DataGridSettings dataGridSettings, Guid userAccountId, DateTime? startAt, DateTime? endAt, out int returnCount)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            returnCount = 0;
            int pageIndex = dataGridSettings.PageIndex;
            int pageCount = dataGridSettings.PageSize ;
            IList<UserAccountRecord> userAccountRecordList = new List<UserAccountRecord>();
            List<IDataParameter> dataParams = new List<IDataParameter>();

            //wuzewu
            // var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, null);
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", string.IsNullOrWhiteSpace(dataGridSettings.QueryExpression) ? "" : dataGridSettings.QueryExpression));

            dataParams.Add(DataParameterFactory.CreateDataParameter("userAccountId", userAccountId));
            if (!string.IsNullOrWhiteSpace(dataGridSettings.SortColumn))
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", dataGridSettings.SortColumn));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr() == "desc" ? 1 : 0));
            }
            else
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", DBNull.Value));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", DBNull.Value));
            }
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", pageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", pageCount));
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetUserAccountRecordList", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var id = new Guid(result[0].ToString());
                    DateTime? operateTime = null;
                    if(result[1] != DBNull.Value ) operateTime = Convert.ToDateTime(result[1]);
                    var title = result[2] == DBNull.Value ? "" : result[2].ToString();
                    var realCurrency = Convert.ToDouble(result[3] == DBNull.Value ? 0 : result[3]);
                    var virtualCurrency = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                    var subsidyCurrency = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                    var userAccountRecordType = Convert.ToInt32(result[6] == DBNull.Value ? -1 : result[6]);

                    UserAccountRecord userAccountRecord = new UserAccountRecord()
                    {
                        Id = id,
                        OperateTime = operateTime,
                        TitleName = title,
                        RealCurrency = realCurrency,
                        VirtualCurrency = virtualCurrency,
                        SubsidyCurrency = subsidyCurrency,
                        UserAccountRecordType = userAccountRecordType
                    };
                    userAccountRecordList.Add(userAccountRecord);
                    returnCount = Convert.ToInt32(returnValueDataParameter.Value);
                }
            }
            return userAccountRecordList;
        }
        public GridData<UserAccountRecord> GetGridUserAccountRecordList(DataGridSettings dataGridSettings, Guid userAccountId, DateTime? startAt, DateTime? endAt)
        {
            int count = 0;
            var userAccountRecordList = GetUserAccountRecordList(dataGridSettings, userAccountId, startAt, endAt, out count);
            return new GridData<UserAccountRecord>()
            {
                Data = userAccountRecordList,
                Count = count
            };
        }
        public UserAccountRecord GetUserAccountRecordTotal(Guid userAccountId, DateTime? startAt, DateTime? endAt)
        {
            UserAccountRecord userAccountRecord = null;
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("userAccountId", userAccountId));
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetUserAccountRecordTotal", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                var result = results[0];
                Guid id = Guid.Empty;
                DateTime? operateTime = null;
                if (result[1] != DBNull.Value) operateTime = Convert.ToDateTime(result[1]);
                var title = result[2] == DBNull.Value ? "" : result[2].ToString();
                var realCurrency = Convert.ToDouble(result[3] == DBNull.Value ? 0 : result[3]);
                var virtualCurrency = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                var subsidyCurrency = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                var userAccountRecordType = Convert.ToInt32(result[6] == DBNull.Value ? 0 : result[6]);
                var depositRecordRealCurrency = Convert.ToInt32(result[7] == DBNull.Value ? 0 : result[7]);
                var depositRecordVirtualCurrency = Convert.ToInt32(result[8] == DBNull.Value ? 0 : result[8]);
                var openFundRealCurrency = Convert.ToInt32(result[9] == DBNull.Value ? 0 : result[9]);
                var openFundSubsidyCurrency = Convert.ToInt32(result[10] == DBNull.Value ? 0 : result[10]);
                var materialRealCurrency = Convert.ToInt32(result[11] == DBNull.Value ? 0 : result[11]);

                var outlayRealCurrency = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                var outlayVirtualCurrency = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                var outlaySubsidyCurrency = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                userAccountRecord = new UserAccountRecord()
                {
                    Id = id,
                    OperateTime = operateTime,
                    TitleName = title,
                    RealCurrency = realCurrency,
                    VirtualCurrency = virtualCurrency,
                    SubsidyCurrency = subsidyCurrency,
                    UserAccountRecordType = userAccountRecordType,
                    DepositRecordRealCurrency = depositRecordRealCurrency,
                    DepositRecordVirtualCurrency = depositRecordVirtualCurrency,
                    OpenFundRealCurrency = openFundRealCurrency,
                    OpenFundSubsidyCurrency = openFundSubsidyCurrency,
                    MaterialRealCurrency = materialRealCurrency,
                    OutlayRealCurrency = 0 - outlayRealCurrency,
                    OutlayVirtualCurrency = 0 - outlayVirtualCurrency,
                    OutlaySubsidyCurrency = 0 - outlaySubsidyCurrency
                };
            }
            ZsdxAccountBLL bll = new ZsdxAccountBLL();
            try
            {
                var userAcc = GetEntityById(userAccountId);
                if (userAcc != null)
                {
                    var accInfo = bll.GetAccount(userAcc.GetUser().Label) as XjDsAccountResult;
                    if(accInfo!=null)
                    {
                        userAccountRecord.XjCurrencyStr = accInfo.balance.ToString();
                        userAccountRecord.XjDebtStr = accInfo.outstanding.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return userAccountRecord;
        }

    }
}