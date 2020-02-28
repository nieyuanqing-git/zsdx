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
    public class MaterialBrokenBLL : BLLBase<MaterialBroken>, IMaterialBrokenBLL
    {
        private IMaterialBrokenItemBLL __materialBrokenItemBLL;
        private IMaterialInfoBLL __materialInfoBLL;
        private IMaterialInfoLogBLL __materialInfoLogBLL;
        public MaterialBrokenBLL()
        {
            __materialBrokenItemBLL = BLLFactory.CreateMaterialBrokenItemBLL();
            __materialInfoBLL = BLLFactory.CreateMaterialInfoBLL();
            __materialInfoLogBLL = BLLFactory.CreateMaterialInfoLogBLL();
        }
        private bool DelMaterialBroken(Guid userId, Guid materialBrokenId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialBroken = GetEntityById(materialBrokenId);
                if (materialBroken == null) throw new Exception(string.Format("出错,找不到编码为【{0}】的报废单信息", materialBrokenId));
                materialBroken.IsDelete = true;
                var itemList = __materialBrokenItemBLL.GetEntitiesByExpression(string.Format("MaterialBrokenId=\"{0}\"*IsDelete=false", materialBrokenId), null, "", true);
                if (itemList != null && itemList.Count() > 0)
                {
                    foreach (var item in itemList)
                    {
                        item.IsDelete = true;

                        if (materialBroken.StatusEnum == MaterialBrokenStatus.Output)
                        {
                            var materialInfo = __materialInfoBLL.GetEntityById(item.Id);
                            materialInfo.StockValue += item.BrokenValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialBroken.UpdateUserId,
                            materialInfo.Id,
                            item.BrokenValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Delete,
                            MaterialInfoLogBusinessType.Broken,
                            materialBroken.Id,
                            "耗材报废删除,库存+" + item.BrokenValue,
                            ref tran);
                        }
                    }
                    __materialBrokenItemBLL.Save(itemList, ref tran, true);
                }
                Save(new MaterialBroken[] { materialBroken }, ref tran, true);
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
        public bool DelMaterialBroken(Guid userId, Guid materialBrokenId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialBroken(userId, materialBrokenId, ref tran, out errorMessage);
        }
        public bool DelMaterialBrokens(Guid userId, IList<MaterialBroken> materialBrokens, out  string errorMessage)
        {
            errorMessage = "";
            if (materialBrokens != null && materialBrokens.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialBroken in materialBrokens)
                    {
                        var result = DelMaterialBroken(userId, materialBroken.Id, ref tran, out errorMessage);
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

        public bool SaveMaterialBroken(MaterialBroken materialBroken, IList<MaterialBrokenItem> materialBrokenItemList, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (materialBroken == null || materialBrokenItemList == null || materialBrokenItemList.Count() == 0) throw new Exception("报废单和报废项目信息不完整");
                tran = SessionManager.Instance.BeginTransaction();
                var status = materialBroken.StatusEnum;
                var oldMaterialBroken = GetEntityById(materialBroken.Id);
                if (oldMaterialBroken != null)
                {
                    var oldStatus = oldMaterialBroken.StatusEnum;
                    var delMaterialBrokenItemList = __materialBrokenItemBLL.GetEntitiesByExpression(
                        string.Format("MaterialBrokenId=\"{0}\"*IsDelete=false*{1}", oldMaterialBroken.Id, materialBrokenItemList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot)));
                    if (delMaterialBrokenItemList != null && delMaterialBrokenItemList.Count() > 0)
                    {
                        foreach (var item in delMaterialBrokenItemList)
                        {
                            item.IsDelete = true;
                            if (oldStatus == MaterialBrokenStatus.Output)
                            {
                                var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                                materialInfo.StockValue += item.BrokenValue;
                                __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                                __materialInfoLogBLL.AddInfoLog(materialBroken.UpdateUserId,
                                materialInfo.Id,
                                item.BrokenValue,
                                materialInfo.StockValue,
                                MaterialInfoLogOperateType.Delete,
                                MaterialInfoLogBusinessType.Broken,
                                materialBroken.Id,
                                "耗材报废删除,库存+" + item.BrokenValue,
                                ref tran);
                            }
                        }
                        __materialBrokenItemBLL.Save(delMaterialBrokenItemList, ref tran, true);
                    }
                    Save(new MaterialBroken[] { materialBroken }, ref tran, true);
                }
                else
                {
                    Add(new MaterialBroken[] { materialBroken }, ref tran, true);
                }
                var isOutput = status == MaterialBrokenStatus.Output;
                foreach (var item in materialBrokenItemList)
                {
                    var oldItem = __materialBrokenItemBLL.GetEntityById(item.Id);
                    if (isOutput && oldItem != null && oldMaterialBroken != null && oldMaterialBroken.StatusEnum == MaterialBrokenStatus.Output)
                    {
                        if (item.BrokenValue != oldItem.BrokenValue)
                        {
                            var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                            materialInfo.StockValue -= item.BrokenValue - oldItem.BrokenValue;
                            __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                            __materialInfoLogBLL.AddInfoLog(materialBroken.UpdateUserId,
                               materialInfo.Id,
                               oldItem.BrokenValue - item.BrokenValue,
                               materialInfo.StockValue,
                               MaterialInfoLogOperateType.Edit,
                               MaterialInfoLogBusinessType.Broken,
                               materialBroken.Id,
                               "耗材报废修改,报废量:" + oldItem.BrokenValue + "->" + item.BrokenValue,
                               ref tran);
                        }
                    }
                    else if(isOutput)
                    {
                        var materialInfo = __materialInfoBLL.GetEntityById(item.MaterialInfoId);
                        materialInfo.StockValue -= item.BrokenValue;
                        __materialInfoBLL.Save(new MaterialInfo[] { materialInfo }, ref tran, true);

                        __materialInfoLogBLL.AddInfoLog(materialBroken.UpdateUserId,
                            materialInfo.Id,
                            0 - item.BrokenValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Add,
                            MaterialInfoLogBusinessType.Broken,
                            materialBroken.Id,
                            "耗材报废新增,报废量:" + item.BrokenValue,
                            ref tran);
                    }
                    if (oldItem != null)
                        __materialBrokenItemBLL.Save(new MaterialBrokenItem[] { item }, ref tran, true);
                    else
                        __materialBrokenItemBLL.Add(new MaterialBrokenItem[] { item }, ref tran, true);
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