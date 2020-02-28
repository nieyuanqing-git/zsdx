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
    public class MaterialOutputBLL : BLLBase<MaterialOutput>, IMaterialOutputBLL
    {
        private IMaterialRecipientBLL __materialRecipientBLL;
        private IMaterialOutputItemBLL __materialOutputItemBLL;
        private IMaterialInfoBLL __materialInfoBLL;
        private IMaterialInfoLogBLL __materialInfoLogBLL;
        public MaterialOutputBLL()
        {
            __materialRecipientBLL = BLLFactory.CreateMaterialRecipientBLL();
            __materialOutputItemBLL = BLLFactory.CreateMaterialOutputItemBLL();
            __materialInfoBLL = BLLFactory.CreateMaterialInfoBLL();
            __materialInfoLogBLL = BLLFactory.CreateMaterialInfoLogBLL();
        }
        private bool DelMaterialOutput(Guid userId, Guid materialOutputId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialOutput = GetEntityById(materialOutputId);
                if (materialOutput == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的出库单信息", materialOutputId));
                if (materialOutput.StatusEnum != MaterialOutputStatus.UnDeduct) throw new Exception("出错,当前出库单已扣费");
                materialOutput.IsDelete = true;
                var itemList = __materialOutputItemBLL.GetEntitiesByExpression(string.Format("MaterialOutputId=\"{0}\"*IsDelete=false", materialOutputId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList)
                    {
                        item.IsDelete = true;

                        var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                        materialInfo.StockValue += item.OutputValue;
                        __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                        __materialInfoLogBLL.AddInfoLog(materialOutput.UpdateUserId,
                        materialInfo.Id,
                        item.OutputValue,
                        materialInfo.StockValue,
                        MaterialInfoLogOperateType.Delete,
                        MaterialInfoLogBusinessType.Output,
                        materialOutput.Id,
                        "耗材出库删除,库存+" + item.OutputValue,
                        ref tran);
                    }
                    __materialOutputItemBLL.Save(itemList, ref tran, true);
                }
                if (materialOutput.MaterialRecipientId.HasValue)
                {
                    var materialRecipient = __materialRecipientBLL.GetEntityById(materialOutput.MaterialRecipientId.Value);
                    materialRecipient.Status = (int)MaterialRecipientStatus.AuditPass;
                    materialRecipient.UpdateUserId = userId;
                    materialRecipient.UpdateTime = DateTime.Now;
                    __materialRecipientBLL.Save(new MaterialRecipient[] { materialRecipient }, ref tran, true);
                }
                Save(new MaterialOutput[] { materialOutput }, ref tran, true);
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
        public bool DelMaterialOutput(Guid userId, Guid materialOutputId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialOutput(userId, materialOutputId, ref tran, out errorMessage);
        }
        public bool DelMaterialOutputs(Guid userId, IList<MaterialOutput> materialOutputs, out  string errorMessage)
        {
            errorMessage = "";
            if (materialOutputs != null && materialOutputs.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialOutput in materialOutputs)
                    {
                        var result = DelMaterialOutput(userId, materialOutput.Id, ref tran, out errorMessage);
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
        public bool SaveMaterialOutput(MaterialOutput materialOutput, IList<MaterialOutputItem> materialOutputItemList, bool isDeduct, bool isAllowAccountMinuse, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (materialOutput == null || materialOutputItemList == null || materialOutputItemList.Count() == 0) throw new Exception("出库单和出库项目信息不完整");
                tran = SessionManager.Instance.BeginTransaction();
                var oldMaterialOutput = GetEntityById(materialOutput.Id);
                if (oldMaterialOutput != null)
                {
                    var delMaterialOutputItemList = __materialOutputItemBLL.GetEntitiesByExpression(
                        string.Format("MaterialOutputId=\"{0}\"*IsDelete=false*{1}", oldMaterialOutput.Id, materialOutputItemList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot)));
                    if (delMaterialOutputItemList != null && delMaterialOutputItemList.Count() > 0)
                    {
                        foreach (var item in delMaterialOutputItemList)
                        {
                            item.IsDelete = true;

                            var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                            materialInfo.StockValue += item.OutputValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialOutput.UpdateUserId,
                            materialInfo.Id,
                            item.OutputValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Delete,
                            MaterialInfoLogBusinessType.Output,
                            materialOutput.Id,
                            "耗材出库删除,库存+" + item.OutputValue,
                            ref tran);
                        }
                        __materialOutputItemBLL.Save(delMaterialOutputItemList, ref tran, true);
                    }
                    Save(new MaterialOutput[] { materialOutput }, ref tran, true);
                }
                else
                {
                    if (materialOutput.MaterialRecipientId.HasValue)
                    {
                        if (GetFirstOrDefaultEntityByExpression(string.Format("MaterialRecipientId=\"{0}\"*IsDelete=false", materialOutput.MaterialRecipientId.Value), null, "", true) != null)
                            throw new Exception(string.Format("采购单【{0}】已经出库", materialOutput.MaterialRecipientId.Value));
                        var materialRecipient = __materialRecipientBLL.GetEntityById(materialOutput.MaterialRecipientId.Value);
                        materialRecipient.Status = (int)MaterialRecipientStatus.Output;
                        materialRecipient.UpdateUserId = materialOutput.UpdateUserId;
                        materialRecipient.UpdateTime = DateTime.Now;
                        __materialRecipientBLL.Save(new MaterialRecipient[] { materialRecipient }, ref tran, true);
                    }
                    Add(new MaterialOutput[] { materialOutput }, ref tran, true);
                }
                foreach (var item in materialOutputItemList)
                {
                    var oldItem = __materialOutputItemBLL.GetEntityById(item.Id);
                    if (oldItem != null)
                    {
                        if (item.OutputValue != oldItem.OutputValue)
                        {
                            var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                            materialInfo.StockValue -= item.OutputValue - oldItem.OutputValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialOutput.UpdateUserId,
                               materialInfo.Id,
                               oldItem.OutputValue - item.OutputValue,
                               materialInfo.StockValue,
                               MaterialInfoLogOperateType.Edit,
                               MaterialInfoLogBusinessType.Output,
                               materialOutput.Id,
                               "耗材出库修改,出库量:" + oldItem.OutputValue + "->" + item.OutputValue,
                               ref tran);
                        }
                        __materialOutputItemBLL.Save(new MaterialOutputItem[] { item }, ref tran, true);
                    }
                    else
                    {
                        var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                        materialInfo.StockValue -= item.OutputValue;
                        __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                        __materialInfoLogBLL.AddInfoLog(materialOutput.UpdateUserId,
                            materialInfo.Id,
                            0-item.OutputValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Add,
                            MaterialInfoLogBusinessType.Output,
                            materialOutput.Id,
                            "耗材出库新增,出库量:" + item.OutputValue,
                            ref tran);
                        __materialOutputItemBLL.Add(new MaterialOutputItem[] { item }, ref tran, true);
                    }
                }
                if (isDeduct)
                {
                    materialOutput.DeductTime = DateTime.Now;
                    materialOutput.DeductUserId = materialOutput.UpdateUserId;
                    new CostDeductManager().MaterialCostDeduct(materialOutput, materialOutput.UpdateUserId, isAllowAccountMinuse, false, ref tran, "");
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

        private bool CancelDeductMaterialOutput(Guid userId, Guid materialOutputId, ref XTransaction tran, out string errorMessage)
        {
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialOutput = GetEntityById(materialOutputId);
                if (materialOutput == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的出库单信息", materialOutputId));
                if (materialOutput.StatusEnum == MaterialOutputStatus.ClosingAccount) throw new Exception("出错,当前出库单已结算");
                if (materialOutput.StatusEnum == MaterialOutputStatus.UnDeduct) throw new Exception("出错,当前出库单未扣费");
                materialOutput.Status = (int)MaterialOutputStatus.UnDeduct;
                materialOutput.DeductUserId = null;
                materialOutput.DeductTime = null;
                materialOutput.UpdateUserId = userId;
                materialOutput.UpdateTime = DateTime.Now;
                Save(new MaterialOutput[] { materialOutput }, ref tran, true);
                new CostDeductManager().CancelMaterialCostDeduct(materialOutput, ref tran, materialOutput.UpdateUserId, "");
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
        public bool CancelDeductMaterialOutput(Guid userId, Guid materialOutputId, out  string errorMessage)
        {
            XTransaction tran = null;
            return CancelDeductMaterialOutput(userId, materialOutputId, ref tran, out errorMessage);
        }
        public bool CancelDeductMaterialOutputs(Guid userId, IList<MaterialOutput> materialOutputs, out  string errorMessage)
        {
            errorMessage = "";
            if (materialOutputs != null && materialOutputs.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialOutput in materialOutputs)
                    {
                        var result = DelMaterialOutput(userId, materialOutput.Id, ref tran, out errorMessage);
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