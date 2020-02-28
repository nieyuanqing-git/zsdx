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
    public class SJ4StatisticsBLL : BLLBase<SJ4Statistics>, ISJ4StatisticsBLL
    {
        private ISJ4BLL __sJ4BLL = null;
        public SJ4StatisticsBLL()
        {
            __sJ4BLL = BLLFactory.CreateSJ4BLL();
        }
        public bool SJ4StatisticsAdd(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, returnValueDataParam };
                ProcedureAdapter.ExecuteProcedure("ProSJ4StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }

        public bool SJ4StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ4AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ4Statistics = GetEntityById(id);
                if (sJ4Statistics == null) throw new Exception("SJ4报表统计为空");
                sJ4Statistics.Status = (int)sJ4AuditStatus;
                sJ4Statistics.UpdateUserId = userId.Value;
                sJ4Statistics.UpdateTime = DateTime.Now;
                if (sJ4AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ4Statistics.AuditUserId = null;
                    sJ4Statistics.AuditTime = null;
                }
                else
                {
                    sJ4Statistics.AuditUserId = userId.Value;
                    sJ4Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ4Statistics[] { sJ4Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ4StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ4Statistics = GetEntityById(id);
                if (sJ4Statistics == null) throw new Exception("SJ4报表统计为空");
                var sJ4List = __sJ4BLL.GetEntitiesByExpression(string.Format("SJ4StatisticsId=\"{0}\"", id));
                if(sJ4List != null && sJ4List.Count() > 0)  
                    __sJ4BLL.Delete(sJ4List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ4Statistics.Id }, ref tran, true);
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