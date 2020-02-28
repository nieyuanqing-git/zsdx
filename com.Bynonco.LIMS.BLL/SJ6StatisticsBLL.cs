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
    public class SJ6StatisticsBLL : BLLBase<SJ6Statistics>, ISJ6StatisticsBLL
    {
        private ISJ6BLL __sJ6BLL = null;
        public SJ6StatisticsBLL()
        {
            __sJ6BLL = BLLFactory.CreateSJ6BLL();
        }
        public bool SJ6StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ6StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ6StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ6AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ6Statistics = GetEntityById(id);
                if (sJ6Statistics == null) throw new Exception("SJ6报表统计为空");
                sJ6Statistics.Status = (int)sJ6AuditStatus;
                sJ6Statistics.UpdateUserId = userId.Value;
                sJ6Statistics.UpdateTime = DateTime.Now;
                if (sJ6AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ6Statistics.AuditUserId = null;
                    sJ6Statistics.AuditTime = null;
                }
                else
                {
                    sJ6Statistics.AuditUserId = userId.Value;
                    sJ6Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ6Statistics[] { sJ6Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ6StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ6Statistics = GetEntityById(id);
                if (sJ6Statistics == null) throw new Exception("SJ6报表统计为空");
                var sJ6List = __sJ6BLL.GetEntitiesByExpression(string.Format("SJ6StatisticsId=\"{0}\"", id));
                if(sJ6List != null && sJ6List.Count() > 0)  
                    __sJ6BLL.Delete(sJ6List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ6Statistics.Id }, ref tran, true);
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