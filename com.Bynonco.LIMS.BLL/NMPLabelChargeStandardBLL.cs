using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPLabelChargeStandardBLL : BLLBase<NMPLabelChargeStandard>, INMPLabelChargeStandardBLL
    {
        public int GetNMPLabelChargeStandardCount(Guid nmpId, Guid labelId)
        {
            var count = CountModelListByExpression(string.Format("NMPId=\"{0}\"*NMPLabelId=\"{1}\"", nmpId, labelId));
            return count;
        }
        public IList<NMPLabelChargeStandard> GetNMPLabelChargeStandardList(Guid nmpId, Guid labelId)
        {
            var nmpLabelChargeStandardList = GetEntitiesByExpression(string.Format("NMPId=\"{0}\"*NMPLabelId=\"{1}\"", nmpId, labelId));
            return nmpLabelChargeStandardList;
        }
        public NMPLabelChargeStandard GetNMPLabelChargeStandard(Guid nmpId, Guid relationId)
        {
            return GetFirstOrDefaultEntityByExpression(string.Format("NMPId=\"{0}\"*RelationId=\"{1}\"", nmpId, relationId));
        }
        public IList<ILabelChargeStandard> GetNMPLabelChargeStandardListByNMPIdAndLableId(Guid nmpId, Guid labelId)
        {
            IList<NMPLabelChargeStandard> nmpLabelChargeStandardList = GetNMPLabelChargeStandardList(nmpId, labelId);
            if (nmpLabelChargeStandardList == null || nmpLabelChargeStandardList.Count == 0) return null;
            IList<ILabelChargeStandard> lstLabelChargeStandard = new List<ILabelChargeStandard>();
            foreach (var nmpLabelChargeStandard in nmpLabelChargeStandardList) lstLabelChargeStandard.Add(nmpLabelChargeStandard);
            return lstLabelChargeStandard;

        }
    }
}
