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
    public class MaterialRecipientBLL : BLLBase<MaterialRecipient>, IMaterialRecipientBLL
    {
        private IMaterialRecipientItemBLL __materialRecipientItemBLL;
        public MaterialRecipientBLL()
        {
            __materialRecipientItemBLL = BLLFactory.CreateMaterialRecipientItemBLL();
        }
        private bool DelMaterialRecipient(Guid materialRecipientId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialRecipient = GetEntityById(materialRecipientId);
                if (materialRecipient == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的领用单信息", materialRecipientId));
                materialRecipient.IsDelete = true;
                var itemList = __materialRecipientItemBLL.GetEntitiesByExpression(string.Format("MaterialRecipientId=\"{0}\"*IsDelete=false", materialRecipientId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList) item.IsDelete = true;
                    __materialRecipientItemBLL.Save(itemList, ref tran, true);
                }
                Save(new MaterialRecipient[] { materialRecipient }, ref tran, true);
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
        public bool DelMaterialRecipient(Guid materialRecipientId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialRecipient(materialRecipientId, ref tran, out errorMessage);
        }
        public bool DelMaterialRecipients(IList<MaterialRecipient> materialRecipients, out  string errorMessage)
        {
            errorMessage = "";
            if (materialRecipients != null && materialRecipients.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialRecipient in materialRecipients)
                    {
                        var result = DelMaterialRecipient(materialRecipient.Id, ref tran, out errorMessage);
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