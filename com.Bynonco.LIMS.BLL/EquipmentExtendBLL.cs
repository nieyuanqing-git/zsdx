using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Business.CERS.InstrusAndGroups;
using com.Bynonco.LIMS.Model.View;
using System.Web;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentExtendBLL : BLLBase<EquipmentExtend>, IEquipmentExtendBLL
    {
        private IDictCodeBLL __dictCodeBLL = null;
        private IEquipmentBLL __equipmentBLL;
        private IViewEquipmentExtendBLL __viewEquipmentExtendBLL;
        private string LIMSUrl;
        public EquipmentExtendBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __viewEquipmentExtendBLL = BLLFactory.CreateViewEquipmentExtendBLL();
            LIMSUrl = __dictCodeBLL.GetDictCodeValueByCode("System", "LimsUrl");
        }
        public RootInstrusInstru[] FillInstrus()
        {
            var viewEquipmentInstruList = __viewEquipmentExtendBLL.GetEntitiesByExpression("EquipmentExtendId=-null*IsUpload=true");
            if (viewEquipmentInstruList == null || viewEquipmentInstruList.Count() == 0) return null;
            RootInstrusInstru[] Instrus = new RootInstrusInstru[viewEquipmentInstruList.Count()];
            var schoolCode = __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolCode");
            for (int i = 0; i < viewEquipmentInstruList.Count(); i++)
            {
                Instrus[i] = FillInstru(viewEquipmentInstruList[i]);
                if (Instrus[i] != null)
                {
                    Instrus[i].SchoolCode = schoolCode;
                }
            }
            return Instrus;
        }

        private RootInstrusInstru FillInstru(ViewEquipmentExtend viewEquipmentExtend)
        {
            RootInstrusInstru instru = new RootInstrusInstru();
            instru.InnerID = viewEquipmentExtend.Label;
            instru.AssetsCode = viewEquipmentExtend.AssetsCode;
            instru.ClassificationCode = viewEquipmentExtend.ClassificationCode;
            instru.CHNName = viewEquipmentExtend.Name;
            instru.ENGName = viewEquipmentExtend.ENGName;
            instru.Alias = viewEquipmentExtend.Alias;
            instru.Model = viewEquipmentExtend.Model;
            instru.TechParameter = viewEquipmentExtend.QualificationNoHTML;
            instru.ApplicationArea = viewEquipmentExtend.ApplicationArea;
            instru.ApplicationCode = viewEquipmentExtend.ApplicationCode;
            instru.Accessory = viewEquipmentExtend.Accessory;
            instru.Certification = viewEquipmentExtend.Certification;
            instru.Manufacturer = viewEquipmentExtend.Factory;
            instru.ManuCertification = viewEquipmentExtend.ManuCertification;
            instru.ManuCountryCode = viewEquipmentExtend.ProductionArea;
            if (viewEquipmentExtend.UnitPrice.HasValue)
                instru.PriceRMB = decimal.Parse(viewEquipmentExtend.PriceRMB.ToString());
            if (viewEquipmentExtend.Price.HasValue)
            {
                instru.PriceSpecified = true;
                instru.Price = decimal.Parse(viewEquipmentExtend.Price.ToString());
            }
            else
                instru.PriceSpecified = false;
            instru.PriceUnit = viewEquipmentExtend.PriceUnit;
            if (viewEquipmentExtend.ProduceDate.HasValue)
                instru.ProduceDate = viewEquipmentExtend.ProduceDate.Value.Date;
            else
                instru.ProduceDateSpecified = false;
            if (viewEquipmentExtend.BuyDate.HasValue)
                instru.PurchaseDate = viewEquipmentExtend.BuyDate.Value.Date;
            else
                instru.PurchaseDateSpecified = false;
            if (viewEquipmentExtend.ServiceDate.HasValue)
                instru.ServiceDate = viewEquipmentExtend.ServiceDate.Value.Date;
            else
                instru.ServiceDate = DateTime.Now;
            instru.ImageURL = LIMSUrl+viewEquipmentExtend.ImageURL;
            instru.Organization = viewEquipmentExtend.Organization;
            instru.GroupID = viewEquipmentExtend.GroupID;
            instru.Location = viewEquipmentExtend.Location;
            instru.ZIPCode = viewEquipmentExtend.ZIPCode;
            instru.ContactPerson = viewEquipmentExtend.ContactPerson;
            instru.Telephone = viewEquipmentExtend.LinkTelNo;
            instru.Email = viewEquipmentExtend.Email;
            instru.ShareLevel = viewEquipmentExtend.ShareLevel.ToString();
            instru.OpenCalendar = viewEquipmentExtend.OpenCalendar;
            instru.ServiceCharge = viewEquipmentExtend.ServiceCharge;
            instru.ServiceUsers = viewEquipmentExtend.ServiceUsers;
            instru.ServiceAchievements = viewEquipmentExtend.ServiceAchievements;
            instru.State = viewEquipmentExtend.State.HasValue ? viewEquipmentExtend.State.Value : 0;
            instru.URL = viewEquipmentExtend.URL;
            if (viewEquipmentExtend.UpdateDate.HasValue)
                instru.UpdateDate = viewEquipmentExtend.UpdateDate.Value.Date;
            else
                instru.UpdateDate = DateTime.Now;
            instru.OtherInfo = viewEquipmentExtend.OtherInfo;
            instru.IsSupervised = viewEquipmentExtend.IsSupervised.HasValue && viewEquipmentExtend.IsSupervised.Value == true ? 1 : 0;
            return instru;
        }
    }
}