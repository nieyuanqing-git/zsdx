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
    public class AutoDeductManager : IAutoDeductManager
    {
        private static IAppointmentBLL __appointmentBLL;
        private static object __lockObjCreate = new object();
        private static object __lockObjBusiness = new object();
        private static IAutoDeductManager __autoDeductManager;
        private static AutoDeductSetting __autoDeductSetting;
        private static ICostDeductManager __costDeductManager;
        static AutoDeductManager()
        {
            __appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __costDeductManager = new CostDeductManager();
        }
        private AutoDeductManager(AutoDeductSetting autoDeductSetting)
        {
            if (__autoDeductSetting == null)
                __autoDeductSetting = autoDeductSetting;
        }
        public static IAutoDeductManager GetInstance(AutoDeductSetting autoDeductSetting)
        {
            if (__autoDeductManager == null)
            {
                lock (__lockObjCreate)
                {
                    __autoDeductManager = new AutoDeductManager(autoDeductSetting);
                }
            }
            return __autoDeductManager;
        }
        public void Deduct(out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (__autoDeductSetting.Equipments != null && __autoDeductSetting.Equipments.Count() > 0)
                {
                    lock(__lockObjBusiness)
                    {
                        DateTime beginDate = DateTime.Now.Date.AddDays(__autoDeductSetting.Days);
                        var queryExpression = string.Format("Status={0}*BeginTime>\"{1}\"*((IsNeedAudit=false+IsNeedAudit=null)+(IsNeedAudit=true*AuditingStatus={2}))",(int)AppointmentStatus.Waitting,beginDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentAuditStatus.Pass);
                        queryExpression += "*" + __autoDeductSetting.EquipmentIds.ToFormatStr("EquipmentId");
                        var appointmentList = __appointmentBLL.GetEntitiesByExpression(queryExpression);
                        if (appointmentList != null && appointmentList.Count > 0)
                        {
                            foreach (var appointment in appointmentList)
                            {
                                XTransaction tran = null;
                                try
                                {
                                    if ((appointment.BeginTime.Value - DateTime.Now).TotalMinutes < (appointment.Equipment.MinAppointmentCancelTime.HasValue ? appointment.Equipment.MinAppointmentCancelTime.Value : 0))
                                    {
                                        tran = SessionManager.Instance.BeginTransaction();
                                        __costDeductManager.AppointmenDeduct(appointment, appointment.BeginTime.Value, appointment.EndTime.Value, null, ref tran, null, "");
                                        if (tran != null && tran.HasTransaction) tran.CommitTransaction();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorMessage = ex.Message;
                                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                                }
                                finally { if (tran != null) tran.Dispose(); }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
