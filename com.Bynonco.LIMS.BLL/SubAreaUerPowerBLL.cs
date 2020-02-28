using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaUerPowerBLL : BLLBase<SubAreaUerPower>, ISubAreaUerPowerBLL
    {
        public override bool Add(IEnumerable<SubAreaUerPower> entities , ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            try
            {
                List<SubAreaUerPower> lstSubAreaUerPowers = new List<SubAreaUerPower>();
                foreach (var entity in entities)
                {
                    if (GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*SubAreaId=\"{1}\"", entity.UserId.ToString(), entity.SubAreaId.ToString())) == null)
                        lstSubAreaUerPowers.Add(entity);
                }
                return base.Add(lstSubAreaUerPowers.ToArray(), ref tran, isSupress);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                return false;
            }

        }
    }
}
