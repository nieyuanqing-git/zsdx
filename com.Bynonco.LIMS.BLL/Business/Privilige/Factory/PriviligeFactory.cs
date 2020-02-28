using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class PriviligeFactory
    {
        private static readonly string nameSpace = "com.Bynonco.LIMS.BLL.Business.Privilige";
        private static readonly string assembly = "com.Bynonco.LIMS.BLL";
        private static IUserBLL __userBLL;
        static PriviligeFactory()
        {
            __userBLL = BLLFactory.CreateUserBLL();
        }
        private static PriviligeBase Resolve(object[] parameters, string controllerName)
        {
            var privilige = (PriviligeBase)Assembly.Load(assembly).CreateInstance(string.Format("{0}.{1}{2}", nameSpace, controllerName, "Privilige"), true, BindingFlags.CreateInstance, null, parameters, null, null);
            privilige = privilige.GetPrivilige();
            return privilige;
        }
        private static PriviligeBase Resolve(Guid userId, string controllerName)
        {
            return Resolve(new object[] { userId }, controllerName);
        }
        private static PriviligeBase Resolve(Guid userId, Guid entityId, string controllerName)
        {
            return Resolve(new object[] { userId, entityId }, controllerName);
        }
        private static PriviligeBase Resolve(string label, string controllerName)
        {
            var user = __userBLL.GetUserByLabel(label);
            if (user == null) return null;
            return Resolve(user.Id, controllerName);
        }
        private static PriviligeBase Resolve(string label, Guid entityId, string controllerName)
        {
            var user = __userBLL.GetUserByLabel(label);
            return Resolve(user.Id, entityId, controllerName);
        }
        public static AchievementPrivilige GetAchievementPrivilige(string label)
        {
            return (AchievementPrivilige)Resolve(label, "Achievement");
        }
        public static AppointmentPrivilige GetAppointmentPrivilige(string label)
        {
            return (AppointmentPrivilige)Resolve(label, "Appointment");
        }
        public static AppointmentPrivilige GetAppointmentPrivilige(string label, ViewAppointment viewAppointment)
        {
            return (AppointmentPrivilige)Resolve(new object[] { label, viewAppointment }, "Appointment");
        }
        public static AppointmentPrivilige GetAppointmentPrivilige(Guid userId, ViewAppointment viewAppointment)
        {
            return (AppointmentPrivilige)Resolve(new object[] { userId, viewAppointment }, "Appointment");
        }
        public static ArticleCategoryPrivilige GetArticleCategoryPrivilige(string label)
        {
            return (ArticleCategoryPrivilige)Resolve(label, "ArticleCategory");
        }
        public static ArticlePrivilige GetArticlePrivilige(string label)
        {
            return (ArticlePrivilige)Resolve(label, "Article");
        }
        public static QAPrivilige GetQAPrivilige(string label)
        {
            return (QAPrivilige)Resolve(label, "QA");
        }
        public static CreditLimitPrivilige GetCreditLimitPrivilige(string label)
        {
            return (CreditLimitPrivilige)Resolve(label, "CreditLimit");
        }
        public static CountryPrivilige GetCountryPrivilige(string label)
        {
            return (CountryPrivilige)Resolve(label, "Country");
        }
        public static CostDeductPrivilige GetCostDeductPrivilige(string label)
        {
            return (CostDeductPrivilige)Resolve(label, "CostDeduct");
        }
        public static CostDeductPrivilige GetCostDeductPrivilige(string label, Guid costDeductId)
        {
            return (CostDeductPrivilige)Resolve(label, costDeductId, "CostDeduct");
        }
        public static CostDeductPrivilige GetCostDeductPrivilige(Guid userId, Guid costDeductId)
        {
            return (CostDeductPrivilige)Resolve(userId, costDeductId, "CostDeduct");
        }
        public static CostDeductPrivilige GetCostDeductPrivilige(Guid userId)
        {
            return (CostDeductPrivilige)Resolve(userId, "CostDeduct");
        }
        public static DelinquencyPrivilige GetDelinquencyPrivilige(string label)
        {
            return (DelinquencyPrivilige)Resolve(label, "Delinquency");
        }
        public static DelinquencyPrivilige GetDelinquencyPrivilige(Guid userId)
        {
            return (DelinquencyPrivilige)Resolve(userId, "Delinquency");
        }
        public static DelinquencyPrivilige GetDelinquencyPrivilige(Guid userId, Guid punisherId)
        {
            return (DelinquencyPrivilige)Resolve(userId, punisherId, "Delinquency");
        }
        public static DepositRecordPrivilige GetDepositRecordPrivilige(string label)
        {
            return (DepositRecordPrivilige)Resolve(label, "DepositRecord");
        }
        public static DepositRecordPrivilige GetDepositRecordPrivilige(Guid userId)
        {
            return (DepositRecordPrivilige)Resolve(userId, "DepositRecord");
        }
        public static DepositRecordOpenFundPrivilige GetDepositRecordOpenFundPrivilige(string label)
        {
            return (DepositRecordOpenFundPrivilige)Resolve(label, "DepositRecordOpenFund");
        }
        public static DepositRecordOpenFundPrivilige GetDepositRecordOpenFundPrivilige(Guid userId)
        {
            return (DepositRecordOpenFundPrivilige)Resolve(userId, "DepositRecordOpenFund");
        }
        public static DictCodePrivilige GetDictCodePrivilige(string label)
        {
            return (DictCodePrivilige)Resolve(label, "DictCode");
        }
        public static DictCodeTypePrivilige GetDictCodeTypePrivilige(string label)
        {
            return (DictCodeTypePrivilige)Resolve(label, "DictCodeType");
        }
        public static DoorPrivilige GetDoorPrivilige(string label)
        {
            return (DoorPrivilige)Resolve(label, "Door");
        }
        public static EquipmentAuthorizationPrivilige GetEquipmentAuthorizationPrivilige(string label)
        {
            return (EquipmentAuthorizationPrivilige)Resolve(label, "EquipmentAuthorization");
        }
        public static EquipmentCategoryPrivilige GetEquipmentCategoryPrivilige(string label)
        {
            return (EquipmentCategoryPrivilige)Resolve(label, "EquipmentCategory");
        }
        public static EquipmentPrivilige GetEquipmentPrivilige(string label)
        {
            return (EquipmentPrivilige)Resolve(label, "Equipment");
        }
        public static EquipmentPrivilige GetEquipmentPrivilige(string label, Guid equipmentId)
        {
            return (EquipmentPrivilige)Resolve(label, equipmentId, "Equipment");
        }
        public static EquipmentPrivilige GetEquipmentPrivilige(Guid userId, Guid equipmentId)
        {
            return (EquipmentPrivilige)Resolve(userId, equipmentId, "Equipment");
        }
        public static EquipmentTrainningPrivilige GetEquipmentTrainningPrivilige(string label)
        {
            return (EquipmentTrainningPrivilige)Resolve(label, "EquipmentTrainning");
        }
        public static EquipmentTrainningPrivilige GetEquipmentTrainningPrivilige(string label, Guid equipmentId)
        {
            return (EquipmentTrainningPrivilige)Resolve(label, equipmentId, "EquipmentTrainning");
        }
        public static EquipmentTrainningPrivilige GetEquipmentTrainningPrivilige(Guid userId, Guid equipmentId)
        {
            return (EquipmentTrainningPrivilige)Resolve(userId, equipmentId, "EquipmentTrainning");
        }
        public static HolidayPrivilige GetHolidayPrivilige(string label)
        {
            return (HolidayPrivilige)Resolve(label, "Holiday");
        }
        public static HomeMenuPrivilige GetHomeMenuPrivilige(string label)
        {
            return (HomeMenuPrivilige)Resolve(label, "HomeMenu");
        }
        public static LabOrganizationPrivilige GetLabOrganizationPrivilige(string label)
        {
            return (LabOrganizationPrivilige)Resolve(label, "LabOrganization");
        }
        public static MessagePrivilige GetMessagePrivilige(string label)
        {
            return (MessagePrivilige)Resolve(label, "Message");
        }
        public static ModuleFunctionPrivilige GetModuleFunctionPrivilige(string label)
        {
            return (ModuleFunctionPrivilige)Resolve(label, "ModuleFunction");
        }
        public static SampleApplyPrivilige GetSampleApplyPrivilige(string label)
        {
            return (SampleApplyPrivilige)Resolve(label, "SampleApply");
        }
        public static SampleApplyPrivilige GetSampleApplyPrivilige(Guid userId)
        {
            return (SampleApplyPrivilige)Resolve(userId, "SampleApply");
        }
        public static SubjectPrivilige GetSubjectPrivilige(string label)
        {
            return (SubjectPrivilige)Resolve(label, "Subject");
        }
        public static SubjectPrivilige GetSubjectPrivilige(Guid userId, Guid subjectId)
        {
            return (SubjectPrivilige)Resolve(userId, subjectId, "Subject");
        }
        public static SubjectPrivilige GetSubjectPrivilige(string label, Guid subjectId, bool isCooperative)
        {
            var user = __userBLL.GetUserByLabel(label);
            return (SubjectPrivilige)Resolve(new object[] { user.Id, subjectId, isCooperative }, "Subject");
        }
        public static SubjectPrivilige GetSubjectPrivilige(Guid userId, Guid subjectId, bool isCooperative)
        {
            return (SubjectPrivilige)Resolve(new object[] { userId, subjectId, isCooperative }, "Subject");
        }
        public static TrainningExaminationPrivilige GetTrainningExaminationPrivilige(string label)
        {
            return (TrainningExaminationPrivilige)Resolve(label, "TrainningExamination");
        }
        public static TrainningMaterialPrivilige GetTrainningMaterialPrivilige(string label)
        {
            return (TrainningMaterialPrivilige)Resolve(label, "TrainningMaterial");
        }
        public static TrainningQuestionPrivilige GetTrainningQuestionPrivilige(string label)
        {
            return (TrainningQuestionPrivilige)Resolve(label, "TrainningQuestion");
        }
        public static TrainningTypePrivilige GetTrainningTypePrivilige(string label)
        {
            return (TrainningTypePrivilige)Resolve(label, "TrainningType");
        }
        public static UserEquipmentPrivilige GetUserEquipmentPrivilige(string label)
        {
            return (UserEquipmentPrivilige)Resolve(label, "UserEquipment");
        }
        public static UserEquipmentPrivilige GetUserEquipmentPrivilige(Guid userId)
        {
            return (UserEquipmentPrivilige)Resolve(userId, "UserEquipment");
        }
        public static UserEquipmentPrivilige GetUserEquipmentPrivilige(Guid userId, Guid userEquipmentId)
        {
            return (UserEquipmentPrivilige)Resolve(userId, userEquipmentId, "UserEquipment");
        }
        public static UserEquipmentExaminationPrivilige GetUserEquipmentExaminationPrivilige(string label)
        {
            return (UserEquipmentExaminationPrivilige)Resolve(label, "UserEquipmentExamination");
        }
        public static UserEquipmentExaminationPrivilige GetUserEquipmentExaminationPrivilige(Guid userId)
        {
            return (UserEquipmentExaminationPrivilige)Resolve(userId, "UserEquipmentExamination");
        }
        public static UserEquipmentExaminationPrivilige GetUserEquipmentExaminationPrivilige(string label, Guid userExaminationId)
        {
            return (UserEquipmentExaminationPrivilige)Resolve(label, userExaminationId, "UserEquipmentExamination");
        }
        public static UserEquipmentExaminationPrivilige GetUserEquipmentExaminationPrivilige(Guid userId, Guid userExaminationId)
        {
            return (UserEquipmentExaminationPrivilige)Resolve(userId, userExaminationId, "UserEquipmentExamination");
        }
        public static UserLabOrganizationExaminationPrivilige GetUserLabOrganizationExaminationPrivilige(string label)
        {
            return (UserLabOrganizationExaminationPrivilige)Resolve(label, "UserLabOrganizationExamination");
        }
        public static UserLabOrganizationExaminationPrivilige GetUserLabOrganizationExaminationPrivilige(Guid userId)
        {
            return (UserLabOrganizationExaminationPrivilige)Resolve(userId, "UserLabOrganizationExamination");
        }
        public static UserLabOrganizationExaminationPrivilige GetUserLabOrganizationExaminationPrivilige(string label, Guid userExaminationId)
        {
            return (UserLabOrganizationExaminationPrivilige)Resolve(label, userExaminationId, "UserLabOrganizationExamination");
        }
        public static UserLabOrganizationExaminationPrivilige GetUserLabOrganizationExaminationPrivilige(Guid userId, Guid userExaminationId)
        {
            return (UserLabOrganizationExaminationPrivilige)Resolve(userId, userExaminationId, "UserLabOrganizationExamination");
        }
        public static UserPrivilige GetUserPrivilige(string label)
        {
            return (UserPrivilige)Resolve(label, "User");
        }
        public static UserPrivilige GetUserPrivilige(string label, Guid targetUserId)
        {
            return (UserPrivilige)Resolve(label, targetUserId, "User");
        }
        public static UserPrivilige GetUserPrivilige(Guid userId, Guid targetUserId)
        {
            return (UserPrivilige)Resolve(userId, targetUserId, "User");
        }
        public static UserTagPrivilige GetTagPrivilige(string label)
        {
            return (UserTagPrivilige)Resolve(label, "UserTag");
        }
        public static UserTypePrivilige GetUserTypePrivilige(string label)
        {
            return (UserTypePrivilige)Resolve(label, "UserType");
        }
        public static UserAuthorizationPrivilige GetUserAuthorizationPrivilige(string label)
        {
            return (UserAuthorizationPrivilige)Resolve(label, "UserAuthorization");
        }
        public static WorkGroupPrivilige GetWorkGroupPrivilige(string label)
        {
            return (WorkGroupPrivilige)Resolve(label, "WorkGroup");
        }
        public static UsedConfirmPrivilige GetUsedConfirmPrivilige(string label)
        {
            return (UsedConfirmPrivilige)Resolve(label, "UsedConfirm");
        }
        public static UsedConfirmPrivilige GetUsedConfirmPrivilige(string label, Guid usedConfirmId)
        {
            return (UsedConfirmPrivilige)Resolve(label, usedConfirmId, "UsedConfirm");
        }
        public static UsedConfirmPrivilige GetUsedConfirmPrivilige(Guid userId, Guid usedConfirmId)
        {
            return (UsedConfirmPrivilige)Resolve(userId, usedConfirmId, "UsedConfirm");
        }
        public static BalanceAccountPrivilige GetBalanceAccountPrivilige(string label)
        {
            return (BalanceAccountPrivilige)Resolve(label, "BalanceAccount");
        }
        public static BalanceAccountPrivilige GetBalanceAccountPrivilige(Guid userId)
        {
            return (BalanceAccountPrivilige)Resolve(userId, "BalanceAccount");
        }
        public static SampleStatusPrivilige GetSampleStatusPrivilige(string label)
        {
            return (SampleStatusPrivilige)Resolve(label, "SampleStatus");
        }
        public static SampleStatusPrivilige GetSampleStatusPrivilige(Guid userId)
        {
            return (SampleStatusPrivilige)Resolve(userId, "SampleStatus");
        }
        public static SampleItemPrivilige GetSampleItemPrivilige(string label)
        {
            return (SampleItemPrivilige)Resolve(label, "SampleItem");
        }
        public static SampleItemPrivilige GetSampleItemPrivilige(Guid userId)
        {
            return (SampleItemPrivilige)Resolve(userId, "SampleItem");
        }
        public static SampleItemPrivilige GetSampleItemPrivilige(string label, Guid equipmentId)
        {
            return (SampleItemPrivilige)Resolve(label, equipmentId, "SampleItem");
        }
        public static SampleItemPrivilige GetSampleItemPrivilige(Guid userId, Guid equipmentId)
        {
            return (SampleItemPrivilige)Resolve(userId, equipmentId, "SampleItem");
        }
        public static TagSampleItemDiscountPrivilige GetTagSampleItemDiscountPrivilige(Guid userId)
        {
            return (TagSampleItemDiscountPrivilige)Resolve(userId, "TagSampleItemDiscount");
        }
        public static TagSampleItemDiscountPrivilige GetTagSampleItemDiscountPrivilige(string label)
        {
            return (TagSampleItemDiscountPrivilige)Resolve(label, "TagSampleItemDiscount");
        }
        public static UserSampleItemDiscountPrivilige GetUserSampleItemDiscountPrivilige(Guid userId)
        {
            return (UserSampleItemDiscountPrivilige)Resolve(userId, "UserSampleItemDiscount");
        }
        public static UserSampleItemDiscountPrivilige GetUserSampleItemDiscountPrivilige(string label)
        {
            return (UserSampleItemDiscountPrivilige)Resolve(label, "UserSampleItemDiscount");
        }
        public static SampleChargeItemPrivilige GetSampleChargeItemPrivilige(Guid userId)
        {
            return (SampleChargeItemPrivilige)Resolve(userId, "SampleChargeItem");
        }
        public static SampleChargeItemPrivilige GetSampleChargeItemPrivilige(string label)
        {
            return (SampleChargeItemPrivilige)Resolve(label, "SampleChargeItem");
        }
        public static UserEquipmentPriviligePrivilige GetUserEquipmentPriviligePrivilige(Guid userId)
        {
            return (UserEquipmentPriviligePrivilige)Resolve(userId, "UserEquipmentPrivilige");
        }
        public static UserEquipmentPriviligePrivilige GetUserEquipmentPriviligePrivilige(string label)
        {
            return (UserEquipmentPriviligePrivilige)Resolve(label, "UserEquipmentPrivilige");
        }
        public static PowerOperationPrivilige GetPowerOperationPrivilige(string label)
        {
            return (PowerOperationPrivilige)Resolve(label, "PowerOperation");
        }
        public static CameraPrivilige GetCameraPrivilige(string label)
        {
            return (CameraPrivilige)Resolve(label, "Camera");
        }
        public static SystemPrivilige GetSystemPrivilige(string label)
        {
            return (SystemPrivilige)Resolve(label, "System");
        }
        public static NetDiskPrivilige GetNetDiskPrivilige(string label)
        {
            return (NetDiskPrivilige)Resolve(label, "NetDisk");
        }
        public static EquipmentApplyPrivilige GetEquipmentApplyPrivilige(Guid userId)
        {
            return (EquipmentApplyPrivilige)Resolve(userId, "EquipmentApply");
        }
        public static EquipmentApplyPrivilige GetEquipmentApplyPrivilige(string label)
        {
            return (EquipmentApplyPrivilige)Resolve(label, "EquipmentApply");
        }
        public static SampleParameterPrivilige GetSampleParameterPrivilige(Guid userId)
        {
            return (SampleParameterPrivilige)Resolve(userId, "SampleParameter");
        }
        public static SampleParameterPrivilige GetSampleParameterPrivilige(string label)
        {
            return (SampleParameterPrivilige)Resolve(label, "SampleParameter");
        }
        public static SMSSendPrivilige GetSMSSendPrivilige(string label)
        {
            return (SMSSendPrivilige)Resolve(label, "SMSSend");
        }
        public static EquipmentRepairFundsApplyPrivilige GetEquipmentRepairFundsApplyPrivilige(Guid userId)
        {
            return (EquipmentRepairFundsApplyPrivilige)Resolve(userId, "EquipmentRepairFundsApply");
        }
        public static EquipmentRepairFundsApplyPrivilige GetEquipmentRepairFundsApplyPrivilige(string label)
        {
            return (EquipmentRepairFundsApplyPrivilige)Resolve(label, "EquipmentRepairFundsApply");
        }
        public static SubjectProjectImcomePrivilige GetSubjectProjectImcomePrivilige(string label)
        {
            return (SubjectProjectImcomePrivilige)Resolve(label, "SubjectProjectImcome");
        }
        public static DataCachePrivilige GetDataCachePrivilige(Guid userId)
        {
            return (DataCachePrivilige)Resolve(userId, "DataCache");
        }
        public static DataCachePrivilige GetDataCachePrivilige(string label)
        {
            return (DataCachePrivilige)Resolve(label, "DataCache");
        }
        public static DefaultTagEquipmentFunPrivilige GetDefaultTagEquipmentFunPrivilige(Guid userId)
        {
            return (DefaultTagEquipmentFunPrivilige)Resolve(userId, "DefaultTagEquipmentFun");
        }
        public static DefaultTagEquipmentFunPrivilige GetDefaultTagEquipmentFunPrivilige(string label)
        {
            return (DefaultTagEquipmentFunPrivilige)Resolve(label, "DefaultTagEquipmentFun");
        }
        public static SupplierPrivilige GetSupplierPrivilige(string label)
        {
            return (SupplierPrivilige)Resolve(label, "Supplier");
        }
        public static MaterialCategoryPrivilige GetMaterialCategoryPrivilige(string label)
        {
            return (MaterialCategoryPrivilige)Resolve(label, "MaterialCategory");
        }
        public static MaterialInfoPrivilige GetMaterialInfoPrivilige(string label)
        {
            return (MaterialInfoPrivilige)Resolve(label, "MaterialInfo");
        }
        public static MaterialInfoPrivilige GetMaterialInfoPrivilige(string label, Guid materialInfoId)
        {
            return (MaterialInfoPrivilige)Resolve(label, materialInfoId, "MaterialInfo");
        }
        public static MaterialInfoPrivilige GetMaterialInfoPrivilige(Guid userId, Guid materialInfoId)
        {
            return (MaterialInfoPrivilige)Resolve(userId, materialInfoId, "MaterialInfo");
        }
        public static MaterialPurchasePrivilige GetMaterialPurchasePrivilige(string label)
        {
            return (MaterialPurchasePrivilige)Resolve(label, "MaterialPurchase");
        }
        public static MaterialPurchasePrivilige GetMaterialPurchasePrivilige(string label, Guid materialPurchaseId)
        {
            return (MaterialPurchasePrivilige)Resolve(label, materialPurchaseId, "MaterialPurchase");
        }
        public static MaterialPurchasePrivilige GetMaterialPurchasePrivilige(Guid userId, Guid materialPurchaseId)
        {
            return (MaterialPurchasePrivilige)Resolve(userId, materialPurchaseId, "MaterialPurchase");
        }
        public static MaterialInputPrivilige GetMaterialInputPrivilige(string label)
        {
            return (MaterialInputPrivilige)Resolve(label, "MaterialInput");
        }
        public static MaterialInputPrivilige GetMaterialInputPrivilige(string label, Guid materialInputId)
        {
            return (MaterialInputPrivilige)Resolve(label, materialInputId, "MaterialInput");
        }
        public static MaterialInputPrivilige GetMaterialInputPrivilige(Guid userId, Guid materialInputId)
        {
            return (MaterialInputPrivilige)Resolve(userId, materialInputId, "MaterialInput");
        }
        public static MaterialRecipientPrivilige GetMaterialRecipientPrivilige(string label)
        {
            return (MaterialRecipientPrivilige)Resolve(label, "MaterialRecipient");
        }
        public static MaterialRecipientPrivilige GetMaterialRecipientPrivilige(string label, Guid materialRecipientId)
        {
            return (MaterialRecipientPrivilige)Resolve(label, materialRecipientId, "MaterialRecipient");
        }
        public static MaterialRecipientPrivilige GetMaterialRecipientPrivilige(Guid userId, Guid materialRecipientId)
        {
            return (MaterialRecipientPrivilige)Resolve(userId, materialRecipientId, "MaterialRecipient");
        }
        public static MaterialOutputPrivilige GetMaterialOutputPrivilige(string label)
        {
            return (MaterialOutputPrivilige)Resolve(label, "MaterialOutput");
        }
        public static MaterialOutputPrivilige GetMaterialOutputPrivilige(string label, Guid materialOutputId)
        {
            return (MaterialOutputPrivilige)Resolve(label, materialOutputId, "MaterialOutput");
        }
        public static MaterialOutputPrivilige GetMaterialOutputPrivilige(Guid userId, Guid materialOutputId)
        {
            return (MaterialOutputPrivilige)Resolve(userId, materialOutputId, "MaterialOutput");
        }
        public static MaterialBrokenPrivilige GetMaterialBrokenPrivilige(string label)
        {
            return (MaterialBrokenPrivilige)Resolve(label, "MaterialBroken");
        }
        public static MaterialBrokenPrivilige GetMaterialBrokenPrivilige(string label, Guid materialBrokenId)
        {
            return (MaterialBrokenPrivilige)Resolve(label, materialBrokenId, "MaterialBroken");
        }
        public static MaterialBrokenPrivilige GetMaterialBrokenPrivilige(Guid userId, Guid materialBrokenId)
        {
            return (MaterialBrokenPrivilige)Resolve(userId, materialBrokenId, "MaterialBroken");
        }
        public static EquipmentApplyStatisticsPrivilige GetEquipmentApplyStatisticsPrivilige(Guid userId)
        {
            return (EquipmentApplyStatisticsPrivilige)Resolve(userId, "EquipmentApplyStatistics");
        }
        public static EquipmentApplyStatisticsPrivilige GetEquipmentApplyStatisticsPrivilige(string label)
        {
            return (EquipmentApplyStatisticsPrivilige)Resolve(label, "EquipmentApplyStatistics");
        }
        public static MaterialUserAccountPrivilige GetMaterialUserAccountPrivilige(string label)
        {
            return (MaterialUserAccountPrivilige)Resolve(label, "MaterialUserAccount");
        }
        public static MaterialUserAccountPrivilige GetMaterialUserAccountPrivilige(Guid userId)
        {
            return (MaterialUserAccountPrivilige)Resolve(userId, "MaterialUserAccount");
        }
        public static AnimalCategoryPrivilige GetAnimalCategoryPrivilige(Guid userId)
        {
            return (AnimalCategoryPrivilige)Resolve(userId, "AnimalCategory");
        }
        public static AnimalCategoryPrivilige GetAnimalCategoryPrivilige(string label)
        {
            return (AnimalCategoryPrivilige)Resolve(label, "AnimalCategory");
        }
        public static AnimalPrivilige GetAnimalPrivilige(Guid userId)
        {
            return (AnimalPrivilige)Resolve(userId, "Animal");
        }
        public static AnimalPrivilige GetAnimalPrivilige(string label)
        {
            return (AnimalPrivilige)Resolve(label, "Animal");
        }
        public static EthicsSettingPrivilige GetEthicsSettingPrivilige(Guid userId)
        {
            return (EthicsSettingPrivilige)Resolve(userId, "EthicsSetting");
        }
        public static EthicsSettingPrivilige GetEthicsSettingPrivilige(string label)
        {
            return (EthicsSettingPrivilige)Resolve(label, "EthicsSetting");
        }
        public static EthicsApplyPrivilige GetEthicsApplyPrivilige(Guid userId)
        {
            return (EthicsApplyPrivilige)Resolve(userId, "EthicsApply");
        }
        public static EthicsApplyPrivilige GetEthicsApplyPrivilige(string label)
        {
            return (EthicsApplyPrivilige)Resolve(label, "EthicsApply");
        }
        public static AnimalFramePrivilige GetAnimalFramePrivilige(Guid userId)
        {
            return (AnimalFramePrivilige)Resolve(userId, "AnimalFrame");
        }
        public static AnimalFramePrivilige GetAnimalFramePrivilige(string label)
        {
            return (AnimalFramePrivilige)Resolve(label, "AnimalFrame");
        }
        public static AnimalCagePrivilige GetAnimalCagePrivilige(Guid userId)
        {
            return (AnimalCagePrivilige)Resolve(userId, "AnimalCage");
        }
        public static AnimalCagePrivilige GetAnimalCagePrivilige(string label)
        {
            return (AnimalCagePrivilige)Resolve(label, "AnimalCage");
        }
        public static AnimalAppointmentPrivilige GetAnimalAppointmentPrivilige(Guid userId)
        {
            return (AnimalAppointmentPrivilige)Resolve(userId, "AnimalAppointment");
        }
        public static AnimalAppointmentPrivilige GetAnimalAppointmentPrivilige(string label)
        {
            return (AnimalAppointmentPrivilige)Resolve(label, "AnimalAppointment");
        }
        public static AnimalOutAppointmentPrivilige GetAnimalOutAppointmentPrivilige(string label)
        {
            return (AnimalOutAppointmentPrivilige)Resolve(label, "AnimalOutAppointment");
        }
        public static AnimalOutAppointmentPrivilige GetAnimalOutAppointmentPrivilige(Guid userId)
        {
            return (AnimalOutAppointmentPrivilige)Resolve(userId, "AnimalOutAppointment");
        }
        public static OpenFundApplyPrivilige GetOpenFundApplyPrivilige(string label)
        {
            return (OpenFundApplyPrivilige)Resolve(label, "OpenFundApply");
        }
        public static OpenFundApplyPrivilige GetOpenFundApplyPrivilige(Guid userId)
        {
            return (OpenFundApplyPrivilige)Resolve(userId, "OpenFundApply");
        }
        public static JudgeProjectPrivilige GetJudgeProjectPrivilige(string label)
        {
            return (JudgeProjectPrivilige)Resolve(label, "JudgeProject");
        }
        public static JudgeProjectPrivilige GetJudgeProjectPrivilige(Guid userId)
        {
            return (JudgeProjectPrivilige)Resolve(userId, "JudgeProject");
        }
        public static JudgeEquipmentRecordPrivilige GetJudgeEquipmentRecordPrivilige(string label)
        {
            return (JudgeEquipmentRecordPrivilige)Resolve(label, "JudgeEquipmentRecord");
        }
        public static JudgeEquipmentRecordPrivilige GetJudgeEquipmentRecordPrivilige(string label, Guid judgeEquipmentRecordId)
        {
            return (JudgeEquipmentRecordPrivilige)Resolve(label, judgeEquipmentRecordId, "JudgeEquipmentRecord");
        }
        public static JudgeEquipmentRecordPrivilige GetJudgeEquipmentRecordPrivilige(Guid userId, Guid judgeEquipmentRecordId)
        {
            return (JudgeEquipmentRecordPrivilige)Resolve(userId, judgeEquipmentRecordId, "JudgeEquipmentRecord");
        }
        public static SampleStatisticsPrivilige GetSampleStatisticsPrivilige(string label)
        {
            return (SampleStatisticsPrivilige)Resolve(label, "SampleStatistics");
        }
        public static SampleStatisticsPrivilige GetSampleStatisticsPrivilige(Guid userId)
        {
            return (SampleStatisticsPrivilige)Resolve(userId, "SampleStatistics");
        }
        public static EquipmentSampleFeedBackSettingPrivilige GetEquipmentSampleFeedBackSettingPrivilige(string label)
        {
            return (EquipmentSampleFeedBackSettingPrivilige)Resolve(label, "EquipmentSampleFeedBackSetting");
        }
        public static EquipmentSampleFeedBackSettingPrivilige GetEquipmentSampleFeedBackSettingPrivilige(Guid userId)
        {
            return (EquipmentSampleFeedBackSettingPrivilige)Resolve(userId, "EquipmentSampleFeedBackSetting");
        }
        public static StatisticsPrivilige GetStatisticsPrivilige(string label)
        {
            return (StatisticsPrivilige)Resolve(label, "Statistics");
        }
        public static StatisticsPrivilige GetStatisticsPrivilige(Guid userId)
        {
            return (StatisticsPrivilige)Resolve(userId, "Statistics");
        }
        public static SampleItemLabelPrivilige GetSampleItemLabelPrivilige(string label)
        {
            return (SampleItemLabelPrivilige)Resolve(label, "SampleItemLabel");
        }
        public static SampleItemLabelPrivilige GetSampleItemLabelPrivilige(Guid userId)
        {
            return (SampleItemLabelPrivilige)Resolve(userId, "SampleItemLabel");
        }
        public static EquipmentRelationPrivilige GetEquipmentRelationPrivilige(string label)
        {
            return (EquipmentRelationPrivilige)Resolve(label, "EquipmentRelation");
        }
        public static EquipmentGroupPrivilige GetEquipmentGroupPrivilige(string label)
        {
            return (EquipmentGroupPrivilige)Resolve(label, "EquipmentGroup");
        }
        public static EquipmentGroupPrivilige GetEquipmentGroupPrivilige(Guid userId)
        {
            return (EquipmentGroupPrivilige)Resolve(userId, "EquipmentGroup");
        }
        public static EquipmentGroupRegPrivilige GetEquipmentGroupRegPrivilige(string label)
        {
            return (EquipmentGroupRegPrivilige)Resolve(label, "EquipmentGroupReg");
        }
        public static EquipmentGroupRegPrivilige GetEquipmentGroupRegPrivilige(Guid userId)
        {
            return (EquipmentGroupRegPrivilige)Resolve(userId, "EquipmentGroupReg");
        }
        public static SemesterPrivilige GetSemesterPrivilige(string label)
        {
            return (SemesterPrivilige)Resolve(label, "Semester");
        }
        public static SemesterPrivilige GetSemesterPrivilige(Guid userId)
        {
            return (SemesterPrivilige)Resolve(userId, "Semester");
        }
        public static EquipmentSemesterCostPrivilige GetEquipmentSemesterCostPrivilige(string label)
        {
            return (EquipmentSemesterCostPrivilige)Resolve(label, "EquipmentSemesterCost");
        }
        public static EquipmentSemesterCostPrivilige GetEquipmentSemesterCostPrivilige(Guid userId)
        {
            return (EquipmentSemesterCostPrivilige)Resolve(userId, "EquipmentSemesterCost");
        }
        public static ActivityTypePrivilige GetActivityTypePrivilige(string label)
        {
            return (ActivityTypePrivilige)Resolve(label, "ActivityType");
        }
        public static ActivityTypePrivilige GetActivityTypePrivilige(Guid userId)
        {
            return (ActivityTypePrivilige)Resolve(userId, "ActivityType");
        }
        public static ActivityApplyPrivilige GetActivityApplyPrivilige(string label)
        {
            return (ActivityApplyPrivilige)Resolve(label, "ActivityApply");
        }
        public static ActivityApplyPrivilige GetActivityApplyPrivilige(Guid userId)
        {
            return (ActivityApplyPrivilige)Resolve(userId, "SystemLog");
        }
        public static SystemLogPrivilige GetSystemLogPrivilige(string label)
        {
            return (SystemLogPrivilige)Resolve(label, "SystemLog");
        }
        public static SystemLogPrivilige GetSystemLogPrivilige(Guid userId)
        {
            return (SystemLogPrivilige)Resolve(userId, "SystemLog");
        }
        public static EquipmentExtendPrivilige GetEquipmentExtendPrivilige(string label)
        {
            return (EquipmentExtendPrivilige)Resolve(label, "EquipmentExtend");
        }
        public static EquipmentExtendPrivilige GetEquipmentExtendPrivilige(Guid userId)
        {
            return (EquipmentExtendPrivilige)Resolve(userId, "EquipmentExtend");
        }
        public static EquipmentCustomsPrivilige GetEquipmentCustomsPrivilige(string label)
        {
            return (EquipmentCustomsPrivilige)Resolve(label, "EquipmentCustoms");
        }
        public static EquipmentCustomsPrivilige GetEquipmentCustomsPrivilige(Guid userId)
        {
            return (EquipmentCustomsPrivilige)Resolve(userId, "EquipmentCustoms");
        }
        public static TemperatureHumiditySetupPrivilige GetTemperatureHumiditySetupPrivilige(string label)
        {
            return (TemperatureHumiditySetupPrivilige)Resolve(label, "TemperatureHumiditySetup");
        }
        public static MessageReceiveSettingPrivilige GetMessageReceiveSettingPrivilige(string label)
        {
            return (MessageReceiveSettingPrivilige)Resolve(label, "MessageReceiveSetting");
        }
        public static MessageReceiveSettingPrivilige GetMessageReceiveSettingPrivilige(Guid userId)
        {
            return (MessageReceiveSettingPrivilige)Resolve(userId, "MessageReceiveSetting");
        }

        public static EquipmentUndergraduateOpenPrivilige GetEquipmentUndergraduateOpenPrivilige(string label)
        {
            return (EquipmentUndergraduateOpenPrivilige)Resolve(label, "EquipmentUndergraduateOpen");
        }
        public static EquipmentUndergraduateOpenPrivilige GetEquipmentUndergraduateOpenPrivilige(Guid userId)
        {
            return (EquipmentUndergraduateOpenPrivilige)Resolve(userId, "EquipmentUndergraduateOpen");
        }
        public static EquipmentOpenTrainingPrivilige GetEquipmentOpenTrainingPrivilige(string label)
        {
            return (EquipmentOpenTrainingPrivilige)Resolve(label, "EquipmentOpenTraining");
        }
        public static EquipmentOpenTrainingPrivilige GetEquipmentOpenTrainingPrivilige(Guid userId)
        {
            return (EquipmentOpenTrainingPrivilige)Resolve(userId, "EquipmentOpenTraining");
        }
        public static EquipmentOpenTrainingCarryOutPrivilige GetEquipmentOpenTrainingCarryOutPrivilige(string label)
        {
            return (EquipmentOpenTrainingCarryOutPrivilige)Resolve(label, "EquipmentOpenTrainingCarryOut");
        }
        public static EquipmentOpenTrainingCarryOutPrivilige GetEquipmentOpenTrainingCarryOutPrivilige(Guid userId)
        {
            return (EquipmentOpenTrainingCarryOutPrivilige)Resolve(userId, "EquipmentOpenTrainingCarryOut");
        }
        public static EquipmentOpenTrainingCarryOutSignUpPrivilige GetEquipmentOpenTrainingCarryOutSignUpPrivilige(string label)
        {
            return (EquipmentOpenTrainingCarryOutSignUpPrivilige)Resolve(label, "EquipmentOpenTrainingCarryOutSignUp");
        }
        public static EquipmentOpenTrainingCarryOutSignUpPrivilige GetEquipmentOpenTrainingCarryOutSignUpPrivilige(Guid userId)
        {
            return (EquipmentOpenTrainingCarryOutSignUpPrivilige)Resolve(userId, "EquipmentOpenTrainingCarryOutSignUp");
        }
        public static EquipmentOpenPracticePrivilige GetEquipmentOpenPracticePrivilige(string label)
        {
            return (EquipmentOpenPracticePrivilige)Resolve(label, "EquipmentOpenPractice");
        }
        public static EquipmentOpenPracticePrivilige GetEquipmentOpenPracticePrivilige(Guid userId)
        {
            return (EquipmentOpenPracticePrivilige)Resolve(userId, "EquipmentOpenPractice");
        }
        public static WindowsPrivilige GetWindowsPrivilige(string label)
        {
            return (WindowsPrivilige)Resolve(label, "Windows");
        }
        public static BuildingPrivilige GetBuildingPrivilige(string label)
        {
            return (BuildingPrivilige)Resolve(label, "Building");
        }
        public static WaterControlPrivilige GetWaterControlPrivilige(string label)
        {
            return (WaterControlPrivilige)Resolve(label, "WaterControl");
        }
        public static NMPPrivilige GetNMPPrivilige(string label)
        {
            return (NMPPrivilige)Resolve(label, "NMP");
        }
        public static NMPPrivilige GetNMPPrivilige(Guid userId)
        {
            return (NMPPrivilige)Resolve(userId, "NMP");
        }
        public static NMPPrivilige GetNMPPrivilige(Guid userId, Guid nmpId)
        {
            return (NMPPrivilige)Resolve(userId, nmpId, "NMP");
        }
        public static NMPPrivilige GetNMPPrivilige(string label, Guid nmpId)
        {
            return (NMPPrivilige)Resolve(label, nmpId, "NMP");
        }
        public static RepairPrivilige GetRepairPrivilige(string label)
        {
            return (RepairPrivilige)Resolve(label, "Repair");
        }
        public static SJ3StatisticsUsedHourPrivilige GetSJ3StatisticsUsedHourPrivilige(string label)
        {
            return (SJ3StatisticsUsedHourPrivilige)Resolve(label, "SJ3StatisticsUsedHour");
        }
        public static ShareFundApplyPrivilige GetShareFundApplyPrivilige(string label)
        {
            return (ShareFundApplyPrivilige)Resolve(label, "ShareFundApply");
        }
        public static ShareFundApplyPrivilige GetShareFundApplyPrivilige(Guid userId)
        {
            return (ShareFundApplyPrivilige)Resolve(userId, "ShareFundApply");
        }
        public static EquipmentImportPrivilige GetEquipmentImportPrivilige(string label)
        {
            return (EquipmentImportPrivilige)Resolve(label, "EquipmentImport");
        }
        public static EquipmentImportPrivilige GetEquipmentImportPrivilige(Guid userId)
        {
            return (EquipmentImportPrivilige)Resolve(userId, "EquipmentImport");
        }
        public static NMPAppointmentPrivilige GetNMPAppointmentPrivilige(string label)
        {
            return (NMPAppointmentPrivilige)Resolve(label, "NMPAppointment");
        }
        public static NMPAppointmentPrivilige GetNMPAppointmentPrivilige(string label, ViewNMPAppointment viewNMPAppointment)
        {
            return (NMPAppointmentPrivilige)Resolve(new object[] { label, viewNMPAppointment }, "NMPAppointment");
        }
        public static NMPAppointmentPrivilige GetNMPAppointmentPrivilige(Guid userId, ViewNMPAppointment viewNMPAppointment)
        {
            return (NMPAppointmentPrivilige)Resolve(new object[] { userId, viewNMPAppointment }, "NMPAppointment");
        }
        public static NMPUsedConfirmPrivilige GetNMPUsedConfirmPrivilige(string label)
        {
            return (NMPUsedConfirmPrivilige)Resolve(label, "NMPUsedConfirm");
        }
        public static NMPUsedConfirmPrivilige GetNMPUsedConfirmPrivilige(string label, Guid nmpUsedConfirmId)
        {
            return (NMPUsedConfirmPrivilige)Resolve(label, nmpUsedConfirmId, "NMPUsedConfirm");
        }
        public static NMPUsedConfirmPrivilige GetNMPUsedConfirmPrivilige(Guid userId, Guid nmpUsedConfirmId)
        {
            return (NMPUsedConfirmPrivilige)Resolve(userId, nmpUsedConfirmId, "NMPUsedConfirm");
        }
        /// <summary> 效益评价表权限 </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static EvaluationTablePrivilige GetEvaluationTablePrivilige(Guid userId)
        {
            var privilege = new EvaluationTablePrivilige(userId);
            return (EvaluationTablePrivilige)privilege.GetPrivilige();
            //return (EvaluationTablePrivilige)Resolve(userId, "EvaluationTablePrivilige");
        }
        /// <summary> 效益评价表权限 </summary>
        /// <param name="label">用户标签</param>
        /// <returns></returns>
        public static EvaluationTablePrivilige GetEvaluationTablePrivilige(string label)
        {
            var user = __userBLL.GetUserByLabel(label);
            if (user == null) return null;
            var privilege = new EvaluationTablePrivilige(user.Id);
            return (EvaluationTablePrivilige)privilege.GetPrivilige();
            //return (EvaluationTablePrivilige)Resolve(new Guid(userId), "EvaluationTablePrivilige");
        }

    }
}
