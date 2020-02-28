using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class RegisterAppoinmentsOfTimeoutManager : IRegisterAppoinmentsOfTimeoutManager
    {
        private static IAppointmentBLL __appointmentBLL;
        private static IUsedConfirmBLL __usedConfirmBLL;
        private static IUserBLL __userBLL;
        private static IEquipmentBLL __equipmentBLL;
        private static IPunishRecordBLL __punishRecordBLL;
        private static IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        private static IPowerOperationBLL __powerOperationBLL;
        private static IRegisterAppoinmentsOfTimeoutManager __registerAppoinmentsOfTimeoutManager;
        private static IBadBehaviorPunishManager __badBehaviorPunishManager = new BadBehaviorPunishManager();
        private static IMessageHandler __messageHandler = new MessageHandler();
        private static object __lockObjCreate = new object();
        private int __registerCountPerTime = 10;
        private int __timeOutMinutes = 0;
        private IEnumerable<string> __equipmentLabels;
        private User __register;
        static RegisterAppoinmentsOfTimeoutManager()
        {
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
            __delinquencyConfirmBLL = BLLFactory.CreateDelinquencyConfirmBLL();
            __powerOperationBLL = BLLFactory.CreatePowerOperationBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
        }
        private RegisterAppoinmentsOfTimeoutManager(string registerLoginName,string equimentLabels,int registerCountPerTime,int timeOutMinutes) 
        {
            __registerCountPerTime = registerCountPerTime;
            __timeOutMinutes = timeOutMinutes;
            __register = __userBLL.GetUserByLabel(registerLoginName);
            if (!string.IsNullOrWhiteSpace(equimentLabels))__equipmentLabels = equimentLabels.Replace("，", ",").Split(',');
            
        }
        public static IRegisterAppoinmentsOfTimeoutManager GetInstance(string registerLoginName, string equimentLabels, int registerCountPerTime, int timeOutMinutes)
        {
            if (__registerAppoinmentsOfTimeoutManager == null)
            {
                lock (__lockObjCreate)
                {
                    __registerAppoinmentsOfTimeoutManager = new RegisterAppoinmentsOfTimeoutManager(registerLoginName, equimentLabels, registerCountPerTime, timeOutMinutes);
                }
            }
            return __registerAppoinmentsOfTimeoutManager;
        }
        public void Register(out int totalCount, out int successCount, out int failCount, out string errorMessage)
        {
            errorMessage = "";
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            try
            {
                var queryExpression = string.Format("EndTime<\"{0}\"*Status={1}*(IsNeedAudit=false+(IsNeedAudit=true*AuditingStatus={2}))", DateTime.Now.AddMinutes(__timeOutMinutes * -1), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.Pass);
                if (__equipmentLabels != null && __equipmentLabels.Count() > 0)
                {
                    var equipmentList = __equipmentBLL.GetEquipmentListByLabels(__equipmentLabels.ToArray());
                    queryExpression += "*" + (
                            equipmentList != null && equipmentList.Count > 0 ?
                            equipmentList.Select(p => p.Id).ToFormatStr("EquipmentId") : 
                            "EquipmentId=null"
                        );
                }
                var timeOutAppointmentList = __appointmentBLL.GetScopeEntitiesByExpression(queryExpression, 1, __registerCountPerTime, null, "EndTime D");
                if (timeOutAppointmentList != null && timeOutAppointmentList.Count > 0)
                {
                    totalCount = timeOutAppointmentList.Count;
                    foreach (var timeOutAppointment in timeOutAppointmentList)
                    {
                        try
                        {
                            var count = __powerOperationBLL.CountModelListByExpression(string.Format("AppointmentId=\"{0}\"", timeOutAppointment.Id));
                            if (count == 0) count = __usedConfirmBLL.CountModelListByExpression(string.Format("AppointmentId=\"{0}\"", timeOutAppointment.Id));
                            if (count > 0)
                            {
                                totalCount--;
                                continue;
                            }
                            var delinquencyRule =  __appointmentBLL.RegisterBreakAppointment(false,timeOutAppointment, __register.Id);
                            if (delinquencyRule != null)
                            {
                                var punishRecord = __punishRecordBLL.GetUserPubishRecord(timeOutAppointment.UserId.Value);
                                if (punishRecord != null)
                                {
                                    DateTime? endDate = null;
                                    if (delinquencyRule.PersisDays.HasValue) endDate = DateTime.Now.AddDays(delinquencyRule.PersisDays.Value);
                                    __delinquencyConfirmBLL.Punish(punishRecord, delinquencyRule, DateTime.Now.Date, endDate, __badBehaviorPunishManager.MakePunishActionContent(delinquencyRule, timeOutAppointment.User.Name, punishRecord, DateTime.Now.Date, delinquencyRule.PersisDays), __register.Id);
                                }
                            }
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            errorMessage += string.IsNullOrWhiteSpace(ex.Message) ? ex.Message : "\r\n" + ex.Message;
                            failCount++;
                        }
                    }
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; }
        }
    }
}
