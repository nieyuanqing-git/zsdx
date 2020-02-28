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
    public class SJ2StatisticsBLL : BLLBase<SJ2Statistics>, ISJ2StatisticsBLL
    {
        private ISJ2BLL __sJ2BLL = null;
        public SJ2StatisticsBLL()
        {
            __sJ2BLL = BLLFactory.CreateSJ2BLL();
        }
        public bool SJ2StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ2StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ2StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ2AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ2Statistics = GetEntityById(id);
                if (sJ2Statistics == null) throw new Exception("SJ2报表统计为空");
                sJ2Statistics.Status = (int)sJ2AuditStatus;
                sJ2Statistics.UpdateUserId = userId.Value;
                sJ2Statistics.UpdateTime = DateTime.Now;
                if (sJ2AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ2Statistics.AuditUserId = null;
                    sJ2Statistics.AuditTime = null;
                }
                else
                {
                    sJ2Statistics.AuditUserId = userId.Value;
                    sJ2Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ2Statistics[] { sJ2Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ2StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ2Statistics = GetEntityById(id);
                if (sJ2Statistics == null) throw new Exception("SJ2报表统计为空");
                var sJ2List = __sJ2BLL.GetEntitiesByExpression(string.Format("SJ2StatisticsId=\"{0}\"", id));
                if(sJ2List != null && sJ2List.Count() > 0)  
                    __sJ2BLL.Delete(sJ2List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ2Statistics.Id }, ref tran, true);
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