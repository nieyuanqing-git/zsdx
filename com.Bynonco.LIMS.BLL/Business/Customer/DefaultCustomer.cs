using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.Business
{
    public class DefaultCustomer : ICutomer
    {
        protected IDictCodeBLL __dictCodeBLL;
        public DefaultCustomer()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public virtual string GetSystemName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SystemName");
        }
        public virtual string GetCode()
        {
            return "";
        }
        public virtual string GetSchoolName()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolName");
        }
        public virtual bool GetIsLoginShowRememberMe()
        {
            return true;
        }
        public virtual string GetHomeIndex(string xpath)
        {
            return "Index";
        }
        public virtual string GetHomeBanner(string xpath)
        {
            var isBannerShowLogo = __dictCodeBLL.GetDictCodeBoolValueByCode("SiteBannerMenu", "IsBannerShowLogo");
            var isBannerShowSearch = __dictCodeBLL.GetDictCodeBoolValueByCode("SiteBannerMenu", "IsBannerShowSearch");
            bool isShowLogo = isBannerShowLogo.HasValue && isBannerShowLogo.Value;
            bool isShowSearch = isBannerShowSearch.HasValue && isBannerShowSearch.Value;
            string viewName = "";
            if (isShowLogo && isShowSearch) viewName = "DefaultBanner";
            else if (!isShowLogo && !isShowSearch) viewName = "DefaultBannerEmpty";
            else if (!isShowLogo) viewName = "DefaultBannerEmptyLogo";
            else viewName = "DefaultBannerEmptySearch";
            return viewName;
        }
        public virtual string GetHomeMenu()
        {
            var isMenuShowSearch = __dictCodeBLL.GetDictCodeBoolValueByCode("SiteBannerMenu", "IsMenuShowSearch");
            bool isShowSearch = isMenuShowSearch.HasValue && isMenuShowSearch.Value;
            return isShowSearch ? "MenuWithSearch" : "DefaultMenu";
        }
        public virtual string GetHomeSkins(string xpath)
        {
            return "DefaultSkins";
        }

        public virtual string GetHomeFooter(string xpath)
        {
            return "Footer";
        }

        public virtual string GetHomeLayoutXPath(string xpath)
        {
            return "";
        }
        public virtual string GetEquipmentManageSearch()
        {
            return "PaginationSearch";
        }
        public virtual string GetEquipmentIconListPage()
        {
            return "PaginationItemForIcon";
        }
        public virtual string GetEquipmentShowList()
        {
            return "ShowList";
        }
        public virtual string GetEquipmentShowListConditionSeach()
        {
            return "ShowListConditionSeach";
        }
        public virtual string GetEquipmentShow()
        {
            return "Show";
        }
        public virtual string GetEquipmentShowTopInfo()
        {
            return "ShowTopInfo";
        }
        public virtual string GetEquipmentShowBasic()
        {
            return "ShowBasic";
        }
        public virtual string GetEquipmentDirectorName()
        {
            return "负责人";
        }
        public virtual string GetEquipmentLinkMenName()
        {
            return "联系人";
        }
        public virtual string GetSampleInnerApplyViewName()
        {
            return "SampleApplyInner";
        }
        public virtual string GetSampleInnerOrOuterApplyViewName()
        {
            return "SampleApplyInnerOrOuter";
        }
        public virtual bool JudgeIsEnableEditSampleAppyNumber()
        {
            return true;
        }
        public virtual string GetSampleInfoViewName()
        {
            return "ViewSampleApplyInfo";
        }
        public virtual bool JudgeIsNeedInputSamplePredictResultTimeWhenAudit()
        {
            return false;
        }

        public virtual bool JudgeIsNeedSelectSampleTesterWhenAudit()
        {
            return false;
        }
        public virtual string GetUnAuditSampleListSortName()
        {
            return "OperateDate";
        }
        public virtual int GetUnAuditSampleListPageSize()
        {
            return 10;
        }
        public virtual string GetSubjectProjectShowName()
        {
            return "项目名称";
        }
        public virtual string GetLabOrganizationName()
        {
            return "所属机构";
        }
        public virtual bool GetIsShowAppointmentSampleInfo()
        {
            return true;
        }
        public virtual bool GetIsBindEquipmentAdminUser()
        {
            return true;
        }
        public virtual bool GetIsBindEquipmentLinkUser()
        {
            return true;
        }
        public virtual bool GetIsRegistUserUploadRelativePic()
        {
            return false;
        }
        public virtual bool GetIsRegistUserUploadCertificatePhoto()
        {
            return false;
        }
        public virtual bool GetIsRegistUserAnimalExperimentInfo()
        {
            return false;
        }
        public virtual bool GetIsNJYKDRegistNoteInfo()
        {
            return false;
        }
        public virtual bool GetIsRegistTutorMoneyCard()
        {
            return false;
        }
        public virtual bool GetIsDepositAutoPrintDoc()
        {
            return false;
        }
        public virtual bool GetIsDepositPhoto()
        {
            return false;
        }
        public virtual bool GetIsDepositPhotoAfterPreAudit()
        {
            return false;
        }
        public virtual bool GetIsDepositPhotoAutoPreAuditPass()
        {
            return false;
        }
        public virtual bool GetIsDepositPhotoRequired()
        {
            return false;
        }
        public virtual bool GetIsDepositTesterRequired()
        {
            return false;
        }
        public virtual bool GetIsRegistryCard()
        {
            return false;
        }
        public virtual bool GetIsUserWorkTeam()
        {
            return false;
        }
        public virtual bool GetIsShowNewEquipmetn()
        {
            return false;
        }
        public virtual bool GetIsShowEquipmetnCategoryForHome()
        {
            return false;
        }
        public virtual bool GetIsShowMenuEquipmetnSearch()
        {
            return false;
        }
        public virtual bool GetIsShowCostDeductCalcDurationStatistics()
        {
            return true;
        }
        public virtual bool GetIsAutoCalUsedConfirmFee()
        {
            return true;
        }
        public virtual bool GetIsOnlySuperAdminEnableHandleMinuseCostDeduct()
        {
            return false;
        }
        public virtual bool GetIsEnableInputAppointmentTimes()
        {
            return false;
        }


        public virtual bool GetIsOpenFundCostDeduct()
        {
            return false;
        }

        public virtual bool GetIsAdminEnableChangeAppointmentOfOtherUser()
        {
            return true;
        }


        public virtual bool GetIsAdminEnableCancelAppointmentOfOtherUser()
        {
            return true;
        }

        public virtual bool GetIsArticleShowOneColumn()
        {
            return false;
        }
        public virtual bool GetIsArticleShowListOneColumn()
        {
            return false;
        }
        public virtual bool GetIsOpenAnimalModules()
        {
            return false;
        }
        public virtual bool GetIsShowMemuOrganizationList()
        {
            return true;
        }
        public virtual bool GetIsMainPageChangeToMore()
        {
            return false;
        }
        public virtual string GetEquipmentSearchAdvanced()
        {
            return "SearchAdvanced";
        }
        public virtual bool GetIsShowEquipmentSampleItemSearch()
        {
            return true;
        }
        public virtual bool GetIsSampleItemTestConditionRequired()
        {
            return true;
        }
        public virtual string GetSampleApplyStatusName(Model.Enum.SampleApplyStatus sampleApplyStatus)
        {
            return sampleApplyStatus.ToCnName();
        }

        public virtual string GetSampleChargeStatusName(Model.Enum.SampleApplyStatus sampleApplyStatus, Model.Enum.SampleChargeStatus sampleChargeStatus)
        {
            return sampleApplyStatus == SampleApplyStatus.Audited && sampleChargeStatus == SampleChargeStatus.Charged ? SampleChargeStatus.PreCharged.ToCnName() : sampleChargeStatus.ToCnName();
        }
        public virtual bool GetIsRegisterDoc()
        {
            return false;
        }
        public virtual bool GetIsStartAnimalModule()
        {
            return false;
        }
        public virtual bool GetIsShowAppointmentSampleStuff()
        {
            return false;
        }
        public virtual bool GetIsShowAppointmentSampleSize()
        {
            return false;
        }
        public virtual bool GetIsShowAppointmentSampleTarget()
        {
            return false;
        }
        public virtual bool GetIsAppointmentSampleAmountRequired()
        {
            return false;
        }
        public virtual bool GetIsAppointmentEqAdminNeedSelectSampleApply()
        {
            return false;
        }
        public virtual bool GetIsShowEquipmentMenuRedirectToHome()
        {
            return false;
        }
        public virtual string GetDepositSamplyNoDisplayName()
        {
            return "样品编号";
        }
        public virtual bool GetIsUserManageSubjectProject()
        {
            return false;
        }

        public virtual bool GetIsDepositRecordEquipmentRequired()
        {
            return false;
        }

        public virtual bool GetIsSendMessageToTutorSameTime()
        {
            return false;
        }
        public virtual bool GetIsSuperAdminOnlyShowAdminEqList()
        {
            return false;
        }
        public virtual bool IsManualCostDeductByCard()
        {
            var isManualCostDeductByCard = __dictCodeBLL.GetDictCodeBoolValueByCode("System", "IsManualCostDeductByCard"); ;
            return isManualCostDeductByCard.HasValue ? isManualCostDeductByCard.Value : false;
        }
        public virtual bool GetIsShowEquipmentPriceUnit()
        {
            return false;
        }

        public virtual bool GetIsOriginalCheckLogin()
        {
            return false;
        }

        public virtual bool GetIsShowSampleSubmitOpWhenAuditing()
        {
            return true;
        }
        public virtual bool GetIsHidePredictPriceWhenInputSampleApply()
        {
            return false;
        }
        public virtual bool GetIsHideChargeInfoWhenViewSampleApplyDetail()
        {
            return false;
        }

        public virtual string GetAchievementSubjectComeCode()
        {
            return "SubjectCome";
        }


        public virtual string GetDefaultAnimalEnviromentQuatityNo()
        {
            return "";
        }
        public virtual bool GetIsShowDutyFreeEquipment()
        {
            return false;
        }
        public virtual bool GetIsShowSecondLineCenter153()
        {
            return false;
        }
        public virtual bool GetIsLoadRepariUnit()
        {
            return false;
        }
        public virtual bool GetIsShowBJJTDXCol()
        {
            return false;
        }
        public virtual string GetHomeTopLogin()
        {
            return "TopLogin";
        }
        public virtual bool GetIsHomeHideTopLogin()
        {
            return false;
        }
        public virtual string GetHomeCollege()
        {
            return "College";
        }
        public virtual bool GetIsShowEquipmentNameCss()
        {
            return true;
        }
        public virtual bool GetIsSampleItemUnitUsedHourRequired()
        {
            return false;
        }
        public virtual bool GetIsAudiExportSlwUser()
        {
            return false;
        }
        public virtual bool GetIsShowFriendList()
        {
            return true;
        }
        public virtual bool GetIsAppointmentSubjectProjectRequired()
        {
            return false;
        }
        public virtual string GetLabelShowName()
        {
            return "资产编号";
        }
        public virtual bool GetIsShowImportAuditUser()
        {
            return false;
        }
        public virtual string GetArticleCategoryShowList()
        {
            return "HomeShowList";
        }
        public virtual bool GetIsUseNatureFromDictcode()
        {
            return false;
        }
        public virtual bool GetIsShowMessageShareFunApply()
        {
            return false;
        }
        public virtual bool GetIsToSystemSJ3Index()
        {
            return false;
        }
        public virtual bool GetIsShowExaminationSystemSpecialRelativeInfo()
        {
            return false;
        }
        public virtual string GetExaminationSystemName()
        {
            return "在线考试系统";
        }
        public virtual bool GetIsCheckAppointmentCancelTime()
        {
            return true;
        }
    }
}
