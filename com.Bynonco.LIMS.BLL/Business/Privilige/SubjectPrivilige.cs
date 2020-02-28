using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SubjectPrivilige:PriviligeBase
    {
        private IViewSubjectBLL __viewSubjectBLL;
        private IExperimenterSubjectBLL __experimenterSubjectBLL;
        private IUserBLL __userBLL;
        private bool __isSubjectAdmin = false;
        private bool __isCooperative = false;
        private bool __isUserOrgAuthorizedPower { get; set; }
        private ViewSubject __viewSubject;
        private User __user;
        public Guid? SubjectId { get; private set; }
        public bool IsAuthorizedPower { get; private set; }
        public SubjectPrivilige(Guid userId)
            : base(userId)
        {
            __viewSubjectBLL = BLLFactory.CreateViewSubjectBLL();
            __experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __user = __userBLL.GetEntityById(userId);
        }
        public SubjectPrivilige(Guid userId, Guid subjectId)
            : this(userId)
        {
            SubjectId = subjectId;
            __viewSubject = __viewSubjectBLL.GetEntityById(subjectId);
            __isSubjectAdmin = __viewSubject.Subject.SubjectDirectorId == userId || (__viewSubject.Subject.Experiments != null && __viewSubject.Subject.Experiments.Any(p => p.ExperimenterId.Value == userId && !p.IsDelete && p.IsAdmin.HasValue && p.IsAdmin.Value));
            if (__viewSubject.OrgId.HasValue)
                __isUserOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, __viewSubject.OrgId.Value, LabOrganizationAuthorizedType.User);
        }
        public SubjectPrivilige(Guid userId, Guid subjectId, bool isCooperative)
            : this(userId, subjectId)
        {
            if (__viewSubject.OrgId.HasValue)
                __isUserOrgAuthorizedPower = BLLFactory.CreateLabOrganizationAuthorizedBLL().CheckOrganizationControlPower(userId, __viewSubject.OrgId.Value, LabOrganizationAuthorizedType.User);
        }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Subject");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;
            IsAuthorizedPower = !SubjectId.HasValue || GetIsEnableOpearteExtend(viewModuleFunctionList, "Index", true);
            IsEnableGetGridViewSubjectList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetGridViewSubjectList");
            IsEnableGetPersonalGridViewSubjectList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetPersonalGridViewSubjectList");
            IsEnableAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "Add");
            IsEnableEdit = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableBasicContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "BasicContainer");
            IsEnableSaveBasic = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveBasic");
            IsEnableDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "Delete");
            IsEnablePhoto = GetIsEnableOpearteExtend(viewModuleFunctionList, "Photo");
            IsEnableUploadSubjectPhoto = GetIsEnableOpearteExtend(viewModuleFunctionList, "UploadSubjectPhoto");
            IsEnableSaveSubjectPhoto = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSubjectPhoto");
            IsEnableExperimenterSubjectContainer = GetIsEnableOpearteExtend(viewModuleFunctionList, "EdExperimenterSubjectContainerit");
            IsEnableGetExperimenterSubjectList = GetIsEnableOpearteExtend(viewModuleFunctionList, "GetExperimenterSubjectList");
            IsEnableAddExperimenterSubject = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableEditExperimenterSubject = GetIsEnableOpearteExtend(viewModuleFunctionList, "Edit");
            IsEnableDeleteExperimenterSubject = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteExperimenterSubject");
            IsEnableSaveExperimenterSubject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveExperimenterSubject");
            IsEnableExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "ExportExcel");
            IsEnableAddScientificProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddScientificProject");
            IsEnableEditScientificProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditScientificProject");
            IsEnableDeleteScientificProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteScientificProject");
            IsEnableSaveScientificProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveScientificProject");
            IsEnableAddEducationProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddEducationProject");
            IsEnableEditEducationProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditEducationProject");
            IsEnableSaveEducationProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveEducationProject");
            IsEnableDeleteEducationProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteEducationProject");
            IsEnableAddSocietyProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "AddSocietyProject");
            IsEnableEditSocietyProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "EditSocietyProject");
            IsEnableSaveSocietyProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveSocietyProject");
            IsEnableDeleteSocietyProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "DeleteSocietyProject");
            IsEnableSaveProject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SaveProject");
            if ( __isCooperative && __viewSubject != null)
            {
                if (IsEnableEdit || IsEnableSaveBasic || IsEnableDelete ||
                    IsEnableDeleteExperimenterSubject || IsEnableEditExperimenterSubject || IsEnableSaveExperimenterSubject)
                {
                    var experimenterSubject = __experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectId=\"{0}\"*ExperimenterId=\"{1}\"*IsDelete=false*IsAdmin=true*OwnerId=\"{2}\"",
                                                                                                                  __viewSubject.Id, _userId, __viewSubject.SubjectDirectorId.Value));
                    if (experimenterSubject == null)
                    {
                        IsEnableEdit = false;
                        IsEnableSaveBasic = false;
                        IsEnableDelete = false;

                    }
                }
                if (__user.UserType.UserIdentityEnum == UserIdentity.Tutor || __user.UserType.UserIdentityEnum == UserIdentity.OutTutor)
                {
                    IsEnableAddExperimenterSubject = true;
                }
            }
        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> moduleFunctionList, string actionName, bool? isAuthorizedPower = null)
        {
            if (!isAuthorizedPower.HasValue) isAuthorizedPower = IsAuthorizedPower;
            return IsSuperAmin || __isSubjectAdmin || (isAuthorizedPower.Value && GetIsEnableOpearte(moduleFunctionList, actionName, __isUserOrgAuthorizedPower, false));
        }
        public bool IsEnableGetGridViewSubjectList { get; private set; }
        public bool IsEnableGetPersonalGridViewSubjectList { get; private set; }
        public bool IsEnableAdd { get; private set; }
        public bool IsEnableEdit { get; private set; }
        public bool IsEnableBasicContainer { get; private set; }
        public bool IsEnableSaveBasic { get; private set; }
        public bool IsEnableDelete { get; private set; }
        public bool IsEnablePhoto { get; private set; }
        public bool IsEnableUploadSubjectPhoto { get; private set; }
        public bool IsEnableSaveSubjectPhoto { get; private set; }
        public bool IsEnableExperimenterSubjectContainer { get; private set; }
        public bool IsEnableGetExperimenterSubjectList { get; private set; }
        public bool IsEnableAddExperimenterSubject { get; private set; }
        public bool IsEnableEditExperimenterSubject { get; private set; }
        public bool IsEnableDeleteExperimenterSubject { get; private set; }
        public bool IsEnableSaveExperimenterSubject { get; private set; }
        public bool IsEnableExportExcel { get; private set; }

        public bool IsEnableAddScientificProject { get; private set; }
        public bool IsEnableEditScientificProject { get; private set; }
        public bool IsEnableSaveScientificProject { get; private set; }
        public bool IsEnableDeleteScientificProject { get; private set; }

        public bool IsEnableAddEducationProject { get; private set; }
        public bool IsEnableEditEducationProject { get; private set; }
        public bool IsEnableDeleteEducationProject { get; private set; }
        public bool IsEnableSaveEducationProject { get; private set; }

        public bool IsEnableAddSocietyProject { get; private set; }
        public bool IsEnableEditSocietyProject { get; private set; }
        public bool IsEnableSaveSocietyProject { get; private set; }
        public bool IsEnableDeleteSocietyProject { get; private set; }

        public bool IsEnableSaveProject { get; private set; }

    }
}
