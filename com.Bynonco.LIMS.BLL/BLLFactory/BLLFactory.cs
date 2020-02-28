using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.BLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class BLLFactory
    {
        private static readonly string __nameSpace = "com.Bynonco.LIMS.BLL";
        public static IBLLBase<T> Resolve<T>() where T : com.august.DataLink.DataObject<Guid>
        {
            return (IBLLBase<T>)Activator.CreateInstance(__nameSpace, typeof(T).Name);
        }

        #region 前台菜单
        private static IHomeMenuBLL __homeMenuBLL;
        public static IHomeMenuBLL CreateHomeMenuBLL()
        {
            if (__homeMenuBLL == null)
                __homeMenuBLL = new HomeMenuBLL();
            return __homeMenuBLL;
        }
        private static IViewHomeMenuBLL __viewHomeMenuBLL;
        public static IViewHomeMenuBLL CreateViewHomeMenuBLL()
        {
            if (__viewHomeMenuBLL == null)
                __viewHomeMenuBLL = new ViewHomeMenuBLL();
            return __viewHomeMenuBLL;
        }

        public static void SetBLLManage(object p)
        {
           
        }
        #endregion

        #region 功能操作管理
        private static IModuleFunctionBLL __moduleFunctionBLL;
        public static IModuleFunctionBLL CreateModuleFunctionBLL()
        {
            if (__moduleFunctionBLL == null)
                __moduleFunctionBLL = new ModuleFunctionBLL();
            return __moduleFunctionBLL;
        }
        private static IViewModuleFunctionBLL __viewModuleFunctionBLL;
        public static IViewModuleFunctionBLL CreateViewModuleFunctionBLL()
        {
            if (__viewModuleFunctionBLL == null)
                __viewModuleFunctionBLL = new ViewModuleFunctionBLL();
            return __viewModuleFunctionBLL;
        }
        #endregion

        #region 辅助字典
        private static IDictCodeTypeBLL __dictCodeTypeBLL;
        public static IDictCodeTypeBLL CreateDictCodeTypeBLL()
        {
            if (__dictCodeTypeBLL == null)
                __dictCodeTypeBLL = new DictCodeTypeBLL();
            return __dictCodeTypeBLL;
        }
        private static IDictCodeBLL __dictCodeBLL;
        public static IDictCodeBLL CreateDictCodeBLL()
        {
            if (__dictCodeBLL == null)
                __dictCodeBLL = new DictCodeBLL();
            return __dictCodeBLL;
        }
        #endregion

        #region 组织机构
        private static ILabOrganizationBLL __labOrganizationBLL;
        public static ILabOrganizationBLL CreateLabOrganizationBLL()
        {
            if (__labOrganizationBLL == null)
                __labOrganizationBLL = new LabOrganizationBLL();
            return __labOrganizationBLL;
        }
        private static ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        public static ILabOrganizationAuthorizedBLL CreateLabOrganizationAuthorizedBLL()
        {
            if (__labOrganizationAuthorizedBLL == null)
                __labOrganizationAuthorizedBLL = new LabOrganizationAuthorizedBLL();
            return __labOrganizationAuthorizedBLL;
        }
        private static IViewLabOrganizationAuthorizedBLL __viewLabOrganizationAuthorizedBLL;
        public static IViewLabOrganizationAuthorizedBLL CreateViewLabOrganizationAuthorizedBLL()
        {
            if (__viewLabOrganizationAuthorizedBLL == null)
                __viewLabOrganizationAuthorizedBLL = new ViewLabOrganizationAuthorizedBLL();
            return __viewLabOrganizationAuthorizedBLL;
        }
        public static ILabOrganizationAdminBLL __labOrganizationAdminBLL;
        public static ILabOrganizationAdminBLL CreateLabOrganizationAdminBLL()
        {
            if (__labOrganizationAdminBLL == null)
                __labOrganizationAdminBLL = new LabOrganizationAdminBLL();
            return __labOrganizationAdminBLL;
        }
        private static IViewLabOrganizationAdminBLL __viewLabOrganizationAdminBLL;
        public static IViewLabOrganizationAdminBLL CreateViewLabOrganizationAdminBLL()
        {
            if (__viewLabOrganizationAdminBLL == null)
                __viewLabOrganizationAdminBLL = new ViewLabOrganizationAdminBLL();
            return __viewLabOrganizationAdminBLL;
        }
        private static ILabOrganizationRelationBLL __labOrganizationRelationBLL;
        public static ILabOrganizationRelationBLL CreateLabOrganizationRelationBLL()
        {
            if (__labOrganizationRelationBLL == null)
                __labOrganizationRelationBLL = new LabOrganizationRelationBLL();
            return __labOrganizationRelationBLL;
        }
        private static ILabOrganizationDirectorBLL __labOrganizationDirectorBLL;
        public static ILabOrganizationDirectorBLL CreateLabOrganizationDirectorBLL()
        {
            if (__labOrganizationDirectorBLL == null)
                __labOrganizationDirectorBLL = new LabOrganizationDirectorBLL();
            return __labOrganizationDirectorBLL;
        }
        private static ILabOrganizationUpdatedBLL __labOrganizationUpdatedBLL;
        public static ILabOrganizationUpdatedBLL CreateLabOrganizationUpdatedBLL()
        {
            if (__labOrganizationUpdatedBLL == null)
                __labOrganizationUpdatedBLL = new LabOrganizationUpdatedBLL();
            return __labOrganizationUpdatedBLL;
        }
        private static ILabOrganizationAdminDeleteBLL __labOrganizationAdminDeleteBLL;
        public static ILabOrganizationAdminDeleteBLL CreateLabOrganizationAdminDeleteBLL()
        {
            if (__labOrganizationAdminDeleteBLL == null)
                __labOrganizationAdminDeleteBLL = new LabOrganizationAdminDeleteBLL();
            return __labOrganizationAdminDeleteBLL;
        }
        #endregion

        #region 用户管理
        private static IUserBLL __userBLL;
        public static IUserBLL CreateUserBLL()
        {
            if (__userBLL == null)
                __userBLL = new UserBLL();
            return __userBLL;
        }
        private static IViewUserBLL __viewUserBLL;
        public static IViewUserBLL CreateViewUserBLL()
        {
            if (__viewUserBLL == null)
                __viewUserBLL = new ViewUserBLL();
            return __viewUserBLL;
        }
        private static IUserAccountBLL __userAccountBLL;
        public static IUserAccountBLL CreateUserAccountBLL()
        {
            if (__userAccountBLL == null)
                __userAccountBLL = new UserAccountBLL();
            return __userAccountBLL;
        }
        private static IUserTypeBLL __userTypeBLL;
        public static IUserTypeBLL CreateUserTypeBLL()
        {
            if (__userTypeBLL == null)
                __userTypeBLL = new UserTypeBLL();
            return __userTypeBLL;
        }
        private static ITagBLL __tagBLL;
        public static ITagBLL CreateTagBLL()
        {
            if (__tagBLL == null)
                __tagBLL = new TagBLL();
            return __tagBLL;
        }
        private static ICreditLimitBLL __creditLimitBLL;
        public static ICreditLimitBLL CreateCreditLimitBLL()
        {
            if (__creditLimitBLL == null)
                __creditLimitBLL = new CreditLimitBLL();
            return __creditLimitBLL;
        }
        private static IUserSystemSettingBLL __userSystemSettingBLL;
        public static IUserSystemSettingBLL CreateUserSystemSettingBLL()
        {
            if (__userSystemSettingBLL == null)
                __userSystemSettingBLL = new UserSystemSettingBLL();
            return __userSystemSettingBLL;
        }
        private static IViewUserSystemSettingBLL __viewUserSystemSettingBLL;
        public static IViewUserSystemSettingBLL CreateViewUserSystemSettingBLL()
        {
            if (__viewUserSystemSettingBLL == null)
                __viewUserSystemSettingBLL = new ViewUserSystemSettingBLL();
            return __viewUserSystemSettingBLL;
        }
        private static IWorkGroupBLL __workGroupBLL;
        public static IWorkGroupBLL CreateWorkGroupBLL()
        {
            if (__workGroupBLL == null)
                __workGroupBLL = new WorkGroupBLL();
            return __workGroupBLL;
        }
        private static IViewUserWorkGroupBLL __viewUsverWorkGroupBLL;
        public static IViewUserWorkGroupBLL CreateViewUserWorkGroupBLL()
        {
            if (__viewUsverWorkGroupBLL == null)
                __viewUsverWorkGroupBLL = new ViewUserWorkGroupBLL();
            return __viewUsverWorkGroupBLL;
        }
        private static IUserWorkGroupBLL __userWorkGroupBLL;
        public static IUserWorkGroupBLL CreateUserWorkGroupBLL()
        {
            if (__userWorkGroupBLL == null)
                __userWorkGroupBLL = new UserWorkGroupBLL();
            return __userWorkGroupBLL;
        }
        private static IWorkGroupModuleBLL __workGroupModuleBLL;
        public static IWorkGroupModuleBLL CreateWorkGroupModuleBLL()
        {
            if (__workGroupModuleBLL == null)
                __workGroupModuleBLL = new WorkGroupModuleBLL();
            return __workGroupModuleBLL;
        }
        private static IViewWorkGroupModuleBLL __viewWorkGroupModuleBLL;
        public static IViewWorkGroupModuleBLL CreateViewWorkGroupModuleBLL()
        {
            if (__viewWorkGroupModuleBLL == null)
                __viewWorkGroupModuleBLL = new ViewWorkGroupModuleBLL();
            return __viewWorkGroupModuleBLL;
        }
        private static IViewUserWorkGroupModuleFunctionBLL __viewUserWorkGroupModuleFunctionBLL;
        public static IViewUserWorkGroupModuleFunctionBLL CreateViewUserWorkGroupModuleFunctionBLL()
        {
            if (__viewUserWorkGroupModuleFunctionBLL == null)
                __viewUserWorkGroupModuleFunctionBLL = new ViewUserWorkGroupModuleFunctionBLL();
            return __viewUserWorkGroupModuleFunctionBLL;
        }
        private static IFindPasswordBLL __findPasswordBLL;
        public static IFindPasswordBLL CreateFindPasswordBLL()
        {
            if (__findPasswordBLL == null)
                __findPasswordBLL = new FindPasswordBLL();
            return __findPasswordBLL;
        }
        private static IUserUpdatedBLL __userUpdatedBLL;
        public static IUserUpdatedBLL CreateUserUpdatedBLL()
        {
            if (__userUpdatedBLL == null)
                __userUpdatedBLL = new UserUpdatedBLL();
            return __userUpdatedBLL;
        }
        private static IViewWorkGroupUserBLL __viewWorkGroupUserBLL;
        public static IViewWorkGroupUserBLL CreateViewWorkGroupUserBLL()
        {
            if (__viewWorkGroupUserBLL == null)
                __viewWorkGroupUserBLL = new ViewWorkGroupUserBLL();
            return __viewWorkGroupUserBLL;
        }
        #endregion

        #region 存款管理
        private static IDepositRecordBLL __depositRecordBLL;
        public static IDepositRecordBLL CreateDepositRecordBLL()
        {
            if (__depositRecordBLL == null)
                __depositRecordBLL = new DepositRecordBLL();
            return __depositRecordBLL;
        }
        private static IViewDepositRecordBLL __viewDepositRecordBLL;
        public static IViewDepositRecordBLL CreateViewDepositRecordBLL()
        {
            if (__viewDepositRecordBLL == null)
                __viewDepositRecordBLL = new ViewDepositRecordBLL();
            return __viewDepositRecordBLL;
        }
        private static IDepositRecordEquipmentBLL __depositRecordEquipmentBLL;
        public static IDepositRecordEquipmentBLL CreateDepositRecordEquipmentBLL()
        {
            if (__depositRecordEquipmentBLL == null)
                __depositRecordEquipmentBLL = new DepositRecordEquipmentBLL();
            return __depositRecordEquipmentBLL;
        }
        #endregion

        #region 站内消息
        private static IMessageBLL __messageBLL;
        public static IMessageBLL CreateMessageBLL()
        {
            if (__messageBLL == null)
                __messageBLL = new MessageBLL();
            return __messageBLL;
        }
        private static IReceiverBLL __receiverBLL;
        public static IReceiverBLL CreateReceiverBLL()
        {
            if (__receiverBLL == null)
                __receiverBLL = new ReceiverBLL();
            return __receiverBLL;
        }
        private static IViewMessageReceiveBLL __viewMessageReceiveBLL;
        public static IViewMessageReceiveBLL CreateViewMessageReceiveBLL()
        {
            if (__viewMessageReceiveBLL == null)
                __viewMessageReceiveBLL = new ViewMessageReceiveBLL();
            return __viewMessageReceiveBLL;
        }
        private static ISendMailBLL __sendMailBLL;
        public static ISendMailBLL CreateSendMailBLL()
        {
            if (__sendMailBLL == null)
                __sendMailBLL = new SendMailBLL();
            return __sendMailBLL;
        }
        #endregion

        #region 文章管理
        private static IArticleBLL __articleBLL;
        public static IArticleBLL CreateArticleBLL()
        {
            if (__articleBLL == null)
                __articleBLL = new ArticleBLL();
            return __articleBLL;
        }
        private static IViewArticleBLL __viewArticleBLL;
        public static IViewArticleBLL CreateViewArticleBLL()
        {
            if (__viewArticleBLL == null)
                __viewArticleBLL = new ViewArticleBLL();
            return __viewArticleBLL;
        }
        private static IArticleCategoryBLL __articleCategoryBLL;
        public static IArticleCategoryBLL CreateArticleCategoryBLL()
        {
            if (__articleCategoryBLL == null)
                __articleCategoryBLL = new ArticleCategoryBLL();
            return __articleCategoryBLL;
        }
        private static IViewArticleCategoryBLL __viewArticleCategoryBLL;
        public static IViewArticleCategoryBLL CreateViewArticleCategoryBLL()
        {
            if (__viewArticleCategoryBLL == null)
                __viewArticleCategoryBLL = new ViewArticleCategoryBLL();
            return __viewArticleCategoryBLL;
        }
        private static IArticleRelationBLL __articleRelationBLL;
        public static IArticleRelationBLL CreateArticleRelationBLL()
        {
            if (__articleRelationBLL == null)
                __articleRelationBLL = new ArticleRelationBLL();
            return __articleRelationBLL;
        }
        private static IViewArticleRelationBLL __viewArticleRelationBLL;
        public static IViewArticleRelationBLL CreateViewArticleRelationBLL()
        {
            if (__viewArticleRelationBLL == null)
                __viewArticleRelationBLL = new ViewArticleRelationBLL();
            return __viewArticleRelationBLL;
        }
        #endregion

        #region 交流天地
        private static IExchangeFairylandBLL __exchangeFairylandBLL;
        public static IExchangeFairylandBLL CreateExchangeFairylandBLL()
        {
            if (__exchangeFairylandBLL == null)
                __exchangeFairylandBLL = new ExchangeFairylandBLL();
            return __exchangeFairylandBLL;
        }
        private static IExchangeFairylandCommentBLL __exchangeFairylandCommentBLL;
        public static IExchangeFairylandCommentBLL CreateExchangeFairylandCommentBLL()
        {
            if (__exchangeFairylandCommentBLL == null)
                __exchangeFairylandCommentBLL = new ExchangeFairylandCommentBLL();
            return __exchangeFairylandCommentBLL;
        }
        #endregion

        #region 问与答QA
        private static IQuestionBLL __questionBLL;
        public static IQuestionBLL CreateQuestionBLL()
        {
            if (__questionBLL == null)
                __questionBLL = new QuestionBLL();
            return __questionBLL;
        }
        private static IViewQuestionBLL __viewQuestionBLL;
        public static IViewQuestionBLL CreateViewQuestionBLL()
        {
            if (__viewQuestionBLL == null)
                __viewQuestionBLL = new ViewQuestionBLL();
            return __viewQuestionBLL;
        }
        private static IAnswerBLL __answerBLL;
        public static IAnswerBLL CreateAnswerBLL()
        {
            if (__answerBLL == null)
                __answerBLL = new AnswerBLL();
            return __answerBLL;
        }
        #endregion

        #region 课题管理
        private static ISubjectBLL __subjectBLL;
        public static ISubjectBLL CreateSubjectBLL()
        {
            if (__subjectBLL == null)
                __subjectBLL = new SubjectBLL();
            return __subjectBLL;
        }
        private static IViewSubjectBLL __viewSubjectBLL;
        public static IViewSubjectBLL CreateViewSubjectBLL()
        {
            if (__viewSubjectBLL == null)
                __viewSubjectBLL = new ViewSubjectBLL();
            return __viewSubjectBLL;
        }
        private static IExperimenterSubjectBLL __experimenterSubjectBLL;
        public static IExperimenterSubjectBLL CreateExperimenterSubjectBLL()
        {
            if (__experimenterSubjectBLL == null)
                __experimenterSubjectBLL = new ExperimenterSubjectBLL();
            return __experimenterSubjectBLL;
        }
        private static ISubjectProjectBLL __subjectProjectBLL;
        public static ISubjectProjectBLL CreateSubjectProjectBLL()
        {
            if (__subjectProjectBLL == null)
                __subjectProjectBLL = new SubjectProjectBLL();
            return __subjectProjectBLL;
        }
        #endregion

        #region 设备管理
        private static IEquipmentBLL __equipmentBLL;
        public static IEquipmentBLL CreateEquipmentBLL()
        {
            if (__equipmentBLL == null)
                __equipmentBLL = new EquipmentBLL();
            return __equipmentBLL;
        }
        private static IViewEquipmentBLL __viewEquipmentBLL;
        public static IViewEquipmentBLL CreateViewEquipmentBLL()
        {
            if (__viewEquipmentBLL == null)
                __viewEquipmentBLL = new ViewEquipmentBLL();
            return __viewEquipmentBLL;
        }
        private static IEquipmentLabelBLL __equipmentLabelBLL;
        public static IEquipmentLabelBLL CreateEquipmentLabelBLL()
        {
            if (__equipmentLabelBLL == null)
                __equipmentLabelBLL = new EquipmentLabelBLL();
            return __equipmentLabelBLL;
        }
        private static IEquipmentLabelItemBLL __equipmentLabelItemBLL;
        public static IEquipmentLabelItemBLL CreateEquipmentLabelItemBLL()
        {
            if (__equipmentLabelItemBLL == null)
                __equipmentLabelItemBLL = new EquipmentLabelItemBLL();
            return __equipmentLabelItemBLL;
        }
        private static IEquipmentLabelAddtionChargeItemBLL __equipmentLabelAddtionChargeItemBLL;
        public static IEquipmentLabelAddtionChargeItemBLL CreateEquipmentLabelAddtionChargeItemBLL()
        {
            if (__equipmentLabelAddtionChargeItemBLL == null)
                __equipmentLabelAddtionChargeItemBLL = new EquipmentLabelAddtionChargeItemBLL();
            return __equipmentLabelAddtionChargeItemBLL;
        }
        private static IEquipmentLabelChargeStandardBLL __equipmentLabelChargeStandardBLL;
        public static IEquipmentLabelChargeStandardBLL CreateEquipmentLabelChargeStandardBLL()
        {
            if (__equipmentLabelChargeStandardBLL == null)
                __equipmentLabelChargeStandardBLL = new EquipmentLabelChargeStandardBLL();
            return __equipmentLabelChargeStandardBLL;
        }
        private static IEquipmentNoticeBLL __equipmentNoticeBLL;
        public static IEquipmentNoticeBLL CreateEquipmentNoticeBLL()
        {
            if (__equipmentNoticeBLL == null)
                __equipmentNoticeBLL = new EquipmentNoticeBLL();
            return __equipmentNoticeBLL;
        }
        private static IEquipmentNoticeAttachmentBLL __equipmentNoticeAttachmentBLL;
        public static IEquipmentNoticeAttachmentBLL CreateEquipmentNoticeAttachmentBLL()
        {
            if (__equipmentNoticeAttachmentBLL == null)
                __equipmentNoticeAttachmentBLL = new EquipmentNoticeAttachmentBLL();
            return __equipmentNoticeAttachmentBLL;
        }
        private static IEquipmentNoticeReadBLL __equipmentNoticeReadBLL;
        public static IEquipmentNoticeReadBLL CreateEquipmentNoticeReadBLL()
        {
            if (__equipmentNoticeReadBLL == null)
                __equipmentNoticeReadBLL = new EquipmentNoticeReadBLL();
            return __equipmentNoticeReadBLL;
        }
        private static IEquipmentMarkingBLL __equipmentMarkingBLL;
        public static IEquipmentMarkingBLL CreateEuipmentMarkingBLL()
        {
            if (__equipmentMarkingBLL == null)
                __equipmentMarkingBLL = new EquipmentMarkingBLL();
            return __equipmentMarkingBLL;
        }
        private static IViewEquipmentMarkingBLL __viewEquipmentMarkingBLL;
        public static IViewEquipmentMarkingBLL CreateViewEuipmentMarkingBLL()
        {
            if (__viewEquipmentMarkingBLL == null)
                __viewEquipmentMarkingBLL = new ViewEquipmentMarkingBLL();
            return __viewEquipmentMarkingBLL;
        }
        private static IEquipmentTranningBLL __equipmentTranningBLL;
        public static IEquipmentTranningBLL CreateEquipmentTranningBLL()
        {
            if (__equipmentTranningBLL == null)
                __equipmentTranningBLL = new EquipmentTranningBLL();
            return __equipmentTranningBLL;
        }
        private static IViewEquipmentTrainningBLL __viewEquipmentTrainningBLL;
        public static IViewEquipmentTrainningBLL CreateViewEquipmentTrainningBLL()
        {
            if (__viewEquipmentTrainningBLL == null)
                __viewEquipmentTrainningBLL = new ViewEquipmentTrainningBLL();
            return __viewEquipmentTrainningBLL;
        }
        private static IEquipmentRepairBLL __equipmentRepairBLL;
        public static IEquipmentRepairBLL CreateEquipmentRepairBLL()
        {
            if (__equipmentRepairBLL == null)
                __equipmentRepairBLL = new EquipmentRepairBLL();
            return __equipmentRepairBLL;
        }
        private static IViewEquipmentRepairBLL __viewEquipmentRepairBLL;
        public static IViewEquipmentRepairBLL CreateViewEquipmentRepairBLL()
        {
            if (__viewEquipmentRepairBLL == null)
                __viewEquipmentRepairBLL = new ViewEquipmentRepairBLL();
            return __viewEquipmentRepairBLL;
        }
        private static IEquipmentPartBLL __equipmentPartBLL;
        public static IEquipmentPartBLL CreateEquipmentPartBLL()
        {
            if (__equipmentPartBLL == null)
                __equipmentPartBLL = new EquipmentPartBLL();
            return __equipmentPartBLL;
        }
        private static IEquipmentCategoryBLL __equipmentCategoryBLL;
        public static IEquipmentCategoryBLL CreateEquipmentCategoryBLL()
        {
            if (__equipmentCategoryBLL == null)
                __equipmentCategoryBLL = new EquipmentCategoryBLL();
            return __equipmentCategoryBLL;
        }
        private static IEquipmentCategoryBindBLL __equipmentCategoryBindBLL;
        public static IEquipmentCategoryBindBLL CreateEquipmentCategoryBindBLL()
        {
            if (__equipmentCategoryBindBLL == null)
                __equipmentCategoryBindBLL = new EquipmentCategoryBindBLL();
            return __equipmentCategoryBindBLL;
        }
        private static IViewEquipmentCategoryBindBLL __viewEquipmentCategoryBindBLL;
        public static IViewEquipmentCategoryBindBLL CreateViewEquipmentCategoryBindBLL()
        {
            if (__viewEquipmentCategoryBindBLL == null)
                __viewEquipmentCategoryBindBLL = new ViewEquipmentCategoryBindBLL();
            return __viewEquipmentCategoryBindBLL;
        }
        private static IEquipmentAttachmentBLL __equipmentAttachmentBLL;
        public static IEquipmentAttachmentBLL CreateEquipmentAttachmentBLL()
        {
            if (__equipmentAttachmentBLL == null)
                __equipmentAttachmentBLL = new EquipmentAttachmentBLL();
            return __equipmentAttachmentBLL;
        }
        private static IEquipmentLinkmanBLL __equipmentLinkmanBLL;
        public static IEquipmentLinkmanBLL CreateEquipmentLinkmanBLL()
        {
            if (__equipmentLinkmanBLL == null)
                __equipmentLinkmanBLL = new EquipmentLinkmanBLL();
            return __equipmentLinkmanBLL;
        }
        private static IEquipmentBlackListBLL __equipmentBlackListBLL;
        public static IEquipmentBlackListBLL CreateEquipmentBlackListBLL()
        {
            if (__equipmentBlackListBLL == null)
                __equipmentBlackListBLL = new EquipmentBlackListBLL();
            return __equipmentBlackListBLL;
        }
        private static IEquipmentBlackListMessageTemplateBLL __equipmentBlackListMessageTemplateBLL;
        public static IEquipmentBlackListMessageTemplateBLL CreateEquipmentBlackListMessageTemplateBLL()
        {
            if (__equipmentBlackListMessageTemplateBLL == null)
                __equipmentBlackListMessageTemplateBLL = new EquipmentBlackListMessageTemplateBLL();
            return __equipmentBlackListMessageTemplateBLL;
        }
        private static IEquipmentBrokenReportBLL __equipmentBrokenReportBLL;
        public static IEquipmentBrokenReportBLL CreateEquipmentBrokenReportBLL()
        {
            if (__equipmentBrokenReportBLL == null)
                __equipmentBrokenReportBLL = new EquipmentBrokenReportBLL();
            return __equipmentBrokenReportBLL;
        }
        private static IViewEquipmentBrokenReportBLL __viewEquipmentBrokenReportBLL;
        public static IViewEquipmentBrokenReportBLL CreateViewEquipmentBrokenReportBLL()
        {
            if (__viewEquipmentBrokenReportBLL == null)
                __viewEquipmentBrokenReportBLL = new ViewEquipmentBrokenReportBLL();
            return __viewEquipmentBrokenReportBLL;
        }
        private static IEquipmentStatusLogBLL __equipmentStatusLogBLL;
        public static IEquipmentStatusLogBLL CreateEquipmentStatusLogBLL()
        {
            if (__equipmentStatusLogBLL == null)
                __equipmentStatusLogBLL = new EquipmentStatusLogBLL();
            return __equipmentStatusLogBLL;
        }
        private static IViewEquipmentStatusLogBLL __viewEquipmentStatusLogBLL;
        public static IViewEquipmentStatusLogBLL CreateViewEquipmentStatusLogBLL()
        {
            if (__viewEquipmentStatusLogBLL == null)
                __viewEquipmentStatusLogBLL = new ViewEquipmentStatusLogBLL();
            return __viewEquipmentStatusLogBLL;
        }
        private static IUserWorkScopeBLL __userWorkScopeBLL;
        public static IUserWorkScopeBLL CreateUserWorkScopeBLL()
        {
            if (__userWorkScopeBLL == null)
                __userWorkScopeBLL = new UserWorkScopeBLL();
            return __userWorkScopeBLL;
        }
        private static IEquipmentAdditionChargeItemBLL __equipmentAdditionChargeItemBLL;
        public static IEquipmentAdditionChargeItemBLL CreateEquipmentAdditionChargeItemBLL()
        {
            if (__equipmentAdditionChargeItemBLL == null)
                __equipmentAdditionChargeItemBLL = new EquipmentAdditionChargeItemBLL();
            return __equipmentAdditionChargeItemBLL;
        }
        private static ICalcFeeTimeRuleBLL __calcFeeTimeRuleBLL;
        public static ICalcFeeTimeRuleBLL CreateCalcFeeTimeRuleBLL()
        {
            if (__calcFeeTimeRuleBLL == null)
                __calcFeeTimeRuleBLL = new CalcFeeTimeRuleBLL();
            return __calcFeeTimeRuleBLL;
        }
        private static IEquipmentAuthorizationAndAppointmentBLL __equipmentAuthorizationAndAppointmentBLL;
        public static IEquipmentAuthorizationAndAppointmentBLL CreateEquipmentAuthorizationAndAppointmentBLL()
        {
            if (__equipmentAuthorizationAndAppointmentBLL == null)
                __equipmentAuthorizationAndAppointmentBLL = new EquipmentAuthorizationAndAppointmentBLL();
            return __equipmentAuthorizationAndAppointmentBLL;
        }
        private static IUserEquipmentAuthorizationBLL __userEquipmentAuthorizationBLL;
        public static IUserEquipmentAuthorizationBLL CreateUserEquipmentAuthorizationBLL()
        {
            if (__userEquipmentAuthorizationBLL == null)
                __userEquipmentAuthorizationBLL = new UserEquipmentAuthorizationBLL();
            return __userEquipmentAuthorizationBLL;
        }
        private static IViewUserEquipmentAuthorizationBLL __viewUserEquipmentAuthorizationBLL;
        public static IViewUserEquipmentAuthorizationBLL CreateViewUserEquipmentAuthorizationBLL()
        {
            if (__viewUserEquipmentAuthorizationBLL == null)
                __viewUserEquipmentAuthorizationBLL = new ViewUserEquipmentAuthorizationBLL();
            return __viewUserEquipmentAuthorizationBLL;
        }
        private static IUserEquipmentContinuedAuthorizationBLL __userEquipmentContinuedAuthorizationBLL;
        public static IUserEquipmentContinuedAuthorizationBLL CreateUserEquipmentContinuedAuthorizationBLL()
        {
            if (__userEquipmentContinuedAuthorizationBLL == null)
                __userEquipmentContinuedAuthorizationBLL = new UserEquipmentContinuedAuthorizationBLL();
            return __userEquipmentContinuedAuthorizationBLL;
        }
        private static IViewUserEquipmentContinuedAuthorizationBLL __viewUserEquipmentContinuedAuthorizationBLLl;
        public static IViewUserEquipmentContinuedAuthorizationBLL CreateViewUserEquipmentContinuedAuthorizationBLL()
        {
            if (__viewUserEquipmentContinuedAuthorizationBLLl == null)
                __viewUserEquipmentContinuedAuthorizationBLLl = new ViewUserEquipmentContinuedAuthorizationBLL();
            return __viewUserEquipmentContinuedAuthorizationBLLl;
        }
        private static IEquipmentUseConditionBLL __equipmentUseConditionBLL;
        public static IEquipmentUseConditionBLL CreateEquipmentUseConditionBLL()
        {
            if (__equipmentUseConditionBLL == null)
                __equipmentUseConditionBLL = new EquipmentUseConditionBLL();
            return __equipmentUseConditionBLL;
        }
        private static IAppointmentEquipmentUseConditionBLL __appointmentEquipmentUseConditionBLL;
        public static IAppointmentEquipmentUseConditionBLL CreateAppointmentEquipmentUseConditionBLL()
        {
            if (__appointmentEquipmentUseConditionBLL == null)
                __appointmentEquipmentUseConditionBLL = new AppointmentEquipmentUseConditionBLL();
            return __appointmentEquipmentUseConditionBLL;
        }
        private static IViewEquipmentTotalBLL __viewEquipmentTotalBLL;
        public static IViewEquipmentTotalBLL CreateViewEquipmentTotalBLL()
        {
            if (__viewEquipmentTotalBLL == null)
                __viewEquipmentTotalBLL = new ViewEquipmentTotalBLL();
            return __viewEquipmentTotalBLL;
        }

        private static IEquipmentRelationBLL __equipmentRelationBLL;
        public static IEquipmentRelationBLL CreateEquipmentRelationBLL()
        {
            if (__equipmentRelationBLL == null)
                __equipmentRelationBLL = new EquipmentRelationBLL();
            return __equipmentRelationBLL;
        }
        private static IViewEquipmentRelationBLL __viewEquipmentRelationBLL;
        public static IViewEquipmentRelationBLL CreateViewEquipmentRelationBLL()
        {
            if (__viewEquipmentRelationBLL == null)
                __viewEquipmentRelationBLL = new ViewEquipmentRelationBLL();
            return __viewEquipmentRelationBLL;
        }
        private static IEquipmentGroupBLL __equipmentGroupBLL;
        public static IEquipmentGroupBLL CreateEquipmentGroupBLL()
        {
            if (__equipmentGroupBLL == null)
                __equipmentGroupBLL = new EquipmentGroupBLL();
            return __equipmentGroupBLL;
        }
        private static IEquipmentGroupAdminBLL __equipmentGroupAdminBLL;
        public static IEquipmentGroupAdminBLL CreateEquipmentGroupAdminBLL()
        {
            if (__equipmentGroupAdminBLL == null)
                __equipmentGroupAdminBLL = new EquipmentGroupAdminBLL();
            return __equipmentGroupAdminBLL;
        }
        private static IEquipmentGroupMemberBLL __equipmentGroupMemberBLL;
        public static IEquipmentGroupMemberBLL CreateEquipmentGroupMemberBLL()
        {
            if (__equipmentGroupMemberBLL == null)
                __equipmentGroupMemberBLL = new EquipmentGroupMemberBLL();
            return __equipmentGroupMemberBLL;
        }
        private static IViewEquipmentSiteMapBLL __viewEquipmentSiteMapBLL;
        public static IViewEquipmentSiteMapBLL CreateViewEquipmentSiteMapBLL()
        {
            if (__viewEquipmentSiteMapBLL == null)
                __viewEquipmentSiteMapBLL = new ViewEquipmentSiteMapBLL();
            return __viewEquipmentSiteMapBLL;
        }
        private static IEquipmentLogBLL __equipmentLogBLL;
        public static IEquipmentLogBLL CreateEquipmentLogBLL()
        {
            if (__equipmentLogBLL == null)
                __equipmentLogBLL = new EquipmentLogBLL();
            return __equipmentLogBLL;
        }
        private static IEquipmentImportBLL __equipmentImportBLL;
        public static IEquipmentImportBLL CreateEquipmentImportBLL()
        {
            if (__equipmentImportBLL == null)
                __equipmentImportBLL = new EquipmentImportBLL();
            return __equipmentImportBLL;
        }
        private static IViewPatentEquipmentBLL __viewPatentEquipmentBLL;
        public static IViewPatentEquipmentBLL CreateViewPatentEquipmentBLL()
        {
            if (__viewPatentEquipmentBLL == null)
                __viewPatentEquipmentBLL = new ViewPatentEquipmentBLL();
            return __viewPatentEquipmentBLL;
        }
        private static IEquipmentRoomDirectorBLL __equipmentRoomDirectorBLL;
        public static IEquipmentRoomDirectorBLL CreateEquipmentRoomDirectorBLL()
        {
            if (__equipmentRoomDirectorBLL == null)
                __equipmentRoomDirectorBLL = new EquipmentRoomDirectorBLL();
            return __equipmentRoomDirectorBLL;

        }
        private static IEquipmentProfessorBLL __equipmentProfessorBLL;
        public static IEquipmentProfessorBLL CreateEquipmentProfessorBLL()
        {
            if (__equipmentProfessorBLL == null)
                __equipmentProfessorBLL = new EquipmentProfessorBLL();
            return __equipmentProfessorBLL;
        }
        #endregion

        #region 用户设备
        private static IUserEquipmentPriviligeBLL __userEquipmentPriviligeBLL;
        public static IUserEquipmentPriviligeBLL CreateUserEquipmentPriviligeBLL()
        {
            if (__userEquipmentPriviligeBLL == null)
                __userEquipmentPriviligeBLL = new UserEquipmentPriviligeBLL();
            return __userEquipmentPriviligeBLL;
        }
        private static IViewUserEquipmentPriviligeBLL __viewUserEquipmentPriviligeBLL;
        public static IViewUserEquipmentPriviligeBLL CreateViewUserEquipmentPriviligeBLL()
        {
            if (__viewUserEquipmentPriviligeBLL == null)
                __viewUserEquipmentPriviligeBLL = new ViewUserEquipmentPriviligeBLL();
            return __viewUserEquipmentPriviligeBLL;
        }
        private static IUserEquipmentBLL __userEquipmentBLL;
        public static IUserEquipmentBLL CreateUserEquipmentBLL()
        {
            if (__userEquipmentBLL == null)
                __userEquipmentBLL = new UserEquipmentBLL();
            return __userEquipmentBLL;
        }
        private static IViewUserEquipmentBLL __viewUserEquipmentBLL;
        public static IViewUserEquipmentBLL CreateViewUserEquipmentBLL()
        {
            if (__viewUserEquipmentBLL == null)
                __viewUserEquipmentBLL = new ViewUserEquipmentBLL();
            return __viewUserEquipmentBLL;
        }
        #endregion

        #region 设备预约规则
        private static IUnAppointmentPeriodTimeBLL __unAppointmentPeriodTimeBLL;
        public static IUnAppointmentPeriodTimeBLL CreateUnAppointmentPeriodTimeBLL()
        {
            if (__unAppointmentPeriodTimeBLL == null)
                __unAppointmentPeriodTimeBLL = new UnAppointmentPeriodTimeBLL();
            return __unAppointmentPeriodTimeBLL;
        }
        private static IAppointmentPriorityBLL __appointmentPriorityBLL;
        public static IAppointmentPriorityBLL CreateAppointmentPriorityBLL()
        {
            if (__appointmentPriorityBLL == null)
                __appointmentPriorityBLL = new AppointmentPriorityBLL();
            return __appointmentPriorityBLL;
        }
        private static IAppointmentPriorityUserBLL __appointmentPriorityUserBLL;
        public static IAppointmentPriorityUserBLL CreateAppointmentPriorityUserBLL()
        {
            if (__appointmentPriorityUserBLL == null)
                __appointmentPriorityUserBLL = new AppointmentPriorityUserBLL();
            return __appointmentPriorityUserBLL;
        }
        private static IUserEquipmentTimeBLL __userEquipmentTimeBLL;
        public static IUserEquipmentTimeBLL CreateUserEquipmentTimeBLL()
        {
            if (__userEquipmentTimeBLL == null)
                __userEquipmentTimeBLL = new UserEquipmentTimeBLL();
            return __userEquipmentTimeBLL;
        }
        private static IUserEquipmentTimeUserBLL __userEquipmentTimeUserBLL;
        public static IUserEquipmentTimeUserBLL CreateUserEquipmentTimeUserBLL()
        {
            if (__userEquipmentTimeUserBLL == null)
                __userEquipmentTimeUserBLL = new UserEquipmentTimeUserBLL();
            return __userEquipmentTimeUserBLL;
        }
        private static IAppointmentTimeLimitBLL __appointmentTimeLimitBLL;
        public static IAppointmentTimeLimitBLL CreateAppointmentTimeLimitBLL()
        {
            if (__appointmentTimeLimitBLL == null)
                __appointmentTimeLimitBLL = new AppointmentTimeLimitBLL();
            return __appointmentTimeLimitBLL;
        }
        private static IAppointmentTimeLimitUserBLL __appointmentTimeLimitUserBLL;
        public static IAppointmentTimeLimitUserBLL CreateAppointmentTimeLimitUserBLL()
        {
            if (__appointmentTimeLimitUserBLL == null)
                __appointmentTimeLimitUserBLL = new AppointmentTimeLimitUserBLL();
            return __appointmentTimeLimitUserBLL;
        }
        private static ITagEquipmentFunBLL __tagEquipmentFunBLL;
        public static ITagEquipmentFunBLL CreateTagEquipmentFunBLL()
        {
            if (__tagEquipmentFunBLL == null)
                __tagEquipmentFunBLL = new TagEquipmentFunBLL();
            return __tagEquipmentFunBLL;
        }
        private static IPublicHolidaysBLL __publicHolidaysBLL;
        public static IPublicHolidaysBLL CreatePublicHolidaysBLL()
        {
            if (__publicHolidaysBLL == null)
                __publicHolidaysBLL = new PublicHolidaysBLL();
            return __publicHolidaysBLL;
        }
        private static IHolidaysExcludeBLL __holidaysExcludeBLL;
        public static IHolidaysExcludeBLL CreateHolidaysExcludeBLL()
        {
            if (__holidaysExcludeBLL == null)
                __holidaysExcludeBLL = new HolidaysExcludeBLL();
            return __holidaysExcludeBLL;
        }
        private static IHolidaysIncludeBLL __holidaysIncludeBLL;
        public static IHolidaysIncludeBLL CreateHolidaysIncludeBLL()
        {
            if (__holidaysIncludeBLL == null)
                __holidaysIncludeBLL = new HolidaysIncludeBLL();
            return __holidaysIncludeBLL;
        }
        private static IAppointmentSpeciaRuleBLL __appointmentSpeciaRuleBLL;
        public static IAppointmentSpeciaRuleBLL CreateAppointmentSpeciaRuleBLL()
        {
            if (__appointmentSpeciaRuleBLL == null)
                __appointmentSpeciaRuleBLL = new AppointmentSpeciaRuleBLL();
            return __appointmentSpeciaRuleBLL;
        }
        private static IAppointmentSpeciaRuleUserBLL __appointmentSpeciaRuleUserBLL;
        public static IAppointmentSpeciaRuleUserBLL CreateAppointmentSpeciaRuleUserBLL()
        {
            if (__appointmentSpeciaRuleUserBLL == null)
                __appointmentSpeciaRuleUserBLL = new AppointmentSpeciaRuleUserBLL();
            return __appointmentSpeciaRuleUserBLL;
        }
        private static ISubjectAppointmentTimeLimitBLL __subjectAppointmentTimeLimitBLL;
        public static ISubjectAppointmentTimeLimitBLL CreateSubjectAppointmentTimeLimitBLL()
        {
            if (__subjectAppointmentTimeLimitBLL == null)
                __subjectAppointmentTimeLimitBLL = new SubjectAppointmentTimeLimitBLL();
            return __subjectAppointmentTimeLimitBLL;
        }
        public static ISubjectAppointmentTimeLimitUserBLL __subjectAppointmentTimeLimitUserBLL;
        public static ISubjectAppointmentTimeLimitUserBLL CreateSubjectAppointmentTimeLimitUserBLL()
        {
            if (__subjectAppointmentTimeLimitUserBLL == null)
                __subjectAppointmentTimeLimitUserBLL = new SubjectAppointmentTimeLimitUserBLL();
            return __subjectAppointmentTimeLimitUserBLL;
        }
        #endregion

        #region 预约管理
        private static IViewAppointmentBLL __viewAppointmentBLL;
        public static IViewAppointmentBLL CreateViewAppointmentBLL()
        {
            if (__viewAppointmentBLL == null)
                __viewAppointmentBLL = new ViewAppointmentBLL();
            return __viewAppointmentBLL;
        }
        private static IAppointmentBLL __appointmentBLL;
        public static IAppointmentBLL CreateAppointmentBLL()
        {
            if (__appointmentBLL == null)
                __appointmentBLL = new AppointmentBLL();
            return __appointmentBLL;
        }
        private static IAppointmentEquipmentPartBLL __appointmentEquipmentPartBLL;
        public static IAppointmentEquipmentPartBLL CreateAppointmentEquipmentPartBLL()
        {
            if (__appointmentEquipmentPartBLL == null)
                __appointmentEquipmentPartBLL = new AppointmentEquipmentPartBLL();
            return __appointmentEquipmentPartBLL;
        }
        private static IAppointmentEquipmentAddtionChargeItemBLL __appointmentEquipmentAddtionChargeItemBLL;
        public static IAppointmentEquipmentAddtionChargeItemBLL CreateAppointmentEquipmentAddtionChargeItemBLL()
        {
            if (__appointmentEquipmentAddtionChargeItemBLL == null)
                __appointmentEquipmentAddtionChargeItemBLL = new AppointmentEquipmentAddtionChargeItemBLL();
            return __appointmentEquipmentAddtionChargeItemBLL;
        }
        private static IUsedConfirmBLL __usedConfirmBLL;
        public static IUsedConfirmBLL CreateUsedConfirmBLL()
        {
            if (__usedConfirmBLL == null)
                __usedConfirmBLL = new UsedConfirmBLL();
            return __usedConfirmBLL;
        }
        private static IUsedConfirmFeedBackBLL __usedConfirmFeedBackBLL;
        public static IUsedConfirmFeedBackBLL CreateUsedConfirmFeedBackBLL()
        {
            if (__usedConfirmFeedBackBLL == null)
                __usedConfirmFeedBackBLL = new UsedConfirmFeedBackBLL();
            return __usedConfirmFeedBackBLL;
        }
        private static IViewAppointmentEquipmentUseConditionBLL __viewAppointmentEquipmentUseConditionBLL;
        public static IViewAppointmentEquipmentUseConditionBLL CreateViewAppointmentEquipmentUseConditionBLL()
        {
            if (__viewAppointmentEquipmentUseConditionBLL == null)
                __viewAppointmentEquipmentUseConditionBLL = new ViewAppointmentEquipmentUseConditionBLL();
            return __viewAppointmentEquipmentUseConditionBLL;
        }
        #endregion

        #region 扣费管理
        private static ICostDeductSynBLL __costDeductSynBLL;
        public static ICostDeductSynBLL CreateCostDeductSynBLL()
        {
            if (__costDeductSynBLL == null)
                __costDeductSynBLL = new CostDeductSynBLL();
            return __costDeductSynBLL;
        }
        private static IManualCostDeductBLL __manualCostDeductBLL;
        public static IManualCostDeductBLL CreateManualCostDeductBLL()
        {
            if (__manualCostDeductBLL == null)
                __manualCostDeductBLL = new ManualCostDeductBLL();
            return __manualCostDeductBLL;
        }
        private static ICostDeductLogBLL __costDeductLogBLL;
        public static ICostDeductLogBLL CreateCostDeductLogBLL()
        {
            if (__costDeductLogBLL == null)
                __costDeductLogBLL = new CostDeductLogBLL();
            return __costDeductLogBLL;
        }
        private static ICostDeductBLL __costDeductBLL;
        public static ICostDeductBLL CreateCostDeductBLL()
        {
            if (__costDeductBLL == null)
                __costDeductBLL = new CostDeductBLL();
            return __costDeductBLL;
        }
        private static IChargeStandardBLL __chargeStandardBLL;
        public static IChargeStandardBLL CreateChargeStandardBLL()
        {
            if (__chargeStandardBLL == null)
                __chargeStandardBLL = new ChargeStandardBLL();
            return __chargeStandardBLL;
        }
        private static IViewCostDeductBLL __viewCostDeductBLL;
        public static IViewCostDeductBLL CreateViewCostDeductBLL()
        {
            if (__viewCostDeductBLL == null)
                __viewCostDeductBLL = new ViewCostDeductBLL();
            return __viewCostDeductBLL;
        }
        private static ICostDeductEquipmentOpenFundBLL __costDeductEquipmentOpenFundBLL;
        public static ICostDeductEquipmentOpenFundBLL CreateCostDeductEquipmentOpenFundBLL()
        {
            if (__costDeductEquipmentOpenFundBLL == null)
                __costDeductEquipmentOpenFundBLL = new CostDeductEquipmentOpenFundBLL();
            return __costDeductEquipmentOpenFundBLL;
        }
        private static IAnimalCostDeductBLL __animalCostDeductBLL;
        public static IAnimalCostDeductBLL CreateAnimalCostDeductBLL()
        {
            if (__animalCostDeductBLL == null)
                __animalCostDeductBLL = new AnimalCostDeductBLL();
            return __animalCostDeductBLL;
        }
        private static ICostDeductForStatisticsBLL __costDeductForStatisticsBLL;
        public static ICostDeductForStatisticsBLL CreateCostDeductForStatisticsBLL()
        {
            if (__costDeductForStatisticsBLL == null)
                __costDeductForStatisticsBLL = new CostDeductForStatisticsBLL();
            return __costDeductForStatisticsBLL;
        }
        private static IWaterControlCostDeductBLL __waterControlCostDeductBLL;
        public static IWaterControlCostDeductBLL CreateWaterControlCostDeductBLL()
        {
            if (__waterControlCostDeductBLL == null)
                __waterControlCostDeductBLL = new WaterControlCostDeductBLL();
            return __waterControlCostDeductBLL;
        }
        private static IManualTimeCostDeductBLL __manualTimeCostDeductBLL;
        public static IManualTimeCostDeductBLL CreateManualTimeCostDeductBLL()
        {
            if (__manualTimeCostDeductBLL == null)
                __manualTimeCostDeductBLL = new ManualTimeCostDeductBLL();
            return __manualTimeCostDeductBLL;
        }
        private static IMoneyValidateBLL __moneyValidateBLL;
        public static IMoneyValidateBLL CreateMoneyValidateBLL()
        {
            if (__moneyValidateBLL == null)
                __moneyValidateBLL = new ZsdxMoneyValidateBLL();
            return __moneyValidateBLL;
        }

        #endregion

        #region 处罚管理
        private static IDelinquencyCategoryBLL __delinquencyCategoryBLL;
        public static IDelinquencyCategoryBLL CreateDelinquencyCategoryBLL()
        {
            if (__delinquencyCategoryBLL == null)
                __delinquencyCategoryBLL = new DelinquencyCategoryBLL();
            return __delinquencyCategoryBLL;
        }
        private static IDelinquencyRuleBLL __delinquencyRuleBLL;
        public static IDelinquencyRuleBLL CreateDelinquencyRuleBLL()
        {
            if (__delinquencyRuleBLL == null)
                __delinquencyRuleBLL = new DelinquencyRuleBLL();
            return __delinquencyRuleBLL;
        }
        private static IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        public static IDelinquencyConfirmBLL CreateDelinquencyConfirmBLL()
        {
            if (__delinquencyConfirmBLL == null)
                __delinquencyConfirmBLL = new DelinquencyConfirmBLL();
            return __delinquencyConfirmBLL;
        }
        private static IViewDelinquencyConfirmBLL __viewDelinquencyConfirmBLL;
        public static IViewDelinquencyConfirmBLL CreateViewDelinquencyConfirmBLL()
        {
            if (__viewDelinquencyConfirmBLL == null)
                __viewDelinquencyConfirmBLL = new ViewDelinquencyConfirmBLL();
            return __viewDelinquencyConfirmBLL;
        }
        private static IPunishRecordBLL __punishRecordBLL;
        public static IPunishRecordBLL CreatePunishRecordBLL()
        {
            if (__punishRecordBLL == null)
                __punishRecordBLL = new PunishRecordBLL();
            return __punishRecordBLL;
        }
        private static IPunishActionBLL __punishActionBLL;
        public static IPunishActionBLL CreatePunishActionBLL()
        {
            if (__punishActionBLL == null)
                __punishActionBLL = new PunishActionBLL();
            return __punishActionBLL;
        }
        private static IRepealDelinquencyBLL __repealDelinquencyBLL;
        public static IRepealDelinquencyBLL CreateRepealDelinquencyBLL()
        {
            if (__repealDelinquencyBLL == null)
                __repealDelinquencyBLL = new RepealDelinquencyBLL();
            return __repealDelinquencyBLL;
        }
        private static IPunishActionDelinquencyBLL __punishActionDelinquencyBLL;
        public static IPunishActionDelinquencyBLL CreatePunishActionDelinquencyBLL()
        {
            if (__punishActionDelinquencyBLL == null)
                __punishActionDelinquencyBLL = new PunishActionDelinquencyBLL();
            return __punishActionDelinquencyBLL;
        }
        private static IViewPunishActionBLL __viewPunishActionBLL;
        public static IViewPunishActionBLL CreateViewActionBLL()
        {
            if (__viewPunishActionBLL == null)
                __viewPunishActionBLL = new ViewPunishActionBLL();
            return __viewPunishActionBLL;
        }
        #endregion

        #region 送样管理
        private static IViewSampleStatisticsBLL __viewSampleStatisticsBLL;
        public static IViewSampleStatisticsBLL CreateViewSampleStatisticsBLL()
        {
            if (__viewSampleStatisticsBLL == null)
                __viewSampleStatisticsBLL = new ViewSampleStatisticsBLL();
            return __viewSampleStatisticsBLL;
        }
        private static IViewSampleApplyChargeItemBLL __viewSampleApplyChargeItemBLL;
        public static IViewSampleApplyChargeItemBLL CreateViewSampleApplyChargeItemBLL()
        {
            if (__viewSampleApplyChargeItemBLL == null)
                __viewSampleApplyChargeItemBLL = new ViewSampleApplyChargeItemBLL();
            return __viewSampleApplyChargeItemBLL;
        }
        private static ISampleApplyNumberBLL __sampleApplyNumberBLL;
        public static ISampleApplyNumberBLL CreateSampleApplyNumberBLL()
        {
            if (__sampleApplyNumberBLL == null)
                __sampleApplyNumberBLL = new SampleApplyNumberBLL();
            return __sampleApplyNumberBLL;
        }
        private static ISampleApplyAttachmentBLL __sampleApplyAttachmentBLL;
        public static ISampleApplyAttachmentBLL CreateSampleApplyAttachmentBLL()
        {
            if (__sampleApplyAttachmentBLL == null)
                __sampleApplyAttachmentBLL = new SampleApplyAttachmentBLL();
            return __sampleApplyAttachmentBLL;
        }
        private static ISampleStatusBLL __sampleStatusBLL;
        public static ISampleStatusBLL CreateSampleStatusBLL()
        {
            if (__sampleStatusBLL == null)
                __sampleStatusBLL = new SampleStatusBLL();
            return __sampleStatusBLL;
        }
        private static ISampleItemStatusBLL __sampleItemStatusBLL;
        public static ISampleItemStatusBLL CreateSampleItemStatusBLL()
        {
            if (__sampleItemStatusBLL == null)
                __sampleItemStatusBLL = new SampleItemStatusBLL();
            return __sampleItemStatusBLL;
        }
        private static ISampleItemBLL __sampleItemBLL;
        public static ISampleItemBLL CreateSampleItemBLL()
        {
            if (__sampleItemBLL == null)
                __sampleItemBLL = new SampleItemBLL();
            return __sampleItemBLL;
        }
        private static IViewSampleItemBLL __viewSampleItemBLL;
        public static IViewSampleItemBLL CreateViewSampleItemBLL()
        {
            if (__viewSampleItemBLL == null)
                __viewSampleItemBLL = new ViewSampleItemBLL();
            return __viewSampleItemBLL;
        }
        private static ITagSampleItemDiscountBLL __tagSampleItemDiscountBLL;
        public static ITagSampleItemDiscountBLL CreateTagSampleItemDiscountBLL()
        {
            if (__tagSampleItemDiscountBLL == null)
                __tagSampleItemDiscountBLL = new TagSampleItemDiscountBLL();
            return __tagSampleItemDiscountBLL;
        }
        private static IUserSampleItemDiscountBLL __userSampleItemDiscountBLL;
        public static IUserSampleItemDiscountBLL CreateUserSampleItemDiscountBLL()
        {
            if (__userSampleItemDiscountBLL == null)
                __userSampleItemDiscountBLL = new UserSampleItemDiscountBLL();
            return __userSampleItemDiscountBLL;
        }
        private static IViewUserSampleItemDiscountBLL __viewUserSampleItemDiscountBLL;
        public static IViewUserSampleItemDiscountBLL CreateViewUserSampleItemDiscountBLL()
        {
            if (__viewUserSampleItemDiscountBLL == null)
                __viewUserSampleItemDiscountBLL = new ViewUserSampleItemDiscountBLL();
            return __viewUserSampleItemDiscountBLL;
        }
        private static ISampleChargeItemBLL __sampleChargeItemBLL;
        public static ISampleChargeItemBLL CreateSampleChargeItemBLL()
        {
            if (__sampleChargeItemBLL == null)
                __sampleChargeItemBLL = new SampleChargeItemBLL();
            return __sampleChargeItemBLL;
        }
        private static ISampleChargeItemRelateBLL __sampleChargeItemRelateBLL;
        public static ISampleChargeItemRelateBLL CreateSampleChargeItemRelateBLL()
        {
            if (__sampleChargeItemRelateBLL == null)
                __sampleChargeItemRelateBLL = new SampleChargeItemRelateBLL();
            return __sampleChargeItemRelateBLL;
        }
        private static IViewSampleChargeItemRelateBLL __viewSampleChargeItemRelateBLL;
        public static IViewSampleChargeItemRelateBLL CreateViewSampleChargeItemRelateBLL()
        {
            if (__viewSampleChargeItemRelateBLL == null)
                __viewSampleChargeItemRelateBLL = new ViewSampleChargeItemRelateBLL();
            return __viewSampleChargeItemRelateBLL;
        }
        private static IViewEquipmentSampleItemQueryBLL __viewEquipmentSampleItemQueryBLL;
        public static IViewEquipmentSampleItemQueryBLL CreateViewEquipmentSampleItemQueryBLL()
        {
            if (__viewEquipmentSampleItemQueryBLL == null)
                __viewEquipmentSampleItemQueryBLL = new ViewEquipmentSampleItemQueryBLL();
            return __viewEquipmentSampleItemQueryBLL;
        }
        private static IViewSampleApplyBLL __viewSampleApplyBLL;
        public static IViewSampleApplyBLL CreateViewSampleApplyBLL()
        {
            if (__viewSampleApplyBLL == null)
                __viewSampleApplyBLL = new ViewSampleApplyBLL();
            return __viewSampleApplyBLL;
        }
        private static ISampleApplyBLL __sampleApplyBLL;
        public static ISampleApplyBLL CreateSampleApplyBLL()
        {
            if (__sampleApplyBLL == null)
                __sampleApplyBLL = new SampleApplyBLL();
            return __sampleApplyBLL;
        }
        private static IViewSampleTesterBLL __viewSampleTesterBLL;
        public static IViewSampleTesterBLL CreateViewSampleTesterBLL()
        {
            if (__viewSampleTesterBLL == null)
                __viewSampleTesterBLL = new ViewSampleTesterBLL();
            return __viewSampleTesterBLL;
        }
        private static ISampleTesterBLL __sampleTesterBLL;
        public static ISampleTesterBLL CreateSampleTesterBLL()
        {
            if (__sampleTesterBLL == null)
                __sampleTesterBLL = new SampleTesterBLL();
            return __sampleTesterBLL;
        }
        private static ISampleTestRecordBLL __sampleTestRecordBLL;
        public static ISampleTestRecordBLL CreateSampleTestRecordBLL()
        {
            if (__sampleTestRecordBLL == null)
                __sampleTestRecordBLL = new SampleTestRecordBLL();
            return __sampleTestRecordBLL;
        }
        private static ISampleItemResultBLL __sampleItemResultBLL;
        public static ISampleItemResultBLL CreateSampleItemResultBLL()
        {
            if (__sampleItemResultBLL == null)
                __sampleItemResultBLL = new SampleItemResultBLL();
            return __sampleItemResultBLL;
        }
        private static ISampleAnalysisAttachmentBLL __sampleAnalysisAttachmentBLL;
        public static ISampleAnalysisAttachmentBLL CreateSampleAnalysisAttachmentBLL()
        {
            if (__sampleAnalysisAttachmentBLL == null)
                __sampleAnalysisAttachmentBLL = new SampleAnalysisAttachmentBLL();
            return __sampleAnalysisAttachmentBLL;
        }
        private static IViewSampleTestRecordBLL __viewSampleTestRecordBLL;
        public static IViewSampleTestRecordBLL CreateViewSampleTestRecordBLL()
        {
            if (__viewSampleTestRecordBLL == null)
                __viewSampleTestRecordBLL = new ViewSampleTestRecordBLL();
            return __viewSampleTestRecordBLL;
        }
        private static ISampleApplyChargeItemBLL __sampleApplyChargeItemBLL;
        public static ISampleApplyChargeItemBLL CreateSampleApplyChargeItemBLL()
        {
            if (__sampleApplyChargeItemBLL == null)
                __sampleApplyChargeItemBLL = new SampleApplyChargeItemBLL();
            return __sampleApplyChargeItemBLL;
        }
        private static ISampleParameterBLL __sampleParameterBLL;
        public static ISampleParameterBLL CreateSampleParameterBLL()
        {
            if (__sampleParameterBLL == null)
                __sampleParameterBLL = new SampleParameterBLL();
            return __sampleParameterBLL;
        }
        private static ISampleParameterEquipmentBLL __sampleParameterEquipmentBLL;
        public static ISampleParameterEquipmentBLL CreateSampleParameterEquipmentBLL()
        {
            if (__sampleParameterEquipmentBLL == null)
                __sampleParameterEquipmentBLL = new SampleParameterEquipmentBLL();
            return __sampleParameterEquipmentBLL;
        }
        private static ISampleParameterStatusBLL __sampleParameterStatusBLL;
        public static ISampleParameterStatusBLL CreateSampleParameterStatusBLL()
        {
            if (__sampleParameterStatusBLL == null)
                __sampleParameterStatusBLL = new SampleParameterStatusBLL();
            return __sampleParameterStatusBLL;
        }
        private static ISampleParameterItemBLL __sampleParameterItemBLL;
        public static ISampleParameterItemBLL CreateSampleParameterItemBLL()
        {
            if (__sampleParameterItemBLL == null)
                __sampleParameterItemBLL = new SampleParameterItemBLL();
            return __sampleParameterItemBLL;
        }
        private static IViewSampleParameterBLL __viewSampleParameterBLL;
        public static IViewSampleParameterBLL CreateViewSampleParameterBLL()
        {
            if (__viewSampleParameterBLL == null)
                __viewSampleParameterBLL = new ViewSampleParameterBLL();
            return __viewSampleParameterBLL;
        }
        private static ISampleApplyParameterBLL __sampleApplyParameterBLL;
        public static ISampleApplyParameterBLL CreateSampleApplyParameterBLL()
        {
            if (__sampleApplyParameterBLL == null)
                __sampleApplyParameterBLL = new SampleApplyParameterBLL();
            return __sampleApplyParameterBLL;
        }
        private static ISampleUnAppointmentPeriodTimeBLL __sampleUnAppointmentPeriodTimeBLL;
        public static ISampleUnAppointmentPeriodTimeBLL CreateSampleUnAppointmentPeriodTimeBLL()
        {
            if (__sampleUnAppointmentPeriodTimeBLL == null)
                __sampleUnAppointmentPeriodTimeBLL = new SampleUnAppointmentPeriodTimeBLL();
            return __sampleUnAppointmentPeriodTimeBLL;
        }
        private static ISampleApplyReportNumberBLL __sampleApplyReportNumberBLL;
        public static ISampleApplyReportNumberBLL CreateSampleApplyReportNumberBLL()
        {
            if (__sampleApplyReportNumberBLL == null)
                __sampleApplyReportNumberBLL = new SampleApplyReportNumberBLL();
            return __sampleApplyReportNumberBLL;
        }
        private static ISampleApplyReportNumberLogBLL __sampleApplyReportNumberLogBLL;
        public static ISampleApplyReportNumberLogBLL CreateSampleApplyReportNumberLogBLL()
        {
            if (__sampleApplyReportNumberLogBLL == null)
                __sampleApplyReportNumberLogBLL = new SampleApplyReportNumberLogBLL();
            return __sampleApplyReportNumberLogBLL;
        }
        private static IEquipmentSampleFeedBackSettingBLL __equipmentSampleFeedBackSettingBLL;
        public static IEquipmentSampleFeedBackSettingBLL CreateEquipmentSampleFeedBackSettingBLL()
        {
            if (__equipmentSampleFeedBackSettingBLL == null)
                __equipmentSampleFeedBackSettingBLL = new EquipmentSampleFeedBackSettingBLL();
            return __equipmentSampleFeedBackSettingBLL;
        }
        private static IViewEquipmentSampleFeedBackSettingBLL __viewEquipmentSampleFeedBackSettingBLL;
        public static IViewEquipmentSampleFeedBackSettingBLL CreateViewEquipmentSampleFeedBackSettingBLL()
        {
            if (__viewEquipmentSampleFeedBackSettingBLL == null)
                __viewEquipmentSampleFeedBackSettingBLL = new ViewEquipmentSampleFeedBackSettingBLL();
            return __viewEquipmentSampleFeedBackSettingBLL;
        }
        private static ISampleFeedBackAttachmentBLL __sampleFeedBackAttachmentBLL;
        public static ISampleFeedBackAttachmentBLL CreateSampleFeedBackAttachmentBLL()
        {
            if (__sampleFeedBackAttachmentBLL == null)
                __sampleFeedBackAttachmentBLL = new SampleFeedBackAttachmentBLL();
            return __sampleFeedBackAttachmentBLL;
        }
        private static ISampleItemLabelBLL __sampleItemLabelBLL;
        public static ISampleItemLabelBLL CreateSampleItemLabelBLL()
        {
            if (__sampleItemLabelBLL == null)
                __sampleItemLabelBLL = new SampleItemLabelBLL();
            return __sampleItemLabelBLL;
        }
        private static ISampleItemLabelItemBLL __sampleItemLabelItemBLL;
        public static ISampleItemLabelItemBLL CreateSampleItemLabelItemBLL()
        {
            if (__sampleItemLabelItemBLL == null)
                __sampleItemLabelItemBLL = new SampleItemLabelItemBLL();
            return __sampleItemLabelItemBLL;
        }
        private static IViewSampleItemLabelBLL __viewSampleItemLabelBLL;
        public static IViewSampleItemLabelBLL CreateViewSampleItemLabelBLL()
        {
            if (__viewSampleItemLabelBLL == null)
                __viewSampleItemLabelBLL = new ViewSampleItemLabelLabelBLL();
            return __viewSampleItemLabelBLL;
        }
        private static ISampleItemLabelChargeStandardBLL __sampleItemLabelChargeStandardBLL;
        public static ISampleItemLabelChargeStandardBLL CreateSampleItemLabelChargeStandardBLL()
        {
            if (__sampleItemLabelChargeStandardBLL == null)
                __sampleItemLabelChargeStandardBLL = new SampleItemLabelChargeStandardBLL();
            return __sampleItemLabelChargeStandardBLL;
        }
        private static IViewUnTestSampleApplyBLL __viewUnTestSampleApplyBLL;
        public static IViewUnTestSampleApplyBLL CreateViewUnTestSampleApplyBLL()
        {
            if (__viewUnTestSampleApplyBLL == null) __viewUnTestSampleApplyBLL = new ViewUnTestSampleApplyBLL();
            return __viewUnTestSampleApplyBLL;
        }
        #endregion

        #region 成果管理
        private static IAttachmentBLL __attachmentBLL;
        public static IAttachmentBLL CreateAttachmentBLL()
        {
            if (__attachmentBLL == null)
                __attachmentBLL = new AttachmentBLL();
            return __attachmentBLL;
        }
        private static ISubjectAchievementBLL __subjectAchievementBLL;
        public static ISubjectAchievementBLL CreateSubjectAchievementBLL()
        {
            if (__subjectAchievementBLL == null)
                __subjectAchievementBLL = new SubjectAchievementBLL();
            return __subjectAchievementBLL;
        }
        private static IViewSubjectAchievementBLL __viewSubjectAchievementBLL;
        public static IViewSubjectAchievementBLL CreateViewSubjectAchievementBLL()
        {
            if (__viewSubjectAchievementBLL == null)
                __viewSubjectAchievementBLL = new ViewSubjectAchievementBLL();
            return __viewSubjectAchievementBLL;
        }
        private static ISubjectMemberBLL __subjectMemberBLL;
        public static ISubjectMemberBLL CreateSubjectMemberBLL()
        {
            if (__subjectMemberBLL == null)
                __subjectMemberBLL = new SubjectMemberBLL();
            return __subjectMemberBLL;
        }
        private static ISubjectDepartmentBLL __subjectDepartmentBLL;
        public static ISubjectDepartmentBLL CreateSubjectDepartmentBLL()
        {
            if (__subjectDepartmentBLL == null)
                __subjectDepartmentBLL = new SubjectDepartmentBLL();
            return __subjectDepartmentBLL;
        }
        private static ISubjectEquipmentBLL __subjectEquipmentBLL;
        public static ISubjectEquipmentBLL CreateSubjectEquipmentBLL()
        {
            if (__subjectEquipmentBLL == null)
                __subjectEquipmentBLL = new SubjectEquipmentBLL();
            return __subjectEquipmentBLL;
        }
        private static IViewSubjectEquipmentBLL __viewSubjectEquipmentBLL;
        public static IViewSubjectEquipmentBLL CreateViewSubjectEquipmentBLL()
        {
            if (__viewSubjectEquipmentBLL == null)
                __viewSubjectEquipmentBLL = new ViewSubjectEquipmentBLL();
            return __viewSubjectEquipmentBLL;
        }
        private static IThesisBLL __thesisBLL;
        public static IThesisBLL CreateThesisBLL()
        {
            if (__thesisBLL == null)
                __thesisBLL = new ThesisBLL();
            return __thesisBLL;
        }
        private static IViewThesisBLL __viewThesisBLL;
        public static IViewThesisBLL CreateViewThesisBLL()
        {
            if (__viewThesisBLL == null)
                __viewThesisBLL = new ViewThesisBLL();
            return __viewThesisBLL;
        }
        private static IThesisAuthorBLL __thesisAuthorBLL;
        public static IThesisAuthorBLL CreateThesisAuthorBLL()
        {
            if (__thesisAuthorBLL == null)
                __thesisAuthorBLL = new ThesisAuthorBLL();
            return __thesisAuthorBLL;
        }
        private static IThesisDepartmentBLL __thesisDepartmentBLL;
        public static IThesisDepartmentBLL CreateThesisDepartmentBLL()
        {
            if (__thesisDepartmentBLL == null)
                __thesisDepartmentBLL = new ThesisDepartmentBLL();
            return __thesisDepartmentBLL;
        }
        private static IThesisEquipmentBLL __thesisEquipmentBLL;
        public static IThesisEquipmentBLL CreateThesisEquipmentBLL()
        {
            if (__thesisEquipmentBLL == null)
                __thesisEquipmentBLL = new ThesisEquipmentBLL();
            return __thesisEquipmentBLL;
        }
        private static IViewThesisEquipmentBLL __viewThesisEquipmentBLL;
        public static IViewThesisEquipmentBLL CreateViewThesisEquipmentBLL()
        {
            if (__viewThesisEquipmentBLL == null)
                __viewThesisEquipmentBLL = new ViewThesisEquipmentBLL();
            return __viewThesisEquipmentBLL;
        }
        private static IPublicationBLL __publicationBLL;
        public static IPublicationBLL CreatePublicationBLL()
        {
            if (__publicationBLL == null)
                __publicationBLL = new PublicationBLL();
            return __publicationBLL;
        }
        private static IViewPublicationBLL __viewPublicationBLL;
        public static IViewPublicationBLL CreateViewPublicationBLL()
        {
            if (__viewPublicationBLL == null)
                __viewPublicationBLL = new ViewPublicationBLL();
            return __viewPublicationBLL;
        }
        private static IPatentBLL __patentBLL;
        public static IPatentBLL CreatePatentBLL()
        {
            if (__patentBLL == null)
                __patentBLL = new PatentBLL();
            return __patentBLL;
        }
        private static IViewPatentBLL __viewPatentBLL;
        public static IViewPatentBLL CreateViewPatentBLL()
        {
            if (__viewPatentBLL == null)
                __viewPatentBLL = new ViewPatentBLL();
            return __viewPatentBLL;
        }
        private static IPatentUserBLL __patentUserBLL;
        public static IPatentUserBLL CreatePatentUserBLL()
        {
            if (__patentUserBLL == null)
                __patentUserBLL = new PatentUserBLL();
            return __patentUserBLL;
        }
        private static IPatentEquipmentBLL __patentEquipmentBLL;
        public static IPatentEquipmentBLL CreatePatentEquipmentBLL()
        {
            if (__patentEquipmentBLL == null)
                __patentEquipmentBLL = new PatentEquipmentBLL();
            return __patentEquipmentBLL;
        }
        private static IAwardsBLL __awardsBLL;
        public static IAwardsBLL CreateAwardsBLL()
        {
            if (__awardsBLL == null)
                __awardsBLL = new AwardsBLL();
            return __awardsBLL;
        }
        private static IViewAwardsBLL __viewAwardsBLL;
        public static IViewAwardsBLL CreateViewAwardsBLL()
        {
            if (__viewAwardsBLL == null)
                __viewAwardsBLL = new ViewAwardsBLL();
            return __viewAwardsBLL;
        }
        private static IAwardsAuthorBLL __awardsAuthorBLL;
        public static IAwardsAuthorBLL CreateAwardsAuthorBLL()
        {
            if (__awardsAuthorBLL == null)
                __awardsAuthorBLL = new AwardsAuthorBLL();
            return __awardsAuthorBLL;
        }
        private static IAwardsDepartmentBLL __awardsDepartmentBLL;
        public static IAwardsDepartmentBLL CreateAwardsDepartmentBLL()
        {
            if (__awardsDepartmentBLL == null)
                __awardsDepartmentBLL = new AwardsDepartmentBLL();
            return __awardsDepartmentBLL;
        }
        private static IAwardsEquipmentBLL __awardsEquipmentBLL;
        public static IAwardsEquipmentBLL CreateAwardsEquipmentBLL()
        {
            if (__awardsEquipmentBLL == null)
                __awardsEquipmentBLL = new AwardsEquipmentBLL();
            return __awardsEquipmentBLL;
        }
        private static IAcademicPositionBLL __academicPositionBLL;
        public static IAcademicPositionBLL CreateAcademicPositionBLL()
        {
            if (__academicPositionBLL == null)
                __academicPositionBLL = new AcademicPositionBLL();
            return __academicPositionBLL;
        }
        private static IViewAcademicPositionBLL __viewAcademicPositionBLL;
        public static IViewAcademicPositionBLL CreateViewAcademicPositionBLL()
        {
            if (__viewAcademicPositionBLL == null)
                __viewAcademicPositionBLL = new ViewAcademicPositionBLL();
            return __viewAcademicPositionBLL;
        }
        private static IAcademicExchangesBLL __academicExchangesBLL;
        public static IAcademicExchangesBLL CreateAcademicExchangesBLL()
        {
            if (__academicExchangesBLL == null)
                __academicExchangesBLL = new AcademicExchangesBLL();
            return __academicExchangesBLL;
        }
        private static IViewAcademicExchangesBLL __viewAcademicExchangesBLL;
        public static IViewAcademicExchangesBLL CreateViewAcademicExchangesBLL()
        {
            if (__viewAcademicExchangesBLL == null)
                __viewAcademicExchangesBLL = new ViewAcademicExchangesBLL();
            return __viewAcademicExchangesBLL;
        }
        private static IAcademicExchangesUserBLL __academicExchangesUserBLL;
        public static IAcademicExchangesUserBLL CreateAcademicExchangesUserBLL()
        {
            if (__academicExchangesUserBLL == null)
                __academicExchangesUserBLL = new AcademicExchangesUserBLL();
            return __academicExchangesUserBLL;
        }
        private static IAcademicExchangesDepBLL __academicExchangesDepBLL;
        public static IAcademicExchangesDepBLL CreateAcademicExchangesDepBLL()
        {
            if (__academicExchangesDepBLL == null)
                __academicExchangesDepBLL = new AcademicExchangesDepBLL();
            return __academicExchangesDepBLL;
        }
        private static IAchievementStudentBLL __achievementStudentBLL;
        public static IAchievementStudentBLL CreateAchievementStudentBLL()
        {
            if (__achievementStudentBLL == null)
                __achievementStudentBLL = new AchievementStudentBLL();
            return __achievementStudentBLL;
        }
        private static IViewAchievementStudentBLL __viewAchievementStudentBLL;
        public static IViewAchievementStudentBLL CreateViewAchievementStudentBLL()
        {
            if (__viewAchievementStudentBLL == null)
                __viewAchievementStudentBLL = new ViewAchievementStudentBLL();
            return __viewAchievementStudentBLL;
        }
        private static IPatentImportBLL __patentImportBLL;
        public static IPatentImportBLL CreatePatentImportBLL()
        {
            if (__patentImportBLL == null)
                __patentImportBLL = new PatentImportBLL();
            return __patentImportBLL;
        }
        #endregion

        #region 在线培训考试
        private static ITrainningMaterialBLL _trainningMaterialBLL;
        public static ITrainningMaterialBLL CreateTrainningMaterialBLL()
        {
            if (_trainningMaterialBLL == null)
                _trainningMaterialBLL = new TrainningMaterialBLL();
            return _trainningMaterialBLL;
        }
        private static IViewTrainningMaterialBLL __viewTrainningMaterialBLL;
        public static IViewTrainningMaterialBLL CreateViewTrainningMaterialBLL()
        {
            if (__viewTrainningMaterialBLL == null)
                __viewTrainningMaterialBLL = new ViewTrainningMaterialBLL();
            return __viewTrainningMaterialBLL;
        }
        private static ITrainningQuestionBLL __trainningQuestionBLL;
        public static ITrainningQuestionBLL CreateTrainningQuestionBLL()
        {
            if (__trainningQuestionBLL == null)
                __trainningQuestionBLL = new TrainningQuestionBLL();
            return __trainningQuestionBLL;
        }
        private static IViewTrainningQuestionBLL __viewTrainningQuestionBLL;
        public static IViewTrainningQuestionBLL CreateViewTrainningQuestionBLL()
        {
            if (__viewTrainningQuestionBLL == null)
                __viewTrainningQuestionBLL = new ViewTrainningQuestionBLL();
            return __viewTrainningQuestionBLL;
        }
        private static ITrainningQuestionItemBLL __trainningQuestionItemBLL;
        public static ITrainningQuestionItemBLL CreateTrainningQuestionItemBLL()
        {
            if (__trainningQuestionItemBLL == null)
                __trainningQuestionItemBLL = new TrainningQuestionItemBLL();
            return __trainningQuestionItemBLL;
        }
        private static ITrainningExaminationBLL __trainningExaminationBLL;
        public static ITrainningExaminationBLL CreateTrainningExaminationBLL()
        {
            if (__trainningExaminationBLL == null)
                __trainningExaminationBLL = new TrainningExaminationBLL();
            return __trainningExaminationBLL;
        }
        private static IViewTrainningExaminationBLL __viewTrainningExaminationBLL;
        public static IViewTrainningExaminationBLL CreateViewTrainningExaminationBLL()
        {
            if (__viewTrainningExaminationBLL == null)
                __viewTrainningExaminationBLL = new ViewTrainningExaminationBLL();
            return __viewTrainningExaminationBLL;
        }
        private static ITrainningExaminationQuestionBLL __trainningExaminationQuestionBLL;
        public static ITrainningExaminationQuestionBLL CreateTrainningExaminationQuestionBLL()
        {
            if (__trainningExaminationQuestionBLL == null)
                __trainningExaminationQuestionBLL = new TrainningExaminationQuestionBLL();
            return __trainningExaminationQuestionBLL;
        }
        private static ITrainningExaminationMaterialBLL __trainningExaminationMaterialBLL;
        public static ITrainningExaminationMaterialBLL CreateTrainningExaminationMaterialBLL()
        {
            if (__trainningExaminationMaterialBLL == null)
                __trainningExaminationMaterialBLL = new TrainningExaminationMaterialBLL();
            return __trainningExaminationMaterialBLL;
        }
        private static IUserExaminationBLL __userExaminationBLL;
        public static IUserExaminationBLL CreateUserExaminationBLL()
        {
            if (__userExaminationBLL == null)
                __userExaminationBLL = new UserExaminationBLL();
            return __userExaminationBLL;
        }
        private static IUserExaminationQuestionBLL __userExaminationQuestionBLL;
        public static IUserExaminationQuestionBLL CreateUserExaminationQuestionBLL()
        {
            if (__userExaminationQuestionBLL == null)
                __userExaminationQuestionBLL = new UserExaminationQuestionBLL();
            return __userExaminationQuestionBLL;
        }
        private static IViewUserEquipmentExaminationBLL __viewUserEquipmentExaminationBLL;
        public static IViewUserEquipmentExaminationBLL CreateViewUserEquipmentExaminationBLL()
        {
            if (__viewUserEquipmentExaminationBLL == null)
                __viewUserEquipmentExaminationBLL = new ViewUserEquipmentExaminationBLL();
            return __viewUserEquipmentExaminationBLL;
        }
        private static IViewUserLabOrganizationExaminationBLL __viewUserLabOrganizationExaminationBLL;
        public static IViewUserLabOrganizationExaminationBLL CreateViewUserLabOrganizationExaminationBLL()
        {
            if (__viewUserLabOrganizationExaminationBLL == null)
                __viewUserLabOrganizationExaminationBLL = new ViewUserLabOrganizationExaminationBLL();
            return __viewUserLabOrganizationExaminationBLL;
        }
        private static ITrainningTypeBLL __trainningTypeBLL;
        public static ITrainningTypeBLL CreateTrainningTypeBLL()
        {
            if (__trainningTypeBLL == null)
                __trainningTypeBLL = new TrainningTypeBLL();
            return __trainningTypeBLL;
        }
        public static IUserMaterialReadingTimeBLL CreateUserMaterialReadingTimeBLL()
        {
            return new UserMaterialReadingTimeBLL();
        }
        public static IViewUserMaterialReadingTimeBLL CreateViewUserMaterialReadingTimeBLL()
        {
            return new ViewUserMaterialReadingTimeBLL();
        }
        #endregion

        #region 使用记录管理
        private static IViewUsedConfirmBLL __viewUsedConfirmBLL;
        public static IViewUsedConfirmBLL CreateViewUsedConfirmBLL()
        {
            if (__viewUsedConfirmBLL == null)
                __viewUsedConfirmBLL = new ViewUsedConfirmBLL();
            return __viewUsedConfirmBLL;
        }
        private static IViewCurrentUsingEquipmentBLL __viewCurrentUsingEquipmentBLL;
        public static IViewCurrentUsingEquipmentBLL CreateViewCurrentUsingEquipmentBLL()
        {
            if (__viewCurrentUsingEquipmentBLL == null)
                __viewCurrentUsingEquipmentBLL = new ViewCurrentUsingEquipmentBLL();
            return __viewCurrentUsingEquipmentBLL;
        }
        private static IPowerOperationBLL __powerOperationBLL;
        public static IPowerOperationBLL CreatePowerOperationBLL()
        {
            if (__powerOperationBLL == null)
                __powerOperationBLL = new PowerOperationBLL();
            return __powerOperationBLL;
        }
        private static IViewPowerOperationBLL __viewPowerOperationBLL;
        public static IViewPowerOperationBLL CreateViewPowerOperationBLL()
        {
            if (__viewPowerOperationBLL == null)
                __viewPowerOperationBLL = new ViewPowerOperationBLL();
            return __viewPowerOperationBLL;
        }
        private static IViewCurrentUsingEquipmentByPowerOperationBLL __viewCurrentUsingEquipmentByPowerOperationBLL;
        public static IViewCurrentUsingEquipmentByPowerOperationBLL CreateViewCurrentUsingEquipmentByPowerOperationBLL()
        {
            if (__viewCurrentUsingEquipmentByPowerOperationBLL == null)
                __viewCurrentUsingEquipmentByPowerOperationBLL = new ViewCurrentUsingEquipmentByPowerOperationBLL();
            return __viewCurrentUsingEquipmentByPowerOperationBLL;
        }
        private static IViewUsedConfirmFeedBackBLL __viewUsedConfirmFeedBackBLL;
        public static IViewUsedConfirmFeedBackBLL CreateViewUsedConfirmFeedBackBLL()
        {
            if (__viewUsedConfirmFeedBackBLL == null)
                __viewUsedConfirmFeedBackBLL = new ViewUsedConfirmFeedBackBLL();
            return __viewUsedConfirmFeedBackBLL;
        }
        private static IEquipmentAccessBLL __equipmentAccessBLL;
        public static IEquipmentAccessBLL CreateEquipmentAccessBLL()
        {
            if (__equipmentAccessBLL == null)
                __equipmentAccessBLL = new EquipmentAccessBLL();
            return __equipmentAccessBLL;
        }
        private static IUsedConfirmEquipmentUseConditionBLL __usedConfirmEquipmentUseConditionBLL;
        public static IUsedConfirmEquipmentUseConditionBLL CreateUsedConfirmEquipmentUseConditionBLL()
        {
            if (__usedConfirmEquipmentUseConditionBLL == null)
                __usedConfirmEquipmentUseConditionBLL = new UsedConfirmEquipmentUseConditionBLL();
            return __usedConfirmEquipmentUseConditionBLL;
        }
        private static IUsedConfirmEquipmentPartBLL __usedConfirmEquipmentPartBLL;
        public static IUsedConfirmEquipmentPartBLL CreateUsedConfirmEquipmentPartBLL()
        {
            if (__usedConfirmEquipmentPartBLL == null)
                __usedConfirmEquipmentPartBLL = new UsedConfirmEquipmentPartBLL();
            return __usedConfirmEquipmentPartBLL;
        }
        private static IUsedConfirmEquipmentAddtionChargeItemBLL __usedConfirmEquipmentAddtionChargeItemBLL;
        public static IUsedConfirmEquipmentAddtionChargeItemBLL CreateUsedConfirmEquipmentAddtionChargeItemBLL()
        {
            if (__usedConfirmEquipmentAddtionChargeItemBLL == null)
                __usedConfirmEquipmentAddtionChargeItemBLL = new UsedConfirmEquipmentAddtionChargeItemBLL();
            return __usedConfirmEquipmentAddtionChargeItemBLL;
        }
        private static IViewUsedConfirmUseConditionBLL __viewUsedConfirmUseConditionBLL;
        public static IViewUsedConfirmUseConditionBLL CreateViewUsedConfirmUseConditionBLL()
        {
            if (__viewUsedConfirmUseConditionBLL == null)
                __viewUsedConfirmUseConditionBLL = new ViewUsedConfirmUseConditionBLL();
            return __viewUsedConfirmUseConditionBLL;
        }
        #endregion

        #region 结算管理
        private static IBalanceAccountBLL __balanceAccountBLL;
        public static IBalanceAccountBLL CreateBalanceAccountBLL()
        {
            if (__balanceAccountBLL == null)
                __balanceAccountBLL = new BalanceAccountBLL();
            return __balanceAccountBLL;
        }
        private static IViewBalanceAccountBLL __viewBalanceAccountBLL;
        public static IViewBalanceAccountBLL CreateViewBalanceAccountBLL()
        {
            if (__viewBalanceAccountBLL == null)
                __viewBalanceAccountBLL = new ViewBalanceAccountBLL();
            return __viewBalanceAccountBLL;
        }
        private static IBalanceAccountItemBLL __balanceAccountItemBLL;
        public static IBalanceAccountItemBLL CreateBalanceAccountItemBLL()
        {
            if (__balanceAccountItemBLL == null)
                __balanceAccountItemBLL = new BalanceAccountItemBLL();
            return __balanceAccountItemBLL;
        }
        private static IViewBalanceAccountItemBLL __viewBalanceAccountItemBLL;
        public static IViewBalanceAccountItemBLL CreateViewBalanceAccountItemBLL()
        {
            if (__viewBalanceAccountItemBLL == null)
                __viewBalanceAccountItemBLL = new ViewBalanceAccountItemBLL();
            return __viewBalanceAccountItemBLL;
        }
        #endregion

        #region 门禁管理
        private static IDoorBLL __doorBLL;
        public static IDoorBLL CreateDoorBLL()
        {
            if (__doorBLL == null)
                __doorBLL = new DoorBLL();
            return __doorBLL;
        }
        private static IViewDoorBLL __viewDoorBLL;
        public static IViewDoorBLL CreateViewDoorBLL()
        {
            if (__viewDoorBLL == null)
                __viewDoorBLL = new ViewDoorBLL();
            return __viewDoorBLL;
        }
        private static IUserDoorAuthorizationBLL __userDoorAuthorizationBLL;
        public static IUserDoorAuthorizationBLL CreateUserDoorAuthorizationBLL()
        {
            if (__userDoorAuthorizationBLL == null)
                __userDoorAuthorizationBLL = new UserDoorAuthorizationBLL();
            return __userDoorAuthorizationBLL;
        }
        private static IViewUserDoorAuthorizationBLL __viewUserDoorAuthorizationBLL;
        public static IViewUserDoorAuthorizationBLL CreateViewUserDoorAuthorizationBLL()
        {
            if (__viewUserDoorAuthorizationBLL == null)
                __viewUserDoorAuthorizationBLL = new ViewUserDoorAuthorizationBLL();
            return __viewUserDoorAuthorizationBLL;
        }
        private static IUserDoorContinuedAuthorizationBLL __userDoorContinuedAuthorizationBLL;
        public static IUserDoorContinuedAuthorizationBLL CreateUserDoorContinuedAuthorizationBLL()
        {
            if (__userDoorContinuedAuthorizationBLL == null)
                __userDoorContinuedAuthorizationBLL = new UserDoorContinuedAuthorizationBLL();
            return __userDoorContinuedAuthorizationBLL;
        }
        private static IViewUserDoorContinuedAuthorizationBLL __viewUserDoorContinuedAuthorizationBLL;
        public static IViewUserDoorContinuedAuthorizationBLL CreateViewUserDoorContinuedAuthorizationBLL()
        {
            if (__viewUserDoorContinuedAuthorizationBLL == null)
                __viewUserDoorContinuedAuthorizationBLL = new ViewUserDoorContinuedAuthorizationBLL();
            return __viewUserDoorContinuedAuthorizationBLL;
        }
        private static IDoorAccessRecordBLL __doorAccessRecordBLL;
        public static IDoorAccessRecordBLL CreateDoorAccessRecordBLL()
        {
            if (__doorAccessRecordBLL == null)
                __doorAccessRecordBLL = new DoorAccessRecordBLL();
            return __doorAccessRecordBLL;
        }
        private static IViewDoorAccessRecordBLL __viewDoorAccessRecordBLL;
        public static IViewDoorAccessRecordBLL CreateViewDoorAccessRecordBLL()
        {
            if (__viewDoorAccessRecordBLL == null)
                __viewDoorAccessRecordBLL = new ViewDoorAccessRecordBLL();
            return __viewDoorAccessRecordBLL;
        }
        private static IDoorOfflineAuthorizationBLL __doorOfflineAuthorizationBLL;
        public static IDoorOfflineAuthorizationBLL CreateDoorOfflineAuthorizationBLL()
        {
            if (__doorOfflineAuthorizationBLL == null)
                __doorOfflineAuthorizationBLL = new DoorOfflineAuthorizationBLL();
            return __doorOfflineAuthorizationBLL;
        }
        private static IViewDoorEquipmentBLL __viewDoorEquipmentBLL;
        public static IViewDoorEquipmentBLL CreateViewDoorEquipmentBLL()
        {
            if (__viewDoorEquipmentBLL == null)
                __viewDoorEquipmentBLL = new ViewDoorEquipmentBLL();
            return __viewDoorEquipmentBLL;
        }
        #endregion

        #region 视频管理
        private static ICameraBLL __cameraBLL;
        public static ICameraBLL CreateCameraBLL()
        {
            if (__cameraBLL == null)
                __cameraBLL = new CameraBLL();
            return __cameraBLL;
        }
        private static IViewCameraBLL __viewCameraBLL;
        public static IViewCameraBLL CreateViewCameraBLL()
        {
            if (__viewCameraBLL == null)
                __viewCameraBLL = new ViewCameraBLL();
            return __viewCameraBLL;
        }
        private static ICameraRelationBLL __cameraRelationBLL;
        public static ICameraRelationBLL CreateCameraRelationBLL()
        {
            if (__cameraRelationBLL == null)
                __cameraRelationBLL = new CameraRelationBLL();
            return __cameraRelationBLL;
        }
        private static IViewCameraRelationBLL __viewCameraRelationBLL;
        public static IViewCameraRelationBLL CreateViewCameraRelationBLL()
        {
            if (__viewCameraRelationBLL == null)
                __viewCameraRelationBLL = new ViewCameraRelationBLL();
            return __viewCameraRelationBLL;
        }
        #endregion

        #region 网盘管理
        private static ISubAreaBLL __subAreaBLL;
        public static ISubAreaBLL CreateSubAreaBLL()
        {
            if (__subAreaBLL == null)
                __subAreaBLL = new SubAreaBLL();
            return __subAreaBLL;
        }
        private static ISubAreaCategoryBLL __subAreaCategoryBLL;
        public static ISubAreaCategoryBLL CreateSubAreaCategoryBLL()
        {
            if (__subAreaCategoryBLL == null)
                __subAreaCategoryBLL = new SubAreaCategoryBLL();
            return __subAreaCategoryBLL;
        }
        private static ISubAreaFileBLL __subAreaFileBLL;
        public static ISubAreaFileBLL CreateSubAreaFileBLL()
        {
            if (__subAreaFileBLL == null)
                __subAreaFileBLL = new SubAreaFileBLL();
            return __subAreaFileBLL;
        }
        private static ISubAreaTagPowerBLL __subAreaTagPowerBLL;
        public static ISubAreaTagPowerBLL CreateSubAreaTagPowerBLL()
        {
            if (__subAreaTagPowerBLL == null)
                __subAreaTagPowerBLL = new SubAreaTagPowerBLL();
            return __subAreaTagPowerBLL;
        }
        private static ISubAreaUerPowerBLL __subAreaUerPowerBLL;
        public static ISubAreaUerPowerBLL CreateSubAreaUerPowerBLL()
        {
            if (__subAreaUerPowerBLL == null)
                __subAreaUerPowerBLL = new SubAreaUerPowerBLL();
            return __subAreaUerPowerBLL;
        }
        private static ISubAreaUserWithoutPowerBLL __subAreaUserWithoutPowerBLL;
        public static ISubAreaUserWithoutPowerBLL CreateSubAreaUserWithoutPowerBLL()
        {
            if (__subAreaUserWithoutPowerBLL == null)
                __subAreaUserWithoutPowerBLL = new SubAreaUserWithoutPowerBLL();
            return __subAreaUserWithoutPowerBLL;
        }
        #endregion

        #region 基础信息
        private static ICountryBLL __countryBLL;
        public static ICountryBLL CreateCountryBLL()
        {
            if (__countryBLL == null)
                __countryBLL = new CountryBLL();
            return __countryBLL;
        }
        private static IRepairBLL __repairBLL;
        public static IRepairBLL CreareRepairBLL()
        {
            if (__repairBLL == null)
                __repairBLL = new RepairBLL();
            return __repairBLL;
        }
        #endregion

        #region LUA注册
        private static ILuaRegiserBLL __luaRegiserBLL;
        public static ILuaRegiserBLL CreateLuaRegiserBLL()
        {
            if (__luaRegiserBLL == null)
                __luaRegiserBLL = new LuaRegiserBLL();
            return __luaRegiserBLL;
        }
        #endregion

        #region 校级用户
        private static ISchoolUserBLL __schoolUserBLL;
        public static ISchoolUserBLL CreateSchoolUserBLL()
        {
            if (__schoolUserBLL == null)
                __schoolUserBLL = new SchoolUserBLL();
            return __schoolUserBLL;
        }
        private static IViewSchoolUserBLL __viewSchoolUserBLL;
        public static IViewSchoolUserBLL CreateViewSchoolUserBLL()
        {
            if (__viewSchoolUserBLL == null)
                __viewSchoolUserBLL = new ViewSchoolUserBLL();
            return __viewSchoolUserBLL;
        }
        private static ISchoolUserLogBLL __schoolUserLogBLL;
        public static ISchoolUserLogBLL CreateSchoolUserLogBLL()
        {
            if (__schoolUserLogBLL == null)
                __schoolUserLogBLL = new SchoolUserLogBLL();
            return __schoolUserLogBLL;
        }
        private static IViewSchoolUserLogBLL __viewSchoolUserLogBLL;
        public static IViewSchoolUserLogBLL CreateViewSchoolUserLogBLL()
        {
            if (__viewSchoolUserLogBLL == null)
                __viewSchoolUserLogBLL = new ViewSchoolUserLogBLL();
            return __viewSchoolUserLogBLL;
        }
        #endregion

        #region 校级组织机构
        private static ISchoolOrganizationBLL __schoolOrganizationBLL;
        public static ISchoolOrganizationBLL CreateSchoolOrganizationBLL()
        {
            if (__schoolOrganizationBLL == null)
                __schoolOrganizationBLL = new SchoolOrganizationBLL();
            return __schoolOrganizationBLL;
        }
        #endregion

        #region 设备入网申请
        private static IEquipmentApplyBLL __equipmentApplyBLL;
        public static IEquipmentApplyBLL CreateEquipmentApplyBLL()
        {
            if (__equipmentApplyBLL == null)
                __equipmentApplyBLL = new EquipmentApplyBLL();
            return __equipmentApplyBLL;
        }
        private static IEquipmentGroupServerBLL __equipmentGroupServerBLL;
        public static IEquipmentGroupServerBLL CreateEquipmentGroupServerBLL()
        {
            if (__equipmentGroupServerBLL == null)
                __equipmentGroupServerBLL = new EquipmentGroupServerBLL();
            return __equipmentGroupServerBLL;
        }
        private static IEquipmentServiceHoursStatBLL __equipmentServiceHoursStatBLL;
        public static IEquipmentServiceHoursStatBLL CreateEquipmentServiceHoursStatBLL()
        {
            if (__equipmentServiceHoursStatBLL == null)
                __equipmentServiceHoursStatBLL = new EquipmentServiceHoursStatBLL();
            return __equipmentServiceHoursStatBLL;
        }
        private static IViewEquipmentApplyBLL __viewEquipmentApplyBLL;
        public static IViewEquipmentApplyBLL CreateViewEquipmentApplyBLL()
        {
            if (__viewEquipmentApplyBLL == null)
                __viewEquipmentApplyBLL = new ViewEquipmentApplyBLL();
            return __viewEquipmentApplyBLL;
        }
        #endregion

        #region 数据字典模板
        private static IDataDictTemplateBLL __dataDictTemplateBLL;
        public static IDataDictTemplateBLL CreateDataDictTemplateBLL()
        {
            if (__dataDictTemplateBLL == null)
                __dataDictTemplateBLL = new DataDictTemplateBLL();
            return __dataDictTemplateBLL;
        }
        #endregion

        #region 科研收入明细对接
        private static ISubjectProjectSyncBLL __subjectProjectSyncBLL;
        public static ISubjectProjectSyncBLL CreateSubjectProjectSyncBLL()
        {
            if (__subjectProjectSyncBLL == null)
                __subjectProjectSyncBLL = new SubjectProjectSyncBLL();
            return __subjectProjectSyncBLL;
        }
        private static ISubjectProjectImcomeLogBLL __subjectProjectImcomeLogBLL;
        public static ISubjectProjectImcomeLogBLL CreateSubjectProjectImcomeLogBLL()
        {
            if (__subjectProjectImcomeLogBLL == null)
                __subjectProjectImcomeLogBLL = new SubjectProjectImcomeLogBLL();
            return __subjectProjectImcomeLogBLL;
        }
        private static ISubjectProjectImcomeErrorLogBLL __subjectProjectImcomeErrorLogBLL;
        public static ISubjectProjectImcomeErrorLogBLL CreateSubjectProjectImcomeErrorLogBLL()
        {
            if (__subjectProjectImcomeErrorLogBLL == null)
                __subjectProjectImcomeErrorLogBLL = new SubjectProjectImcomeErrorLogBLL();
            return __subjectProjectImcomeErrorLogBLL;
        }
        private static IViewSubjectProjectImcomeBLL __viewSubjectProjectImcomeBLL;
        public static IViewSubjectProjectImcomeBLL CreateViewSubjectProjectImcomeBLL()
        {
            if (__viewSubjectProjectImcomeBLL == null)
                __viewSubjectProjectImcomeBLL = new ViewSubjectProjectImcomeBLL();
            return __viewSubjectProjectImcomeBLL;
        }
        private static IViewSubjectProjectImcomeLogBLL __viewSubjectProjectImcomeLogBLL;
        public static IViewSubjectProjectImcomeLogBLL CreateViewSubjectProjectImcomeLogBLL()
        {
            if (__viewSubjectProjectImcomeLogBLL == null)
                __viewSubjectProjectImcomeLogBLL = new ViewSubjectProjectImcomeLogBLL();
            return __viewSubjectProjectImcomeLogBLL;
        }
        #endregion

        #region 设备维修基金申请
        private static IEquipmentRepairFundsApplyBLL __equipmentRepairFundsApplyBLL;
        public static IEquipmentRepairFundsApplyBLL CreateEquipmentRepairFundsApplyBLL()
        {
            if (__equipmentRepairFundsApplyBLL == null)
                __equipmentRepairFundsApplyBLL = new EquipmentRepairFundsApplyBLL();
            return __equipmentRepairFundsApplyBLL;
        }
        private static IViewEquipmentRepairFundsApplyBLL __viewEquipmentRepairFundsApplyBLL;
        public static IViewEquipmentRepairFundsApplyBLL CreateViewEquipmentRepairFundsApplyBLL()
        {
            if (__viewEquipmentRepairFundsApplyBLL == null)
                __viewEquipmentRepairFundsApplyBLL = new ViewEquipmentRepairFundsApplyBLL();
            return __viewEquipmentRepairFundsApplyBLL;
        }
        private static IEquipmentRepairFundsApplyAttachmentBLL __equipmentRepairFundsApplyAttachmentBLL;
        public static IEquipmentRepairFundsApplyAttachmentBLL CreateEquipmentRepairFundsApplyAttachmentBLL()
        {
            if (__equipmentRepairFundsApplyAttachmentBLL == null)
                __equipmentRepairFundsApplyAttachmentBLL = new EquipmentRepairFundsApplyAttachmentBLL();
            return __equipmentRepairFundsApplyAttachmentBLL;
        }
        #endregion

        #region 升级工具
        private static ISystemUpgradeLogBLL __systemUpgradeLogBLL;
        public static ISystemUpgradeLogBLL CreateSystemUpgradeLogBLL()
        {
            if (__systemUpgradeLogBLL == null)
                __systemUpgradeLogBLL = new SystemUpgradeLogBLL();
            return __systemUpgradeLogBLL;
        }
        #endregion

        #region 冰箱监控
        private static IViewSssJBLL __viewSssJBLL;
        public static IViewSssJBLL CreateViewSssJBLL()
        {
            if (__viewSssJBLL == null) __viewSssJBLL = new ViewSssJBLL("IceBox");
            return __viewSssJBLL;
        }
        #endregion

        #region 统计信息
        private static IStatisticsInfoBLL __statisticsInfoBLL;
        public static IStatisticsInfoBLL CreateStatisticsInfoBLL()
        {
            if (__statisticsInfoBLL == null) __statisticsInfoBLL = new StatisticsInfoBLL();
            return __statisticsInfoBLL;
        }
        #endregion

        #region 授权升级
        private static IProjectBLL __projectBLL;
        public static IProjectBLL CreateProjectBLL()
        {
            if (__projectBLL == null) __projectBLL = new ProjectBLL();
            return __projectBLL;
        }

        private static IFeedbackBLL __feedbackBLL;
        public static IFeedbackBLL CreateFeedbackBLL()
        {
            if (__feedbackBLL == null) __feedbackBLL = new FeedbackBLL();
            return __feedbackBLL;
        }
        #endregion

        #region 耗材管理

        private static ISupplierBLL __supplierBLL;
        public static ISupplierBLL CreateSupplierBLL()
        {
            if (__supplierBLL == null) __supplierBLL = new SupplierBLL();
            return __supplierBLL;
        }
        private static IMaterialSupplierBLL __materialSupplierBLL;
        public static IMaterialSupplierBLL CreateMaterialSupplierBLL()
        {
            if (__materialSupplierBLL == null) __materialSupplierBLL = new MaterialSupplierBLL();
            return __materialSupplierBLL;
        }
        private static IViewMaterialSupplierBLL __viewMaterialSupplierBLL;
        public static IViewMaterialSupplierBLL CreateViewMaterialSupplierBLL()
        {
            if (__viewMaterialSupplierBLL == null) __viewMaterialSupplierBLL = new ViewMaterialSupplierBLL();
            return __viewMaterialSupplierBLL;
        }
        private static IMaterialCategoryBLL __materialCategoryBLL;
        public static IMaterialCategoryBLL CreateMaterialCategoryBLL()
        {
            if (__materialCategoryBLL == null) __materialCategoryBLL = new MaterialCategoryBLL();
            return __materialCategoryBLL;
        }
        private static IMaterialInfoBLL __materialInfoBLL;
        public static IMaterialInfoBLL CreateMaterialInfoBLL()
        {
            if (__materialInfoBLL == null) __materialInfoBLL = new MaterialInfoBLL();
            return __materialInfoBLL;
        }
        private static IViewMaterialInfoBLL __viewMaterialInfoBLL;
        public static IViewMaterialInfoBLL CreateViewMaterialInfoBLL()
        {
            if (__viewMaterialInfoBLL == null) __viewMaterialInfoBLL = new ViewMaterialInfoBLL();
            return __viewMaterialInfoBLL;
        }
        private static IMaterialAdminBLL __materialAdminBLL;
        public static IMaterialAdminBLL CreateMaterialAdminBLL()
        {
            if (__materialAdminBLL == null) __materialAdminBLL = new MaterialAdminBLL();
            return __materialAdminBLL;
        }
        private static IMaterialPurchaseBLL __materialPurchaseBLL;
        public static IMaterialPurchaseBLL CreateMaterialPurchaseBLL()
        {
            if (__materialPurchaseBLL == null) __materialPurchaseBLL = new MaterialPurchaseBLL();
            return __materialPurchaseBLL;
        }
        private static IViewMaterialPurchaseBLL __viewMaterialPurchaseBLL;
        public static IViewMaterialPurchaseBLL CreateViewMaterialPurchaseBLL()
        {
            if (__viewMaterialPurchaseBLL == null) __viewMaterialPurchaseBLL = new ViewMaterialPurchaseBLL();
            return __viewMaterialPurchaseBLL;
        }
        private static IMaterialPurchaseItemBLL __materialPurchaseItemBLL;
        public static IMaterialPurchaseItemBLL CreateMaterialPurchaseItemBLL()
        {
            if (__materialPurchaseItemBLL == null) __materialPurchaseItemBLL = new MaterialPurchaseItemBLL();
            return __materialPurchaseItemBLL;
        }
        private static IViewMaterialPurchaseItemBLL __viewMaterialPurchaseItemBLL;
        public static IViewMaterialPurchaseItemBLL CreateViewMaterialPurchaseItemBLL()
        {
            if (__viewMaterialPurchaseItemBLL == null) __viewMaterialPurchaseItemBLL = new ViewMaterialPurchaseItemBLL();
            return __viewMaterialPurchaseItemBLL;
        }
        private static IMaterialInputBLL __materialInputBLL;
        public static IMaterialInputBLL CreateMaterialInputBLL()
        {
            if (__materialInputBLL == null) __materialInputBLL = new MaterialInputBLL();
            return __materialInputBLL;
        }
        private static IViewMaterialInputBLL __viewMaterialInputBLL;
        public static IViewMaterialInputBLL CreateViewMaterialInputBLL()
        {
            if (__viewMaterialInputBLL == null) __viewMaterialInputBLL = new ViewMaterialInputBLL();
            return __viewMaterialInputBLL;
        }
        private static IMaterialInputItemBLL __materialInputItemBLL;
        public static IMaterialInputItemBLL CreateMaterialInputItemBLL()
        {
            if (__materialInputItemBLL == null) __materialInputItemBLL = new MaterialInputItemBLL();
            return __materialInputItemBLL;
        }
        private static IViewMaterialInputItemBLL __viewMaterialInputItemBLL;
        public static IViewMaterialInputItemBLL CreateViewMaterialInputItemBLL()
        {
            if (__viewMaterialInputItemBLL == null) __viewMaterialInputItemBLL = new ViewMaterialInputItemBLL();
            return __viewMaterialInputItemBLL;
        }
        private static IMaterialRecipientBLL __materialRecipientBLL;
        public static IMaterialRecipientBLL CreateMaterialRecipientBLL()
        {
            if (__materialRecipientBLL == null) __materialRecipientBLL = new MaterialRecipientBLL();
            return __materialRecipientBLL;
        }
        private static IViewMaterialRecipientBLL __viewMaterialRecipientBLL;
        public static IViewMaterialRecipientBLL CreateViewMaterialRecipientBLL()
        {
            if (__viewMaterialRecipientBLL == null) __viewMaterialRecipientBLL = new ViewMaterialRecipientBLL();
            return __viewMaterialRecipientBLL;
        }
        private static IMaterialRecipientItemBLL __materialRecipientItemBLL;
        public static IMaterialRecipientItemBLL CreateMaterialRecipientItemBLL()
        {
            if (__materialRecipientItemBLL == null) __materialRecipientItemBLL = new MaterialRecipientItemBLL();
            return __materialRecipientItemBLL;
        }
        private static IViewMaterialRecipientItemBLL __viewMaterialRecipientItemBLL;
        public static IViewMaterialRecipientItemBLL CreateViewMaterialRecipientItemBLL()
        {
            if (__viewMaterialRecipientItemBLL == null) __viewMaterialRecipientItemBLL = new ViewMaterialRecipientItemBLL();
            return __viewMaterialRecipientItemBLL;
        }
        private static IMaterialOutputBLL __materialOutputBLL;
        public static IMaterialOutputBLL CreateMaterialOutputBLL()
        {
            if (__materialOutputBLL == null) __materialOutputBLL = new MaterialOutputBLL();
            return __materialOutputBLL;
        }
        private static IViewMaterialOutputBLL __viewMaterialOutputBLL;
        public static IViewMaterialOutputBLL CreateViewMaterialOutputBLL()
        {
            if (__viewMaterialOutputBLL == null) __viewMaterialOutputBLL = new ViewMaterialOutputBLL();
            return __viewMaterialOutputBLL;
        }
        private static IMaterialOutputItemBLL __materialOutputItemBLL;
        public static IMaterialOutputItemBLL CreateMaterialOutputItemBLL()
        {
            if (__materialOutputItemBLL == null) __materialOutputItemBLL = new MaterialOutputItemBLL();
            return __materialOutputItemBLL;
        }
        private static IViewMaterialOutputItemBLL __viewMaterialOutputItemBLL;
        public static IViewMaterialOutputItemBLL CreateViewMaterialOutputItemBLL()
        {
            if (__viewMaterialOutputItemBLL == null) __viewMaterialOutputItemBLL = new ViewMaterialOutputItemBLL();
            return __viewMaterialOutputItemBLL;
        }
        private static IMaterialBrokenBLL __materialBrokenBLL;
        public static IMaterialBrokenBLL CreateMaterialBrokenBLL()
        {
            if (__materialBrokenBLL == null) __materialBrokenBLL = new MaterialBrokenBLL();
            return __materialBrokenBLL;
        }
        private static IViewMaterialBrokenBLL __viewMaterialBrokenBLL;
        public static IViewMaterialBrokenBLL CreateViewMaterialBrokenBLL()
        {
            if (__viewMaterialBrokenBLL == null) __viewMaterialBrokenBLL = new ViewMaterialBrokenBLL();
            return __viewMaterialBrokenBLL;
        }
        private static IMaterialBrokenItemBLL __materialBrokenItemBLL;
        public static IMaterialBrokenItemBLL CreateMaterialBrokenItemBLL()
        {
            if (__materialBrokenItemBLL == null) __materialBrokenItemBLL = new MaterialBrokenItemBLL();
            return __materialBrokenItemBLL;
        }
        private static IViewMaterialBrokenItemBLL __viewMaterialBrokenItemBLL;
        public static IViewMaterialBrokenItemBLL CreateViewMaterialBrokenItemBLL()
        {
            if (__viewMaterialBrokenItemBLL == null) __viewMaterialBrokenItemBLL = new ViewMaterialBrokenItemBLL();
            return __viewMaterialBrokenItemBLL;
        }
        private static IMaterialDepositRecordBLL __materialDepositRecordBLL;
        public static IMaterialDepositRecordBLL CreateMaterialDepositRecordBLL()
        {
            if (__materialDepositRecordBLL == null) __materialDepositRecordBLL = new MaterialDepositRecordBLL();
            return __materialDepositRecordBLL;
        }
        private static IViewMaterialDepositRecordBLL __viewMaterialDepositRecordBLL;
        public static IViewMaterialDepositRecordBLL CreateViewMaterialDepositRecordBLL()
        {
            if (__viewMaterialDepositRecordBLL == null) __viewMaterialDepositRecordBLL = new ViewMaterialDepositRecordBLL();
            return __viewMaterialDepositRecordBLL;
        }
        private static IMaterialUserAccountBLL __materialUserAccountBLL;
        public static IMaterialUserAccountBLL CreateMaterialUserAccountBLL()
        {
            if (__materialUserAccountBLL == null) __materialUserAccountBLL = new MaterialUserAccountBLL();
            return __materialUserAccountBLL;
        }
        private static IViewMaterialUserAccountBLL __viewMaterialUserAccountBLL;
        public static IViewMaterialUserAccountBLL CreateViewMaterialUserAccountBLL()
        {
            if (__viewMaterialUserAccountBLL == null) __viewMaterialUserAccountBLL = new ViewMaterialUserAccountBLL();
            return __viewMaterialUserAccountBLL;
        }
        private static IMaterialInfoLogBLL __materialInfoLogBLL;
        public static IMaterialInfoLogBLL CreateMaterialInfoLogBLL()
        {
            if (__materialInfoLogBLL == null) __materialInfoLogBLL = new MaterialInfoLogBLL();
            return __materialInfoLogBLL;
        }
        private static IViewMaterialInfoLogBLL __viewMaterialInfoLogBLL;
        public static IViewMaterialInfoLogBLL CreateViewMaterialInfoLogBLL()
        {
            if (__viewMaterialInfoLogBLL == null) __viewMaterialInfoLogBLL = new ViewMaterialInfoLogBLL();
            return __viewMaterialInfoLogBLL;
        }
        #endregion

        #region 业务流水号
        private static IBusinessIdBLL __businessIdBLL;
        public static IBusinessIdBLL CreateBusinessIdBLL()
        {
            if (__businessIdBLL == null)
                __businessIdBLL = new BusinessIdBLL();
            return __businessIdBLL;
        }
        #endregion

        #region 设备考核评价

        private static IJudgeProjectBLL __judgeProjectBLL;
        public static IJudgeProjectBLL CreateJudgeProjectBLL()
        {
            if (__judgeProjectBLL == null)
                __judgeProjectBLL = new JudgeProjectBLL();
            return __judgeProjectBLL;
        }
        private static IViewJudgeProjectBLL __viewJudgeProjectBLL;
        public static IViewJudgeProjectBLL CreateViewJudgeProjectBLL()
        {
            if (__viewJudgeProjectBLL == null)
                __viewJudgeProjectBLL = new ViewJudgeProjectBLL();
            return __viewJudgeProjectBLL;
        }
        private static IJudgeProjectContentBLL __judgeProjectContentBLL;
        public static IJudgeProjectContentBLL CreateJudgeProjectContentBLL()
        {
            if (__judgeProjectContentBLL == null)
                __judgeProjectContentBLL = new JudgeProjectContentBLL();
            return __judgeProjectContentBLL;
        }
        private static IViewJudgeProjectContentBLL __viewJudgeProjectContentBLL;
        public static IViewJudgeProjectContentBLL CreateViewJudgeProjectContentBLL()
        {
            if (__viewJudgeProjectContentBLL == null)
                __viewJudgeProjectContentBLL = new ViewJudgeProjectContentBLL();
            return __viewJudgeProjectContentBLL;
        }
        private static IJudgeEquipmentRecordBLL __judgeEquipmentRecordBLL;
        public static IJudgeEquipmentRecordBLL CreateJudgeEquipmentRecordBLL()
        {
            if (__judgeEquipmentRecordBLL == null)
                __judgeEquipmentRecordBLL = new JudgeEquipmentRecordBLL();
            return __judgeEquipmentRecordBLL;
        }
        private static IViewJudgeEquipmentRecordBLL __viewJudgeEquipmentRecordBLL;
        public static IViewJudgeEquipmentRecordBLL CreateViewJudgeEquipmentRecordBLL()
        {
            if (__viewJudgeEquipmentRecordBLL == null)
                __viewJudgeEquipmentRecordBLL = new ViewJudgeEquipmentRecordBLL();
            return __viewJudgeEquipmentRecordBLL;
        }
        private static IJudgeProjectRecordBLL __judgeProjectRecordBLL;
        public static IJudgeProjectRecordBLL CreateJudgeProjectRecordBLL()
        {
            if (__judgeProjectRecordBLL == null)
                __judgeProjectRecordBLL = new JudgeProjectRecordBLL();
            return __judgeProjectRecordBLL;
        }
        private static IViewJudgeProjectRecordBLL __viewJudgeProjectRecordBLL;
        public static IViewJudgeProjectRecordBLL CreateViewJudgeProjectRecordBLL()
        {
            if (__viewJudgeProjectRecordBLL == null)
                __viewJudgeProjectRecordBLL = new ViewJudgeProjectRecordBLL();
            return __viewJudgeProjectRecordBLL;
        }
        private static IJudgeProjectContentRecordBLL __judgeProjectContentRecordBLL;
        public static IJudgeProjectContentRecordBLL CreateJudgeProjectContentRecordBLL()
        {
            if (__judgeProjectContentRecordBLL == null)
                __judgeProjectContentRecordBLL = new JudgeProjectContentRecordBLL();
            return __judgeProjectContentRecordBLL;
        }
        private static IViewJudgeProjectContentRecordBLL __viewJudgeProjectContentRecordBLL;
        public static IViewJudgeProjectContentRecordBLL CreateViewJudgeProjectContentRecordBLL()
        {
            if (__viewJudgeProjectContentRecordBLL == null)
                __viewJudgeProjectContentRecordBLL = new ViewJudgeProjectContentRecordBLL();
            return __viewJudgeProjectContentRecordBLL;
        }
        #endregion

        #region 系统运行设置
        private static ISystemRunSettingBLL __systemRunSettingBLL;
        public static ISystemRunSettingBLL CreateSystemRunSettingBLL()
        {
            if (__systemRunSettingBLL == null)
                __systemRunSettingBLL = new SystemRunSettingBLL();
            return __systemRunSettingBLL;
        }
        #endregion

        #region 实验动物管理
        private static IAnimalCategoryBLL __animalCategoryBLL;
        public static IAnimalCategoryBLL CreateAnimalCategoryBLL()
        {
            if (__animalCategoryBLL == null)
                __animalCategoryBLL = new AnimalCategoryBLL();
            return __animalCategoryBLL;
        }
        private static IAnimalBLL __animalBLL;
        public static IAnimalBLL CreateAnimalBLL()
        {
            if (__animalBLL == null)
                __animalBLL = new AnimalBLL();
            return __animalBLL;
        }
        private static IViewAnimalBLL __viewAnimalBLL;
        public static IViewAnimalBLL CreateViewAnimalBLL()
        {
            if (__viewAnimalBLL == null)
                __viewAnimalBLL = new ViewAnimalBLL();
            return __viewAnimalBLL;
        }
        private static IEthicsApplyBLL __ethicsApplyBLL;
        public static IEthicsApplyBLL CreateEthicsApplyBLL()
        {
            if (__ethicsApplyBLL == null)
                __ethicsApplyBLL = new EthicsApplyBLL();
            return __ethicsApplyBLL;
        }
        private static IViewEthicsApplyBLL __viewEthicsApplyBLL;
        public static IViewEthicsApplyBLL CreateViewEthicsApplyBLL()
        {
            if (__viewEthicsApplyBLL == null)
                __viewEthicsApplyBLL = new ViewEthicsApplyBLL();
            return __viewEthicsApplyBLL;
        }
        private static IEthicsApplyAnimalExperimenterBLL __ethicsApplyAnimalExperimenterBLL;
        public static IEthicsApplyAnimalExperimenterBLL CreateEthicsApplyAnimalExperimenterBLL()
        {
            if (__ethicsApplyAnimalExperimenterBLL == null)
                __ethicsApplyAnimalExperimenterBLL = new EthicsApplyAnimalExperimenterBLL();
            return __ethicsApplyAnimalExperimenterBLL;
        }
        private static IAnimalFrameBLL __animalFrameBLL;
        public static IAnimalFrameBLL CreateAnimalFrameBLL()
        {
            if (__animalFrameBLL == null)
                __animalFrameBLL = new AnimalFrameBLL();
            return __animalFrameBLL;
        }
        private static IViewAnimalFrameBLL __viewAnimalFrameBLL;
        public static IViewAnimalFrameBLL CreateViewAnimalFrameBLL()
        {
            if (__viewAnimalFrameBLL == null)
                __viewAnimalFrameBLL = new ViewAnimalFrameBLL();
            return __viewAnimalFrameBLL;
        }
        private static IAnimalCageBLL __animalCageBLL;
        public static IAnimalCageBLL CreateAnimalCageBLL()
        {
            if (__animalCageBLL == null)
                __animalCageBLL = new AnimalCageBLL();
            return __animalCageBLL;
        }
        private static IViewAnimalCageBLL __viewAnimalCageBLL;
        public static IViewAnimalCageBLL CreateViewAnimalCageBLL()
        {
            if (__viewAnimalCageBLL == null)
                __viewAnimalCageBLL = new ViewAnimalCageBLL();
            return __viewAnimalCageBLL;
        }
        private static IViewAnimalStoreLabRoomBLL __viewAnimalStoreLabRoomBLL;
        public static IViewAnimalStoreLabRoomBLL CreateViewAnimalStoreLabRoomBLL()
        {
            if (__viewAnimalStoreLabRoomBLL == null)
                __viewAnimalStoreLabRoomBLL = new ViewAnimalStoreLabRoomBLL();
            return __viewAnimalStoreLabRoomBLL;
        }
        private static IAnimalAppointmentBLL __animalAppointmentBLL;
        public static IAnimalAppointmentBLL CreateAnimalAppointmentBLL()
        {
            if (__animalAppointmentBLL == null)
                __animalAppointmentBLL = new AnimalAppointmentBLL();
            return __animalAppointmentBLL;
        }
        private static IViewAnimalAppointmentBLL __viewAnimalAppointmentBLL;
        public static IViewAnimalAppointmentBLL CreateViewAnimalAppointmentBLL()
        {
            if (__viewAnimalAppointmentBLL == null)
                __viewAnimalAppointmentBLL = new ViewAnimalAppointmentBLL();
            return __viewAnimalAppointmentBLL;
        }
        private static IAnimalOutAppointmentBLL __animalOutAppointmentBLL;
        public static IAnimalOutAppointmentBLL CreateAnimalOutAppointmentBLL()
        {
            if (__animalOutAppointmentBLL == null)
                __animalOutAppointmentBLL = new AnimalOutAppointmentBLL();
            return __animalOutAppointmentBLL;
        }
        private static IViewAnimalOutAppointmentBLL __viewAnimalOutAppointmentBLL;
        public static IViewAnimalOutAppointmentBLL CreateViewAnimalOutAppointmentBLL()
        {
            if (__viewAnimalOutAppointmentBLL == null)
                __viewAnimalOutAppointmentBLL = new ViewAnimalOutAppointmentBLL();
            return __viewAnimalOutAppointmentBLL;
        }
        private static IAnimalOutAppointmentDetailBLL __animalOutAppointmentDetailBLL;
        public static IAnimalOutAppointmentDetailBLL CreateAnimalOutAppointmentDetailBLL()
        {
            if (__animalOutAppointmentDetailBLL == null)
                __animalOutAppointmentDetailBLL = new AnimalOutAppointmentDetailBLL();
            return __animalOutAppointmentDetailBLL;
        }
        private static IEthicsApplyAnimalDataBLL __ethicsApplyAnimalDataBLL;
        public static IEthicsApplyAnimalDataBLL CreateEthicsApplyAnimalDataBLL()
        {
            if (__ethicsApplyAnimalDataBLL == null)
                __ethicsApplyAnimalDataBLL = new EthicsApplyAnimalDataBLL();
            return __ethicsApplyAnimalDataBLL;
        }
        private static IViewNeedCostDeductAnimalBLL __viewNeedCostDeductAnimalBLL;
        public static IViewNeedCostDeductAnimalBLL CreateViewNeedCostDeductAnimalBLL()
        {
            if (__viewNeedCostDeductAnimalBLL == null)
                __viewNeedCostDeductAnimalBLL = new ViewNeedCostDeductAnimalBLL();
            return __viewNeedCostDeductAnimalBLL;
        }
        #endregion

        #region 开放基金
        //private static IOpenFundApplyBLL __openFundApplyBLL;
        public static IOpenFundApplyBLL CreateOpenFundApplyBLL()
        {
            return CustomDiffHandlerManager.CreateOpenFundApplyBLL();
            //if (__openFundApplyBLL == null)
            //    __openFundApplyBLL = new OpenFundApplyBLL();
            //return __openFundApplyBLL;
        }
        private static IViewOpenFundApplyBLL __viewOpenFundApplyBLL;
        public static IViewOpenFundApplyBLL CreateViewOpenFundApplyBLL()
        {
            if (__viewOpenFundApplyBLL == null)
                __viewOpenFundApplyBLL = new ViewOpenFundApplyBLL();
            return __viewOpenFundApplyBLL;
        }
        private static IOpenFundApplyEquipmentBLL __openFundApplyEquipment;
        public static IOpenFundApplyEquipmentBLL CreateOpenFundApplyEquipmentBLL()
        {
            if (__openFundApplyEquipment == null)
                __openFundApplyEquipment = new OpenFundApplyEquipmentBLL();
            return __openFundApplyEquipment;
        }
        private static IViewOpenFundApplyEquipmentBLL __viewOpenFundApplyEquipmentBLL;
        public static IViewOpenFundApplyEquipmentBLL CreateViewOpenFundApplyEquipmentBLL()
        {
            if (__viewOpenFundApplyEquipmentBLL == null)
                __viewOpenFundApplyEquipmentBLL = new ViewOpenFundApplyEquipmentBLL();
            return __viewOpenFundApplyEquipmentBLL;
        }
        private static IOpenFundDepositRecordBLL __openFundDepositRecordBLL;
        public static IOpenFundDepositRecordBLL CreateOpenFundDepositRecordBLL()
        {
            if (__openFundDepositRecordBLL == null)
                __openFundDepositRecordBLL = new OpenFundDepositRecordBLL();
            return __openFundDepositRecordBLL;
        }
        private static IViewOpenFundDepositRecordBLL __viewOpenFundDepositRecordBLL;
        public static IViewOpenFundDepositRecordBLL CreateViewOpenFundDepositRecordBLL()
        {
            if (__viewOpenFundDepositRecordBLL == null)
                __viewOpenFundDepositRecordBLL = new ViewOpenFundDepositRecordBLL();
            return __viewOpenFundDepositRecordBLL;
        }
        private static IViewOpenFundDetailBLL __viewOpenFundDetailBLL;
        public static IViewOpenFundDetailBLL CreateViewOpenFundDetailBLL()
        {
            if (__viewOpenFundDetailBLL == null)
                __viewOpenFundDetailBLL = new ViewOpenFundDetailBLL();
            return __viewOpenFundDetailBLL;
        }

        private static IDepositRecordOpenFundBLL __depositRecordOpenFundBLL;
        public static IDepositRecordOpenFundBLL CreateDepositRecordOpenFundBLL()
        {
            if (__depositRecordOpenFundBLL == null)
                __depositRecordOpenFundBLL = new DepositRecordOpenFundBLL();
            return __depositRecordOpenFundBLL;
        }
        private static IViewDepositRecordOpenFundBLL __viewDepositRecordOpenFundBLL;
        public static IViewDepositRecordOpenFundBLL CreateViewDepositRecordOpenFundBLL()
        {
            if (__viewDepositRecordOpenFundBLL == null)
                __viewDepositRecordOpenFundBLL = new ViewDepositRecordOpenFundBLL();
            return __viewDepositRecordOpenFundBLL;
        }
        private static IDepositRecordOpenFundEquipmentBLL __depositRecordOpenFundEquipmentBLL;
        public static IDepositRecordOpenFundEquipmentBLL CreateDepositRecordOpenFundEquipmentBLL()
        {
            if (__depositRecordOpenFundEquipmentBLL == null)
                __depositRecordOpenFundEquipmentBLL = new DepositRecordOpenFundEquipmentBLL();
            return __depositRecordOpenFundEquipmentBLL;
        }
        private static IOpenFundApplyExcelBLL __openFundApplyExcelBLL;
        public static IOpenFundApplyExcelBLL CreateOpenFundApplyExcelBLL()
        {
            if (__openFundApplyExcelBLL == null)
                __openFundApplyExcelBLL = new OpenFundApplyExcelBLL();
            return __openFundApplyExcelBLL;
        }
        #endregion

        #region 设备警告
        private static IEquipmentAlarmBLL __equipmentAlarmBLL;
        public static IEquipmentAlarmBLL CreateEquipmentAlarmBLL()
        {
            if (__equipmentAlarmBLL == null)
                __equipmentAlarmBLL = new EquipmentAlarmBLL();
            return __equipmentAlarmBLL;
        }
        private static IViewEquipmentAlarmBLL __viewEquipmentAlarmBLL;
        public static IViewEquipmentAlarmBLL CreateViewEquipmentAlarmBLL()
        {
            if (__viewEquipmentAlarmBLL == null)
                __viewEquipmentAlarmBLL = new ViewEquipmentAlarmBLL();
            return __viewEquipmentAlarmBLL;
        }
        #endregion

        #region 上报信息统计
        private static ISJ1StatisticsBLL __sJ1StatisticsBLL;
        public static ISJ1StatisticsBLL CreateSJ1StatisticsBLL()
        {
            if (__sJ1StatisticsBLL == null)
                __sJ1StatisticsBLL = new SJ1StatisticsBLL();
            return __sJ1StatisticsBLL;
        }
        private static IViewSJ1StatisticsBLL __viewSJ1StatisticsBLL;
        public static IViewSJ1StatisticsBLL CreateViewSJ1StatisticsBLL()
        {
            if (__viewSJ1StatisticsBLL == null)
                __viewSJ1StatisticsBLL = new ViewSJ1StatisticsBLL();
            return __viewSJ1StatisticsBLL;
        }
        private static ISJ1BLL __sJ1BLL;
        public static ISJ1BLL CreateSJ1BLL()
        {
            if (__sJ1BLL == null)
                __sJ1BLL = new SJ1BLL();
            return __sJ1BLL;
        }
        private static IViewSJ1BLL __viewSJ1BLL;
        public static IViewSJ1BLL CreateViewSJ1BLL()
        {
            if (__viewSJ1BLL == null)
                __viewSJ1BLL = new ViewSJ1BLL();
            return __viewSJ1BLL;
        }
        private static ISJ2StatisticsBLL __sJ2StatisticsBLL;
        public static ISJ2StatisticsBLL CreateSJ2StatisticsBLL()
        {
            if (__sJ2StatisticsBLL == null)
                __sJ2StatisticsBLL = new SJ2StatisticsBLL();
            return __sJ2StatisticsBLL;
        }
        private static IViewSJ2StatisticsBLL __viewSJ2StatisticsBLL;
        public static IViewSJ2StatisticsBLL CreateViewSJ2StatisticsBLL()
        {
            if (__viewSJ2StatisticsBLL == null)
                __viewSJ2StatisticsBLL = new ViewSJ2StatisticsBLL();
            return __viewSJ2StatisticsBLL;
        }
        private static ISJ2BLL __sJ2BLL;
        public static ISJ2BLL CreateSJ2BLL()
        {
            if (__sJ2BLL == null)
                __sJ2BLL = new SJ2BLL();
            return __sJ2BLL;
        }
        private static IViewSJ2BLL __viewSJ2BLL;
        public static IViewSJ2BLL CreateViewSJ2BLL()
        {
            if (__viewSJ2BLL == null)
                __viewSJ2BLL = new ViewSJ2BLL();
            return __viewSJ2BLL;
        }
        private static ISJ3StatisticsBLL __sJ3StatisticsBLL;
        public static ISJ3StatisticsBLL CreateSJ3StatisticsBLL()
        {
            if (__sJ3StatisticsBLL == null)
                __sJ3StatisticsBLL = new SJ3StatisticsBLL();
            return __sJ3StatisticsBLL;
        }
        private static IViewSJ3StatisticsBLL __viewSJ3StatisticsBLL;
        public static IViewSJ3StatisticsBLL CreateViewSJ3StatisticsBLL()
        {
            if (__viewSJ3StatisticsBLL == null)
                __viewSJ3StatisticsBLL = new ViewSJ3StatisticsBLL();
            return __viewSJ3StatisticsBLL;
        }
        private static ISJ3StatisticsUsedHourBLL __sJ3StatisticsUsedHourBLL;
        public static ISJ3StatisticsUsedHourBLL CreateSJ3StatisticsUsedHourBLL()
        {
            if (__sJ3StatisticsUsedHourBLL == null)
                __sJ3StatisticsUsedHourBLL = new SJ3StatisticsUsedHourBLL();
            return __sJ3StatisticsUsedHourBLL;
        }
        private static ISJ3BLL __sJ3BLL;
        public static ISJ3BLL CreateSJ3BLL()
        {
            if (__sJ3BLL == null)
                __sJ3BLL = new SJ3BLL();
            return __sJ3BLL;
        }
        private static IViewSJ3BLL __viewSJ3BLL;
        public static IViewSJ3BLL CreateViewSJ3BLL()
        {
            if (__viewSJ3BLL == null)
                __viewSJ3BLL = new ViewSJ3BLL();
            return __viewSJ3BLL;
        }
        private static ISJ4StatisticsBLL __sJ4StatisticsBLL;
        public static ISJ4StatisticsBLL CreateSJ4StatisticsBLL()
        {
            if (__sJ4StatisticsBLL == null)
                __sJ4StatisticsBLL = new SJ4StatisticsBLL();
            return __sJ4StatisticsBLL;
        }
        private static IViewSJ4StatisticsBLL __viewSJ4StatisticsBLL;
        public static IViewSJ4StatisticsBLL CreateViewSJ4StatisticsBLL()
        {
            if (__viewSJ4StatisticsBLL == null)
                __viewSJ4StatisticsBLL = new ViewSJ4StatisticsBLL();
            return __viewSJ4StatisticsBLL;
        }
        private static ISJ4BLL __sJ4BLL;
        public static ISJ4BLL CreateSJ4BLL()
        {
            if (__sJ4BLL == null)
                __sJ4BLL = new SJ4BLL();
            return __sJ4BLL;
        }
        private static IViewSJ4BLL __viewSJ4BLL;
        public static IViewSJ4BLL CreateViewSJ4BLL()
        {
            if (__viewSJ4BLL == null)
                __viewSJ4BLL = new ViewSJ4BLL();
            return __viewSJ4BLL;
        }
        private static ISJ5StatisticsBLL __sJ5StatisticsBLL;
        public static ISJ5StatisticsBLL CreateSJ5StatisticsBLL()
        {
            if (__sJ5StatisticsBLL == null)
                __sJ5StatisticsBLL = new SJ5StatisticsBLL();
            return __sJ5StatisticsBLL;
        }
        private static IViewSJ5StatisticsBLL __viewSJ5StatisticsBLL;
        public static IViewSJ5StatisticsBLL CreateViewSJ5StatisticsBLL()
        {
            if (__viewSJ5StatisticsBLL == null)
                __viewSJ5StatisticsBLL = new ViewSJ5StatisticsBLL();
            return __viewSJ5StatisticsBLL;
        }
        private static ISJ5BLL __sJ5BLL;
        public static ISJ5BLL CreateSJ5BLL()
        {
            if (__sJ5BLL == null)
                __sJ5BLL = new SJ5BLL();
            return __sJ5BLL;
        }
        private static IViewSJ5BLL __viewSJ5BLL;
        public static IViewSJ5BLL CreateViewSJ5BLL()
        {
            if (__viewSJ5BLL == null)
                __viewSJ5BLL = new ViewSJ5BLL();
            return __viewSJ5BLL;
        }
        private static ISJ6StatisticsBLL __sJ6StatisticsBLL;
        public static ISJ6StatisticsBLL CreateSJ6StatisticsBLL()
        {
            if (__sJ6StatisticsBLL == null)
                __sJ6StatisticsBLL = new SJ6StatisticsBLL();
            return __sJ6StatisticsBLL;
        }
        private static IViewSJ6StatisticsBLL __viewSJ6StatisticsBLL;
        public static IViewSJ6StatisticsBLL CreateViewSJ6StatisticsBLL()
        {
            if (__viewSJ6StatisticsBLL == null)
                __viewSJ6StatisticsBLL = new ViewSJ6StatisticsBLL();
            return __viewSJ6StatisticsBLL;
        }
        private static ISJ6BLL __sJ6BLL;
        public static ISJ6BLL CreateSJ6BLL()
        {
            if (__sJ6BLL == null)
                __sJ6BLL = new SJ6BLL();
            return __sJ6BLL;
        }
        private static IViewSJ6BLL __viewSJ6BLL;
        public static IViewSJ6BLL CreateViewSJ6BLL()
        {
            if (__viewSJ6BLL == null)
                __viewSJ6BLL = new ViewSJ6BLL();
            return __viewSJ6BLL;
        }
        private static ISJ7StatisticsBLL __sJ7StatisticsBLL;
        public static ISJ7StatisticsBLL CreateSJ7StatisticsBLL()
        {
            if (__sJ7StatisticsBLL == null)
                __sJ7StatisticsBLL = new SJ7StatisticsBLL();
            return __sJ7StatisticsBLL;
        }
        private static IViewSJ7StatisticsBLL __viewSJ7StatisticsBLL;
        public static IViewSJ7StatisticsBLL CreateViewSJ7StatisticsBLL()
        {
            if (__viewSJ7StatisticsBLL == null)
                __viewSJ7StatisticsBLL = new ViewSJ7StatisticsBLL();
            return __viewSJ7StatisticsBLL;
        }
        private static ISJ7BLL __sJ7BLL;
        public static ISJ7BLL CreateSJ7BLL()
        {
            if (__sJ7BLL == null)
                __sJ7BLL = new SJ7BLL();
            return __sJ7BLL;
        }
        private static IViewSJ7BLL __viewSJ7BLL;
        public static IViewSJ7BLL CreateViewSJ7BLL()
        {
            if (__viewSJ7BLL == null)
                __viewSJ7BLL = new ViewSJ7BLL();
            return __viewSJ7BLL;
        }
        #endregion

        #region 设备学期费用
        private static ISemesterBLL __semesterBLL;
        public static ISemesterBLL CreateSemesterBLL()
        {
            if (__semesterBLL == null)
                __semesterBLL = new SemesterBLL();
            return __semesterBLL;
        }
        private static IEquipmentSemesterCostBLL __equipmentSemesterCostBLL;
        public static IEquipmentSemesterCostBLL CreateEquipmentSemesterCostBLL()
        {
            if (__equipmentSemesterCostBLL == null)
                __equipmentSemesterCostBLL = new EquipmentSemesterCostBLL();
            return __equipmentSemesterCostBLL;
        }
        private static IViewEquipmentSemesterCostBLL __viewEquipmentSemesterCostBLL;
        public static IViewEquipmentSemesterCostBLL CreateViewEquipmentSemesterCostBLL()
        {
            if (__viewEquipmentSemesterCostBLL == null)
                __viewEquipmentSemesterCostBLL = new ViewEquipmentSemesterCostBLL();
            return __viewEquipmentSemesterCostBLL;
        }
        #endregion

        #region 外出及特殊业务活动申请及报销流程
        private static IActivityTypeBLL __activityTypeBLL;
        public static IActivityTypeBLL CreateActivityTypeBLL()
        {
            if (__activityTypeBLL == null)
                __activityTypeBLL = new ActivityTypeBLL();
            return __activityTypeBLL;
        }
        private static IActivityApplyBLL __activityApplyBLL;
        public static IActivityApplyBLL CreateActivityApplyBLL()
        {
            if (__activityApplyBLL == null)
                __activityApplyBLL = new ActivityApplyBLL();
            return __activityApplyBLL;
        }
        private static IViewActivityApplyBLL __viewActivityApplyBLL;
        public static IViewActivityApplyBLL CreateViewActivityApplyBLL()
        {
            if (__viewActivityApplyBLL == null)
                __viewActivityApplyBLL = new ViewActivityApplyBLL();
            return __viewActivityApplyBLL;
        }
        #endregion

        #region 获取二维码打印设置
        private static IQrCodePrintSettingsBLL __qrCodePrintSettingsBLL;
        public static IQrCodePrintSettingsBLL CreateQrCodePrintSettingsBLL()
        {
            if (__qrCodePrintSettingsBLL == null)
                __qrCodePrintSettingsBLL = new QrCodePrintSettingsBLL();
            return __qrCodePrintSettingsBLL;
        }
        #endregion

        #region  短信发送
        private static ISendShortMailBLL _sendShortMailBLL;
        public static ISendShortMailBLL CreateSendShortMailBLL()
        {
            if (_sendShortMailBLL == null)
                _sendShortMailBLL = new SendShortMailBLL();
            return _sendShortMailBLL;
        }
        private static IViewSendShortMailBLL _viewSendShortMailBLL;
        public static IViewSendShortMailBLL CreateViewSendShortMailBLL()
        {
            if (_viewSendShortMailBLL == null)
                _viewSendShortMailBLL = new ViewSendShortMailBLL();
            return _viewSendShortMailBLL;
        }
        #endregion

        #region  CERS
        private static ICersPlatformBLL _cersPlatformBLL;
        public static ICersPlatformBLL CreateCersPlatformBLL()
        {
            if (_cersPlatformBLL == null)
                _cersPlatformBLL = new CersPlatformBLL();
            return _cersPlatformBLL;
        }

        private static IViewEquipmentExtendBLL _viewequipmentExtendBLL;
        public static IViewEquipmentExtendBLL CreateViewEquipmentExtendBLL()
        {
            if (_viewequipmentExtendBLL == null)
                _viewequipmentExtendBLL = new ViewEquipmentExtendBLL();
            return _viewequipmentExtendBLL;
        }

        private static IViewEquipmentExtendUploadBLL _viewequipmentExtendUploadBLL;
        public static IViewEquipmentExtendUploadBLL CreateViewEquipmentExtendUploadBLL()
        {
            if (_viewequipmentExtendUploadBLL == null)
                _viewequipmentExtendUploadBLL = new ViewEquipmentExtendUploadBLL();
            return _viewequipmentExtendUploadBLL;
        }

        private static IEquipmentExtendBLL __equipmentExtendBLL;
        public static IEquipmentExtendBLL CreateEquipmentExtendBLL()
        {
            if (__equipmentExtendBLL == null)
                __equipmentExtendBLL = new EquipmentExtendBLL();
            return __equipmentExtendBLL;
        }

        private static IEquipmentCategoryShareBLL __equipmentCategoryShareBLL;
        public static IEquipmentCategoryShareBLL CreateEquipmentCategoryShareBLL()
        {
            if (__equipmentCategoryShareBLL == null)
                __equipmentCategoryShareBLL = new EquipmentCategoryShareBLL();
            return __equipmentCategoryShareBLL;
        }

        private static IEquipmentCustomsBLL __equipmentCustomsBLL;
        public static IEquipmentCustomsBLL CreateEquipmentCustomsBLL()
        {
            if (__equipmentCustomsBLL == null)
                __equipmentCustomsBLL = new EquipmentCustomsBLL();
            return __equipmentCustomsBLL;
        }

        private static IEquipmentCustomsBindBLL __equipmentCustomsBindBLL;
        public static IEquipmentCustomsBindBLL CreateEquipmentCustomsBindBLL()
        {
            if (__equipmentCustomsBindBLL == null)
                __equipmentCustomsBindBLL = new EquipmentCustomsBindBLL();
            return __equipmentCustomsBindBLL;
        }

        private static IViewEquipmentCustomsBLL __viewEquipmentCustomsBLL;
        public static IViewEquipmentCustomsBLL CreateViewEquipmentCustomsBLL()
        {
            if (__viewEquipmentCustomsBLL == null)
                __viewEquipmentCustomsBLL = new ViewEquipmentCustomsBLL();
            return __viewEquipmentCustomsBLL;
        }

        private static IImportCersEquipmnetBLL __importCersEquipmentBLL;
        public static IImportCersEquipmnetBLL CreateImportCersEquipmnetBLL()
        {
            if (__importCersEquipmentBLL == null)
                __importCersEquipmentBLL = new ImportCersEquipmnetBLL();
            return __importCersEquipmentBLL;
        }


        #endregion

        #region  温湿度监控
        private static IViewTemperatureHumiditySetupBLL _viewTemperatureHumiditySetupBLL;
        public static IViewTemperatureHumiditySetupBLL CreateViewTemperatureHumiditySetupBLL()
        {
            if (_viewTemperatureHumiditySetupBLL == null)
                _viewTemperatureHumiditySetupBLL = new ViewTemperatureHumiditySetupBLL();
            return _viewTemperatureHumiditySetupBLL;
        }
        private static ITemperatureHumiditySetupBLL _TemperatureHumiditySetupBLL;
        public static ITemperatureHumiditySetupBLL CreateTemperatureHumiditySetupBLL()
        {
            if (_TemperatureHumiditySetupBLL == null)
                _TemperatureHumiditySetupBLL = new TemperatureHumiditySetupBLL();
            return _TemperatureHumiditySetupBLL;
        }

        private static IEquipmentTemperatureHumidityBLL _EquipmentTemperatureHumidityBLL;
        public static IEquipmentTemperatureHumidityBLL CreateEquipmentTemperatureHumidityBLL()
        {
            if (_EquipmentTemperatureHumidityBLL == null)
                _EquipmentTemperatureHumidityBLL = new EquipmentTemperatureHumidityBLL();
            return _EquipmentTemperatureHumidityBLL;
        }

        #endregion

        #region 系统日志
        private static ISystemLogBLL __systemLogBLL;
        public static ISystemLogBLL CreateSystemLogBLL()
        {
            if (__systemLogBLL == null)
                __systemLogBLL = new SystemLogBLL();
            return __systemLogBLL;
        }
        private static IViewSystemLogBLL __viewSystemLogBLL;
        public static IViewSystemLogBLL CreateViewSystemLogBLL()
        {
            if (__viewSystemLogBLL == null)
                __viewSystemLogBLL = new ViewSystemLogBLL();
            return __viewSystemLogBLL;
        }
        #endregion

        #region 用户消息设置
        private static IUserMessageNoticeSettingBLL __userMessageNoticeSettingBLL;
        public static IUserMessageNoticeSettingBLL CreateUserMessageNoticeSettingBLL()
        {
            if (__userMessageNoticeSettingBLL == null)
                __userMessageNoticeSettingBLL = new UserMessageNoticeSettingBLL();
            return __userMessageNoticeSettingBLL;
        }
        private static IUserUnReceiveMessageTypeBLL __userUnReceiveMessageTypeBLL;
        public static IUserUnReceiveMessageTypeBLL CreateUserUnReceiveMessageTypeBLL()
        {
            if (__userUnReceiveMessageTypeBLL == null)
                __userUnReceiveMessageTypeBLL = new UserUnReceiveMessageTypeBLL();
            return __userUnReceiveMessageTypeBLL;
        }
        #endregion

        #region 面向本科生开放
        private static IEquipmentUndergraduateOpenBLL __equipmentUndergraduateOpenBLL;
        public static IEquipmentUndergraduateOpenBLL CreateEquipmentUndergraduateOpenBLL()
        {
            if (__equipmentUndergraduateOpenBLL == null)
                __equipmentUndergraduateOpenBLL = new EquipmentUndergraduateOpenBLL();
            return __equipmentUndergraduateOpenBLL;
        }
        private static IViewEquipmentUndergraduateOpenBLL __viewEquipmentUndergraduateOpenBLL;
        public static IViewEquipmentUndergraduateOpenBLL CreateViewEquipmentUndergraduateOpenBLL()
        {
            if (__viewEquipmentUndergraduateOpenBLL == null)
                __viewEquipmentUndergraduateOpenBLL = new ViewEquipmentUndergraduateOpenBLL();
            return __viewEquipmentUndergraduateOpenBLL;
        }
        private static IEquipmentOpenTrainingBLL __equipmentOpenTrainingBLL;
        public static IEquipmentOpenTrainingBLL CreateEquipmentOpenTrainingBLL()
        {
            if (__equipmentOpenTrainingBLL == null)
                __equipmentOpenTrainingBLL = new EquipmentOpenTrainingBLL();
            return __equipmentOpenTrainingBLL;
        }
        private static IEquipmentOpenTrainingPlanBLL __equipmentOpenTrainingPlanBLL;
        public static IEquipmentOpenTrainingPlanBLL CreateEquipmentOpenTrainingPlanBLL()
        {
            if (__equipmentOpenTrainingPlanBLL == null)
                __equipmentOpenTrainingPlanBLL = new EquipmentOpenTrainingPlanBLL();
            return __equipmentOpenTrainingPlanBLL;
        }
        private static IEquipmentOpenTrainingCarryOutBLL __equipmentOpenTrainingCarryOutBLL;
        public static IEquipmentOpenTrainingCarryOutBLL CreateEquipmentOpenTrainingCarryOutBLL()
        {
            if (__equipmentOpenTrainingCarryOutBLL == null)
                __equipmentOpenTrainingCarryOutBLL = new EquipmentOpenTrainingCarryOutBLL();
            return __equipmentOpenTrainingCarryOutBLL;
        }
        private static IEquipmentOpenTrainingCarryOutPlanBLL __equipmentOpenTrainingCarryOutPlanBLL;
        public static IEquipmentOpenTrainingCarryOutPlanBLL CreateEquipmentOpenTrainingCarryOutPlanBLL()
        {
            if (__equipmentOpenTrainingCarryOutPlanBLL == null)
                __equipmentOpenTrainingCarryOutPlanBLL = new EquipmentOpenTrainingCarryOutPlanBLL();
            return __equipmentOpenTrainingCarryOutPlanBLL;
        }
        private static IViewEquipmentOpenTrainingBLL __viewEquipmentOpenTrainingBLL;
        public static IViewEquipmentOpenTrainingBLL CreateViewEquipmentOpenTrainingBLL()
        {
            if (__viewEquipmentOpenTrainingBLL == null)
                __viewEquipmentOpenTrainingBLL = new ViewEquipmentOpenTrainingBLL();
            return __viewEquipmentOpenTrainingBLL;
        }
        private static IViewEquipmentOpenTrainingCarryOutBLL __viewEquipmentOpenTrainingCarryOutBLL;
        public static IViewEquipmentOpenTrainingCarryOutBLL CreateViewEquipmentOpenTrainingCarryOutBLL()
        {
            if (__viewEquipmentOpenTrainingCarryOutBLL == null)
                __viewEquipmentOpenTrainingCarryOutBLL = new ViewEquipmentOpenTrainingCarryOutBLL();
            return __viewEquipmentOpenTrainingCarryOutBLL;
        }
        private static IEquipmentOpenTrainingCarryOutSignUpBLL __equipmentOpenTrainingCarryOutSignUpBLL;
        public static IEquipmentOpenTrainingCarryOutSignUpBLL CreateEquipmentOpenTrainingCarryOutSignUpBLL()
        {
            if (__equipmentOpenTrainingCarryOutSignUpBLL == null)
                __equipmentOpenTrainingCarryOutSignUpBLL = new EquipmentOpenTrainingCarryOutSignUpBLL();
            return __equipmentOpenTrainingCarryOutSignUpBLL;
        }
        private static IViewEquipmentOpenTrainingCarryOutSignUpBLL __viewEquipmentOpenTrainingCarryOutSignUpBLL;
        public static IViewEquipmentOpenTrainingCarryOutSignUpBLL CreateViewEquipmentOpenTrainingCarryOutSignUpBLL()
        {
            if (__viewEquipmentOpenTrainingCarryOutSignUpBLL == null)
                __viewEquipmentOpenTrainingCarryOutSignUpBLL = new ViewEquipmentOpenTrainingCarryOutSignUpBLL();
            return __viewEquipmentOpenTrainingCarryOutSignUpBLL;
        }
        private static IEquipmentOpenPracticeBLL __equipmentOpenPracticeBLL;
        public static IEquipmentOpenPracticeBLL CreateEquipmentOpenPracticeBLL()
        {
            if (__equipmentOpenPracticeBLL == null)
                __equipmentOpenPracticeBLL = new EquipmentOpenPracticeBLL();
            return __equipmentOpenPracticeBLL;
        }
        private static IViewEquipmentOpenPracticeBLL __viewEquipmentOpenPracticeBLL;
        public static IViewEquipmentOpenPracticeBLL CreateViewEquipmentOpenPracticeBLL()
        {
            if (__viewEquipmentOpenPracticeBLL == null)
                __viewEquipmentOpenPracticeBLL = new ViewEquipmentOpenPracticeBLL();
            return __viewEquipmentOpenPracticeBLL;
        }
        private static IEquipmentOpenPracticeEquipmentBLL __equipmentOpenPracticeEquipmentBLL;
        public static IEquipmentOpenPracticeEquipmentBLL CreateEquipmentOpenPracticeEquipmentBLL()
        {
            if (__equipmentOpenPracticeEquipmentBLL == null)
                __equipmentOpenPracticeEquipmentBLL = new EquipmentOpenPracticeEquipmentBLL();
            return __equipmentOpenPracticeEquipmentBLL;
        }
        private static IViewEquipmentOpenPracticeEquipmentBLL __viewEquipmentOpenPracticeEquipmentBLL;
        public static IViewEquipmentOpenPracticeEquipmentBLL CreateViewEquipmentOpenPracticeEquipmentBLL()
        {
            if (__viewEquipmentOpenPracticeEquipmentBLL == null)
                __viewEquipmentOpenPracticeEquipmentBLL = new ViewEquipmentOpenPracticeEquipmentBLL();
            return __viewEquipmentOpenPracticeEquipmentBLL;
        }
        private static IEquipmentOpenPracticeMaterialBLL __equipmentOpenPracticeMaterialBLL;
        public static IEquipmentOpenPracticeMaterialBLL CreateEquipmentOpenPracticeMaterialBLL()
        {
            if (__equipmentOpenPracticeMaterialBLL == null)
                __equipmentOpenPracticeMaterialBLL = new EquipmentOpenPracticeMaterialBLL();
            return __equipmentOpenPracticeMaterialBLL;
        }
        private static IEquipmentOpenPracticeMemberBLL __equipmentOpenPracticeMemberBLL;
        public static IEquipmentOpenPracticeMemberBLL CreateEquipmentOpenPracticeMemberBLL()
        {
            if (__equipmentOpenPracticeMemberBLL == null)
                __equipmentOpenPracticeMemberBLL = new EquipmentOpenPracticeMemberBLL();
            return __equipmentOpenPracticeMemberBLL;
        }
        private static IViewEquipmentOpenPracticeMemberBLL __viewEquipmentOpenPracticeMemberBLL;
        public static IViewEquipmentOpenPracticeMemberBLL CreateViewEquipmentOpenPracticeMemberBLL()
        {
            if (__viewEquipmentOpenPracticeMemberBLL == null)
                __viewEquipmentOpenPracticeMemberBLL = new ViewEquipmentOpenPracticeMemberBLL();
            return __viewEquipmentOpenPracticeMemberBLL;
        }
        private static EquipmentOpenPracticeExperienceBLL __equipmentOpenPracticeExperienceBLL;
        public static EquipmentOpenPracticeExperienceBLL CreateEquipmentOpenPracticeExperienceBLL()
        {
            if (__equipmentOpenPracticeExperienceBLL == null)
                __equipmentOpenPracticeExperienceBLL = new EquipmentOpenPracticeExperienceBLL();
            return __equipmentOpenPracticeExperienceBLL;
        }
        private static IEquipmentOpenPracticeTeacherBLL __equipmentOpenPracticeTeacherBLL;
        public static IEquipmentOpenPracticeTeacherBLL CreateEquipmentOpenPracticeTeacherBLL()
        {
            if (__equipmentOpenPracticeTeacherBLL == null)
                __equipmentOpenPracticeTeacherBLL = new EquipmentOpenPracticeTeacherBLL();
            return __equipmentOpenPracticeTeacherBLL;
        }
        private static IViewEquipmentOpenPracticeTeacherBLL __viewEquipmentOpenPracticeTeacherBLL;
        public static IViewEquipmentOpenPracticeTeacherBLL CreateViewEquipmentOpenPracticeTeacherBLL()
        {
            if (__viewEquipmentOpenPracticeTeacherBLL == null)
                __viewEquipmentOpenPracticeTeacherBLL = new ViewEquipmentOpenPracticeTeacherBLL();
            return __viewEquipmentOpenPracticeTeacherBLL;
        }
        private static IEquipmentOpenPracticeRelationSubjectBLL __equipmentOpenPracticeRelationSubjectBLL;
        public static IEquipmentOpenPracticeRelationSubjectBLL CreateEquipmentOpenPracticeRelationSubjectBLL()
        {
            if (__equipmentOpenPracticeRelationSubjectBLL == null)
                __equipmentOpenPracticeRelationSubjectBLL = new EquipmentOpenPracticeRelationSubjectBLL();
            return __equipmentOpenPracticeRelationSubjectBLL;
        }
        private static IEquipmentOpenPracticeRelationOpenFundApplyBLL __equipmentOpenPracticeRelationOpenFundApplyBLL;
        public static IEquipmentOpenPracticeRelationOpenFundApplyBLL CreateEquipmentOpenPracticeRelationOpenFundApplyBLL()
        {
            if (__equipmentOpenPracticeRelationOpenFundApplyBLL == null)
                __equipmentOpenPracticeRelationOpenFundApplyBLL = new EquipmentOpenPracticeRelationOpenFundApplyBLL();
            return __equipmentOpenPracticeRelationOpenFundApplyBLL;
        }
        #endregion

        #region 水控管理
        private static IViewWaterControlCostBLL __viewWaterControlCostBLL;
        public static IViewWaterControlCostBLL CreateViewWaterControlCostBLL()
        {
            if (__viewWaterControlCostBLL == null)
                __viewWaterControlCostBLL = new ViewWaterControlCostBLL();
            return __viewWaterControlCostBLL;
        }
        #endregion

        #region 楼宇管理
        private static IBuildingBLL __buildingBLL;
        public static IBuildingBLL CreateBuildingBLL()
        {
            if (__buildingBLL == null)
                __buildingBLL = new BuildingBLL();
            return __buildingBLL;
        }
        private static IBuildingEquipmentBLL __buildingEquipmentBLL;
        public static IBuildingEquipmentBLL CreateBuildingEquipmentBLL()
        {
            if (__buildingEquipmentBLL == null)
                __buildingEquipmentBLL = new BuildingEquipmentBLL();
            return __buildingEquipmentBLL;
        }
        private static IViewBuildingEquipmentBLL __viewBuildingEquipmentBLL;
        public static IViewBuildingEquipmentBLL CreateViewBuildingEquipmentBLL()
        {
            if (__viewBuildingEquipmentBLL == null)
                __viewBuildingEquipmentBLL = new ViewBuildingEquipmentBLL();
            return __viewBuildingEquipmentBLL;
        }
        #endregion

        #region 共享基金
        private static IShareFundApplyBLL __shareFundApplyBLL;
        public static IShareFundApplyBLL CreateShareFundApplyBLL()
        {
            if (__shareFundApplyBLL == null)
                __shareFundApplyBLL = new ShareFundApplyBLL();
            return __shareFundApplyBLL;
        }
        private static IViewShareFundApplyBLL __viewShareFundApplyBLL;
        public static IViewShareFundApplyBLL CreateViewShareFundApplyBLL()
        {
            if (__viewShareFundApplyBLL == null)
                __viewShareFundApplyBLL = new ViewShareFundApplyBLL();
            return __viewShareFundApplyBLL;
        }
        private static IShareFundApplyEquipmentBLL __shareFundApplyEquipmentBLL;
        public static IShareFundApplyEquipmentBLL CreateShareFundApplyEquipmentBLL()
        {
            if (__shareFundApplyEquipmentBLL == null)
                __shareFundApplyEquipmentBLL = new ShareFundApplyEquipmentBLL();
            return __shareFundApplyEquipmentBLL;
        }
        private static IViewShareFundApplyEquipmentBLL __viewShareFundApplyEquipmentBLL;
        public static IViewShareFundApplyEquipmentBLL CreateViewShareFundApplyEquipmentBLL()
        {
            if (__viewShareFundApplyEquipmentBLL == null)
                __viewShareFundApplyEquipmentBLL = new ViewShareFundApplyEquipmentBLL();
            return __viewShareFundApplyEquipmentBLL;
        }
        #endregion

        #region 工位预约
        private static INMPEquipmentBLL __nmpEquipmentBLL;
        public static INMPEquipmentBLL CreateNMPEquipmentBLL()
        {
            if (__nmpEquipmentBLL == null) __nmpEquipmentBLL = new NMPEquipmentBLL();
            return __nmpEquipmentBLL;
        }
        private static IViewNMPBLL __viewNMPBLL;
        public static IViewNMPBLL CreateViewNMPBLL()
        {
            if (__viewNMPBLL == null) __viewNMPBLL = new ViewNMPBLL();
            return __viewNMPBLL;
        }
        public static INMPBLL __nmpBLL;
        public static INMPBLL CreateNMPBLL()
        {
            if (__nmpBLL == null) __nmpBLL = new NMPBLL();
            return __nmpBLL;
        }
        private static IViewNMPEquipmentBLL __viewNMPEquipmentBLL;
        public static IViewNMPEquipmentBLL CreateViewNMPEquipmentBLL()
        {
            if (__viewNMPEquipmentBLL == null) __viewNMPEquipmentBLL = new ViewNMPEquipmentBLL();
            return __viewNMPEquipmentBLL;
        }
        private static INMPLabelBLL __nmpLabelBLL;
        public static INMPLabelBLL CreateNMPLabelBLL()
        {
            if (__nmpLabelBLL == null) __nmpLabelBLL = new NMPLabelBLL();
            return __nmpLabelBLL;
        }
        private static INMPLabelItemBLL __nmpLabelItemBLL;
        public static INMPLabelItemBLL CreateNMPLabelItemBLL()
        {
            if (__nmpLabelItemBLL == null) __nmpLabelItemBLL = new NMPLabelItemBLL();
            return __nmpLabelItemBLL;
        }
        private static INMPLabelChargeStandardBLL __nmpLabelChargeStandardBLL;
        public static INMPLabelChargeStandardBLL CreateNMPLabelChargeStandardBLL()
        {
            if (__nmpLabelChargeStandardBLL == null) __nmpLabelChargeStandardBLL = new NMPLabelChargeStandardBLL();
            return __nmpLabelChargeStandardBLL;
        }
        private static INMPAdditionChargeItemBLL __nmpAdditionChargeItemBLL;
        public static INMPAdditionChargeItemBLL CreateNMPAdditionChargeItemBLL()
        {
            if (__nmpAdditionChargeItemBLL == null) __nmpAdditionChargeItemBLL = new NMPAdditionChargeItemBLL();
            return __nmpAdditionChargeItemBLL;
        }
        private static INMPLabelAddtionChargeItemBLL __nmpLabelAddtionChargeItemBLL;
        public static INMPLabelAddtionChargeItemBLL CreateNMPLabelAddtionChargeItemBLL()
        {
            if (__nmpLabelAddtionChargeItemBLL == null) __nmpLabelAddtionChargeItemBLL = new NMPLabelAddtionChargeItemBLL();
            return __nmpLabelAddtionChargeItemBLL;
        }
        private static INMPCalcFeeTimeRuleBLL __nmpCalcFeeTimeRuleBLL;
        public static INMPCalcFeeTimeRuleBLL CreateNMPCalcFeeTimeRuleBLL()
        {
            if (__nmpCalcFeeTimeRuleBLL == null) __nmpCalcFeeTimeRuleBLL = new NMPCalcFeeTimeRuleBLL();
            return __nmpCalcFeeTimeRuleBLL;
        }
        private static INMPChargeStandardBLL __NMPChargeStandardBLL;
        public static INMPChargeStandardBLL CreateNMPChargeStandardBLL()
        {
            if (__NMPChargeStandardBLL == null) __NMPChargeStandardBLL = new NMPChargeStandardBLL();
            return __NMPChargeStandardBLL;
        }
        private static INMPAppointmentPriorityBLL __nmpAppointmentPriorityBLL;
        public static INMPAppointmentPriorityBLL CreateNMPAppointmentPriorityBLL()
        {
            if (__nmpAppointmentPriorityBLL == null) __nmpAppointmentPriorityBLL = new NMPAppointmentPriorityBLL();
            return __nmpAppointmentPriorityBLL;
        }
        private static INMPAppointmentPriorityUserBLL __nmpAppointmentPriorityUserBLL;
        public static INMPAppointmentPriorityUserBLL CreateNMPAppointmentPriorityUserBLL()
        {
            if (__nmpAppointmentPriorityUserBLL == null) __nmpAppointmentPriorityUserBLL = new NMPAppointmentPriorityUserBLL();
            return __nmpAppointmentPriorityUserBLL;
        }
        private static INMPAppointmentSpeciaRuleBLL __nmpAppointmentSpeciaRuleBLL;
        public static INMPAppointmentSpeciaRuleBLL CreateNMPAppointmentSpeciaRuleBLL()
        {
            if (__nmpAppointmentSpeciaRuleBLL == null) __nmpAppointmentSpeciaRuleBLL = new NMPAppointmentSpeciaRuleBLL();
            return __nmpAppointmentSpeciaRuleBLL;
        }
        private static INMPAppointmentSpeciaRuleUserBLL __nmpAppointmentSpeciaRuleUserBLL;
        public static INMPAppointmentSpeciaRuleUserBLL CreateNMPAppointmentSpeciaRuleUserBLL()
        {
            if (__nmpAppointmentSpeciaRuleUserBLL == null) __nmpAppointmentSpeciaRuleUserBLL = new NMPAppointmentSpeciaRuleUserBLL();
            return __nmpAppointmentSpeciaRuleUserBLL;
        }
        private static INMPAppointmentTimeLimitBLL __nmpAppointmentTimeLimitBLL;
        public static INMPAppointmentTimeLimitBLL CreateNMPAppointmentTimeLimitBLL()
        {
            if (__nmpAppointmentTimeLimitBLL == null) __nmpAppointmentTimeLimitBLL = new NMPAppointmentTimeLimitBLL();
            return __nmpAppointmentTimeLimitBLL;
        }
        private static INMPAppointmentTimeLimitUserBLL __nmpAppointmentTimeLimitUserBLL;
        public static INMPAppointmentTimeLimitUserBLL CreateNMPAppointmentTimeLimitUserBLL()
        {
            if (__nmpAppointmentTimeLimitUserBLL == null) __nmpAppointmentTimeLimitUserBLL = new NMPAppointmentTimeLimitUserBLL();
            return __nmpAppointmentTimeLimitUserBLL;
        }

        private static INMPSubjectAppointmentTimeLimitUserBLL __nmpSubjectAppointmentTimeLimitUserBLL;
        public static INMPSubjectAppointmentTimeLimitUserBLL CreateNMPSubjectAppointmentTimeLimitUserBLL()
        {
            if (__nmpSubjectAppointmentTimeLimitUserBLL == null) __nmpSubjectAppointmentTimeLimitUserBLL = new NMPSubjectAppointmentTimeLimitUserBLL();
            return __nmpSubjectAppointmentTimeLimitUserBLL;
        }
        private static INMPSubjectAppointmentTimeLimitBLL __nmpSubjectAppointmentTimeLimitBLL;
        public static INMPSubjectAppointmentTimeLimitBLL CreateNMPSubjectAppointmentTimeLimitBLL()
        {
            if (__nmpSubjectAppointmentTimeLimitBLL == null) __nmpSubjectAppointmentTimeLimitBLL = new NMPSubjectAppointmentTimeLimitBLL();
            return __nmpSubjectAppointmentTimeLimitBLL;
        }
        private static INMPTagEquipmentFunBLL __nmpTagEquipmentFunBLL;
        public static INMPTagEquipmentFunBLL CreateNMPTagEquipmentFunBLL()
        {
            if (__nmpTagEquipmentFunBLL == null) __nmpTagEquipmentFunBLL = new NMPTagEquipmentFunBLL();
            return __nmpTagEquipmentFunBLL;
        }
        private static INMPUnAppointmentPeriodTimeBLL __nmpUnAppointmentPeriodTimeBLL;
        public static INMPUnAppointmentPeriodTimeBLL CreateNMPUnAppointmentPeriodTimeBLL()
        {
            if (__nmpUnAppointmentPeriodTimeBLL == null) __nmpUnAppointmentPeriodTimeBLL = new NMPUnAppointmentPeriodTimeBLL();
            return __nmpUnAppointmentPeriodTimeBLL;
        }
        private static IUserNMPTimeUserBLL __userNMPTimeUserBLL;
        public static IUserNMPTimeUserBLL CreateUserNMPTimeUserBLL()
        {
            if (__userNMPTimeUserBLL == null) __userNMPTimeUserBLL = new UserNMPTimeUserBLL();
            return __userNMPTimeUserBLL;
        }
        private static IUserNMPTimeBLL __userNMPTimeBLL;
        public static IUserNMPTimeBLL CreateUserNMPTimeBLL()
        {
            if (__userNMPTimeBLL == null) __userNMPTimeBLL = new UserNMPTimeBLL();
            return __userNMPTimeBLL;
        }
        private static INMPPublicHolidaysBLL __nmpPublicHolidaysBLL;
        public static INMPPublicHolidaysBLL CreateNMPPublicHolidaysBLL()
        {
            if (__nmpPublicHolidaysBLL == null) __nmpPublicHolidaysBLL = new NMPPublicHolidaysBLL();
            return __nmpPublicHolidaysBLL;
        }
        private static INMPAppointmentBLL __nmpAppointmentBLL;
        public static INMPAppointmentBLL CreateNMPAppointmentBLL()
        {
            if (__nmpAppointmentBLL == null) __nmpAppointmentBLL = new NMPAppointmentBLL();
            return __nmpAppointmentBLL;
        }
        private static INMPAppointmentAddtionChargeItemBLL __nmpAppointmentAddtionChargeItemBLL;
        public static INMPAppointmentAddtionChargeItemBLL CreateNMPAppointmentAddtionChargeItemBLL()
        {
            if (__nmpAppointmentAddtionChargeItemBLL == null) __nmpAppointmentAddtionChargeItemBLL = new NMPAppointmentAddtionChargeItemBLL();
            return __nmpAppointmentAddtionChargeItemBLL;
        }
        private static IViewNMPAppointmentBLL __viewNMPAppointmentBLL;
        public static IViewNMPAppointmentBLL CreateViewNMPAppointmentBLL()
        {
            if (__viewNMPAppointmentBLL == null) __viewNMPAppointmentBLL = new ViewNMPAppointmentBLL();
            return __viewNMPAppointmentBLL;
        }
        private static INMPAdminBLL __nmpAdminBLL;
        public static INMPAdminBLL CreateNMPAdminBLL()
        {
            if (__nmpAdminBLL == null) __nmpAdminBLL = new NMPAdminBLL();
            return __nmpAdminBLL;
        }
        private static INMPUsedConfirmFeedBackBLL __nmpUsedConfirmFeedBackBLL;
        public static INMPUsedConfirmFeedBackBLL CreateNMPUsedConfirmFeedBackBLL()
        {
            if (__nmpUsedConfirmFeedBackBLL == null) __nmpUsedConfirmFeedBackBLL = new NMPUsedConfirmFeedBackBLL();
            return __nmpUsedConfirmFeedBackBLL;
        }
        private static INMPUsedConfirmAddtionChargeItemBLL __nmpUsedConfirmAddtionChargeItemBLL;
        public static INMPUsedConfirmAddtionChargeItemBLL CreateNMPUsedConfirmAddtionChargeItemBLL()
        {
            if (__nmpUsedConfirmAddtionChargeItemBLL == null) __nmpUsedConfirmAddtionChargeItemBLL = new NMPUsedConfirmAddtionChargeItemBLL();
            return __nmpUsedConfirmAddtionChargeItemBLL;
        }
        private static INMPUsedConfirmBLL __nmpUsedConfirmBLL;
        public static INMPUsedConfirmBLL CreateNMPUsedConfirmBLL()
        {
            if (__nmpUsedConfirmBLL == null) __nmpUsedConfirmBLL = new NMPUsedConfirmBLL();
            return __nmpUsedConfirmBLL;
        }
        private static IViewNMPUsedConfirmBLL __viewNMPUsedConfirmBLL;
        public static IViewNMPUsedConfirmBLL CreateViewNMPUsedConfirmBLL()
        {
            if (__viewNMPUsedConfirmBLL == null) __viewNMPUsedConfirmBLL = new ViewNMPUsedConfirmBLL();
            return __viewNMPUsedConfirmBLL;
        }
        private static IViewNMPUsedConfirmFeedBackBLL __viewNMPUsedConfirmFeedBackBLL;
        public static IViewNMPUsedConfirmFeedBackBLL CreateViewNMPUsedConfirmFeedBackBLL()
        {
            if (__viewNMPUsedConfirmFeedBackBLL == null) __viewNMPUsedConfirmFeedBackBLL = new ViewNMPUsedConfirmFeedBackBLL();
            return __viewNMPUsedConfirmFeedBackBLL;
        }
        private static INMPEquipmentAccessBLL __nmpEquipmentAccessBLL;
        public static INMPEquipmentAccessBLL CreateNMPEquipmentAccessBLL()
        {
            if (__nmpEquipmentAccessBLL == null) __nmpEquipmentAccessBLL = new NMPEquipmentAccessBLL();
            return __nmpEquipmentAccessBLL;
        }
        private static INMPEquipmentAuthorizationBLL __nmpEquipmentAuthorizationBLL;
        public static INMPEquipmentAuthorizationBLL CreateNMPEquipmentAuthorizationBLL()
        {
            if (__nmpEquipmentAuthorizationBLL == null)
                __nmpEquipmentAuthorizationBLL = new NMPEquipmentAuthorizationBLL();
            return __nmpEquipmentAuthorizationBLL;
        }
        private static IViewNMPEquipmentAuthorizationBLL __viewNMPEquipmentAuthorizationBLL;
        public static IViewNMPEquipmentAuthorizationBLL CreateViewNMPEquipmentAuthorizationBLL()
        {
            if (__viewNMPEquipmentAuthorizationBLL == null)
                __viewNMPEquipmentAuthorizationBLL = new ViewNMPEquipmentAuthorizationBLL();
            return __viewNMPEquipmentAuthorizationBLL;
        }
        #endregion

        #region 校级数据同步
        private static ISchoolEquipmentBLL __schoolEquipmentBLL;
        public static ISchoolEquipmentBLL CreateSchoolEquipmentBLL()
        {
            if (__schoolEquipmentBLL == null) __schoolEquipmentBLL = new SchoolEquipmentBLL();
            return __schoolEquipmentBLL;
        }
        #endregion

        #region OAuth
        private static IOAuthClientBLL __oAuthClientBLL;
        public static IOAuthClientBLL CreateOAuthClientBLL()
        {
            if (__oAuthClientBLL == null) __oAuthClientBLL = new OAuthClientBLL();
            return __oAuthClientBLL;
        }
        private static IOAuthBLL __oAuthBLL;
        public static IOAuthBLL CreateOAuthBLL()
        {
            if (__oAuthBLL == null) __oAuthBLL = new OAuthBLL();
            return __oAuthBLL;
        }
        private static IOAuthCodeBLL __oAuthCodeBLL;
        public static IOAuthCodeBLL CreateOAuthCodeBLL()
        {
            if (__oAuthCodeBLL == null) __oAuthCodeBLL = new OAuthCodeBLL();
            return __oAuthCodeBLL;
        }
        private static IOAuthTokenBLL __oAuthTokenBLL;
        public static IOAuthTokenBLL CreateOAuthTokenBLL()
        {
            if (__oAuthTokenBLL == null) __oAuthTokenBLL = new OAuthTokenBLL();
            return __oAuthTokenBLL;
        }
        #endregion

        #region CAS
        private static ICasBLL __casBLL;
        public static ICasBLL CreateCasBLL()
        {
            if (__casBLL == null) __casBLL = new CasBLL();
            return __casBLL;
        }
        #endregion

        #region 平台数据接口
        private static IViewUserUsingEquipmentStatBLL __viewUserUsingEquipmentStatBLL;
        public static IViewUserUsingEquipmentStatBLL CreateViewUserUsingEquipmentStatBLL()
        {
            if (__viewUserUsingEquipmentStatBLL == null) __viewUserUsingEquipmentStatBLL = new ViewUserUsingEquipmentStatBLL();
            return __viewUserUsingEquipmentStatBLL;
        }
        private static IViewSchoolEquipmentBLL __viewSchoolEquipmentBLL;
        public static IViewSchoolEquipmentBLL CreateViewSchoolEquipmentBLL()
        {
            if (__viewSchoolEquipmentBLL == null) __viewSchoolEquipmentBLL = new ViewSchoolEquipmentBLL();
            return __viewSchoolEquipmentBLL;
        }
        #endregion

        #region 效益评价
        private static IEvaluationTableBLL __evaluationTableBLL;
        public static IEvaluationTableBLL CreateEvaluationTableBLL()
        {
            return __evaluationTableBLL ?? (__evaluationTableBLL = new EvaluationTableBLL());
        }
        private static IViewEvaluationTableStatBLL __viewEvaluationTableStatBLL;
        public static IViewEvaluationTableStatBLL CreateViewEvaluationTableStatBLL()
        {
            return __viewEvaluationTableStatBLL ?? (__viewEvaluationTableStatBLL = new ViewEvaluationTableStatBLL());
        }

        #endregion

        ///// <summary> 创建金额验证业务逻辑接口 </summary>
        ///// <returns></returns>
        //public static IMoneyValidateBLL CreateMoneyValidateBLL()
        //{
        //    return CustomDiffHandlerManager.CreateMoneyValidateBLL();
        //}


        private static IChangeeEquAuthorizationLogBLL _changeeEquAuthorizationLogBLL;
        public static IChangeeEquAuthorizationLogBLL CreateChangeeEquAuthorizationLogBLL()
        {
            if (_changeeEquAuthorizationLogBLL == null)
                _changeeEquAuthorizationLogBLL = new ChangeeEquAuthorizationLogBLL();
            return _changeeEquAuthorizationLogBLL;
        }
    }
}