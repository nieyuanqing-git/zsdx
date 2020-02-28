using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class AchievementStudentBLL : BLLBase<AchievementStudent>, IAchievementStudentBLL
    {
        private IAttachmentBLL __attachmentBLL;
        public AchievementStudentBLL()
        {
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
        }
        public bool DeleteAchievementStudent(AchievementStudent achievementStudent)
        {
            if (achievementStudent == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", achievementStudent.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { achievementStudent.Id }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
        public IList<AchievementStudent> ImportAchievementStudent(IList<AchievementStudent> achievementStudentList, bool isDeal = false)
        {
            if (achievementStudentList == null || achievementStudentList.Count == 0)
                return null;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            foreach (var item in achievementStudentList)
            {

                try
                {
                    Add(new AchievementStudent[] { item }, ref tran, true);
                    tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    achievementStudentList.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            return achievementStudentList;
        }
    }
}