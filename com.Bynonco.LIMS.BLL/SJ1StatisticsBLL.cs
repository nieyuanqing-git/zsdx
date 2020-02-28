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
    public class SJ1StatisticsBLL : BLLBase<SJ1Statistics>, ISJ1StatisticsBLL
    {
        private ISJ1BLL __sJ1BLL = null;
        public SJ1StatisticsBLL()
        {
            __sJ1BLL = BLLFactory.CreateSJ1BLL();
        }
        public bool SJ1StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ1StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ1StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ1AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ1Statistics = GetEntityById(id);
                if (sJ1Statistics == null) throw new Exception("SJ1报表统计为空");
                sJ1Statistics.Status = (int)sJ1AuditStatus;
                sJ1Statistics.UpdateUserId = userId.Value;
                sJ1Statistics.UpdateTime = DateTime.Now;
                if (sJ1AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ1Statistics.AuditUserId = null;
                    sJ1Statistics.AuditTime = null;
                }
                else
                {
                    sJ1Statistics.AuditUserId = userId.Value;
                    sJ1Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ1Statistics[] { sJ1Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ1StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ1Statistics = GetEntityById(id);
                if (sJ1Statistics == null) throw new Exception("SJ1报表统计为空");
                var sJ1List = __sJ1BLL.GetEntitiesByExpression(string.Format("SJ1StatisticsId=\"{0}\"", id));
                if(sJ1List != null && sJ1List.Count() > 0)  
                    __sJ1BLL.Delete(sJ1List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ1Statistics.Id }, ref tran, true);
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