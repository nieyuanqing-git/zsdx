using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.SubAreaBehavior
{
    public class PeronalSubAreaBehavior:SubAreaBehaviorBase
    {
        public override bool Add(IEnumerable<SubArea> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            tran = august.DataLink.SessionManager.Instance.BeginTransaction();
            try
            {
                List<SubArea> lstSubAreas = new List<SubArea>();
                List<SubAreaUerPower> lstSubAreaUerPowers = new List<SubAreaUerPower>();
                foreach (var entity in entities)
                {
                    var subArea = subAreaBLL.GetFirstOrDefaultEntityByExpression(string.Format("OwnerId=\"{0}\"*SubAreaCategoryId=\"{1}\"", entity.OwnerId, entity.SubAreaCategoryId));
                    if (subArea != null) continue;
                    var subAreaUerPower = subAreaUerPowerBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*UserId=\"{1}\"", entity.Id.ToString(), entity.OwnerId.ToString()));
                    if (subAreaUerPower == null)
                        lstSubAreaUerPowers.Add(new SubAreaUerPower()
                        {
                            Id = Guid.NewGuid(),
                            IsStop = false,
                            SubAreaId = entity.Id,
                            UserId = entity.OwnerId,
                            IsAdmin = true
                        });
                    lstSubAreas.Add(entity);
                }
                subAreaBLL.Add(lstSubAreas.ToArray(), ref tran, true);
                if (lstSubAreaUerPowers.Count > 0) subAreaUerPowerBLL.Add(lstSubAreaUerPowers.ToArray(), ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
        }
           
    }
}
