using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class JudgeProjectBLL:BLLBase<JudgeProject>  ,IJudgeProjectBLL 
    {
        private IJudgeProjectContentBLL __judgeProjectContentBLL;
        public JudgeProjectBLL()
        {
            __judgeProjectContentBLL = BLLFactory.CreateJudgeProjectContentBLL();
        }

        public bool DeleteJudgeProject(Guid? userId, Guid judgeProjectId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var judgeProject = GetEntityById(judgeProjectId);
                if (judgeProject == null) throw new ArgumentException(string.Format("考核评价项目为空", judgeProjectId));
                judgeProject.IsDelete = true;
                Save(new JudgeProject[] { judgeProject }, ref tran, true);
                IList<JudgeProject> judgeProjectList = GetEntitiesByExpression("IsDelete=false");
                judgeProjectList = judgeProjectList.Where(p => !p.XPath.StartsWith(judgeProject.XPath) && double.Parse(p.XPath) > double.Parse(judgeProject.XPath) && p.XPath.Length >= judgeProject.XPath.Length).ToList();
                var entities = XPathUtility.AdjustXPath(judgeProject.XPath, judgeProjectList);
                Save(entities.ToArray(), ref tran, true);
                var delJudgeProjectContentList = __judgeProjectContentBLL.GetEntitiesByExpression(string.Format("JudgeProject=\"{0}\"*IsDelete=false", judgeProjectId));
                if (delJudgeProjectContentList != null && delJudgeProjectContentList.Count() > 0)
                    __judgeProjectContentBLL.Save(delJudgeProjectContentList, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }

    }
}
