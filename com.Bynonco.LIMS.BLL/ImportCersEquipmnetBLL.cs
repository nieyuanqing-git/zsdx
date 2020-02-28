using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using System.Reflection;
using com.Bynonco.LIMS.Model.Business.CERS.Platform;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System.Web;

namespace com.Bynonco.LIMS.BLL
{
    public class ImportCersEquipmnetBLL : BLLBase<ImportCersEquipmnet>, IImportCersEquipmnetBLL
    {
        private IEquipmentBLL __equipmentBLL;
        private IEquipmentExtendBLL __equipmentExtendBLL;
        private ICountryBLL __countryBLL;
        private ILabOrganizationBLL __labOrganizationBLL;
        private static IUserBLL __userBLL;
        private static IEquipmentLinkmanBLL __equipmentLinkmanBLL;
        private static IEquipmentExtendBLL __EquipmentExtendBLL;
        private static IEquipmentCustomsBLL __equipmentCustomsBLL;
        private static IEquipmentCustomsBindBLL __equipmentCustomsBindBLL;

        public ImportCersEquipmnetBLL()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __equipmentExtendBLL = BLLFactory.CreateEquipmentExtendBLL();
            __countryBLL = BLLFactory.CreateCountryBLL();
            __labOrganizationBLL = BLLFactory.CreateLabOrganizationBLL();
            __userBLL = BLLFactory.CreateUserBLL();
            __equipmentLinkmanBLL = BLLFactory.CreateEquipmentLinkmanBLL();
            __EquipmentExtendBLL = BLLFactory.CreateEquipmentExtendBLL();
            __equipmentCustomsBLL = BLLFactory.CreateEquipmentCustomsBLL();
            __equipmentCustomsBindBLL = BLLFactory.CreateEquipmentCustomsBindBLL();

        }

