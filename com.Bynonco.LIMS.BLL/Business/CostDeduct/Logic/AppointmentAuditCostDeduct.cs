using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentAuditCostDeduct:CommondCostDeduct
    {
        private Appointment __appointment;
        private UserAccount __userAccount;
        public AppointmentAuditCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP) 
        {
            __appointment = businessObjects[0] as Appointment;
            if (businessObjects.Length > 1 && businessObjects[1] != null)
                __userAccount = (UserAccount)businessObjects[1];
            if (businessObjects.Length > 2 && businessObjects[2] != null)
                _isDeductSubjectHook = bool.Parse(businessObjects[2].ToString());
        }

        protected override Model.CostDeduct CreateCostDeduct()
        {
            var costDeduct = _costDeductBLL.CreateAppointmentCostDeduct(__appointment, __userAccount, null);
            costDeduct.AppointmentForLog = __appointment;
            return costDeduct;
        }
        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            return _costDeductBLL.HandleAppointmentCostDeduct(__appointment, out subjectId, out userId, out paymentMethod);
        }
        protected override bool SendDeductMessage(UserAccount userAccount, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                return _messageHandler.SendAppointmentCostDeductNotice(__appointment, null);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
