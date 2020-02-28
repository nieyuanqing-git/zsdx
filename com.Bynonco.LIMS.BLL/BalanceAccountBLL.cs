using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.View;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using System.Data;
using com.Bynonco.LIMS.DAL;

namespace com.Bynonco.LIMS.BLL
{
    public class BalanceAccountBLL : BLLBase<BalanceAccount>, IBalanceAccountBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        private ICostDeductBLL __costDeductBLL;
        private IDepositRecordBLL __depositRecordBLL;
        private IUserAccountBLL __userAccountBLL;
        private IBalanceAccountItemBLL __balanceAccountItemBLL;
        private IUsedConfirmBLL __usedConfirmBLL;
        private IManualCostDeductBLL __manualCostDeductBLL;
        private IAppointmentBLL __appointmentBLL;
        private ISampleApplyBLL __sampleApplyBLL;
        private ICostDeductEquipmentOpenFundBLL __costDeductEquipmentOpenFundBLL;
        public BalanceAccountBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __depositRecordBLL = BLLFactory.CreateDepositRecordBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __balanceAccountItemBLL = BLLFactory.CreateBalanceAccountItemBLL();
            __costDeductBLL = BLLFactory.CreateCostDeductBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __manualCostDeductBLL = BLLFactory.CreateManualCostDeductBLL();
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __sampleApplyBLL = BLLFactory.CreateSampleApplyBLL();
            __costDeductEquipmentOpenFundBLL = BLLFactory.CreateCostDeductEquipmentOpenFundBLL();
        }
        private IList<ViewCostDeduct> GetPayerViewCostDeductList(User user, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            var viewCostDeductBLL = BLLFactory.CreateViewCostDeductBLL();

            var viewCostDeductList = viewCostDeductBLL.GetViewCostDeductListByExpression(user.Id, dataGridSettings, mapping, "PayerId", isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, false, null, true, false);
            return viewCostDeductList;
        }
        public IList<PayerBalanceTotal> GetPayerBalanceTotalList(User user, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            dataGridSettings.PageIndex = 1;
            dataGridSettings.PageSize = 1000000000;
            var gridPayerUnCloseBalanceTotal = GetGridPayerBalanceTotalList(user, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            return gridPayerUnCloseBalanceTotal != null && gridPayerUnCloseBalanceTotal.Data != null ? gridPayerUnCloseBalanceTotal.Data : null;
        }
        public GridData<PayerBalanceTotal> GetGridPayerBalanceTotalList(User user, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            double discount = 0.5;
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=\"EquipmentOpenFundDiscount\"");
            if (dictCodeType != null && (dictCodeType.Value.IsInt() || dictCodeType.Value.Trim() == "0"))
                discount = double.Parse(dictCodeType.Value) / 100;
            GridData<PayerBalanceTotal> gridData = new GridData<PayerBalanceTotal>();
            var viewCostDeductBLL = BLLFactory.CreateViewCostDeductBLL();
            var queryExpression = viewCostDeductBLL.GenerateQueryExpression(user.Id, dataGridSettings.QueryExpression);
            Dictionary<string, string> viewCostDeductMapping = new Dictionary<string, string>();
            //viewCostDeductMapping["Id"] = "CostDeductId";
            viewCostDeductMapping["Id"] = "ViewCostDeduct.CostDeductId";
            var sqlStr = viewCostDeductBLL.ConverExpressionToSql(queryExpression, viewCostDeductMapping);
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParameter = new List<IDataParameter>()
            {
                 DataParameterFactory.CreateDataParameter("queryExpression",sqlStr, ParameterDirection.Input),
                 DataParameterFactory.CreateDataParameter("orderExpression",dataGridSettings.GetOrderExpression(), ParameterDirection.Input),
                 DataParameterFactory.CreateDataParameter("pageIndex",dataGridSettings.PageIndex, ParameterDirection.Input),
                 DataParameterFactory.CreateDataParameter("pageCount",dataGridSettings.PageSize, ParameterDirection.Input),
                 returnValueDataParameter
            };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("proGetPayerBalanceTotal", dataParameter).Result;
            IList<PayerBalanceTotal> lstPayerBalanceTotal = new List<PayerBalanceTotal>();
            var balanceAccountPrivilige = PriviligeFactory.GetBalanceAccountPrivilige(user.Id);
            foreach (var result in results)
            {
                PayerBalanceTotal payerBalanceTotal = new PayerBalanceTotal();
                payerBalanceTotal.PayerId = new Guid(result[0].ToString());
                payerBalanceTotal.PayerName = result[1].ToString();
                payerBalanceTotal.SubjectId = new Guid(result[2].ToString());
                payerBalanceTotal.SubjectName = result[3].ToString();
                payerBalanceTotal.CostRealFee = Convert.ToDouble(result[4]);
                payerBalanceTotal.CostVirtualFee = Convert.ToDouble(result[5]);
                payerBalanceTotal.CostOpenFundRealFee = Convert.ToDouble(result[6]);
                payerBalanceTotal.CostSubsidiesFee = Convert.ToDouble(result[7]);
                payerBalanceTotal.CostFirstHalfFee = Convert.ToDouble(result[8]) / (discount > 0 ? discount : 1);
                payerBalanceTotal.CostSecondHalfFee = Convert.ToDouble(result[9]) / (discount > 0 ? discount : 1);
                lstPayerBalanceTotal.Add(payerBalanceTotal);
                payerBalanceTotal.BuildOperate();
                if (balanceAccountPrivilige == null || !balanceAccountPrivilige.IsEnableViewUnCloseBalanceTotalDetail)
                {
                    payerBalanceTotal.ViewBalanceTotalDetailOperate = "";
                }
            }
            gridData.Data = lstPayerBalanceTotal;
            gridData.Count = Convert.ToInt32(returnValueDataParameter.Value);
            return gridData;
        }
        public void Settle(User creator, DataGridSettings dataGridSettings, DateTime? beginDate, DateTime? endDate, string remark)
        {
            var viewCostDeductBLL = BLLFactory.CreateViewCostDeductBLL();

            XTransaction tran = null;
            try
            {
                double discount = 0.5;
                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=\"EquipmentOpenFundDiscount\"");
                if (dictCodeType != null && (dictCodeType.Value.IsInt() || dictCodeType.Value.Trim() == "0"))
                    discount = double.Parse(dictCodeType.Value) / 100;

                dataGridSettings.AppendAndQueryExpression("HasClossingAccount=false");
                var viewCostDeductList = GetPayerViewCostDeductList(creator, dataGridSettings, null, "", false, false, true, new string[] { "EquipmentAdminId" });
                if (viewCostDeductList == null || viewCostDeductList.Count == 0) throw new Exception("找不到扣费信息");

                tran = SessionManager.Instance.BeginTransaction();
                var payerIds = viewCostDeductList.Select(p => p.PayerId.Value).Distinct();
                var totalPerson = viewCostDeductList.Select(p => p.UserId).Distinct().Count();
                var totalRealCurrency = viewCostDeductList.Sum(p => p.RealCurrency != null ? p.RealCurrency.Value : 0);
                var totalVirtualCurrency = viewCostDeductList.Sum(p => p.VirtualCurrency != null ? p.VirtualCurrency.Value : 0);
                var totalSubsidiesCurrency = viewCostDeductList.Sum(p => p.OpenFundCurrency);
                var totalMoney = totalRealCurrency + totalVirtualCurrency + totalSubsidiesCurrency;
                if (!beginDate.HasValue) beginDate = viewCostDeductList.Min(p => p.DeductAt != null ? p.DeductAt.Value : new DateTime());
                if (!endDate.HasValue) endDate = viewCostDeductList.Max(p => p.DeductAt != null ? p.DeductAt.Value : new DateTime());
                BalanceAccount balanceAccount = new BalanceAccount();
                balanceAccount.Id = Guid.NewGuid();
                balanceAccount.BeginAt = beginDate.Value;
                balanceAccount.EndAt = endDate.Value;
                balanceAccount.CreateAt = DateTime.Now;
                balanceAccount.CreatorId = creator.Id;
                balanceAccount.TotalMoney = totalMoney;
                balanceAccount.TotalPersons = totalPerson;
                balanceAccount.TotalRealCurrency = totalRealCurrency;
                balanceAccount.TotalVirtualCurrency = totalVirtualCurrency;
                balanceAccount.TotalSubsidiesCurrency = totalSubsidiesCurrency;
                balanceAccount.BalanceAccountTitle = remark;
                Add(new BalanceAccount[] { balanceAccount }, ref tran, true);

                foreach (var payerId in payerIds)
                {
                    var payerViewCostDeductList = viewCostDeductList.Where(p => p.PayerId == payerId);
                    BalanceAccountItem balanceAccountItem = new BalanceAccountItem() { Id = Guid.NewGuid(), PayerId = payerId };
                    balanceAccountItem.BalanceAccountId = balanceAccount.Id;
                    balanceAccountItem.TotalTimes = viewCostDeductList.Count(p => p.PayerId == payerId);
                    balanceAccountItem.TotalRealCurrency = payerViewCostDeductList.Sum(p => p.RealCurrency != null ? p.RealCurrency.Value : 0);
                    balanceAccountItem.TotalVirtualCurrency = payerViewCostDeductList.Sum(p => p.VirtualCurrency != null ? p.VirtualCurrency.Value : 0);
                    balanceAccountItem.TotalSubsidiesCurrency = payerViewCostDeductList.Sum(p => p.OpenFundCurrency);
                    balanceAccountItem.TotalMoney = balanceAccountItem.TotalVirtualCurrency + balanceAccountItem.TotalRealCurrency + balanceAccountItem.TotalSubsidiesCurrency;
                    balanceAccountItem.TotalDuration = payerViewCostDeductList.Sum(p => p.Duration != null ? p.Duration.Value : 0);
                    balanceAccountItem.TotalCalcDuration = payerViewCostDeductList.Sum(p => p.CalcDuration != null ? p.CalcDuration.Value : 0);

                    balanceAccountItem.TotalGeneralRealCurrency = payerViewCostDeductList.Where(p => p.IsOpenFundCostDeduct == null || !p.IsOpenFundCostDeduct.Value).Sum(p => p.RealCurrency != null ? p.RealCurrency.Value : 0);
                    balanceAccountItem.TotalOpenFundRealCurrency = payerViewCostDeductList.Where(p => p.IsOpenFundCostDeduct != null && p.IsOpenFundCostDeduct.Value).Sum(p => p.RealCurrency != null ? p.RealCurrency.Value : 0);

                    var expression = viewCostDeductBLL.GenerateQueryExpression(creator.Id, dataGridSettings.QueryExpression, null, true);
                    var sqlstr = viewCostDeductBLL.ConverExpressionToSql(expression, null);
                    var sql =
                        string.Format(
                            "select * from CostDeductEquipmentOpenFund where CostDeductId in (select CostDeductId from ViewCostDeduct where 1=1 {0} )",
                            string.IsNullOrEmpty(sqlstr) ? "" : " and " + sqlstr);
                    var costDeductEquipmentOpenFundList = __costDeductEquipmentOpenFundBLL.GetEntitiesBySql(sql);
                    //foreach (var viewCostDeduct in payerViewCostDeductList)
                    //{
                    //    __costDeductEquipmentOpenFundBLL.GetEntitiesBySql("select * from CostDeductEquipmentOpenFund where CostDeductId in ()")
                    //}
                    if (costDeductEquipmentOpenFundList != null && costDeductEquipmentOpenFundList.Count() > 0)
                    {
                        // chao: 开放基金分上下学期扣费
                        balanceAccountItem.TotalFirstHalfOpenFundCurrency = costDeductEquipmentOpenFundList.Where(p => p.OpenFundApply != null && p.OpenFundApply.CreateTime.Month < 7).Sum(p => p.CalFee) / (discount > 0 ? discount : 1);
                        balanceAccountItem.TotalSecondHalfOpenFundCurrency = costDeductEquipmentOpenFundList.Where(p => p.OpenFundApply != null && p.OpenFundApply.CreateTime.Month >= 7).Sum(p => p.CalFee) / (discount > 0 ? discount : 1);
                    }
                    __balanceAccountItemBLL.Add(new BalanceAccountItem[] { balanceAccountItem }, ref tran, true);

                    foreach (var viewCostDeduct in payerViewCostDeductList)
                    {
                        var costDeduct = __costDeductBLL.GetEntityById(viewCostDeduct.Id);
                        costDeduct.HasClossingAccount = true;
                        costDeduct.BalanceAccountItemId = balanceAccountItem.Id;
                        __costDeductBLL.Save(new CostDeduct[] { costDeduct }, ref tran, true);
                        switch (costDeduct.CostDeductTypeEnum)
                        {
                            case Model.Enum.CostDeductType.ManualCostDeduct:
                                if (costDeduct.ManualCostDeductId.HasValue)
                                {
                                    var manualCostDeduct = __manualCostDeductBLL.GetEntityById(costDeduct.ManualCostDeductId.Value);
                                    manualCostDeduct.Status = (int)ManualCostDeductStatus.CloseAccount;
                                    __manualCostDeductBLL.Save(new ManualCostDeduct[] { manualCostDeduct }, ref tran, true);
                                }
                                break;
                            case Model.Enum.CostDeductType.UsedCostDeduct:
                                if (costDeduct.UsedConfirmId.HasValue)
                                {
                                    var usedConfirm = __usedConfirmBLL.GetEntityById(costDeduct.UsedConfirmId.Value);
                                    usedConfirm.Status = (int)UsedConfirmStatus.ClosingAccount;
                                    __usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, true);
                                }
                                break;
                            case Model.Enum.CostDeductType.AppointmentPredictCostDeduct:
                                if (costDeduct.AppointmentId.HasValue)
                                {
                                    var appointment = __appointmentBLL.GetEntityById(costDeduct.AppointmentId.Value);
                                    appointment.HasClossingAccount = true;
                                    __appointmentBLL.Save(new Appointment[] { appointment }, ref tran, true);
                                }
                                break;
                            case Model.Enum.CostDeductType.SampleCostDeduct:
                                if (costDeduct.SampleApplyId.HasValue)
                                {
                                    var sampleApply = __sampleApplyBLL.GetEntityById(costDeduct.SampleApplyId.Value);
                                    sampleApply.HasClossingAccount = true;
                                    __sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                                }
                                break;
                        }

                    }
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                throw;
            }
            finally { if (tran != null) tran.Dispose(); }
        }

        public void UnSettle(Guid[] ids)
        {
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();

                var balances = GetEntitiesByExpression(string.Join("+", ids.Select(x => string.Format("Id=\"{0}\"", x)))) ?? new List<BalanceAccount>();
                var confirmBalances = balances.Where(x => x.HasConfirm).ToList();
                if (confirmBalances.Count > 0)
                {
                    throw new Exception(string.Format("下列结算项目：{0} 已经确认，不能删除!", string.Join(",", confirmBalances.Select(x => x.BalanceAccountTitle))));
                }
                foreach (var account in balances)
                {
                    var items = __balanceAccountItemBLL.GetEntitiesByExpression(
                         string.Format("BalanceAccountId=\"{0}\"", account.Id)) ?? new List<BalanceAccountItem>();
                    foreach (var item in items)
                    {
                        var costDeducts = __costDeductBLL.GetEntitiesByExpression(new DataGridSettings()
                        {
                            QueryExpression = string.Format("BalanceAccountItemId=\"{0}\" ", item.Id)
                        }
                            ) ?? new List<CostDeduct>();
                        foreach (var costDeduct in costDeducts)
                        {
                            costDeduct.HasClossingAccount = false;
                            costDeduct.BalanceAccountItemId = null;
                            __costDeductBLL.Save(new CostDeduct[] { costDeduct }, ref tran, true, false);
                            switch (costDeduct.CostDeductTypeEnum)
                            {
                                case Model.Enum.CostDeductType.ManualCostDeduct:
                                    if (costDeduct.ManualCostDeductId.HasValue)
                                    {
                                        var manualCostDeduct = __manualCostDeductBLL.GetEntityById(costDeduct.ManualCostDeductId.Value);
                                        manualCostDeduct.Status = (int)ManualCostDeductStatus.Deducted;
                                        __manualCostDeductBLL.Save(new ManualCostDeduct[] { manualCostDeduct }, ref tran, true);
                                    }
                                    break;
                                case Model.Enum.CostDeductType.UsedCostDeduct:
                                    if (costDeduct.UsedConfirmId.HasValue)
                                    {
                                        var usedConfirm = __usedConfirmBLL.GetEntityById(costDeduct.UsedConfirmId.Value);
                                        usedConfirm.Status = (int)UsedConfirmStatus.Deduct;
                                        __usedConfirmBLL.Save(new UsedConfirm[] { usedConfirm }, ref tran, true);
                                    }
                                    break;
                                case Model.Enum.CostDeductType.AppointmentPredictCostDeduct:
                                    if (costDeduct.AppointmentId.HasValue)
                                    {
                                        var appointment = __appointmentBLL.GetEntityById(costDeduct.AppointmentId.Value);
                                        appointment.HasClossingAccount = false;
                                        __appointmentBLL.Save(new Appointment[] { appointment }, ref tran, true);
                                    }
                                    break;
                                case Model.Enum.CostDeductType.SampleCostDeduct:
                                    if (costDeduct.SampleApplyId.HasValue)
                                    {
                                        var sampleApply = __sampleApplyBLL.GetEntityById(costDeduct.SampleApplyId.Value);
                                        sampleApply.HasClossingAccount = false;
                                        __sampleApplyBLL.Save(new SampleApply[] { sampleApply }, ref tran, true);
                                    }
                                    break;
                            }
                        }
                    }
                    if (items.Count > 0)
                        __balanceAccountItemBLL.Delete(items.Select(x => x.id), ref tran, true);
                }

                if (balances.Count > 0)
                    Delete(balances.Select(x => x.id), ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                throw;
            }
            finally { if (tran != null) tran.Dispose(); }
        }

        public void SettleConfirm(Guid[] ids)
        {
            XTransaction tran = null;
            try
            {
                tran = SessionManager.Instance.BeginTransaction();

                var balances = GetEntitiesByExpression(string.Join("+", ids.Select(x => string.Format("Id=\"{0}\"", x)))) ?? new List<BalanceAccount>();

                foreach (var account in balances)
                {
                    account.HasConfirm = true;
                }
                if (balances.Count > 0)
                    Save(balances, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                throw;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
    }
}