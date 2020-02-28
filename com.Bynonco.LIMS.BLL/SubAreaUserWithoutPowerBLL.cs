using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaUserWithoutPowerBLL : BLLBase<SubAreaUserWithoutPower>, ISubAreaUserWithoutPowerBLL
    {
        public override bool Add(IEnumerable<SubAreaUserWithoutPower> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            try
            {
                List<SubAreaUserWithoutPower> lstSubAreaUserWithoutPowers = new List<SubAreaUserWithoutPower>();
                foreach (var entity in entities)
                {
                    if (GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*SubAreaId=\"{1}\"", entity.UserId.ToString(), entity.SubAreaId.ToString())) == null)
                        lstSubAreaUserWithoutPowers.Add(entity);
                }
                return base.Add(lstSubAreaUserWithoutPowers.ToArray(), ref tran, isSupress);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                return false;
            }
        }
    }
}
