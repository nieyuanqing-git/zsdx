using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAdditionChargeItemBLL : BLLBase<NMPAdditionChargeItem>, INMPAdditionChargeItemBLL
    {
        private INMPLabelBLL __nmpLabelBLL;
        private INMPLabelChargeStandardBLL __nmpLabelChargeStandardBLL;
        private INMPLabelAddtionChargeItemBLL __nmpLabelAddtionChargeItemBLL;
        public NMPAdditionChargeItemBLL()
        {
            __nmpLabelBLL = BLLFactory.CreateNMPLabelBLL();
            __nmpLabelChargeStandardBLL = BLLFactory.CreateNMPLabelChargeStandardBLL();
            __nmpLabelAddtionChargeItemBLL = BLLFactory.CreateNMPLabelAddtionChargeItemBLL();
        }
        private IList<NMPAdditionChargeItem> GeNMPAdditionChargeItemList(Guid nmpId, Guid? userId,Guid? subjectId)
        {
            if (!userId.HasValue) return null;
            var nmpLable = __nmpLabelBLL.GetNMPLabelByUserId(nmpId, userId.Value, subjectId);
            if (nmpLable != null)
            {
                var nmpLabelChargeStandardList = __nmpLabelChargeStandardBLL.GetEntitiesByExpression(string.Format("NMPLabelId=\"{0}\"", nmpLable.Id));
                if (nmpLabelChargeStandardList != null && nmpLabelChargeStandardList.Count > 0)
                {
                    var nmpLabelAddtionChargeItemList = __nmpLabelAddtionChargeItemBLL.GetEntitiesByExpression(string.Format("NMPLabelId=\"{0}\"", nmpLable.Id));
                    if (nmpLabelAddtionChargeItemList == null || nmpLabelAddtionChargeItemList.Count == 0) return null;
                    var nmpAdditionChargeItemList = GetEntitiesByExpression(nmpLabelAddtionChargeItemList.Select(p => p.NMPAdditionChargeItemId).ToFormatStr() + "*IsStop=false*IsDelete=false");
                    if (nmpAdditionChargeItemList != null && nmpAdditionChargeItemList.Count > 0)
                    {
                        foreach (var nmpAdditionChargeItem in nmpAdditionChargeItemList)
                            nmpAdditionChargeItem.StandardPrice = nmpLabelAddtionChargeItemList.First(p => p.NMPAdditionChargeItemId == nmpAdditionChargeItem.Id).StandardPrice;
                    }
                    return nmpAdditionChargeItemList;
                }
            }
            return GetEntitiesByExpression(string.Format("NMPId=\"{0}\"*IsStop=false*IsDelete=false", nmpId.ToString()));
        }
        public IList<IAdditionChargeItem> GetNMPAdditionChargeItemListByNMPIdAndUserId(Guid nmpId, Guid userId, Guid? subjectId)
        {
            var nmpAdditionChargeItemList = GeNMPAdditionChargeItemList(nmpId, userId,subjectId);
            if (nmpAdditionChargeItemList == null || nmpAdditionChargeItemList.Count == 0) return null;
            IList<IAdditionChargeItem> lstAdditionChargeItem = new List<IAdditionChargeItem>();
            foreach (var nmpAdditionChargeItem in nmpAdditionChargeItemList)
            {
                lstAdditionChargeItem.Add(nmpAdditionChargeItem);
            }
            return lstAdditionChargeItem;
        }
        public IList<NMPAdditionChargeItem> GetUserNMPAdditionChargeItemList(Guid nmpId, Guid? userId, Guid? subjectId)
        {
            return GeNMPAdditionChargeItemList(nmpId, userId, subjectId);
        }
    }
}
