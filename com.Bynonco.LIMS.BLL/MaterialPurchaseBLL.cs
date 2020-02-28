using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class MaterialPurchaseBLL : BLLBase<MaterialPurchase>, IMaterialPurchaseBLL
    {
        private IMaterialPurchaseItemBLL __materialPurchaseItemBLL;
        public MaterialPurchaseBLL()
        {
            __materialPurchaseItemBLL = BLLFactory.CreateMaterialPurchaseItemBLL();
        }
        private bool DelMaterialPurchase(Guid materialPurchaseId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialPurchase = GetEntityById(materialPurchaseId);
                if (materialPurchase == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的采购单信息", materialPurchaseId));
                materialPurchase.IsDelete = true;
                var itemList = __materialPurchaseItemBLL.GetEntitiesByExpression(string.Format("MaterialPurchaseId=\"{0}\"*IsDelete=false", materialPurchaseId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList) item.IsDelete = true;
                    __materialPurchaseItemBLL.Save(itemList, ref tran, true);
                }
                Save(new MaterialPurchase[] { materialPurchase }, ref tran, true);
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
        public bool DelMaterialPurchase(Guid materialPurchaseId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialPurchase(materialPurchaseId, ref tran, out errorMessage);
        }
        public bool DelMaterialPurchases(IList<MaterialPurchase> materialPurchases, out  string errorMessage)
        {
            errorMessage = "";
            if (materialPurchases != null && materialPurchases.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialPurchase in materialPurchases)
                    {
                        var result = DelMaterialPurchase(materialPurchase.Id, ref tran, out errorMessage);
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
    }
}