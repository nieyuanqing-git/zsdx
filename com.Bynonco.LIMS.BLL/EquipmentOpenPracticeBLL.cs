using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentOpenPracticeBLL : BLLBase<EquipmentOpenPractice>, IEquipmentOpenPracticeBLL
    {
        private IEquipmentOpenPracticeEquipmentBLL __equipmentOpenPracticeEquipmentBLL;
        private IEquipmentOpenPracticeMaterialBLL __equipmentOpenPracticeMaterialBLL;
        private IEquipmentOpenPracticeMemberBLL __equipmentOpenPracticeMemberBLL;
        private IEquipmentOpenPracticeTeacherBLL __equipmentOpenPracticeTeacherBLL;
        public EquipmentOpenPracticeBLL()
        {
            __equipmentOpenPracticeEquipmentBLL = BLLFactory.CreateEquipmentOpenPracticeEquipmentBLL();
            __equipmentOpenPracticeMaterialBLL = BLLFactory.CreateEquipmentOpenPracticeMaterialBLL();
            __equipmentOpenPracticeMemberBLL = BLLFactory.CreateEquipmentOpenPracticeMemberBLL();
            __equipmentOpenPracticeTeacherBLL = BLLFactory.CreateEquipmentOpenPracticeTeacherBLL();
        }
        public bool DelEquipmentOpenPractice(Guid id, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var delItem = GetEntityById(id);
                if (delItem == null) throw new Exception("实践研究项目为空");
                if (delItem.StatusEnum == EquipmentOpenPracticeStatus.WaitingAudit) throw new Exception(string.Format("无法删除状态为{0}的项", delItem.StatusStr));
                delItem.IsDelete = true;
                Save(new EquipmentOpenPractice[] { delItem }, ref tran, true);
                if (delItem.EquipmentOpenPracticeEquipmentList != null && delItem.EquipmentOpenPracticeEquipmentList.Count() > 0)
                {
                    delItem.EquipmentOpenPracticeEquipmentList.ToList().ForEach(m => m.IsDelete = true);
                    __equipmentOpenPracticeEquipmentBLL.Save(delItem.EquipmentOpenPracticeEquipmentList, ref tran, true);
                }
                if (delItem.EquipmentOpenPracticeMaterialList != null && delItem.EquipmentOpenPracticeMaterialList.Count() > 0)
                {
                    delItem.EquipmentOpenPracticeMaterialList.ToList().ForEach(m => m.IsDelete = true);
                    __equipmentOpenPracticeMaterialBLL.Save(delItem.EquipmentOpenPracticeMaterialList, ref tran, true);
                }
                if (delItem.EquipmentOpenPracticeMemberList != null && delItem.EquipmentOpenPracticeMemberList.Count() > 0)
                {
                    delItem.EquipmentOpenPracticeMemberList.ToList().ForEach(m => m.IsDelete = true);
                    __equipmentOpenPracticeMemberBLL.Save(delItem.EquipmentOpenPracticeMemberList, ref tran, true);
                }
                if (delItem.EquipmentOpenPracticeTeacherList != null && delItem.EquipmentOpenPracticeTeacherList.Count() > 0)
                {
                    delItem.EquipmentOpenPracticeTeacherList.ToList().ForEach(m => m.IsDelete = true);
                    __equipmentOpenPracticeTeacherBLL.Save(delItem.EquipmentOpenPracticeTeacherList, ref tran, true);
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.ToString().StartsWith("出错,") ?  ex.ToString() : "出错, " + ex.ToString();
                return false;
            }
            finally { tran.Dispose(); }
        }
    }
}