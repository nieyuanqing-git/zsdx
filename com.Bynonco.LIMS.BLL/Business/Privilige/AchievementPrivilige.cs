using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class AchievementPrivilige : PriviligeBase
    {
        public AchievementPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Achievement");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsEnableSubjectAchievementManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementManage");
            IsEnableSubjectAchievementList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetSubjectAchievementList");
            IsEnableSubjectAchievementAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementAdd");
            IsEnableSubjectAchievementEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementEdit");
            IsEnableSubjectAchievementSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementSave");
            IsEnableSubjectAchievementDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementDelete");
            IsEnableSubjectAchievementExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementExportExcel");
            IsEnableSubjectAchievementImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "SubjectAchievementImportExcel");

            IsEnableThesisManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisManage");
            IsEnableThesisList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetThesisList");
            IsEnableThesisAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisAdd");
            IsEnableThesisEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisEdit");
            IsEnableThesisSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisSave");
            IsEnableThesisDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisDelete");
            IsEnableThesisExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisExportExcel");
            IsEnableThesisImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ThesisImportExcel");

            IsEnablePublicationManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationManage");
            IsEnablePublicationList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetPublicationList");
            IsEnablePublicationAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationAdd");
            IsEnablePublicationEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationEdit");
            IsEnablePublicationSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationSave");
            IsEnablePublicationDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationDelete");
            IsEnablePublicationExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "PublicationExportExcel");

            IsEnablePatentManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentManage");
            IsEnablePatentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetPatentList");
            IsEnablePatentAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentAdd");
            IsEnablePatentEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentEdit");
            IsEnablePatentSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentSave");
            IsEnablePatentDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentDelete");
            IsEnablePatentExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentExportExcel");
            IsEnablePatentImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "PatentImportExcel");

            IsEnableAwardsManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsManage");
            IsEnableAwardsList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAwardsList");
            IsEnableAwardsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsAdd");
            IsEnableAwardsEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsEdit");
            IsEnableAwardsSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsSave");
            IsEnableAwardsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsDelete");
            IsEnableAwardsExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AwardsExportExcel");

            IsEnableAchievementStudentManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentManage");
            IsEnableAchievementStudentList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAchievementStudentList");
            IsEnableAchievementStudentAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentAdd");
            IsEnableAchievementStudentEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentEdit");
            IsEnableAchievementStudentSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentSave");
            IsEnableAchievementStudentDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentDelete");
            IsEnableAchievementStudentExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentExportExcel");
            IsEnableAchievementStudentImportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "AchievementStudentImportExcel");

            IsEnableAcademicPositionManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionManage");
            IsEnableAcademicPositionList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAcademicPositionList");
            IsEnableAcademicPositionAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionAdd");
            IsEnableAcademicPositionEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionEdit");
            IsEnableAcademicPositionSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionSave");
            IsEnableAcademicPositionDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionDelete");
            IsEnableAcademicPositionExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicPositionExportExcel");

            IsEnableAcademicExchangesMeetingManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesMeetingManage");
            IsEnableAcademicExchangesMeetingList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAcademicExchangesMeetingList");
            IsEnableAcademicExchangesMeetingAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesMeetingAdd");
            IsEnableAcademicExchangesMeetingEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesMeetingEdit");
            IsEnableAcademicExchangesMeetingDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesMeetingDelete");
            IsEnableAcademicExchangesMeetingExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesMeetingExportExcel");
            IsEnableAcademicExchangesOutManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesOutManage");
            IsEnableAcademicExchangesOutList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAcademicExchangesOutList");
            IsEnableAcademicExchangesOutAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesOutAdd");
            IsEnableAcademicExchangesOutEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesOutEdit");
            IsEnableAcademicExchangesOutDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesOutDelete");
            IsEnableAcademicExchangesOutExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesOutExportExcel");
            IsEnableAcademicExchangesInviteManage = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesInviteManage");
            IsEnableAcademicExchangesInviteList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetAcademicExchangesInviteList");
            IsEnableAcademicExchangesInviteAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesInviteAdd");
            IsEnableAcademicExchangesInviteEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesInviteEdit");
            IsEnableAcademicExchangesInviteDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesInviteDelete");
            IsEnableAcademicExchangesInviteExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesInviteExportExcel");
            IsEnableAcademicExchangesEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesEidt");
            IsEnableAcademicExchangesSave = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesSave");
            IsEnableAcademicExchangesDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesDelete");
            IsEnableAcademicExchangesExpert = GetIsEnableOpearteExtend(viewModuleFunctionList, "AcademicExchangesExportExcel");
        }

        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }

        public bool IsEnableSubjectAchievementManage { get; private set; }
        public bool IsEnableSubjectAchievementList { get; private set; }
        public bool IsEnableSubjectAchievementAdd { get; private set; }
        public bool IsEnableSubjectAchievementEdit { get; private set; }
        public bool IsEnableSubjectAchievementSave { get; private set; }
        public bool IsEnableSubjectAchievementDelete { get; private set; }
        public bool IsEnableSubjectAchievementExpert { get; private set; }
        public bool IsEnableSubjectAchievementImportExcel { get; private set; }

        public bool IsEnableThesisManage { get; private set; }
        public bool IsEnableThesisList { get; private set; }
        public bool IsEnableThesisAdd { get; private set; }
        public bool IsEnableThesisEdit { get; private set; }
        public bool IsEnableThesisSave { get; private set; }
        public bool IsEnableThesisDelete { get; private set; }
        public bool IsEnableThesisExpert { get; private set; }
        public bool IsEnableThesisImportExcel { get; private set; }

        public bool IsEnablePublicationManage { get; private set; }
        public bool IsEnablePublicationList { get; private set; }
        public bool IsEnablePublicationAdd { get; private set; }
        public bool IsEnablePublicationEdit { get; private set; }
        public bool IsEnablePublicationSave { get; private set; }
        public bool IsEnablePublicationDelete { get; private set; }
        public bool IsEnablePublicationExpert { get; private set; }

        public bool IsEnablePatentManage { get; private set; }
        public bool IsEnablePatentList { get; private set; }
        public bool IsEnablePatentAdd { get; private set; }
        public bool IsEnablePatentEdit { get; private set; }
        public bool IsEnablePatentSave { get; private set; }
        public bool IsEnablePatentDelete { get; private set; }
        public bool IsEnablePatentExpert { get; private set; }
        public bool IsEnablePatentImportExcel { get; private set; }

        public bool IsEnableAwardsManage { get; private set; }
        public bool IsEnableAwardsList { get; private set; }
        public bool IsEnableAwardsAdd { get; private set; }
        public bool IsEnableAwardsEdit { get; private set; }
        public bool IsEnableAwardsSave { get; private set; }
        public bool IsEnableAwardsDelete { get; private set; }
        public bool IsEnableAwardsExpert { get; private set; }

        public bool IsEnableAchievementStudentManage { get; private set; }
        public bool IsEnableAchievementStudentList { get; private set; }
        public bool IsEnableAchievementStudentAdd { get; private set; }
        public bool IsEnableAchievementStudentEdit { get; private set; }
        public bool IsEnableAchievementStudentSave { get; private set; }
        public bool IsEnableAchievementStudentDelete { get; private set; }
        public bool IsEnableAchievementStudentExpert { get; private set; }
        public bool IsEnableAchievementStudentImportExcel { get; private set; }

        public bool IsEnableAcademicPositionManage { get; private set; }
        public bool IsEnableAcademicPositionList { get; private set; }
        public bool IsEnableAcademicPositionAdd { get; private set; }
        public bool IsEnableAcademicPositionEdit { get; private set; }
        public bool IsEnableAcademicPositionSave { get; private set; }
        public bool IsEnableAcademicPositionDelete { get; private set; }
        public bool IsEnableAcademicPositionExpert { get; private set; }

        public bool IsEnableAcademicExchangesMeetingManage { get; private set; }
        public bool IsEnableAcademicExchangesMeetingList { get; private set; }
        public bool IsEnableAcademicExchangesMeetingAdd { get; private set; }
        public bool IsEnableAcademicExchangesMeetingEdit { get; private set; }
        public bool IsEnableAcademicExchangesMeetingDelete { get; private set; }
        public bool IsEnableAcademicExchangesMeetingExpert { get; private set; }
        public bool IsEnableAcademicExchangesOutManage { get; private set; }
        public bool IsEnableAcademicExchangesOutList { get; private set; }
        public bool IsEnableAcademicExchangesOutAdd { get; private set; }
        public bool IsEnableAcademicExchangesOutEdit { get; private set; }
        public bool IsEnableAcademicExchangesOutDelete { get; private set; }
        public bool IsEnableAcademicExchangesOutExpert { get; private set; }
        public bool IsEnableAcademicExchangesInviteManage { get; private set; }
        public bool IsEnableAcademicExchangesInviteList { get; private set; }
        public bool IsEnableAcademicExchangesInviteAdd { get; private set; }
        public bool IsEnableAcademicExchangesInviteEdit { get; private set; }
        public bool IsEnableAcademicExchangesInviteDelete { get; private set; }
        public bool IsEnableAcademicExchangesInviteExpert { get; private set; }
        public bool IsEnableAcademicExchangesEdit { get; private set; }
        public bool IsEnableAcademicExchangesSave { get; private set; }
        public bool IsEnableAcademicExchangesDelete { get; private set; }
        public bool IsEnableAcademicExchangesExpert { get; private set; }
    }
}
