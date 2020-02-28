using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 开放基金申请业务逻辑 </summary>
    public class OpenFundApplyBLL : BLLBase<OpenFundApply>, IOpenFundApplyBLL
    {
        private IOpenFundApplyEquipmentBLL __openFundApplyEquipmentBLL;
        protected IViewOpenFundDetailBLL __viewOpenFundDetailBLL;
        //private IDictCodeTypeBLL __dictCodeTypeBLL;
        //private static DictCodeType __equipmentOpenFundDiscountDictCodeType = null;
        public OpenFundApplyBLL()
        {
            __openFundApplyEquipmentBLL = BLLFactory.CreateOpenFundApplyEquipmentBLL();
            __viewOpenFundDetailBLL = BLLFactory.CreateViewOpenFundDetailBLL();
            //__dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        private bool DelOpenFundApply(Guid openFundApplyId, ref XTransaction tran, out string errorMessage)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var openFundApply = GetEntityById(openFundApplyId);
                if (openFundApply == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的开放基金申请单信息", openFundApplyId));
                if (openFundApply.StatusEnum != OpenFundApplyStatus.AuditWaitting)
                    throw new Exception(string.Format("出错,申请单状态为【{0}】,无法进行删除", openFundApply.StatusStr));
                openFundApply.IsDelete = true;
                var itemList = __openFundApplyEquipmentBLL.GetEntitiesByExpression(string.Format("OpenFundApplyId=\"{0}\"*IsDelete=false", openFundApplyId), null, "");
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList) item.IsDelete = true;
                    __openFundApplyEquipmentBLL.Save(itemList, ref tran, true);
                }
                Save(new OpenFundApply[] { openFundApply }, ref tran, true);
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
        public virtual bool DelOpenFundApply(Guid openFundApplyId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelOpenFundApply(openFundApplyId, ref tran, out errorMessage);
        }
        public virtual bool DelOpenFundApplys(IList<OpenFundApply> openFundApplys, out  string errorMessage)
        {
            errorMessage = "";
            if (openFundApplys != null && openFundApplys.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var openFundApply in openFundApplys)
                    {
                        var result = DelOpenFundApply(openFundApply.Id, ref tran, out errorMessage);
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
        #region original
        //public double CalEquipmentOpenFundFee(IList<ViewOpenFundDetail> viewOpenFundDetailList, double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        //{
        //    realEquipmentOpenFundDiscount = null;
        //    costDeductEquipmentOpenFunds = null;
        //    isEquipmentOpenFundDiscount = false;
        //    if (!equipmentOpenFundDiscount.HasValue)
        //    {
        //        equipmentOpenFundDiscount = GetEquipmentOpenFundDiscount();
        //    }
        //    IList<ViewOpenFundDetail> validViewOpenFundDetailList = null;
        //    if (viewOpenFundDetailList == null || viewOpenFundDetailList.Count == 0)
        //    {
        //        viewOpenFundDetailList = __viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(equipmentId, payerId, endTime);
        //        validViewOpenFundDetailList = viewOpenFundDetailList;
        //    }
        //    else if (viewOpenFundDetailList.Any(p => p.ValidTime >= endTime && p.EquipmentId == equipmentId))
        //    {
        //        var viewEquipemyOpenFundDetailList = viewOpenFundDetailList.Where(p => p.ValidTime >= endTime && p.EquipmentId == equipmentId);
        //        validViewOpenFundDetailList = viewOpenFundDetailList.Where(p => viewEquipemyOpenFundDetailList.Select(x => x.OpenFundApplyId).Contains(p.OpenFundApplyId)).ToList();
        //    }
        //    //处理设备开发基金
        //    if (validViewOpenFundDetailList != null && validViewOpenFundDetailList.Count > 0)
        //    {
        //        var totalBalanceMoney = validViewOpenFundDetailList.GroupBy(p => new { OpenFundApplyId = p.OpenFundApplyId, BalanceMoney = p.BalanceMoney }).Sum(p => p.Key.BalanceMoney);
        //        if (curCostDeductEquipmentOpenFunds != null && curCostDeductEquipmentOpenFunds.Count > 0)
        //        {
        //            foreach (var curCostDeductEquipmentOpenFund in curCostDeductEquipmentOpenFunds)
        //            {
        //                var viewOpenFundDetailListFind = validViewOpenFundDetailList.Where(p => p.OpenFundApplyId == curCostDeductEquipmentOpenFund.OpenFundApplyId);
        //                if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
        //                {
        //                    foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
        //                    {
        //                        viewOpenFundDetailFind.BalanceMoney += curCostDeductEquipmentOpenFund.CalFee;
        //                    }
        //                    totalBalanceMoney += curCostDeductEquipmentOpenFund.CalFee;
        //                }
        //            }
        //        }
        //        IList<Guid> openFundApplyIds = new List<Guid>();
        //        if (totalBalanceMoney >= calFee * equipmentOpenFundDiscount)
        //        {
        //            isEquipmentOpenFundDiscount = true;
        //            realEquipmentOpenFundDiscount = equipmentOpenFundDiscount;
        //            costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
        //            var oddCalFee = calFee * equipmentOpenFundDiscount;
        //            foreach (var viewOpenFundDetail in validViewOpenFundDetailList)
        //            {
        //                if (openFundApplyIds.Any(p => p == viewOpenFundDetail.OpenFundApplyId)) continue;
        //                openFundApplyIds.Add(viewOpenFundDetail.OpenFundApplyId);
        //                costDeductEquipmentOpenFunds.Add(new CostDeductEquipmentOpenFund()
        //                {
        //                    Id = Guid.NewGuid(),
        //                    OpenFundApplyId = viewOpenFundDetail.OpenFundApplyId,
        //                    CalFee = viewOpenFundDetail.BalanceMoney > oddCalFee ? oddCalFee.Value : viewOpenFundDetail.BalanceMoney
        //                });
        //                var viewOpenFundDetailListFind = validViewOpenFundDetailList.Where(p => p.OpenFundApplyId == viewOpenFundDetail.OpenFundApplyId);
        //                if (viewOpenFundDetail.BalanceMoney >= oddCalFee)
        //                {
        //                    //viewOpenFundDetail.BalanceMoney -= oddCalFee.Value;
        //                    if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
        //                    {
        //                        foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
        //                        {
        //                            viewOpenFundDetailFind.BalanceMoney -= oddCalFee.Value;
        //                        }
        //                    }
        //                    break;
        //                }
        //                oddCalFee = oddCalFee - viewOpenFundDetail.BalanceMoney;
        //                //viewOpenFundDetail.BalanceMoney = 0;
        //                if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
        //                {
        //                    foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
        //                    {
        //                        viewOpenFundDetailFind.BalanceMoney = 0;
        //                    }
        //                }
        //            }
        //            calFee = calFee * equipmentOpenFundDiscount.Value;
        //        }
        //    }
        //    return calFee;
        //}
        //public double CalEquipmentOpenFundFee(double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        //{
        //    return CalEquipmentOpenFundFee(null, calFee, equipmentOpenFundDiscount, endTime, equipmentId, payerId, curCostDeductEquipmentOpenFunds, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscount);
        //}
        //public double GetEquipmentOpenFundDiscount()
        //{
        //    IDictCodeTypeBLL dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        //    var equipmentOpenFundDiscount = 0.5;
        //    if (__equipmentOpenFundDiscountDictCodeType == null)
        //    {
        //        double equipmentOpenFundDiscountTemp = equipmentOpenFundDiscount;
        //        __equipmentOpenFundDiscountDictCodeType = dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=EquipmentOpenFundDiscount*IsDelete=false");
        //        if (__equipmentOpenFundDiscountDictCodeType != null
        //            && !string.IsNullOrWhiteSpace(__equipmentOpenFundDiscountDictCodeType.Value)
        //            && double.TryParse(__equipmentOpenFundDiscountDictCodeType.Value.Trim(), out equipmentOpenFundDiscountTemp))
        //        {
        //            equipmentOpenFundDiscount = Math.Round(double.Parse(__equipmentOpenFundDiscountDictCodeType.Value.Trim()) / 100, 2);
        //        }
        //    }
        //    return equipmentOpenFundDiscount;
        //}
        #endregion
        /// <summary> 计算开放基金费用 </summary>
        /// <param name="viewOpenFundDetailList">开放基金明细视图</param>
        /// <param name="calFee">费用</param>
        /// <param name="equipmentOpenFundDiscount">开放基金设备优惠</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="equipmentId">设备</param>
        /// <param name="payerId">付款人</param>
        /// <param name="curCostDeductEquipmentOpenFunds">当前开放基金设备费用</param>
        /// <param name="isEquipmentOpenFundDiscount">是否开放基金设备优惠</param>
        /// <param name="costDeductEquipmentOpenFunds">开放基金设备扣费明细</param>
        /// <param name="realEquipmentOpenFundDiscount">开放基设备真实优</param>
        /// <returns>真实费用</returns>
        public virtual double CalEquipmentOpenFundFee(IList<ViewOpenFundDetail> viewOpenFundDetailList, double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        {
            realEquipmentOpenFundDiscount = null;
            costDeductEquipmentOpenFunds = null;
            isEquipmentOpenFundDiscount = false;
            if (!equipmentOpenFundDiscount.HasValue)
            {
                // 如果没有开放基金优惠参数，则从辅助字典获取
                equipmentOpenFundDiscount = GetEquipmentOpenFundDiscount();
            }
            // 开放基金设备明细
            IList<ViewOpenFundDetail> validViewOpenFundDetailList = null;
            if (viewOpenFundDetailList == null || viewOpenFundDetailList.Count == 0)
            {
                // 如果没有开放基金申请明细，则获取开放基金设备明细
                viewOpenFundDetailList = __viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(equipmentId, payerId, endTime);
                // 提取开放基金设备
                validViewOpenFundDetailList = viewOpenFundDetailList;
            }
            else if (viewOpenFundDetailList.Any(p => p.ValidTime >= endTime && p.EquipmentId == equipmentId))
            {
                // 如果开放基金申请明细中有效时间大于等于结束时间，并且存在相同设备，则提取有效设备开放基金明细
                var viewEquipemyOpenFundDetailList = viewOpenFundDetailList.Where(p => p.ValidTime >= endTime && p.EquipmentId == equipmentId);
                // 提取开放基金设备
                validViewOpenFundDetailList = viewOpenFundDetailList.Where(p => viewEquipemyOpenFundDetailList.Select(x => x.OpenFundApplyId).Contains(p.OpenFundApplyId)).ToList();
            }
            // 付费人使用的设备在开放基金设备有效期内
            if (validViewOpenFundDetailList != null && validViewOpenFundDetailList.Count > 0)
            {
                // 如果存在设备开放基金明细，则计算各项明细
                // 计算剩余补贴基金
                // 将开放基金申请表与剩余补贴基金分组，避免同一开放基金申请重复合计
                var totalBalanceMoney = validViewOpenFundDetailList.GroupBy(p => new { OpenFundApplyId = p.OpenFundApplyId, BalanceMoney = p.BalanceMoney }).Sum(p => p.Key.BalanceMoney);
                // 预约扣费
                if (curCostDeductEquipmentOpenFunds != null && curCostDeductEquipmentOpenFunds.Count > 0)
                {
                    // 如果存在设备开放基金费用扣除，则累加剩余补贴基金
                    foreach (var curCostDeductEquipmentOpenFund in curCostDeductEquipmentOpenFunds)
                    {
                        // 查找相同的开放基金申请
                        var viewOpenFundDetailListFind = validViewOpenFundDetailList.Where(p => p.OpenFundApplyId == curCostDeductEquipmentOpenFund.OpenFundApplyId);
                        if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
                        {
                            foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                            {
                                viewOpenFundDetailFind.BalanceMoney += curCostDeductEquipmentOpenFund.CalFee;
                            }
                            totalBalanceMoney += curCostDeductEquipmentOpenFund.CalFee;
                        }
                    }
                }
                IList<Guid> openFundApplyIds = new List<Guid>();
                var oddCalFee = 0d;
                // 如果开放基金有剩余补贴币，重新计算真实币
                if (totalBalanceMoney > 0)
                {
                    // 如果合计剩余补贴基金大于0，则使用开放基金
                    oddCalFee = calFee;
                    isEquipmentOpenFundDiscount = true;
                    realEquipmentOpenFundDiscount = equipmentOpenFundDiscount;
                    costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                    foreach (var viewOpenFundDetail in validViewOpenFundDetailList)
                    {
                        if (openFundApplyIds.Any(p => p == viewOpenFundDetail.OpenFundApplyId)) continue;
                        openFundApplyIds.Add(viewOpenFundDetail.OpenFundApplyId);
                        // 记录所有的开放基金设备扣费信息
                        costDeductEquipmentOpenFunds.Add(new CostDeductEquipmentOpenFund()
                        {
                            Id = Guid.NewGuid(),
                            OpenFundApplyId = viewOpenFundDetail.OpenFundApplyId,
                            // 真实币大于补贴币
                            CalFee = viewOpenFundDetail.BalanceMoney >= oddCalFee * equipmentOpenFundDiscount ? oddCalFee * equipmentOpenFundDiscount.Value : viewOpenFundDetail.BalanceMoney
                        });
                        var viewOpenFundDetailListFind = validViewOpenFundDetailList.Where(p => p.OpenFundApplyId == viewOpenFundDetail.OpenFundApplyId);
                        if (viewOpenFundDetail.BalanceMoney >= oddCalFee * equipmentOpenFundDiscount)
                        {
                            if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
                            {
                                foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                                {
                                    viewOpenFundDetailFind.BalanceMoney -= oddCalFee * equipmentOpenFundDiscount.Value;
                                    totalBalanceMoney -= oddCalFee * equipmentOpenFundDiscount.Value;
                                    calFee -= oddCalFee * (equipmentOpenFundDiscount.Value);
                                    oddCalFee = 0d;
                                }
                            }
                            break;
                        }
                        calFee = calFee - viewOpenFundDetail.BalanceMoney / equipmentOpenFundDiscount.Value + viewOpenFundDetail.BalanceMoney / equipmentOpenFundDiscount.Value * (1d - equipmentOpenFundDiscount.Value);
                        oddCalFee = oddCalFee - viewOpenFundDetail.BalanceMoney / equipmentOpenFundDiscount.Value;
                        if (viewOpenFundDetailListFind != null && viewOpenFundDetailListFind.Count() > 0)
                        {
                            foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                            {
                                viewOpenFundDetailFind.BalanceMoney = 0;
                                totalBalanceMoney -= viewOpenFundDetailFind.BalanceMoney;
                            }
                        }
                    }
                }
                else
                {
                    calFee += oddCalFee;
                }
            }
            return calFee;
        }
        public virtual double CalEquipmentOpenFundFee(double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        {
            var viewOpenFundDetailList = __viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(equipmentId, payerId, endTime);
            return CalEquipmentOpenFundFee(viewOpenFundDetailList, calFee, equipmentOpenFundDiscount, endTime, equipmentId, payerId, curCostDeductEquipmentOpenFunds, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscount);
        }

        private double _equipmentOpenFundDiscount = double.NaN;
        /// <summary> 获取开放基金扣费折扣比（辅助字典） </summary>
        /// <returns>返回带小数点值</returns>
        public virtual double GetEquipmentOpenFundDiscount()
        {
            if (!double.IsNaN(_equipmentOpenFundDiscount)) return _equipmentOpenFundDiscount;
            var types = BLLFactory.CreateDictCodeTypeBLL().GetEntitiesByExpression("Code=\"EquipmentOpenFundDiscount\"*IsDelete=false");
            double discount = 0.5;
            if (types != null && types.Count > 0)
            {
                var discountStr = types[0].Value;
                if (!string.IsNullOrEmpty(discountStr))
                {
                    if (double.TryParse(discountStr, out discount))
                    {
                        return discount / 100;
                    }
                }
            }
            _equipmentOpenFundDiscount = discount;
            return _equipmentOpenFundDiscount;
        }
        public virtual bool SaveSampleAppyOpenFund(IList<ViewOpenFundDetail> viewOpenFundDetailList, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFundList, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFundList, ref XTransaction tran)
        {

            if (viewOpenFundDetailList != null && viewOpenFundDetailList.Count > 0)
            {
                var openFundApplyList = GetEntitiesByExpression(viewOpenFundDetailList.Select(p => p.OpenFundApplyId).ToFormatStr());
                if (preCostDeductEquipmentOpenFundList != null && preCostDeductEquipmentOpenFundList.Count() > 0)
                {
                    foreach (var preCostDeductEquipmentOpenFund in preCostDeductEquipmentOpenFundList)
                    {
                        var openFundApply = openFundApplyList.FirstOrDefault(p => p.Id == preCostDeductEquipmentOpenFund.OpenFundApplyId);
                        if (openFundApply != null)
                        {
                            openFundApply.BalanceMoney += preCostDeductEquipmentOpenFund.CalFee;
                        }
                    }
                }
                if (curCostDeductEquipmentOpenFundList != null && curCostDeductEquipmentOpenFundList.Count() > 0)
                {
                    foreach (var curCostDeductEquipmentOpenFund in curCostDeductEquipmentOpenFundList)
                    {
                        var openFundApply = openFundApplyList.FirstOrDefault(p => p.Id == curCostDeductEquipmentOpenFund.OpenFundApplyId);
                        if (openFundApply != null)
                        {
                            openFundApply.BalanceMoney -= curCostDeductEquipmentOpenFund.CalFee;
                        }
                    }
                }
                Save(openFundApplyList, ref tran, true);
            }
            return true;
        }
        /// <summary> 获取用户有效期内开放基金 </summary>
        /// <param name="userId">用户</param>
        /// <returns>开放基金</returns>
        public virtual OpenFundApply GetOpenFundApply(Guid userId)
        {
            var expression = string.Format("UserId=\"{0}\"*Status=\"{1}\"(ValidTime>\"{2}\"+ValidTime=\"{2}\")*IsDelete=false", userId, (int)OpenFundApplyStatus.AuditPass, DateTime.Now.ToString("yyyy-MM-dd"));
            var openFundApplys = GetEntitiesByExpression(expression, 1, 1, null, "ApplyTime Asc");
            if (openFundApplys == null || openFundApplys.Count == 0)
                return null;
            return openFundApplys[0];
        }
    }

    /// <summary> 华东师范大学开放基金申请业务逻辑 </summary>
    public class HDSFDXOpenFundApplyBLL : OpenFundApplyBLL
    {
        /// <summary> 计算开放基金费用 </summary>
        /// <param name="viewOpenFundDetailList">开放基金明细视图</param>
        /// <param name="calFee">费用</param>
        /// <param name="equipmentOpenFundDiscount">开放基金设备优惠</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="equipmentId">设备</param>
        /// <param name="payerId">付款人</param>
        /// <param name="curCostDeductEquipmentOpenFunds">当前开放基金设备费用</param>
        /// <param name="isEquipmentOpenFundDiscount">是否开放基金设备优惠</param>
        /// <param name="costDeductEquipmentOpenFunds">开放基金设备扣费明细</param>
        /// <param name="realEquipmentOpenFundDiscount">开放基设备真实优</param>
        /// <returns>真实费用</returns>
        public override double CalEquipmentOpenFundFee(IList<ViewOpenFundDetail> viewOpenFundDetailList, double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        {
            realEquipmentOpenFundDiscount = null;
            costDeductEquipmentOpenFunds = null;
            isEquipmentOpenFundDiscount = false;
            if (!equipmentOpenFundDiscount.HasValue)
            {
                // 如果没有开放基金优惠参数，则从辅助字典获取
                equipmentOpenFundDiscount = GetEquipmentOpenFundDiscount();
            }
            // 用户有效期内的开放基金设备
            IList<ViewOpenFundDetail> validViewOpenFundDetailList = __viewOpenFundDetailBLL.GetEquipmentViewOpenFundDetail(null, payerId, endTime);

            // 付费人申请开放基金在有效期内
            if (validViewOpenFundDetailList != null && validViewOpenFundDetailList.Count > 0)
            {
                // 按照有效期排序，用于扣费顺序控制
                validViewOpenFundDetailList = validViewOpenFundDetailList.OrderBy(x => x.ValidTime).ToList();
                var equipmentOpenFundDetails = validViewOpenFundDetailList.Where(x => x.EquipmentId == equipmentId).ToList();
                // 当前设备有开放基金
                if (equipmentOpenFundDetails.Any())
                {
                    // 如果存在设备开放基金明细，则计算各项明细
                    // 计算剩余补贴基金
                    // 将开放基金申请表与剩余补贴基金分组，避免同一开放基金申请重复合计
                    var totalBalanceMoney = validViewOpenFundDetailList.GroupBy(p => new { OpenFundApplyId = p.OpenFundApplyId, BalanceMoney = p.BalanceMoney }).Sum(p => p.Key.BalanceMoney);
                    // 预约扣费金额应计算到剩余补贴基金
                    if (curCostDeductEquipmentOpenFunds != null && curCostDeductEquipmentOpenFunds.Count > 0)
                    {
                        // 如果存在设备开放基金费用扣除，则累加剩余补贴基金
                        foreach (var curCostDeductEquipmentOpenFund in curCostDeductEquipmentOpenFunds)
                        {
                            CostDeductEquipmentOpenFund fund = curCostDeductEquipmentOpenFund;
                            var viewOpenFundDetailListFind = validViewOpenFundDetailList.Where(p => p.OpenFundApplyId == fund.OpenFundApplyId).ToList();
                            if (viewOpenFundDetailListFind.Any())
                            {
                                foreach (var viewOpenFundDetailFind in viewOpenFundDetailListFind)
                                {
                                    viewOpenFundDetailFind.BalanceMoney += fund.CalFee;
                                }
                                totalBalanceMoney += fund.CalFee;
                            }
                        }
                    }
                    IList<Guid> openFundApplyIds = new List<Guid>();

                    // 如果开放基金有剩余补贴币，重新计算真实币
                    if (totalBalanceMoney > 0)
                    {
                        // 如果合计剩余补贴基金大于0，则使用开放基金
                        isEquipmentOpenFundDiscount = true;
                        realEquipmentOpenFundDiscount = equipmentOpenFundDiscount;
                        costDeductEquipmentOpenFunds = new List<CostDeductEquipmentOpenFund>();
                        /* 混用模式
                         * 1.优先从设备所在开放基金及按照有效期扣除费用
                         * 2.继续从其他开放基金中扣费
                         */
                        var openFundFee = calFee * equipmentOpenFundDiscount.Value;
                        foreach (var viewOpenFundDetail in equipmentOpenFundDetails)
                        {
                            CalFeeProgress(ref calFee, ref openFundFee, ref totalBalanceMoney, costDeductEquipmentOpenFunds, openFundApplyIds, viewOpenFundDetail);
                        }
                        if (openFundFee > 0)
                        {
                            // 混用实现，移除已计算开发基金，查询与设备在同一申请日期的开放基金
                            //var exceptEquipmentOpenFundDetails = validViewOpenFundDetailList.Except(equipmentOpenFundDetails).Where(p => equipmentOpenFundDetails.Any(e => e.ValidTime.Date == p.ValidTime.Date)).ToList();
                            // 2015.11.10：混用实现改为只移除已计算开发基金
                            var exceptEquipmentOpenFundDetails = validViewOpenFundDetailList.Except(equipmentOpenFundDetails).ToList();
                            foreach (var viewOpenFundDetail in exceptEquipmentOpenFundDetails)
                            {
                                if (openFundFee > 0)
                                {
                                    CalFeeProgress(ref calFee, ref openFundFee, ref totalBalanceMoney, costDeductEquipmentOpenFunds, openFundApplyIds, viewOpenFundDetail);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return calFee;
        }

        /// <summary> 计算开放基金费用 </summary>
        /// <param name="calFee">费用</param>
        /// <param name="equipmentOpenFundDiscount">开放基金设备优惠</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="equipmentId">设备</param>
        /// <param name="payerId">付款人</param>
        /// <param name="curCostDeductEquipmentOpenFunds">当前开放基金设备费用</param>
        /// <param name="isEquipmentOpenFundDiscount">是否开放基金设备优惠</param>
        /// <param name="costDeductEquipmentOpenFunds">开放基金设备扣费明细</param>
        /// <param name="realEquipmentOpenFundDiscount">开放基设备真实优</param>
        /// <returns></returns>
        public override double CalEquipmentOpenFundFee(double calFee, double? equipmentOpenFundDiscount, DateTime endTime, Guid equipmentId, Guid payerId, IList<CostDeductEquipmentOpenFund> curCostDeductEquipmentOpenFunds, out bool isEquipmentOpenFundDiscount, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, out double? realEquipmentOpenFundDiscount)
        {
            return CalEquipmentOpenFundFee(null, calFee, equipmentOpenFundDiscount, endTime, equipmentId, payerId, curCostDeductEquipmentOpenFunds, out isEquipmentOpenFundDiscount, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscount);
        }

        /// <summary> 获取用户有效期内开放基金 </summary>
        /// <param name="userId">用户</param>
        /// <returns>开放基金</returns>
        public override OpenFundApply GetOpenFundApply(Guid userId)
        {
            var expression = string.Format("UserId=\"{0}\"*Status=\"{1}\"(ValidTime>\"{2}\"+ValidTime=\"{2}\")*(ApplyTime>\"{2}\"*ApplyTime<\"{3}\")*IsDelete=false", userId, (int)OpenFundApplyStatus.AuditPass, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            var openFundApplys = GetEntitiesByExpression(expression, 1, 1, null, "ApplyTime Asc");
            if (openFundApplys == null || openFundApplys.Count == 0)
                return null;
            return openFundApplys[0];
        }

        /// <summary> 单个开放基金扣费处理过程 </summary>
        /// <param name="calFee">真实金额</param>
        /// <param name="openFundFee">可用补贴金额</param>
        /// <param name="costDeductEquipmentOpenFunds"></param>
        /// <param name="openFundApplyIds"></param>
        /// <param name="viewOpenFundDetail"></param>
        /// <param name="totalBalanceMoney"></param>
        private static void CalFeeProgress(ref double calFee, ref double openFundFee, ref double totalBalanceMoney, IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds, IList<Guid> openFundApplyIds, ViewOpenFundDetail viewOpenFundDetail)
        {
            if (openFundApplyIds.Any(p => p == viewOpenFundDetail.OpenFundApplyId)) return;
            openFundApplyIds.Add(viewOpenFundDetail.OpenFundApplyId);
            /* 构建开放基金，设备扣费记录
             * 如对应开发基金剩余补贴基金充足（大于应付金额*开放基金优惠），记录金额为应付金额*开放基金优惠，否则全扣
             */
            if (viewOpenFundDetail.BalanceMoney >= openFundFee)
            {
                costDeductEquipmentOpenFunds.Add(new CostDeductEquipmentOpenFund()
                {
                    Id = Guid.NewGuid(),
                    OpenFundApplyId = viewOpenFundDetail.OpenFundApplyId,
                    CalFee = openFundFee
                });
                // 扣除补贴币
                totalBalanceMoney -= openFundFee;
                viewOpenFundDetail.BalanceMoney -= openFundFee;
                calFee -= openFundFee;
                openFundFee = 0;
            }
            else
            {
                costDeductEquipmentOpenFunds.Add(new CostDeductEquipmentOpenFund()
                {
                    Id = Guid.NewGuid(),
                    OpenFundApplyId = viewOpenFundDetail.OpenFundApplyId,
                    CalFee = viewOpenFundDetail.BalanceMoney
                });
                // 扣除补贴币
                totalBalanceMoney -= viewOpenFundDetail.BalanceMoney;
                openFundFee -= viewOpenFundDetail.BalanceMoney;
                calFee -= viewOpenFundDetail.BalanceMoney;
                viewOpenFundDetail.BalanceMoney = 0;
            }
        }
    }
}
