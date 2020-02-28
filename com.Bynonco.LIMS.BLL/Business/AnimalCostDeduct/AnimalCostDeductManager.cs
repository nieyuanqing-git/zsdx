using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;
namespace com.Bynonco.LIMS.BLL
{
    public class AnimalCostDeductManager : IAnimalCostDeductManager
    {
        private static IViewNeedCostDeductAnimalBLL __viewNeedCostDeductAnimalBLL;
        private static IAnimalBLL __animalBLL;
        private static ISystemLogBLL __systemLogBLL;
        private static object __lockObjCreate = new object();
        private static object __lockObjBusiness = new object();
        private static IAnimalCostDeductManager __animalCostDeductManager;
        private static ICostDeductManager __costDeductManager;
        static AnimalCostDeductManager()
        {
            __viewNeedCostDeductAnimalBLL = BLLFactory.CreateViewNeedCostDeductAnimalBLL();
            __animalBLL = BLLFactory.CreateAnimalBLL();
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
            __costDeductManager = new CostDeductManager();
        }
        private AnimalCostDeductManager() { }
        public static IAnimalCostDeductManager GetInstance()
        {
            if (__animalCostDeductManager == null)
            {
                lock (__lockObjCreate)
                {
                    __animalCostDeductManager = new AnimalCostDeductManager();
                }
            }
            return __animalCostDeductManager;
        }
        public void Deduct(IEnumerable<Animal> animalList, DateTime endCostDeductDate, Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage)
        {
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            Guid? businessId = null;
            StringBuilder sbErrorMessage = new StringBuilder();
            try
            {
                lock (__lockObjBusiness)
                {
                    if (animalList == null || animalList.Count() == 0) throw new Exception("扣费动物信息为空");
                    if (animalList.Count() == 1) businessId = animalList.First().Id;
                    foreach (var animal in animalList)
                    {
                        XTransaction tran = null;
                        try
                        {
                            tran = SessionManager.Instance.BeginTransaction();
                            __costDeductManager.AnimalCostDeduct(animal, ref tran, endCostDeductDate, null, "");
                            if (tran != null && tran.HasTransaction) tran.CommitTransaction();
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            sbErrorMessage.AppendLine(string.Format("动物:{0}扣费失败,原因:{1}", animal.Name, ex.Message));
                            if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                        }
                        finally { if (tran != null) tran.Dispose(); }
                    }
                    errorMessage = sbErrorMessage.ToString();
                    try
                    {
                        XTransaction logTran = null;
                        var logContent = string.Format("总数量:{0},成功数量:{1},失败数量{2},失败原因:{3}\r\n扣费截止时间:{4}\r\n 扣费动物列表:{5}", successCount + failCount, successCount, failCount, sbErrorMessage.ToString(), endCostDeductDate.ToString("yyyy-MM-dd HH:mm:ss"), string.Join(",", animalList.Select(p => p.Name)));
                        SystemLog systemLog = new SystemLog()
                        {
                            Id = Guid.NewGuid(),
                            LogContent = logContent,
                            LogContentHTML = logContent.Replace("\r\n", "<br />"),
                            OperateEntityCnName = "实验动物扣费",
                            OperateEntityName = "AnimalCostDeduct",
                            OperateIP = ip,
                            OperateTime = DateTime.Now,
                            OperateTypeEnum = OperateType.Other,
                            OperatorId = operatorId
                        };
                        if (businessId.HasValue) systemLog.BusinessId = businessId.Value;
                        __systemLogBLL.Add(new SystemLog[] { systemLog }, ref logTran);
                    }
                    catch { }
                }
                
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            
        }
        public void Deduct(Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage)
        {
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            Deduct(DateTime.Now.Date.AddSeconds(-1), operatorId, ip, out successCount, out failCount, out errorMessage);
        }
        public void Deduct(DateTime endCostDeductDate, Guid? operatorId, string ip, out int successCount, out int failCount, out string errorMessage)
        {
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            try
            {
                var needCostDeductAnimalList = __viewNeedCostDeductAnimalBLL.GetEntitiesByExpression("");
                if (needCostDeductAnimalList != null && needCostDeductAnimalList.Count > 0)
                {
                    var animalList = __animalBLL.GetEntitiesByExpression(needCostDeductAnimalList.Select(p=>p.Id).ToFormatStr());
                    Deduct(animalList, endCostDeductDate, operatorId, ip, out successCount, out failCount, out errorMessage);
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
