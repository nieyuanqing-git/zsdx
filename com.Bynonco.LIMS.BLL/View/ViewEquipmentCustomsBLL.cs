using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business.CERS.InstrusAndGroups;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewEquipmentCustomsBLL : BLLBase<ViewEquipmentCustoms>, IViewEquipmentCustomsBLL
    {
        private IDictCodeBLL __dictCodeBLL = null;

        public ViewEquipmentCustomsBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public RootCustomsCustom[] FillCustoms()
        {
            var viewEquipmentCustomsList = GetEntitiesByExpression("Id=-null");
            if (viewEquipmentCustomsList == null || viewEquipmentCustomsList.Count() == 0) return null;
            RootCustomsCustom[] customs = new RootCustomsCustom[viewEquipmentCustomsList.Count()];
            var schoolCode = __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolCode");
            for (int i = 0; i < viewEquipmentCustomsList.Count(); i++)
            {
                customs[i] = FillCustoms(viewEquipmentCustomsList[i]);
                if (customs[i] != null)
                {
                    customs[i].SchoolCode = schoolCode;
                }
            }
            return customs;
        }

        private RootCustomsCustom FillCustoms(ViewEquipmentCustoms viewEquipmentCustoms)
        {
            RootCustomsCustom customs = new RootCustomsCustom();
            customs.InnerID = viewEquipmentCustoms.Label;
            customs.TaxProveID = viewEquipmentCustoms.TaxProveID;
            customs.SerialNO = viewEquipmentCustoms.SerialNO.HasValue ? viewEquipmentCustoms.SerialNO.Value : 0;
            customs.ImportOrderID = viewEquipmentCustoms.ImportOrderID;
            customs.ContractID = viewEquipmentCustoms.ContractID;
            customs.ImportPort = viewEquipmentCustoms.ImportPort;
            customs.InChargeCustoms = viewEquipmentCustoms.InChargeCustoms;
            customs.ImportDate = viewEquipmentCustoms.ImportDate;
            customs.IsShare = viewEquipmentCustoms.IsShare ? 1 : 0;
            customs.IsPriceApproved = viewEquipmentCustoms.IsPriceApproved.HasValue && viewEquipmentCustoms.IsPriceApproved.Value == true ? 1 : 0;
            customs.HSCode = viewEquipmentCustoms.HSCode;
            customs.Record = viewEquipmentCustoms.Record == null ? "" : viewEquipmentCustoms.Record;
            return customs;
        }
    }
}