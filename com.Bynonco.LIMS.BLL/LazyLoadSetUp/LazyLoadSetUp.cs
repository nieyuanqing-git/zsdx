using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL.LazyLoadSetUp
{
    public class LazyLoadSetUp
    {
        public static void SetUp()
        {
            #region 前台菜单设置
            ViewHomeMenuLazyLoadHandlerSetUp();
            ViewHomeMenuIsEnableUpOperateLazyLoadHandlerSetUp();
            ViewHomeMenuIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region 系统模块
            ModuleFunctionParentLazyLoadHandlerSetUp();
            ModuleFunctionChildrenLazyLoadHandlerSetUp();
            ViewModuleFunctionParentLazyLoadHandlerSetUp();
            ViewModuleFunctionChildrenCountLazyLoadHandlerSetUp();
            ModuleFunctionIsEnableUpOperateLazyLoadHandlerSetUp();
            ModuleFunctionIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region 用户管理
            UserTypeLazyLoadHandlerSetUp();
            UserTypeChildrenCountLazyLoadHandlerSetUp();
            UserTypeIsEnableUpOperateLazyLoadHandlerSetUp();
            UserTypeIsEnableDownOperateLazyLoadHandlerSetUp();
            TagIsEnableUpOperateLazyLoadHandlerSetUp();
            TagIsEnableDownOperateLazyLoadHandlerSetUp();
            UserEquipmentLazyLoadHandlerSetUp();
            UserRelevantSubjectsLazyLoadHandlerSetUp();
            UserWorkGroupsLazyLoadHandlerSetUp();
            WorkGroupLazyHandlerSetUp();
            UserMySubjectLazyLoadHandlerSetUp();
            UsercCooperativeSubjectsLazyLoadHandlerSetUp();
            UserLazyLoadHandlerSetUp();
            CreditLimitLazyLoadHandlerSetUp();
            UserAccountLazyLoadHandlerSetUp();
            UserSelfAccountLazyLoadHandlerSetUp();
            TagLazyLoadHandlerSetUp();
            #endregion

            #region 辅助字典管理
            DictCodeTypeDictCodesLazyLoadHandlerSetUp();
            DictCodeTypeParentLazyLoadHandlerSetUp();
            DictCodeTypeLazyLoadHandlerSetUp();
            DictCodeTypeLazyLoadByCodeHandlerSetUp();
            DictCodeLazyLoadHandlerSetUp();
            #endregion

            #region 送样管理
            SampleApplyLazyLoadHandlerSetUp();
            SampleApplyChargeItemLazyLoadHandlerSetUp();
            SampleApplyLazyLoadHandlerSetUp();
            SampleApplyTesterLazyLoadHandlerSetUp();
            SampleApplyItemsLazyLoadHandlerSetUp();
            SampleApplyJoinedTesterLazyLoadHandlerSetUp();
            SampleApplyAnalysisAttachmentsLazyLoadHandlerSetUp();
            SampleItemLazyLoadHandlerSetUp();
            SampleStatusLazyLoadHandlerSetUp();
            SampleChargeItemLazyLoadHandlerSetUp();
            SampleChargeItemRelateSampleChargeItemsLazyLoadHandlerSetUp();
            SampleStatusSampleItemsLazyLoadHandlerSetUp();
            SampleItemSampleItemStatusesLazyLoadHandlerSetUp();
            RelateSampleItemsLazyLoadHandlerSetUp();
            SampleTestRecordLazyLoadHandlerSetUp();
            SampleApplyChargeItemsLazyLoadHandlerSetUp();
            SampleApplySampleItemResultsLazyHanderSetUp();
            SampleApplyAttachmentsHandlerSetUp();
            SampleApplyNumbersLazyLoadHandlerSetUp();
            SampleParameterEquipmentsLazyLoadHandlerSetUp();
            SampleParameterStatusesLazyLoadHandlerSetUp();
            SampleParameterItemsLazyLoadHandlerSetUp();
            SampleParameterLazyLoadHandlerSetUp();
            SampleApplyParametersLazyLoadHandlerSetUp();
            SampleParameterLazyLoadHandlerSetUp();
            SampleApplyBySampleNoLazyLoadHandlerSetUp();
            SampleFeedBackAttachmentsLazyLoadHandlerSetUp();
            SampleItemLabelItemsLazyLoadHandlerSetUp();
            SampleItemLabelLazyLoadHandlerSetUp();
            SampleItemLabelChargeStandardsLazyLoadHanderSetUp();
            SampleApplyCostDeductLazyLoadHandlerSetUp();
            SampleApplyStatusNameLazyLoadHandlerSetUp();
            SampleChargeStatusNameLazyLoadHandlerSetUp();
            #endregion

            #region 设备管理
            EquipmentCurStatusInfoLazyLoadHandlerSetUp();
            EquipmentLazyLoadHandlerSetUp();
            ViewEquipmentLazyLoadHandlerSetUp();
            EquipmentLinkMenLazyLoadHandlerSetUp();
            EquipmentCategoryBindsLazyLoadHandlerSetUp();
            EquipmentDirectorsLazyLoadHandlerSetUp();
            EquipmentSampleStatusLazyLoadHandlerSetUp();
            EquipmentFocusUsersLazyLoadHandlerSetUp();
            EquipmentDefaultSettingLazyLoadHandlerSetUp();
            EquipmentLabelItemsLazyLoadHandlerSetUp();
            EquipmentLabelsLazyLoadHandlerSetUp();
            EquipmentCalcFeeTimeRuleLazyLoadHandlerSetUp();
            EquipmentChargeStandardLazyLoadHandlerSetUp();
            EquipmentAdditionChargeItemsLazyLoadHandlerSetUp();
            EquipmentLabelAddtionChargeItemsLazyLoadHanderSetUp();
            EquipmentLabelChargeStandardsLazyLoadHanderSetUp();
            EquipmentLabelLazyLoadHandlerSetUp();
            EquipmentBlackListLazyLoadHandlerSetUp();
            EquipmentBlackListMessageTemplateLazyLoadHandlerSetUp();
            EquipmentBlackListLabelsHandlerLazyLoadHandlerSetup();
            EquipmentNoticeAttachmentsLazyLoadHandlerSetUp();
            EquipmentNoticesLazyLoadHandlerSetUp();
            EquipmentTrainningLazyLoadHandlerSetUp();
            EquipmentTrainningsLazyLoadHandlerSetUp();
            EquipmentUnAppointmentPeriodTimesLazyLoadHandlerSetUp();
            EquipmentUserEquipmentTimeLazyLoadHandlerSetUp();
            EquipmentAppointmentTimeLimitUserLazyLoadHandlerSetUp();
            EquipmentTagEquipmentFunsLazyLoadHandlerSetUp();
            EquipmentAppointmentSpeciaRuleLazyLoadHandlerSetUp();
            EquipmentMarkingLazyLoadHandlerSetUp();
            EquipmentBrokenReportLazyLoadHandlerSetUp();
            EquipmentRepairLazyLoadHandlerSetUp();
            EquipmentRepairsLazyLoadHandlerSetUp();
            EquipmentStatusLogsLazyLoadHandlerSetUp();
            EquipmentMarkingsLazyLoadHandlerSetUp();
            EquipmentPartLazyLoadHandlerSetUp();
            EquipmentAdditionChargeItemLazyLoadHandlerSetUp();
            EquipmentCountByOrgXPathLazyLoadHandlerSetUp();
            EquipmentChildCountByOrgXPathLazyLoadHandlerSetUp();
            EquipmentCountByRoomXPathLazyLoadHandlerSetUp();
            EquipmentChildCountByRoomXPathLazyLoadHandlerSetUp();
            EquipmentCountByControllerTypeLazyLoadHandlerSetUp();
            EquipmentCountByCategoryXPathLazyLoadHandlerSetUp();            
            EquipmentChildCountByCategoryXPathLazyLoadHandlerSetUp();
            EquipmentTotalInfoLazyLoadHandlerSetUp();
            EquipmentUseConditionLoadHandlerSetUp();
            EquipmentsLazyLoadHandlerSetUp();
            EquipmentRelationListByEquipmentIdLazyLoadHandlerSetUp();
            EquipmentDutyFreeCountByCategoryXPathLazyLoadHandlerSetUp();
            EquipmentDutyFreeCountByOrgXPathLazyLoadHandlerSetUp();
            EquipmentDutyFreeCountByRoomXPathLazyLoadHandlerSetUp();
            EquipmentAchievementStudentLazyLoadHandlerSetUp();
            EquipmentViewAchievementStudentLazyLoadHandlerSetUp();
            EquipmentViewThesisLazyLoadHandlerSetUp();
            EquipmentViewThesisEquipmentLazyLoadHandlerSetUp();
            EquipmentViewPatentisLazyLoadHandlerSetUp();
            EquipmentViewPatentEquipmentLazyLoadHandlerSetUp();
            EquipmentViewSubjectAchievementLazyLoadHandlerSetUp();
            EquipmentViewSubjectEquipmentLazyLoadHandlerSetUp();
            EquipmentRoomDirectorLazyLoadHandlerSetUp();
            EquipmentProfessorLazyLoadHandlerSetUp();            

            #endregion

            #region  仪器组群
            EquipmentGroupLazyLoadHandlerSetUp();
            EquipmentGroupMembersLoadHandlerSetUp();
            EquipmentGroupAdminsLoadHandlerSetUp();
            EquipmentGroupAdminIdListLoadHandlerSetUp();
            #endregion
            
            #region 设备分类管理
            EquipmentCategoryLazyLoadHandlerSetUp();
            EquipmentCategoryChildrenCountLazyLoadHandlerSetUp();
            EquipmentCategoryIsEnableUpOperateLazyLoadHandlerSetUp();
            EquipmentCategoryIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region 机构管理
            LabOrganizationLazyLoadHandlerSetUp();
            LabOrganizationChildrenCountLazyLoadHandlerSetUp();
            LabOrganizationIsEnableUpOperateLazyLoadHandlerSetUp();
            LabOrganizationIsEnableDownOperateLazyLoadHandlerSetUp();
            LabOrganizationAuthorizedListLazyLoadHandlerSetUp();
            LabOrganizationDirectorsLazyLoadHandlerSetUp();
            #endregion

            #region 成果管理
            AttachmentLazyLoadHandlerSetUp();
            SubjectAchievementMemberListLazyLoadHandlerSetUp();
            SubjectAchievementDepartmentListLazyLoadHandlerSetUp();
            SubjectEquipmentLazyLoadHandlerSetUp();
            ThesisLazyLoadHandlerSetUp();
            FirstThesisAuthorListLoadHandlerSetUp();
            OtherThesisAuthorListLazyLoadHandlerSetUp();
            ThesisEquipmentListLazyLoadHandlerSetUp();
            ThesisDepartmentListLazyLoadHandlerSetUp();
            PatentLazyLoadHandlerSetUp();
            PatentUserListLazyLoadHandlerSetUp();
            PatentEquipmentListLazyLoadHandlerSetUp();
            AwardsLazyLoadHandlerSetUp();
            AwardsAuthorListLazyLoadHandlerSetUp();
            AwardsDepartmentListLazyLoadHandlerSetUp();
            AwardsEquipmentLazyLoadHandlerSetUp();
            AcademicExchangesLazyLoadHandlerSetUp();
            AcademicExchangesUserLocalListLazyLoadHandlerSetUp();
            AcademicExchangesUserOutListLazyLoadHandlerSetUp();
            AcademicExchangesDepLocalListLazyLoadHandlerSetUp();
            AcademicExchangesDepOutListLazyLoadHandlerSetUp();
            #endregion

            #region 交流天地
            ExchangeFairylandCommentLazyLoadHandlerSetUp();
            ExchangeFairylandCreatorLazyLoadHandlerSetUp();
            ExchangeFairylandCommentCreatorLazyLoadHandlerSetUp();
            #endregion

            #region 文章管理
            ArticleCategoryLazyLoadHandlerSetUp();
            ArticleInCategoryLazyLoadHandlerSetUp();
            ArticleCategoryChildrenCountLazyLoadHandlerSetUp();
            ArticleCategoryIsEnableUpOperateLazyLoadHandlerSetUp();
            ArticleCategoryIsEnableDownOperateLazyLoadHandlerSetUp();
            ArticleRelationListByArticleIdLazyLoadHandlerSetUp();
            #endregion
            
            #region Q&A
            AnswerLazyLoadHandlerSetUp();
            #endregion

            #region 预约规则管理
            AppointmentPriorityUsersLazyLoadHandlerSetUp();
            UserEquipmentTimeUsersLazyLoadHandlerSetUp();
            HolidaysExcludesLazyLoadHandlerSetUp();
            HolidaysIncludesLazyLoadHandlerSetUp();
            AppointmentSpeciaRuleUsersLazyLoadSetUp();
            #endregion
            
            #region 课题组管理
            SubjectLazyLoadHandlerSetUp();
            SubjectByDirectorIdLazyLoadHandlerSetUp();
            SubjectExperimentersLazyLoadHanlderSetUp();
            ViewSubjectLazyLoadHandlerSetUp();
            SubjectProjectsLazyLoadHandlerSetUp();
            SubjectProjectLazyLoadHandlerSetUp();
            #endregion

            #region 预约管理
            AppointmentLazyLoadHandlerSetUp();
            AppointmentEquipmentPartsLazyLoadHandlerSetUp();
            AppointmentEquipmentAddtionChargeItemsLazyLoadHandlerSetUp();
            AppointmentEquipmentUseConditionsLazyLoadHandlerSetUp();
            AppointmentPredictCostDeductLazyLoadHandlerSetUp();
            #endregion

            #region 扣费管理
            UsedConfirmLazyLoadHandlerSetUp();
            ManualCostDeductLazyLoadHandlerSetUp();
            CostDeductLogsLazyLoadHandlerSetUp();
            UsedConfirmCostDeductLazyLoadHandlerSetUp();
            UsedConfirmFeedBackLazyLoadHandlerSetUp();
            CostDeductEquipmentOpenFundsLazyLoadHandlerSetUp();
            AnimalCostDeductLazyLoadHandlerSetUp();
            WaterControlCostDeductLazyLoadHandlerSetUp();
            CostDeductLazyLoadHandlerSetUp();
            #endregion

            #region 站内消息
            ReceiverListLogsLazyLoadHandlerSetUp();
            #endregion

            #region 不良行为管理
            PunishRecordLazyLoadHandlerSetUp();
            DelinquencyConfirmsLazyLoadHandlerSetUp();
            PunishActionDelinquencyConfirmsLazyLoadHandlerSetUp();
            DelinquencyConfirmPunishActionsLazyLoadHandlerSetUp();
            DelinquencyConfirmLazyLoadHandlerSetUp();
            PunishRecordPunishActionsLazyLoadHandlerSetUp();
            RepealDelinquencyLazyLoadHandlerSetUp();
            PunishActionLazyLoadHandlerSetUp();
            #endregion

            #region 在线培训考试
            TrainningMaterialIsEnableUpOperateLazyLoadHandlerSetUp();
            TrainningMaterialIsEnableDownOperateLazyLoadHandlerSetUp();
            ViewTrainningQuestionLazyLoadHandlerSetUp();
            TrainningQuestionListLazyLoadHandlerSetUp();
            TrainningMaterialListLazyLoadHandlerSetUp();
            TrainningQuestionIsEnableUpOperateLazyLoadHandlerSetUp();
            TrainningQuestionIsEnableDownOperateLazyLoadHandlerSetUp();
            TrainningQuestionItemListLazyLoadHandlerSetUp();
            TrainningQuestionItemIsEnableUpOperateLazyLoadHandlerSetUp();
            TrainningQuestionItemIsEnableDownOperateLazyLoadHandlerSetUp();
            TrainningExaminationLazyLoadHandlerSetUp();
            UserExaminationLazyLoadHandlerSetUp();
            UserExaminationQuestionListLazyLoadHandlerSetUp();
            TrainningTypeLoadHandlerSetUp();
            TrainningTypeChildrenCountLazyLoadHandlerSetUp();
            TrainningTypeIsEnableUpOperateLazyLoadHandlerSetUp();
            TrainningTypeIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region 门禁管理 
            DoorLazyLoadHandlerSetUp();
            #endregion

            #region 视频监控
            CameraRelationEquipmentLazyLoadHandlerSetUp();
            CameraRelationDoorLazyLoadHandlerSetUp();
            CameraRelationLazyLoadHandlerSetUp();
            #endregion

            #region 网盘管理
            SubAreaCategorySubAreasSetUp();
            SubAreaSubAreaCategorySetUp();
            SubAreaSubAreaFilesSetUp();
            SubAreaSubAreaTagPowersSetUp();
            SubAreaSubAreaUerPowersSetUp();
            SubAreaSubAreaUserWithoutPowersSetUp();
            SubAreaFileSubAreaSetUp();
            SubAreaFileParentSetUp();
            SubAreaFileChildrensSetUp();
            SubAreaCategoryNameSetUp();
            SubAreaNameSetUp();
            SubAreaUsedSizeSetUp();
            SubAreaFileChildrenTotalSizeSetUp();
            UserSubAreasSetUp();
            #endregion

            #region 基础信息
            CountryLazyLoadHandlerSetUp();
            #endregion

            #region 计费标准管理
            UnitPriceLoadHandlerSetUp();
            #endregion

            #region 设备入网申请
            EquipmentServiceHoursStatsLoadHandlerSetUp();
            EquipmentGroupServersLoadHandlerSetUp();
            EquipmentApplyLoadHandlerSetUp();
            #endregion

            #region 设备维修基金申请
            EquipmentRepairFundsApplyLoadHandlerSetUp();
            EquipmentRepairFundsApplyAttachmentsLoadHandlerSetUp();
            #endregion

            #region 耗材管理
            MaterialCategoryLazyLoadHandlerSetUp();
            MaterialCategoryChildrenCountLazyLoadHandlerSetUp();
            MaterialCategoryIsEnableUpOperateLazyLoadHandlerSetUp();
            MaterialCategoryIsEnableDownOperateLazyLoadHandlerSetUp();
            MaterialInfoLazyLoadHandlerSetUp();
            MaterialAdminListLazyLoadHandlerSetUp();
            MaterialPurchaseLazyLoadHandlerSetUp();
            MaterialPurchaseItemListLazyLoadHandlerSetUp();
            ViewMaterialPurchaseItemListLazyLoadHandlerSetUp();
            PurchaseItemInputCountLazyLoadHandlerSetUp();
            MaterialInputItemListLazyLoadHandlerSetUp();
            ViewMaterialInputItemListLazyLoadHandlerSetUp();
            MaterialRecipientLazyLoadHandlerSetUp();
            MaterialRecipientItemListLazyLoadHandlerSetUp();
            ViewMaterialRecipientItemListLazyLoadHandlerSetUp();
            MaterialOutputLazyLoadHandlerSetUp();
            MaterialOutputItemListLazyLoadHandlerSetUp();
            ViewMaterialOutputItemListLazyLoadHandlerSetUp();
            MaterialBrokenLazyLoadHandlerSetUp();
            MaterialBrokenItemListLazyLoadHandlerSetUp();
            ViewMaterialBrokenItemListLazyLoadHandlerSetUp();
            MaterialSupplierListLazyLoadHandlerSetUp();
            #endregion

            #region 实验动物管理
            AnimalLazyLoadHandlerSetUp();
            AnimalCategoryLazyLoadHandlerSetUp();
            AnimalCategoryChildrenCountLazyLoadHandlerSetUp();
            AnimalCategoryIsEnableUpOperateLazyLoadHandlerSetUp();
            AnimalCategoryIsEnableDownOperateLazyLoadHandlerSetUp();
            EthicsApplyAnimalExperimentersLazyLoadHandlerSetUp();
            AnimalFrameLazyLoadHandlerSetUp();
            AnimalOutAppointmentDetailsLazyLoadHandlerSetUp();
            EthicsApplyByNoLazyLoadHandlerSetUp();
            EthicsApplyAnimalDatasLazyLoadHandlerSetUp();
            EthicsApplyLazyLoadHandlerSetUp();
            #endregion

            #region 设备考核评价
            JudgeProjectListLazyLoadHandlerSetUp();
            JudgeProjectIsEnableUpOperateLazyLoadHandlerSetUp();
            JudgeProjectIsEnableDownOperateLazyLoadHandlerSetUp();
            JudgeProjectContentListLazyLoadHandlerSetUp();
            JudgeProjectContentIsEnableUpOperateLazyLoadHandlerSetUp();
            JudgeProjectContentIsEnableDownOperateLazyLoadHandlerSetUp();
            JudgeProjectRecordListLazyLoadHandlerSetUp();
            ViewJudgeProjectRecordListLazyLoadHandlerSetUp();
            JudgeProjectContentRecordListLazyLoadHandlerSetUp();
            ViewJudgeProjectContentRecordListLazyLoadHandlerSetUp();
            #endregion

            #region 存款管理
            DepositRecordEquipmentListLazyLoadHandlerSetUp();
            DepositRecordOpenFundLazyLoadHandlerSetUp();
            #endregion
 
            #region 开放基金
            OpenFundApplyLazyLoadHandlerSetUp();
            EquipmentListByOpenFundApplyIdLazyLoadHandlerSetUp();
            OpenFundDepositRecordListLazyLoadHandlerSetUp();
            OpenFundDepositRecordListByDepositRecordOpenFundIdLazyLoadHandlerSetUp();
            DepositRecordOpenFundEquipmentListLazyLoadHandlerSetUp();
            #endregion

            #region 学期管理
            SemesterLazyLoadHandlerSetUp();
            SemesterIsEnableUpOperateLazyLoadHandlerSetUp();
            SemesterIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region 外出及特殊业务活动申请及报销流程
            ActivityTypeLazyLoadHandlerSetUp();
            ActivityTypeIsEnableUpOperateLazyLoadHandlerSetUp();
            ActivityTypeIsEnableDownOperateLazyLoadHandlerSetUp();
            #endregion

            #region CERS
            EquipmentExtendLazyLoadHandlerSetUp();
            EquipmentCategoryShareLazyLoadHandlerSetUp();
            EquipmentCustomsBindLoadHandlerSetUp();
            #endregion

            #region 上报报表
            SJ3StatisticsLazyLoadHandlerSetUp();
            SJ3ListLazyLoadHandlerSetUp();
            #endregion

            #region 面向本科生开放管理
            EquipmentOpenTrainingLazyLoadHandlerSetUp();
            EquipmentOpenTrainingPlanListLazyLoadHandlerSetUp();
            EquipmentOpenTrainingCarryOutLazyLoadHandlerSetUp();
            EquipmentOpenTrainingCarryOutPlanListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeEquipmentListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeExperienceListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeTeacherListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeMemberListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeMaterialListLazyLoadHandlerSetUp();
            EquipmentOpenPracticeUsedEquipmentHoursLazyLoadHandlerSetUp();
            EquipmentOpenPracticeUsedEquipmentMoneyLazyLoadHandlerSetUp();
            #endregion

            #region 楼宇管理
            BuildingLazyLoadHandlerSetUp();
            BuildingChildrenCountLazyLoadHandlerSetUp();
            BuildingIsEnableUpOperateLazyLoadHandlerSetUp();
            BuildingIsEnableDownOperateLazyLoadHandlerSetUp();
            BuildingUpInfoLazyLoadHandlerSetUp();
            BuildingDownInfoLazyLoadHandlerSetUp();
            BuildingEquipmentListByBuildingIdLazyLoadHandlerSetUp();
            #endregion

            #region 工位预约
            NMPEquipmentsLazyLoadHandlerSetUp();
            NMPDefaultSettingLazyLoadHandlerSetUp();
            NMPLazyLoadHandlerSetUp();
            NMPLabelItemsLazyLoadHandlerSetUp();
            NMPLabelLazyLoadHandlerSetUp();
            NMPCalcFeeTimeRuleLazyLoadHandlerSetUp();
            NMPChargeStandardLazyLoadHandlerSetUp();
            NMPAdditionChargeItemsLazyLoadHandlerSetUp();
            NMPLabelAddtionChargeItemsLazyLoadHanderSetUp();
            NMPLabelChargeStandardsLazyLoadHanderSetUp();
            NMPLabelsLazyLoadHandlerSetUp();
            NMPAppointmentPriorityUsersLazyLoadHandlerSetUp();
            NMPAppointmentSpeciaRuleUsersLazyLoadSetUp();
            NMPAppointmentSpeciaRuleLazyLoadHandlerSetUp();
            NMPAppointmentTimeLimitUsersLazyLoadHandlerSetUp();
            UserNMPTimeUsersLazyLoadHandlerSetUp();
            NMPEquipmentLazyLoadHandlerSetUp();
            NMPAdditionChargeItemLazyLoadHandlerSetUp();
            NMPAppointmentAddtionChargeItemsLazyLoadHandlerSetUp();
            NMPAppointmentLazyLoadHandlerSetUp();
            NMPDirectorsLazyLoadHandlerSetUp();
            NMPUsedConfirmCostDeductLazyLoadHandlerSetUp();
            NMPUsedConfirmAddtionChargeItemsLazyLoadHandlerSetUp();
            #endregion

            #region 共享基金
            ShareFundApplyLazyLoadHandlerSetUp();
            ShareFundApplyEquipmentListLazyLoadHandlerSetUp();
            ViewShareFundApplyEquipmentListLazyLoadHandlerSetUp();
            #endregion
        }

        #region 前台菜单设置延迟加载
        /// <summary>
        /// 前台菜单设置延迟加载
        /// </summary>
        public static void ViewHomeMenuLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewHomeMenuLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewHomeMenuLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewHomeMenuBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 前台菜单设置是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void ViewHomeMenuIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewHomeMenuIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewHomeMenuIsEnableUpOperateLazyLoadHandler += (xPath, parentId, labOrganizationId) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        long xPathValue = 0;
                        if (xPathLength > 0 && long.TryParse(xPath, out xPathValue))
                        {
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ViewHomeMenu WHERE
                                                  XPathLength = {0} AND XPathValue <{1} AND LabOrganizationId='{2}'", xPathLength, xPathValue, labOrganizationId);
                            sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ParentId IS NULL" : " AND ParentId='" + parentId.Value.ToString() + "'");
                            var result = BLLFactory.CreateViewHomeMenuBLL().GetSingleResult(sbSql.ToString());
                            return Convert.ToInt32(result) > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 前台菜单设置是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void ViewHomeMenuIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewHomeMenuIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewHomeMenuIsEnableDownOperateLazyLoadHandler += (xPath, parentId,labOrganizationId) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        long xPathValue = 0;
                        if (xPathLength > 0 && long.TryParse(xPath, out xPathValue))
                        {
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ViewHomeMenu WHERE
                                                  XPathLength = {0} AND XPathValue >{1} AND LabOrganizationId='{2}'", xPathLength, xPathValue, labOrganizationId);
                            sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ParentId IS NULL" : " AND ParentId='" + parentId.Value.ToString() + "'");
                            var result = BLLFactory.CreateViewHomeMenuBLL().GetSingleResult(sbSql.ToString());
                            return Convert.ToInt32(result) > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch { return false; }
                };
            }
        }
        #endregion

        #region 系统模块
        /// <summary>
        /// 模块父模块延迟加载
        /// </summary>
        public static void ViewModuleFunctionParentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewModuleFunctionParentLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewModuleFunctionParentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewModuleFunctionBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 模块父模块延迟加载
        /// </summary>
        public static void ModuleFunctionParentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ModuleFunctionParentLazyLoadHandler == null)
            {
                LazyLoadDefinition.ModuleFunctionParentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateModuleFunctionBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 模块子模块延迟加载
        /// </summary>
        public static void ModuleFunctionChildrenLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ModuleFunctionChildrenLazyLoadHandler == null)
            {
                LazyLoadDefinition.ModuleFunctionChildrenLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateModuleFunctionBLL().GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsStop=false*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 模块是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void ModuleFunctionIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ModuleFunctionIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ModuleFunctionIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        if (xPathLength > 0 && xPath.Substring(xPathLength - 1) != "0")
                        {
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ModuleFunction WHERE
                                                  XPathLength = {0} AND XPath <{1}", xPathLength, xPath);
                            sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ModuleFunction.ParentId IS NULL" : " AND ModuleFunction.ParentId='" + parentId.Value.ToString() + "'");
                            var result = BLLFactory.CreateModuleFunctionBLL().GetSingleResult(sbSql.ToString());
                            return Convert.ToInt32(result) > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 模块是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void ModuleFunctionIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ModuleFunctionIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ModuleFunctionIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        if (xPathLength > 0)
                        {
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ModuleFunction WHERE
                                                  XPathLength = {0} AND XPath >{1}", xPathLength, xPath);
                            sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ModuleFunction.ParentId IS NULL" : " AND ModuleFunction.ParentId='" + parentId.Value.ToString() + "'");
                            var result = BLLFactory.CreateModuleFunctionBLL().GetSingleResult(sbSql.ToString());
                            return Convert.ToInt32(result) > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// View模块孩子数延迟加载
        /// </summary>
        public static void ViewModuleFunctionChildrenCountLazyLoadHandlerSetUp()
        {
            if(LazyLoadDefinition.ViewModuleFunctionChildrenCountLazyLoadHandler==null)
            {
                LazyLoadDefinition.ViewModuleFunctionChildrenCountLazyLoadHandler+=(xPath)=>
                    {
                        try
                        {
                            return (int)BLLFactory.CreateViewModuleFunctionBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewModuleFunction WHERE IsDelete=0 And XPath LIKE '{0}%' And XPath<>'{0}'", xPath));
                        }
                        catch{ return 0;}
                    };
            }
        }
        #endregion

        #region 用户管理
        /// <summary>
        /// 用户身份延迟加载
        /// </summary>
        public static void UserTypeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserTypeLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserTypeLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserTypeBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        ///用户身份孩子个数延迟加载
        /// </summary>
        public static void UserTypeChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserTypeChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserTypeChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateUserTypeBLL().GetSingleResult(string.Format("select count(*) from UserType where XPath like '{0}%' and IsDelete=0 ", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 用户身份是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void UserTypeIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserTypeIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserTypeIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM UserType WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(UserType.XPath) = len('{0}')
                                                 AND CAST(UserType.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND UserType.ParentId IS NULL" : " AND UserType.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateUserTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 用户身份是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void UserTypeIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserTypeIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserTypeIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM UserType WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(UserType.XPath) = len('{0}')
                                                 AND CAST(UserType.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND UserType.ParentId IS NULL" : " AND UserType.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateUserTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 用户参与课题延迟加载
        /// </summary>
        public static void UserRelevantSubjectsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserRelevantSubjectsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserRelevantSubjectsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSubjectBLL().GetUserRelevantSubjects(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 用户工作组延迟加载
        /// </summary>
        public static void UserWorkGroupsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserWorkGroupsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserWorkGroupsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserWorkGroupBLL().GetEntitiesByExpression(string.Format("UserId=\"{0}\"", id.ToString())).Select(p => p.GetWorkGroup()).ToList();
                        }
                        catch { return null; }
                    };
            }

        }
        /// <summary>
        /// 工作组延迟加载
        /// </summary>
        public static void WorkGroupLazyHandlerSetUp()
        {
            if (LazyLoadDefinition.WorkGroupLazyHandler == null)
            {
                LazyLoadDefinition.WorkGroupLazyHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateWorkGroupBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 用户课题组延迟加载
        /// </summary>
        public static void UserMySubjectLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserMySubjectLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserMySubjectLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSubjectBLL().GetUserSelfJoinSubject(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 用户合作课题延迟加载
        /// </summary>
        public static void UsercCooperativeSubjectsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsercCooperativeSubjectsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsercCooperativeSubjectsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectBLL().GetUserCooperativeSubjects(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 用户信用额度延迟加载
        /// </summary>
        public static void CreditLimitLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CreditLimitLazyLoadHandler == null)
            {
                LazyLoadDefinition.CreditLimitLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateCreditLimitBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 用户账户延迟加载
        /// </summary>
        public static void UserAccountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserAccountLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserAccountLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserAccountBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 用户帐户延迟加载
        /// </summary>
        public static void UserSelfAccountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserSelfAccountLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserSelfAccountLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserAccountBLL().GetEntitiesByExpression(string.Format("UserId=\"{0}\"", id.ToString())).First();
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 用户延迟加载
        /// </summary>
        public static void UserLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 用户身份类型延迟加载
        /// </summary>
        public static void TagLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TagLazyLoadHandler == null)
            {
                LazyLoadDefinition.TagLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateTagBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 用户类型延迟处理
        /// <summary>
        /// 用户类型是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void TagIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TagIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TagIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Tag WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(Tag.XPath) = len('{0}')
                                                 AND CAST(Tag.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateTagBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 用户类型是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void TagIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TagIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TagIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Tag WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(Tag.XPath) = len('{0}')
                                                 AND CAST(Tag.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateTagBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        public static void UserEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserEquipmentLazyLoadHandler += (userId) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserEquipmentBLL().GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
                    }
                    catch { return null; }
                };
            }
        }

        
        #endregion

        #region 送样管理
        /// <summary>
        /// 样品编号延迟加载
        /// </summary>
        public static void SampleApplyNumbersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyNumbersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyNumbersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyNumberBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()), null, "RowNo");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单附件延迟加载
        /// </summary>
        public static void SampleApplyAttachmentsHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyAttachmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyAttachmentsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyAttachmentBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单项目结果延迟加载
        /// </summary>
        public static void SampleApplySampleItemResultsLazyHanderSetUp()
        {
            if (LazyLoadDefinition.SampleApplySampleItemResultsLazyLoadHander == null)
            {
                LazyLoadDefinition.SampleApplySampleItemResultsLazyLoadHander += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleItemResultBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单延迟加载
        /// </summary>
        public static void SampleApplyChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyChargeItemsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyChargeItemBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 测试记录延迟加载
        /// </summary>
        public static void SampleTestRecordLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleTestRecordLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleTestRecordLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleTestRecordBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单延迟加载
        /// </summary>
        public static void SampleApplyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单测试人延迟加载
        /// </summary>
        public static void SampleApplyTesterLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyTestersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyTestersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleTesterBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 样品项目延迟加载
        /// </summary>
        public static void SampleItemLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleItemLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleItemLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleItemBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样项目相关联项目延迟加载
        /// </summary>
        public static void RelateSampleItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.RelateSampleItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.RelateSampleItemsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSampleItemBLL().GetEntitiesByExpression(string.Format("RelationId=\"{0}\"*IsDelete=false", id));
                    };
            }
        }
        /// <summary>
        /// 样品项目形态延迟加载
        /// </summary>
        public static void SampleItemSampleItemStatusesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleItemSampleItemStatusesLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleItemSampleItemStatusesLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleItemStatusBLL().GetEntitiesByExpression(string.Format("SampleItemId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 样品形态延迟加载
        /// </summary>
        public static void SampleStatusLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleStatusLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleStatusLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleStatusBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 收费项目延迟加载
        /// </summary>
        public static void SampleChargeItemLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleChargeItemLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleChargeItemLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleChargeItemBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 样品收费明细项目收费项目延迟加载
        /// </summary>
        public static void SampleChargeItemRelateSampleChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleChargeItemRelateSampleChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleChargeItemRelateSampleChargeItemsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleChargeItemRelateBLL().GetEntitiesByExpression(string.Format("SampleItemId=\"{0}\"", id.ToString())).Select(p => p.SampleChargeItem).ToList();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单延迟加载
        /// </summary>
        public static void SampleApplyChargeItemLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyChargeItemLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyChargeItemLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyChargeItemBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 送样申请单项目延迟加载
        /// </summary>
        public static void SampleApplyItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyItemsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyBLL().GetEntitiesByExpression(string.Format("RelateId=\"{0}\"", id.ToString())).Select(p => p.SampleItem).ToList();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 送样申请单参与实际测试人员延迟加载
        /// </summary>
        public static void SampleApplyJoinedTesterLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyTestRecordsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyTestRecordsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleTestRecordBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 申请单项目结果附件延迟加载
        /// </summary>
        public static void SampleApplyAnalysisAttachmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyAnalysisAttachmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyAnalysisAttachmentsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleAnalysisAttachmentBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 样品状态相关样品项目延迟
        /// </summary>
        public static void SampleStatusSampleItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleStatusSampleItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleStatusSampleItemsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var sampleStatusIds = BLLFactory.CreateViewEquipmentSampleItemQueryBLL().GetEntitiesByExpression(string.Format("SampleStatusId=\"{0}\"", id.ToString()));
                        return sampleStatusIds.Select(p => p.SampleItem).ToList();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 样品参数设备延迟加载
        /// </summary>
        public static void SampleParameterEquipmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleParameterEquipmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleParameterEquipmentsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSampleParameterEquipmentBLL().GetEntitiesByExpression(string.Format("SampleParameterId=\"{0}\"", id));
                    };
            }
        }
        /// <summary>
        /// 样品参数形态延迟加载
        /// </summary>
        public static void SampleParameterStatusesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleParameterStatusesLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleParameterStatusesLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateSampleParameterStatusBLL().GetEntitiesByExpression(string.Format("SampleParameterId=\"{0}\"", id));
                };
            }
        }
        /// <summary>
        /// 样品参数项目延迟加载
        /// </summary>
        public static void SampleParameterItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleParameterItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleParameterItemsLazyLoadHandler = (id) =>
                    {
                        return BLLFactory.CreateSampleParameterItemBLL().GetEntitiesByExpression(string.Format("SampleParameterId=\"{0}\"", id));
                    };
            }
        }
        /// <summary>
        /// 样品参数延迟加载
        /// </summary>
        public static void SampleParameterLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleParameterLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleParameterLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSampleParameterBLL().GetEntityById(id);
                    };
            }
        }
        /// <summary>
        /// 样品申请单参数延迟加载
        /// </summary>
        public static void SampleApplyParametersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyParametersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyParametersLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSampleApplyParameterBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id));
                    };
            }
        }
        /// <summary>
        /// 送样申请单延迟加载
        /// </summary>
        public static void SampleApplyBySampleNoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyBySampleNoLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyBySampleNoLazyLoadHandler += (sampleNo) =>
                {
                    try
                    {
                        return BLLFactory.CreateSampleApplyBLL().GetFirstOrDefaultEntityByExpression(string.Format("SampleNo=\"{0}\"", sampleNo));
                    }
                    catch { return null; }
                };
            }
        }
        public static void SampleFeedBackAttachmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleFeedBackAttachmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleFeedBackAttachmentsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSampleFeedBackAttachmentBLL().GetEntitiesByExpression(string.Format("SampleApplyId=\"{0}\"", id));
                        }
                        catch { return null; }
                      
                    };
            }
        }
        public static void SampleItemLabelItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleItemLabelItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleItemLabelItemsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSampleItemLabelItemBLL().GetEntitiesByExpression(string.Format("SampleItemLabelId=\"{0}\"", id));
                        }
                        catch { return null; }

                    };
            }
        }
        public static void SampleItemLabelLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleItemLabelLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleItemLabelLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSampleItemLabelBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        public static void SampleItemLabelChargeStandardsLazyLoadHanderSetUp()
        {
            if (LazyLoadDefinition.SampleItemLabelChargeStandardsLazyLoadHander == null)
            {
                LazyLoadDefinition.SampleItemLabelChargeStandardsLazyLoadHander += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateSampleItemLabelChargeStandardBLL().GetEntitiesByExpression(string.Format("SampleItemId=\"{0}\"",id));
                        }
                        catch { return null; }
                    };
            }
        }
        public static void SampleApplyCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyCostDeductLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateCostDeductBLL().GetFirstOrDefaultEntityByExpression(string.Format("SampleApplyId=\"{0}\"", id));
                    };
            }
        }
        public static void SampleApplyStatusNameLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleApplyStatusNameLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleApplyStatusNameLazyLoadHandler += (sampleApplyStatus) =>
                    {
                        return CustomerFactory.GetCustomer().GetSampleApplyStatusName(sampleApplyStatus);
                    };
            }
        }
        public static void SampleChargeStatusNameLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SampleChargeStatusNameLazyLoadHandler == null)
            {
                LazyLoadDefinition.SampleChargeStatusNameLazyLoadHandler += (sampleApplyStatus,sampleChargeStatus) =>
                {
                    return CustomerFactory.GetCustomer().GetSampleChargeStatusName(sampleApplyStatus, sampleChargeStatus);
                };
            }
        }
        #endregion

        #region 辅助字典管理
        /// <summary>
        /// 辅助字典分类项目延迟加载
        /// </summary>
        public static void DictCodeTypeDictCodesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DictCodeTypeDictCodesLazyLoadHandler == null)
            {
                LazyLoadDefinition.DictCodeTypeDictCodesLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeBLL().GetEntitiesByExpression(string.Format("DictCodeTypeId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 辅助字典分类延迟加载
        /// </summary>
        public static void DictCodeTypeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DictCodeTypeLazyLoadHandler == null)
            {
                LazyLoadDefinition.DictCodeTypeLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeTypeBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 辅助字典分类父分类延迟加载
        /// </summary>
        public static void DictCodeTypeParentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DictCodeTypeParentLazyLoadHandler == null)
            {
                LazyLoadDefinition.DictCodeTypeParentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeTypeBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 通过代码获取DictCodeType延迟加载
        /// </summary>
        public static void DictCodeTypeLazyLoadByCodeHandlerSetUp()
        {
            if (LazyLoadDefinition.DictCodeTypeLazyLoadByCodeHandler == null)
            {
                LazyLoadDefinition.DictCodeTypeLazyLoadByCodeHandler += (code) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeTypeBLL().GetEntitiesByExpression(string.Format("Code=\"{0}\"", code)).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 获取DictCode延迟加载
        /// </summary>
        public static void DictCodeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DictCodeLazyLoadHandler == null)
            {
                LazyLoadDefinition.DictCodeLazyLoadHandler += (Id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", Id)).First();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 设备管理
        public static void EquipmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentsLazyLoadHandler += (ids) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentBLL().GetEntitiesByExpression(ids.ToFormatStr() + "*IsDelete=false", null, "", true);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备相关样品状态延迟加载
        /// </summary>
        public static void EquipmentSampleStatusLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentSampleStatusLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentSampleStatusLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var sampleStatusIds = BLLFactory.CreateViewEquipmentSampleItemQueryBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        return sampleStatusIds.Select(p => p.SampleStatus).Distinct().ToList();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备视图延迟加载
        /// </summary>
        public static void ViewEquipmentLazyLoadHandlerSetUp()
        {
            if(LazyLoadDefinition.ViewEquipmentLazyLoadHandler==null)
            {
                LazyLoadDefinition.ViewEquipmentLazyLoadHandler+=(id)=>
                    {
                        try
                        {
                            return BLLFactory.CreateViewEquipmentBLL().GetEntityById(id);
                        }
                        catch{return null;}
                    };
            }
        }
        /// <summary>
        /// 设备延迟加载
        /// </summary>
        public static void EquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备状态延迟加载
        /// </summary>
        public static void EquipmentCurStatusInfoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCurStatusInfoLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCurStatusInfoLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentBLL().GetEquipmentStatus(id);
                        }
                        catch { return new EquipmentCurStatusInfo(); }
                    };
            }
        }
        /// <summary>
        /// 设备联系人延迟加载
        /// </summary>
        public static void EquipmentLinkMenLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentLinkMenLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentLinkMenLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLinkmanBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备所属于分类延迟加载
        /// </summary>
        public static void EquipmentCategoryBindsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryBindsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryBindsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentCategoryBindBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备负责人延迟加载
        /// </summary>
        public static void EquipmentDirectorsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentDirectorsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentDirectorsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserWorkScopeBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 用户设备延迟加载
        /// </summary>
        public static void EquipmentFocusUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentFocusUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentFocusUsersLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserEquipmentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备默认设置延迟加载
        /// </summary>
        public static void EquipmentDefaultSettingLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentDefaultSettingLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentDefaultSettingLazyLoadHandler += (code) =>
                {
                    try
                    {
                        return BLLFactory.CreateDictCodeTypeBLL().GetEntitiesByExpression(string.Format("Code=\"{0}\"", code)).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备用户标签项延迟加载
        /// </summary>
        public static void EquipmentLabelItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentLabelItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentLabelItemsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLabelItemBLL().GetEntitiesByExpression(string.Format("EquipmentLabelId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户标签延迟加载
        /// </summary>
        public static void EquipmentLabelLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentLabelLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentLabelLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLabelBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户标签延迟加载
        /// </summary>
        public static void EquipmentLabelsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentLabelsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentLabelsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLabelBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备计费规则延迟加载
        /// </summary>
        public static void EquipmentCalcFeeTimeRuleLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCalcFeeTimeRuleLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCalcFeeTimeRuleLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCalcFeeTimeRuleBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString())).First();
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户标签附加收费项目收费标准延迟加载
        /// </summary>
        public static void EquipmentLabelAddtionChargeItemsLazyLoadHanderSetUp()
        {
            if (LazyLoadDefinition.EquipmentLabelAddtionChargeItemsLazyLoadHander == null)
            {
                LazyLoadDefinition.EquipmentLabelAddtionChargeItemsLazyLoadHander += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLabelAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户标签收费标准延迟加载
        /// </summary>
        public static void EquipmentLabelChargeStandardsLazyLoadHanderSetUp()
        {
            if (LazyLoadDefinition.EquipmentLabelChargeStandardsLazyLoadHander == null)
            {
                LazyLoadDefinition.EquipmentLabelChargeStandardsLazyLoadHander += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentLabelChargeStandardBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()),null);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备收费标准延迟加载
        /// </summary>
        public static void EquipmentChargeStandardLazyLoadHandlerSetUp()
        {
           if(LazyLoadDefinition.EquipmentChargeStandardLazyLoadHandler==null)
           {
               LazyLoadDefinition.EquipmentChargeStandardLazyLoadHandler+=(id)=>
                   {
                       try

                       {
                           return BLLFactory.CreateChargeStandardBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",id.ToString())).First();
                       }
                       catch{return null;}
                   };
           }
        }
        /// <summary>
        /// 设备附加收费项目延迟加载
        /// </summary>
        public static void EquipmentAdditionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentAdditionChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentAdditionChargeItemsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentAdditionChargeItemBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*IsDelete=false", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备黑名单延迟加载
        /// </summary>
        public static void EquipmentBlackListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentBlackListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentBlackListLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentBlackListBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备黑名单消息通知模板延迟加载
        /// </summary>
        public static void EquipmentBlackListMessageTemplateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentBlackListMessageTemplateLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentBlackListMessageTemplateLazyLoadHandler += (equipmentId) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentBlackListMessageTemplateBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",equipmentId.ToString())).First();
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备黑名单标签延迟加载
        /// </summary>
        public static void EquipmentBlackListLabelsHandlerLazyLoadHandlerSetup()
        {
            if (LazyLoadDefinition.EquipmentBlackListLabelsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentBlackListLabelsLazyLoadHandler += (id, equipmentLabelType) =>
                    {
                        try
                        {
                            var equipmentBlackListBLL = BLLFactory.CreateEquipmentBlackListBLL();
                            var equipmentBlackLists = equipmentBlackListBLL.GetEntitiesByExpression(string.Format("RelationId=\"{0}\"+Id=\"{0}\"", id.ToString()));
                            if (equipmentBlackLists == null || equipmentBlackLists.Count == 0) return null;
                            switch (equipmentLabelType)
                             {
                                case LabelType.Organization:
                                     return BLLFactory.CreateLabOrganizationBLL().GetEntitiesByExpression(equipmentBlackLists.Select(p => p.LabelId).ToFormatStr());
                                case LabelType.Suject:
                                     return BLLFactory.CreateSubjectBLL().GetEntitiesByExpression(equipmentBlackLists.Select(p => p.LabelId).ToFormatStr());
                                case LabelType.Tag:
                                    return BLLFactory.CreateTagBLL().GetEntitiesByExpression(equipmentBlackLists.Select(p => p.LabelId).ToFormatStr());
                                case LabelType.User:
                                    return BLLFactory.CreateUserBLL().GetEntitiesByExpression(equipmentBlackLists.Select(p => p.LabelId).ToFormatStr());
                            }
                        }
                        catch { return null; }
                        return null;
                    };
            }
        }
        /// <summary>
        /// 设备公告附件延迟加载
        /// </summary>
        public static void EquipmentNoticeAttachmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentNoticeAttachmentsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentNoticeAttachmentsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentNoticeAttachmentBLL().GetEntitiesByExpression(string.Format("EquipmentNoticeId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备公告延迟加载
        /// </summary>
        public static void EquipmentNoticesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentNoticesLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentNoticesLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentNoticeBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备培训延迟加载
        /// </summary>
        public static void EquipmentTrainningLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentTrainningLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentTrainningLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentTranningBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备培训延迟加载
        /// </summary>
        public static void EquipmentTrainningsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentTrainningsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentTrainningsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentTranningBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备不可预约时间段延迟加载
        /// </summary>
        public static void EquipmentUnAppointmentPeriodTimesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentUnAppointmentPeriodTimesLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentUnAppointmentPeriodTimesLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUnAppointmentPeriodTimeBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户预约时间限制延迟加载
        /// </summary>
        public static void EquipmentUserEquipmentTimeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentUserEquipmentTimesLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentUserEquipmentTimesLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserEquipmentTimeBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户预约限制延迟加载
        /// </summary>
        public static void EquipmentAppointmentTimeLimitUserLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentAppointmentTimeLimitUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentAppointmentTimeLimitUsersLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentTimeLimitUserBLL().GetEntitiesByExpression(string.Format("AppointmentTimeLimitId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备用户类型可预约时间端延迟加载
        /// </summary>
        public static void EquipmentTagEquipmentFunsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentTagEquipmentFunsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentTagEquipmentFunsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateTagEquipmentFunBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备特殊预约规则延迟加载
        /// </summary>
        public static void EquipmentAppointmentSpeciaRuleLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentAppointmentSpeciaRuleLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentAppointmentSpeciaRuleLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentSpeciaRuleBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString())).First();
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备评分延迟加载
        /// </summary>
        public static void EquipmentMarkingLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentMarkingLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentMarkingLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEuipmentMarkingBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备故障报告延迟加载
        /// </summary>
        public static void EquipmentBrokenReportLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentBrokenReportLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentBrokenReportLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentBrokenReportBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备维修记录延迟加载
        /// </summary>
        public static void EquipmentRepairLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRepairLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRepairLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentRepairBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备维修记录延迟加载
        /// </summary>
        public static void EquipmentRepairsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRepairsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRepairsLazyLoadHandler+=(id)=>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentRepairBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备状态日志记录延迟加载
        /// </summary>
        public static void EquipmentStatusLogsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentStatusLogsLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentStatusLogsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentStatusLogBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备平路记录延迟加载
        /// </summary>
        public static void EquipmentMarkingsLazyLoadHandlerSetUp()
        {
            if(LazyLoadDefinition.EquipmentMarkingsLazyLoadHandler==null)
            {
                LazyLoadDefinition.EquipmentMarkingsLazyLoadHandler+=(id)=>
                    {
                        try
                        {
                            return BLLFactory.CreateEuipmentMarkingBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",id.ToString()));
                        }
                        catch{return null;}
                    };
            }
        }
        public static void EquipmentPartLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentPartLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentPartLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentPartBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 设备附加收费项目延迟加载
        /// </summary>
        public static void EquipmentAdditionChargeItemLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentAdditionChargeItemLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentAdditionChargeItemLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateEquipmentAdditionChargeItemBLL().GetEntityById(id);
                        }
                        catch (System.Exception ex)
                        {
                            return null;
                        }
                    };
            }
        }
        /// <summary>
        /// 单位下设备数量延迟加载
        /// </summary>
        public static void EquipmentCountByOrgXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCountByOrgXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCountByOrgXPathLazyLoadHandler += (orgXPath) =>
                    {
                        try
                        {
                            var viewEquipmentRelationList = BLLFactory.CreateViewEquipmentRelationBLL().GetEntitiesByExpression(string.Format("RelationOrgXPath→\"{0}\"*Status=\"{1}\"", orgXPath, (int)EquipmentRelationStatus.AuditPass));
                            var equipmentRelationQueryExpression = "";
                            if (viewEquipmentRelationList != null && viewEquipmentRelationList.Count() > 0)
                            {
                                equipmentRelationQueryExpression = "+" + viewEquipmentRelationList.Select(p => p.EquipmentId).ToFormatStr();
                            }
                            return BLLFactory.CreateViewEquipmentBLL().CountModelListByExpression(string.Format("(OrgXPath→\"{0}\"{1})*IsDelete=false*IsStop=false*IsOpen=true", orgXPath, equipmentRelationQueryExpression));
                        }
                        catch (System.Exception ex)
                        {
                            return 0;
                        }
                    };
            }
        }
        /// <summary>
        /// 单位下免税设备数量延迟加载
        /// </summary>
        public static void EquipmentDutyFreeCountByOrgXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentDutyFreeCountByOrgXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentDutyFreeCountByOrgXPathLazyLoadHandler += (orgXPath) =>
                {
                    try
                    {
                        var viewEquipmentRelationList = BLLFactory.CreateViewEquipmentRelationBLL().GetEntitiesByExpression(string.Format("RelationOrgXPath→\"{0}\"*Status=\"{1}\"", orgXPath, (int)EquipmentRelationStatus.AuditPass));
                        var equipmentRelationQueryExpression = "";
                        if (viewEquipmentRelationList != null && viewEquipmentRelationList.Count() > 0)
                        {
                            equipmentRelationQueryExpression = "+" + viewEquipmentRelationList.Select(p => p.EquipmentId).ToFormatStr();
                        }
                        return BLLFactory.CreateViewEquipmentBLL().CountModelListByExpression(string.Format("(OrgXPath→\"{0}\"{1})*IsDelete=false*IsStop=false*IsOpen=true*IsDutyFree=true", orgXPath, equipmentRelationQueryExpression));
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 子单位下设备数量延迟加载
        /// </summary>
        public static void EquipmentChildCountByOrgXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentChildCountByOrgXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentChildCountByOrgXPathLazyLoadHandler += (orgXPath) =>
                {
                    try
                    {
                        var count = BLLFactory.CreateViewEquipmentBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewEquipment WHERE ViewEquipment.OrgXPath LIKE '{0}%' AND ViewEquipment.OrgXPath<>'{0}' AND ViewEquipment.IsDelete=0 AND ViewEquipment.IsStop=0 AND ViewEquipment.IsOpen=1", orgXPath));
                        return count == null ? 0 : int.Parse(count.ToString());
                        //var viewEquipmentList = BLLFactory.CreateViewEquipmentBLL().GetEntitiesByExpression(string.Format("OrgXPath→\"{0}\"*OrgXPath=-\"{0}\"*IsDelete=false*IsStop=false", orgXPath), null, "", true);
                        //return viewEquipmentList == null ? 0 : viewEquipmentList.Count();
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 地点下设备数量延迟加载
        /// </summary>
        public static void EquipmentCountByRoomXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCountByRoomXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCountByRoomXPathLazyLoadHandler += (roomXPath) =>
                    {
                        try
                        {
                            var count = BLLFactory.CreateViewEquipmentBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewEquipment WHERE ViewEquipment.RoomXPath LIKE '{0}%' AND ViewEquipment.IsDelete=0 AND ViewEquipment.IsStop=0", roomXPath));
                            return count == null ? 0 : int.Parse(count.ToString());
                            //var viewEquipmentList = BLLFactory.CreateViewEquipmentBLL().GetEntitiesByExpression(string.Format("RoomXPath→\"{0}\"*IsDelete=false*IsStop=false", roomXPath), null, "", true);
                            //return viewEquipmentList == null ?  0 : viewEquipmentList.Count();
                        }
                        catch (System.Exception ex)
                        {
                            return 0;
                        }
                    };
            }
        }
        /// <summary>
        /// 地点下免税设备数量延迟加载
        /// </summary>
        public static void EquipmentDutyFreeCountByRoomXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentDutyFreeCountByRoomXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentDutyFreeCountByRoomXPathLazyLoadHandler += (roomXPath) =>
                {
                    try
                    {
                        var count = BLLFactory.CreateViewEquipmentBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewEquipment WHERE ViewEquipment.RoomXPath LIKE '{0}%' AND ViewEquipment.IsDelete=0 AND IsDutyFree = 1 AND ViewEquipment.IsStop=0", roomXPath));
                        return count == null ? 0 : int.Parse(count.ToString());
                        //var viewEquipmentList = BLLFactory.CreateViewEquipmentBLL().GetEntitiesByExpression(string.Format("RoomXPath→\"{0}\"*IsDelete=false*IsStop=false", roomXPath), null, "", true);
                        //return viewEquipmentList == null ?  0 : viewEquipmentList.Count();
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 子地点下设备数量延迟加载
        /// </summary>
        public static void EquipmentChildCountByRoomXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentChildCountByRoomXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentChildCountByRoomXPathLazyLoadHandler += (roomXPath) =>
                {
                    try
                    {
                        var count = BLLFactory.CreateViewEquipmentBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewEquipment WHERE ViewEquipment.RoomXPath LIKE '{0}%' AND RoomXPath<>'{0}' AND ViewEquipment.IsDelete=0 AND ViewEquipment.IsStop=0", roomXPath));
                        return count == null ? 0 : int.Parse(count.ToString());
                        //var viewEquipmentList = BLLFactory.CreateViewEquipmentBLL().GetEntitiesByExpression(string.Format("RoomXPath→\"{0}\"*RoomXPath=-\"{0}\"*IsDelete=false*IsStop=false", roomXPath), null, "", true);
                        //return viewEquipmentList == null ? 0 : viewEquipmentList.Count();
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 控制方式下设备数量延迟加载
        /// </summary>
        public static void EquipmentCountByControllerTypeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCountByControllerTypeLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCountByControllerTypeLazyLoadHandler += (controllerType) =>
                    {
                        try
                        {
                            var count = BLLFactory.CreateViewEquipmentBLL().GetSingleResult(string.Format("SELECT count(*) FROM ViewEquipment WHERE ViewEquipment.ControllerType='{0}' AND ViewEquipment.IsDelete=0 AND ViewEquipment.IsStop=0", controllerType));
                            return count == null ? 0 : int.Parse(count.ToString());
                            //var viewEquipmentList = BLLFactory.CreateViewEquipmentBLL().GetEntitiesByExpression(string.Format("ControllerType=\"{0}\"*IsDelete=false*IsStop=false", controllerType), null, "", true);
                            //return viewEquipmentList == null ?  0 : viewEquipmentList.Count();
                        }
                        catch (System.Exception ex)
                        {
                            return 0;
                        }
                    };
            }
        }
        /// <summary>
        /// 设备分类下设备数量延迟加载
        /// </summary>
        public static void EquipmentCountByCategoryXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCountByCategoryXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCountByCategoryXPathLazyLoadHandler += (categoryXPath) =>
                    {
                        try
                        {
                            var count = BLLFactory.CreateViewEquipmentCategoryBindBLL().GetSingleResult(string.Format("SELECT COUNT(DISTINCT EquipmentId) FROM ViewEquipmentCategoryBind WHERE CategoryXPath LIKE '{0}%' AND CategoryIsDelete=0 AND CategoryIsStop=0 AND EquipmenIsDelete = 0 AND EquipmenIsStop=0 AND EquipmenIsOpen=1", categoryXPath));
                            return count == null ? 0 : int.Parse(count.ToString());
                            //var viewEquipmentCategoryBindList = BLLFactory.CreateViewEquipmentCategoryBindBLL().GetEntitiesByExpression(string.Format("CategoryXPath→\"{0}\"*CategoryIsDelete=false*CategoryIsStop=false*EquipmenIsDelete=false*EquipmenIsStop=false", categoryXPath), null, "", true);
                            //return viewEquipmentCategoryBindList == null ? 0 : viewEquipmentCategoryBindList.Select(p=>p.EquipmentId).Distinct().Count();
                        }
                        catch (System.Exception ex)
                        {
                            return 0;
                        }
                    };
            }
        }
        /// <summary>
        /// 设备分类下免税设备数量延迟加载
        /// </summary>
        public static void EquipmentDutyFreeCountByCategoryXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentDutyFreeCountByCategoryXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentDutyFreeCountByCategoryXPathLazyLoadHandler += (categoryXPath) =>
                {
                    try
                    {
                        var count = BLLFactory.CreateViewEquipmentCategoryBindBLL().GetSingleResult(string.Format("SELECT COUNT(DISTINCT EquipmentId) FROM ViewEquipmentCategoryBind WHERE CategoryXPath LIKE '{0}%' AND CategoryIsDelete=0 AND CategoryIsStop=0 AND EquipmentIsDutyFree = 1 AND EquipmenIsDelete = 0 AND EquipmenIsStop=0 AND EquipmenIsOpen=1", categoryXPath));
                        return count == null ? 0 : int.Parse(count.ToString());                       
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 设备子分类下设备数量延迟加载
        /// </summary>
        public static void EquipmentChildCountByCategoryXPathLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentChildCountByCategoryXPathLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentChildCountByCategoryXPathLazyLoadHandler += (categoryXPath) =>
                {
                    try
                    {
                        var count = BLLFactory.CreateViewEquipmentCategoryBindBLL().GetSingleResult(string.Format("SELECT COUNT(*) FROM ViewEquipmentCategoryBind WHERE CategoryXPath LIKE '{0}%' AND CategoryXPath<>'{0}' AND CategoryIsDelete=0 AND CategoryIsStop=0 AND EquipmenIsDelete = 0 AND EquipmenIsStop=0 AND EquipmenIsOpen=1", categoryXPath));
                        return count == null ? 0 : int.Parse(count.ToString());
                        //var viewEquipmentCategoryBindList = BLLFactory.CreateViewEquipmentCategoryBindBLL().GetEntitiesByExpression(string.Format("CategoryXPath→\"{0}\"*CategoryXPath=-\"{0}\"*CategoryIsDelete=false*CategoryIsStop=false*EquipmenIsDelete=false*EquipmenIsStop=false", categoryXPath), null, "", true);
                        //return viewEquipmentCategoryBindList == null ? 0 : viewEquipmentCategoryBindList.Select(p => p.EquipmentId).Distinct().Count();
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                };
            }
        }
        /// <summary>
        /// 设备预约汇总信息延迟加载
        /// </summary>
        public static void EquipmentTotalInfoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentTotalInfoLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentTotalInfoLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentBLL().GetEquipmentTotalInfo(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备运行参数延迟加载
        /// </summary>
        public static void EquipmentUseConditionLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentUseConditionLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentUseConditionLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEquipmentUseConditionBLL().GetEntityById(id);
                    };
            }
        }
        /// <summary>
        /// 设备关联机构列表延迟加载
        /// </summary>
        public static void EquipmentRelationListByEquipmentIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRelationListByEquipmentIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRelationListByEquipmentIdLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateEquipmentRelationBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"",id));
                };
            }
        }
        /// <summary>
        /// 设备联系人延迟加载
        /// </summary>
        public static void EquipmentAchievementStudentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentAchievementStudentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentAchievementStudentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAchievementStudentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备联系人延迟加载
        /// </summary>
        public static void EquipmentViewAchievementStudentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewAchievementStudentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewAchievementStudentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewAchievementStudentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联论文列表延迟加载
        /// </summary>
        public static void EquipmentViewThesisLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewThesisLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewThesisLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewThesisBLL().GetFirstOrDefaultEntityByExpression(string.Format("ThesisId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联专利列表延迟加载
        /// </summary>
        public static void EquipmentViewPatentisLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewPatentisLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewPatentisLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewPatentBLL().GetFirstOrDefaultEntityByExpression(string.Format("PatentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联论文列表延迟加载
        /// </summary>
        public static void EquipmentViewThesisEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewThesisEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewThesisEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewThesisEquipmentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联专利列表延迟加载
        /// </summary>
        public static void EquipmentViewPatentEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewPatentEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewPatentEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewPatentEquipmentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联科研项目列表延迟加载
        /// </summary>
        public static void EquipmentViewSubjectAchievementLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewSubjectAchievementLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewSubjectAchievementLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewSubjectAchievementBLL().GetFirstOrDefaultEntityByExpression(string.Format("SubjectAchievementId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联科研项目列表延迟加载
        /// </summary>
        public static void EquipmentViewSubjectEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentViewSubjectEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentViewSubjectEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewSubjectEquipmentBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联实验室主任列表延迟加载
        /// </summary>
        public static void EquipmentRoomDirectorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRoomDirectorLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRoomDirectorLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentRoomDirectorBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备关联负责教授列表延迟加载
        /// </summary>
        public static void EquipmentProfessorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentProfessorLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentProfessorLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentProfessorBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 仪器组群
        public static void EquipmentGroupLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentGroupLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentGroupLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateEquipmentGroupBLL().GetEntityById(id);
                };
            }
        }

        public static void EquipmentGroupMembersLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentGroupMembersLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentGroupMembersLoadHandler += (id) =>
                {
                    return BLLFactory.CreateEquipmentGroupMemberBLL().GetEntitiesByExpression(string.Format("EquipmentGroupId=\"{0}\"", id));
                };
            }
        }
        public static void EquipmentGroupAdminsLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentGroupAdminsLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentGroupAdminsLoadHandler += (id) =>
                {
                    return BLLFactory.CreateEquipmentGroupAdminBLL().GetEntitiesByExpression(string.Format("EquipmentGroupId=\"{0}\"", id));
                };
            }

        }
        public static void EquipmentGroupAdminIdListLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentGroupAdminIdListLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentGroupAdminIdListLoadHandler += (id) =>
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.AppendFormat(@"SELECT DISTINCT EquipmentGroupAdmin.AdminId
                                FROM Equipment INNER JOIN EquipmentGroupMember ON Equipment.EquipmentId = EquipmentGroupMember.EquipmentId
                                INNER JOIN EquipmentGroupAdmin ON EquipmentGroupAdmin.EquipmentGroupId = EquipmentGroupMember.EquipmentGroupId
                                WHERE Equipment.EquipmentId='{0}'", id);
                    var result = BLLFactory.CreateEquipmentGroupAdminBLL().GetMutiResult(sbSql.ToString());
                    IList<Guid> adminIdList = new List<Guid>();
                    if (result != null)
                    {
                        foreach (var item in (System.Collections.IList)result)
                            adminIdList.Add(new Guid(item.ToString()));
                    }
                    return adminIdList;
                };
            }
        }
        #endregion

        #region 设备分类管理
        /// <summary>
        /// 设备分类延迟加载
        /// </summary>
        public static void EquipmentCategoryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentCategoryBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 下级分类数延迟加载
        /// </summary>
        public static void EquipmentCategoryChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateEquipmentCategoryBLL().GetSingleResult(string.Format("select count(*) from EquipmentCategory where XPath like '{0}%' and IsDelete=0", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 设备分类是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void EquipmentCategoryIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM EquipmentCategory WHERE XPath <>'{0}'  and IsDelete=0
                                                 AND len(EquipmentCategory.XPath) = len('{0}')
                                                 AND CAST(EquipmentCategory.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND EquipmentCategory.ParentId IS NULL" : " AND EquipmentCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateEquipmentCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 实验室机构是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void EquipmentCategoryIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM EquipmentCategory WHERE XPath <>'{0}'
                                                 and IsDelete=0
                                                 AND len(EquipmentCategory.XPath) = len('{0}')
                                                 AND CAST(EquipmentCategory.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND EquipmentCategory.ParentId IS NULL" : " AND EquipmentCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateEquipmentCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        #endregion

        #region 实验室机构管理
        /// <summary>
        /// 实验室机构延迟加载
        /// </summary>
        public static void LabOrganizationLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateLabOrganizationBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        ///实验室机构孩子机构个数延迟加载
        /// </summary>
        public static void LabOrganizationChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationChildrenCountLazyLoadHandler += (xPath) =>
                    {
                        try
                        {
                            return Convert.ToInt32(BLLFactory.CreateLabOrganizationBLL().GetSingleResult(string.Format("select count(*) from LabOrganization where XPath like '{0}%'  and IsDelete=0 ", xPath))) - 1;
                        }
                        catch { return 0; }
                    };
            }
        }
        /// <summary>
        /// 实验室机构是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void LabOrganizationIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationIsEnableUpOperateLazyLoadHandler += (xPath,parentId) =>
                    {
                        try
                        {
                            StringBuilder sbSql = new StringBuilder();
                            sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM LabOrganization WHERE XPath <>'{0}'
                                                 and IsDelete=0
                                                 AND len(LabOrganization.XPath) = len('{0}')
                                                 AND CAST(LabOrganization.XPath AS BIGINT)<CAST('{0}' AS BIGINT)", xPath);
                                                 //AND CAST(LabOrganization.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                            sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid)? " AND LabOrganization.ParentId IS NULL" : " AND LabOrganization.ParentId='" + parentId.Value.ToString() + "'");
                            var result = BLLFactory.CreateLabOrganizationBLL().GetSingleResult(sbSql.ToString());
                            return Convert.ToInt32(result) > 0;
                        }
                        catch { return false; }
                    };
            }
        }
        /// <summary>
        /// 实验室机构是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void LabOrganizationIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM LabOrganization WHERE XPath <>'{0}'
                                                 and IsDelete=0
                                                 AND len(LabOrganization.XPath) = len('{0}')
                                                 AND CAST(LabOrganization.XPath AS BIGINT)>CAST('{0}' AS BIGINT)", xPath);
                                                 //AND CAST(LabOrganization.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND LabOrganization.ParentId IS NULL" : " AND LabOrganization.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateLabOrganizationBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }

        /// <summary>
        /// 获取机构授权列表
        /// </summary>
        public static void LabOrganizationAuthorizedListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationAuthorizedListLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationAuthorizedListLazyLoadHandler += (businessId) =>
                {
                    try
                    {
                        return BLLFactory.CreateLabOrganizationAuthorizedBLL().GetEntitiesByExpression(string.Format("BusinessId=\"{0}\"", businessId));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 机构管理员延迟加载
        /// </summary>
        public static void LabOrganizationDirectorsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.LabOrganizationDirectorsLazyLoadHandler == null)
            {
                LazyLoadDefinition.LabOrganizationDirectorsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateLabOrganizationDirectorBLL().GetEntitiesByExpression(string.Format("LabOrganizationId=\"{0}\"", id));
                    };
            }
        }
        #endregion

        #region 成果管理延迟加载
        /// <summary>
        /// 附件延迟加载
        /// </summary>
        public static void AttachmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AttachmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.AttachmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAttachmentBLL().GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 科研项目成员延迟加载
        /// </summary>
        public static void SubjectAchievementMemberListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectAchievementMemberListLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectAchievementMemberListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectMemberBLL().GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 科研项目单位延迟加载
        /// </summary>
        public static void SubjectAchievementDepartmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectAchievementDepartmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectAchievementDepartmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectDepartmentBLL().GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 科研项目设备延迟加载
        /// </summary>
        public static void SubjectEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectEquipmentBLL().GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 论文
        /// </summary>
        public static void ThesisLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ThesisLazyLoadHandler == null)
            {
                LazyLoadDefinition.ThesisLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateThesisBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }


        /// <summary>
        /// 论文第一作者延迟加载
        /// </summary>
        public static void FirstThesisAuthorListLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.FirstThesisAuthorListLazyLoadHandler == null)
            {
                LazyLoadDefinition.FirstThesisAuthorListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateThesisAuthorBLL().GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"*AuthorType=\"FirstThesisAuthor\"", id.ToString()), null, "AuthorOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 论文通讯作者延迟加载
        /// </summary>
        public static void OtherThesisAuthorListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.OtherThesisAuthorListLazyLoadHandler == null)
            {
                LazyLoadDefinition.OtherThesisAuthorListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateThesisAuthorBLL().GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"*AuthorType=\"OtherThesisAuthor\"", id.ToString()), null, "AuthorOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 论文使用设备延迟加载
        /// </summary>
        public static void ThesisEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ThesisEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ThesisEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateThesisEquipmentBLL().GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"", id.ToString()), null, "EquipmentOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 论文单位延迟加载
        /// </summary>
        public static void ThesisDepartmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ThesisDepartmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ThesisDepartmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateThesisDepartmentBLL().GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"", id.ToString()), null, "DepartmentOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 专利
        /// </summary>
        public static void PatentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PatentLazyLoadHandler == null)
            {
                LazyLoadDefinition.PatentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreatePatentBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 专利人员延迟加载
        /// </summary>
        public static void PatentUserListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PatentUserListLazyLoadHandler == null)
            {
                LazyLoadDefinition.PatentUserListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreatePatentUserBLL().GetEntitiesByExpression(string.Format("PatentId=\"{0}\"", id.ToString()), null, "UserOrder");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 专利设备延迟加载
        /// </summary>
        public static void PatentEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PatentEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.PatentEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreatePatentEquipmentBLL().GetEntitiesByExpression(string.Format("PatentId=\"{0}\"", id.ToString()), null, "EquipmentOrder");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 获奖成果延迟加载
        /// </summary>
        public static void AwardsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AwardsLazyLoadHandler == null)
            {
                LazyLoadDefinition.AwardsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAwardsBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 获奖成果作者延迟加载
        /// </summary>
        public static void AwardsAuthorListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AwardsAuthorListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AwardsAuthorListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAwardsAuthorBLL().GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", id.ToString()), null, "AuthorOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 获奖成果单位延迟加载
        /// </summary>
        public static void AwardsDepartmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AwardsDepartmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AwardsDepartmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAwardsDepartmentBLL().GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", id.ToString()), null, "DepartmentOrder");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 获奖成果设备延迟加载
        /// </summary>
        public static void AwardsEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AwardsEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.AwardsEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAwardsEquipmentBLL().GetEntitiesByExpression(string.Format("AwardsId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 学术交流人员Local延迟加载
        /// </summary>
        public static void AcademicExchangesUserLocalListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AcademicExchangesUserLocalListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AcademicExchangesUserLocalListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAcademicExchangesUserBLL().GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"*UserType=UserLocal", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 学术交流延迟加载
        /// </summary>
        public static void AcademicExchangesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AcademicExchangesLazyLoadHandler == null)
            {
                LazyLoadDefinition.AcademicExchangesLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAcademicExchangesBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 学术交流人员Out延迟加载
        /// </summary>
        public static void AcademicExchangesUserOutListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AcademicExchangesUserOutListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AcademicExchangesUserOutListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAcademicExchangesUserBLL().GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"*UserType=UserOut", id.ToString()), null, "UserOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 学术交流单位Local延迟加载
        /// </summary>
        public static void AcademicExchangesDepLocalListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AcademicExchangesDepLocalListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AcademicExchangesDepLocalListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAcademicExchangesDepBLL().GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"*DepartmentType=DepartmentLocal", id.ToString()), null, "DepartmentOrder");
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 学术交流单位Out延迟加载
        /// </summary>
        public static void AcademicExchangesDepOutListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AcademicExchangesDepOutListLazyLoadHandler == null)
            {
                LazyLoadDefinition.AcademicExchangesDepOutListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAcademicExchangesDepBLL().GetEntitiesByExpression(string.Format("AcademicExchangesId=\"{0}\"*DepartmentType=DepartmentOut", id.ToString()), null, "DepartmentOrder");
                    }
                    catch { return null; }
                };
            }
        }

        #endregion
         
        #region 交流天地
        /// <summary>
        /// 原贴的评论列表定义实现
        /// </summary>
        public static void ExchangeFairylandCommentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ExchangeFairylandCommentLazyLoadHandler == null)
            {
                LazyLoadDefinition.ExchangeFairylandCommentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateExchangeFairylandCommentBLL().GetEntitiesByExpression(string.Format("ExchangeFairylandId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 原贴的创建人定义实现
        /// </summary>
        public static void ExchangeFairylandCreatorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ExchangeFairylandCreatorLazyLoadHandler == null)
            {
                LazyLoadDefinition.ExchangeFairylandCreatorLazyLoadHandler += (CreatorId) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", CreatorId.ToString())).FirstOrDefault();
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 评论的创建人定义实现
        /// </summary>
        public static void ExchangeFairylandCommentCreatorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ExchangeFairylandCommentCreatorLazyLoadHandler == null)
            {
                LazyLoadDefinition.ExchangeFairylandCommentCreatorLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 课题组管理
        /// <summary>
        /// 课题延迟加载
        /// </summary>
        public static void SubjectLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 课题延迟加载
        /// </summary>
        public static void SubjectByDirectorIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectByDirectorIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectByDirectorIdLazyLoadHandler += (subjectDirectorId) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubjectBLL().GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", subjectDirectorId)).First();
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 课题组成员延迟加载
        /// </summary>
        public static void SubjectExperimentersLazyLoadHanlderSetUp()
        {
            if (LazyLoadDefinition.SubjectExperimentersLazyLoadHanlder == null)
            {
                LazyLoadDefinition.SubjectExperimentersLazyLoadHanlder += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateExperimenterSubjectBLL().GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*IsDelete=false", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 课题组视图延迟加载
        /// </summary>
        public static void ViewSubjectLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewSubjectLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewSubjectLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateViewSubjectBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 课题组项目信息列表延迟加载
        /// </summary>
        public static void SubjectProjectsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectProjectsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectProjectsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSubjectProjectBLL().GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"", id));
                    };
            }
        }
        /// <summary>
        /// 课题组项目信息延迟加载
        /// </summary>
        public static void SubjectProjectLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SubjectProjectLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubjectProjectLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateSubjectProjectBLL().GetEntityById(id);
                    };
            }
        }
        
        #endregion

        #region 文章管理
        public static void ArticleCategoryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleCategoryLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateArticleCategoryBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        public static void ArticleInCategoryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleInCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleInCategoryLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateArticleBLL().GetEntitiesByExpression(string.Format("ArticleCategoryId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        public static void ArticleCategoryChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleCategoryChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleCategoryChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateArticleCategoryBLL().GetSingleResult(string.Format("select count(*) from ArticleCategory where IsDelete=0 AND XPath like '{0}%'", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        public static void ArticleCategoryIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleCategoryIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleCategoryIsEnableUpOperateLazyLoadHandler += (xPath, parentId,orgId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ArticleCategory WHERE IsDelete=0 AND OrgId='{1}' AND XPath <>'{0}'
                                                 AND len(ArticleCategory.XPath) = len('{0}')
                                                 AND CAST(ArticleCategory.XPath AS INT)<CAST('{0}' AS INT)", xPath,orgId.ToString());
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ArticleCategory.ParentId IS NULL" : " AND ArticleCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateArticleCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        public static void ArticleCategoryIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleCategoryIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleCategoryIsEnableDownOperateLazyLoadHandler += (xPath, parentId,orgId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ArticleCategory WHERE IsDelete=0 AND OrgId='{1}' AND XPath <>'{0}'
                                                 AND len(ArticleCategory.XPath) = len('{0}')
                                                 AND CAST(ArticleCategory.XPath AS INT)>CAST('{0}' AS INT)", xPath,orgId.ToString());
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND ArticleCategory.ParentId IS NULL" : " AND ArticleCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateArticleCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        public static void ArticleRelationListByArticleIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ArticleRelationListByArticleIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.ArticleRelationListByArticleIdLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateArticleRelationBLL().GetEntitiesByExpression(string.Format("ArticleId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region Q&A
        /// <summary>
        /// 问题的答案延迟加载
        /// </summary>
        public static void AnswerLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnswerLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnswerLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateAnswerBLL().GetEntitiesByExpression(string.Format("QuestionId=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 预约规则管理
        /// <summary>
        /// 预约优先权用户延迟加载
        /// </summary>
        public static void AppointmentPriorityUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentPriorityUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentPriorityUsersLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentPriorityUserBLL().GetEntitiesByExpression(string.Format("AppointmentPriorityId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 用户设备可预约时间段延迟加载
        /// </summary>
        public static void UserEquipmentTimeUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserEquipmentTimeUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserEquipmentTimeUsersLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUserEquipmentTimeUserBLL().GetEntitiesByExpression(string.Format("UserEquipmentTimeId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 节假日不包含设备延迟加载
        /// </summary>
        public static void HolidaysExcludesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.HolidaysExcludesLazyLoadHandler == null)
            {
                LazyLoadDefinition.HolidaysExcludesLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateHolidaysExcludeBLL().GetEntitiesByExpression(string.Format("HolidaysId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 节假日包含设备延迟加载
        /// </summary>
        public static void HolidaysIncludesLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.HolidaysIncludesLazyLoadHandler == null)
            {
                LazyLoadDefinition.HolidaysIncludesLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateHolidaysIncludeBLL().GetEntitiesByExpression(string.Format("HolidaysId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 预约特殊规则用户延迟加载
        /// </summary>
        public static void AppointmentSpeciaRuleUsersLazyLoadSetUp()
        {
            if (LazyLoadDefinition.AppointmentSpeciaRuleUsersLazyLoad == null)
            {
                LazyLoadDefinition.AppointmentSpeciaRuleUsersLazyLoad += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentSpeciaRuleUserBLL().GetEntitiesByExpression(string.Format("AppointmentSpecialRuleId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        #endregion

        #region 预约管理
        public static void AppointmentPredictCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentPredictCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentPredictCostDeductLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCostDeductBLL().GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"", id));
                        }
                        catch { return null; }
                    };
            }
        }
        public static void AppointmentEquipmentPartsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentEquipmentPartsLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentEquipmentPartsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentEquipmentPartBLL().GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                        
                    };
            }
        }
        /// <summary>
        /// 预约设备附加收费项目延迟加载
        /// </summary>
        public static void AppointmentEquipmentAddtionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentEquipmentAddtionChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentEquipmentAddtionChargeItemsLazyLoadHandler += (id) =>
                    {

                        try
                        {
                            return BLLFactory.CreateAppointmentEquipmentAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 预约延迟加载定义
        /// </summary>
        public static void AppointmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateAppointmentBLL().GetEntityById(id);
                        }
                        catch (System.Exception ex)
                        {
                            return null;
                        }
                    };
            }
        }
        public static void AppointmentEquipmentUseConditionsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AppointmentEquipmentUseConditionsLazyLoadHandler == null)
            {
                LazyLoadDefinition.AppointmentEquipmentUseConditionsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateAppointmentEquipmentUseConditionBLL().GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", id));
                    };
            }
        }
        #endregion

        #region 扣费管理
        public static void UsedConfirmLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateUsedConfirmBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 手工扣费延迟加载
        /// </summary>
        public static void ManualCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ManualCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.ManualCostDeductLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateManualCostDeductBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 水控扣费延迟加载
        /// </summary>
        public static void WaterControlCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.WaterControlCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.WaterControlCostDeductLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateWaterControlCostDeductBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }    
   
        /// <summary>
        /// 扣费延迟加载
        /// </summary>
        public static void  CostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.CostDeductLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCostDeductBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }       

        /// <summary>
        /// 扣费日志延迟加载
        /// </summary>
        public static void CostDeductLogsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CostDeductLogsLazyLoadHandler == null)
            {
                LazyLoadDefinition.CostDeductLogsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCostDeductLogBLL().GetEntitiesByExpression(string.Format("CostDeductId=\"{0}\"",id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 使用扣费延迟加载
        /// </summary>
        public static void UsedConfirmCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmCostDeductLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCostDeductBLL().GetFirstOrDefaultEntityByExpression(string.Format("UsedConfirmId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 使用记录反馈延迟加载
        /// </summary>
        public static void UsedConfirmFeedBackLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmFeedBackLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmFeedBackLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateUsedConfirmFeedBackBLL().GetFirstOrDefaultEntityByExpression(string.Format("UsedConfirmId=\"{0}\"", id));
                        }
                        catch { return null; }
                    };
            }
        }
        public static void CostDeductEquipmentOpenFundsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CostDeductEquipmentOpenFundsLazyLoadHandler == null)
            {
                LazyLoadDefinition.CostDeductEquipmentOpenFundsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateCostDeductEquipmentOpenFundBLL().GetEntitiesByExpression(string.Format("CostDeductId=\"{0}\"", id));
                        }
                        catch { return null; }
                    };
            }
        }
        public static void UsedConfirmEquipmentPartsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmEquipmentPartsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmEquipmentPartsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUsedConfirmEquipmentPartBLL().GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }

                };
            }
        }
        /// <summary>
        /// 使用记录设备附加收费项目延迟加载
        /// </summary>
        public static void UsedConfirmEquipmentAddtionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmEquipmentAddtionChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmEquipmentAddtionChargeItemsLazyLoadHandler += (id) =>
                {

                    try
                    {
                        return BLLFactory.CreateUsedConfirmEquipmentAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        public static void UsedConfirmEquipmentUseConditionsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UsedConfirmEquipmentUseConditionsLazyLoadHandler == null)
            {
                LazyLoadDefinition.UsedConfirmEquipmentUseConditionsLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateUsedConfirmEquipmentUseConditionBLL().GetEntitiesByExpression(string.Format("UsedConfirmId=\"{0}\"", id));
                };
            }
        }
        #endregion

        #region 站内消息
        /// <summary>
        /// 收件人
        /// </summary>
        public static void ReceiverListLogsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ReceiverListLogsLazyLoadHandler == null)
            {
                LazyLoadDefinition.ReceiverListLogsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateReceiverBLL().GetEntitiesByExpression(string.Format("MessageId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 不良行为管理
        /// <summary>
        /// 处罚记录延迟加载
        /// </summary>
        public static void PunishRecordLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PunishRecordLazyLoadHandler == null)
            {
                LazyLoadDefinition.PunishRecordLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreatePunishRecordBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 不良行为延迟加载
        /// </summary>
        public static void DelinquencyConfirmsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DelinquencyConfirmsLazyLoadHandler == null)
            {
                LazyLoadDefinition.DelinquencyConfirmsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateDelinquencyConfirmBLL().GetEntitiesByExpression(string.Format("PunishRecordId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 处罚不良行为记录延迟加载
        /// </summary>
        public static void PunishActionDelinquencyConfirmsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PunishActionDelinquencyConfirmsLazyLoadHandler == null)
            {
                LazyLoadDefinition.PunishActionDelinquencyConfirmsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            var punishActionDelinquencyList = BLLFactory.CreatePunishActionDelinquencyBLL().GetEntitiesByExpression(string.Format("PunishActionId=\"{0}\"",id.ToString()));
                            if(punishActionDelinquencyList==null || punishActionDelinquencyList.Count==0) return null;
                            return BLLFactory.CreateDelinquencyConfirmBLL().GetEntitiesByExpression(punishActionDelinquencyList.Select(p => p.DelinquencyConfirmId).ToFormatStr());
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 不良行为处罚延迟加载
        /// </summary>
        public static void DelinquencyConfirmPunishActionsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DelinquencyConfirmPunishActionsLazyLoadHandler == null)
            {
                LazyLoadDefinition.DelinquencyConfirmPunishActionsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            var punishActionDelinquencyList = BLLFactory.CreatePunishActionDelinquencyBLL().GetEntitiesByExpression(string.Format("DelinquencyConfirmId=\"{0}\"", id.ToString()));
                            if (punishActionDelinquencyList == null || punishActionDelinquencyList.Count == 0) return null;
                            return BLLFactory.CreatePunishActionBLL().GetEntitiesByExpression(punishActionDelinquencyList.Select(p => p.PunishActionId).ToFormatStr());
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 不良行为延迟加载
        /// </summary>
        public static void DelinquencyConfirmLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DelinquencyConfirmLazyLoadHandler == null)
            {
                LazyLoadDefinition.DelinquencyConfirmLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateDelinquencyConfirmBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 不良行为记录处罚延迟加载
        /// </summary>
        public static void PunishRecordPunishActionsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PunishRecordPunishActionsLazyLoadHandler == null)
            {
                LazyLoadDefinition.PunishRecordPunishActionsLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreatePunishActionBLL().GetEntitiesByExpression(string.Format("PunishRecordId=\"{0}\"", id.ToString()));
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 取消不良行为延迟加载
        /// </summary>
        public static void RepealDelinquencyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.RepealDelinquencyLazyLoadHandler == null)
            {
                LazyLoadDefinition.RepealDelinquencyLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreateRepealDelinquencyBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        /// <summary>
        /// 处罚延迟加载
        /// </summary>
        public static void PunishActionLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PunishActionLazyLoadHandler == null)
            {
                LazyLoadDefinition.PunishActionLazyLoadHandler += (id) =>
                    {
                        try
                        {
                            return BLLFactory.CreatePunishActionBLL().GetEntityById(id);
                        }
                        catch { return null; }
                    };
            }
        }
        #endregion

        #region 在线培训考试
        /// <summary>
        /// 培训资料是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void TrainningMaterialIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningMaterialIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningMaterialIsEnableUpOperateLazyLoadHandler += (xPath, trainningTypeId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningMaterial WHERE XPath <>'{0}'
                                                 AND len(TrainningMaterial.XPath) = len('{0}')
                                                 AND CAST(TrainningMaterial.XPath AS INT)<CAST('{0}' AS INT)
                                                 AND TrainningTypeId='{1}'
                                                 AND IsDelete=0", xPath, trainningTypeId);
                        var result = BLLFactory.CreateTrainningMaterialBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 培训资料是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void TrainningMaterialIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningMaterialIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningMaterialIsEnableDownOperateLazyLoadHandler += (xPath, trainningTypeId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningMaterial WHERE XPath <>'{0}'
                                                 AND len(TrainningMaterial.XPath) = len('{0}')
                                                 AND CAST(TrainningMaterial.XPath AS INT)>CAST('{0}' AS INT)
                                                 AND TrainningTypeId='{1}'
                                                 AND IsDelete=0", xPath, trainningTypeId);
                        var result = BLLFactory.CreateTrainningMaterialBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 获取考题
        /// </summary>
        public static void ViewTrainningQuestionLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewTrainningQuestionLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewTrainningQuestionLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewTrainningQuestionBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 根据试卷获取当前考题
        /// </summary>
        public static void TrainningQuestionListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionListLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionListLazyLoadHandler += (trainningExaminationId) =>
                {
                    try
                    {
                        IList<TrainningQuestion> trainningQuestionList = null;
                        var trainningExaminationQuestionList = BLLFactory.CreateTrainningExaminationQuestionBLL().GetEntitiesByExpression(string.Format("TrainningExaminationId=\"{0}\"", trainningExaminationId));
                        if (trainningExaminationQuestionList != null && trainningExaminationQuestionList.Count() > 0)
                        {
                            var queryExpression = trainningExaminationQuestionList.Select(p => p.TrainningQuestionId).ToFormatStr();
                            queryExpression += (queryExpression == "" ? "" : "*") + "IsDelete=false*IsStop=false";
                            trainningQuestionList = BLLFactory.CreateTrainningQuestionBLL().GetEntitiesByExpression(queryExpression, null, "XPath", false);
                        }
                        return trainningQuestionList;
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 根据试卷获取当前学习资料
        /// </summary>
        public static void TrainningMaterialListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningMaterialListLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningMaterialListLazyLoadHandler += (trainningExaminationId) =>
                {
                    try
                    {
                        IList<TrainningMaterial> trainningMaterialList = null;
                        var trainningExaminationMaterialList = BLLFactory.CreateTrainningExaminationMaterialBLL().GetEntitiesByExpression(string.Format("TrainningExaminationId=\"{0}\"", trainningExaminationId));
                        if (trainningExaminationMaterialList != null && trainningExaminationMaterialList.Count() > 0)
                        {
                            var queryExpression = trainningExaminationMaterialList.Select(p => p.TrainningMaterialId).ToFormatStr();
                            queryExpression += (queryExpression == "" ? "" : "*") + "IsDelete=false*IsStop=false";
                            trainningMaterialList = BLLFactory.CreateTrainningMaterialBLL().GetEntitiesByExpression(queryExpression, null, "XPath", false);
                        }
                        return trainningMaterialList;
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 考题是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void TrainningQuestionIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionIsEnableUpOperateLazyLoadHandler += (xPath, trainningTypeId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningQuestion WHERE XPath <>'{0}'
                                                 AND len(TrainningQuestion.XPath) = len('{0}')
                                                 AND CAST(TrainningQuestion.XPath AS INT)<CAST('{0}' AS INT)
                                                 AND TrainningTypeId='{1}'
                                                 AND IsDelete=0", xPath, trainningTypeId);
                        var result = BLLFactory.CreateTrainningQuestionBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考题资料是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void TrainningQuestionIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionIsEnableDownOperateLazyLoadHandler += (xPath, trainningTypeId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningQuestion WHERE XPath <>'{0}'
                                                 AND len(TrainningQuestion.XPath) = len('{0}')
                                                 AND CAST(TrainningQuestion.XPath AS INT)>CAST('{0}' AS INT)
                                                 AND TrainningTypeId='{1}'
                                                 AND IsDelete=0", xPath, trainningTypeId);
                        var result = BLLFactory.CreateTrainningQuestionBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 根据考题获取当前选项
        /// </summary>
        public static void TrainningQuestionItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionItemListLazyLoadHandler += (trainningQuestionId) =>
                {
                    try
                    {
                        var trainningQuestionItemList = BLLFactory.CreateTrainningQuestionItemBLL().GetEntitiesByExpression(string.Format("IsStop=false*IsDelete=false*TrainningQuestionId=\"{0}\"", trainningQuestionId.ToString()),null,"XPath");
                        return trainningQuestionItemList;
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 考题选项是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void TrainningQuestionItemIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionItemIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionItemIsEnableUpOperateLazyLoadHandler += (xPath, trainningQuestionId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningQuestionItem WHERE XPath <>'{0}'
                                                 AND len(TrainningQuestionItem.XPath) = len('{0}')
                                                 AND CAST(TrainningQuestionItem.XPath AS INT)<CAST('{0}' AS INT)
                                                 AND TrainningQuestionId='{1}'
                                                 AND IsDelete=0", xPath, trainningQuestionId);
                        var result = BLLFactory.CreateTrainningQuestionItemBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考题选项是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void TrainningQuestionItemIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningQuestionItemIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningQuestionItemIsEnableDownOperateLazyLoadHandler += (xPath, trainningQuestionId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningQuestionItem WHERE XPath <>'{0}'
                                                 AND len(TrainningQuestionItem.XPath) = len('{0}')
                                                 AND CAST(TrainningQuestionItem.XPath AS INT)>CAST('{0}' AS INT)
                                                 AND TrainningQuestionId='{1}'
                                                 AND IsDelete=0", xPath, trainningQuestionId);
                        var result = BLLFactory.CreateTrainningQuestionItemBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 试卷延迟加载
        /// </summary>
        public static void TrainningExaminationLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningExaminationLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningExaminationLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateTrainningExaminationBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 考试记录延迟加载
        /// </summary>
        public static void UserExaminationLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserExaminationLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserExaminationLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserExaminationBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 获取考试记录考题
        /// </summary>
        public static void UserExaminationQuestionListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserExaminationQuestionListLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserExaminationQuestionListLazyLoadHandler += (userExaminationId) =>
                {
                    try
                    {
                        var userExaminationQuestionList = BLLFactory.CreateUserExaminationQuestionBLL().GetEntitiesByExpression(string.Format("UserExaminationId=\"{0}\"", userExaminationId), null, "QuestionXPath");
                        return userExaminationQuestionList;
                    }
                    catch { return null; }
                };
            }
        }
         /// <summary>
        /// 考试类型延迟加载
        /// </summary>
        public static void TrainningTypeLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningTypeLoadHandler == null)
            {
                LazyLoadDefinition.TrainningTypeLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateTrainningTypeBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        ///考试类型孩子个数延迟加载
        /// </summary>
        public static void TrainningTypeChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningTypeChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningTypeChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateTrainningTypeBLL().GetSingleResult(string.Format("select count(*) from TrainningType where XPath like '{0}%'", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 考试类型是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void TrainningTypeIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningTypeIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningTypeIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningType WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(TrainningType.XPath) = len('{0}')
                                                 AND CAST(TrainningType.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND TrainningType.ParentId IS NULL" : " AND TrainningType.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateTrainningTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考试类型是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void TrainningTypeIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.TrainningTypeIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.TrainningTypeIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM TrainningType WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(TrainningType.XPath) = len('{0}')
                                                 AND CAST(TrainningType.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND TrainningType.ParentId IS NULL" : " AND TrainningType.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateTrainningTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        #endregion

        #region 门禁管理
        /// <summary>
        /// 门禁延迟加载
        /// </summary>
        public static void DoorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DoorLazyLoadHandler == null)
            {
                LazyLoadDefinition.DoorLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDoorBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 视频监控
        /// <summary>
        /// 关联设备仪器延迟加载
        /// </summary>
        public static void CameraRelationLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CameraRelationLazyLoadHandler == null)
            {
                LazyLoadDefinition.CameraRelationLazyLoadHandler += (cameraId, relationId) =>
                {
                    try
                    {
                        string queryExpression = "";
                        if (cameraId.HasValue) queryExpression += string.Format("CameraId=\"{0}\"", cameraId);
                        if (relationId.HasValue) queryExpression += (queryExpression == "" ? "" : "*") + string.Format("RelationId=\"{0}\"", relationId);
                        return BLLFactory.CreateCameraRelationBLL().GetEntitiesByExpression(queryExpression);
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 关联设备仪器延迟加载
        /// </summary>
        public static void CameraRelationEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CameraRelationEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.CameraRelationEquipmentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var cameraRelationList = BLLFactory.CreateCameraRelationBLL().GetEntitiesByExpression(string.Format("CameraId=\"{0}\"*RelationType={1}",id,(int)CameraRelationType.Equipment));
                        if(cameraRelationList == null || cameraRelationList.Count() == 0) return null;
                        return BLLFactory.CreateEquipmentBLL().GetEntitiesByExpression(cameraRelationList.Select(p=>p.RelationId).ToFormatStr(),null,"Name",true);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 关联门禁延迟加载
        /// </summary>
        public static void CameraRelationDoorLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CameraRelationDoorLazyLoadHandler == null)
            {
                LazyLoadDefinition.CameraRelationDoorLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var cameraRelationList = BLLFactory.CreateCameraRelationBLL().GetEntitiesByExpression(string.Format("CameraId=\"{0}\"*RelationType={1}",id,(int)CameraRelationType.Door));
                        if(cameraRelationList == null || cameraRelationList.Count() == 0) return null;
                        return BLLFactory.CreateDoorBLL().GetEntitiesByExpression(cameraRelationList.Select(p=>p.RelationId).ToFormatStr(),null,"Name",true);
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region  网盘管理
  
        /// <summary>
        /// 获取属于某个用户的所有分区的延迟加载
        /// </summary>
        public static void UserSubAreasSetUp()
        {
            if (LazyLoadDefinition.UserSubAreasLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserSubAreasLazyLoadHandler += (subAreaCategoryId, userId) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaBLL().GetSubAreasByUserId(userId, subAreaCategoryId);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        ///  获取分区文件所有子文件容量之和延迟
        /// </summary>
        public static void SubAreaFileChildrenTotalSizeSetUp()
        {
            if (LazyLoadDefinition.SubAreaFileChildrenTotalSizeLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaFileChildrenTotalSizeLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaFileBLL().GetChildrenTotalSize(id);
                    }
                    catch { return 0d; }
                };
            }
        }
        /// <summary>
        /// 获取文件分区使用容量延迟
        /// </summary>
        public static void SubAreaUsedSizeSetUp()
        {
            if (LazyLoadDefinition.SubAreaUsedSizeLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaUsedSizeLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaBLL().GetTotalSizeBySubAreaId(id);
                    }
                    catch { return 0d; }
                };
            }
        }
        /// <summary>
        /// 分区名称延迟加载
        /// </summary>
        public static void SubAreaNameSetUp()
        {
            if (LazyLoadDefinition.SubAreaNameLazyLoadHandller == null)
            {
                LazyLoadDefinition.SubAreaNameLazyLoadHandller += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault().Name;
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        ///  分区分类所对应该所有分区延迟加载
        /// </summary>
        /// 
        public static void SubAreaCategorySubAreasSetUp()
        {
            if (LazyLoadDefinition.SubAreaCategorySubAreasLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaCategorySubAreasLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaBLL().GetEntitiesByExpression(string.Format("SubAreaCategoryId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 获取分区分类名称延迟加载
        /// </summary>
        public static void SubAreaCategoryNameSetUp()
        {
            if (LazyLoadDefinition.SubAreaCategoryNameLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaCategoryNameLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaCategoryBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault().Name;
                    }
                    catch { return ""; }
                };
            }
        }
        /// <summary>
        /// 分区所属分区分类延迟加载
        /// </summary>
        public static void SubAreaSubAreaCategorySetUp()
        {
            if (LazyLoadDefinition.SubAreaSubAreaCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaSubAreaCategoryLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaCategoryBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区所对应所有分区文件延迟加载
        /// </summary>
        public static void SubAreaSubAreaFilesSetUp()
        {
            if (LazyLoadDefinition.SubAreaSubAreaFilesLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaSubAreaFilesLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaFileBLL().GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区所有对应用户类型访问权限延迟加载
        /// </summary>
        public static void SubAreaSubAreaTagPowersSetUp()
        {
            if (LazyLoadDefinition.SubAreaSubAreaTagPowersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaSubAreaTagPowersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaTagPowerBLL().GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区所有对应用户访问权限延迟加载
        /// </summary>
        public static void SubAreaSubAreaUerPowersSetUp()
        {
            if (LazyLoadDefinition.SubAreaSubAreaUerPowersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaSubAreaUerPowersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaUerPowerBLL().GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区所有对应不包含用户访问权限延迟加载
        /// </summary>
        public static void SubAreaSubAreaUserWithoutPowersSetUp()
        {
            if (LazyLoadDefinition.SubAreaSubAreaUserWithoutPowersLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaSubAreaUserWithoutPowersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaUserWithoutPowerBLL().GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区文件所在分区延迟加载
        /// </summary>
        public static void SubAreaFileSubAreaSetUp()
        {
            if (LazyLoadDefinition.SubAreaLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区文件对应父级文件延迟加载
        /// </summary>
        public static void SubAreaFileParentSetUp()
        {
            if (LazyLoadDefinition.SubAreaFileParentLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaFileParentLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSubAreaFileBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).FirstOrDefault();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 分区文件对应孩子文件延迟加载
        /// </summary>
        public static void SubAreaFileChildrensSetUp()
        {
            if (LazyLoadDefinition.SubAreaFileChildrensLazyLoadHandler == null)
            {
                LazyLoadDefinition.SubAreaFileChildrensLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var entities = BLLFactory.CreateSubAreaFileBLL().GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", id.ToString()));
                        if (entities != null && entities.Count > 0) entities = entities.Where(p => p.Id != new Guid()).ToList();
                        return entities.ToList();
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 基础信息
         /// <summary>
        /// 获取属于某个用户的所有分区的延迟加载
        /// </summary>
        public static void CountryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.CountryLazyLoadHandler == null)
            {
                LazyLoadDefinition.CountryLazyLoadHandler += (countryName) =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(countryName)) return null;
                        return BLLFactory.CreateCountryBLL().GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"",countryName));
                    }
                    catch { return null; }
                };
            }
        }
        
        #endregion

        #region 计费标准管理
        /// <summary>
        /// 获取指定格式计费标准延迟加载
        /// </summary>
        public static void UnitPriceLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UnitPriceLoadHandler == null)
            {
                LazyLoadDefinition.UnitPriceLoadHandler += (isDurationCharge,chargeType, unitPrice) =>
                    {
                        com.Bynonco.LIMS.Model.Enum.ChargeType? equipmentChargeType = null;
                        if (chargeType.HasValue) equipmentChargeType = (com.Bynonco.LIMS.Model.Enum.ChargeType)System.Enum.ToObject(typeof(com.Bynonco.LIMS.Model.Enum.ChargeType), chargeType.Value);
                        return BLLFactory.CreateChargeStandardBLL().GetUnitPriceStr(isDurationCharge,equipmentChargeType, unitPrice);
                    };
            }
        }
        #endregion

        #region 设备入网申请
            public static void EquipmentServiceHoursStatsLoadHandlerSetUp()
            {
                if(LazyLoadDefinition.EquipmentServiceHoursStatsLoadHandler==null)
                {
                    LazyLoadDefinition.EquipmentServiceHoursStatsLoadHandler+=(id)=>
                        {
                            return BLLFactory.CreateEquipmentServiceHoursStatBLL().GetEntitiesByExpression(string.Format("EquipmentApplyId=\"{0}\"",id));
                        };
                }
            }
            public static void EquipmentGroupServersLoadHandlerSetUp()
            {
                if (LazyLoadDefinition.EquipmentGroupServersLoadHandler == null)
                {
                    LazyLoadDefinition.EquipmentGroupServersLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEquipmentGroupServerBLL().GetEntitiesByExpression(string.Format("EquipmentApplyId=\"{0}\"", id));
                    };
                }
            }
            public static void EquipmentApplyLoadHandlerSetUp()
            {
                if (LazyLoadDefinition.EquipmentApplyLoadHandler == null)
                {
                    LazyLoadDefinition.EquipmentApplyLoadHandler += (id) =>
                    {
                       return BLLFactory.CreateEquipmentApplyBLL().GetEntityById(id);
                    };
                }
            }
        #endregion

        #region 设备维修基金申请
        public static void EquipmentRepairFundsApplyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRepairFundsApplyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRepairFundsApplyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEquipmentRepairFundsApplyBLL().GetEntityById(id);
                    };
            }
        }
        public static void EquipmentRepairFundsApplyAttachmentsLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentRepairFundsApplyAttachmentsLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentRepairFundsApplyAttachmentsLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEquipmentRepairFundsApplyAttachmentBLL().GetEntitiesByExpression(string.Format("EquipmentRepairFundsApplyId=\"{0}\"",id));
                    };
            }
        }
        #endregion

        #region 耗材管理
        /// <summary>
        /// 耗材分类延迟加载
        /// </summary>
        public static void MaterialCategoryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialCategoryLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialCategoryBLL().GetEntitiesByExpression(string.Format("Id=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 下级分类数延迟加载
        /// </summary>
        public static void MaterialCategoryChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialCategoryChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialCategoryChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateMaterialCategoryBLL().GetSingleResult(string.Format("select count(*) from MaterialCategory where XPath like '{0}%' and IsDelete=0", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 耗材分类是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void MaterialCategoryIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialCategoryIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialCategoryIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM MaterialCategory WHERE XPath <>'{0}'  and IsDelete=0
                                                 AND len(MaterialCategory.XPath) = len('{0}')
                                                 AND CAST(MaterialCategory.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND MaterialCategory.ParentId IS NULL" : " AND MaterialCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateMaterialCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 耗材分类是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void MaterialCategoryIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialCategoryIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialCategoryIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM MaterialCategory WHERE XPath <>'{0}'
                                                 and IsDelete=0
                                                 AND len(MaterialCategory.XPath) = len('{0}')
                                                 AND CAST(MaterialCategory.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND MaterialCategory.ParentId IS NULL" : " AND MaterialCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateMaterialCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }

        /// <summary>
        /// 耗材延迟加载
        /// </summary>
        public static void MaterialInfoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialInfoLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialInfoLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateMaterialInfoBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 耗材负责人延迟加载
        /// </summary>
        public static void MaterialAdminListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialAdminListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialAdminListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialAdminBLL().GetEntitiesByExpression(string.Format("MaterialInfoId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材采购单延迟加载
        /// </summary>
        public static void MaterialPurchaseLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialPurchaseLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialPurchaseLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateMaterialPurchaseBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 耗材采购单项目延迟加载定义
        /// </summary>
        public static void MaterialPurchaseItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialPurchaseItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialPurchaseItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialPurchaseItemBLL().GetEntitiesByExpression(string.Format("MaterialPurchaseId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材采购单项目视图延迟加载定义
        /// </summary>
        public static void ViewMaterialPurchaseItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewMaterialPurchaseItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewMaterialPurchaseItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewMaterialPurchaseItemBLL().GetEntitiesByExpression(string.Format("MaterialPurchaseId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材采购单项目视图延迟加载定义
        /// </summary>
        public static void PurchaseItemInputCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.PurchaseItemInputCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.PurchaseItemInputCountLazyLoadHandler += (id) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT TOP 1 InputValue FROM MaterialInputItem 
                            INNER JOIN MaterialInput ON MaterialInput.MaterialInputId = MaterialInputItem.MaterialInputId
                            INNER JOIN MaterialPurchaseItem ON MaterialPurchaseItem.MaterialPurchaseId = MaterialInput.MaterialPurchaseId AND MaterialInputItem.MaterialInfoId = MaterialPurchaseItem.MaterialInfoId
                            WHERE MaterialPurchaseItem.MaterialPurchaseItemId = '{0}'
                            AND MaterialInputItem.IsDelete = 0", id);
                        var result = BLLFactory.CreateMaterialPurchaseItemBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToDouble(result);
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 耗材进库单项目延迟加载定义
        /// </summary>
        public static void MaterialInputItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialInputItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialInputItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialInputItemBLL().GetEntitiesByExpression(string.Format("MaterialInputId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材进库单项目视图延迟加载定义
        /// </summary>
        public static void ViewMaterialInputItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewMaterialInputItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewMaterialInputItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewMaterialInputItemBLL().GetEntitiesByExpression(string.Format("MaterialInputId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材领用单延迟加载
        /// </summary>
        public static void MaterialRecipientLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialRecipientLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialRecipientLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateMaterialRecipientBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 耗材领用单项目延迟加载定义
        /// </summary>
        public static void MaterialRecipientItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialRecipientItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialRecipientItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialRecipientItemBLL().GetEntitiesByExpression(string.Format("MaterialRecipientId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材领用单项目视图延迟加载定义
        /// </summary>
        public static void ViewMaterialRecipientItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewMaterialRecipientItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewMaterialRecipientItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewMaterialRecipientItemBLL().GetEntitiesByExpression(string.Format("MaterialRecipientId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材出库单延迟加载
        /// </summary>
        public static void MaterialOutputLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialOutputLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialOutputLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateMaterialOutputBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 耗材出库单项目延迟加载定义
        /// </summary>
        public static void MaterialOutputItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialOutputItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialOutputItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialOutputItemBLL().GetEntitiesByExpression(string.Format("MaterialOutputId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材出库单项目视图延迟加载定义
        /// </summary>
        public static void ViewMaterialOutputItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewMaterialOutputItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewMaterialOutputItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewMaterialOutputItemBLL().GetEntitiesByExpression(string.Format("MaterialOutputId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材报废单延迟加载
        /// </summary>
        public static void MaterialBrokenLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialBrokenLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialBrokenLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateMaterialBrokenBLL().GetEntityById(id);
                };
            }
        }
        /// <summary>
        /// 耗材报废单项目延迟加载定义
        /// </summary>
        public static void MaterialBrokenItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialBrokenItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialBrokenItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialBrokenItemBLL().GetEntitiesByExpression(string.Format("MaterialBrokenId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材报废单项目视图延迟加载定义
        /// </summary>
        public static void ViewMaterialBrokenItemListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewMaterialBrokenItemListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewMaterialBrokenItemListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewMaterialBrokenItemBLL().GetEntitiesByExpression(string.Format("MaterialBrokenId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 耗材报废单项目视图延迟加载定义
        /// </summary>
        public static void MaterialSupplierListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.MaterialSupplierListLazyLoadHandler == null)
            {
                LazyLoadDefinition.MaterialSupplierListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateMaterialSupplierBLL().GetEntitiesByExpression(string.Format("MaterialInfoId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 实验动物管理
        public static void AnimalLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateAnimalBLL().GetEntityById(id);
                    };
            }
        }
        public static void AnimalCategoryLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalCategoryLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalCategoryLazyLoadHandler = (id) =>
                {
                    return BLLFactory.CreateAnimalCategoryBLL().GetEntityById(id);
                };
            }
        }
        public static void AnimalCategoryChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalCategoryChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalCategoryChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateAnimalCategoryBLL().GetSingleResult(string.Format("select count(*) from AnimalCategory where IsDelete=0 AND XPath like '{0}%'", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        public static void AnimalCategoryIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalCategoryIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalCategoryIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM AnimalCategory WHERE IsDelete=0  AND XPath <>'{0}'
                                                 AND len(AnimalCategory.XPath) = len('{0}')
                                                 AND CAST(AnimalCategory.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND AnimalCategory.ParentId IS NULL" : " AND AnimalCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateAnimalCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        public static void AnimalCategoryIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalCategoryIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalCategoryIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM AnimalCategory WHERE IsDelete=0 AND XPath <>'{0}'
                                                 AND len(AnimalCategory.XPath) = len('{0}')
                                                 AND CAST(AnimalCategory.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND AnimalCategory.ParentId IS NULL" : " AND AnimalCategory.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateAnimalCategoryBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        public static void EthicsApplyAnimalExperimentersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EthicsApplyAnimalExperimentersLazyLoadHandler == null)
            {
                LazyLoadDefinition.EthicsApplyAnimalExperimentersLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEthicsApplyAnimalExperimenterBLL().GetEntitiesByExpression(string.Format("EthicsApplyId=\"{0}\"",id));
                    };
            }
        }
        public static void AnimalFrameLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalFrameLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalFrameLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateAnimalFrameBLL().GetEntityById(id);
                    };
            }
        }
        public static void AnimalOutAppointmentDetailsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalOutAppointmentDetailsLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalOutAppointmentDetailsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateAnimalOutAppointmentDetailBLL().GetEntitiesByExpression(string.Format("AnimalOutAppointmentId=\"{0}\"", id));
                    };
            }
        }
        public static void EthicsApplyByNoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EthicsApplyByNoLazyLoadHandler == null)
            {
                LazyLoadDefinition.EthicsApplyByNoLazyLoadHandler += (ethicsNo) =>
                    {
                        if (string.IsNullOrWhiteSpace(ethicsNo)) return null;
                        return BLLFactory.CreateEthicsApplyBLL().GetFirstOrDefaultEntityByExpression(string.Format("EthicsNo=\"{0}\"", ethicsNo.Trim()));
                    };
            }
        }
        public static void EthicsApplyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EthicsApplyLazyLoadHandler == null)
            {
                LazyLoadDefinition.EthicsApplyLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateEthicsApplyBLL().GetEntityById(id);
                    };
            }
        }
        public static void AnimalCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.AnimalCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.AnimalCostDeductLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateAnimalCostDeductBLL().GetEntityById(id);
                    };
            }
        }
        public static void EthicsApplyAnimalDatasLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EthicsApplyAnimalDatasLazyLoadHandler == null)
            {
                LazyLoadDefinition.EthicsApplyAnimalDatasLazyLoadHandler += (id) =>
                {
                    return BLLFactory.CreateEthicsApplyAnimalDataBLL().GetEntitiesByExpression(string.Format("EthicsApplyId=\"{0}\"",id));
                };
            }
        }
        #endregion

        #region 设备考核评价
        /// <summary>
        /// 考核评价项目列表延迟加载定义
        /// </summary>
        public static void JudgeProjectListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectListLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectListLazyLoadHandler += (XPath) =>
                {
                    try
                    {
                        return BLLFactory.CreateJudgeProjectBLL().GetEntitiesByExpression("IsDelete=false", null, XPath);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 考核评价项目是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void JudgeProjectIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM JudgeProject WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(JudgeProject.XPath) = len('{0}')
                                                 AND CAST(JudgeProject.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateJudgeProjectBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考核评价项目是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void JudgeProjectIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM JudgeProject WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(JudgeProject.XPath) = len('{0}')
                                                 AND CAST(JudgeProject.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateJudgeProjectBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考核评价项目内容延迟加载定义
        /// </summary>
        public static void JudgeProjectContentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectContentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectContentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateJudgeProjectContentBLL().GetEntitiesByExpression(string.Format("JudgeProjectId=\"{0}\"*IsDelete=false", id.ToString()),null,"XPath");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 考核评价项目内容是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void JudgeProjectContentIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectContentIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectContentIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM JudgeProjectContent WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND JudgeProjectId='{1}'
                                                 AND len(JudgeProjectContent.XPath) = len('{0}')
                                                 AND CAST(JudgeProjectContent.XPath AS INT)<CAST('{0}' AS INT)", xPath, parentId);
                        var result = BLLFactory.CreateJudgeProjectContentBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 考核评价项目内容是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void JudgeProjectContentIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectContentIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectContentIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM JudgeProjectContent WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND JudgeProjectId='{1}'
                                                 AND len(JudgeProjectContent.XPath) = len('{0}')
                                                 AND CAST(JudgeProjectContent.XPath AS INT)>CAST('{0}' AS INT)", xPath, parentId);
                        var result = BLLFactory.CreateJudgeProjectContentBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 设备考核评价项目列表延迟加载
        /// </summary>
        public static void JudgeProjectRecordListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectRecordListLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectRecordListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateJudgeProjectRecordBLL().GetEntitiesByExpression(string.Format("JudgeEquipmentRecordId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备考核评价项目视图列表延迟加载
        /// </summary>
        public static void ViewJudgeProjectRecordListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewJudgeProjectRecordListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewJudgeProjectRecordListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewJudgeProjectRecordBLL().GetEntitiesByExpression(string.Format("JudgeEquipmentRecordId=\"{0}\"*IsDelete=false", id.ToString()), null, "XPath");
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备考核评价项目内容列表延迟加载
        /// </summary>
        public static void JudgeProjectContentRecordListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.JudgeProjectContentRecordListLazyLoadHandler == null)
            {
                LazyLoadDefinition.JudgeProjectContentRecordListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateJudgeProjectContentRecordBLL().GetEntitiesByExpression(string.Format("JudgeProjectRecordId=\"{0}\"*IsDelete=false", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 设备考核评价项目内容视图列表延迟加载
        /// </summary>
        public static void ViewJudgeProjectContentRecordListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewJudgeProjectContentRecordListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewJudgeProjectContentRecordListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewJudgeProjectContentRecordBLL().GetEntitiesByExpression(string.Format("JudgeProjectRecordId=\"{0}\"*IsDelete=false", id.ToString()), null, "XPath");
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 存款管理
        /// 开放存款设备
        /// <summary>
        /// 开放存款设备
        /// </summary>
        public static void DepositRecordEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DepositRecordEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.DepositRecordEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDepositRecordEquipmentBLL().GetEntitiesByExpression(string.Format("DepositRecordId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// 存款开放基凭证
        /// <summary>
        /// 存款开放基凭证
        /// </summary>
        public static void DepositRecordOpenFundLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DepositRecordOpenFundLazyLoadHandler == null)
            {
                LazyLoadDefinition.DepositRecordOpenFundLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDepositRecordOpenFundBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 开放基金
        /// <summary>
        /// 开放基金延迟加载
        /// </summary>
        public static void OpenFundApplyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.OpenFundApplyLazyLoadHandler == null)
            {
                LazyLoadDefinition.OpenFundApplyLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateOpenFundApplyBLL().GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 开放基金申请单可用设备延迟加载定义
        /// </summary>
        public static void EquipmentListByOpenFundApplyIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentListByOpenFundApplyIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentListByOpenFundApplyIdLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var openFundApplyEquipmentList = BLLFactory.CreateOpenFundApplyEquipmentBLL().GetEntitiesByExpression(string.Format("OpenFundApplyId=\"{0}\"*IsDelete=false", id));
                        if (openFundApplyEquipmentList != null && openFundApplyEquipmentList.Count() > 0)
                            return BLLFactory.CreateEquipmentBLL().GetEntitiesByExpression(openFundApplyEquipmentList.Select(p => p.EquipmentId).ToFormatStr());
                        return null;
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 开放基金申请单审核通过基金存款记录
        /// </summary>
        public static void OpenFundDepositRecordListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.OpenFundDepositRecordListLazyLoadHandler == null)
            {
                LazyLoadDefinition.OpenFundDepositRecordListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateOpenFundDepositRecordBLL().GetEntitiesByExpression(string.Format("OpenFundApplyId=\"{0}\"*HasChecked=true",id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 开放基金存款记录
        /// </summary>
        public static void OpenFundDepositRecordListByDepositRecordOpenFundIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.OpenFundDepositRecordListByDepositRecordOpenFundIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.OpenFundDepositRecordListByDepositRecordOpenFundIdLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateOpenFundDepositRecordBLL().GetEntitiesByExpression(string.Format("DepositRecordOpenFundId=\"{0}\"", id));
                    }
                    catch { return null; }
                };
            }
        }
         /// <summary>
        /// 开放存款设备
        /// </summary>
        public static void DepositRecordOpenFundEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.DepositRecordOpenFundEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.DepositRecordOpenFundEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateDepositRecordOpenFundEquipmentBLL().GetEntitiesByExpression(string.Format("DepositRecordOpenFundId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 学期管理
        /// <summary>
        /// 学期延迟加载
        /// </summary>
        public static void SemesterLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SemesterLazyLoadHandler == null)
            {
                LazyLoadDefinition.SemesterLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSemesterBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 学期是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void SemesterIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SemesterIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.SemesterIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Semester WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(Semester.XPath) = len('{0}')
                                                 AND CAST(Semester.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateSemesterBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 学期是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void SemesterIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SemesterIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.SemesterIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Semester WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(Semester.XPath) = len('{0}')
                                                 AND CAST(Semester.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateSemesterBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        #endregion

        #region 外出及特殊业务活动申请及报销流程
        /// <summary>
        /// 报销范围延迟加载定义
        /// </summary>
        public static void ActivityTypeLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ActivityTypeLazyLoadHandler == null)
            {
                LazyLoadDefinition.ActivityTypeLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateActivityTypeBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 报销范围是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void ActivityTypeIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ActivityTypeIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ActivityTypeIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ActivityType WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(ActivityType.XPath) = len('{0}')
                                                 AND CAST(ActivityType.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateActivityTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 报销范围是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void ActivityTypeIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ActivityTypeIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.ActivityTypeIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM ActivityType WHERE XPath <>'{0}'
                                                 AND IsDelete=0
                                                 AND len(ActivityType.XPath) = len('{0}')
                                                 AND CAST(ActivityType.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        var result = BLLFactory.CreateActivityTypeBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        #endregion

        #region 仪器扩展
        /// <summary>
        /// 仪器扩展延迟加载
        /// </summary>
       

        public static void EquipmentExtendLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentExtendLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentExtendLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var equipmentExtend = BLLFactory.CreateEquipmentExtendBLL().GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"", id.ToString()));
                        return equipmentExtend;
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 仪器共享分类编码扩展
        /// <summary>
        /// 仪器共享分类编码延迟加载
        /// </summary>
        public static void EquipmentCategoryShareLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCategoryShareLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCategoryShareLazyLoadHandler += (id) =>
                {
                    try
                    {
                        var equipmentCategoryShareLazyLoadHandler = BLLFactory.CreateEquipmentCategoryShareBLL().GetEntitiesByExpression(string.Format("Code=\"{0}\"", id.ToString()));
                        return equipmentCategoryShareLazyLoadHandler;
                    }
                    catch { return null; }
                };
            }
        }

        /// <summary>
        /// 海关监管信息关联仪器延迟加载
        /// </summary>
        public static void EquipmentCustomsBindLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentCustomsBindLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentCustomsBindLoadHandler += (id) =>
                {
                    try
                    {
                        var equipmentCustomsBindLoadHandler = BLLFactory.CreateEquipmentCustomsBindBLL().GetEntitiesByExpression(string.Format("EquipmentCustomsId=\"{0}\"", id.ToString()));
                        return equipmentCustomsBindLoadHandler;
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 上报报表
        /// <summary>
        /// SJ3Statistics延迟加载定义
        /// </summary>
        public static void SJ3StatisticsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SJ3StatisticsLazyLoadHandler == null)
            {
                LazyLoadDefinition.SJ3StatisticsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSJ3StatisticsBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// SJ3List延迟加载定义
        /// </summary>
        public static void SJ3ListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.SJ3ListLazyLoadHandler == null)
            {
                LazyLoadDefinition.SJ3ListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateSJ3BLL().GetEntitiesByExpression(string.Format("SJ3StatisticsId=\"{0}\"",id));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 面向本科生开放管理
        /// <summary>
        /// 应用知识培训申请表延迟加载
        /// </summary>
        public static void EquipmentOpenTrainingLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenTrainingLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenTrainingLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenTrainingBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 应用知识培训申请表计划延迟加载
        /// </summary>
        public static void EquipmentOpenTrainingPlanListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenTrainingPlanListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenTrainingPlanListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenTrainingPlanBLL().GetEntitiesByExpression(string.Format("EquipmentOpenTrainingId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
         /// <summary>
        /// 应用知识培训实施计划延迟加载
        /// </summary>
        public static void EquipmentOpenTrainingCarryOutLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenTrainingCarryOutLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenTrainingCarryOutLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenTrainingCarryOutBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        
        /// <summary>
        /// 应用知识培训实施计划安排列表延迟加载
        /// </summary>
        public static void EquipmentOpenTrainingCarryOutPlanListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenTrainingCarryOutPlanListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenTrainingCarryOutPlanListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenTrainingCarryOutPlanBLL().GetEntitiesByExpression(string.Format("EquipmentOpenTrainingCarryOutId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目设备列表列表延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenPracticeEquipmentBLL().GetEntitiesByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目教师列表列表延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeExperienceListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeExperienceListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeExperienceListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenPracticeExperienceBLL().GetEntitiesByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目教师列表列表延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeTeacherListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeTeacherListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeTeacherListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenPracticeTeacherBLL().GetEntitiesByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目成员列表列表延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeMemberListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeMemberListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeMemberListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenPracticeMemberBLL().GetEntitiesByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目耗材列表列表延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeMaterialListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeMaterialListLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeMaterialListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateEquipmentOpenPracticeMaterialBLL().GetEntitiesByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目使用仪器机时延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeUsedEquipmentHoursLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeUsedEquipmentHoursLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeUsedEquipmentHoursLazyLoadHandler += (id, equipmentId) =>
                {
                    try
                    {
                        var equipmentOpenPracticeRelationSubject = BLLFactory.CreateEquipmentOpenPracticeRelationSubjectBLL().GetFirstOrDefaultEntityByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                        if (equipmentOpenPracticeRelationSubject == null) return 0;
                        var usedConfirmList = BLLFactory.CreateUsedConfirmBLL().GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*EquipmentId=\"{1}\"*EndAt=-null*BeginAt=-null", equipmentOpenPracticeRelationSubject.SubjectId, equipmentId));
                        if (usedConfirmList == null || usedConfirmList.Count() == 0) return 0;
                        return usedConfirmList.Sum(p => p.Duration);
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 实践研究项目使用仪器机时延迟加载
        /// </summary>
        public static void EquipmentOpenPracticeUsedEquipmentMoneyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.EquipmentOpenPracticeUsedEquipmentMoneyLazyLoadHandler == null)
            {
                LazyLoadDefinition.EquipmentOpenPracticeUsedEquipmentMoneyLazyLoadHandler += (id, equipmentId) =>
                {
                    try
                    {
                        var equipmentOpenPracticeRelationSubject = BLLFactory.CreateEquipmentOpenPracticeRelationSubjectBLL().GetFirstOrDefaultEntityByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*IsDelete=false", id));
                        if (equipmentOpenPracticeRelationSubject == null) return 0;
                        var viewCostDeductList = BLLFactory.CreateViewCostDeductBLL().GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*EquipmentId=\"{1}\"*IsOpenFundCostDeduct=true", equipmentOpenPracticeRelationSubject.SubjectId, equipmentId), null, "", false, false, true, new string[] { "EquipmentAdminId" });
                        if (viewCostDeductList == null || viewCostDeductList.Count() == 0) return 0;
                        return viewCostDeductList.Sum(p => p.OpenFundCurrency);
                    }
                    catch { return 0; }
                };
            }
        }
        
        #endregion

        #region 楼宇管理
        /// <summary>
        /// 楼宇延迟加载
        /// </summary>
        public static void BuildingLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateBuildingBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        ///楼宇孩子个数延迟加载
        /// </summary>
        public static void BuildingChildrenCountLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingChildrenCountLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingChildrenCountLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        return Convert.ToInt32(BLLFactory.CreateBuildingBLL().GetSingleResult(string.Format("select count(*) from Building where XPath like '{0}%' and IsDelete=0 ", xPath))) - 1;
                    }
                    catch { return 0; }
                };
            }
        }
        /// <summary>
        /// 楼宇是否可以进行向上调整次序操作延迟加载
        /// </summary>
        public static void BuildingIsEnableUpOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingIsEnableUpOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingIsEnableUpOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Building WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(Building.XPath) = len('{0}')
                                                 AND CAST(Building.XPath AS INT)<CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND Building.ParentId IS NULL" : " AND Building.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateBuildingBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }
        /// <summary>
        /// 楼宇是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void BuildingIsEnableDownOperateLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingIsEnableDownOperateLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingIsEnableDownOperateLazyLoadHandler += (xPath, parentId) =>
                {
                    try
                    {
                        StringBuilder sbSql = new StringBuilder();
                        sbSql.AppendFormat(@"SELECT COUNT(*)
                                                 FROM Building WHERE XPath <>'{0}' and IsDelete=0
                                                 AND len(Building.XPath) = len('{0}')
                                                 AND CAST(Building.XPath AS INT)>CAST('{0}' AS INT)", xPath);
                        sbSql.Append(!parentId.HasValue || parentId.Value == default(Guid) ? " AND Building.ParentId IS NULL" : " AND Building.ParentId='" + parentId.Value.ToString() + "'");
                        var result = BLLFactory.CreateBuildingBLL().GetSingleResult(sbSql.ToString());
                        return Convert.ToInt32(result) > 0;
                    }
                    catch { return false; }
                };
            }
        }


        /// <summary>
        /// 楼宇上层延迟加载
        /// </summary>
        public static void BuildingUpInfoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingUpInfoLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingUpInfoLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        long step = 0;
                        if (long.TryParse(xPath, out step) && step - 1 >= 0)
                        {
                            return BLLFactory.CreateBuildingBLL().GetFirstOrDefaultEntityByExpression(string.Format("XPath=\"{0}\"*IsDelete=false", (step - 1).ToString().PadLeft(xPathLength, '0')));
                        }
                        return null;
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 楼宇是否可以进行向下调整次序操作延迟加载
        /// </summary>
        public static void BuildingDownInfoLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingDownInfoLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingDownInfoLazyLoadHandler += (xPath) =>
                {
                    try
                    {
                        int xPathLength = xPath.Length;
                        long step = 0;
                        if (long.TryParse(xPath, out step))
                        {
                            return BLLFactory.CreateBuildingBLL().GetFirstOrDefaultEntityByExpression(string.Format("XPath=\"{0}\"*IsDelete=false", (step + 1).ToString().PadLeft(xPathLength, '0')));
                        }
                        return null;
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 楼宇地图上设备列表延迟加载
        /// </summary>
        public static void BuildingEquipmentListByBuildingIdLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.BuildingEquipmentListByBuildingIdLazyLoadHandler == null)
            {
                LazyLoadDefinition.BuildingEquipmentListByBuildingIdLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateBuildingEquipmentBLL().GetEntitiesByExpression(string.Format("BuildingId=\"{0}\"", id));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion

        #region 工位预约
        /// <summary>
        /// 工位管理仪器延迟加载
        /// </summary>
        public static void NMPEquipmentsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPEquipmentsLazyLoadHandler == null)
                LazyLoadDefinition.NMPEquipmentsLazyLoadHandler = (id) =>
            {
                return BLLFactory.CreateNMPEquipmentBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"*IsDelete=false",id));
            };
        }
        /// <summary>
        /// 工位预约默认设置延迟加载
        /// </summary>
        public static void NMPDefaultSettingLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPDefaultSettingLazyLoadHandler == null)
                LazyLoadDefinition.NMPDefaultSettingLazyLoadHandler = (code) =>
                {
                    return BLLFactory.CreateDictCodeTypeBLL().GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"*IsDelete=false", code));
                };
        }
        /// <summary>
        /// 工位延迟加载
        /// </summary>
        public static void NMPLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPLazyLoadHandler == null)
                LazyLoadDefinition.NMPLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPBLL().GetEntityById(id);
                    };
        }
        /// <summary>
        /// 工位用户标签项延迟加载
        /// </summary>
        public static void NMPLabelItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPLabelItemsLazyLoadHandler == null)
                LazyLoadDefinition.NMPLabelItemsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPLabelItemBLL().GetEntitiesByExpression(string.Format("NMPLabelId=\"{0}\"", id));
                    };
        }
        /// <summary>
        /// 工作用户标签延迟加载
        /// </summary>
        public static void NMPLabelLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPLabelLazyLoadHandler == null)
                LazyLoadDefinition.NMPLabelLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPLabelBLL().GetEntityById(id);
                    };
        }
        /// <summary>
        /// 工位计费时间规则延迟加载
        /// </summary>
        public static void NMPCalcFeeTimeRuleLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPCalcFeeTimeRuleLazyLoadHandler == null)
                LazyLoadDefinition.NMPCalcFeeTimeRuleLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPCalcFeeTimeRuleBLL().GetFirstOrDefaultEntityByExpression(string.Format("NMPId=\"{0}\"", id));
                    };
        }
        /// <summary>
        /// 工位计费标准延迟加载
        /// </summary>
        public static void NMPChargeStandardLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPChargeStandardLazyLoadHandler == null)
                LazyLoadDefinition.NMPChargeStandardLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPChargeStandardBLL().GetFirstOrDefaultEntityByExpression(string.Format("NMPId=\"{0}\"", id));
                    };
        }
        /// <summary>
        /// 工位附加收费项目延迟加载
        /// </summary>
        public static void NMPAdditionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAdditionChargeItemsLazyLoadHandler == null)
                LazyLoadDefinition.NMPAdditionChargeItemsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPAdditionChargeItemBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id));
                    };
        }
        /// <summary>
        /// 工位用户标签附加收费项目收费标准延迟加载
        /// </summary>
        public static void NMPLabelAddtionChargeItemsLazyLoadHanderSetUp()
        {
            if (LazyLoadDefinition.NMPLabelAddtionChargeItemsLazyLoadHander == null)
            {
                LazyLoadDefinition.NMPLabelAddtionChargeItemsLazyLoadHander += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPLabelAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位用户标签收费标准延迟加载
        /// </summary>
        public static void NMPLabelChargeStandardsLazyLoadHanderSetUp()
        {
            if (LazyLoadDefinition.NMPLabelChargeStandardsLazyLoadHander == null)
            {
                LazyLoadDefinition.NMPLabelChargeStandardsLazyLoadHander += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPLabelChargeStandardBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id.ToString()), null);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位用户标签延迟加载
        /// </summary>
        public static void NMPLabelsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPLabelsLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPLabelsLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPLabelBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位预约优先权用户延迟加载
        /// </summary>
        public static void NMPAppointmentPriorityUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentPriorityUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAppointmentPriorityUsersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPAppointmentPriorityUserBLL().GetEntitiesByExpression(string.Format("NMPAppointmentPriorityId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位预约特殊规则用户延迟加载
        /// </summary>
        public static void NMPAppointmentSpeciaRuleUsersLazyLoadSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentSpeciaRuleUsersLazyLoad == null)
            {
                LazyLoadDefinition.NMPAppointmentSpeciaRuleUsersLazyLoad += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPAppointmentSpeciaRuleUserBLL().GetEntitiesByExpression(string.Format("NMPAppointmentSpecialRuleId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位特殊预约规则延迟加载
        /// </summary>
        public static void NMPAppointmentSpeciaRuleLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentSpeciaRuleLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAppointmentSpeciaRuleLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPAppointmentSpeciaRuleBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id.ToString())).First();
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        ///  工位用户预约限制延迟加载
        /// </summary>
        public static void NMPAppointmentTimeLimitUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentTimeLimitUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAppointmentTimeLimitUsersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateNMPAppointmentTimeLimitUserBLL().GetEntitiesByExpression(string.Format("NMPAppointmentTimeLimitId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位用户设备可预约时间段延迟加载
        /// </summary>
        public static void UserNMPTimeUsersLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.UserNMPTimeUsersLazyLoadHandler == null)
            {
                LazyLoadDefinition.UserNMPTimeUsersLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateUserNMPTimeUserBLL().GetEntitiesByExpression(string.Format("UserNMPTimeId=\"{0}\"", id.ToString()));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 工位关联仪器延迟加载
        /// </summary>
        public static void NMPEquipmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPEquipmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPEquipmentLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPEquipmentBLL().GetEntityById(id);
                    };
            }

        }
        /// <summary>
        /// 工位附加收费项目延迟加载
        /// </summary>
        public static void NMPAdditionChargeItemLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAdditionChargeItemLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAdditionChargeItemLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPAdditionChargeItemBLL().GetEntityById(id);
                    };
            }
        }
        /// <summary>
        /// 工位预约附件收费项目延迟加载
        /// </summary>
        public static void NMPAppointmentAddtionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentAddtionChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAppointmentAddtionChargeItemsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPAppointmentAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("NMPAppointmentId=\"{0}\"",id));
                    };
            }
        }
        /// <summary>
        /// 工位预约延迟加载
        /// </summary>
        public static void NMPAppointmentLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPAppointmentLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPAppointmentLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPAppointmentBLL().GetEntityById(id);
                    };
            }
        }
        /// <summary>
        /// 工位负责人延迟加载
        /// </summary>
        public static void NMPDirectorsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPDirectorsLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPDirectorsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPAdminBLL().GetEntitiesByExpression(string.Format("NMPId=\"{0}\"", id));
                    };
            }
        } /// <summary>
        /// 工位使用扣费延迟加载
        /// </summary>
        public static void NMPUsedConfirmCostDeductLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPUsedConfirmCostDeductLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPUsedConfirmCostDeductLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateCostDeductBLL().GetFirstOrDefaultEntityByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", id));
                    };
            }
        }
        /// <summary>
        /// 工位使用记录设备附加收费项目延迟加载
        /// </summary>
        public static void NMPUsedConfirmAddtionChargeItemsLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.NMPUsedConfirmAddtionChargeItemsLazyLoadHandler == null)
            {
                LazyLoadDefinition.NMPUsedConfirmAddtionChargeItemsLazyLoadHandler += (id) =>
                    {
                        return BLLFactory.CreateNMPUsedConfirmAddtionChargeItemBLL().GetEntitiesByExpression(string.Format("NMPUsedConfirmId=\"{0}\"", id));
                    };
            }
        }
        #endregion

        #region 共享基金
        /// <summary>
        /// 共享基金延迟加载
        /// </summary>
        public static void ShareFundApplyLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ShareFundApplyLazyLoadHandler == null)
            {
                LazyLoadDefinition.ShareFundApplyLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateShareFundApplyBLL().GetEntityById(id);
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 共享基金设备延迟加载
        /// </summary>
        public static void ShareFundApplyEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ShareFundApplyEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ShareFundApplyEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateShareFundApplyEquipmentBLL().GetEntitiesByExpression(string.Format("ShareFundApplyId=\"{0}\"",id));
                    }
                    catch { return null; }
                };
            }
        }
        /// <summary>
        /// 共享基金设备视图延迟加载
        /// </summary>
        public static void ViewShareFundApplyEquipmentListLazyLoadHandlerSetUp()
        {
            if (LazyLoadDefinition.ViewShareFundApplyEquipmentListLazyLoadHandler == null)
            {
                LazyLoadDefinition.ViewShareFundApplyEquipmentListLazyLoadHandler += (id) =>
                {
                    try
                    {
                        return BLLFactory.CreateViewShareFundApplyEquipmentBLL().GetEntitiesByExpression(string.Format("ShareFundApplyId=\"{0}\"",id));
                    }
                    catch { return null; }
                };
            }
        }
        #endregion
    }
}
