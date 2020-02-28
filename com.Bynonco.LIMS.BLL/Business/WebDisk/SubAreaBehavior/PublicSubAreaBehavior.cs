using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.SubAreaBehavior
{
    public class PublicSubAreaBehavior :SubAreaBehaviorBase
    {
        public override bool Add(IEnumerable<SubArea> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            tran = august.DataLink.SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var entity in entities)
                {
                    var entityTemp = subAreaBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubAreaCategoryId=\"{0}\"", entity.SubAreaCategoryId.ToString()));
                    if (entityTemp != null) throw new Exception("已存在该类型的分区");
                    var tags = tagBLL.GetEntitiesByExpression("");//获取全部用户类型
                    List<SubAreaTagPower> lstSubAreaTagPowers = new List<SubAreaTagPower>();
                    if (tags != null && tags.Count > 0)
                    {
                        foreach (var tag in tags)
                        {
                            lstSubAreaTagPowers.Add(new SubAreaTagPower()
                                {
                                    Id = Guid.NewGuid(),
                                    IsStop = false,
                                    SubAreaId = entity.Id,
                                    TagId = tag.Id
                                });
                        }
                    }
                    subAreaBLL.Add(entities, ref tran, true);
                    if (lstSubAreaTagPowers.Count > 0)
                        subAreaTagPowerBLL.Add(lstSubAreaTagPowers.ToArray(), ref tran, true);
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                tran.RollbackTransaction();
                return false;
            }
        }
    }
}
