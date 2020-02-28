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
    public class ThesisBLL : BLLBase<Thesis>, IThesisBLL
    {
        private IThesisDepartmentBLL __thesisDepartmentBLL;
        private IThesisAuthorBLL __thesisAuthorBLL;
        private IThesisEquipmentBLL __thesisEquipmentBLL;
        private IAttachmentBLL __attachmentBLL;
        public ThesisBLL()
        {
            __thesisDepartmentBLL = BLLFactory.CreateThesisDepartmentBLL();
            __thesisAuthorBLL = BLLFactory.CreateThesisAuthorBLL();
            __thesisEquipmentBLL = BLLFactory.CreateThesisEquipmentBLL();
            __attachmentBLL = BLLFactory.CreateAttachmentBLL();
            
        }
        public bool DeleteThesis(Thesis thesis)
        {
            if (thesis == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var thesisDepartmentList = __thesisDepartmentBLL.GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"", thesis.Id));
                if (thesisDepartmentList != null && thesisDepartmentList.Count() > 0)
                {
                    __thesisDepartmentBLL.Delete(thesisDepartmentList.Select(m => m.id), ref tran, true);
                }
                var thesisAuthorList = __thesisAuthorBLL.GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"", thesis.Id));
                if (thesisAuthorList != null && thesisAuthorList.Count() > 0)
                {
                    __thesisAuthorBLL.Delete(thesisAuthorList.Select(m => m.id), ref tran, true);
                }
                var thesisEquipmentList = __thesisEquipmentBLL.GetEntitiesByExpression(string.Format("ThesisId=\"{0}\"", thesis.Id));
                if (thesisEquipmentList != null && thesisEquipmentList.Count() > 0)
                {
                    __thesisEquipmentBLL.Delete(thesisEquipmentList.Select(m => m.id), ref tran, true);
                }
                var attachmentList = __attachmentBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"", thesis.Id));
                if (attachmentList != null && attachmentList.Count() > 0)
                {
                    __attachmentBLL.Delete(attachmentList.Select(m => m.id), ref tran, true);
                }
                Delete(new Guid[] { thesis.Id }, ref tran, true);
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

        public IList<ThesisImport> ImportThesis(IList<ThesisImport> thesisImportList, bool isDeal = true)
        {
            if (thesisImportList == null || thesisImportList.Count == 0)
                return null;
            var __thesisAuthorBLL = BLLFactory.CreateThesisAuthorBLL();
            var __thesisDepartmentBLL = BLLFactory.CreateThesisDepartmentBLL();
            var __thesisEquipmentBLL = BLLFactory.CreateThesisEquipmentBLL();
            var _labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            var dictCode = BLLFactory.CreateDictCodeBLL();
            var equipmentBLL = BLLFactory.CreateEquipmentBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            foreach (var item in thesisImportList)
            {

                try
                {
                    item.IsHandled = false;
                    Thesis thesis = new Thesis();                    
                    thesis.Id = Guid.NewGuid();
                    if (item.EquipmentLabel != "")
                    {
                        IList<ThesisEquipment> thesisEquipmentList = new List<ThesisEquipment>();
                        var equipment = equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", item.EquipmentLabel));
                        if (equipment == null) continue;
                        else
                        {
                            ThesisEquipment thesisEquipment = new ThesisEquipment();
                            thesisEquipment.Id = Guid.NewGuid();
                            thesisEquipment.EquipmentId = equipment.id;
                            thesisEquipment.ThesisId = thesis.Id;
                            thesisEquipment.EquipmentOrder = 1;
                            thesisEquipmentList.Add(thesisEquipment);
                            thesis.ThesisEquipmentList = thesisEquipmentList;
                        }                        
                    }
                    else continue;
                    thesis.Title = item.Title;
                    #region 第一作者
                    IList<ThesisAuthor> firstThesisAuthorList = new List<ThesisAuthor>();
                    if (item.FirstAuthorName == null) continue;
                    else
                    {
                        var firstAuthorNameList = item.FirstAuthorName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < firstAuthorNameList.Count(); i++)
                        {
                            string itemName = firstAuthorNameList[i].Trim();
                            var userinfo = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", itemName));
                            string itemId = "";
                            if (userinfo == null) itemId = "";
                            else itemId = userinfo.id.ToString();
                            ThesisAuthor thesisAuthor = new ThesisAuthor();
                            thesisAuthor.Id = Guid.NewGuid();
                            thesisAuthor.ThesisId = thesis.id;
                            if (itemId != "") thesisAuthor.AuthorId = new Guid(itemId);
                            thesisAuthor.AuthorName = itemName;
                            thesisAuthor.AuthorOrder = i + 1;
                            thesisAuthor.AuthorType = "FirstThesisAuthor";
                            firstThesisAuthorList.Add(thesisAuthor);
                        }
                    }
                    thesis.FirstThesisAuthorList = firstThesisAuthorList;
                    #endregion

                    #region 通讯作者
                    IList<ThesisAuthor> otherThesisAuthorList = new List<ThesisAuthor>();
                    if (item.OtherAuthor == null) continue;
                    else
                    {
                        var otherAuthorNameList = item.OtherAuthor.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < otherAuthorNameList.Count(); i++)
                        {
                            string itemName = otherAuthorNameList[i].Trim();
                            var userinfo = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", itemName));
                            string itemId = "";
                            if (userinfo == null) itemId = "";
                            else itemId = userinfo.id.ToString();
                            ThesisAuthor thesisAuthor = new ThesisAuthor();
                            thesisAuthor.Id = Guid.NewGuid();
                            thesisAuthor.ThesisId = thesis.id;
                            if (itemId != "") thesisAuthor.AuthorId = new Guid(itemId);
                            thesisAuthor.AuthorName = itemName;
                            thesisAuthor.AuthorOrder = i + 1;
                            thesisAuthor.AuthorType = "OtherThesisAuthor";
                            otherThesisAuthorList.Add(thesisAuthor);
                        }
                    }
                    thesis.OtherThesisAuthorList = otherThesisAuthorList;
                    #endregion

                    thesis.JournalType = new Guid(item.JournalType);
                    thesis.JournalName = item.JournalName;
                    DateTime dtDate;
                    if (DateTime.TryParse(item.JournalTime, out dtDate))
                        thesis.JournalTime = dtDate;
                    else
                        continue;

                    var tutor = userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"", item.Tutor));
                    if (tutor != null)
                        thesis.TutorId = tutor.Id;
                    else
                        thesis.TutorId = null;
                    thesis.VolumeNumber = item.VolumeNumber;
                    thesis.Pagination = item.Pagination;
                    thesis.ImpactFactor = item.ImpactFactor;
                    thesis.ISSN = item.ISSN;

                    #region 发表论文单位
                    IList<ThesisDepartment> thesisDepartmentList = new List<ThesisDepartment>();
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
                                ThesisDepartment thesisDepartment = new ThesisDepartment();
                                thesisDepartment.Id = Guid.NewGuid();
                                thesisDepartment.ThesisId = thesis.id;
                                if (itemId != "") thesisDepartment.DepartmentId = new Guid(itemId);
                                thesisDepartment.DepartmentName = itemName;
                                thesisDepartment.DepartmentOrder = i + 1;
                                thesisDepartmentList.Add(thesisDepartment);
                            }
                        }
                    }
                    thesis.ThesisDepartmentList = thesisDepartmentList;
                    #endregion


                    Add(new Thesis[] { thesis }, ref tran, true);
                    if (thesis.FirstThesisAuthorList != null && thesis.FirstThesisAuthorList.Count > 0)
                        __thesisAuthorBLL.Add(thesis.FirstThesisAuthorList, ref tran, true);
                    if (thesis.OtherThesisAuthorList != null && thesis.OtherThesisAuthorList.Count > 0)
                        __thesisAuthorBLL.Add(thesis.OtherThesisAuthorList, ref tran, true);
                    if (thesis.ThesisDepartmentList != null && thesis.ThesisDepartmentList.Count > 0)
                        __thesisDepartmentBLL.Add(thesis.ThesisDepartmentList, ref tran, true);
                    if (thesis.ThesisEquipmentList != null && thesis.ThesisEquipmentList.Count > 0)
                        __thesisEquipmentBLL.Add(thesis.ThesisEquipmentList, ref tran, true);
                    tran.CommitTransaction();
                    item.IsHandled = true;
                }
                catch (Exception ex)
                {
                    thesisImportList.Remove(item);
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            return thesisImportList;
        }
    }
}