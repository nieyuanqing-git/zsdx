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
    public class MaterialInfoBLL : BLLBase<MaterialInfo>, IMaterialInfoBLL
    {
        private IDictCodeBLL __dictCodeBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialInfoLogBLL __materialInfoLogBLL;
        private IMaterialSupplierBLL __materialSupplierBLL;
        public MaterialInfoBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialInfoLogBLL = BLLFactory.CreateMaterialInfoLogBLL();
            __materialSupplierBLL = BLLFactory.CreateMaterialSupplierBLL();
        }

        private bool DelMaterialInfo(Guid userId, Guid materialInfoId, ref XTransaction tran, out string errorMessage)
        {
            IMaterialAdminBLL materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var materialInfo = GetEntityById(materialInfoId);
                if (materialInfo == null) string.Format("出错,找不到编码为【{0}】的耗材信息", materialInfoId);
                if (materialInfo.MaterialAdminList != null && materialInfo.MaterialAdminList.Count() > 0)
                    materialAdminBLL.Delete(materialInfo.MaterialAdminList.Select(p => p.Id), ref tran, true);
                materialInfo.Label = "_del_" + materialInfo.Label;
                materialInfo.IsDelete = true;
                Save(new MaterialInfo[] { materialInfo }, ref tran, true);
                __materialInfoLogBLL.AddInfoLog(userId,
                    materialInfo.Id,
                    0,
                    materialInfo.StockValue,
                    MaterialInfoLogOperateType.Delete,
                    MaterialInfoLogBusinessType.Info,
                    materialInfo.Id,
                    "耗材删除",
                    ref tran);
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
        public bool DelMaterialInfo(Guid userId, Guid materialInfoId, out  string errorMessage)
        {
            XTransaction tran = null;
            return DelMaterialInfo(userId, materialInfoId, ref tran, out errorMessage);
        }
        public bool DelMaterialInfos(Guid userId, IList<MaterialInfo> materialInfos, out  string errorMessage)
        {
            errorMessage = "";
            if (materialInfos != null && materialInfos.Count > 0)
            {
                var tran = SessionManager.Instance.BeginTransaction();
                try
                {
                    foreach (var materialInfo in materialInfos)
                    {
                        var result = DelMaterialInfo(userId, materialInfo.Id, ref tran, out errorMessage);
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
        public bool SaveMaterialInfo(MaterialInfo materialInfo,IList<MaterialSupplier> materialSupplierList, DictCode dictCodeUnit, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                if (materialInfo == null) throw new Exception("耗材信息为空");
                tran = SessionManager.Instance.BeginTransaction();
                
                if (dictCodeUnit != null && __dictCodeBLL.GetEntityById(dictCodeUnit.Id) == null)
                    __dictCodeBLL.Add(new DictCode[] { dictCodeUnit }, ref tran, true);
                var oldMaterialInfo = GetEntityById(materialInfo.Id);
                if (oldMaterialInfo != null)
                {
                    if (oldMaterialInfo.MaterialAdminList != null && oldMaterialInfo.MaterialAdminList.Count() > 0)
                        __materialAdminBLL.Delete(oldMaterialInfo.MaterialAdminList.Select(p => p.Id), ref tran, true);
                    if (oldMaterialInfo.StockValue != materialInfo.StockValue)
                    {
                        __materialInfoLogBLL.AddInfoLog(materialInfo.UpdateUserId,
                            materialInfo.Id,
                            materialInfo.StockValue - oldMaterialInfo.StockValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Edit,
                            MaterialInfoLogBusinessType.Info,
                            materialInfo.Id,
                            "库存值: " + oldMaterialInfo.StockValue + "修改为" + materialInfo.StockValue, 
                            ref tran);
                    }
                    var delQueryExpress = "";
                    if(materialSupplierList != null && materialSupplierList.Count() > 0)
                        delQueryExpress = "*" + materialSupplierList.Select(p=>p.Id).ToFormatStr("Id",LogicRelation.AndNot);
                    var delMaterialSupplierList = __materialSupplierBLL.GetEntitiesByExpression(string.Format("MaterialInfoId=\"{0}\"{1}", materialInfo.Id, delQueryExpress));
                    if (delMaterialSupplierList != null && delMaterialSupplierList.Count() > 0)
                        __materialSupplierBLL.Delete(delMaterialSupplierList.Select(p => p.Id), ref tran, true);
                    Save(new MaterialInfo[] { materialInfo }, ref tran, true);
                }
                else
                {
                    Add(new MaterialInfo[] { materialInfo }, ref tran, true);
                    __materialInfoLogBLL.AddInfoLog(materialInfo.CreaterId,
                            materialInfo.Id,
                            materialInfo.StockValue,
                            materialInfo.StockValue,
                            MaterialInfoLogOperateType.Add,
                            MaterialInfoLogBusinessType.Info,
                            materialInfo.Id,
                            "库存值:" + materialInfo.StockValue,
                            ref tran);
                }
                if (materialInfo.MaterialAdminList != null && materialInfo.MaterialAdminList.Count() > 0)
                    __materialAdminBLL.Add(materialInfo.MaterialAdminList, ref tran, true);
                if (materialSupplierList != null && materialSupplierList.Count() > 0)
                {
                    foreach (var item in materialSupplierList)
                    {
                        var oldItem = __materialSupplierBLL.GetEntityById(item.Id);
                        if (oldItem != null)
                            __materialSupplierBLL.Save(new MaterialSupplier[] { item }, ref tran, true);
                        else
                            __materialSupplierBLL.Add(new MaterialSupplier[] { item }, ref tran, true);
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