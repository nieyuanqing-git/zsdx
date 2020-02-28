using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class AnimalBLL : BLLBase<Animal>, IAnimalBLL
    {
        public IList<Animal> GetAnimalListByCageId(Guid cageId)
        {
            var cageAnimalList = GetEntitiesByExpression(string.Format("IsDelete=false*AnimalCageId=\"{0}\"*StoreStatus=-{1}*StoreStatus=-{2}*StoreStatus=-{3}*StoreStatus=-{4}", cageId, (int)AnimalStoreStatus.RegisterDeath, (int)AnimalStoreStatus.Die, (int)AnimalStoreStatus.GetOut, (int)AnimalStoreStatus.RegisterGettingOut));
            return cageAnimalList;
        }
        public bool ConfirmRegisterDeath(Animal animal, Guid? operatorId, ConfirmDeathStatus confirmDeathStatus, string confirmDeathRemark, out string errorMessage)
        {
            return ConfirmRegisterDeath(false, animal, operatorId, confirmDeathStatus, confirmDeathRemark, out errorMessage);
        }
        public bool ConfirmRegisterDeath(Animal animal,out string errorMessage)
        {
            return ConfirmRegisterDeath(true, animal, null, ConfirmDeathStatus.Agree, "系统自动进行死亡登记确认", out errorMessage);
        }
        private bool ConfirmRegisterDeath(bool isAuto,Animal animal, Guid? operatorId, ConfirmDeathStatus confirmDeathStatus, string confirmDeathRemark, out string errorMessage)
        {
            var viewAnimalBLL = BLLFactory.CreateViewAnimalBLL();
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                var viewAnimal = viewAnimalBLL.GetEntityById(animal.Id);
                if (!viewAnimalBLL.JudgeIsEnableConfirmRegistingDeath(isAuto,operatorId, viewAnimal, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                if (confirmDeathStatus == ConfirmDeathStatus.Refuse && string.IsNullOrWhiteSpace(confirmDeathRemark))
                {
                    throw new Exception(ConfirmDeathStatus.Refuse.ToCnName() + ",备注不能为空");
                }
                tran = SessionManager.Instance.BeginTransaction();
                animal.ConfirmDeathDate = DateTime.Now;
                animal.ConfirmDeathOperatorId = operatorId.HasValue ? operatorId.Value : animal.OwnerId.Value;
                animal.ConfirmDeathRemark = confirmDeathRemark;
                animal.ConfirmDeathStatus = (int)confirmDeathStatus;
                if (((ConfirmDeathStatus)confirmDeathStatus) == ConfirmDeathStatus.Agree)
                {
                    animal.Status = (int)AnimalStatus.Dead;
                    animal.StoreStatus = (int)AnimalStoreStatus.Die;
                }
                Save(new Animal[] { animal }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
        }
        public bool RegisterDeath(Animal animal, Guid? operatorId,bool isNotice, DateTime dieTime,double? dieWeight,Guid? dieWeightUnitId,string remark, out string errorMessage)
        {
            var viewAnimalBLL = BLLFactory.CreateViewAnimalBLL();
            var dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
            var userBLL = BLLFactory.CreateUserBLL();
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                User operateUser = operatorId.HasValue ? userBLL.GetEntityById(operatorId.Value) : null;
                var viewAnimal = viewAnimalBLL.GetEntityById(animal.Id);
                if (!viewAnimalBLL.JudgeIsEnableRegisterDeath(operatorId, viewAnimal, out errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                int maxConfirmDeathDays = 30;
                var dictCodeType = dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression("Code=ConfirmAnimalDeathMaxDays");
                if (dictCodeType != null && !string.IsNullOrWhiteSpace(dictCodeType.Value) && int.TryParse(dictCodeType.Value, out maxConfirmDeathDays) && maxConfirmDeathDays > 0)
                {
                    maxConfirmDeathDays = int.Parse(dictCodeType.Value);
                }
                tran = SessionManager.Instance.BeginTransaction();
                animal.ConfirmDeathMaxDays = maxConfirmDeathDays;
                animal.RegisterDeathDate = DateTime.Now;
                animal.RegisterDeathOperatorId = operatorId;
                animal.RegisterDeathRemark = remark;
                animal.StoreStatus = (int)AnimalStoreStatus.RegisterDeath;
                animal.IsNotice = isNotice;
                animal.DieTime = dieTime;
                animal.DieWeight = null;
                animal.DieWeightUnitId = null;
                if (dieWeight.HasValue)
                {
                    animal.DieWeight = dieWeight.Value;
                    animal.DieWeightUnitId = dieWeightUnitId.Value;
                }
                viewAnimal.ConfirmDeathMaxDays = animal.ConfirmDeathMaxDays;
                viewAnimal.RegisterDeathDate = animal.RegisterDeathDate;
                viewAnimal.RegisterDeathOperatorId = animal.RegisterDeathOperatorId;
                viewAnimal.RegisterDeathRemark = animal.RegisterDeathRemark;
                viewAnimal.StoreStatus = animal.StoreStatus;
                viewAnimal.IsNotice = animal.IsNotice;
                viewAnimal.DieTime = animal.DieTime;
                viewAnimal.DieWeight = animal.DieWeight;
                viewAnimal.DieWeightUnitId = animal.DieWeightUnitId;
                Save(new Animal[] { animal }, ref tran, true);
                tran.CommitTransaction();
                if (animal.IsNotice)
                {
                    new MessageHandler().SendAnimalRegisterDeathNotice(viewAnimal, operateUser);
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction(); 
                return false; 
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
        public bool ValidatePos(string idStr, string animalCageIdStr, string animalCageRowNoStr, string animalCageColNoStr, string animalCategoryIdStr, out string errorMessage)
        {
            errorMessage = "";
            var animalCageBLL = BLLFactory.CreateAnimalCageBLL();
            var viewAnimalBLL = BLLFactory.CreateViewAnimalBLL();
            if (string.IsNullOrWhiteSpace(animalCageIdStr))
            {
                errorMessage = "笼位信息为空";
                return false;
            }
            if (!animalCageIdStr.Trim().IsGuid())
            {
                errorMessage = "笼子编号错误";
                return false;
            }
            var animalCage = animalCageBLL.GetEntityById(new Guid(animalCageIdStr));
            if (animalCage == null)
            {
                errorMessage = string.Format("找不到编码为【{0}】的笼子信息", animalCageIdStr);
                return false;
            }
            int animalCageRowNo = -1;
            if (string.IsNullOrWhiteSpace(animalCageRowNoStr))
            {
                errorMessage = "笼子行号为空";
                return false;
            }
            if (!int.TryParse(animalCageRowNoStr, out animalCageRowNo) || animalCageRowNo <= 0)
            {
                errorMessage = "笼子行号不是大于0的整数";
                return false;
            }
            if (animalCageRowNo > animalCage.RowCnt)
            {
                errorMessage = string.Format("笼子行号不是大于笼子的行的最大数量【{0}】", animalCage.RowCnt);
                return false;
            }
            int animalCageColNo = -1;
            if (string.IsNullOrWhiteSpace(animalCageColNoStr))
            {
                errorMessage = "笼子列号为空";
                return false;
            }
            if (!int.TryParse(animalCageColNoStr, out animalCageColNo) || animalCageColNo <= 0)
            {
                errorMessage = "笼子列号不是大于0的整数";
                return false;
            }
            if (animalCageColNo > animalCage.ColCnt)
            {
                errorMessage = string.Format("笼子列号不是大于笼子的列的最大数量【{0}】", animalCage.ColCnt);
                return false;
            }
            var enbaleQueryExpression = GenerateEnableAnimalQueryExpression();
            //var animalFind = GetFirstOrDefaultEntityByExpression(string.Format("IsDelete=false*AnimalCageId=\"{0}\"*AnimalCageRowNo={1}*AnimalCageColNo={2}*Id=-\"{3}\"*(StoreStatus=-{4}*StoreStatus=-{5}*StoreStatus=-{6}*StoreStatus=-{7})", animalCage.Id, animalCageRowNo, animalCageColNoStr, idStr, (int)AnimalStoreStatus.GetOut, (int)AnimalStoreStatus.Die, (int)AnimalStoreStatus.RegisterDeath, (int)AnimalStoreStatus.RegisterGettingOut));
            var animalFind = GetFirstOrDefaultEntityByExpression(string.Format("AnimalCageId=\"{0}\"*AnimalCageRowNo={1}*AnimalCageColNo={2}*Id=-\"{3}\"*{4}", animalCage.Id, animalCageRowNo, animalCageColNoStr, idStr, enbaleQueryExpression));
            if (animalFind != null)
            {
                errorMessage = string.Format("笼子【{0}】【{1}】行【{2}】列【{3}】已经存在动物【{4}】【{5}】", animalCage.Name, animalCage.Code, animalCageRowNo, animalCageColNo, animalFind.Name, animalFind.Code);
                return false;
            }
            var viewAnimalFind = viewAnimalBLL.GetFirstOrDefaultEntityByExpression(string.Format("IsDelete=false*AnimalCageId=\"{0}\"*Id=-\"{1}\"*(StoreStatus=-{2}*StoreStatus=-{3}*StoreStatus=-{4}*StoreStatus=-{5})*AnimalCategoryId=-\"{6}\"", animalCage.Id, idStr, (int)AnimalStoreStatus.GetOut, (int)AnimalStoreStatus.RegisterDeath, (int)AnimalStoreStatus.Die, (int)AnimalStoreStatus.RegisterGettingOut, animalCategoryIdStr));
            if (viewAnimalFind != null)
            {
                errorMessage = string.Format("同一个笼子不能存放不同品系的动物,该笼子已存放动物【{0}({1})】的品系为【{2}】与当前动物的品系不同", viewAnimalFind.Name, viewAnimalFind.Code, viewAnimalFind.AnimalCategoryName);
                return false;
            }
            return true;
        }
        public bool BatchIn(IEnumerable<Guid> animalIds, Guid animalCageId, Guid operatorId, string remark,out string errorMessage)
        {
            var animalCageBLL = BLLFactory.CreateAnimalCageBLL();
            var animalCage = animalCageBLL.GetEntityById(animalCageId);
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                var animalList = GetEntitiesByExpression(animalIds.ToFormatStr());
                if (animalList == null || animalList.Count == 0) throw new Exception("动物信息为空");
                var hasStoredAnimalList = animalList.Where(p=>p.AnimalCageId.HasValue);
                if (hasStoredAnimalList != null && hasStoredAnimalList.Count() > 0) throw new Exception(string.Format("动物:{0},已经入库,不能重复入库", string.Join(",", hasStoredAnimalList.Select(p => p.Name))));
                var animalCategoryGroup = animalList.GroupBy(p=>p.AnimalCategoryId);
                if (animalCategoryGroup.Count() > 1) throw new Exception(string.Format("一个笼位只能存放一种品系的动物，存在{0}个品系", animalCategoryGroup.Count()));
                var cageAnimalList = GetAnimalListByCageId(animalCageId);
                var freePosCount = animalCage.RowCnt * animalCage.ColCnt - (cageAnimalList != null ? cageAnimalList.Count : 0);
                var count = animalList.Count(p => !p.AnimalCageId.HasValue);
                if (count > freePosCount) throw new Exception(string.Format("入库的动物数量:{0}大于空闲笼位数量{1}", count, freePosCount));
                tran = SessionManager.Instance.BeginTransaction();
                foreach (var animal in animalList)
                {
                    for (int i = 1; i <= animalCage.RowCnt; i++)
                    {
                        if (animal.AnimalCageId.HasValue) break;
                        for (int j = 1; j <= animalCage.ColCnt; j++)
                        {
                            if (cageAnimalList != null && cageAnimalList.Count > 0 && cageAnimalList.Any(p => p.AnimalCageRowNo == i && p.AnimalCageColNo == j)) continue;
                            if (animalList.Any(p => p.AnimalCageRowNo == i && p.AnimalCageColNo == j)) continue;
                            if (!In(animal, animalCage, i, j, operatorId, remark, ref tran, out errorMessage)) throw new Exception(errorMessage);
                            break;
                        }
                    }
                }
                if ( tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
        private bool In(Animal animal, AnimalCage animalCage, int animalCageRowNo, int animalCageColNo, Guid operatorId, string remark, ref XTransaction tran, out string errorMessage)
        {
            var viewAnimalBLL = BLLFactory.CreateViewAnimalBLL();
            errorMessage = "";
            bool isSupress = tran != null;
            try
            {
                if (tran != null) tran = SessionManager.Instance.BeginTransaction();
                var viewAnimal = viewAnimalBLL.GetEntityById(animal.Id);
                if (!ValidatePos(animal.Id.ToString(), animalCage.Id.ToString(), animalCageRowNo.ToString(), animalCageColNo.ToString(), animal.AnimalCategoryId.ToString(), out errorMessage))
                {
                    throw new Exception(errorMessage);
                }
                if (!animal.EnterCageTime.HasValue)
                {
                    animal.EnterCageTime = DateTime.Now;
                }
                animal.AnimalCageId = animalCage.Id;
                animal.AnimalCageRowNo = animalCageRowNo;
                animal.AnimalCageColNo = animalCageColNo;
                animal.InDate = DateTime.Now;
                animal.InOperatorId = operatorId;
                animal.InRemark = remark;
                if (animal.StoreStatus == (int)AnimalStoreStatus.Input)
                {
                    animal.StoreStatus = (int)AnimalStoreStatus.In;
                }
                Save(new Animal[] { animal }, ref tran);
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (!isSupress && tran != null && tran.HasTransaction) tran.RollbackTransaction();
                return false;
            }
            finally { if (!isSupress && tran != null) tran.Dispose(); }
            return true;
        }
        public bool In(Guid animalId, Guid animalCageId, int animalCageRowNo, int animalCageColNo, Guid operatorId, string remark, ref XTransaction tran, out string errorMessage)
        {
            var animalCageBLL = BLLFactory.CreateAnimalCageBLL();
            var animal = GetEntityById(animalId);
            var animalCage = animalCageBLL.GetEntityById(animalCageId);
            return In(animal, animalCage, animalCageRowNo, animalCageColNo, operatorId, remark, ref tran, out errorMessage);
        }
        public string GenerateEnableAnimalQueryExpression()
        {
            return string.Format("IsDelete=false*(StoreStatus=-{0}*StoreStatus=-{1})", (int)AnimalStoreStatus.GetOut, (int)AnimalStoreStatus.Die);
        }
    }
}
