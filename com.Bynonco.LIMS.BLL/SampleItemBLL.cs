using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;
namespace com.Bynonco.LIMS.BLL
{
    public class SampleItemBLL : BLLBase<SampleItem>, ISampleItemBLL
    {
        private IUserBLL __userBLL;
        private ITagSampleItemDiscountBLL __tagSampleItemDiscountBLL;
        private IUserSampleItemDiscountBLL __userSampleItemDiscountBLL;
        private IEquipmentBLL __equipmentBLL;
        private ISampleItemStatusBLL __sampleItemStatusBLL;
        public SampleItemBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __tagSampleItemDiscountBLL = BLLFactory.CreateTagSampleItemDiscountBLL();
            __userSampleItemDiscountBLL = BLLFactory.CreateUserSampleItemDiscountBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __sampleItemStatusBLL = BLLFactory.CreateSampleItemStatusBLL();
        }
        public double GetSampleItemPrice(Guid sampleItemId, Guid? userId,Guid? tagId)
        {
            ISampleItemLabelBLL sampleItemLabelBLL = BLLFactory.CreateSampleItemLabelBLL();
            double discount = 1;
            if (userId.HasValue)
            {
                var sampleItem = sampleItemLabelBLL.GetSampleItemChargeStandardByUserId(sampleItemId, userId, out discount);
                return Math.Round(sampleItem.UnitPrice, 2);
            }
            if (tagId.HasValue)
            {
                var sampleItem = sampleItemLabelBLL.GetSampleItemChargeStandardByTagId(sampleItemId, tagId, out discount);
                return Math.Round(sampleItem.UnitPrice, 2);
            }
            return 0d;
        }
        public IList<SampleItem> GetSampleItemLitByEquipmentIds(IEnumerable<Guid> equipmentIds, IEnumerable<Guid> selectedSampleItemIds)
        {
            if (equipmentIds == null || equipmentIds.Count() == 0) return null;
            var sampleItemList = GetEntitiesByExpression(equipmentIds.ToFormatStr("EquipmentId") + "*IsStop=false*IsDelete=false", null, "Name", true);
            if (sampleItemList != null && sampleItemList.Count > 0)
            {
                foreach (var sampleItem in sampleItemList)
                {
                    if (selectedSampleItemIds != null && selectedSampleItemIds.Count() > 0)
                        sampleItem.selected = selectedSampleItemIds.Any(p => p == sampleItem.Id);
                }
            }
            return sampleItemList;
        }

        public bool ImportSampleItem(IList<SampleItem> SampleItemList, bool isDeal = false)
        {
            if (SampleItemList == null || SampleItemList.Count < 0)
                return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                int count = 0;
                foreach (var sampleItem in SampleItemList)
                {
                    count++;
                    sampleItem.Id = Guid.NewGuid();
                    sampleItem.IsDelete = false;
                    sampleItem.IsStop = false;
                    sampleItem.InputStr = ShortcutStringUtility.Chinese2Pinyin(sampleItem.Name.ToString()).ToLower();
                    var obj = GetSingleResult("select max(orderno) from sampleitem where isdelete<>1");
                    if (obj == null) obj = 0;
                    sampleItem.OrderNo = Convert.ToInt32(obj) + count;
                    Add(new SampleItem[] { sampleItem }, ref tran, true);    
                    if (sampleItem.SampleItemStatuses != null && sampleItem.SampleItemStatuses.Count > 0)
                    {
                        foreach (var sampleItemStatus in sampleItem.SampleItemStatuses)
                        {
                            SampleItemStatus newSampleItemStatus = new SampleItemStatus();
                            newSampleItemStatus.Id = Guid.NewGuid();
                            newSampleItemStatus.SampleItemId = sampleItem.Id;
                            newSampleItemStatus.SampleStatusId = sampleItemStatus.SampleStatusId;
                            __sampleItemStatusBLL.Add(new SampleItemStatus[] { newSampleItemStatus }, ref tran, true);
                        }
                    }                           
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
        }
    }
}