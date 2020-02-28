using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>预约预成本扣费</summary>
    public class AppointmentPredictCostDeduct :CommondCostDeduct
    {
        //private IAppointmentBLL __appointmentBLL;
        /// <summary>设备预约</summary>
        private Appointment __appointment;
        /// <summary>用户账号</summary>
        private UserAccount __userAccount;
        /// <summary>构造函数</summary>
        /// <param name="businessObjects">
        /// appointment, 设备预约
        /// userAccount, 用户账号
        /// isDeductSubject, 是否扣费课题组
        /// costDeductEquipmentOpenFund, 开放基金扣费设备
        /// isSaveEquipmentOpenFun, 是否保存开放基金设备
        /// viewOpenFundDetails 开放基金明细视图
        /// </param>
        /// <param name="operatorId">用户ID</param>
        /// <param name="operatorIP">终端IP</param>
        public AppointmentPredictCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP) 
        {
            //__appointmentBLL = BLLFactory.CreateAppointmentBLL();
            __appointment = businessObjects[0] as Appointment;
            if (businessObjects.Length > 1 && businessObjects[1] != null)
                __userAccount = (UserAccount)businessObjects[1];
            if (businessObjects.Length > 2 && businessObjects[2] != null)
                _isDeductSubjectHook = bool.Parse(businessObjects[2].ToString());
            if (businessObjects.Length > 3 && businessObjects[3] != null)
                _preCostDeductEquipmentOpenFunds = (IList<CostDeductEquipmentOpenFund>)businessObjects[3];
            if (businessObjects.Length > 4 && businessObjects[4] != null)
                _isSaveEquipmentOpenFund = (bool)businessObjects[4];
            if(businessObjects.Length > 5 && businessObjects[5] != null)
                _viewOpenFundDetails = (IList<ViewOpenFundDetail>)businessObjects[5];
        }
        protected override Model.CostDeduct CreateCostDeduct()
        {
            var costDeduct = _costDeductBLL.CreateAppointmentCostDeduct(__appointment, __userAccount, _viewOpenFundDetails);
            costDeduct.AppointmentForLog = __appointment;
            return costDeduct;
        }
        protected override CostDeduct BusinessHandle(out Guid subjectId, out Guid userId, out PaymentMethod paymentMethod)
        {
            return _costDeductBLL.HandleAppointmentCostDeduct(__appointment, out subjectId, out userId, out paymentMethod);
        }
        protected override void DeductHandle(ref com.august.DataLink.XTransaction tran, com.Bynonco.LIMS.Model.CostDeduct costDeduct, com.Bynonco.LIMS.Model.CostDeduct originalCostDeduct, bool isNew) { }
        protected override void CancelDeductHandle(ref XTransaction tran, UserAccount userAccount) { }
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
