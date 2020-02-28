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
    public class NMPLabelBLL : BLLBase<NMPLabel>, INMPLabelBLL
    {
        private INMPLabelChargeStandardBLL __nmpLabelChargeStandardBLL;
        private LabelRelativeBLL __labelRelativeBLL;
        public NMPLabelBLL()
        {
            __nmpLabelChargeStandardBLL = BLLFactory.CreateNMPLabelChargeStandardBLL();
            __labelRelativeBLL = new LabelRelativeBLL(GetNMPLabelList, GetNMPLabelListByEquipmentIdAndLabelName, GetNMPLabel, __nmpLabelChargeStandardBLL.GetNMPLabelChargeStandardCount);
        }
        private IList<ILabel> GetNMPLabelList(Guid nmpId)
        {
            IList<ILabel> lstLabel = new List<ILabel>();
            var nmpLabelList = GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", nmpId), null, "LabelType");
            if (nmpLabelList == null || nmpLabelList.Count == 0) return null;
            foreach (var nmpLabel in nmpLabelList) lstLabel.Add(nmpLabel);
            return lstLabel;
        }
        private IList<ILabel> GetNMPLabelListByEquipmentIdAndLabelName(Guid nmpId, string labelName)
        {
            IList<ILabel> lstLabel = new List<ILabel>();
            var nmpLabelList = GetEntitiesByExpression(string.Format("NMPId=\"{0}\"*Name=\"{1}\"", nmpId, labelName));
            if (nmpLabelList == null || nmpLabelList.Count == 0) return null;
            foreach (var nmpLabel in nmpLabelList) lstLabel.Add(nmpLabel);
            return lstLabel;
        }
        private ILabel GetNMPLabel(IList<ILabel> lstEquipmentLabel, Guid nmpId)
        {
            if (lstEquipmentLabel.Count > 0)
            {
                var queryExpression = string.Format("NMPId=\"{0}\"", nmpId);
                queryExpression += "*" + lstEquipmentLabel.Select(p => p.Id.ToString()).ToFormatStr("NMPLabelId");
                var nmpChargeStandards = __nmpLabelChargeStandardBLL.GetEntitiesByExpression(queryExpression);
                if (nmpChargeStandards != null && nmpChargeStandards.Count > 0)
                {
                    return nmpChargeStandards.OrderBy(p => p.OrderNo).ThenBy(p => p.NMPLabel.LabelType).First().NMPLabel;
                }
            }
            return null;
        }
        public bool JudgeIsExistNMPLabelName(Guid nmpId, Guid? userId, string labelName)
        {
            return __labelRelativeBLL.JudgeIsExistLabelName(nmpId, userId, labelName);
        }
        public IList<User> GetNMPLabelUserList(Guid nmpId)
        {
            return __labelRelativeBLL.GetLabelUserList(nmpId);
        }
        public NMPLabel GetNMPLabelByUserId(Guid nmpId, Guid? userId, Guid? subjectId,bool isNeedExistNMPLaeblChargeStandard = false)
        {
            return (NMPLabel)__labelRelativeBLL.GetLabelByUserId(nmpId, userId, subjectId, isNeedExistNMPLaeblChargeStandard);
        }
        private IList<User> GetEquipmentLabelUserList(IEnumerable<NMPLabel> nmpLabelList)
        {
            return __labelRelativeBLL.GetLabelUserList(nmpLabelList);
        }
    }
}
