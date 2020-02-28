using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Business;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 自动取消预约管理者 </summary>
    public class AutoCancelAppointmentManager : IAutoCancelAppointmentManager
    {
        private static IViewAppointmentBLL __viewAppointmentBLL;
        private static IUsedConfirmBLL __usedConfirmBLL;
        private static IAppointmentBLL __appointmentBLL;
        private static IPunishRecordBLL __punishRecordBLL;
        private static IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        private static IBadBehaviorPunishManager __badBehaviorPunishManager = new BadBehaviorPunishManager();
        private static IUserBLL __userBLL;
        private static object __lockObjCreate = new object();
        private static IAutoCancelAppointmentManager __autoCancelAppointmentManager;
        private User __cancelOperator;
        private bool __isRegisterAppoinmentsOfTimeout;
        static AutoCancelAppointmentManager()
        {
            __viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
            __delinquencyConfirmBLL = BLLFactory.CreateDelinquencyConfirmBLL();
        }
        private AutoCancelAppointmentManager(string cancelOperatorLoginName, bool isRegisterAppoinmentsOfTimeout) 
        {
            __cancelOperator = __userBLL.GetUserByLabel(cancelOperatorLoginName);
            __isRegisterAppoinmentsOfTimeout = isRegisterAppoinmentsOfTimeout;
        }
        public static IAutoCancelAppointmentManager GetInstance(string cancelOperatorLoginName, bool isRegisterAppoinmentsOfTimeout)
        {
            if (__autoCancelAppointmentManager == null)
            {
                lock (__lockObjCreate)
                {
                    __autoCancelAppointmentManager = new AutoCancelAppointmentManager(cancelOperatorLoginName, isRegisterAppoinmentsOfTimeout);
                }
            }
            return __autoCancelAppointmentManager;
        }
        public void Cancel(out int totalCount, out int successCount, out int failCount, out string errorMessage)
        {
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            try
            {

                var viewAppointmentList = __viewAppointmentBLL.GetWillAutoCancleAppointmentList();
                if (viewAppointmentList != null && viewAppointmentList.Count > 0)
                {
                    foreach (var viewAppointment in viewAppointmentList)
                    {
                        var now = DateTime.Now;
                        if (viewAppointment.BeginTime <= now.AddMinutes(viewAppointment.AutoCancelAppoinmentMinutes*-1))
                        {
                            totalCount++;
                            try
                            {
                                var usedConfirmCount = __usedConfirmBLL.CountModelListByExpression(string.Format("AppointmentId=\"{0}\"", viewAppointment.Id));
                                if (usedConfirmCount > 0 || viewAppointment.StatusEnum!= Model.Enum.AppointmentStatus.Waitting) continue;
                                AppointmentMidiator appointmentMidiator = new AppointmentMidiator(viewAppointment.Appointment, true);
                                AppointmentManagerBase appointmentManager = AppointmentManagerFactoryMethod.CreateAppointmentManager(appointmentMidiator, AppointmentBusinessType.CancelAppointment, __cancelOperator.Id, "");
                                if (!appointmentManager.Handler(out errorMessage)) throw new Exception(errorMessage);
                                if (__isRegisterAppoinmentsOfTimeout)
                                {
                                    var delinquencyRule = __appointmentBLL.RegisterBreakAppointment(true, viewAppointment.Appointment, __cancelOperator.Id);
                                    if (delinquencyRule != null)
                                    {
                                        var punishRecord = __punishRecordBLL.GetUserPubishRecord(viewAppointment.UserId.Value);
                                        if (punishRecord != null)
                                        {
                                            DateTime? endDate = null;
                                            if (delinquencyRule.PersisDays.HasValue) endDate = DateTime.Now.AddDays(delinquencyRule.PersisDays.Value);
                                            __delinquencyConfirmBLL.Punish(punishRecord, delinquencyRule, DateTime.Now.Date, endDate, __badBehaviorPunishManager.MakePunishActionContent(delinquencyRule, viewAppointment.UserName, punishRecord, DateTime.Now.Date, delinquencyRule.PersisDays), __cancelOperator.Id);
                                        }
                                    }
                                }
                                successCount++;
                            }
                            catch (Exception ex)
                            {
                                errorMessage = ex.Message;
                                failCount++;
                            }
                        }

                    }
                }
            }
            catch (Exception ex) { errorMessage = ex.Message; }
        }
        
    }
}
