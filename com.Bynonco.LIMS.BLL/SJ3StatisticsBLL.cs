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
using com.Bynonco.LIMS.Model.Business.CERS.ShareEffect;

namespace com.Bynonco.LIMS.BLL
{
    public class SJ3StatisticsBLL : BLLBase<SJ3Statistics>, ISJ3StatisticsBLL
    {
        private ISJ3BLL __sJ3BLL = null;
        private ISJ3StatisticsUsedHourBLL __sJ3StatisticsUsedHourBLL = null;
        private IDictCodeBLL __dictCodeBLL = null;
        public SJ3StatisticsBLL()
        {
            __sJ3BLL = BLLFactory.CreateSJ3BLL();
            __sJ3StatisticsUsedHourBLL = BLLFactory.CreateSJ3StatisticsUsedHourBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public bool SJ3StatisticsAdd(Guid? userId, double? unitPrice, DateTime? startAt,DateTime? endAt, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (!unitPrice.HasValue) unitPrice = 400000;
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var createrDataParam = DataParameterFactory.CreateDataParameter("userId", userId);
                var statisticsUnitPriceDataParam = DataParameterFactory.CreateDataParameter("statisticsUnitPrice",unitPrice.Value.ToString());
                var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
                
                List<IDataParameter> dataParams = new List<IDataParameter>() { createrDataParam, statisticsUnitPriceDataParam, returnValueDataParam };
                if (startAt.HasValue)
                    dataParams.Add(DataParameterFactory.CreateDataParameter("StartAt", startAt.Value.ToString("yyyy-MM-dd")));
                else
                    dataParams.Add(DataParameterFactory.CreateDataParameter("StartAt", DBNull.Value));
                if (endAt.HasValue)
                    dataParams.Add(DataParameterFactory.CreateDataParameter("EndAt", endAt.Value.ToString("yyyy-MM-dd 23:59:59")));
                else
                    dataParams.Add(DataParameterFactory.CreateDataParameter("EndAt", DBNull.Value));
                ProcedureAdapter.ExecuteProcedure("ProSJ3StatisticsAdd", dataParams);
                return returnValueDataParam.Value == null || returnValueDataParam.Value.ToString() != "1" ? false : true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ3StatisticsChangeStatus(Guid? userId, Guid id, StatisticsReportAuditStatus sJ3AuditStatus, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                XTransaction tran = null;
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ3Statistics = GetEntityById(id);
                if (sJ3Statistics == null) throw new Exception("SJ3报表统计为空");
                sJ3Statistics.Status = (int)sJ3AuditStatus;
                sJ3Statistics.UpdateUserId = userId.Value;
                sJ3Statistics.UpdateTime = DateTime.Now;
                if (sJ3AuditStatus == StatisticsReportAuditStatus.WaitingAudit)
                {
                    sJ3Statistics.AuditUserId = null;
                    sJ3Statistics.AuditTime = null;
                }
                else
                {
                    sJ3Statistics.AuditUserId = userId.Value;
                    sJ3Statistics.AuditTime = DateTime.Now;
                }
                Save(new SJ3Statistics[] { sJ3Statistics }, ref tran);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
        }
        public bool SJ3StatisticsDelete(Guid? userId, Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("用户为空");
                var sJ3Statistics = GetEntityById(id);
                if (sJ3Statistics == null) throw new Exception("SJ3报表统计为空");
                var sJ3StatisticsUsedHourList = __sJ3StatisticsUsedHourBLL.GetEntitiesByExpression(string.Format("SJ3StatisticsId=\"{0}\"", id));
                if (sJ3StatisticsUsedHourList != null && sJ3StatisticsUsedHourList.Count() > 0)
                    __sJ3StatisticsUsedHourBLL.Delete(sJ3StatisticsUsedHourList.Select(p => p.Id), ref tran, true);
                var sJ3List = __sJ3BLL.GetEntitiesByExpression(string.Format("SJ3StatisticsId=\"{0}\"", id));
                if(sJ3List != null && sJ3List.Count() > 0)  
                    __sJ3BLL.Delete(sJ3List.Select(p => p.Id), ref tran, true);
                Delete(new Guid[] { sJ3Statistics.Id }, ref tran, true);
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
        public ShareEffect FillShareEffect()
        {
            var sJ3Statistics = GetFirstOrDefaultEntityByExpression("", null, "CreateTime D");
            if(sJ3Statistics == null) return null;
            ShareEffect shareEffect = new ShareEffect();
            if(sJ3Statistics.SJ3 != null && sJ3Statistics.SJ3.Count() > 0)
            {
                var schoolCode = __dictCodeBLL.GetDictCodeValueByCode("System","SchoolCode");
                var year = sJ3Statistics.StartAt.HasValue ? sJ3Statistics.StartAt.Value.ToString("yyyy") : DateTime.Now.ToString("yyyy");
                shareEffect.Instrus = new Instru[sJ3Statistics.SJ3.Count()];
                for(int i=0;i < sJ3Statistics.SJ3.Count(); i++)
                {
                    shareEffect.Instrus[i] = FillShareEffectInstru(sJ3Statistics.SJ3[i]);
                    if (shareEffect.Instrus[i] != null)
                    {
                        shareEffect.Instrus[i].SchoolCode = schoolCode;
                        shareEffect.Instrus[i].YEAR = year;
                    }
                }
            }
            return shareEffect;
        }

        private Instru FillShareEffectInstru(SJ3 sj3)
        {
            var instru = new Instru();
            instru.InnerID = sj3.Label.Trim();
            instru.LSNUSEDHRS = string.IsNullOrWhiteSpace(sj3.UsedHourEducation) ? 0 : int.Parse(sj3.UsedHourEducation);
            instru.RSCHUSEDHRS = string.IsNullOrWhiteSpace(sj3.UsedHourScientificResearch) ? 0 : int.Parse(sj3.UsedHourScientificResearch);
            instru.SERUSEDHRS = string.IsNullOrWhiteSpace(sj3.UsedHourSocialServices) ? 0 : int.Parse(sj3.UsedHourSocialServices);
            instru.OPENHRS = string.IsNullOrWhiteSpace(sj3.UsedHourOpen) ? 0 : int.Parse(sj3.UsedHourOpen);
            instru.SAMPLENUM = string.IsNullOrWhiteSpace(sj3.SampleItemCount) ? 0 : int.Parse(sj3.SampleItemCount);
            instru.TRNSTUD = string.IsNullOrWhiteSpace(sj3.TrainingStudent) ? 0 : int.Parse(sj3.TrainingStudent);
            instru.TRNTEACH = string.IsNullOrWhiteSpace(sj3.TrainingTutor) ? 0 : int.Parse(sj3.TrainingTutor);
            instru.TRNOTHERS = string.IsNullOrWhiteSpace(sj3.TrainingOther) ? 0 : int.Parse(sj3.TrainingOther);
            instru.EDUPROJ = string.IsNullOrWhiteSpace(sj3.ProjectEducation) ? 0 : int.Parse(sj3.ProjectEducation);
            instru.RSCHPROJ = string.IsNullOrWhiteSpace(sj3.ProjectScientificResearch) ? 0 : int.Parse(sj3.ProjectScientificResearch);
            instru.SOCIALPROJ = string.IsNullOrWhiteSpace(sj3.ProjectSocialServices) ? 0 : int.Parse(sj3.ProjectSocialServices);
            instru.RWDNATION = string.IsNullOrWhiteSpace(sj3.AwardsCountry) ? 0 : int.Parse(sj3.AwardsCountry);
            instru.RWDPROV = string.IsNullOrWhiteSpace(sj3.AwardsProvince) ? 0 : int.Parse(sj3.AwardsProvince);
            instru.RWDTEACH = string.IsNullOrWhiteSpace(sj3.PatentTutor) ? 0 : int.Parse(sj3.PatentTutor);
            instru.RWDSTUD = string.IsNullOrWhiteSpace(sj3.PatentStudent) ? 0 : int.Parse(sj3.PatentStudent);
            instru.PAPERINDEX = string.IsNullOrWhiteSpace(sj3.ThesisThreeSearch) ? 0 : int.Parse(sj3.ThesisThreeSearch);
            instru.PAPERKERNEL = string.IsNullOrWhiteSpace(sj3.ThesisPublication) ? 0 : int.Parse(sj3.ThesisPublication);
            instru.CHARGEMAN = sj3.AdminName.Trim();
            instru.OtherInfo = "";
            return instru;
        }
    }
}