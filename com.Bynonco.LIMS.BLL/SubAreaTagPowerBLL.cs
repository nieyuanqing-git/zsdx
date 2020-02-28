using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaTagPowerBLL : BLLBase<SubAreaTagPower>, ISubAreaTagPowerBLL
    {
        public override bool Add(IEnumerable<SubAreaTagPower> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            try
            {
                List<SubAreaTagPower> lstSubAreaTagPowers = new List<SubAreaTagPower>();
                foreach (var entity in entities)
                {
                    if (GetFirstOrDefaultEntityByExpression(string.Format("TagId=\"{0}\"*SubAreaId=\"{1}\"", entity.TagId.ToString(), entity.SubAreaId.ToString())) != null)
                        continue;
                    lstSubAreaTagPowers.Add(entity);
                }
                return base.Add(lstSubAreaTagPowers.ToArray(), ref tran, isSupress);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                return false;
            }
        }
    }
}
