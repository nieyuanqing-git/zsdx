using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>
    /// 成本扣费管理者接口
    /// </summary>
    public interface ICostDeductManager
    {
        CostDeduct AppointmentPredictDeduct(Appointment appointment, IList<ViewOpenFundDetail> viewOpenFundDetails, UserAccount userAccount, ref XTransaction tran, Guid? operatorId, string operatorIP, bool isDeductSubject = true, IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFundd = null, bool isSaveEquipmentOpenFun = true);
        /// <summary> 取消预约预扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <param name="isDeductSubject"></param>
        /// <param name="isSaveEquipmentOpenFun"></param>
        /// <returns></returns>
        UserAccount CancelAppointmentPredictDeduct(Model.Appointment appointment, ref XTransaction tran, Guid? operatorId, string operatorIP, bool isDeductSubject = true, bool isSaveEquipmentOpenFun = true);
        CostDeduct AppointmentAuditDeduct(Appointment appointment, UserAccount userAccount, ref XTransaction tran, Guid? operatorId, string operatorIP);
        UserAccount CancelAppointmentAuditDeduct(Model.Appointment appointment, ref XTransaction tran, Guid? operatorId, string operatorIP);
        /// <summary> 预约扣费 </summary>
        /// <param name="appointment"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="usedConfirmFeedback"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <returns></returns>
        UsedConfirm AppointmenDeduct(Model.Appointment appointment, DateTime startAt, DateTime endAt, UsedConfirmFeedBack usedConfirmFeedback, ref XTransaction tran, Guid? operatorId, string operatorIP);
        /// <summary> 新使用记录 </summary>
        /// <param name="recordId"></param>
        /// <param name="userLabel"></param>
        /// <param name="equipmentLabel"></param>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <param name="isDeduct"></param>
        /// <param name="subjectInfo"></param>
        /// <param name="remarkInfo"></param>
        /// <param name="usedConfirmFeedback"></param>
        /// <param name="errorMsg"></param>
        /// <param name="tran"></param>
        /// <param name="operatorId"></param>
        /// <param name="operatorIP"></param>
        /// <returns></returns>
        UsedConfirm NewUsedRecordDeduct(string recordId, string userLabel, string equipmentLabel, DateTime startAt, DateTime endAt, bool isDeduct, string subjectInfo, string remarkInfo, UsedConfirmFeedBack usedConfirmFeedback, out string errorMsg, ref XTransaction tran, Guid? operatorId, string operatorIP);
        UsedConfirm UsedConfirmCostDeduct(UsedConfirm usedConfirm, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP);
        UsedConfirm CancelUsedConfirmCostDeduct(UsedConfirm usedConfirm, ref XTransaction tran, Guid? operatorId, string operatorIP);
        ManualCostDeduct ManualCostDeduct(ManualCostDeduct manualCostDeduct, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP);
        void CancleManualCostDeduct(ManualCostDeduct manualCostDeduct, ref XTransaction tran, Guid? operatorId, string operatorIP);
        AnimalCostDeduct AnimalCostDeduct(Animal animal, ref XTransaction tran,DateTime endCostDeductDate, Guid? operatorId, string operatorIP);
        void CancleAnimalCostDeduct(Animal animal, AnimalCostDeduct animalCostDeduct, ref XTransaction tran, Guid? operatorId, string operatorIP);
        MaterialOutput MaterialCostDeduct(MaterialOutput materialOutput, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP);
        void CancelMaterialCostDeduct(MaterialOutput materialOutput, ref XTransaction tran, Guid? operatorId, string operatorIP);
        NMPUsedConfirm NMPAppointmenDeduct(Model.NMPAppointment nmpAppointment, DateTime startAt, DateTime endAt, NMPUsedConfirmFeedBack nmpUsedConfirmFeedBack, ref XTransaction tran, Guid? operatorId, string operatorIP);
        NMPUsedConfirm NMPNewUsedRecordDeduct(string recordId, string userLabel, string equipmentLabel, DateTime startAt, DateTime endAt, bool isDeduct, string subjectInfo, string remarkInfo, NMPUsedConfirmFeedBack nmpUsedConfirmFeedBack, out string errorMsg, ref XTransaction tran, Guid? operatorId, string operatorIP);
        NMPUsedConfirm NMPUsedConfirmCostDeduct(NMPUsedConfirm nmpUsedConfirm, Guid operatorId, bool isAllowAccountMinuse, bool isSuperAmin, ref XTransaction tran, string operatorIP);
        NMPUsedConfirm CancelNMPUsedConfirmCostDeduct(NMPUsedConfirm nmpUsedConfirm, ref XTransaction tran, Guid? operatorId, string operatorIP);
    }
}