        public IList<ImportCersEquipmnet> ImportCersEquipmnet(IList<ImportCersEquipmnet> importCersEquipmnetList, bool isDeal = false)
        {
            if (importCersEquipmnetList == null || importCersEquipmnetList.Count == 0)
                return null;
            
            foreach (var item in importCersEquipmnetList)//.Where(p => p.IsValidate == true))
            {
                    ImportCersEquipmnet importCersEquipmnet = new ImportCersEquipmnet();
                XTransaction tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    var isNew = false;
                    item.Ishandled = false;
                    if (item.Label != "")
                    {
                        var tempImportCersEquipmnet = GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", item.Label));
                        if (tempImportCersEquipmnet != null)
                        {
                            importCersEquipmnet = tempImportCersEquipmnet;
                            item.Id = tempImportCersEquipmnet.Id;
                            importCersEquipmnet.IsValidate = item.IsValidate;
                            importCersEquipmnet.Ishandled = false;
                            //importCersEquipmnet.ErrorMessage = "";
                        }
                        else
                        {
                            isNew = true;
                            importCersEquipmnet.Id = Guid.NewGuid();
                            importCersEquipmnet.Label = item.Label;
                            importCersEquipmnet.IsValidate = item.IsValidate;
                        }
                        // 2015.12.20 记录错误信息 chao
                        importCersEquipmnet.ErrorMessage = item.ErrorMessage;
                    }
                    else
                    {
                        importCersEquipmnet.IsValidate = false;
                        importCersEquipmnet.ErrorMessage += "资产编号为空;";
                    }
                    importCersEquipmnet.AssetsCode = item.AssetsCode;
                    importCersEquipmnet.ClassificationCode = item.ClassificationCode;
                    importCersEquipmnet.CHNName = item.CHNName;
                    importCersEquipmnet.ENGName = item.ENGName;
                    importCersEquipmnet.Name = item.Name;
                    importCersEquipmnet.Alias = item.Alias;
                    importCersEquipmnet.Model = item.Model;
                    importCersEquipmnet.QualificationNoHTML = item.QualificationNoHTML;
                    importCersEquipmnet.ApplicationArea = item.ApplicationArea;
                    importCersEquipmnet.ApplicationCode = item.ApplicationCode;
                    importCersEquipmnet.Accessory = item.Accessory;
                    importCersEquipmnet.Certification = item.Certification;
                    importCersEquipmnet.Manufacturer = item.Manufacturer;
                    importCersEquipmnet.ManuCertification = item.ManuCertification;
                    importCersEquipmnet.ProductionArea = item.ProductionArea;
                    importCersEquipmnet.PriceRMB = item.PriceRMB;
                    importCersEquipmnet.PriceUnit = item.PriceUnit;
                    importCersEquipmnet.Factory = item.Factory;
                    importCersEquipmnet.Price = item.Price;
                    importCersEquipmnet.ProduceDate = item.ProduceDate;
                    importCersEquipmnet.PurchaseDate = item.PurchaseDate;
                    importCersEquipmnet.BuyDate = item.BuyDate;
                    importCersEquipmnet.ServiceDate = item.ServiceDate;
                    importCersEquipmnet.RelativePic = item.RelativePic;
                    importCersEquipmnet.Organization = item.Organization;
                    importCersEquipmnet.GroupID = item.GroupID;
                    importCersEquipmnet.Location = item.Location;
                    importCersEquipmnet.ZIPCode = item.ZIPCode;
                    importCersEquipmnet.ContactPerson = item.ContactPerson;
                    importCersEquipmnet.Telephone = item.Telephone;
                    importCersEquipmnet.Email = item.Email;
                    importCersEquipmnet.ShareLevel = item.ShareLevel;
                    importCersEquipmnet.OpenCalendar = item.OpenCalendar;
                    importCersEquipmnet.ServiceCharge = item.ServiceCharge;
                    importCersEquipmnet.ServiceUsers = item.ServiceUsers;
                    importCersEquipmnet.ServiceAchievements = item.ServiceAchievements;
                    importCersEquipmnet.State = item.State;
                    importCersEquipmnet.URL = item.URL;
                    importCersEquipmnet.UpdateDate = DateTime.Now.ToString();
                    importCersEquipmnet.OtherInfo = item.OtherInfo;
                    importCersEquipmnet.TaxProveID = item.TaxProveID;
                    importCersEquipmnet.SerialNO = item.SerialNO;
                    importCersEquipmnet.ImportOrderID = item.ImportOrderID;
                    importCersEquipmnet.ContractID = item.ContractID;
                    importCersEquipmnet.ImportPort = item.ImportPort;
                    importCersEquipmnet.InChargeCustoms = item.InChargeCustoms;
                    importCersEquipmnet.ImportDate = item.ImportDate;
                    importCersEquipmnet.IsShare = item.IsShare;
                    importCersEquipmnet.IsPriceApproved = item.IsPriceApproved;
                    importCersEquipmnet.Record = item.Record;
                    importCersEquipmnet.HSCode = item.HSCode;
                    if (importCersEquipmnet.ErrorMessage!=null && importCersEquipmnet.ErrorMessage.Length > 800) importCersEquipmnet.ErrorMessage = importCersEquipmnet.ErrorMessage.Substring(0, 400);
                    if (isNew)
                        Add(new ImportCersEquipmnet[] { importCersEquipmnet }, ref tran, true);      
                    else
                        Save(new ImportCersEquipmnet[] { importCersEquipmnet }, ref tran, true);     
                    tran.CommitTransaction();
                }
                catch (Exception ex)
                {
                    importCersEquipmnet.ErrorMessage = ex.GetBaseException().Message;
                    importCersEquipmnet.IsValidate = false;
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            if (isDeal) importCersEquipmnetList = DealImportCersEquipmnet(importCersEquipmnetList);
            return importCersEquipmnetList;
        }

        private IList<ImportCersEquipmnet> DealImportCersEquipmnet(IList<ImportCersEquipmnet> importCersEquipmnetList)
        {
            if (importCersEquipmnetList == null || importCersEquipmnetList.Count == 0)
                return null;
            foreach (var item in importCersEquipmnetList.Where(p => p.IsValidate == true))
            {
                XTransaction tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    bool isNewEquipment = false;
                    bool isNewEquipmentExtend = false;
                    bool isSave = true;
                    bool isNewEquipmentCustoms = false;
                    bool isNewEquipmentCustomsBind = false;
                    EquipmentCustoms equipmentCustoms = null;
                    EquipmentCustomsBind equipmentCustomsBind = null;
                    var equipment = __equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"*IsStop=false*IsDelete=false", item.Label));
                    if (equipment == null)
                    {
                        isNewEquipment = true;
                        isNewEquipmentExtend = true;
                        equipment = new Equipment();
                        equipment.Id = Guid.NewGuid();
                        equipment.Label = item.Label;
                        equipment.EquipmentExtend = new List<EquipmentExtend>();
                        EquipmentExtend equipmentExtend = new EquipmentExtend();
                        equipmentExtend.Id = Guid.NewGuid();
                        equipmentExtend.EquipmentId = equipment.Id;
                        equipmentExtend.UpdateDate = DateTime.Now;
                        equipmentExtend.IsUpload = true;
                        equipment.EquipmentExtend.Add(equipmentExtend);
                    }
                    else
                    {
                        var tempEquipmentExtend = __equipmentExtendBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"", equipment.Id.ToString()));
                        if (tempEquipmentExtend != null)
                        {
                            equipment.EquipmentExtend[0] = tempEquipmentExtend;
                            equipment.EquipmentExtend[0].UpdateDate = DateTime.Now;
                            equipment.EquipmentExtend[0].IsUpload = true;
                        }
                        else
                        {
                            isNewEquipmentExtend = true;
                            equipment.EquipmentExtend = new List<EquipmentExtend>();
                            EquipmentExtend equipmentExtend = new EquipmentExtend();
                            equipmentExtend.Id = Guid.NewGuid();
                            equipmentExtend.EquipmentId = equipment.Id;
                            equipmentExtend.UpdateDate = DateTime.Now;
                            equipmentExtend.IsUpload = true;
                            equipment.EquipmentExtend.Add(equipmentExtend);
                        }
                    }

                    equipment.EquipmentExtend[0].AssetsCode = item.AssetsCode;
                    equipment.EquipmentExtend[0].ClassificationCode = item.ClassificationCode;
                    equipment.Name = item.Name;
                    equipment.EquipmentExtend[0].ENGName = item.ENGName;
                    equipment.EquipmentExtend[0].Alias = item.Alias;
                    equipment.Model = item.Model;
                    equipment.QualificationNoHTML = item.QualificationNoHTML;
                    equipment.EquipmentExtend[0].ApplicationArea = item.ApplicationArea;
                    equipment.EquipmentExtend[0].ApplicationCode = item.ApplicationCode;
                    equipment.EquipmentExtend[0].Accessory = item.Accessory;
                    equipment.EquipmentExtend[0].Certification = item.Certification;
                    equipment.Factory = item.Factory;
                    equipment.EquipmentExtend[0].ManuCertification = item.ManuCertification;
                    if (!string.IsNullOrWhiteSpace(item.ProductionArea))
                    {
                        var country = __countryBLL.GetFirstOrDefaultEntityByExpression(string.Format("(CodeNum=\"{0}\"+CountryCode=\"{0}\")", item.ProductionArea));
                        if (country != null) equipment.ProductionArea = country.Name;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "国家代码错误;";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.PriceRMB))
                    {
                        double priceRMBTemp = 0d;
                        if (!double.TryParse(item.PriceRMB, out priceRMBTemp) || priceRMBTemp < 0d)
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "仪器原值必须是大于或者等于的数字;";
                        }
                        else
                            equipment.UnitPrice = double.Parse(item.PriceRMB);
                    }
                    equipment.CurrencyType = item.UnitPrice;
                    if (!string.IsNullOrWhiteSpace(item.Price))
                    {
                        double priceTemp = 0d;
                        if (!double.TryParse(item.Price, out priceTemp) || priceTemp < 0d)
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "外币原值必须是大于或者等于的数字;";
                        }
                        else
                            equipment.EquipmentExtend[0].Price = decimal.Parse(item.Price);
                    }
                    if (!string.IsNullOrWhiteSpace(item.ProduceDate))
                    {
                        DateTime tempProduceDate;
                        if (CheckDate(item.ProduceDate, out tempProduceDate)) equipment.ProductionDate = tempProduceDate;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "生产日期格式错误;";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.BuyDate))
                    {
                        DateTime tempBuyDate;
                        if (CheckDate(item.BuyDate, out tempBuyDate)) equipment.BuyDate = tempBuyDate;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "购置日期格式错误;";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.ServiceDate))
                    {

                        DateTime tempServiceDate;
                        if (CheckDate(item.ServiceDate, out tempServiceDate)) equipment.EquipmentExtend[0].ServiceDate = tempServiceDate;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "启用日期格式错误;";
                        }
                    }
                    else
                    {
                        isSave = false;
                        item.IsValidate = false;
                        item.ErrorMessage += "启用日期为空";
                    }
                    if (item.RelativePic.IndexOf("UploadFiles") > 0)
                        equipment.RelativePic = item.RelativePic;
                    else
                        equipment.RelativePic = "/UploadFiles/EquipmentIcons/" + item.RelativePic + ".jpg";
                    var organization = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*IsStop=false*IsDelete=false*Type={1}", item.Organization, 0));
                    if (organization != null)
                        equipment.OrgId = organization.id;
                    else
                    {
                        isSave = false;
                        item.IsValidate = false;
                        item.ErrorMessage += "找不到所在院系或实验室名称,必须填院校名称;";
                    }

                    var room = __labOrganizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"*ParentId=\"{1}\"*IsStop=false*IsDelete=false*Type={2}", item.Location, organization == null ? "" : organization.Id.ToString(), 1));
                    if (room != null)
                    {
                        equipment.RoomId = room.id;
                    }
                    if (organization != null && room == null)
                    {

                        LabOrganization equipmentRoom = new LabOrganization()
                        {
                            Id = Guid.NewGuid(),
                            LabOrganizationType = LabOrganizationType.LabRoom,
                            ParentId = equipment.OrgId,
                            Name = item.Location,
                            IsDelete = false
                        };
                        equipmentRoom.XPath = XPathUtility.GenerateXPath(null, equipment.OrgId,
                            (entityId) => { return __labOrganizationBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                            (parentEntityId) => { return __labOrganizationBLL.GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                            () => { return __labOrganizationBLL.GetEntitiesByExpression("ParentId=null*IsDelete=false"); });
                        __labOrganizationBLL.Add(new LabOrganization[] { equipmentRoom }, ref tran, true);
                        //tran.CommitTransaction();
                        equipment.RoomId = equipmentRoom.id;
                    }
                    if (!string.IsNullOrWhiteSpace(item.ZIPCode))
                    {
                        if (!!string.IsNullOrWhiteSpace(item.ZIPCode) && !ValidateUtility.IsPostcode(item.ZIPCode))
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "邮政编码格式错误;";
                        }
                        else equipment.EquipmentExtend[0].ZIPCode = item.ZIPCode;
                    }
                    if (!string.IsNullOrWhiteSpace(item.ContactPerson))
                    {
                        if (equipment.Linkmen == null) equipment.Linkmen = new List<EquipmentLinkman>();
                        var user = __userBLL.GetEntitiesByExpression(string.Format("UserName=\"{0}\"*IsDel=false", item.ContactPerson));
                        if (user != null)
                        {
                            var equipmentLinkman = __equipmentLinkmanBLL.GetEntitiesByExpression(string.Format("LinkmanId=\"{0}\"*EquipmentId=\"{1}\"", user[0].id, equipment.Id.ToString()));
                            if (equipmentLinkman == null)
                            {
                                EquipmentLinkman equipmentLinkman2 = new EquipmentLinkman();
                                equipmentLinkman2.Id = Guid.NewGuid();
                                equipmentLinkman2.EquipmentId = equipment.Id;
                                equipmentLinkman2.LinkmanId = user[0].id;
                                equipment.Linkmen.Add(equipmentLinkman2);
                            }
                        }
                        else
                        {
                            equipment.LinkUserName = item.ContactPerson;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.LinkTelNo)) equipment.LinkTelNo = item.LinkTelNo.Length > 19 ? item.LinkTelNo.Substring(0, 19) : item.LinkTelNo;
                    equipment.EquipmentExtend[0].Email = item.Email;
                    equipment.EquipmentExtend[0].ShareLevel = item.ShareLevel.Length > 1 ? item.ShareLevel.Substring(0, 1) : item.ShareLevel;
                    equipment.EquipmentExtend[0].OpenCalendar = item.OpenCalendar;
                    equipment.EquipmentExtend[0].ServiceCharge = item.ServiceCharge;
                    equipment.EquipmentExtend[0].ServiceUsers = item.ServiceUsers;
                    equipment.EquipmentExtend[0].ServiceAchievements = item.ServiceAchievements;
                    equipment.Remark = item.OtherInfo;
                    equipment.EquipmentExtend[0].URL = "http://" + HttpContext.Current.Request.UrlReferrer.Authority + "/Equipment/Show?Id=" + equipment.Id.ToString();
                    equipment.IsDutyFree = item.IsCustoms != null && item.IsCustoms == "1" ? true : false;                       
                        
                    if (item.IsCustoms != null && item.IsCustoms == "1")
                    {

                        equipment.IsDutyFree = true;
                        if (!string.IsNullOrWhiteSpace(item.TaxProveID))
                        {
                            equipmentCustoms = __equipmentCustomsBLL.GetFirstOrDefaultEntityByExpression(string.Format("TaxProveID=\"{0}\"", item.TaxProveID));
                            if (equipmentCustoms == null)
                            {
                                isNewEquipmentCustoms = true;
                                equipmentCustoms = new EquipmentCustoms();
                                equipmentCustoms.Id = Guid.NewGuid();
                                equipmentCustoms.TaxProveID = item.TaxProveID;
                                isNewEquipmentCustomsBind = true;
                                equipmentCustomsBind = new EquipmentCustomsBind();
                                equipmentCustomsBind.Id = Guid.NewGuid();
                                equipmentCustomsBind.EquipmentCustomsId = equipmentCustoms.Id;
                                equipmentCustomsBind.EquipmentId = equipment.Id;

                            }
                            else
                            {
                                equipmentCustomsBind = __equipmentCustomsBindBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentCustomsId=\"{0}\"*EquipmentId", equipmentCustoms.Id.ToString(), equipment.Id.ToString()));
                                if (equipmentCustomsBind == null)
                                {
                                    isNewEquipmentCustomsBind = true;
                                    equipmentCustomsBind = new EquipmentCustomsBind();
                                    equipmentCustomsBind.Id = Guid.NewGuid();
                                    equipmentCustomsBind.EquipmentCustomsId = equipmentCustoms.Id;
                                    equipmentCustomsBind.EquipmentId = equipment.Id;
                                }
                            }
                        }
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "征免税证明编号;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.SerialNO))
                        {
                            int serialNOTemp = 0;
                            if (!int.TryParse(item.SerialNO, out serialNOTemp) || serialNOTemp < 0)
                            {
                                isSave = false;
                                item.IsValidate = false;
                                item.ErrorMessage += "序号必须是大于或者等于0的整数;";
                            }
                            else
                                equipmentCustoms.SerialNO = int.Parse(item.SerialNO);
                        }
                        if (!string.IsNullOrWhiteSpace(item.ImportOrderID)) equipmentCustoms.ImportOrderID = item.ImportOrderID;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "进口报关单编号为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.ContractID)) equipmentCustoms.ContractID = item.ContractID;
                        if (!string.IsNullOrWhiteSpace(item.ImportPort)) equipmentCustoms.ImportPort = item.ImportPort;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "进口口岸为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.InChargeCustoms)) equipmentCustoms.InChargeCustoms = item.InChargeCustoms;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "主管海关为空;";
                        }

                        if (!string.IsNullOrWhiteSpace(item.ImportDate))
                        {
                            DateTime tempImportDate;
                            if (CheckDate(item.ImportDate, out tempImportDate)) equipmentCustoms.ImportDate = tempImportDate;
                            else
                            {
                                isSave = false;
                                item.IsValidate = false;
                                item.ErrorMessage += "进口日期格式错误;";
                            }
                        }
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "进口日期为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.IsShare)) equipmentCustoms.IsShare = item.IsShare == "1" ? true : false;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "申报共享标志为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.IsPriceApproved)) equipmentCustoms.IsPriceApproved = item.IsPriceApproved == "1" ? true : false;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "收费标准已评议标志为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.HSCode)) equipmentCustoms.HSCode = item.HSCode;
                        else
                        {
                            isSave = false;
                            item.IsValidate = false;
                            item.ErrorMessage += "HS编码为空;";
                        }
                        if (!string.IsNullOrWhiteSpace(item.Record)) equipmentCustoms.Record = item.Record;
                    }
                    if (isSave)
                    {
                        if (isNewEquipment)
                            __equipmentBLL.Add(new Equipment[] { equipment }, ref tran, true);
                        else
                            __equipmentBLL.Save(new Equipment[] { equipment }, ref tran, true);
                        if (equipment.Linkmen != null && equipment.Linkmen.Count > 0)
                        {
                            var Linkmen = __equipmentLinkmanBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*LinkmanId=\"{1}\"", equipment.Linkmen[0].EquipmentId, equipment.Linkmen[0].LinkmanId));
                            if (Linkmen == null) __equipmentLinkmanBLL.Add(equipment.Linkmen, ref tran, true);
                        }
                        if (isNewEquipmentExtend)
                        {
                            __EquipmentExtendBLL.Add(new EquipmentExtend[] { equipment.EquipmentExtend[0] }, ref tran, true);
                        }
                        else
                        {
                            __EquipmentExtendBLL.Save(new EquipmentExtend[] { equipment.EquipmentExtend[0] }, ref tran, true);
                        }
                        if (isNewEquipmentCustoms)
                        {
                            __equipmentCustomsBLL.Add(new EquipmentCustoms[] { equipmentCustoms }, ref tran, true);
                        }
                        if (!isNewEquipmentCustoms && equipmentCustoms != null)
                        {
                            __equipmentCustomsBLL.Save(new EquipmentCustoms[] { equipmentCustoms }, ref tran, true);
                        }
                        if (isNewEquipmentCustomsBind && equipmentCustomsBind != null) __equipmentCustomsBindBLL.Add(new EquipmentCustomsBind[] { equipmentCustomsBind }, ref tran, true);
                        item.Ishandled = true;
                        item.ErrorMessage = "";
                        item.IsValidate = true;
                        Save(new ImportCersEquipmnet[] { item }, ref tran, true);
                        tran.CommitTransaction();

                    }
                    else
                    {
                        Save(new ImportCersEquipmnet[] { item }, ref tran, true);
                        tran.CommitTransaction();
                    }
                }
                catch (Exception ex)
                {
                    //importCersEquipmnetList.Remove(item);
                    item.Ishandled = false;
                    item.IsValidate = false;
                    item.ErrorMessage = ex.Message;
                    if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                }
                finally
                {
                    if (tran != null) tran.Dispose();
                }
            }
            return importCersEquipmnetList;
        }

        private bool CheckDate(string oldDate, out DateTime newDate)
        {
            string importDate;
            if (oldDate.Length == 6)
                importDate = oldDate.Substring(0, 4) + "-" + oldDate.Substring(4, 2) + "-01";
            else if (oldDate.Length == 8)
                importDate = oldDate.Substring(0, 4) + "-" + oldDate.Substring(4, 2) + "-" + oldDate.Substring(6, 2);
            else
                importDate = oldDate;
            DateTime tempImportDate;
            if (DateTime.TryParse(importDate, out tempImportDate))
            {
                newDate = tempImportDate;
                return true;
            }
            else
            {
                newDate = DateTime.Parse("");
                return false;
            }
        }   
    }
}