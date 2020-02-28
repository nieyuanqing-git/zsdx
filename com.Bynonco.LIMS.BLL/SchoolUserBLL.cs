using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;
using System.Threading;
using log4net;

namespace com.Bynonco.LIMS.BLL
{
    public class SchoolUserBLL :ISchoolUserBLL
    {
        private static readonly ILog __log = LogManager.GetLogger(typeof(SchoolUserBLL));
        public void SplitMobileTelNo(string phoneNo, out string mobileNo, out string telNo)
        {
            mobileNo = "";
            telNo = "";
            if (!string.IsNullOrWhiteSpace(phoneNo))
            {
                IList<string> phoneNos = new List<string>();
                if (phoneNo.IndexOf(",") != -1 || phoneNo.IndexOf(",") != -1)
                {
                    phoneNos = phoneNo.Replace("，", ",").Split(',').ToList();
                }
                else if (phoneNo.IndexOf(";") != -1 || phoneNo.IndexOf("；") != -1)
                {
                    phoneNos = phoneNo.Replace("；", ";").Split(';').ToList();
                }
                else if (phoneNo.IndexOf("、") != -1)
                {
                    phoneNos = phoneNo.Split('、').ToList();
                }
                else if (phoneNo.IndexOf("/") != -1)
                {
                    phoneNos = phoneNo.Split('/').ToList();
                }
                else if (phoneNo.IndexOf(" ") != -1)
                {
                    phoneNos = phoneNo.Split(' ').ToList();
                }
                else
                {
                    phoneNos.Add(phoneNo);
                }
                foreach (var phoneNoTemp in phoneNos)
                {
                    if (phoneNoTemp.Trim().Length == 11 && phoneNoTemp.Trim().Substring(0, 1) == "1")
                    {
                        mobileNo = string.IsNullOrWhiteSpace(mobileNo) ? phoneNoTemp.Trim() : "," + phoneNoTemp.Trim();
                    }
                    else
                    {
                        telNo = string.IsNullOrWhiteSpace(mobileNo) ? phoneNoTemp.Trim() : "," + phoneNoTemp.Trim();
                    }
                }
            }
        }
        private string GenerateErroMessage(ViewSchoolUser schoolUser,SchoolUserLogType schoolUserLogType, string errorMessage)
        {
            return string.Format("当前校级用户信息：姓名【{0}】编码【{1}】导入{2}，原因：{3}", schoolUser.Name, schoolUser.SchoolUserId, schoolUserLogType.ToCnName(), errorMessage);
        }
        private string GeneratorUpdateInfo(ViewSchoolUser schoolUser, User user)
        {
            StringBuilder sbRemark = new StringBuilder();
            if (schoolUser.Name != user.UserName)
            {
                var remak = string.Format("用户名从【{0}】→【{1}】", user.UserName, schoolUser.Name);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.SexStr != user.SexStr)
            {
                var remak = string.Format("性别从【{0}】→【{1}】", user.SexStr, schoolUser.SexStr);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            string telNo = "", mobileNo = "";
            SplitMobileTelNo(schoolUser.PhoneNumber, out mobileNo, out telNo);
            if (telNo != user.FixedPhone)
            {
                var remak = string.Format("固话从【{0}】→【{1}】", user.FixedPhone, telNo);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (mobileNo != user.PhoneNumber)
            {
                var remak = string.Format("手机从【{0}】→【{1}】", user.PhoneNumber, mobileNo);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.Email != user.Email)
            {
                var remak = string.Format("邮箱从【{0}】→【{1}】", user.Email, schoolUser.Email);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.Grade != user.Grade)
            {
                var remak = string.Format("入学年份从【{0}】→【{1}】", user.Grade, schoolUser.Grade);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if ((user.CertificateType == null && !string.IsNullOrWhiteSpace(schoolUser.CertificateType)) || schoolUser.CertificateType != user.CertificateType.Name)
            {
                var remak = string.Format("证件类型年份从【{0}】→【{1}】", schoolUser.CertificateType == null ? "" : user.CertificateType.Name, schoolUser.CertificateType);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.Card != user.IdentityCardNo)
            {
                var remak = string.Format("证件号从【{0}】→【{1}】", user.IdentityCardNo, schoolUser.Card);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            //if ((user.Card == null && !string.IsNullOrWhiteSpace(schoolUser.Card)) || schoolUser.Card != user.Card.Value.ToString())
            //{
            //    var remak = string.Format("卡号从【{0}】→【{1}】", user.Card.HasValue ? user.Card.Value.ToString() : "", schoolUser.Card);
            //    sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            //}
            if (user.UserType == null || schoolUser.UserType != user.UserType.UserIdentity.Value)
            {
                var remak = string.Format("用户类型从【{0}】→【{1}】", user.UserType != null && user.UserType.UserIdentity.HasValue ? user.UserType.UserIdentityEnum.ToCnName() : "", schoolUser.UserTypeEnmu.ToCnName());
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if ((user.Tag == null && !string.IsNullOrWhiteSpace(schoolUser.TagName)) || (user.Tag != null && (schoolUser.TagName != user.Tag.Name)))
            {
                var remak = string.Format("身份类型从【{0}】→【{1}】", user.Tag != null ? user.Tag.Name : "", schoolUser.TagName);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.ValidityTime != user.ValidityTime)
            {
                var remak = string.Format("有效登录日期从【{0}】→【{1}】", user.ValidityTime.HasValue ? user.ValidityTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "", schoolUser.ValidityTime.HasValue ? schoolUser.ValidityTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.Jobtitle != user.Jobtitle)
            {
                var remak = string.Format("职称号从【{0}】→【{1}】", user.Jobtitle, schoolUser.Jobtitle);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.ResearchDirection != user.ResearchDirection)
            {
                var remak = string.Format("研究方向从【{0}】→【{1}】", user.ResearchDirection, schoolUser.ResearchDirection);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (string.IsNullOrWhiteSpace(schoolUser.TutorName)) schoolUser.TutorName = "";
            if (schoolUser.TutorName != user.TutorName)
            {
                var remak = string.Format("导师从【{0}】→【{1}】", user.TutorName, schoolUser.TutorName);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (schoolUser.LoginName != user.Label)
            {
                var remak = string.Format("登录名从【{0}】→【{1}】", user.TutorName, schoolUser.TutorName);
                sbRemark.Append(sbRemark.ToString() == "" ? remak : "," + remak);
            }
            if (sbRemark.ToString() == "") sbRemark.Append("未修改该用户信息");
            return sbRemark.ToString();
        }
        private void FinishSync(string schoolUserId)
        {
            using (var session = SessionManager.Instance.GetSession().BaseSession)
            {
                using (var cmd = session.Connection.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("UPDATE SchoolUser SET IsHandled=1 WHERE SchoolUserId='{0}';IF OBJECT_ID ('dbo.njnu_ryxx') IS NOT NULL BEGIN UPDATE dbo.njnu_ryxx SET IND_UPDATE = '1' WHERE RYBH='{0}' END;", schoolUserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool SyncSchoolUser(string syncQueryExpression,int syncCountPerTime,Guid? operatorId,out int totalCount, out int successCount, out int failCount,out string errorMessage)
        {
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            IViewSchoolUserBLL viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            try
            {
                var viewSchoolUserList = viewSchoolUserBLL.GetUnsyncViewSchoolUserList(syncQueryExpression,syncCountPerTime);
                if (viewSchoolUserList != null && viewSchoolUserList.Count > 0)
                {
                    totalCount = viewSchoolUserList.Count;
                    foreach (var viewSchoolUser in viewSchoolUserList)
                    {
                        XTransaction tran = SessionManager.Instance.BeginTransaction();
                        User user = null;
                        //var identityNo = string.IsNullOrWhiteSpace(viewSchoolUser.IdentityCardNo) ? viewSchoolUser.LoginName : viewSchoolUser.IdentityCardNo;
                        //var loginName = string.IsNullOrWhiteSpace(viewSchoolUser.LoginName) ? identityNo : viewSchoolUser.LoginName;
                        //var isNew = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDel=false", loginName)) == null;
                        try
                        {                           
                            var result = userBLL.GenerateUserRelativeInfo(viewSchoolUser, "000", ref tran, out errorMessage, out user);
                            //WriteSchoolUserLog(viewSchoolUser, user, DateTime.Now, GenerateErroMessage(viewSchoolUser, !result ? SchoolUserLogType.Failed : SchoolUserLogType.Successed, !result ? errorMessage : "导入成功"), operatorId, !result ? SchoolUserLogType.Failed : SchoolUserLogType.Successed, ref tranTemp);
                            if (result)
                            {
                                if (tran.HasTransaction) tran.CommitTransaction();
                                //if (!isNew)
                                //{
                                //    tranTemp = null;
                                //    var remark = GeneratorUpdateInfo(viewSchoolUser, user);
                                //    WriteSchoolUserLog(viewSchoolUser, user, DateTime.Now, remark, operatorId, SchoolUserLogType.Successed, ref tranTemp);
                                //}
                                FinishSync(viewSchoolUser.SchoolUserId);
                            }
                            else __log.ErrorFormat("{0}:{1}", DateTime.Now, errorMessage);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                            //WriteSchoolUserLog(viewSchoolUser, null, DateTime.Now, ex.Message, operatorId, SchoolUserLogType.Failed, ref tranTemp);
                            failCount++;
                            __log.ErrorFormat("{0}:{1}", DateTime.Now, ex.Message);
                            continue;
                        }
                        finally { if (tran != null) tran.Dispose(); }
                        
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        private void WriteSchoolUserLog(ViewSchoolUser schoolUser, User user, DateTime operateTime, string remark, Guid? operatorId, SchoolUserLogType schoolUserLogType,ref XTransaction tran)
        {
            ISchoolUserLogBLL schoolUserLogBLL = BLLFactory.CreateSchoolUserLogBLL();
            var isSupress = tran != null;
            try
            {
                if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
                Guid? userId = null;
                if (user != null) userId = user.Id;
                string schoolUserId = schoolUser != null ? schoolUser.SchoolUserId : null;
                SchoolUserLog schoolUserLog = new SchoolUserLog()
                {
                    Id = Guid.NewGuid(),
                    OperateTime = operateTime,
                    Remark = remark,
                    SchoolUserId = schoolUserId,
                    UserId = userId,
                    OperatorId = operatorId,
                    Type = (int)schoolUserLogType,
                };
                schoolUserLogBLL.Add(new SchoolUserLog[] { schoolUserLog }, ref tran, isSupress);
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex) { if (!isSupress && tran != null) tran.RollbackTransaction(); }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
        }
        public IList<SchoolUser> ImportSchoolUser(IList<SchoolUser> schoolUserList, bool isDeal=false)
        {
            if (schoolUserList == null || !schoolUserList.Any()) return null;
            var viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            var returns = new List<SchoolUser>();
            foreach (var item in schoolUserList)
            {
                bool isNew = false;
                var dataItem = viewSchoolUserBLL.GetFirstOrDefaultEntityByExpression(string.Format("LoginName=\"{0}\"", item.LoginName));
                if (dataItem == null) { item.Id = Guid.NewGuid().ToString(); isNew = true; }
                else item.Id = dataItem.SchoolUserId;
                XTransaction tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    using (var session = SessionManager.Instance.GetSession().BaseSession)
                    {
                        using (var cmd = session.Connection.CreateCommand())
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            if (isNew)
                            {
                                cmd.CommandText = @"INSERT INTO dbo.SchoolUser
	                                            (
                                                    SchoolUserId,Name,Sex,PhoneNumber,Email,Grade,CertificateType,IdentityCardNo,
	                                                Card,OrganizationName,UserType,TagName,Jobtitle,Speciality,ResearchDirection,
                                                    TutorName,TutorIdentityCardNo,LoginName,UserTypeDetail,IsOuter,Remark,IsHandled
	                                            )
                                                VALUES 
	                                            (
                                                    @schooluserid,@name,@sex,@phonenumber,@email,@grade,@certificatetype,@identitycardno,
	                                                @card,@organizationname,@usertype,@tagname,@jobtitle,@speciality,@researchdirection,
	                                                @tutorname,@tutoridentitycardno,@loginname,@userTypeDetail,@isouter,@remark,0
	                                            )";
                            }
                            #region SchoolUser属性赋值,可以优化用反射
                            var Parameter_Schooluserid = cmd.CreateParameter();
                            Parameter_Schooluserid.ParameterName = "@schooluserid";
                            Parameter_Schooluserid.Value = item.Id;
                            cmd.Parameters.Add(Parameter_Schooluserid);

                            var Parameter_Name = cmd.CreateParameter();
                            Parameter_Name.ParameterName = "@name";
                            Parameter_Name.Value = item.Name;
                            cmd.Parameters.Add(Parameter_Name);

                            var Parameter_Sex = cmd.CreateParameter();
                            Parameter_Sex.ParameterName = "@sex";
                            if(item.Sex.HasValue)
                                Parameter_Sex.Value = item.Sex.Value;
                            else
                                Parameter_Sex.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Sex);

                            var Parameter_PhoneNumber = cmd.CreateParameter();
                            Parameter_PhoneNumber.ParameterName = "@phonenumber";
                            if (item.PhoneNumber != null)
                                Parameter_PhoneNumber.Value = item.PhoneNumber;
                            else
                                Parameter_PhoneNumber.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_PhoneNumber);

                            var Parameter_Email = cmd.CreateParameter();
                            Parameter_Email.ParameterName = "@email";
                            if (item.Email != null)
                                Parameter_Email.Value = item.Email;
                            else
                                Parameter_Email.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Email);

                            var Parameter_Grade = cmd.CreateParameter();
                            Parameter_Grade.ParameterName = "@grade";
                            if (item.Grade != null)
                                Parameter_Grade.Value = item.Grade;
                            else
                                Parameter_Grade.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Grade);

                            var Parameter_CertificateType = cmd.CreateParameter();
                            Parameter_CertificateType.ParameterName = "@certificatetype";
                            if (item.CertificateType!=null)
                                Parameter_CertificateType.Value = item.CertificateType;
                            else
                                Parameter_CertificateType.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_CertificateType);

                            var Parameter_IdentityCardNo = cmd.CreateParameter();
                            Parameter_IdentityCardNo.ParameterName = "@identitycardno";
                            if (item.IdentityCardNo!=null)
                                Parameter_IdentityCardNo.Value = item.IdentityCardNo;
                            else
                                Parameter_IdentityCardNo.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_IdentityCardNo);

                            var Parameter_Card = cmd.CreateParameter();
                            Parameter_Card.ParameterName = "@card";
                            if(item.Card!=null)
                                Parameter_Card.Value = item.Card;
                            else
                                Parameter_Card.Value = DBNull.Value;
                                
                            cmd.Parameters.Add(Parameter_Card);

                            var Parameter_OrganizationName = cmd.CreateParameter();
                            Parameter_OrganizationName.ParameterName = "@organizationname";
                            if (item.OrganizationName != null)
                                Parameter_OrganizationName.Value = item.OrganizationName;
                            else
                                Parameter_OrganizationName.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_OrganizationName);

                            var Parameter_UserType = cmd.CreateParameter();
                            Parameter_UserType.ParameterName = "@usertype";
                            if (item.UserType.HasValue)
                                Parameter_UserType.Value = item.UserType.Value;
                            else
                                Parameter_UserType.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_UserType);

                            var Parameter_TagName = cmd.CreateParameter();
                            Parameter_TagName.ParameterName = "@tagname";
                            if (item.TagName != null)
                                Parameter_TagName.Value = item.TagName;
                            else
                                Parameter_TagName.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_TagName);

                            var Parameter_Jobtitle = cmd.CreateParameter();
                            Parameter_Jobtitle.ParameterName = "@jobtitle";
                            if (item.Jobtitle != null)
                                Parameter_Jobtitle.Value = item.Jobtitle;
                            else
                                Parameter_Jobtitle.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Jobtitle);

                            var Parameter_Speciality = cmd.CreateParameter();
                            Parameter_Speciality.ParameterName = "@speciality";
                            if (item.Speciality != null)
                                Parameter_Speciality.Value = item.Speciality;
                            else
                                Parameter_Speciality.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Speciality);

                            var Parameter_ResearchDirection = cmd.CreateParameter();
                            Parameter_ResearchDirection.ParameterName = "@researchdirection";
                            if (item.ResearchDirection != null)
                                Parameter_ResearchDirection.Value = item.ResearchDirection;
                            else
                                Parameter_ResearchDirection.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_ResearchDirection);

                            var Parameter_tutorname = cmd.CreateParameter();
                            Parameter_tutorname.ParameterName = "@tutorname";
                            if (item.TutorName != null)
                                Parameter_tutorname.Value = item.TutorName;
                            else
                                Parameter_tutorname.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_tutorname);

                            var Parameter_TutorIdentityCardNo = cmd.CreateParameter();
                            Parameter_TutorIdentityCardNo.ParameterName = "@tutoridentitycardno";
                            if (item.TutorIdentityCardNo != null)
                                Parameter_TutorIdentityCardNo.Value = item.TutorIdentityCardNo;
                            else
                                Parameter_TutorIdentityCardNo.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_TutorIdentityCardNo);

                            var Parameter_LoginName = cmd.CreateParameter();
                            Parameter_LoginName.ParameterName = "@loginname";
                            if (item.LoginName != null)
                                Parameter_LoginName.Value = item.LoginName;
                            else
                                Parameter_LoginName.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_LoginName);

                            var Parameter_UserTypeDetail = cmd.CreateParameter();
                            Parameter_UserTypeDetail.ParameterName = "@userTypeDetail";
                            if (item.UserTypeDetail != null)
                                Parameter_UserTypeDetail.Value = item.UserTypeDetail;
                            else
                                Parameter_UserTypeDetail.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_UserTypeDetail);


                            var Parameter_IsOuter = cmd.CreateParameter();
                            Parameter_IsOuter.ParameterName = "@isouter";
                            Parameter_IsOuter.Value = item.IsOuter ? 1 : 0;
                            cmd.Parameters.Add(Parameter_IsOuter);


                            var Parameter_Remark = cmd.CreateParameter();
                            Parameter_Remark.ParameterName = "@remark";
                            if (item.Remark != null)
                                Parameter_Remark.Value = item.Remark;
                            else
                                Parameter_Remark.Value = DBNull.Value;
                            cmd.Parameters.Add(Parameter_Remark);


                            #endregion
                            cmd.ExecuteNonQuery();
                            if (tran.HasTransaction) tran.CommitTransaction();
                            returns.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //schoolUserList.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally 
                { 
                    if (tran != null) tran.Dispose();
                }
            }
            return isDeal ? DealImportSchoolUserList(returns) : returns;
        }
        public IList<SchoolUser> DealImportSchoolUserList(IList<SchoolUser> schoolUserList)
        {
            var viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            string errorMessage = "";
            if (schoolUserList != null && schoolUserList.Any())
            {
                foreach (var item in schoolUserList)
                {
                    var viewSchoolUser = viewSchoolUserBLL.GetFirstOrDefaultEntityByExpression(string.Format("SchoolUserId=\"{0}\"", item.Id));
                    if (viewSchoolUser == null) continue;
                    XTransaction tran = SessionManager.Instance.BeginTransaction(), tranTemp = null;
                    try
                    {
                        User user;
                        var result = userBLL.GenerateUserRelativeInfo(viewSchoolUser, "000", ref tran, out errorMessage, out user);
                        if (result)
                        {
                            if (tran.HasTransaction) tran.CommitTransaction();
                            FinishSync(viewSchoolUser.SchoolUserId);
                            item.IsHandled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                    }
                    finally { if (tran != null) tran.Dispose(); }
                }
            }
            return schoolUserList;
        }
        public string DealImportViewSchoolUserList(IList<ViewSchoolUser> viewSchoolUserList)
        {
            var viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            string errorMessage = "";
            int totalCount = 0;
            int okCount = 0;
            int errorCount = 0;

            if (viewSchoolUserList != null && viewSchoolUserList.Count() > 0)
            {
                totalCount = viewSchoolUserList.Count();
                foreach (var item in viewSchoolUserList)
                {
                    XTransaction tran = SessionManager.Instance.BeginTransaction();
                    User user;
                    try
                    {
                        var result = userBLL.GenerateUserRelativeInfo(item, "000", ref tran, out errorMessage, out user);
                        if (result)
                        {
                            if (tran.HasTransaction) tran.CommitTransaction();
                            FinishSync(item.SchoolUserId);
                            item.IsHandled = true;
                            okCount++;
                        }
                        else
                            errorCount++;

                    }
                    catch (Exception ex)
                    {
                        if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                        errorCount++;
                        continue;
                    }
                    finally { if (tran != null) tran.Dispose(); }
                }
            }
            return string.Format("成功[{0}], 失败[{1}], 总数[{2}]", okCount, errorCount, totalCount);
        }
    }
}
