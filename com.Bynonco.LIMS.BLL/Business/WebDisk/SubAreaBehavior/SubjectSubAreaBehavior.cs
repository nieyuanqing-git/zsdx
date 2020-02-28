using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.SubAreaBehavior
{
    public class SubjectSubAreaBehavior : SubAreaBehaviorBase
    {
        public override bool Add(IEnumerable<SubArea> entities, ref august.DataLink.XTransaction tran, bool isSupress = false)
        {
            tran = SessionManager.Instance.BeginTransaction();
            try
            {
                List<SubArea> lstSubAreas = new List<SubArea>();
                List<SubAreaUerPower> lstSubAreaUerPowers = new List<SubAreaUerPower>();
                foreach (var entity in entities)
                {
                    if (entity.SubjectId == null || entity.SubjectId == new Guid())
                        throw new Exception("课题编码为空");
                    var subject = subjectBLL.GetEntityById(entity.SubjectId);
                    if (subject == null)
                        throw new Exception("获取课题信息失败");
                    var subArea = subAreaBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"", entity.SubjectId.ToString()));
                    if (subArea != null) continue;
                    else
                    {
                        entity.OwnerId = subject.SubjectDirectorId.Value == new Guid() ? entity.OwnerId : subject.SubjectDirectorId.Value;
                        lstSubAreas.Add(entity);
                        if (subject.Experiments != null && subject.Experiments.Count > 0)
                        {
                            var subAreaUerPower = subAreaUerPowerBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*UserId=\"{1}\"", entity.Id.ToString(), entity.OwnerId.ToString()));
                            //添加可以负责人访问权限
                            if (subAreaUerPower == null)
                            {
                                lstSubAreaUerPowers.Add(new SubAreaUerPower()
                                {
                                    Id = Guid.NewGuid(),
                                    IsStop = false,
                                    SubAreaId = entity.Id,
                                    UserId = entity.OwnerId,
                                    IsAdmin = true
                                });
                            }
                            //添加课题组成员访问权限
                            foreach (var experimenter in subject.Experiments)
                            {
                                subAreaUerPower = subAreaUerPowerBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*UserId=\"{1}\"", entity.Id.ToString(), experimenter.ExperimenterId.ToString()));
                                if (subAreaUerPower == null)
                                    lstSubAreaUerPowers.Add(new SubAreaUerPower()
                                    {
                                        Id = Guid.NewGuid(),
                                        IsStop = false,
                                        SubAreaId = entity.Id,
                                        UserId = experimenter.ExperimenterId.Value,
                                        IsAdmin = experimenter.ExperimenterId == entity.OwnerId
                                    });
                            }
                        }
                    }
                    if (lstSubAreas.Count > 0)
                    {
                        subAreaBLL.Add(lstSubAreas.ToArray(), ref tran, true);
                        if (lstSubAreaUerPowers.Count > 0)
                            subAreaUerPowerBLL.Add(lstSubAreaUerPowers.ToArray(), ref tran, true);
                    }
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                log.Error(string.Format("{0}:{1}", "Add", ex.Message));
                return false;
            }
            finally { tran.Dispose(); }
        }

    }
}
