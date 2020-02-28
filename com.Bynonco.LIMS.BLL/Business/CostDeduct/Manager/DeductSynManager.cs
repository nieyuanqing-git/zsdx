using System;
using System.Collections.Generic;
using System.Linq;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class DeductSynManager
    {
        public void DeductSyn(out string errorMessage)
        {

            var zsdxAccountBLL = new ZsdxAccountBLL();
            var costDeductBll = BLLFactory.CreateCostDeductBLL();
            var costDeductSynBLL = BLLFactory.CreateCostDeductSynBLL();
            var userAccountBLL = BLLFactory.CreateUserAccountBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            var deducts = costDeductSynBLL.GetEntitiesByExpression("IsSyned=false");
            var delDeductDatas = new List<DelDeductData>();
            var deductDatas = new List<DeductData>();
            var costs = new List<CostDeduct>();
            // var userAccounts = new List<UserAccount>();
            var costDeductSyns = new List<CostDeductSyn>();
            if (deducts != null)
            {
                foreach (var costDeductSyn in deducts)
                {
                    var costDeduct = costDeductSyn.CostDeduct;
                    if (costDeduct != null)
                    {
                        costs.Add(costDeduct);
                    }
                    if (costDeductSyn.IsDeleted)
                    {
                        if ((costDeductSyn.RealCurrency != 0 && costDeductSyn.RealCurrency.HasValue) || (costDeductSyn.VirtualCurrency != 0 && costDeductSyn.VirtualCurrency.HasValue))
                        {
                            delDeductDatas.Add(new DelDeductData
                                                  {
                                                      equipmentid = costDeductSyn.EquipmentId.HasValue ? costDeductSyn.EquipmentId.Value.ToString() : string.Empty,
                                                      payerid = userBLL.GetEntityById(costDeductSyn.PayerId).Label,
                                                      deductId = costDeductSyn.DeductId,
                                                      innerid = costDeductSyn.CostDeductId.ToString()
                                                  });
                        }
                        else
                        {
                            XTransaction transaction = null;
                            costDeductSynBLL.Delete(new[] { costDeductSyn.Id }, ref transaction);
                        }
                    }
                    else
                    {
                        if (costDeduct != null)
                        {
                            DeductData data = null;
                            if (costDeduct.UsedConfirmId.HasValue)
                            {
                                data = new DeductData(costDeduct.GetUsedConfirm());
                            }
                            else
                            {
                                data = new DeductData(costDeduct);
                            }
                            deductDatas.Add(data);
                        }
                    }
                }
                if (delDeductDatas.Count > 0)
                {

                    foreach (var delDeduct in delDeductDatas)
                    {
                        try
                        {
                            var deductResult = zsdxAccountBLL.DeductDelect(delDeduct);
                            //取消扣费成功
                            if (deductResult.success)
                            {
                                DeductResult result = deductResult;

                                var deduct =
                                    deducts.FirstOrDefault(
                                        x => x.CostDeductId.ToString().Trim().ToLower() == result.innerid.ToLower().Trim());
                                //扣除本地账户从校级账户转过来的金额
                                if (deduct != null)
                                {

                                    var account = userAccountBLL.GetEntityById(deduct.AccountId);
                                    if (account != null)
                                    {
                                        XTransaction t = null;
                                        //先扣除原来从校级平台补回的费用
                                        account.RealCurrency -= (deduct.RealCurrency.HasValue
                                                                     ? deduct.RealCurrency.Value
                                                                     : 0d);
                                        account.VirtualCurrency -= (deduct.VirtualCurrency.HasValue
                                                                        ? deduct.VirtualCurrency.Value
                                                                        : 0d);
                                        userAccountBLL.Save(new[] { account }, ref t);
                                    }
                                    //deduct.IsSyned = true;
                                    //deduct.SynTime = DateTime.Now;
                                    //deduct.RealCurrency = 0d;
                                    //deduct.VirtualCurrency = 0d;
                                    //costDeductSyns.Add(deduct);

                                    XTransaction transaction = null;
                                    costDeductSynBLL.Delete(new[] { deduct.Id }, ref transaction);
                                }
                            }
                            else
                            {
                                var deduct =
                                     deducts.FirstOrDefault(
                                         x => x.CostDeductId.ToString().Trim().ToLower() == delDeduct.innerid.ToLower().Trim());
                                if (deduct != null)
                                {
                                    deduct.SynTime = DateTime.Now;
                                    deduct.Remark = deductResult.errcode.ToString();
                                    costDeductSyns.Add(deduct);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var deduct =
                                 deducts.FirstOrDefault(
                                     x => x.CostDeductId.ToString().Trim().ToLower() == delDeduct.innerid.ToLower().Trim());
                            if (deduct != null)
                            {
                                deduct.SynTime = DateTime.Now;
                                deduct.Remark = ex.GetBaseException().Message;
                                costDeductSyns.Add(deduct);
                            }
                        }
                    }
                }
                if (deductDatas.Count > 0)
                {
                    foreach (var data in deductDatas)
                    {
                        try
                        {
                            var deductResult = zsdxAccountBLL.Deduct(data);
                            //扣费成功
                            if (deductResult.success)
                            {
                                DeductResult result = deductResult;
                                var cost =
                                    costs.FirstOrDefault(
                                        x => x.Id.ToString().Trim().ToLower() == result.innerid.ToLower().Trim());
                                var deduct =
                                    deducts.FirstOrDefault(
                                        x => x.CostDeductId.ToString().Trim().ToLower() == result.innerid.ToLower().Trim());
                                //校级账户扣费成功，本地账户补加已经扣除的费用
                                if (cost != null && deduct != null)
                                {
                                   var account = userAccountBLL.GetEntityById(deduct.AccountId);
                                   if (account != null)
                                   {
                                       XTransaction t = null;
                                       //先扣除原来从校级平台补回的费用
                                       account.RealCurrency -= (deduct.RealCurrency.HasValue
                                                                    ? deduct.RealCurrency.Value
                                                                    : 0d);
                                       account.VirtualCurrency -= (deduct.VirtualCurrency.HasValue
                                                                       ? deduct.VirtualCurrency.Value
                                                                       : 0d);
                                       //从校级补回费用
                                       account.RealCurrency += (cost.RealCurrency.HasValue
                                                                    ? cost.RealCurrency.Value
                                                                    : 0d);
                                       account.VirtualCurrency += (cost.VirtualCurrency.HasValue
                                                                       ? cost.VirtualCurrency.Value
                                                                       : 0d);
                                       userAccountBLL.Save(new[] { account }, ref t);
                                   }
                                    deduct.IsSyned = true;
                                    deduct.DeductId = deductResult.deductId.ToString();
                                    //记录从校级平台扣除了多少费用
                                    deduct.RealCurrency = cost.RealCurrency;
                                    deduct.VirtualCurrency = cost.VirtualCurrency;
                                    deduct.SynTime = DateTime.Now;
                                    deduct.Remark = result.errcode.ToString();
                                    costDeductSyns.Add(deduct);
                                }
                            }
                            else
                            {
                                var deduct =
                                     deducts.FirstOrDefault(
                                         x => x.CostDeductId.ToString().Trim().ToLower() == data.innerid.ToLower().Trim());
                                if (deduct != null)
                                {
                                    deduct.SynTime = DateTime.Now;
                                    deduct.Remark = deductResult.errcode.ToString();
                                    costDeductSyns.Add(deduct);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            var deduct =
                                 deducts.FirstOrDefault(
                                     x => x.CostDeductId.ToString().Trim().ToLower() == data.innerid.ToLower().Trim());
                            if (deduct != null)
                            {
                                deduct.SynTime = DateTime.Now;
                                deduct.Remark = ex.GetBaseException().Message;
                                costDeductSyns.Add(deduct);
                            }
                        }
                    }
                }
                XTransaction tran = null;
                costDeductSynBLL.Save(costDeductSyns, ref tran);
                //userAccountBLL.Save(userAccounts, ref tran);
            }
            errorMessage = string.Empty;
        }
    }
}