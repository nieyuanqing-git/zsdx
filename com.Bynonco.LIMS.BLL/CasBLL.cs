using com.august.DataLink;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class CasBLL:ICasBLL
    {
        private static IDictCodeTypeBLL __dictCodeTypeBLL;
        private static IDictCodeBLL __dictCodeBLL;
        private static IUserBLL __userBLL;
        private static IViewSchoolUserBLL __viewSchoolUserBLL;
        private static ISchoolUserBLL __schoolUserBLL;
        private static CasDataObject __casDataObject = null;
        private static IDictionary<string, string> __casUserInfoMapping = null;
        private static string __userLoginMode = null;
        public CasBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            __schoolUserBLL = BLLFactory.CreateSchoolUserBLL();
        }
        public CasDataObject GetCasDataObject(string sessionName)
        {
            if (__casDataObject == null)
            {
                var dictcodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=CasLogin");
                string casLoginURL = dictcodeType.DictCodes.First(p => p.Code == "casLoginURL").Value;
                string casLogoutURL = dictcodeType.DictCodes.First(p => p.Code == "casLogoutURL").Value;
                string casValidateSuccessCallBackURL = URLConverterUtility.ToMVCUrl(dictcodeType.DictCodes.First(p => p.Code == "casValidateSuccessCallBackURL").Value);
                string casValidateURL = dictcodeType.DictCodes.First(p => p.Code == "casValidateURL").Value;
                string serviceURL = URLConverterUtility.ToMVCUrl(dictcodeType.DictCodes.First(p => p.Code == "serviceURL").Value);
                int identityNoPosition = int.Parse(dictcodeType.DictCodes.First(p => p.Code == "casResponseIdenityNoPosition").Value);
                CasLoginMode casLoginMode = CasLoginMode.ShowLocalLogin;
                var casLoginModeDictCode = dictcodeType.DictCodes.FirstOrDefault(p => p.Code == "casLoginMode" && !string.IsNullOrWhiteSpace(p.Value));
                if (casLoginModeDictCode != null) casLoginMode = (CasLoginMode)int.Parse(casLoginModeDictCode.Value.Trim());
                bool isUseCasUserInfoMapping = false, isAutoRegistWhenUserNotExist = false, isUseUserNotFindMode = false;
                var isUseCasUserInfoMappingDictCode = dictcodeType.DictCodes.FirstOrDefault(p => p.Code == "isUseCasUserInfoMapping" && !string.IsNullOrWhiteSpace(p.Value));
                if (isUseCasUserInfoMappingDictCode != null) isUseCasUserInfoMapping = isUseCasUserInfoMappingDictCode.Value.Trim() == "1";
                var isAutoRegistWhenUserNotExistDictCode = dictcodeType.DictCodes.FirstOrDefault(p => p.Code == "isAutoRegistWhenUserNotExist" && !string.IsNullOrWhiteSpace(p.Value));
                if (isAutoRegistWhenUserNotExistDictCode != null)
                {
                    isAutoRegistWhenUserNotExist = isAutoRegistWhenUserNotExistDictCode.Value.Trim() == "1";
                }
                var isUseUserNotFindModeDictCode = dictcodeType.DictCodes.FirstOrDefault(p => p.Code == "isUseUserNotFindMode" && !string.IsNullOrWhiteSpace(p.Value));
                if (isUseUserNotFindModeDictCode != null) isUseUserNotFindMode = isUseUserNotFindModeDictCode.Value.Trim() == "1";
                __casDataObject = new CasDataObject(sessionName, casLoginURL, casLogoutURL, casValidateSuccessCallBackURL, casValidateURL, serviceURL, identityNoPosition, casLoginMode, isUseCasUserInfoMapping, isAutoRegistWhenUserNotExist, isUseUserNotFindMode);
            }
            return __casDataObject;
        }
        public IDictionary<string, string> GetCasUserInfoMapping()
        {
            if (__casUserInfoMapping == null)
            {
                var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=CasUserInfoMapping");
                if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
                {
                    __casUserInfoMapping = new Dictionary<string, string>();
                    foreach (var dictCode in dictCodeType.DictCodes)
                        __casUserInfoMapping[dictCode.Code] = dictCode.Value;
                }
            }
            return __casUserInfoMapping;
        }

        public string GetLoginUrl()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("CasLogin", "LoginUrl");
        }

        public string GetTopLoginUrl()
        {
            return __dictCodeBLL.GetDictCodeValueByCode("CasLogin", "TopLoginUrl");
        }

        public UserLoginMode GetUserLoginMode()
        {
            if (string.IsNullOrWhiteSpace(__userLoginMode))
            {
                __userLoginMode = __dictCodeBLL.GetDictCodeValueByCode("System", "UserLoginMode");
            }
            try
            {
                return (UserLoginMode)System.Enum.ToObject(typeof(UserLoginMode), int.Parse(string.IsNullOrWhiteSpace(__userLoginMode) ? "0" : __userLoginMode));
            }
            catch
            {
                return UserLoginMode.Local;
            }
        }
        private bool ExecGenerateSchoolUser(string loginName,out string errorMessage)
        {
            errorMessage = "";
            var loginNameDataParam = DataParameterFactory.CreateDataParameter("loginName", loginName);
            var errorMsgDataParam = DataParameterFactory.CreateDataParameter("errorMsg", null, 200, System.Data.ParameterDirection.Output);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, System.Data.ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>() { loginNameDataParam, errorMsgDataParam, returnValueDataParam };
            __userBLL.ProcedureAdapter.ExecuteProcedure("ProGenerateSchoolUser", dataParams);
            return returnValueDataParam.Value != null && returnValueDataParam.Value.ToString().Trim() == "1";
        }
        public void CasLoginSuccessCallBack(com.Bynonco.LIMS.Utility.CasUserInfo casUserInfo,out string errorMessage)
        {
            errorMessage = "";
            string netId = casUserInfo != null && !string.IsNullOrWhiteSpace(casUserInfo.NetId) ? casUserInfo.NetId.Trim() : "";
            netId = casUserInfo != null && !string.IsNullOrWhiteSpace(casUserInfo.LoginName) ? casUserInfo.LoginName.Trim() : netId;
            if (!string.IsNullOrWhiteSpace(netId))
            {
                var user = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDel=false", netId));
                if (user == null)
                {
                    if (__casDataObject.IsUseUserNotFindMode)
                    {
                        System.Web.HttpContext.Current.Response.Redirect(__casDataObject.GenerateUserUnFindCasVisitedURL());
                        System.Web.HttpContext.Current.Response.End();
                    }
                    bool isNeedSyncSchoolUser = false;
                    var queryExpression = string.Format("LoginName=\"{0}\"", netId);
                    int count = __viewSchoolUserBLL.CountModelListByExpression(queryExpression);
                    if (count > 0) isNeedSyncSchoolUser = true;
                    if (count == 0  && casUserInfo != null)
                    {

                        bool isSuccess = ExecGenerateSchoolUser(netId, out errorMessage);
                        if (isSuccess) isNeedSyncSchoolUser = true;
                        if (!isSuccess && __casDataObject.IsAutoRegistWhenUserNotExist && casUserInfo.Validate(out errorMessage))
                        {
                            string tutorLoginName = casUserInfo.DirectorLoginName;
                            if (!string.IsNullOrWhiteSpace(tutorLoginName))
                            {
                                var tutorCount = __userBLL.CountModelListByExpression(string.Format("IsDel=false*Label=\"{0}\"", tutorLoginName.Trim()));
                                if (tutorCount == 0) tutorLoginName = "";
                            }
                            isNeedSyncSchoolUser = true;
                            SchoolUser schoolUser = new SchoolUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Speciality = casUserInfo.Specialty,
                                LoginName = casUserInfo.NetId,
                                TutorName = casUserInfo.Director,
                                Jobtitle = casUserInfo.JobTitle,
                                Sex = bool.Parse(casUserInfo.Sex) ? 1 : 0,
                                PhoneNumber = casUserInfo.Mobile,
                                TagName = casUserInfo.Degree,
                                TutorIdentityCardNo = tutorLoginName,
                                OrganizationName = casUserInfo.Unit,
                                IsOuter = bool.Parse(casUserInfo.IsOutSchool),
                                CertificateType = casUserInfo.IdentityTypeName,
                                IdentityCardNo = casUserInfo.Identity,
                                Grade = casUserInfo.Grade,
                                Name = casUserInfo.Name,
                                Email = casUserInfo.Email,
                                Remark = casUserInfo.Remark,
                                UserType = casUserInfo.IsTutor ? (int)com.Bynonco.LIMS.Model.Enum.UserIdentity.Tutor : (int)com.Bynonco.LIMS.Model.Enum.UserIdentity.Student
                            };
                            IList<SchoolUser> schoolUserList = new List<SchoolUser>() { schoolUser };
                            schoolUserList = __schoolUserBLL.ImportSchoolUser(schoolUserList);
                            if (schoolUserList == null || schoolUserList.Count == 0) isNeedSyncSchoolUser = false;
                        }
                    }
                    if (isNeedSyncSchoolUser)
                    {
                        string errorMsg = "";
                        int syscCountPerTime = 1, totalCount = 0, successCount = 0, failCount = 0;
                        __schoolUserBLL.SyncSchoolUser(queryExpression, syscCountPerTime, null, out totalCount, out successCount, out failCount, out errorMsg);
                    }
                }
                else if (string.IsNullOrWhiteSpace(user.NetId))
                {
                    XTransaction tran = null;
                    user.NetId = netId;
                    __userBLL.Save(new User[] { user }, ref tran);
                }
            }
        }
    }
}
