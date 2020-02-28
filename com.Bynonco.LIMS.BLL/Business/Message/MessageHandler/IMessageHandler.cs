using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    /// 消息处理接口
    /// <summary>
    /// 消息处理接口
    /// </summary>
    public interface IMessageHandler
    {
        bool SendPunishNotice(Model.PunishAction punishAction, User sender);
        bool SendRepealPunishNotice(Model.RepealDelinquency repealDelinquency, User sender);
        bool SendEquipmentBlackListNotice(EquipmentBlackList equipmentBlackList, User sender);
        bool SendDepositNotice(Model.UserAccount userAccount, User sender);
        bool SendDepositNotice(ExperimenterSubject subjectExperimenter, User sender);
        bool SendDepositNotice(CostDeduct costDeduct, UsedConfirm usedConfirm, User sender);
        bool SendDepositNotice(CostDeduct costDeduct, ManualCostDeduct manualCostDeduct, User sender);
        bool SendDepositNotice(CostDeduct costDeduct, MaterialOutput materialOutput, User sender);
        bool SendDepositNotice(CostDeduct costDeduct, WaterControlCostDeduct waterControlCostDeduct, User sender);
        bool SendDepositNotice(CostDeduct costDeduct, NMPUsedConfirm nmpUsedConfirm, User sender);
        bool SendDepositAuditPassNotice(DepositRecord depositRecord, User sender);
        bool SendAnimalCostDeductNotice(AnimalCostDeduct animalCostDeduct, User sender);
        bool SendUsedConfirmCostDeductNotice(UsedConfirm usedConfirm, User sender);
        bool SendAppointmentCostDeductNotice(Appointment appointment, User sender);
        bool SendManualCostDeductNotice(ManualCostDeduct manualCostDeduct, User sender);
        bool SendMaterialCostDeductNotice(MaterialOutput materialOutput, User sender);
        bool SendSampleApplyCostDeductNotice(SampleApply sampleApply, User sender);
        bool SendBalanceNotice(Model.BalanceAccount balanceAccount, User sender);
        bool SendAppointmentSuccessNotice(Appointment appointment, User sender);
        bool SendAppointmentTutorAuditNotice(Appointment appointment, User sender);
        bool SendAppointmentAuditResultNotice(ViewAppointment viewAppointment, User sender);
        bool SendAppointmentTutorAuditResultNotice(ViewAppointment viewAppointment, User sender);
        bool SendUseEquipmentOutOfTimeNotice(Appointment appointment, User sender);
        bool SendDoorWarringNotice(string doorId, DateTime optTime, string doorName, string labName, string labAddress, string receiveUserLabels, string lastOpenDoorUserLabel, User sender);
        bool SendSampleNotice(SampleApply sampleApply, string applicantName, User sender, string address, string title, string content);
        bool SendSampleNotice(SendMode sendMode, SampleApply sampleApply, string applicantName, User sender, string address, string title, string content);
        bool SendBindTutorNotice(User tutor, User student, User sender);
        bool SendSampleApplyTutorAuditNotice(SampleApply sampleApply, User sender);
        bool SendErrorChargedTypeNotice(UsedConfirm usedConfirm, User sender);
        bool SendAnimalRegisterDeathNotice(ViewAnimal viewAnimal, User sender);
        bool SendDoorAuthorizeNotice(UserDoorAuthorization originalUserDoorAuthorization, UserDoorAuthorization userDoorAuthorization, User sender);
        bool SendDoorAuthorizeNotice(UserDoorContinuedAuthorization originalUserDoorContinuedAuthorization, UserDoorContinuedAuthorization userDoorContinuedAuthorization, User sender);
        bool SendEquipmentAuthorizeNotice(UserEquipmentAuthorization originalUserEquipmentAuthorization, UserEquipmentAuthorization userEquipmentAuthorization, User sender);
        bool SendEquipmentAuthorizeNotice(UserEquipmentContinuedAuthorization originalUserEquipmentContinuedAuthorization, UserEquipmentContinuedAuthorization userEquipmentContinuedAuthorization, User sender);
        bool SendWaterControlCostDeductNotice(WaterControlCostDeduct waterControlCostDeduct, User sender);
        bool SendUserEquipmentUnPassNotice(UserEquipment userEquipment, User sender);
        bool SendDelinquencyConfirmNotice(SendMode sendMode, ViewDelinquencyConfirm viewDelinquencyConfirm, string PunisherName, User sender, string address, string title, string content);
        bool SendNMPAppointmentSuccessNotice(NMPAppointment appointment, User sender);
        bool SendNMPAppointmentTutorAuditNotice(NMPAppointment appointment, User sender);
        bool SendNMPAppointmentTutorAuditResultNotice(ViewNMPAppointment viewNMPAppointment, User sender);
        bool SendNMPAppointmentAuditResultNotice(ViewNMPAppointment viewNMPAppointment, User sender);
        bool SendNMPUsedConfirmCostDeductNotice(NMPUsedConfirm nmpUsedConfirm, User sender);
    }
}
