using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Business;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.View;
using System.Web.Security;
using com.Bynonco.LIMS.Model.Business;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;

namespace com.Bynonco.LIMS.BLL
{
    public class UserBLL : BLLBase<User>, IUserBLL
    {
        public bool LoginAndResetUserPassword(Guid userId, string password, bool beenEncrypt, bool isRememberMe, string loginIP, bool? isMustModifyPwdWhenFirtLogin, ref XTransaction tran, out string errorMessage)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            var loginUser = GetEntityById(userId);
            if (loginUser == null)
            {
                errorMessage = string.Format("出错,无效用户.");
                return false;
            }
            if (loginUser.ValidityTime.HasValue && loginUser.ValidityTime.Value.Date < DateTime.Now.Date)
            {
                errorMessage = string.Format("出错,您的账号有效时间【{0}】已过,请联系管理员.", loginUser.ValidityTime.Value.Date);
                return false;
            }
            if (loginUser.UserStatusEnum == Model.Enum.UserStatus.Invalid)
            {
                errorMessage = string.Format("出错,您的账号已经被禁止登录,原因:{0}", string.IsNullOrWhiteSpace(loginUser.ForbidCause) ? "无" : loginUser.ForbidCause);
                return false;
            }
            if (loginUser.LoginTimes == null)
                loginUser.LoginTimes = 0;
            else
                loginUser.LoginTimes++;
            if (isMustModifyPwdWhenFirtLogin.HasValue) loginUser.IsMustModifyPwdWhenFirtLogin = isMustModifyPwdWhenFirtLogin.Value;
            loginUser.LastLoginTime = loginUser.CurrentLoginTime;
            loginUser.LastLoginIP = loginUser.CurrentLoginIP;
            loginUser.CurrentLoginTime = DateTime.Now;
            loginUser.CurrentLoginIP = loginIP;
            //避免首次登录为空
            if (loginUser.LastLoginTime == null)
                loginUser.LastLoginTime = loginUser.CurrentLoginTime;
            if (loginUser.LastLoginIP == "")
                loginUser.LastLoginIP = loginUser.CurrentLoginIP;
            if (!string.IsNullOrWhiteSpace(password))
            {
                loginUser.LoginPassword = beenEncrypt ? password : FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
                loginUser.Password = loginUser.LoginPassword;
            }
            Save(new User[] { loginUser }, ref tran, isSupress);
            return true;
        }
        /// 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginType">登陆类型</param>
        /// <param name="label">登陆账号</param>
        /// <param name="password">登陆密码</param>
        /// <param name="beenEncrypt">加密</param>
        /// <param name="isRememberMe">记住登陆</param>
        /// <param name="loginIP">登陆IP</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>是否登陆成果</returns>
        public bool UserLogin(LoginType? loginType, string label, string password, bool beenEncrypt, bool isRememberMe, string loginIP, out string errorMessage)
        {
            //IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            XTransaction tran = null;
            try
            {
                ICasBLL casBLL = BLLFactory.CreateCasBLL();
                var defaultqueryExpression = string.Format("Label=\"{0}\"*IsDel=false", label);
                if (beenEncrypt) defaultqueryExpression += string.Format("*LoginPassword=\"{0}\"", password);
                else defaultqueryExpression += string.Format("*(LoginPassword=\"{0}\"+LoginPassword=\"{1}\")", FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"), MD5Utility.MD5hash(password));
                var userLoginMode = casBLL.GetUserLoginMode();
                var queryExpression = defaultqueryExpression;
                if (loginType.HasValue && (userLoginMode == UserLoginMode.InnerOrOuter || userLoginMode == UserLoginMode.Cas) && loginType == LoginType.Inner)
                    queryExpression = string.Format("Label=\"{0}\"*IsDel=false*IsOuter=false", label);
                if (loginType.HasValue && (userLoginMode == UserLoginMode.InnerOrOuter || userLoginMode == UserLoginMode.Cas))
                {
                    if (loginType == LoginType.Outer)
                    {
                        queryExpression = defaultqueryExpression;
                    }
                    else
                    {
                        queryExpression = string.Format("Label=\"{0}\"*IsDel=false", label);
                    }
                    if (loginType == LoginType.LocalOut)
                    {
                        queryExpression = defaultqueryExpression + "*IsOuter=true";
                    }
                }
                var loginUser = GetFirstOrDefaultEntityByExpression(queryExpression);
                if (loginUser == null)
                {
                    if (loginType != LoginType.Outer)
                    {
                        // 非外部用户，并且华东师范大学强制同步SchoolUser
                        loginUser = CustomDiffHandlerManager.SycnUser(label);
                    }
                }
                else
                {
                    // 华东师范大学强制同步SchoolUser
                    CustomDiffHandlerManager.SycnUser(label);
                }
                if (loginUser == null)
                {
                    errorMessage = "出错,用户名和密码不正确!";
                    return false;
                }
                return LoginAndResetUserPassword(loginUser.Id, password, beenEncrypt, isRememberMe, loginIP, null, ref tran, out errorMessage);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.StartsWith("出错,") ? ex.Message : "出错," + ex.Message;
                return false;
            }
        }

        public User UserLogin(string userName, string password, bool isEncrypt = false)
        {
            if (!isEncrypt)
                return GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*(LoginPassword=\"{1}\"+LoginPassword=\"{2}\")*IsDel=false", userName, isEncrypt ? password : FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"), isEncrypt ? password : MD5Utility.MD5hash(password)));
            else
                return GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*(LoginPassword=\"{1}\"+LoginPassword=\"{2}\")*IsDel=false", userName, isEncrypt ? password : FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"), isEncrypt ? password : MD5Utility.MD5hash(password)));
        }
        private bool DelUser(Guid userId, ref XTransaction tran, out  string errorMessage, List<ExperimenterSubject> lstDeletedExperimenterSubject)
        {
            ILabOrganizationAdminBLL labOrganizationAdminBLL = BLLFactory.CreateLabOrganizationAdminBLL();
            ILabOrganizationAdminDeleteBLL labOrganizationAdminDeleteBLL = BLLFactory.CreateLabOrganizationAdminDeleteBLL();
            IUserWorkGroupBLL userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            IUserEquipmentBLL userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {

                var user = GetEntityById(userId);
                if (user == null)
                {
                    errorMessage = string.Format("出错,找不到编码为【{0}】的用户信息", userId);
                    return false;
                }
                user.IsDel = true;
                user.Label = "";
                user.NetId = "";
                user.IsAdmin = false;
                Save(new User[] { user }, ref tran, true);
                //删除授权管理机构
                var labOrganizationAdminList = labOrganizationAdminBLL.GetEntitiesByExpression(string.Format("UserID=\"{0}\"", user.Id));
                if (labOrganizationAdminList != null && labOrganizationAdminList.Count() > 0)
                {
                    foreach (var item in labOrganizationAdminList)
                    {
                        var delItem = new LabOrganizationAdminDelete();
                        delItem.Id = Guid.NewGuid();
                        delItem.LabOrganizationAdminId = item.Id;
                        labOrganizationAdminDeleteBLL.Add(new LabOrganizationAdminDelete[] { delItem }, ref tran, true);
                    }
                    labOrganizationAdminBLL.Delete(labOrganizationAdminList.Select(p => p.Id), ref tran, true);
                }
                //删除工作组
                var userWorkGroupList = userWorkGroupBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id.ToString()), null, "", true);
                if (userWorkGroupList != null && userWorkGroupList.Count() > 0)
                    userWorkGroupBLL.Delete(userWorkGroupList.Select(p => p.Id), ref tran, true);
                //逻辑删除UserAccount
                var userAccount = user.UserAccount;
                if (userAccount != null)
                {
                    userAccount.IsDelete = true;
                    userAccountBLL.Save(new UserAccount[] { userAccount }, ref tran, true);
                }
                //删除UserEquipment
                var userEquipmentList = userEquipmentBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id.ToString()), null, "", true);
                if (userEquipmentList != null && userEquipmentList.Count() > 0)
                    userEquipmentBLL.Delete(userEquipmentList.Select(p => p.Id), ref tran, true);
                //逻辑删除Subject
                string experimenterSubjectExpression = string.Format("ExperimenterId=\"{0}\"*IsDelete=false", user.Id.ToString());
                var subject = subjectBLL.GetSubjectByTurtorId(user.Id);
                if (subject != null)
                {
                    subject.IsDelete = true;
                    subjectBLL.Save(new Subject[] { subject }, ref tran, true);
                    experimenterSubjectExpression = string.Format("ExperimenterId=\"{0}\"+SubjectId=\"{1}\"*IsDelete=false", user.Id.ToString(), subject.Id.ToString());
                }
                //逻辑删除ExperimenterSubject
                var experimenterSubjectList = experimenterSubjectBLL.GetEntitiesByExpression(experimenterSubjectExpression, null, "", true);
                if (experimenterSubjectList != null && experimenterSubjectList.Count() > 0)
                {
                    if (lstDeletedExperimenterSubject != null) lstDeletedExperimenterSubject.AddRange(experimenterSubjectList);
                    foreach (var item in experimenterSubjectList)
                    {
                        if (lstDeletedExperimenterSubject != null && lstDeletedExperimenterSubject.Any(p => p.Id == item.Id)) continue;
                        item.IsDelete = true;
                        experimenterSubjectBLL.Save(new ExperimenterSubject[] { item }, ref tran, true);
                    }

                }
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool DelUser(Guid userId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelUser(userId, ref tran, out errorMessage, null);
        }
        public bool DelUsers(IList<User> users, out  string errorMessage)
        {
            errorMessage = "";
            if (users != null && users.Count > 0)
            {
                List<ExperimenterSubject> lstDeletedExperimenterSubject = new List<ExperimenterSubject>();
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var user in users)
                    {
                        var result = DelUser(user.Id, ref tran, out errorMessage, lstDeletedExperimenterSubject);
                        if (!result)
                            throw new Exception(errorMessage);
                    }
                    if (tran.HasTransaction) tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (tran.HasTransaction) tran.CommitTransaction();
                    errorMessage = ex.Message;
                    return false;
                }
                finally { tran.Dispose(); }
            }
            return true;
        }
        public bool CheckIsPower(Guid? id, string controllerName, string actionName)
        {
            bool isPower = false;
            if (id.HasValue)
            {
                string workGroupId = string.Empty;
                string expression = string.Empty;
                var viewUserSystemSettingBLL = BLLFactory.CreateViewUserSystemSettingBLL();
                var viewUserSystemSetting = viewUserSystemSettingBLL.GetEntityByUserId(id.Value);
                if (viewUserSystemSetting != null)
                {
                    if (viewUserSystemSettingBLL.IsSuperAdminWorkGroup(viewUserSystemSetting))
                    {
                        return true;
                    }
                    workGroupId = viewUserSystemSetting.WorkGroupId != null ? viewUserSystemSetting.WorkGroupId.Value.ToString() : string.Empty;
                }
                if (workGroupId == string.Empty)
                {
                    var userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
                    var userWorkGroupList = userWorkGroupBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", id.Value.ToString()), null, "", true);
                    if (userWorkGroupList != null && userWorkGroupList.Count > 0)
                    {
                        for (int i = 0; i < userWorkGroupList.Count; i++)
                        {
                            var item = userWorkGroupList[i];
                            if (i == 0)
                                expression += (expression == "" ? "" : "*") + "(";
                            expression += (i == 0 ? "" : "+") + string.Format("WorkGroupId=\"{0}\"", item.WorkGroupId.ToString());
                            if (i == userWorkGroupList.Count - 1)
                                expression += ")";
                        }
                    }
                }
                else
                {
                    expression += (expression == "" ? "" : "*") + string.Format("WorkGroupId=\"{0}\"", workGroupId);
                }
                if (expression != "")
                {
                    expression += string.Format("*ControllerName=\"{0}\"*ActionName=\"{1}\"*IsStop=false*IsDelete=false", controllerName, actionName);
                    var viewWorkGroupModuleBLL = BLLFactory.CreateViewWorkGroupModuleBLL();
                    var viewWorkGroupModuleList = viewWorkGroupModuleBLL.GetEntitiesByExpression(expression, null, "XPath", true);
                    if (viewWorkGroupModuleList != null && viewWorkGroupModuleList.Any())
                    {
                        isPower = true;
                    }
                }
            }
            return isPower;
        }
        public bool CheckIsExist(Guid? id, string columnName, string columnValue, string withOutSameName = "")
        {
            bool isOk = false;
            var userInfoList = GetEntitiesByExpression(string.Format("Id=-\"{0}\"*{1}=\"{2}\"*IsDel=false*UserName=-\"{3}\"", id.HasValue ? id.Value.ToString() : default(Guid).ToString(), columnName, columnValue, withOutSameName), null, "", true);
            if (userInfoList != null && userInfoList.Count > 0)
            {
                isOk = true;
            }
            return isOk;
        }
        public bool CheckLabelIsExist(Guid? id, string label)
        {
            return CheckIsExist(id, "Label", label.Trim());
        }
        public bool CheckCardIsExist(Guid? id, int card)
        {
            return CheckIsExist(id, "Card", card.ToString());
        }
        public bool CheckPhoneNumberIsExist(Guid? id, string phoneNumber, string withOutSameName = "")
        {
            return CheckIsExist(id, "PhoneNumber", phoneNumber.Trim(), withOutSameName);
        }
        public bool CheckEmailIsExist(Guid? id, string email, string withOutSameName = "")
        {
            return CheckIsExist(id, "Email", email.Trim(), withOutSameName);
        }
        public bool CheckUserStatus(User user, out string errorMessage)
        {
            errorMessage = "";
            if (user == null) return true;
            if (user.UserStatusEnum != UserStatus.AuditPass)
            {
                errorMessage = user.UserStatusEnum.ToCnName();
                return false;
            }
            return true;
        }
        public bool CheckUserStatus(Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
            var user = GetEntityById(userId.Value);
            errorMessage = user.UserStatusEnum.ToCnName();
            if (user.UserStatusEnum != UserStatus.AuditPass)
            {
                return false;
            }
            return true;
        }
        public bool CheckIsEnableAuditPass(User user, out string errorMessage)
        {
            errorMessage = "";
            if (user.TutorId.HasValue && user.IsNeedTutorAudit && user.TutorAuditStatusEnum != TutorAuditStatus.Passed)
            {
                errorMessage = "出错,需要导师审核通过才能进行操作";
                return false;
            }
            return true;
        }
        public User GetUserByLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return null;
            var userList = GetEntitiesByExpression(string.Format("Label=\"{0}\"*IsDel=false", label), null, "", false, true);
            return userList != null && userList.Count > 0 ? userList.First() : null;
        }
        public User GetUserByNetId(string netId)
        {
            if (string.IsNullOrEmpty(netId)) return null;
            var userList = GetEntitiesByExpression(string.Format("NetId=\"{0}\"*IsDel=false", netId), null, "", false, true);
            return userList != null && userList.Count > 0 ? userList.First() : null;
        }

        public IList<User> GetUserListByLabels(IEnumerable<string> userLabels)
        {
            if (userLabels == null || userLabels.Count() == 0) return null;
            var queryExpression = "";
            foreach (var userLabel in userLabels)
            {
                if (string.IsNullOrWhiteSpace(userLabel)) continue;
                queryExpression += queryExpression == "" ?
                    string.Format("Label=\"{0}\"", userLabel.Trim()) :
                    "+" + string.Format("Label=\"{0}\"", userLabel.Trim());
            }
            if (queryExpression == "") return null;
            return GetEntitiesByExpression(queryExpression);
        }
        /// <summary>
        /// 获取使用用户支付账号
        /// </summary>
        /// <param name="userId">使用用户</param>
        /// <param name="subjectId">课题组</param>
        /// <param name="paymentMethod">支付方式</param>
        /// <param name="userAccount">支付账号</param>
        /// <returns>返回课题组负责人</returns>
        public User GetPayer(Guid userId, Guid subjectId, out PaymentMethod paymentMethod, out UserAccount userAccount)
        {
            var subjectBLL = BLLFactory.CreateSubjectBLL();
            paymentMethod = PaymentMethod.SubjectDirectorPay;
            userAccount = null;
            var relativeSujectList = subjectBLL.GetUserRelevantSubjects(userId);
            if (relativeSujectList == null || relativeSujectList.Count == 0) return null;
            // 提取 subjectId 指定课题组
            var subejct = relativeSujectList.FirstOrDefault(p => p.Id == subjectId);
            if (subejct == null) return null;
            // 课题组负责人账号
            userAccount = subejct.Director.UserAccount;
            if (userId != subejct.SubjectDirectorId.Value)
            {
                // 操作用户非课题组负责人
                var experimenterSubject = subejct.Experiments.FirstOrDefault(p => p.ExperimenterId == userId && !p.IsDelete);
                // 操作用户为该课题组成员，而且没有导师
                if (experimenterSubject.Experimenter.TutorId == null)
                {
                    paymentMethod = PaymentMethod.TutorSubjectFunds;
                    return experimenterSubject.Experimenter;
                }
                // 操作用户为该课题组成员，而且成员为合作课题组负责人
                var tutorExperimenterSubject = subejct.Experiments.FirstOrDefault(p => p.ExperimenterId == experimenterSubject.OwnerId.Value && !p.IsDelete);
                if (tutorExperimenterSubject != null)
                {
                    paymentMethod = PaymentMethod.TutorSubjectFunds;
                    return tutorExperimenterSubject.Experimenter;
                }
            }
            return subejct.Director;
        }
        public IList<User> GetUserList(LabelType labelType, IEnumerable<Guid> labelIds)
        {
            var subjectBLL = BLLFactory.CreateSubjectBLL();
            if (labelIds == null || labelIds.Count() == 0) return null;
            List<User> lstUsers = new List<User>();
            switch (labelType)
            {
                case LabelType.Organization:
                    return GetEntitiesByExpression(labelIds.ToFormatStr("OrganizationId") + "*IsDel=false");
                case LabelType.Tag:
                    return GetEntitiesByExpression(labelIds.ToFormatStr("TagId") + "*IsDel=false");
                case LabelType.User:
                    return GetEntitiesByExpression(labelIds.ToFormatStr() + "*IsDel=false");
                case LabelType.Suject:
                    var subjectList = subjectBLL.GetEntitiesByExpression(labelIds.ToFormatStr() + "*IsDelete=false");
                    if (subjectList == null || subjectList.Count == 0) return null;
                    foreach (var subject in subjectList)
                    {
                        if (subject.Experiments != null && subject.Experiments.Count > 0)
                            lstUsers.AddRange(subject.Experiments.Select(p => p.Experimenter));
                    }
                    break;
            }
            return lstUsers;
        }
        public IList<ViewSampleTester> GetTesterBySampleItemIds(string[] sampleItemIds)
        {
            var sQueryExpression = "(IsEnableTest=true)";
            if (sampleItemIds != null && sampleItemIds.Count() > 0)
                sQueryExpression = sampleItemIds.ToList().ConvertAll(p => new Guid(p)).ToFormatStr("SampleItemId");
            var viewSampleTesterBLL = BLLFactory.CreateViewSampleTesterBLL();
            var vieSampleTesters = viewSampleTesterBLL.GetEntitiesByExpression(sQueryExpression.ToString(), null, "UserName");
            IList<ViewSampleTester> lstViewSampleTesters = new List<ViewSampleTester>();
            if (vieSampleTesters != null && vieSampleTesters.Count > 0)
            {
                var vieSampleTesterGroups = vieSampleTesters.GroupBy(p => new { p.UserId, p.Email, p.LoginName, p.UserName, p.OrganizationName, p.PhoneNumber, p.Sex, p.SexName, p.TagName, p.TagId });
                foreach (var vieSampleTesterGroup in vieSampleTesterGroups)
                {

                    if (sampleItemIds != null && vieSampleTesterGroup.Count() < sampleItemIds.Length) continue;
                    lstViewSampleTesters.Add(new ViewSampleTester()
                    {
                        Email = vieSampleTesterGroup.Key.Email,
                        LoginName = vieSampleTesterGroup.Key.LoginName,
                        UserName = vieSampleTesterGroup.Key.UserName,
                        OrganizationName = vieSampleTesterGroup.Key.OrganizationName,
                        PhoneNumber = vieSampleTesterGroup.Key.PhoneNumber,
                        Sex = vieSampleTesterGroup.Key.Sex,
                        SexName = vieSampleTesterGroup.Key.SexName,
                        TagId = vieSampleTesterGroup.Key.TagId,
                        TagName = vieSampleTesterGroup.Key.TagName,
                        UserId = vieSampleTesterGroup.Key.UserId,
                        Id = Guid.NewGuid()
                    });
                }
            }
            return lstViewSampleTesters;
        }
        public IList<User> GetStudentListByTutorId(Guid tutorId)
        {
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            var self = userBLL.GetEntityById(tutorId);
            if (self == null) throw new Exception(string.Format("找不到编码为【{0}】的用户信息", tutorId));
            self.UserName = "自己";
            List<User> lstStudents = new List<User>() { self };
            var subjectList = subjectBLL.GetSubjectListByDirectorId(tutorId);
            if (subjectList != null && subjectList.Count > 0)
            {
                foreach (var subject in subjectList)
                {
                    if (subject.Experiments == null || subject.Experiments.Count == 0 || subject.Experiments.Where(p => !p.IsDelete).Count() == 0) continue;
                    lstStudents.AddRange(subject.Experiments.Where(p => !lstStudents.Any(x => x.Id == p.Id)).Select(p => p.Experimenter));
                }
            }
            foreach (var student in lstStudents)
                student.IsSupressLazyLoad = true;
            return lstStudents;
        }
        public bool ChangeUserType(User user, ViewSchoolUser schoolUser, ref XTransaction tran, out string errorMessage)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            errorMessage = "";
            var isSupress = tran != null;
            if ((int)user.UserType.UserIdentityEnum == schoolUser.UserType) return true;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                switch (user.UserType.UserIdentityEnum)
                {
                    case UserIdentity.Student:
                        if (schoolUser.UserTypeEnmu != UserIdentity.Tutor)
                            throw new Exception(string.Format("用户不是从【{0}】身份类型到【{1}】身份类型", UserIdentity.Student.ToCnName(), SchoolUserType.Tutor.ToCnName()));
                        var result = GenerateTutorRelativeInfo(user, ref tran, out errorMessage);
                        if (!result) throw new Exception(errorMessage);
                        break;
                    case UserIdentity.Tutor:
                        if (schoolUser.UserTypeEnmu != UserIdentity.Student)
                            throw new Exception(string.Format("用户不是从【{0}】身份类型到【{1}】身份类型", UserIdentity.Tutor.ToCnName(), SchoolUserType.Student.ToCnName()));
                        user.TutorId = null;
                        if (!DelTutorRelativeInfo(user, ref tran, out errorMessage)) throw new Exception(errorMessage);
                        break;
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool ChangeTutor(User user, ExperimenterSubject experimenterSubject, ref XTransaction tran, out string errorMessage)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (user.UserType.UserIdentityEnum == UserIdentity.Student)
                {
                    var userTemp = GetEntityById(user.Id);
                    if (userTemp == null || !userTemp.TutorId.HasValue) return true;
                    if (userTemp.TutorId != user.TutorId)
                    {
                        var experimentList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("ExperimenterId=\"{0}\"*OwnerId=\"{1}\"*IsDelete=false", user.Id, userTemp.TutorId.Value));
                        if (experimentList != null && experimentList.Count > 0)
                        {
                            foreach (var experiment in experimentList)
                                experiment.IsDelete = true;
                            experimenterSubjectBLL.Save(experimentList, ref tran, true);
                        }
                        if (experimenterSubject == null)
                        {
                            var subject = subjectBLL.GetSubjectByTurtorId(user.TutorId);
                            if (subject == null) throw new Exception(string.Format("找不到{0}的课题组信息", user.TutorName));
                            experimenterSubject = experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("ExperimenterId=\"{0}\"*OwnerId=\"{1}\"*IsDelete=false", user.Id, user.TutorId.Value));
                            if (experimenterSubject == null)
                            {
                                experimenterSubject = new ExperimenterSubject()
                                {
                                    Id = Guid.NewGuid(),
                                    ApplyAt = DateTime.Now,
                                    ExperimenterId = user.Id,
                                    IsAdmin = false,
                                    IsDelete = false,
                                    JoinAt = DateTime.Now,
                                    OddSum = 0d,
                                    OwnerId = user.TutorId,
                                    PreAlert = 0d,
                                    RealCurency = 0d,
                                    StatusEnum = ExperimenterSubjectStatus.Authorized,
                                    StopAt = null,
                                    TotalSum = 0d,
                                    SubjectId = subject.Id,
                                    Unappointment = 0d,
                                    UnUseable = 0d,
                                    UseMoneyTypeEnum = ExperimenterSubjectUseMoneyType.TutorAccount
                                };
                            }
                            experimenterSubjectBLL.Add(new ExperimenterSubject[] { experimenterSubject }, ref tran, true);
                        }
                    }
                    if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool GenerateUserRelativeInfo(ViewSchoolUser schoolUser, string rootOrgXPath, ref XTransaction tran, out string errorMessage, out User user)
        {
            ISchoolUserBLL schoolUserBLL = BLLFactory.CreateSchoolUserBLL();
            ILabOrganizationBLL labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            IDictCodeTypeBLL dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            IUserTypeBLL userTypeBLL = BLLFactory.CreateUserTypeBLL();
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IBusinessIdBLL businessIdBLL = BLLFactory.CreateBusinessIdBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            ITagBLL tagBLL = BLLFactory.CreateTagBLL();
            errorMessage = "";
            ExperimenterSubject experimenterSubject = null; user = null;
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            bool isNew = false;
            var identityNo = string.IsNullOrWhiteSpace(schoolUser.IdentityCardNo) ? schoolUser.LoginName : schoolUser.IdentityCardNo;
            var loginName = string.IsNullOrWhiteSpace(schoolUser.LoginName) ? identityNo : schoolUser.LoginName;
            var certificateTypeName = schoolUser.CertificateType;
            var isSyncCard = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "IsSyncCard"); //是否同步卡号
            var isSyncCoverCard = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "IsSyncCoverCard"); //是否覆盖现有卡号
            var isReverseCard = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "IsReverseCard"); //是否16进制反转卡号
            var isUseIdPassword = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "isUseIdPassword"); //是否使用证件号码后六位为密码
            var isSaveXdCard = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "IsSaveXdCard"); //是否16进制保存卡号
            var IsSyncExistsLabel = dictCodeBLL.GetDictCodeBoolValueByCode("SchoolUser", "IsSyncExistsLabel"); //是否处理已经存在的登录名

            int maxRandomCard = 10000;
            var maxRandomCardItem = businessIdBLL.GetFirstOrDefaultEntityByExpression("Code=\"UserRandomCard\"");
            if (maxRandomCardItem != null) maxRandomCard = maxRandomCardItem.Value;
            if (string.IsNullOrWhiteSpace(certificateTypeName))
            {
                certificateTypeName = schoolUser.UserTypeEnmu == UserIdentity.Student ? "学生证" : (schoolUser.UserTypeEnmu == UserIdentity.Tutor || schoolUser.UserTypeEnmu == UserIdentity.OutTutor) ? "教职工证" : "身份证";
            }
            try
            {
                if (string.IsNullOrWhiteSpace(identityNo)) throw new Exception(string.Format("当前用户【{0}】编码【{1}】证件号为空", schoolUser.Name, identityNo));
                var userList = GetEntitiesByExpression(string.Format("Label=\"{0}\"*IsDel=false", loginName));
                if (userList != null && userList.Count > 0)
                {
                    isNew = false;
                    user = userList.First();
                    if (IsSyncExistsLabel == false)
                    {
                        throw new Exception(string.Format("存在{0}个登陆名为【{1}】的用户信息", userList.Count, loginName));
                    }
                }
                else
                {
                    isNew = true;
                    user = new User() { Id = Guid.NewGuid(), UserStatusEnum = UserStatus.AuditPass, RegisterTime = DateTime.Now, IdentityCardNo = identityNo };
                }
                string telNo = "", mobileNo = "";
                schoolUserBLL.SplitMobileTelNo(schoolUser.PhoneNumber, out mobileNo, out telNo);
                user.Name = loginName;
                user.UserName = schoolUser.Name;
                user.Sex = Convert.ToBoolean(schoolUser.Sex);
                if (!string.IsNullOrWhiteSpace(telNo)) user.FixedPhone = telNo;
                if (!string.IsNullOrWhiteSpace(mobileNo)) user.PhoneNumber = mobileNo;
                if (!string.IsNullOrWhiteSpace(schoolUser.Email)) user.Email = schoolUser.Email;
                if (string.IsNullOrWhiteSpace(user.InputStr)) user.InputStr = ShortcutStringUtility.GetFirstPYLetter(user.UserName);
                user.Grade = schoolUser.Grade;
                user.NetId = schoolUser.LoginName;
                user.IdentityCardNo = identityNo;
                user.ValidityTime = schoolUser.ValidityTime;
                user.Jobtitle = schoolUser.Jobtitle;
                user.Speciality = schoolUser.Speciality;
                user.ResearchDirection = schoolUser.ResearchDirection;
                user.Label = loginName;
                user.IsOuter = schoolUser.IsOuter;
                user.IsLeamsAdd = schoolUser.IsLeamsAdd;
                uint uintCard = 0;
                var customer = com.Bynonco.LIMS.BLL.Business.Customer.Factory.CustomerFactory.GetCustomer();

                if (isSyncCard.HasValue && isSyncCard.Value && !string.IsNullOrWhiteSpace(schoolUser.Card))
                {
                    var isCoverCard = isSyncCoverCard.HasValue && isSyncCoverCard.Value;
                    if (isReverseCard.HasValue && isReverseCard.Value)
                    {
                        char[] c1 = new char[8];
                        uint reverseUintCard = 0;
                        if (isSaveXdCard.HasValue && isSaveXdCard.Value)
                        {
                            c1 = schoolUser.Card.PadLeft(8, '0').ToCharArray();
                            reverseUintCard = uint.Parse(new string(c1), System.Globalization.NumberStyles.AllowHexSpecifier);//16转10
                        }
                        else
                        {
                            var tempCard = schoolUser.Card;
                            reverseUintCard = uint.Parse(tempCard);
                            c1 = reverseUintCard.ToString("X").PadLeft(8, '0').ToCharArray();
                        }
                        char[] c2 = new char[8];
                        c2[0] = c1[6];
                        c2[1] = c1[7];
                        c2[2] = c1[4];
                        c2[3] = c1[5];
                        c2[4] = c1[2];
                        c2[5] = c1[3];
                        c2[6] = c1[0];
                        c2[7] = c1[1];
                        try
                        {
                            uintCard = uint.Parse(new string(c2), System.Globalization.NumberStyles.AllowHexSpecifier);//16转10
                            int reverseCard = (int)reverseUintCard;
                            int card = (int)uintCard;
                            if (isCoverCard || (!user.Card.HasValue || user.Card.Value == reverseCard || (user.Card.Value > 0 && user.Card.Value <= maxRandomCard)))
                            {
                                user.Card = card;
                                user.RealCard = uintCard;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        if (uint.TryParse(schoolUser.Card, out uintCard))
                        {
                            int card = (int)uintCard;
                            if (isCoverCard || (!user.Card.HasValue || (user.Card.Value > 0 && user.Card.Value <= maxRandomCard)))
                            {
                                user.Card = card;
                                user.RealCard = uintCard;
                            }
                        }
                    }
                }
                var certificateType = dictCodeBLL.GenerateDefaultCertificateType(certificateTypeName, ref tran);
                user.CertificateTypeId = certificateType.Id;
                if (!user.IsFinishBindingForMutiCard) user.IsWithMutiCard = schoolUser.IsWithMutiCard;
                if (schoolUser.OrganizationId.HasValue) user.OrganizationId = schoolUser.OrganizationId;
                if (!user.OrganizationId.HasValue && !string.IsNullOrWhiteSpace(schoolUser.OrganizationName))
                {
                    var organization = labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*Type={1}*IsDelete=false", schoolUser.OrganizationName.Trim(), (int)LabOrganizationType.Organziton), null, "XPath");
                    if (organization == null)
                    {
                        if (string.IsNullOrWhiteSpace(rootOrgXPath)) throw new ArgumentException("rootOrgXPath为空");
                        var rootOrganization = labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("XPath=\"{0}\"*IsDelete=false", rootOrgXPath));
                        if (rootOrganization == null) throw new Exception(string.Format("找不到XPath为{0}的机构信息", rootOrgXPath));
                        organization = new LabOrganization()
                        {
                            Id = Guid.NewGuid(),
                            LabOrganizationType = LabOrganizationType.Organziton,
                            ParentId = rootOrganization.Id,
                            Name = schoolUser.OrganizationName,
                            IsDelete = false
                        };
                        organization.XPath = XPathUtility.GenerateXPath(null, rootOrganization.Id,
                        (entityId) => { return labOrganizationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                        (parentEntityId) => { return labOrganizationBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                        () => { return labOrganizationBLL.GetEntitiesByExpression("ParentId=null*IsDelete=false"); });
                        labOrganizationBLL.Add(new LabOrganization[] { organization }, ref tran, true);

                    }
                    user.OrganizationId = organization.Id;
                }
                if (isNew)
                {
                    user.IsMustModifyPwdWhenFirtLogin = false;
                    if (isUseIdPassword == true && user.IdentityCardNo.Length > 6 && user.IdentityCardNo != user.Label)
                    {
                        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.IdentityCardNo.Substring(user.IdentityCardNo.Length - 6), "MD5");
                        user.LoginPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.IdentityCardNo.Substring(user.IdentityCardNo.Length - 6), "MD5");
                    }
                    else
                    {
                        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Label, "MD5");
                        user.LoginPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Label, "MD5");
                    }
                    user.IsImported = true;
                    user.ImportedRemark = "从中间库导入";
                    var tagName = schoolUser.TagName;
                    if (string.IsNullOrWhiteSpace(schoolUser.TagName)) schoolUser.TagName = schoolUser.IsOuter ? "校外" : "校内";
                    var tag = tagBLL.GenerateDefaultTag(schoolUser.TagName, ref tran);
                    user.TagId = tag.Id;
                    var userType = userTypeBLL.GenerateDefaultUserType(string.IsNullOrWhiteSpace(schoolUser.UserTypeDetail) ? schoolUser.UserTypeEnmu.ToCnName() : schoolUser.UserTypeDetail, schoolUser.UserTypeEnmu, ref tran);
                    user.UserTypeId = userType.Id;
                    if (schoolUser.UserTypeEnmu == UserIdentity.Student)
                    {
                        if (!string.IsNullOrWhiteSpace(schoolUser.TutorIdentityCardNo))
                        {
                            var tutorList = GetEntitiesByExpression(string.Format("(IdentityCardNo=\"{0}\"+Label=\"{0}\")*IsDel=false", schoolUser.TutorIdentityCardNo));
                            if (tutorList == null) throw new Exception(string.Format("找不到证件号为【{0}】的{1}身份类型", schoolUser.TutorIdentityCardNo, schoolUser.UserTypeEnmu.ToCnName()));
                            if (tutorList.Count > 1) throw new Exception(string.Format("找到{0}个证件号为【{1}】的{2}身份类型", tutorList.Count, schoolUser.TutorIdentityCardNo, schoolUser.UserTypeEnmu.ToCnName()));
                            var tutor = tutorList.First();
                            if ((tutor.UserType.UserIdentityEnum != UserIdentity.Tutor && tutor.UserType.UserIdentityEnum != UserIdentity.OutTutor)) throw new Exception(string.Format("证件号为【{0}】的用户身份类型【{1}】不是【{2}】身份类型", schoolUser.IdentityCardNo, tutor.UserType.UserIdentityEnum.ToCnName(), SchoolUserType.Tutor.ToCnName()));
                            user.TutorId = tutor.Id;
                        }
                        else if (user.TutorAuditStatusEnum == TutorAuditStatus.WaitingAudit && !user.BindTutorId.HasValue)
                        {
                            user.IsNeedTutorAudit = true;
                            user.UserStatusEnum = UserStatus.AuditWaitting;
                        }
                    }
                    if (schoolUser.UserTypeEnmu == UserIdentity.Tutor || schoolUser.UserTypeEnmu == UserIdentity.OutTutor) user.TutorId = null;
                }
                else
                {
                    if (user.UserType != null && user.UserType.UserIdentity.HasValue && user.UserType.UserIdentity.Value == (int)schoolUser.UserTypeEnmu)
                    {
                        var userType = userTypeBLL.GenerateDefaultUserType(string.IsNullOrWhiteSpace(schoolUser.UserTypeDetail) ? schoolUser.UserTypeEnmu.ToCnName() : schoolUser.UserTypeDetail, schoolUser.UserTypeEnmu, ref tran);
                        if (!user.UserType.XPath.StartsWith(userType.XPath)) user.UserTypeId = userType.Id;
                        if (!user.TutorId.HasValue && schoolUser.UserTypeEnmu == UserIdentity.Student)
                        {
                            if (!string.IsNullOrWhiteSpace(schoolUser.TutorIdentityCardNo))
                            {
                                var tutorList = GetEntitiesByExpression(string.Format("(IdentityCardNo=\"{0}\"+Label=\"{0}\")*IsDel=false", schoolUser.TutorIdentityCardNo));
                                if (tutorList == null) throw new Exception(string.Format("找不到证件号为【{0}】的{1}身份类型", schoolUser.TutorIdentityCardNo, schoolUser.UserTypeEnmu.ToCnName()));
                                if (tutorList.Count > 1) throw new Exception(string.Format("找到{0}个证件号为【{1}】的{2}身份类型", tutorList.Count, schoolUser.TutorIdentityCardNo, schoolUser.UserTypeEnmu.ToCnName()));
                                var tutor = tutorList.First();
                                if ((tutor.UserType.UserIdentityEnum != UserIdentity.Tutor && tutor.UserType.UserIdentityEnum != UserIdentity.OutTutor)) throw new Exception(string.Format("证件号为【{0}】的用户身份类型【{1}】不是【{2}】身份类型", schoolUser.IdentityCardNo, tutor.UserType.UserIdentityEnum.ToCnName(), SchoolUserType.Tutor.ToCnName()));
                                user.TutorId = tutor.Id;
                                if (!GenerateStudentRelativeInfo(user, user.Tutor, ref tran, out errorMessage, out experimenterSubject)) throw new Exception(errorMessage);
                                user.IsNeedTutorAudit = false;
                                if (user.UserStatusEnum == UserStatus.AuditWaitting) user.UserStatusEnum = UserStatus.AuditPass;
                            }
                            else if (user.TutorAuditStatusEnum == TutorAuditStatus.WaitingAudit && !user.BindTutorId.HasValue)
                            {
                                user.IsNeedTutorAudit = true;
                                user.UserStatusEnum = UserStatus.AuditWaitting;
                            }
                        }

                    }
                }
                //if (!ChangeTutor(user, experimenterSubject, ref tran, out errorMessage)) throw new Exception(errorMessage);//处理改导师情况
                //if (!ChangeUserType(user, schoolUser, ref tran, out errorMessage)) throw new Exception(errorMessage);//处理改用户身份类型情况
                if (isNew)
                {
                    Add(new User[] { user }, ref tran, true);
                    if (schoolUser.UserTypeEnmu == UserIdentity.Student)
                    {
                        if (!GenerateStudentRelativeInfo(user, user.Tutor, ref tran, out errorMessage, out experimenterSubject)) throw new Exception(errorMessage);
                    }
                    if (schoolUser.UserTypeEnmu == UserIdentity.Tutor || schoolUser.UserTypeEnmu == UserIdentity.OutTutor)
                    {
                        if (!GenerateTutorRelativeInfo(user, ref tran, out errorMessage)) throw new Exception(errorMessage);
                    }
                }
                else
                {
                    Save(new User[] {user}, ref tran, true);
                }
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool GenerateTutorRelativeInfo(User user, ref XTransaction tran, out string errorMessage)
        {
            IWorkGroupBLL workGroupBLL = BLLFactory.CreateWorkGroupBLL();
            IUserWorkGroupBLL userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            ICreditLimitBLL creditLimitBLL = BLLFactory.CreateCreditLimitBLL();
            errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var subject = subjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", user.Id));
                if (subject == null)
                {
                    subject = new Subject()
                    {
                        Id = Guid.NewGuid(),
                        CreateAt = DateTime.Now,
                        SubjectDirectorId = user.Id,
                        IsDelete = false,
                        Name = string.Format("{0}课题组", user.UserName),
                        OddSum = 0d,
                        StatusEnum = SubjectStatus.Doing,
                        TotalSum = 0d
                    };
                    subjectBLL.Add(new Subject[] { subject }, ref tran, true);
                }
                var userAccount = userAccountBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*IsDelete=false", user.Id));
                if (userAccount == null)
                {
                    var creditLimit = creditLimitBLL.GenerateDefaultCreditLimit(ref tran);
                    userAccount = new UserAccount()
                    {
                        CreditLimitId = creditLimit.Id,
                        Id = Guid.NewGuid(),
                        IsDelete = false,
                        PreAlert = 0d,
                        UnAppointment = 0d,
                        UnUseable = 0d,
                        RealCurrency = 0d,
                        UserId = user.Id,
                        VirtualCurrency = 0d
                    };
                    userAccountBLL.Add(new UserAccount[] { userAccount }, ref tran, true);

                    var experimenterList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("IsDelete=false*ExperimenterId=\"{0}\"", user.Id));
                    if (experimenterList != null && experimenterList.Count > 0)
                    {
                        foreach (var experimenter in experimenterList)
                        {
                            if (experimenter.OwnerId.Value != experimenter.GetSubject().SubjectDirectorId.Value)//处理合作课题情况
                                experimenter.IsDelete = true;
                        }
                        experimenterSubjectBLL.Save(experimenterList, ref tran, true);
                    }
                }
                var workGroup = workGroupBLL.GenerateDefaultWorkGroup(user.IsOuter ? UserIdentity.OutTutor.ToCnName() : UserIdentity.Tutor.ToCnName(), ref tran);
                var userWorkGroup = userWorkGroupBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*WorkGroupId=\"{1}\"", user.Id, workGroup.Id));
                if (userWorkGroup == null)
                {
                    userWorkGroup = new UserWorkGroup() { Id = Guid.NewGuid(), UserId = user.Id, WorkGroupId = workGroup.Id };
                    userWorkGroupBLL.Add(new UserWorkGroup[] { userWorkGroup }, ref tran, true);
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool GenerateStudentRelativeInfo(User user, User tutor, ref XTransaction tran, out string errorMessage, out ExperimenterSubject experimenterSubject)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            ICreditLimitBLL creditLimitBLL = BLLFactory.CreateCreditLimitBLL();
            IUserWorkGroupBLL userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
            IWorkGroupBLL workGroupBLL = BLLFactory.CreateWorkGroupBLL();
            errorMessage = "";
            experimenterSubject = null;
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (tutor != null)
                {
                    var subject = subjectBLL.GetSubjectByTurtorId(tutor.Id);
                    if (subject == null) throw new Exception(string.Format("找不到{0}的课题组信息", tutor.UserName));
                    experimenterSubject = experimenterSubjectBLL.GetFirstOrDefaultEntityByExpression(string.Format("ExperimenterId=\"{0}\"*OwnerId=\"{1}\"*IsDelete=false", user.Id, tutor.Id));
                    if (experimenterSubject == null)
                    {
                        experimenterSubject = new ExperimenterSubject()
                        {
                            Id = Guid.NewGuid(),
                            ApplyAt = DateTime.Now,
                            ExperimenterId = user.Id,
                            IsAdmin = false,
                            IsDelete = false,
                            JoinAt = DateTime.Now,
                            OddSum = 0d,
                            OwnerId = tutor.Id,
                            PreAlert = 0d,
                            RealCurency = 0d,
                            StatusEnum = ExperimenterSubjectStatus.Authorized,
                            StopAt = null,
                            TotalSum = 0d,
                            SubjectId = subject.Id,
                            Unappointment = 0d,
                            UnUseable = 0d,
                            UseMoneyTypeEnum = ExperimenterSubjectUseMoneyType.TutorAccount
                        };
                        experimenterSubjectBLL.Add(new ExperimenterSubject[] { experimenterSubject }, ref tran, true);
                    }
                    GenerateUserAuditRelativeInfo(user, user.Organization, tutor.Id, null);
                }
                var workGroup = workGroupBLL.GenerateDefaultWorkGroup(UserIdentity.Student.ToCnName(), ref tran);
                var userWorkGroup = userWorkGroupBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*WorkGroupId=\"{1}\"", user.Id, workGroup.Id));
                if (userWorkGroup == null)
                {
                    userWorkGroup = new UserWorkGroup() { Id = Guid.NewGuid(), UserId = user.Id, WorkGroupId = workGroup.Id };
                    userWorkGroupBLL.Add(new UserWorkGroup[] { userWorkGroup }, ref tran, true);
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        private bool DelTutorRelativeInfo(User user, ref XTransaction tran, out string errorMessage)
        {
            ISubjectBLL subjectBLL = BLLFactory.CreateSubjectBLL();
            IExperimenterSubjectBLL experimenterSubjectBLL = BLLFactory.CreateExperimenterSubjectBLL();
            IUserAccountBLL userAccountBLL = BLLFactory.CreateUserAccountBLL();
            ICreditLimitBLL creditLimitBLL = BLLFactory.CreateCreditLimitBLL();
            errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var subjectList = subjectBLL.GetEntitiesByExpression(string.Format("SubjectDirectorId=\"{0}\"*IsDelete=false", user.Id));
                if (subjectList == null && subjectList.Count > 0)
                {
                    subjectList.ToList().ForEach(p => p.IsDelete = true);
                    subjectBLL.Save(subjectList, ref tran, true);
                }
                var userAccountList = userAccountBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsDelete=false"));
                if (userAccountList == null && userAccountList.Count > 0)
                {
                    userAccountList.ToList().ForEach(p => p.IsDelete = true);
                    userAccountBLL.Save(userAccountList, ref tran, true);
                }
                var experimenterSubjectList = experimenterSubjectBLL.GetEntitiesByExpression(string.Format("OwnerId=\"{0}\"*IsDelete=false", user.Id));
                if (experimenterSubjectList != null && experimenterSubjectList.Count > 0)
                {
                    foreach (var experimenterSubject in experimenterSubjectList)
                        experimenterSubject.IsDelete = true;
                    experimenterSubjectBLL.Save(experimenterSubjectList, ref tran, true);
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool ImportTutorByNames(IList<string> tutorNames, out IList<UserImportLog> importUserLogs, out string errorMessage)
        {
            ITagBLL tagBLL = BLLFactory.CreateTagBLL();
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            IUserTypeBLL userTypeBLL = BLLFactory.CreateUserTypeBLL();
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            importUserLogs = new List<UserImportLog>();
            errorMessage = "";
            try
            {
                if (tutorNames == null || tutorNames.Count == 0) throw new ArgumentException("导师姓名为空");
                foreach (var tutorName in tutorNames)
                {
                    XTransaction tran = SessionManager.Instance.BeginTransaction();
                    Guid userId = Guid.NewGuid();
                    string userName = tutorName, userTypeName = "导师", reason = "导入成功";
                    bool isSuccessed = true;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(tutorName)) throw new Exception("导师姓名为空");
                        var tutor = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"*IsDel=false", tutorName.Trim()));
                        if (tutor != null)
                        {
                            isSuccessed = false;
                            reason = string.Format("{0}已经存在", tutorName.Trim());
                        }
                        else
                        {
                            var userType = userTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserIdentity={0}*IsStop=false*IsDelete=false", (int)UserIdentity.Tutor));
                            if (userType == null)
                            {
                                isSuccessed = false;
                                reason = "找不到导师用户类型的定义";
                            }
                            var certificateType = dictCodeBLL.GenerateDefaultCertificateType("身份证", ref tran);
                            var tag = tagBLL.GenerateDefaultTag("校内", ref tran);
                            var loginName = ShortcutStringUtility.GetAllPYLetters(tutorName.Trim());
                            var user = new User() { Id = userId, UserName = tutorName, UserTypeId = userType.Id, CertificateTypeId = certificateType.Id, TagId = tag.Id, Name = loginName, Label = loginName, LoginPassword = MD5Utility.MD5hash(loginName), IsOuter = false };
                            Add(new User[] { user }, ref tran, true);
                            var result = GenerateTutorRelativeInfo(user, ref tran, out errorMessage);
                            if (!result)
                            {
                                isSuccessed = false;
                                reason = errorMessage;

                            }
                            if (tran.HasTransaction)
                            {
                                if (result) tran.CommitTransaction();
                                else tran.RollbackTransaction();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                        isSuccessed = false;
                        reason = ex.Message;
                    }
                    finally { if (tran != null) tran.Dispose(); }
                    importUserLogs.Add(new UserImportLog(userId, userName, userTypeName, isSuccessed, reason));
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        public override bool Add(IEnumerable<User> entities, ref XTransaction tran, bool isSupress = false)
        {
            FitUserCard(entities, ref tran, true);
            return base.Add(entities, ref tran, isSupress);
        }
        public override bool Save(IEnumerable<User> entities, ref XTransaction tran, bool isSupress = false)
        {
            FitUserCard(entities, ref tran, true);
            SysUserUpdated(entities, ref tran, true);
            return base.Save(entities, ref tran, isSupress);
        }
        private void FitUserCard(IEnumerable<User> entities, ref XTransaction tran, bool isSupress = false)
        {
            var businessIdBLL = BLLFactory.CreateBusinessIdBLL();
            if (entities != null && entities.Count() > 0)
            {
                var fitCardUserList = entities.Where(p => (!p.IsDel.HasValue || !p.IsDel.Value) && (!p.Card.HasValue || p.Card.Value == 0 || (p.Card.Value == 1 && !p.IsSuperAdmin)));
                if (fitCardUserList != null && fitCardUserList.Count() > 0)
                {
                    var isNewBusinessId = false;
                    var maxRandomCard = 10000;
                    var maxRandomCardBusinessId = businessIdBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"UserRandomCard\""));
                    if (maxRandomCardBusinessId != null) maxRandomCard = maxRandomCardBusinessId.Value;
                    else
                    {
                        maxRandomCardBusinessId = new BusinessId();
                        maxRandomCardBusinessId.Id = Guid.NewGuid();
                        maxRandomCardBusinessId.Code = "UserRandomCard";
                        isNewBusinessId = true;
                    }
                    foreach (var item in fitCardUserList)
                    {
                        bool isOk = false;
                        while (!isOk)
                        {
                            isOk = true;
                            if (entities.Count(p => p.Card.Value == (int)(uint.Parse(maxRandomCard.ToString()))) > 0)
                            {
                                isOk = false;
                                maxRandomCard++;
                                continue;
                            }
                            if (CountModelListByExpression(string.Format("IsDel=false*Card=\"{0}\"*{1}", (int)(uint.Parse(maxRandomCard.ToString())), entities.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot))) > 0)
                            {
                                isOk = false;
                                maxRandomCard++;
                                continue;
                            }
                            item.Card = (int)(uint.Parse(maxRandomCard.ToString()));
                            item.RealCard = double.Parse(maxRandomCard.ToString());
                        }
                    }
                    maxRandomCardBusinessId.Value = ++maxRandomCard;
                    if (isNewBusinessId)
                        businessIdBLL.Add(new BusinessId[] { maxRandomCardBusinessId }, ref tran, isSupress);
                    else
                        businessIdBLL.Save(new BusinessId[] { maxRandomCardBusinessId }, ref tran, isSupress);
                }
            }
        }
        private void SysUserUpdated(IEnumerable<User> entities, ref XTransaction tran, bool isSupress = false)
        {
            IUserUpdatedBLL userUpdatedBLL = BLLFactory.CreateUserUpdatedBLL();
            if (entities != null && entities.Count() > 0)
            {
                var delUserUpdatedList = userUpdatedBLL.GetEntitiesByExpression(entities.Select(p => p.Id).ToFormatStr("UserId"));
                if (delUserUpdatedList != null && delUserUpdatedList.Count() > 0)
                    userUpdatedBLL.Delete(delUserUpdatedList.Select(p => p.Id), ref tran, isSupress);
            }
        }
        public bool ExportSlwUser(IList<User> entities, out  string errorMessage)
        {
            errorMessage = "";
            if (entities == null || entities.Count() == 0)
            {
                return false;
            }
            foreach (var item in entities)
            {
                XTransaction tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    using (var session = SessionManager.Instance.GetSession().BaseSession)
                    {
                        using (var cmd = session.Connection.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            {
                                cmd.CommandText = @"if NOT exists(SELECT 1 FROM Basic WHERE CARDNO = @RealCard)INSERT INTO Basic
	                                            (NUM,CARDNO,NAME,Sex,DEPNO,RYLB,ADDR,CERTIFICATE,PSWD,
	                                            CARDFEE,LEFTSUM,DATETIME,GS,DESTORY,STATDATE
	                                            )
                                                 
	                                            (select isnull((select max(NUM)+1 from Basic),1),@RealCard,@name,@sex,0,0,@ContactAddress,'',
	                                            null,0,0,getdate(),0,0,getdate()
	                                            )
                                                else
                                                UPDATE Basic set NAME=@name,Sex=@sex,ADDR=@ContactAddress,DATETIME=getdate() where CARDNO=@RealCard
                                                ";
                            }
                            #region User属性赋值,可以优化用反射
                            var Parameter_RealCard = cmd.CreateParameter();
                            Parameter_RealCard.ParameterName = "@RealCard";
                            Parameter_RealCard.Value = CoverCard2(Convert.ToUInt32(item.RealCard));
                            cmd.Parameters.Add(Parameter_RealCard);

                            var Parameter_Name = cmd.CreateParameter();
                            Parameter_Name.ParameterName = "@name";
                            Parameter_Name.Value = item.UserName;
                            cmd.Parameters.Add(Parameter_Name);

                            var Parameter_Sex = cmd.CreateParameter();
                            Parameter_Sex.ParameterName = "@sex";
                            if (item.Sex.HasValue)
                            {
                                if (item.Sex.Value)
                                    Parameter_Sex.Value = "女";
                                else
                                    Parameter_Sex.Value = "男";
                            }
                            else
                                Parameter_Sex.Value = "";
                            cmd.Parameters.Add(Parameter_Sex);

                            var Parameter_ContactAddress = cmd.CreateParameter();
                            Parameter_ContactAddress.ParameterName = "@ContactAddress";
                            Parameter_ContactAddress.Value = item.ContactAddress == null ? "" : item.ContactAddress;
                            cmd.Parameters.Add(Parameter_ContactAddress);
                            #endregion
                            cmd.ExecuteNonQuery();
                            if (tran.HasTransaction) tran.CommitTransaction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //  entities.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally { if (tran != null) tran.Dispose(); }
            }

            return true;
        }
        public int CoverCard(uint RealCard)
        {
            var tempCard = RealCard;
            //var reverseUintCard = uint.Parse(tempCard);
            char[] c1 = tempCard.ToString("X").PadLeft(8, '0').ToCharArray();
            char[] c2 = new char[8];
            c2[0] = c1[6];
            c2[1] = c1[7];
            c2[2] = c1[4];
            c2[3] = c1[5];
            c2[4] = c1[2];
            c2[5] = c1[3];
            c2[6] = c1[0];
            c2[7] = c1[1];
            try
            {
                tempCard = uint.Parse(new string(c2), System.Globalization.NumberStyles.AllowHexSpecifier);//16转10
                int reverseCard = (int)tempCard;
                int card = (int)tempCard;
                return card;
            }
            catch { return 0; }
        }
        public long CoverCard2(uint RealCard)
        {
            var tempCard = RealCard;
            //var reverseUintCard = uint.Parse(tempCard);
            char[] c1 = tempCard.ToString("X").PadLeft(8, '0').ToCharArray();
            char[] c2 = new char[8];
            c2[0] = c1[6];
            c2[1] = c1[7];
            c2[2] = c1[4];
            c2[3] = c1[5];
            c2[4] = c1[2];
            c2[5] = c1[3];
            c2[6] = c1[0];
            c2[7] = c1[1];
            try
            {
                tempCard = uint.Parse(new string(c2), System.Globalization.NumberStyles.AllowHexSpecifier);//16转10
                long reverseCard = (long)tempCard;
                long card = (long)tempCard;
                return card;
            }
            catch { return 0; }
        }
        public bool SyncZjdxUser(int syncCountPerTime, Guid? operatorId, out int totalCount, out int successCount, out int failCount, out string errorMessage)
        {
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            ISchoolUserBLL schoolUserBLL = BLLFactory.CreateSchoolUserBLL();
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            ITemperatureHumiditySetupBLL temperatureHumiditySetupBLL = BLLFactory.CreateTemperatureHumiditySetupBLL();
            //string url = dictCodeBLL.GetDictCodeValueByCode("ZjdxLogin", "getChangedUserUrl");
            object[] args = new object[]
                {
                    "yxbggjszctx",
                    "20141126_bn"
                };
            object result = temperatureHumiditySetupBLL.SendTemperatureHumidity("http://zuinfo.zju.edu.cn/services/IdentityWebService", "getChangedUser", args);

            XmlDocument doc = new XmlDocument();
            try
            {
                string getXmlStr = (string)result;
                doc.LoadXml(getXmlStr);
                XmlNodeList IdentityList = doc.GetElementsByTagName("Identity");
                IList<SchoolUser> schoolUserList = new List<SchoolUser>();
                foreach (XmlNode node in IdentityList)
                {
                    SchoolUser schoolUser = new SchoolUser();
                    schoolUser.TagName = "校内";
                    schoolUser.Id = Guid.NewGuid().ToString();
                    XmlNodeList childList = node.ChildNodes;
                    foreach (XmlNode userNode in childList)
                    {
                        if (userNode.Name == "ID") schoolUser.LoginName = userNode.InnerText;
                        if (userNode.Name == "Name") schoolUser.Name = userNode.InnerText;
                        if (userNode.Name == "Gender") schoolUser.Sex = int.Parse(userNode.InnerText == "1" ? "1" : "0");
                        if (userNode.Name == "Mobile") schoolUser.PhoneNumber = userNode.InnerText;
                        if (userNode.Name == "Email") schoolUser.Email = userNode.InnerText;
                        if (userNode.Name == "Grade") schoolUser.Grade = userNode.InnerText;
                        if (userNode.Name == "CertificateType") schoolUser.CertificateType = userNode.InnerText;
                        if (userNode.Name == "CredentialID") schoolUser.IdentityCardNo = userNode.InnerText;
                        if (userNode.Name == "Department") schoolUser.OrganizationName = userNode.InnerText;
                        if (userNode.Name == "CardNumber")
                        {
                            if (userNode.InnerText != "")
                                schoolUser.Card = CoverCard2(Convert.ToUInt32(userNode.InnerText)).ToString();
                            else
                                schoolUser.Card = "";
                        }
                        if (userNode.Name == "UserType") schoolUser.UserType = int.Parse(userNode.InnerText == "学校编制教职工" ? "4" : "2");
                        if (userNode.Name == "Title") schoolUser.Jobtitle = userNode.InnerText;
                        if (userNode.Name == "MajorName") schoolUser.Speciality = userNode.InnerText;
                        if (userNode.Name == "IsDelete")
                        {
                            if (userNode.InnerText == "1")
                            {
                                string errorMsg = "";
                                var user = userBLL.GetEntitiesByExpression(string.Format("Label=\"{0}\"", schoolUser.LoginName));
                                if (user != null)
                                {
                                    userBLL.DelUser(user[0].Id, out errorMsg);
                                    schoolUser = null;
                                    break;
                                }
                            }
                        }
                    }
                    if (schoolUser != null)
                    {
                        schoolUser.ResearchDirection = "";
                        schoolUser.TutorName = "";
                        schoolUser.TutorIdentityCardNo = "";
                        schoolUserList.Add(schoolUser);
                    }
                }
                schoolUserBLL.ImportSchoolUser(schoolUserList);
                schoolUserBLL.DealImportSchoolUserList(schoolUserList);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //sr1.Dispose();
                //sr1.Close();
                //sr2.Dispose();
                //sr2.Close();
                //sr3.Dispose();
                //sr3.Close();
                //s.Dispose();
                //s.Close();  
            }
            return true;
        }
        public void GenerateUserAuditRelativeInfo(User user, LabOrganization organization, Guid? tutorId, Guid? operatorId)
        {
            IUserSystemSettingBLL userSystemSettingBLL = BLLFactory.CreateUserSystemSettingBLL();
            var oldUser = GetEntityById(user.Id);
            if (oldUser == null || (user.UserType != null && user.UserType.UserIdentityEnum == UserIdentity.Student && oldUser != null && oldUser.TutorId == null && user.TutorId != null))
            {
                if (organization != null)
                {
                    user.IsNeedTutorAudit = organization.IsUserNeedTutorAudit && (user.UserType != null && user.UserType.UserIdentityEnum == UserIdentity.Student);
                    user.IsNeedAdminAudit = organization.IsUserNeedAdminAudit;
                    if (user.IsNeedTutorAudit)
                    {
                        var userSystemSetting = userSystemSettingBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"", user.TutorId.Value));
                        if (userSystemSetting != null && userSystemSetting.IsStudentAutoPassWhenNeedAudit)
                        {
                            user.TutorAuditStatusEnum = TutorAuditStatus.Passed;
                            user.TutorAuditRemark = "导师设置自动审核通过";
                            user.TutorAuditTime = DateTime.Now;
                        }
                    }
                    if (user.UserStatusEnum == UserStatus.AuditWaitting && !user.IsNeedAdminAudit && (!user.IsNeedTutorAudit || user.TutorAuditStatusEnum == TutorAuditStatus.Passed))
                    {
                        user.UserStatusEnum = UserStatus.AuditPass;
                        user.AuditTime = DateTime.Now;
                        user.AuthorizeTime = DateTime.Now;
                    }
                }
            }
        }
        //public bool SyncZjdxUser(int syncCountPerTime, Guid? operatorId, out int totalCount, out int successCount, out int failCount, out string errorMessage)
        //{
        //    totalCount = 0;
        //    successCount = 0;
        //    failCount = 0;
        //    errorMessage = "";

        //    //string url = "http://zuinfo.zju.edu.cn/services/IdentityWebService?method=getChangedUser&manager=publictest&password=zjuinfo";
        //    //WebRequest request = WebRequest.Create(url);
        //    //WebResponse response = request.GetResponse();
        //    //Stream s = response.GetResponseStream();
        //    //StreamReader sr = new StreamReader(s, Encoding.Default);           
        //    //string getXmlStr = sr.ReadToEnd();
        //    //getXmlStr = getXmlStr.Replace("&lt;", "<");
        //    //getXmlStr = getXmlStr.Replace("&gt;", ">");
        //    //XmlDocument doc = new XmlDocument();
        //    //doc.LoadXml(getXmlStr);
        //    //sr.Dispose();
        //    //sr.Close();
        //    //s.Dispose();
        //    //s.Close();  
        //    IUserBLL userBLL = BLLFactory.CreateUserBLL();
        //    ISchoolUserBLL schoolUserBLL = BLLFactory.CreateSchoolUserBLL();
        //    XmlDocument doc = new XmlDocument();
        //    FileStream aFile = new FileStream("aaa.txt", FileMode.Open);
        //    StreamReader sr = new StreamReader(aFile, Encoding.GetEncoding("gb2312"));
        //    try
        //    {                
        //        string getXmlStr = sr.ReadToEnd();
        //        getXmlStr = getXmlStr.Replace("&lt;", "<");
        //        getXmlStr = getXmlStr.Replace("&gt;", ">");
        //        int position = getXmlStr.LastIndexOf("<?xml");
        //        int getLastStr = getXmlStr.LastIndexOf("</getChangedUserReturn>");
        //        int xmlStrLengt = getXmlStr.Length;
        //        getXmlStr = getXmlStr.Substring(position, getLastStr - position);
        //        doc.LoadXml(getXmlStr);
        //        XmlNodeList IdentityList = doc.GetElementsByTagName("Identity");
        //        IList<SchoolUser> schoolUserList = new List<SchoolUser>();
        //        foreach (XmlNode node in IdentityList)
        //        {
        //            SchoolUser schoolUser = new SchoolUser();
        //            schoolUser.TagName = "校内";
        //            schoolUser.Id = Guid.NewGuid().ToString();
        //            XmlNodeList childList = node.ChildNodes;
        //            foreach (XmlNode userNode in childList)
        //            {                        
        //                if (userNode.Name == "ID") schoolUser.LoginName = userNode.InnerText;
        //                if (userNode.Name == "Name") schoolUser.Name = userNode.InnerText;
        //                if (userNode.Name == "Gender") schoolUser.Sex = int.Parse(userNode.InnerText =="1"?"1":"0");
        //                if (userNode.Name == "Mobile") schoolUser.PhoneNumber = userNode.InnerText;
        //                if (userNode.Name == "Email") schoolUser.Email = userNode.InnerText;
        //                if (userNode.Name == "Grade") schoolUser.Grade = userNode.InnerText;
        //                if (userNode.Name == "CertificateType") schoolUser.CertificateType = userNode.InnerText;
        //                if (userNode.Name == "CredentialID") schoolUser.IdentityCardNo = userNode.InnerText;
        //                if (userNode.Name == "Department") schoolUser.OrganizationName = userNode.InnerText;
        //                if (userNode.Name == "CardNumber")
        //                {
        //                    if (userNode.InnerText != "")
        //                        schoolUser.Card = CoverCard2(Convert.ToUInt32(userNode.InnerText)).ToString();
        //                    else
        //                        schoolUser.Card = "";
        //                }
        //                if (userNode.Name == "UserType") schoolUser.UserType = int.Parse(userNode.InnerText == "学校编制教职工" ? "4" : "2");
        //                if (userNode.Name == "Title") schoolUser.Jobtitle = userNode.InnerText;
        //                if (userNode.Name == "MajorName") schoolUser.Speciality = userNode.InnerText;
        //                if (userNode.Name == "IsDelete")
        //                {
        //                    if (userNode.InnerText == "1")
        //                    {
        //                        string errorMsg = "";
        //                        var user = userBLL.GetEntitiesByExpression(string.Format("Label=\"{0}\"", schoolUser.LoginName));
        //                        if (user != null)
        //                        {
        //                            userBLL.DelUser(user[0].Id, out errorMsg);
        //                            schoolUser = null;
        //                            break;
        //                        }
        //                    }
        //                }                        
        //            }
        //            if (schoolUser != null)
        //            {
        //                schoolUser.ResearchDirection = "";
        //                schoolUser.TutorName = "";
        //                schoolUser.TutorIdentityCardNo = "";
        //                schoolUserList.Add(schoolUser);
        //            }
        //        }
        //        schoolUserBLL.ImportSchoolUser(schoolUserList);
        //        schoolUserBLL.DealImportSchoolUserList(schoolUserList);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        aFile.Close();
        //        sr.Close();
        //    }
        //    return true;
        //}

    }
}
