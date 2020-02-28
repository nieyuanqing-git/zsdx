using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleItemLabelBLL : BLLBase<SampleItemLabel>, ISampleItemLabelBLL
    {
        private IUserBLL __userBLL;
        private ISubjectBLL __subjectBLL;
        private IExperimenterSubjectBLL __experimenterSubjectBLL;
        public SampleItemLabelBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __subjectBLL = BLLFactory.CreateSubjectBLL();
            __experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
        }

        public SampleItemLabel GetSampleItemLabelByUserId(Guid sampleItemId, Guid? userId)
        {
            ISampleItemLabelChargeStandardBLL sampleItemLabelChargeStandardBLL = BLLFactory.CreateSampleItemLabelChargeStandardBLL();   
            if (!userId.HasValue) return null;
            var user = __userBLL.GetEntityById(userId.Value);
            var sampleItemIdLabelList = GetEntitiesByExpression(string.Format("SampleItemId=\"{0}\"", sampleItemId), null, "LabelType");
            if (sampleItemIdLabelList == null || sampleItemIdLabelList.Count == 0) return null;
            SampleItemLabel fistFindSampleItemLabel = null;
            List<SampleItemLabel> lstSampleItemLabel = new List<SampleItemLabel>();
            List<SampleItemLabelChargeStandard> lstSampleItemLabelChargeStandard = new List<SampleItemLabelChargeStandard>();
            foreach (var sampleItemLabel in sampleItemIdLabelList)
            {
                var count = sampleItemLabelChargeStandardBLL.CountModelListByExpression(string.Format("SampleItemId=\"{0}\"*SampleItemLabelId=\"{1}\"", sampleItemId, sampleItemLabel.Id));
                if (sampleItemLabel.LabelItems == null || sampleItemLabel.LabelItems.Count == 0) continue;
                switch (sampleItemLabel.LabelTypeEnum)
                {
                    case Model.Enum.LabelType.User:
                        if (sampleItemLabel.LabelItems.Any(p => p.LabelItemId == userId))
                        {
                            fistFindSampleItemLabel = sampleItemLabel;
                            lstSampleItemLabel.Add(sampleItemLabel);
                        }
                        break;
                    case Model.Enum.LabelType.Suject:
                        var subjects = __subjectBLL.GetUserRelevantSubjects(userId.Value);
                        if (subjects != null && subjects.Count > 0)
                        {
                            if (sampleItemLabel.LabelItems.Any(p => subjects.Select(s => s.Id).Contains(p.LabelItemId)))
                            {
                                fistFindSampleItemLabel = sampleItemLabel;
                                lstSampleItemLabel.Add(sampleItemLabel);
                            }
                        }
                        break;
                    case Model.Enum.LabelType.Organization:
                        if (user.OrganizationId.HasValue && sampleItemLabel.LabelItems.Any(p => p.LabelItemId == user.OrganizationId.Value))
                        {
                            fistFindSampleItemLabel = sampleItemLabel;
                            lstSampleItemLabel.Add(sampleItemLabel);
                        }
                        break;
                    case Model.Enum.LabelType.Tag:
                        if (user.TagId.HasValue && sampleItemLabel.LabelItems.Any(p => p.LabelItemId == user.TagId.Value))
                        {
                            fistFindSampleItemLabel = sampleItemLabel;
                            lstSampleItemLabel.Add(sampleItemLabel);
                        }
                        break;
                }
            }
            if (lstSampleItemLabel.Count > 0)
            {
                var queryExpression = string.Format("SampleItemId=\"{0}\"", sampleItemId);
                queryExpression += "*" + lstSampleItemLabel.Select(p => p.Id.ToString()).ToFormatStr("SampleItemLabelId");
                var sampleItemLabelChargeStandards = sampleItemLabelChargeStandardBLL.GetEntitiesByExpression(queryExpression);
                if (sampleItemLabelChargeStandards != null && sampleItemLabelChargeStandards.Count > 0)
                    return sampleItemLabelChargeStandards.OrderBy(p => p.OrderNo).ThenBy(p => p.SampleItemLabel.LabelType).First().SampleItemLabel;
            }
            return fistFindSampleItemLabel;
        }
        public SampleItem GetSampleItemChargeStandardByUserId(Guid sampleItemId, Guid? userId, out double discount)
        {
            ISampleItemLabelChargeStandardBLL sampleItemLabelChargeStandardBLL = BLLFactory.CreateSampleItemLabelChargeStandardBLL();  
            ISampleItemBLL sampleItemBLL = BLLFactory.CreateSampleItemBLL();
            discount = 1;
            var sampleItem = sampleItemBLL.GetEntityById(sampleItemId);
            var sampleItemLable = GetSampleItemLabelByUserId(sampleItemId, userId.Value);
            if (sampleItemLable == null) return sampleItem;
            var sampleItemLabelChargeStandardList = sampleItemLabelChargeStandardBLL.GetEntitiesByExpression(string.Format("SampleItemId=\"{0}\"*SampleItemLabelId=\"{1}\"", sampleItemId, sampleItemLable.Id));
            if (sampleItemLabelChargeStandardList != null && sampleItemLabelChargeStandardList.Count > 0)
            {
                var sampleItemLabelChargeStandard = sampleItemLabelChargeStandardList.First();
                discount = sampleItemLabelChargeStandard.StandardPrice / sampleItem.UnitPrice;
                sampleItem.UnitPrice = sampleItemLabelChargeStandard.StandardPrice;
                sampleItem.ChargeCategoryEnum = sampleItemLabelChargeStandard.ChargeTypeEnum;
            }
            return sampleItem;
        }
        public SampleItem GetSampleItemChargeStandardByTagId(Guid sampleItemId, Guid? tagId, out double discount)
        {
            ISampleItemBLL sampleItemBLL = BLLFactory.CreateSampleItemBLL();
            ISampleItemLabelChargeStandardBLL sampleItemLabelChargeStandardBLL = BLLFactory.CreateSampleItemLabelChargeStandardBLL();
            discount = 1d;
            var sampleItem = sampleItemBLL.GetEntityById(sampleItemId);
            var sampleItemIdLabel = GetFirstOrDefaultEntityByExpression(string.Format("SampleItemId=\"{0}\"*LabelType={1}", sampleItemId, (int)LabelType.Tag), null, "LabelType");
            if (sampleItemIdLabel != null)
            {
                var sampleItemLabelChargeStandard = sampleItemLabelChargeStandardBLL.GetFirstOrDefaultEntityByExpression(string.Format("SampleItemId=\"{0}\"*SampleItemLabelId=\"{1}\"", sampleItemId, sampleItemIdLabel.Id));
                if (sampleItemLabelChargeStandard != null)
                {
                    sampleItem.UnitPrice = sampleItemLabelChargeStandard.StandardPrice;
                    discount = sampleItemLabelChargeStandard.StandardPrice / sampleItem.UnitPrice;
                    sampleItem.ChargeCategoryEnum = sampleItemLabelChargeStandard.ChargeTypeEnum;
                }
            }
            return sampleItem;
        }
    }
}
