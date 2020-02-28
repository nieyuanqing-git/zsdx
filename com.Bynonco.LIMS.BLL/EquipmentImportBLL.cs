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
using com.Bynonco.LIMS.Utility;
using log4net;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentImportBLL : BLLBase<EquipmentImport>, IEquipmentImportBLL
    {
        private IUserBLL __userBLL;
        private IEquipmentBLL __equipmentBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private IEquipmentLinkmanBLL __equipmentLinkmanBLL;
        private IEquipmentCategoryBLL __equipmentCategoryBLL;
        private IEquipmentCategoryBindBLL __equipmentCategoryBindBLL;
        private ICalcFeeTimeRuleBLL __calcFeeTimeRuleBLL;
        private IChargeStandardBLL __chargeStandardBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IDictCodeBLL __dictCodeBLL;
        private IEquipmentProfessorBLL __equipmentProfessorBLL;
        private IEquipmentRoomDirectorBLL __equipmentRoomDirectorBLL;
        
        public EquipmentImportBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            __equipmentLinkmanBLL = BLLFactory.CreateEquipmentLinkmanBLL();
            __equipmentCategoryBLL = BLLFactory.CreateEquipmentCategoryBLL();
            __equipmentCategoryBindBLL = BLLFactory.CreateEquipmentCategoryBindBLL();
            __calcFeeTimeRuleBLL = BLLFactory.CreateCalcFeeTimeRuleBLL();
            __chargeStandardBLL = BLLFactory.CreateChargeStandardBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __equipmentProfessorBLL = BLLFactory.CreateEquipmentProfessorBLL();
            __equipmentRoomDirectorBLL = BLLFactory.CreateEquipmentRoomDirectorBLL();
        }

        private static readonly ILog __log = LogManager.GetLogger(typeof(SchoolUserBLL));
        public bool SyncEquipmentImport(string queryExpression, int? syncCountPerTime, Guid? operatorId, out int totalCount, out int successCount, out int failCount, out string errorMessage)
        {
            totalCount = 0;
            successCount = 0;
            failCount = 0;
            errorMessage = "";
            try
            {
               var isCoverEquipmentInfo= __dictCodeBLL.GetDictCodeBoolValueByCode("SyncEquipment","IsCoverEquipmentInfo");
               var equipmentImportList = syncCountPerTime.HasValue ? GetScopeEntitiesByExpression(queryExpression, 1, syncCountPerTime.Value, null, "Label") : GetEntitiesByExpression(queryExpression,null,"Label");
                if (equipmentImportList != null && equipmentImportList.Count > 0)
                {
                    totalCount = equipmentImportList.Count;
                    foreach (var item in equipmentImportList)
                    {
                        if (SyncSaveEquipmentInfo(item, isCoverEquipmentInfo.HasValue ? isCoverEquipmentInfo.Value : false, out errorMessage))
                        {
                            XTransaction tran = null;
                            item.IsHandled = true;
                            Save(new EquipmentImport[] { item }, ref tran);
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                            __log.ErrorFormat("{0}[{1}]:{2}", DateTime.Now, item.Label, errorMessage);
                        }
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
        private bool SyncSaveEquipmentInfo(EquipmentImport item,bool isCoverEquipmentInfo, out string errorMessage)
        {
            errorMessage = "";
            if(item == null) return false;
            if(string.IsNullOrWhiteSpace(item.Name) 
                || string.IsNullOrWhiteSpace(item.Label)
                || string.IsNullOrWhiteSpace(item.Model)
                || string.IsNullOrWhiteSpace(item.OrgName)
                || string.IsNullOrWhiteSpace(item.RoomName)
            )
            {
                errorMessage = "仪器必填信息不齐全【名称、资产编号、型号、所属单位、放置地点】";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                bool isNew = false;
                var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsDelete=false",item.Label));
                if(equipment == null)
                {
                    isNew = true;
                    equipment = new Equipment();
                    equipment.Id = Guid.NewGuid();
                }
                else if(!isCoverEquipmentInfo) return true;
                var equipmentOrg = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("IsDelete=false*Name=\"{0}\"*Type=\"{1}\"",item.OrgName, (int)LabOrganizationType.Organziton),null,"XPath");
                if(equipmentOrg == null)
                {
                    errorMessage = string.Format("组织机构【{0}】不存在",item.OrgName);
                    return false;
                }
                var equipmentRoom = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false*Name=\"{1}\"*Type=\"{2}\"", equipmentOrg.Id, item.RoomName,(int)LabOrganizationType.LabRoom),null,"XPath");
                if(equipmentRoom == null)
                {
                    equipmentRoom = new LabOrganization()
                    {
                        Id = Guid.NewGuid(),
                        LabOrganizationType = LabOrganizationType.LabRoom,
                        ParentId = equipmentOrg.Id,
                        Name = item.RoomName,
                        IsDelete = false
                    };
                    equipmentRoom.XPath = XPathUtility.GenerateXPath(null, equipmentOrg.Id,
                                (entityId) => { return __labOrganizationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                                (parentEntityId) => { return __labOrganizationBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                                () => { return __labOrganizationBLL.GetEntitiesByExpression("ParentId=null*IsDelete=false"); });
                    __labOrganizationBLL.Add(new LabOrganization[] { equipmentRoom }, ref tran, true);

                }
                equipment.Name = item.Name;
                equipment.Label = item.Label;
                equipment.Model = item.Model;
                equipment.OrgId = equipmentOrg.Id;
                equipment.RoomId = equipmentRoom.Id;
                equipment.InputStr = ShortcutStringUtility.GetFirstPYLetter(equipment.Name);
                if(!string.IsNullOrWhiteSpace(item.Specifications))
                {
                    equipment.Specifications = item.Specifications;
                    equipment.SpecimenNoticeNoHTML = item.Specifications;
                }
                if(!string.IsNullOrWhiteSpace(item.ProductionArea)) equipment.ProductionArea = item.ProductionArea;
                if(!string.IsNullOrWhiteSpace(item.Factory)) equipment.Factory = item.Factory;
                if(!string.IsNullOrWhiteSpace(item.Brand)) equipment.Brand = item.Brand;
                if(!string.IsNullOrWhiteSpace(item.ProductionNo)) equipment.ProductionNo = item.ProductionNo;
                if(item.ComeFrom.HasValue) equipment.ComeFrom = item.ComeFrom;
                if(!string.IsNullOrWhiteSpace(item.Supplier)) equipment.Supplier = item.Supplier;
                if(item.UnitPrice.HasValue) equipment.UnitPrice = item.UnitPrice;
                if(item.BuyDate.HasValue) equipment.BuyDate = item.BuyDate;
                if(item.UseDirection.HasValue) equipment.UseDirection = item.UseDirection;
                if(item.IsLargeScaleEquipment.HasValue) equipment.IsLargeScaleEquipment = item.IsLargeScaleEquipment;
                if(!string.IsNullOrWhiteSpace(item.AdminUserNane)) equipment.AdminUserName = item.AdminUserNane;
                if(!string.IsNullOrWhiteSpace(item.LinkUserName)) equipment.LinkUserName = item.LinkUserName;
                if(!string.IsNullOrWhiteSpace(item.LinkTelNo)) equipment.LinkTelNo = item.LinkTelNo;
                if(!string.IsNullOrWhiteSpace(item.AdminEmail)) equipment.AdminEmail = item.AdminEmail;
                if(!string.IsNullOrWhiteSpace(item.Qualification))
                {
                    equipment.Qualification = item.Qualification;
                    equipment.QualificationNoHTML = item.Qualification;
                }
                if (!string.IsNullOrWhiteSpace(item.ScopeOfApplication))
                {
                    equipment.ScopeOfApplication = item.ScopeOfApplication;
                    equipment.ScopeOfApplicationNoHTML = item.ScopeOfApplication;
                }
                if (!string.IsNullOrWhiteSpace(item.SpecimenNotice))
                {
                    equipment.SpecimenNotice = item.SpecimenNotice;
                    equipment.SpecimenNoticeNoHTML = item.SpecimenNotice;
                }
                if (!string.IsNullOrWhiteSpace(item.UseNotice))
                {
                    equipment.UseNotice = item.UseNotice;
                    equipment.UseNoticeNoHTML = item.UseNotice;
                }
                if (!string.IsNullOrWhiteSpace(item.Remark))
                {
                    equipment.Remark = item.Remark;
                    equipment.RemarkNoHTML = item.Remark;
                }
                if(item.Status.HasValue) equipment.Status = item.Status;
                if(item.UseType.HasValue) equipment.UseType = item.UseType;
                if(item.AppointmentDays.HasValue)
                {
                    equipment.UseDefaultAppointmentDays = false;
                    equipment.AppointmentDays = item.AppointmentDays;
                }
                if(!string.IsNullOrWhiteSpace(item.WorkDays))
                {
                    equipment.UseDefualWorkDays = false;
                    equipment.WorkDays = item.WorkDays.Replace("，",",").Replace(",","|").Replace(" ","").Replace("||","|")
                        .Replace("0","Sunday")
                        .Replace("1","Monday")
                        .Replace("2","Tuesday")
                        .Replace("3","Wednesday")
                        .Replace("4","Thursday")
                        .Replace("5","Friday")
                        .Replace("6","Saturday");
                }
                if(item.AppointmentTimeStep.HasValue)
                {
                    equipment.UseDefaultAppointmentTimeStep = false;
                    equipment.AppointmentTimeStep = item.AppointmentTimeStep;
                }
                if(!string.IsNullOrWhiteSpace(item.WorkHours))
                {
                    equipment.UseDefualtWorkHours = false;
                    equipment.WorkHours = item.WorkHours;
                }
                if(item.IsNeedTranningBeforeUse.HasValue)
                {
                    equipment.UseDefaultIsNeedTranningBeforeUse = false;
                    equipment.IsNeedTranningBeforeUse = item.IsNeedTranningBeforeUse;
                }
                if(item.IsAppointmentNeedAudit.HasValue)
                {
                    equipment.UseDefaultIsAppointmentNeedAudit = false;
                    equipment.IsAppointmentNeedAudit = item.IsAppointmentNeedAudit;
                }
                else
                {
                    equipment.IsAppointmentNeedAudit = false;
                }
                if(item.IsAcceptSampleTest.HasValue)
                {
                    equipment.UseDefaultIsAcceptSampleTest = false;
                    equipment.IsAcceptSampleTest = item.IsAcceptSampleTest;
                }
                equipment.IsDelete = false;
                if (!equipment.IsLargeScaleEquipment.HasValue) equipment.IsLargeScaleEquipment = false;
                if (!equipment.Status.HasValue || equipment.Status.Value == 0) equipment.Status = (int)EquipmentStatus.Normal;
                if (!equipment.UseType.HasValue) equipment.UseType = (int)EquipmentUseType.None;

                if (!string.IsNullOrWhiteSpace(item.ProfessorPhoneNumber)) equipment.ProfessorPhoneNumber = item.ProfessorPhoneNumber;
                if (!string.IsNullOrWhiteSpace(item.ProfessorEmail)) equipment.ProfessorEmail = item.ProfessorEmail;
                if (!string.IsNullOrWhiteSpace(item.FeeComeFrom)) equipment.FeeComeFrom = int.Parse(item.FeeComeFrom);
                if (!string.IsNullOrWhiteSpace(item.ResearchArea)) equipment.ResearchArea = item.ResearchArea;

                if (isNew) __equipmentBLL.Add(new Equipment[] { equipment }, ref tran, true);
                else __equipmentBLL.Save(new Equipment[] { equipment }, ref tran, true);
                if (isNew && equipment.CategoryBinds != null && equipment.CategoryBinds.Count() > 0)
                {
                    __equipmentCategoryBindBLL.Delete(equipment.CategoryBinds.Select(p => p.Id), ref tran, true);
                }
                if(!string.IsNullOrWhiteSpace(item.CategoryName))
                {
                    var equipmentCategory = __equipmentCategoryBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsDelete=false",item.CategoryName),null, "XPath");
                    if(equipmentCategory != null)
                    {
                        var equipmentCategoryBind = __equipmentCategoryBindBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*EquipmentCategoryId=\"{1}\"",equipment.Id, equipmentCategory.Id));
                        if(equipmentCategoryBind == null)
                        {
                            equipmentCategoryBind = new EquipmentCategoryBind();
                            equipmentCategoryBind.Id = Guid.NewGuid();
                            equipmentCategoryBind.EquipmentId = equipment.Id;
                            equipmentCategoryBind.EquipmentCategoryId = equipmentCategory.Id;
                            __equipmentCategoryBindBLL.Add(new EquipmentCategoryBind[] { equipmentCategoryBind }, ref tran, true);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.AdminUserNane))
                {
                    var adminUser = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("(Label=\"{0}\"+UserName=\"{0}\")*IsDel=false", item.AdminUserNane));
                    if (adminUser != null)
                    {
                        var userWorkScope = __userWorkScopeBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"", adminUser.Id,equipment.Id));
                        if (userWorkScope == null)
                        {
                            userWorkScope = new UserWorkScope()
                            {
                                Id = Guid.NewGuid(),
                                UserId = adminUser.Id,
                                EquipmentId = equipment.Id
                            };
                            __userWorkScopeBLL.Add(new UserWorkScope[] { userWorkScope }, ref tran, true);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(item.LinkUserName))
                {
                    var linkUser = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("(Label=\"{0}\"+UserName=\"{0}\")*IsDel=false", item.LinkUserName));
                    if (linkUser != null)
                    {
                        var equipmentLinkman = __equipmentLinkmanBLL.GetFirstOrDefaultEntityByExpression(string.Format("LinkmanId=\"{0}\"*EquipmentId=\"{1}\"", linkUser.Id, equipment.Id));
                        if (equipmentLinkman == null)
                        {
                            equipmentLinkman = new EquipmentLinkman()
                            {
                                Id = Guid.NewGuid(),
                                LinkmanId = linkUser.Id,
                                EquipmentId = equipment.Id
                            };
                            __equipmentLinkmanBLL.Add(new EquipmentLinkman[] { equipmentLinkman }, ref tran, true);
                        }
                    }
                }
                if (equipment.CalcFeeTimeRule == null)
                {
                    equipment.CalcFeeTimeRule = new CalcFeeTimeRule();
                    equipment.CalcFeeTimeRule.Id = Guid.NewGuid();
                    equipment.CalcFeeTimeRule.EquipmentId = equipment.Id;
                    equipment.CalcFeeTimeRule.Expression = "return t2-t1;";
                    equipment.CalcFeeTimeRule.ReadableExpression = "使用结束时间-开始使用时间";
                    equipment.CalcFeeTimeRule.RoundDigits = 2;
                    equipment.CalcFeeTimeRule.RoundFator = 0;
                    __calcFeeTimeRuleBLL.Add(new CalcFeeTimeRule[] { equipment.CalcFeeTimeRule }, ref tran, true);
                }
                if (equipment.ChargeStandard == null)
                {
                    equipment.ChargeStandard = new ChargeStandard()
                    {
                        Id = Guid.NewGuid(),
                        EquipmentId = equipment.Id,
                        ChargeType = item.ChargeType.HasValue ? item.ChargeType.Value : (int)com.Bynonco.LIMS.Model.Enum.ChargeType.ByHour,
                        StandardPrice = item.StandardPrice.HasValue ? item.StandardPrice.Value : 0d
                    };
                    __chargeStandardBLL.Add(new ChargeStandard[] { equipment.ChargeStandard }, ref tran, true);
                }
                else
                {
                    if (item.ChargeType.HasValue) equipment.ChargeStandard.ChargeType = item.ChargeType.Value;
                    if (item.StandardPrice.HasValue) equipment.ChargeStandard.StandardPrice = item.StandardPrice.Value;
                    __chargeStandardBLL.Save(new ChargeStandard[] { equipment.ChargeStandard }, ref tran, true);
                }
                #region 实验室主任
                IList<EquipmentRoomDirector> equipmentRoomDirectorList = new List<EquipmentRoomDirector>();
                var roomDirectorName = item.RoomDirectorName;
                if (!string.IsNullOrEmpty(roomDirectorName))
                {
                    var equipmentroomDirectorNameList = roomDirectorName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (equipmentroomDirectorNameList.Count() > 0)
                    {
                        for (int i = 0; i < equipmentroomDirectorNameList.Count(); i++)
                        {
                            string itemId = "";
                            string itemName = equipmentroomDirectorNameList[i].Trim();
                            if (itemName != "")
                            {
                                var userinfo = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"*IsDel=false", equipmentroomDirectorNameList[i].Trim()));
                                if (userinfo == null) itemId = "";
                                else itemId = userinfo.Id.ToString();
                            }
                            else itemId = "";
                            EquipmentRoomDirector equipmentRoomDirector = new EquipmentRoomDirector();
                            equipmentRoomDirector.Id = Guid.NewGuid();
                            equipmentRoomDirector.EquipmentId = equipment.id;
                            if (itemId != "") equipmentRoomDirector.UserId = new Guid(itemId);
                            equipmentRoomDirector.UserName = itemName;
                            equipmentRoomDirector.UserOrder = i + 1;
                            equipmentRoomDirectorList.Add(equipmentRoomDirector);
                        }
                    }
                }
                if (equipment.EquipmentRoomDirectorList != null)
                    __equipmentRoomDirectorBLL.Delete(equipment.EquipmentRoomDirectorList.Select(p => p.Id), ref tran, true);
                equipment.EquipmentRoomDirectorList = equipmentRoomDirectorList;
                if (equipment.EquipmentRoomDirectorList != null)
                    __equipmentRoomDirectorBLL.Add(equipment.EquipmentRoomDirectorList, ref tran, true);
                #endregion

                #region 负责教授
                IList<EquipmentProfessor> equipmentProfessorList = new List<EquipmentProfessor>();
                var professorName = item.ProfessorName;
                if (!string.IsNullOrEmpty(professorName))
                {
                    var equipmentProfessorNameList = professorName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (equipmentProfessorNameList.Count() > 0)
                    {
                        for (int i = 0; i < equipmentProfessorNameList.Count(); i++)
                        {
                            string itemId = "";
                            string itemName = equipmentProfessorNameList[i].Trim();
                            if (itemId != "")
                            {
                                var userinfo = __userBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserName=\"{0}\"*IsDel=false", equipmentProfessorNameList[i].Trim()));
                                if (userinfo == null) itemId = "";
                                else itemId = userinfo.Id.ToString();
                            }
                            else itemId = "";
                            EquipmentProfessor equipmentProfessor = new EquipmentProfessor();
                            equipmentProfessor.Id = Guid.NewGuid();
                            equipmentProfessor.EquipmentId = equipment.id;
                            if (itemId != "") equipmentProfessor.UserId = new Guid(itemId);
                            equipmentProfessor.UserName = itemName;
                            equipmentProfessor.UserOrder = i + 1;
                            equipmentProfessorList.Add(equipmentProfessor);
                        }
                    }
                }
                if (equipment.EquipmentProfessorList != null)
                    __equipmentProfessorBLL.Delete(equipment.EquipmentProfessorList.Select(p => p.Id), ref tran, true);
                equipment.EquipmentProfessorList = equipmentProfessorList;
                if (equipment.EquipmentProfessorList != null)
                    __equipmentProfessorBLL.Add(equipment.EquipmentProfessorList, ref tran, true);
                #endregion                
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
    }
}
