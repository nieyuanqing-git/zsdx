using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleApplyBLL : BLLBase<SampleApply>, ISampleApplyBLL
    {
        private ISampleItemBLL __sampleItemBLL;
        private IOpenFundApplyBLL __openFundApplyBLL;
        private ISubjectBLL __subjectBLL;
        public SampleApplyBLL()
        {
            __sampleItemBLL = BLLFactory.CreateSampleItemBLL();
            __openFundApplyBLL = BLLFactory.CreateOpenFundApplyBLL();
            __subjectBLL  = BLLFactory.CreateSubjectBLL();
        }
        public double GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList, bool isNeedOpenFundCostDeduct, Guid? subjectId, Guid? userId, Guid? tagId, Guid sampleItemId, int amount, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, out bool isOpenFundCostDeduct, out double openFundFee,out double? openFundDiscount)
        {
            IDictionary<Guid, double?> sampleItemOpenFundMapping = new Dictionary<Guid, double?>();
            var price =  GetSampleApplyPrice(viewOpenFundDetailList, isNeedOpenFundCostDeduct, subjectId, userId, tagId, new Guid[]{sampleItemId}, amount, preCostDeductEquipmentOpenFunds, out isOpenFundCostDeduct, out openFundFee, out sampleItemOpenFundMapping);
            openFundDiscount = sampleItemOpenFundMapping[sampleItemId];
            return price;
        }
        public double GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList,bool isNeedOpenFundCostDeduct, Guid sampleApplyId, Guid subjectId, IEnumerable<Guid> sampleItemIds, int amount, out bool isOpenFundCostDeduct, out double openFundFee)
        {
            var sampleApply = GetEntityById(sampleApplyId);
            return GetSampleApplyPrice(viewOpenFundDetailList,isNeedOpenFundCostDeduct, subjectId, sampleApply.Applicant.Id, sampleApply.Applicant.TagId, sampleItemIds, amount, sampleApply.CostDeduct != null ? sampleApply.CostDeduct.CostDeductEquipmentOpenFunds : null, out isOpenFundCostDeduct, out openFundFee);
        }
        private double GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList, bool isNeedOpenFundCostDeduct, Guid? subjectId, Guid? userId, Guid? tagId, IEnumerable<Guid> sampleItemIds, int amount, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, out bool isOpenFundCostDeduct, out double openFundFee,out  IDictionary<Guid, double?> sampleItemOpenFundMapping)
        {
            openFundFee = 0d;
            isOpenFundCostDeduct = false;
            bool isOpenFundCostDeductTemp = false;
            double price = 0d;
            int index = 0;
            sampleItemOpenFundMapping = new Dictionary<Guid, double?>();
            foreach (var sampleItemId in sampleItemIds)
            {
                index++;
                double? realEquipmentOpenFundDiscout = 1d;
                var sampleItem = __sampleItemBLL.GetEntityById(sampleItemId);
                var curSampleItemPrice = __sampleItemBLL.GetSampleItemPrice(sampleItemId, userId, tagId);
                curSampleItemPrice = sampleItem.ChargeCategoryEnum == SampleItemChargeCategory.Time ? curSampleItemPrice : curSampleItemPrice * amount;
                if (subjectId.HasValue)
                {
                    IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
                    var subject = __subjectBLL.GetEntityById(subjectId.Value);
                    var curCalPrice = isNeedOpenFundCostDeduct ? __openFundApplyBLL.CalEquipmentOpenFundFee(viewOpenFundDetailList, curSampleItemPrice, null, DateTime.Now, sampleItem.EquipmentId, subject.SubjectDirectorId.Value, index == 1 ? preCostDeductEquipmentOpenFunds : null, out isOpenFundCostDeductTemp, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscout) : curSampleItemPrice;
                    price += curCalPrice;
                    if (isOpenFundCostDeductTemp) isOpenFundCostDeduct = true;
                    if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0) openFundFee += costDeductEquipmentOpenFunds.Sum(p => p.CalFee);
                }
                else
                {
                    price += curSampleItemPrice;
                }
                sampleItemOpenFundMapping[sampleItemId] = realEquipmentOpenFundDiscout;
            }
            return price;
        }
        public double GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList, bool isNeedOpenFundCostDeduct, Guid? subjectId, Guid? userId, Guid? tagId, IEnumerable<Guid> sampleItemIds, int amount, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, out bool isOpenFundCostDeduct, out double openFundFee)
        {
            IDictionary<Guid, double?> sampleItemOpenFundMapping = new Dictionary<Guid, double?>();
            return GetSampleApplyPrice(viewOpenFundDetailList, isNeedOpenFundCostDeduct, subjectId, userId, tagId, sampleItemIds, amount, preCostDeductEquipmentOpenFunds, out isOpenFundCostDeduct, out openFundFee, out sampleItemOpenFundMapping);
        }
        public void GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList, SampleApply sampleApply,Guid? tagId, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, bool isEditSampleApplyChargeItem, bool isCostDeduct, bool isNeedOpenFundCostDeduct, out bool isOpenFundCostDeduct, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds)
        {
            isOpenFundCostDeduct = false;
            costDeductEquipmentOpenFunds = null;
            double? realEquipmentOpenFundDiscoutTemp = 1d;
            double price = 0d, openFundPrice = 0d;
            Guid? userId = null;
            if (sampleApply.Applicant != null) userId = sampleApply.Applicant.Id;
            double calPrice = __sampleItemBLL.GetSampleItemPrice(sampleApply.SampleItemId, userId, tagId);
            if (!isEditSampleApplyChargeItem)
            {
                price = sampleApply.ChargeCategoryEnum == SampleItemChargeCategory.SampleQuatity ? calPrice * (sampleApply.RealQuatity.HasValue ? sampleApply.RealQuatity.Value : sampleApply.Quatity) : calPrice;
            }
            if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
            {
                sampleApply.SampleApplyChargeItems = sampleApply.SampleApplyChargeItems.OrderByDescending(p => p.GetChargeItem().IsDefault).ToList();
                foreach (var sampleApplyChargeItem in sampleApply.SampleApplyChargeItems)
                {
                    if (!isEditSampleApplyChargeItem)//编辑保存申请单模式
                    {
                        if (sampleApplyChargeItem.GetChargeItem().IsDefault)
                        {
                            sampleApplyChargeItem.Price = price;
                            sampleApplyChargeItem.OpenFundPrice = 0d;
                            sampleApplyChargeItem.Quantity = 1;
                        }
                        else
                        {
                            price += (sampleApplyChargeItem.Price + sampleApplyChargeItem.OpenFundPrice) * sampleApplyChargeItem.Quantity;
                        }
                    }
                    else//编辑送样收费项目模式
                    {
                        price += (sampleApplyChargeItem.Price + sampleApplyChargeItem.OpenFundPrice) * sampleApplyChargeItem.Quantity;
                    }
                }
            }
            price = Math.Round(price, 2);
            sampleApply.CalPrice = calPrice;
            sampleApply.Price = price;
            sampleApply.UnitPrice = sampleApply.SampleItem.UnitPrice;
            if ((isCostDeduct || sampleApply.ChargeStatusEnum == SampleChargeStatus.Charged) && sampleApply.Applicant != null)
            {
                if (isNeedOpenFundCostDeduct )
                {
                    if (sampleApply.CostDeduct != null)
                    {
                        sampleApply.Price = __openFundApplyBLL.CalEquipmentOpenFundFee(viewOpenFundDetailList, price, null, sampleApply.ExpectSendDate.Value, sampleApply.SampleItem.EquipmentId, sampleApply.Subject.SubjectDirectorId.Value, preCostDeductEquipmentOpenFunds, out isOpenFundCostDeduct, out costDeductEquipmentOpenFunds, out realEquipmentOpenFundDiscoutTemp);
                        if (costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0) openFundPrice = costDeductEquipmentOpenFunds.Sum(p=>p.CalFee);
                        if (realEquipmentOpenFundDiscoutTemp == null) realEquipmentOpenFundDiscoutTemp = 1d;
                        sampleApply.CostDeduct.IsOpenFundCostDeduct = isOpenFundCostDeduct;
                        sampleApply.CostDeduct.OpenFundDiscount = realEquipmentOpenFundDiscoutTemp.Value;
                        if (sampleApply.SampleApplyChargeItems != null && sampleApply.SampleApplyChargeItems.Count > 0)
                        {
                            sampleApply.SampleApplyChargeItems = sampleApply.SampleApplyChargeItems.OrderByDescending(p => p.GetChargeItem().IsDefault).ToList();
                            foreach (var sampleApplyChargeItem in sampleApply.SampleApplyChargeItems)
                            {
                                var sampleApplyChargeItemPrice = (sampleApplyChargeItem.Price + sampleApplyChargeItem.OpenFundPrice) * sampleApplyChargeItem.Quantity;
                                if(sampleApplyChargeItemPrice * realEquipmentOpenFundDiscoutTemp >= openFundPrice)
                                {
                                    sampleApplyChargeItem.Price = (sampleApplyChargeItemPrice - openFundPrice) / sampleApplyChargeItem.Quantity;
                                    sampleApplyChargeItem.OpenFundPrice = openFundPrice / sampleApplyChargeItem.Quantity;
                                    openFundPrice = 0d;
                                }
                                else
                                {
                                    sampleApplyChargeItem.Price = (sampleApplyChargeItemPrice - sampleApplyChargeItemPrice * realEquipmentOpenFundDiscoutTemp.Value) / sampleApplyChargeItem.Quantity;
                                    sampleApplyChargeItem.OpenFundPrice = sampleApplyChargeItemPrice * realEquipmentOpenFundDiscoutTemp.Value / sampleApplyChargeItem.Quantity;
                                    openFundPrice -= sampleApplyChargeItemPrice * realEquipmentOpenFundDiscoutTemp.Value;
                                }
                                sampleApplyChargeItem.OpenFundDiscount = realEquipmentOpenFundDiscoutTemp.Value;
                                sampleApplyChargeItem.IsOpenFundCostDeduct = isOpenFundCostDeduct;
                            }
                        }
                    }
                }
            }
            sampleApply.IsCalPriceUsingOpenFund = isOpenFundCostDeduct;
            sampleApply.OpenFundDiscount = realEquipmentOpenFundDiscoutTemp;
            sampleApply.OpenFundCurrency = costDeductEquipmentOpenFunds != null && costDeductEquipmentOpenFunds.Count > 0 ? costDeductEquipmentOpenFunds.Sum(p => p.CalFee) : 0d;
        }
        public void GetSampleApplyPrice(IList<ViewOpenFundDetail> viewOpenFundDetailList, SampleApply sampleApply, IList<CostDeductEquipmentOpenFund> preCostDeductEquipmentOpenFunds, bool isEditSampleApplyChargeItem, bool isCostDeduct, bool isNeedOpenFundCostDeduct, out bool isOpenFundCostDeduct, out IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds)
        {
            GetSampleApplyPrice(viewOpenFundDetailList, sampleApply, null, preCostDeductEquipmentOpenFunds, isEditSampleApplyChargeItem, isCostDeduct, isNeedOpenFundCostDeduct, out isOpenFundCostDeduct, out costDeductEquipmentOpenFunds);
        }
        public string GetSampleApplyPriceStr(SampleApply sampleApply)
        {
            bool isOpenFundCostDeduct = CustomerFactory.GetCustomer().GetIsOpenFundCostDeduct();
            var price = sampleApply.Price.HasValue ? sampleApply.Price.Value : 0d;
            var openFundFee = sampleApply.OpenFundCurrency;
            if (sampleApply.CostDeduct != null)
            {
                if (isOpenFundCostDeduct)
                {
                    return string.Format("总金额:{0}\r\n普通币:{1}\r\n开放基金:{2}", sampleApply.CostDeduct.CalFee.ToString("0.00"),
                                                                                sampleApply.CostDeduct.Fee.ToString("0.00"),
                                                                                sampleApply.OpenFundCurrency.ToString("0.00"));
                }
                else
                {
                    return sampleApply.CostDeduct.CalFee.ToString("0.00");
                }
            }
            else
            {
                if (isOpenFundCostDeduct)
                {
                    return string.Format("总金额:{0}\r\n普通币:{1}\r\n开放基金:{2}",price.ToString("0.00"),
                                                                                (price - openFundFee).ToString("0.00"),
                                                                                openFundFee.ToString("0.00"));
                }
                else
                {
                    return price.ToString("0.00");
                }
            }
        }
    }
}