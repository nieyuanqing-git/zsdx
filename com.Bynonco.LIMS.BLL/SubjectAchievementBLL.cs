using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;
namespace com.Bynonco.LIMS.BLL
{
    public class SubjectAchievementBLL : BLLBase<SubjectAchievement>, ISubjectAchievementBLL
    {
        private ISubjectDepartmentBLL __subjectDepartmentBLL;
        private ISubjectMemberBLL __subjectMemberBLL;
        private ISubjectEquipmentBLL __subjectEquipmentBLL;
        private IAttachmentBLL __attachmentBLL;
        public SubjectAchievementBLL()
        {
            __subjectDepartmentBLL = BLLFactory.CreateSubjectDepartmentBLL();
            __subjectMemberBLL = BLLFactory.CreateSubjectMemberBLL();
            __subjectEquipmentBLL = BLLFactory.CreateSubjectEquipmentBLL();
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
        }
        public bool DeleteSubjectAchievement(SubjectAchievement subjectAchievement)
        {
            if (subjectAchievement == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var subjectDepartmentList = __subjectDepartmentBLL.GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", subjectAchievement.Id));
                if (subjectDepartmentList != null && subjectDepartmentList.Count() > 0)
                {
                    __subjectDepartmentBLL.Delete(subjectDepartmentList.Select(m => m.id), ref tran, true);
                }
                var subjectMemberList = __subjectMemberBLL.GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", subjectAchievement.Id));
                if (subjectMemberList != null && subjectMemberList.Count() > 0)
                {
                    __subjectMemberBLL.Delete(subjectMemberList.Select(m => m.id), ref tran, true);
                }
                var subjectEquipmentList = __subjectEquipmentBLL.GetEntitiesByExpression(string.Format("SubjectAchievementId=\"{0}\"", subjectAchievement.Id));
                if (subjectEquipmentList != null && subjectEquipmentList.Count() > 0)
                {
                    __subjectEquipmentBLL.Delete(subjectEquipmentList.Select(m => m.id), ref tran, true);
                }
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", subjectAchievement.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { subjectAchievement.Id }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }

        public IList<SubjectAchievementImport> ImportSubjectAchievement(IList<SubjectAchievementImport> subjectAchievementImportList, bool isDeal = true)
        {
            if (subjectAchievementImportList == null || subjectAchievementImportList.Count == 0)
                return null;
            var __thesisAuthorBLL = BLLFactory.CreateThesisAuthorBLL();
            var __thesisDepartmentBLL = BLLFactory.CreateThesisDepartmentBLL();
            var __thesisEquipmentBLL = BLLFactory.CreateThesisEquipmentBLL();
            var _labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            var __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            var __dictCode = BLLFactory.CreateDictCodeBLL();
            var __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            var __userBLL = BLLFactory.CreateUserBLL();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            foreach (var item in subjectAchievementImportList)
            {

                try
                {
                    item.IsHandled = false;
                    SubjectAchievement subjectAchievement = new SubjectAchievement();
                    subjectAchievement.Id = Guid.NewGuid();
                    if (item.EquipmentLabel != "")
                    {
                        IList<SubjectEquipment> subjectEquipmentList = new List<SubjectEquipment>();
                        var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", item.EquipmentLabel));
                        if (equipment == null) continue;
                        else
                        {
                            SubjectEquipment subjectEquipment = new SubjectEquipment();
                            subjectEquipment.Id = Guid.NewGuid();
                            subjectEquipment.EquipmentId = equipment.id;
                            subjectEquipment.SubjectAchievementId = subjectAchievement.Id;
                            subjectEquipment.EquipmentOrder = 1;
                            subjectEquipmentList.Add(subjectEquipment);
                            subjectAchievement.SubjectEquipmentList = subjectEquipmentList;
                        }
                    }
                    else continue;
                    subjectAchievement.SubjectName = item.SubjectName;
                    subjectAchievement.SubjectChief = item.SubjectChief;
                    subjectAchievement.SubjectChiefId = null;
                    if (!string.IsNullOrWhiteSpace(item.SubjectChief))
                    {
                        var userItems = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", item.SubjectChief));
                        if (userItems != null) subjectAchievement.SubjectChiefId = userItems.Id;
                    }
                    subjectAchievement.SubjectType = new Guid(item.SubjectType);
                    if (item.SubjectCome != "")
                    {
                        var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=\"{0}\"", "SubjectCome"));
                        DictCode dictCode = null;
                        if(dictCodeType!=null)
                            dictCode = __dictCode.GetFirstOrDefaultEntityByExpression(string.Format("Value=\"{0}\"*DictCodeTypeId=\"{1}\"", item.SubjectCome,dictCodeType.Id.ToString()));
                        if (dictCode != null)
                            subjectAchievement.SubjectCome = dictCode.Id;
                    }
                    subjectAchievement.SubjectNum = item.SubjectNum;
                    decimal tempSubjectFunds;
                    if (decimal.TryParse(item.SubjectFunds, out tempSubjectFunds))
                    {
                        subjectAchievement.SubjectFunds = tempSubjectFunds;
                    }
                    DateTime dtStartTime;
                    if (DateTime.TryParse(item.StartTime, out dtStartTime))
                        subjectAchievement.StartTime = dtStartTime;
                    DateTime dtEndTime;
                    if (DateTime.TryParse(item.EndTime, out dtEndTime))
                        subjectAchievement.EndTime = dtEndTime;
  

                    #region 项目组成员
                    IList<SubjectMember> subjectMemberList = new List<SubjectMember>();
                    if (item.MemberName == null) continue;
                    else
                    {
                        var memberNameList = item.MemberName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < memberNameList.Count(); i++)
                        {
                            string itemName = memberNameList[i].Trim();
                            var userinfo = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", itemName));
                            string itemId = "";
                            if (userinfo == null) itemId = "";
                            else itemId = userinfo.id.ToString();
                            SubjectMember subjectMember = new SubjectMember();
                            subjectMember.Id = Guid.NewGuid();
                            subjectMember.SubjectAchievementId = subjectAchievement.Id;
                            if (itemId != "") subjectMember.MemberId = new Guid(itemId);
                            subjectMember.MemberName = itemName;
                            subjectMember.MemberOrder = i + 1;
                            subjectMemberList.Add(subjectMember);
                        }
                    }
                    subjectAchievement.SubjectMemberList = subjectMemberList;
                    #endregion

                    #region 单位
                    IList<SubjectDepartment> subjectDepartmentList = new List<SubjectDepartment>();
                    if (item.DepartmentName == null) continue;
                    else
                    {
                        var departmentNameList = item.DepartmentName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (departmentNameList.Count() > 0)
                        {
                            for (int i = 0; i < departmentNameList.Count(); i++)
                            {
                                string itemName = departmentNameList[i].Trim();
                                string itemId = "";
                                var organizationinfo = _labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"", itemName));
                                if (organizationinfo == null) itemId = "";
                                else itemId = "";
                                SubjectDepartment subjectDepartment = new SubjectDepartment();
                                subjectDepartment.Id = Guid.NewGuid();
                                subjectDepartment.SubjectAchievementId = subjectAchievement.id;
                                if (itemId != "") subjectDepartment.DepartmentId = new Guid(itemId);
                                subjectDepartment.DepartmentName = itemName;
                                subjectDepartment.DepartmentOrder = i + 1;
                                subjectDepartmentList.Add(subjectDepartment);
                            }
                        }
                    }
                    subjectAchievement.SubjectDepartmentList = subjectDepartmentList;
                    #endregion               

                    Add(new SubjectAchievement[] { subjectAchievement }, ref tran, true);
                    if (subjectAchievement.SubjectMemberList != null && subjectAchievement.SubjectMemberList.Count > 0)
                        __subjectMemberBLL.Add(subjectAchievement.SubjectMemberList, ref tran, true);
                    if (subjectAchievement.SubjectEquipmentList != null && subjectAchievement.SubjectEquipmentList.Count > 0)
                        __subjectEquipmentBLL.Add(subjectAchievement.SubjectEquipmentList, ref tran, true);
                    if (subjectAchievement.SubjectDepartmentList != null && subjectAchievement.SubjectDepartmentList.Count > 0)
                        __subjectDepartmentBLL.Add(subjectAchievement.SubjectDepartmentList, ref tran, true);                    
                    tran.CommitTransaction();
                    item.IsHandled = true;
                }
                catch (Exception ex)
                {
                    subjectAchievementImportList.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            return subjectAchievementImportList;
        }
    }
}