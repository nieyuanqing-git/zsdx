using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleApplyReportNumberBLL : BLLBase<SampleApplyReportNumber>, ISampleApplyReportNumberBLL
    {
        public bool GenerateSampleApplyReportNumber(Guid sampleApplyId, Guid operatorId, int reportCount, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                IList<IDataParameter> dataPaeameters = new List<IDataParameter>()
                {
                    DataParameterFactory.CreateDataParameter("sampleApplyId",sampleApplyId, ParameterDirection.Input),
                    DataParameterFactory.CreateDataParameter("operatorId",operatorId, ParameterDirection.Input),
                    DataParameterFactory.CreateDataParameter("reportCount",reportCount, ParameterDirection.Input),
                };
                ProcedureAdapter.ExecuteProcedure("ProGenerateSampleApplyReportNumber", dataPaeameters);
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public bool DeleteSampleApplyReportNumber(Guid sampleApplyReportNumberId, Guid operatorId,string remark, out string errorMessage)
        {
            ISampleApplyReportNumberLogBLL sampleApplyReportNumberLogBLL = BLLFactory.CreateSampleApplyReportNumberLogBLL();
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                var sampleApplyReportNumber = GetEntityById(sampleApplyReportNumberId);
                Delete(new Guid[] { sampleApplyReportNumber.Id }, ref tran, true);
                SampleApplyReportNumberLog log = new SampleApplyReportNumberLog()
                {
                     Id = Guid.NewGuid(),
                     LogContent = string.Format("删除报告号:{0},原因:{1}", sampleApplyReportNumber.ReportNo, remark),
                     OperateTime = DateTime.Now,
                     SampleApplyId = sampleApplyReportNumber.SampleApplyId,
                     ReportNo = sampleApplyReportNumber.ReportNo,
                     OperatorId = operatorId
                };
                sampleApplyReportNumberLogBLL.Add(new SampleApplyReportNumberLog[] { log }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null && tran.HasTransaction) tran.RollbackTransaction(); 
                errorMessage = ex.Message;
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
    }
}
