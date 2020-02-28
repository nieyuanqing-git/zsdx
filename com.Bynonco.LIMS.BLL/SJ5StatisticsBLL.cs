using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class SJ5StatisticsBLL : BLLBase<SJ5Statistics>, ISJ5StatisticsBLL
    {
        private ISJ5BLL __sJ5BLL = null;
        public SJ5StatisticsBLL()
        {
            __sJ5BLL = BLLFactory.CreateSJ5BLL();
        }
        public bool SJ5StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ5StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ5StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ5AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ5Statistics = GetEntityById(id);
                if (sJ5Statistics == null) throw new Exception("SJ5报表统计为空");
                sJ5Statistics.Status = (int)sJ5AuditStatus;
                sJ5Statistics.UpdateUserId = userId.Value;
                sJ5Statistics.UpdateTime = DateTime.Now;
                if (sJ5AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ5Statistics.AuditUserId = null;
                    sJ5Statistics.AuditTime = null;
                }
                else
                {
                    sJ5Statistics.AuditUserId = userId.Value;
                    sJ5Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ5Statistics[] { sJ5Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ5StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ5Statistics = GetEntityById(id);
                if (sJ5Statistics == null) throw new Exception("SJ5报表统计为空");
                var sJ5List = __sJ5BLL.GetEntitiesByExpression(string.Format("SJ5StatisticsId=\"{0}\"", id));
                if(sJ5List != null && sJ5List.Count() > 0)  
                    __sJ5BLL.Delete(sJ5List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ5Statistics.Id }, ref tran, true);
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