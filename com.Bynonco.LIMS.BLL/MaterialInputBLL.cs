using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class MaterialInputBLL : BLLBase<MaterialInput>, IMaterialInputBLL
    {
        private IMaterialPurchaseBLL __materialPurchaseBLL;
        private IMaterialInputItemBLL __materialInputItemBLL;
        private IMaterialInfoBLL __materialInfoBLL;
        private IMaterialInfoLogBLL __materialInfoLogBLL;
        public MaterialInputBLL()
        {
            __materialPurchaseBLL = BLLFactory.CreateMaterialPurchaseBLL();
            __materialInputItemBLL = BLLFactory.CreateMaterialInputItemBLL();
            __materialInfoBLL = BLLFactory.CreateMaterialInfoBLL();
            __materialInfoLogBLL = BLLFactory.CreateMaterialInfoLogBLL();
        }
        private bool DelMaterialInput(Guid userId, Guid materialInputId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialInput = GetEntityById(materialInputId);
                if (materialInput == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的进库单信息", materialInputId));
                if (materialInput.MaterialPurchaseId.HasValue)
                {
                    var materialPurchase = __materialPurchaseBLL.GetEntityById(materialInput.MaterialPurchaseId.Value);
                    if(materialPurchase.StatusEnum != MaterialPurchaseStatus.InputPass)
                        throw new Exception(string.Format("出错,进库单关联的采购单状态为【{0}】,不能删除", materialPurchase.StatusEnum));
                }
                materialInput.IsDelete = true;
                var itemList = __materialInputItemBLL.GetEntitiesByExpression(string.Format("MaterialInputId=\"{0}\"*IsDelete=false", materialInputId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList)
                    {
                        item.IsDelete = true;

                        var materialInfo = __materialInfoBLL.GetEntityById(item.Id);
                        materialInfo.StockValue -= item.InputValue;
                        __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                        __materialInfoLogBLL.AddInfoLog(materialInput.UpdateUserId,
                        materialInfo.Id,
                        0 - item.InputValue,
                        materialInfo.StockValue,
                        MaterialInfoLogOperateType.Delete,
                        MaterialInfoLogBusinessType.Input,
                        materialInput.Id,
                        "耗材进库删除,库存" + (0 - item.InputValue),
                        ref tran);
                    }
                    __materialInputItemBLL.Save(itemList, ref tran, true);
                }
                if (materialInput.MaterialPurchaseId.HasValue)
                {
                    var materialPurchase = __materialPurchaseBLL.GetEntityById(materialInput.MaterialPurchaseId.Value);
                    materialPurchase.Status = (int)MaterialPurchaseStatus.OperatorAuditPass;
                    materialPurchase.AuditInputorId = null;
                    materialPurchase.AuditInputTime = null;
                    materialPurchase.AuditInputRemark = "";
                    materialPurchase.UpdateUserId = userId;
                    materialPurchase.UpdateTime = DateTime.Now;
                    __materialPurchaseBLL.Save(new MaterialPurchase[] { materialPurchase }, ref tran, true);
                }
                Save(new MaterialInput[] { materialInput }, ref tran, true);
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
        public bool DelMaterialInput(Guid userId, Guid materialInputId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialInput(userId, materialInputId, ref tran, out errorMessage);
        }
        public bool DelMaterialInputs(Guid userId, IList<MaterialInput> materialInputs, out  string errorMessage)
        {
            errorMessage = "";
            if (materialInputs != null && materialInputs.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialInput in materialInputs)
                    {
                        var result = DelMaterialInput(userId, materialInput.Id, ref tran, out errorMessage);
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
        public bool SaveMaterialInput(MaterialInput materialInput, IList<MaterialInputItem> materialInputItemList, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (materialInput == null || materialInputItemList == null || materialInputItemList.Count() == 0) throw new Exception("进库单和进库项目信息不完整");
                tran = SessionManager.Instance.BeginTransaction();
                MaterialPurchase materialPurchase = null;
                if (materialInput.MaterialPurchaseId.HasValue)
                    materialPurchase = __materialPurchaseBLL.GetEntityById(materialInput.MaterialPurchaseId.Value);
                var oldMaterialInput = GetEntityById(materialInput.Id);
                if (oldMaterialInput != null)
                {
                    var delMaterialInputItemList = __materialInputItemBLL.GetEntitiesByExpression(
                        string.Format("MaterialInputId=\"{0}\"*IsDelete=false*{1}", oldMaterialInput.Id, materialInputItemList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot)));
                    if (delMaterialInputItemList != null && delMaterialInputItemList.Count() > 0)
                    {
                        foreach (var item in delMaterialInputItemList)
                        {
                            item.IsDelete = true;

                            var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                            materialInfo.StockValue -= item.InputValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialInput.UpdateUserId,
                            materialInfo.Id,
                            0 - item.InputValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Delete,
                            MaterialInfoLogBusinessType.Input,
                            materialInput.Id,
                            "耗材进库删除,库存" + (0 - item.InputValue),
                            ref tran);
                        }
                        __materialInputItemBLL.Save(delMaterialInputItemList, ref tran, true);
                    }

                    Save(new MaterialInput[] { materialInput }, ref tran, true);
                }
                else
                {
                    if (materialPurchase != null && materialPurchase.StatusEnum != MaterialPurchaseStatus.OperatorAuditPass)
                        throw new Exception(string.Format("出错,进库单关联的采购单状态为【{0}】,不能新增", materialPurchase.StatusEnum));
                    if(materialPurchase != null) materialPurchase.Status = (int)MaterialPurchaseStatus.InputPass;
                    Add(new MaterialInput[] { materialInput }, ref tran, true);
                }
                if (materialPurchase != null)
                {
                    if(materialPurchase.StatusEnum == MaterialPurchaseStatus.BalancePass || materialPurchase.StatusEnum == MaterialPurchaseStatus.BalanceReject)
                        throw new Exception(string.Format("出错,进库单关联的采购单状态为【{0}】,不能修改", materialPurchase.StatusEnum));
                    materialPurchase.AuditInputorId = materialInput.UpdateUserId;
                    materialPurchase.AuditInputTime = DateTime.Now;
                    materialPurchase.AuditInputRemark = materialInput.Remark;
                    materialPurchase.UpdateUserId = materialInput.UpdateUserId;
                    materialPurchase.UpdateTime = DateTime.Now;
                    __materialPurchaseBLL.Save(new MaterialPurchase[] { materialPurchase }, ref tran, true);
                }
                foreach (var item in materialInputItemList)
                {
                    var oldItem = __materialInputItemBLL.GetEntityById(item.Id) ;
                    if (oldItem != null)
                    {
                        if (item.InputValue != oldItem.InputValue)
                        {
                            var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                            materialInfo.StockValue += item.InputValue - oldItem.InputValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialInput.UpdateUserId,
                               materialInfo.Id,
                               item.InputValue - oldItem.InputValue,
                               materialInfo.StockValue,
                               MaterialInfoLogOperateType.Edit,
                               MaterialInfoLogBusinessType.Input,
                               materialInput.Id,
                               "耗材进库修改,进库量:" + oldItem.InputValue + "->" + item.InputValue,
                               ref tran);
                        }
                        __materialInputItemBLL.Save(new MaterialInputItem[] { item }, ref tran, true);
                    }
                    else
                    {
                        var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                        materialInfo.StockValue += item.InputValue;
                        __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                        __materialInfoLogBLL.AddInfoLog(materialInput.UpdateUserId,
                            materialInfo.Id,
                            item.InputValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Add,
                            MaterialInfoLogBusinessType.Input,
                            materialInput.Id,
                            "耗材进库新增,进库量:" + item.InputValue,
                            ref tran);
                        __materialInputItemBLL.Add(new MaterialInputItem[] { item }, ref tran, true);
                    }
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null) tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错" + ex.Message : ex.Message;
                return false;
            }
            finally { if (tran != null)tran.Dispose(); }
        }
    }
}