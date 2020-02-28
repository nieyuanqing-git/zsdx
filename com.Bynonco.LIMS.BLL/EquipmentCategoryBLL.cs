using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.JqueryEasyUI.Core;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentCategoryBLL : BLLBase<EquipmentCategory>, IEquipmentCategoryBLL
    {
        public EquipmentCategory GetEquipmentCategoryByXPath(string xPath)
        {
            if (string.IsNullOrEmpty(xPath)) return null;
            var equipmentCategoryList = GetEntitiesByExpression(string.Format("IsDelete=false*XPath=\"{0}\"", xPath));
            return equipmentCategoryList == null || equipmentCategoryList.Count() == 0 ? null : equipmentCategoryList.First();
        }

        public bool SaveImportEquipmentCategory(IList<ImportEquipmentCategoryLog> importEquipmentCategoryLogList, out string errorMessage)
        {
            errorMessage = "";
            if (importEquipmentCategoryLogList != null && importEquipmentCategoryLogList.Count() == 0)
            {
                errorMessage = "导入数据为空";
                return false;
            }
            importEquipmentCategoryLogList = importEquipmentCategoryLogList.OrderBy(p => p.XPath).ToList();
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                IList<EquipmentCategory> equipmentCategoryList = new List<EquipmentCategory>();
                foreach (var item in importEquipmentCategoryLogList)
                {
                    var equipmentCategory = new EquipmentCategory();
                    equipmentCategory.Id = item.EquipmentCategoryId;
                    equipmentCategory.Name = item.Name;
                    equipmentCategory.Code = item.Code;
                    equipmentCategory.InputStr = ShortcutStringUtility.Chinese2Pinyin(equipmentCategory.Name);
                    equipmentCategory.XPath = item.XPath;
                    equipmentCategory.ParentId = item.ParentId;
                    equipmentCategoryList.Add(equipmentCategory);
                }
                Add(equipmentCategoryList, ref tran, true);
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