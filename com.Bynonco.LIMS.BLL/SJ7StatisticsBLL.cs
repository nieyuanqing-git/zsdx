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
    public class SJ7StatisticsBLL : BLLBase<SJ7Statistics>, ISJ7StatisticsBLL
    {
        private ISJ7BLL __sJ7BLL = null;
        public SJ7StatisticsBLL()
        {
            __sJ7BLL = BLLFactory.CreateSJ7BLL();
        }
        public bool SJ7StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ7StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ7StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ7AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ7Statistics = GetEntityById(id);
                if (sJ7Statistics == null) throw new Exception("SJ7报表统计为空");
                sJ7Statistics.Status = (int)sJ7AuditStatus;
                sJ7Statistics.UpdateUserId = userId.Value;
                sJ7Statistics.UpdateTime = DateTime.Now;
                if (sJ7AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ7Statistics.AuditUserId = null;
                    sJ7Statistics.AuditTime = null;
                }
                else
                {
                    sJ7Statistics.AuditUserId = userId.Value;
                    sJ7Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ7Statistics[] { sJ7Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ7StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ7Statistics = GetEntityById(id);
                if (sJ7Statistics == null) throw new Exception("SJ7报表统计为空");
                var sJ7List = __sJ7BLL.GetEntitiesByExpression(string.Format("SJ7StatisticsId=\"{0}\"", id));
                if(sJ7List != null && sJ7List.Count() > 0)  
                    __sJ7BLL.Delete(sJ7List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ7Statistics.Id }, ref tran, true);
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