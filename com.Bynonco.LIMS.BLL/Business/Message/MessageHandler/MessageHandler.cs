using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class MessageHandler:IMessageHandler
    {
        public bool SendPunishNotice(Model.PunishAction punishAction,User sender)
        {
            try
            {
                return new PunishMessageManager(new object[] { punishAction }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendPunishNotice(SendMode sendMode, PunishAction punishAction, User sender)
        {
            try
            {
                return new PunishMessageManager(new object[] { punishAction }, sender).Send(sendMode);
            }
            catch { return false; }
        }
        public bool SendRepealPunishNotice(Model.RepealDelinquency repealDelinquency, User sender)
        {
            try
            {
                return new RepaelPunishMessageManager(new object[] { repealDelinquency }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendEquipmentBlackListNotice(EquipmentBlackList equipmentBlackList, User sender)
        {
            try
            {
                return new EqBlackListMessageManager(new object[] { equipmentBlackList }, sender).SendSeparately();
            }
            catch { return false; }
        }
        public bool SendDepositNotice(Model.UserAccount userAccount, User sender)
        {
            try
            {
                return new UserAccountMessageManager(new object[] { userAccount }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDepositNotice(ExperimenterSubject subjectExperimenter,User sender)
        {
            try
            {
                return new ExperimenterSubjectMessageManager(new object[] { subjectExperimenter }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDepositAuditPassNotice(DepositRecord depositRecord, User sender)
        {
            try
            {
                return new DepositAuditPassMessageManager(new object[] { depositRecord }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDepositAuditRejectNotice(DepositRecord depositRecord, User sender)
        {
            try
            {
                return new DepositAuditRejectMessageManager(new object[] { depositRecord }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendBalanceNotice(Model.BalanceAccount balanceAccount, User sender)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch { return false; }
        }
        public bool SendAppointmentSuccessNotice(Appointment appointment, User sender)
        {
            try
            {
                return new AppointmentSuccessMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendAppointmentTutorAuditResultNotice(ViewAppointment viewAppointment, User sender)
        {
            try
            {
                return new AppointmentTutorAuditResultMessageManager(new object[] { viewAppointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendAppointmentAuditResultNotice(ViewAppointment viewAppointment, User sender)
        {
            try
            {
                return new AppointmentAuditMessageManager(new object[] { viewAppointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendAppointmentTutorAuditNotice(Appointment appointment, User sender)
        {
            try
            {
                return new AppointmentTutorAuditMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendUseEquipmentOutOfTimeNotice(Appointment appointment, User sender)
        {
            try
            {
                return new UseEquipmentOutOfTimeMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDepositNotice(CostDeduct costDeduct, ManualCostDeduct manualCostDeduct, User sender)
        {
            return SendDepositNotice(costDeduct, manualCostDeduct.PaymentMethodEnum, manualCostDeduct.UserId, manualCostDeduct.Subject, sender);
        }
        public bool SendDepositNotice(CostDeduct costDeduct, MaterialOutput materialOutput, User sender)
        {
            return SendDepositNotice(costDeduct, materialOutput.PaymentMethodEnum, materialOutput.OutputUserId, materialOutput.Subject, sender);
        }
        public bool SendDepositNotice(CostDeduct costDeduct, UsedConfirm usedConfirm, User sender)
        {
            return SendDepositNotice(costDeduct, usedConfirm.PaymentMethodEnum, usedConfirm.UserId, usedConfirm.Subject, sender);
        }
        public bool SendDepositNotice(CostDeduct costDeduct, WaterControlCostDeduct waterControlCostDeduct, User sender)
        {
            return SendDepositNotice(costDeduct, waterControlCostDeduct.PaymentMethodEnum, waterControlCostDeduct.UserId, waterControlCostDeduct.Subject, sender);
        }
        public bool SendDepositNotice(CostDeduct costDeduct, NMPUsedConfirm nmpUsedConfirm, User sender)
        {
            return SendDepositNotice(costDeduct, nmpUsedConfirm.PaymentMethodEnum, nmpUsedConfirm.UserId, nmpUsedConfirm.Subject, sender);
        }
        private bool SendDepositNotice(CostDeduct costDeduct, PaymentMethod paymentMethod,Guid uerId, Subject subject,User sender)
        {
            try
            {
                bool result = SendDepositNotice(costDeduct.UserAccount, sender);
                if (paymentMethod == PaymentMethod.SubjectDirectorPay)
                {
                    var subjectExperimenter = subject.Experiments != null && subject.Experiments.Count > 0 ?
                        subject.Experiments.FirstOrDefault(x => x.Experimenter.Id == uerId)
                        : null;
                    if (subjectExperimenter != null)
                        result = SendDepositNotice(subjectExperimenter, sender);
                }
                return result;
            }
            catch { return false; }
        }
        public bool SendAnimalCostDeductNotice(AnimalCostDeduct animalCostDeduct, User sender)
        {
            try
            {
                return new AnimalCostDeductMessageManager(new object[] { animalCostDeduct }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendUsedConfirmCostDeductNotice(UsedConfirm usedConfirm, User sender)
        {
            try
            {
                return new UsedConfirmCostDeductMessageManager(new object[] { usedConfirm }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendAppointmentCostDeductNotice(Appointment appointment, User sender)
        {
            try
            {
                return new AppointmentCostDeductMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendManualCostDeductNotice(ManualCostDeduct manualCostDeduct, User sender)
        {
            try
            {
                return new ManualCostDeductMessageManager(new object[] { manualCostDeduct }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendMaterialCostDeductNotice(MaterialOutput materialOutput, User sender)
        {
            try
            {
                return new MaterialCostDeductMessageManager(new object[] { materialOutput }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendSampleApplyCostDeductNotice(SampleApply sampleApply, User sender)
        {
            try
            {
                return new SampleApplyCostDeductMessageManager(new object[] { sampleApply }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendSampleNotice(SampleApply sampleApply, string applicantName, User sender, string address, string title, string content)
        {
            try
            {
                return new SampleMessageManager(new object[] { sampleApply, applicantName,address,title,content}, sender).Send();
            }
            catch { return false; }
        }
        public bool SendSampleNotice(SendMode sendMode, SampleApply sampleApply, string applicantName, User sender, string address, string title, string content)
        {
            try
            {
                return new SampleMessageManager(new object[] { sampleApply, applicantName, address, title, content }, sender).Send(sendMode);
            }
            catch { return false; }
        }
        public bool SendBindTutorNotice(User tutor, User student, User sender)
        {
            try
            {
                return new BindTutorMessageManager(new object[] { tutor, student }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendSampleApplyTutorAuditNotice(SampleApply sampleApply, User sender)
        {
            try
            {
                return new SampleApplyTutorAuditMessageManager(new object[] { sampleApply }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendErrorChargedTypeNotice(UsedConfirm usedConfirm, User sender)
        {
            try
            {
                return new ErrorChargedTypeMessageManager(new object[] { usedConfirm }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendAnimalRegisterDeathNotice(ViewAnimal viewAnimal, User sender)
        {
            try
            {
                return new AnimalRegisterDeathMessageManager(new object[] { viewAnimal }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDoorWarringNotice(string doorId, DateTime optTime, string doorName, string labName, string labAddress, string receiveUserLabels, string openDoorUserLabel, User sender)
        {
            try
            {
                return new DoorWarringMessageManager(new object[] { doorId, optTime, doorName, labName, labAddress, receiveUserLabels, openDoorUserLabel }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDoorAuthorizeNotice(UserDoorAuthorization originalUserDoorAuthorization, UserDoorAuthorization userDoorAuthorization, User sender)
        {
            try
            {
                return new DoorAuthorizeMessageManager(new object[] { originalUserDoorAuthorization,userDoorAuthorization }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDoorAuthorizeNotice(UserDoorContinuedAuthorization originalUserDoorContinuedAuthorization, UserDoorContinuedAuthorization userDoorContinuedAuthorization, User sender)
        {
            try
            {
                return new DoorContinueAuthorizeMessageManager(new object[] { originalUserDoorContinuedAuthorization, userDoorContinuedAuthorization }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendEquipmentAuthorizeNotice(UserEquipmentAuthorization originalUserEquipmentAuthorization, UserEquipmentAuthorization userEquipmentAuthorization, User sender)
        {
            try
            {
                return new EquipmentAuthorizeMessageManager(new object[] { originalUserEquipmentAuthorization, userEquipmentAuthorization }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendEquipmentAuthorizeNotice(UserEquipmentContinuedAuthorization originalUserEquipmentContinuedAuthorization, UserEquipmentContinuedAuthorization userEquipmentContinuedAuthorization, User sender)
        {
            try
            {
                return new EquipmentContinueAuthorizeMessageManager(new object[] { originalUserEquipmentContinuedAuthorization, userEquipmentContinuedAuthorization }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendWaterControlCostDeductNotice(WaterControlCostDeduct waterControlCostDeduct, User sender)
        {
            try
            {
                return new WaterControlCostDeductMessageManager(new object[] { waterControlCostDeduct }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendUserEquipmentUnPassNotice(UserEquipment userEquipment, User sender)
        {
            try
            {
                return new UserEquipmentUnPassMessageManager(new object[] { userEquipment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendDelinquencyConfirmNotice(SendMode sendMode, ViewDelinquencyConfirm viewDelinquencyConfirm, string PunisherName, User sender, string address, string title, string content)
        {
            try
            {
                return new DelinquencyConfirmMessageManager(new object[] { viewDelinquencyConfirm, PunisherName, address, title, content }, sender).Send(sendMode);
            }
            catch { return false;}
        }
        public bool SendNMPAppointmentSuccessNotice(NMPAppointment appointment, User sender)
        {
            try
            {
                return new NMPAppointmentSuccessMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendNMPAppointmentTutorAuditNotice(NMPAppointment appointment, User sender)
        {
            try
            {
                return new NMPAppointmentTutorAuditMessageManager(new object[] { appointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendNMPAppointmentTutorAuditResultNotice(ViewNMPAppointment viewNMPAppointment, User sender)
        {
            try
            {
                return new NMPAppointmentTutorAuditResultMessageManager(new object[] { viewNMPAppointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendNMPAppointmentAuditResultNotice(ViewNMPAppointment viewNMPAppointment, User sender)
        {
            try
            {
                return new NMPAppointmentAuditMessageManager(new object[] { viewNMPAppointment }, sender).Send();
            }
            catch { return false; }
        }
        public bool SendNMPUsedConfirmCostDeductNotice(NMPUsedConfirm nmpUsedConfirm, User sender)
        {
            try
            {
                return new NMPUsedConfirmCostDeductMessageManager(new object[] { nmpUsedConfirm }, sender).Send();
            }
            catch { return false; }
        }
    }
}
