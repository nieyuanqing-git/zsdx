using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.august.DataLink;
using com.Bynonco.LIMS.DAL;
using System.Data;

namespace com.Bynonco.LIMS.BLL
{
    public class UserDoorAuthorizationBLL : BLLBase<UserDoorAuthorization>, IUserDoorAuthorizationBLL
    {
        private IEquipmentAuthorizationAndAppointmentBLL __equipmentAuthorizationAndAppointmentBLL;
        private IDoorOfflineAuthorizationBLL __doorOfflineAuthorizationBLL;
        private ISystemLogBLL __systemLogBLL;
        public UserDoorAuthorizationBLL()
        {
            __equipmentAuthorizationAndAppointmentBLL = BLLFactory.CreateEquipmentAuthorizationAndAppointmentBLL();
            __doorOfflineAuthorizationBLL = BLLFactory.CreateDoorOfflineAuthorizationBLL();
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
        }
        public bool SaveUserDoorAuthorization(IList<UserDoorAuthorization> userDoorAuthorizationList, string ip, out string errorMessage)
        {
            errorMessage = "";
            if (userDoorAuthorizationList == null || userDoorAuthorizationList.Count() == 0)
            {
                errorMessage = "出错, 操作记录为空.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var item in userDoorAuthorizationList)
                {
                    item.LeamsUpdated = false;
                    var originalEntity = GetEntityById(item.Id);
                    DoorOfflineAuthorization doorOfflineAuthorization = null;
                    bool isNewDoorOfflineAuthorization = true;
                    if (originalEntity == null)
                    {
                        Add(new UserDoorAuthorization[] { item }, ref tran, true);
                        GenerateEntityLog(OperateType.New, originalEntity, item, ip, ref tran, true);
                        doorOfflineAuthorization = new DoorOfflineAuthorization();
                        doorOfflineAuthorization.Id = Guid.NewGuid();
                        isNewDoorOfflineAuthorization = true;
                    }
                    else
                    {
                        Save(new UserDoorAuthorization[] { item }, ref tran, true);
                        GenerateEntityLog(OperateType.Modified, originalEntity, item, ip, ref tran, true);
                        doorOfflineAuthorization = __doorOfflineAuthorizationBLL.GetFirstOrDefaultEntityByExpression(string.Format("AuthorizationId=\"{0}\"", item.Id));
                        if (doorOfflineAuthorization != null) isNewDoorOfflineAuthorization = false;
                        else
                        {
                            doorOfflineAuthorization = new DoorOfflineAuthorization();
                            doorOfflineAuthorization.Id = Guid.NewGuid();
                            isNewDoorOfflineAuthorization = true;
                        }
                    }
                    doorOfflineAuthorization.DoorId = item.DoorID.Value;
                    doorOfflineAuthorization.UserId = item.UserID.Value;
                    doorOfflineAuthorization.StartDate = item.StartDate.Value;
                    doorOfflineAuthorization.EndDate = item.EndDate.Value;
                    doorOfflineAuthorization.Updated = false;
                    doorOfflineAuthorization.IsDel = false;
                    doorOfflineAuthorization.EquipmentId = null;
                    doorOfflineAuthorization.AuthorizationId = item.Id;
                    doorOfflineAuthorization.LeamsUpdated = false;
                    if (isNewDoorOfflineAuthorization) __doorOfflineAuthorizationBLL.Add(new DoorOfflineAuthorization[] { doorOfflineAuthorization }, ref tran, true);
                    else __doorOfflineAuthorizationBLL.Save(new DoorOfflineAuthorization[] { doorOfflineAuthorization }, ref tran, true);
                    var equipmentAuthorizationAndAppointment = __equipmentAuthorizationAndAppointmentBLL.GetEntityById(item.Id);
                    if (equipmentAuthorizationAndAppointment != null)
                    {
                        if (equipmentAuthorizationAndAppointment.StartDate != item.StartDate || equipmentAuthorizationAndAppointment.EndDate != item.EndDate)
                        {
                            equipmentAuthorizationAndAppointment.StartDate = item.StartDate;
                            equipmentAuthorizationAndAppointment.EndDate = item.EndDate;
                            equipmentAuthorizationAndAppointment.Updated = 0;
                            equipmentAuthorizationAndAppointment.LeamsUpdated = false;
                            __equipmentAuthorizationAndAppointmentBLL.Save(new EquipmentAuthorizationAndAppointment[] { equipmentAuthorizationAndAppointment }, ref tran, true);
                        }
                    }
                    else
                    {
                        equipmentAuthorizationAndAppointment = new EquipmentAuthorizationAndAppointment();
                        equipmentAuthorizationAndAppointment.Id = item.Id;
                        equipmentAuthorizationAndAppointment.EquipmentID = item.DoorID;
                        equipmentAuthorizationAndAppointment.UserID = item.UserID;
                        equipmentAuthorizationAndAppointment.StartDate = item.StartDate;
                        equipmentAuthorizationAndAppointment.EndDate = item.EndDate;
                        equipmentAuthorizationAndAppointment.IsDel = 0;
                        equipmentAuthorizationAndAppointment.Updated = 0;
                        equipmentAuthorizationAndAppointment.LeamsUpdated = false;
                        __equipmentAuthorizationAndAppointmentBLL.Add(new EquipmentAuthorizationAndAppointment[] { equipmentAuthorizationAndAppointment }, ref tran, true);
                    }
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString().StartsWith("出错,") ? ex.ToString() : string.Format("出错,{0}", ex.ToString());
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
        public bool DeleteUserDoorAuthorization(IList<UserDoorAuthorization> userDoorAuthorizationList, string ip, out string errorMessage)
        {
            errorMessage = "";
            if (userDoorAuthorizationList == null || userDoorAuthorizationList.Count() == 0)
            {
                errorMessage = "出错, 操作记录为空.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var userDoorAuthorization in userDoorAuthorizationList)
                {
                    Delete(new Guid[] { userDoorAuthorization.Id }, ref tran, true);
                    GenerateEntityLog(OperateType.Deleted, userDoorAuthorization, userDoorAuthorization, ip, ref tran, true);
                    var equipmentAuthorizationAndAppointment = __equipmentAuthorizationAndAppointmentBLL.GetEntityById(userDoorAuthorization.Id);
                    if (equipmentAuthorizationAndAppointment != null)
                    {
                        equipmentAuthorizationAndAppointment.IsDel = 1;
                        equipmentAuthorizationAndAppointment.LeamsUpdated = false;
                        __equipmentAuthorizationAndAppointmentBLL.Save(new EquipmentAuthorizationAndAppointment[] { equipmentAuthorizationAndAppointment }, ref tran, true);
                    }
                    var doorOfflineAuthorizationList = __doorOfflineAuthorizationBLL.GetEntitiesByExpression(string.Format("AuthorizationId=\"{0}\"", userDoorAuthorization.Id));
                    if (doorOfflineAuthorizationList != null && doorOfflineAuthorizationList.Count() > 0)
                    {
                        foreach (var item in doorOfflineAuthorizationList)
                        {
                            item.IsDel = true;
                            item.Updated = false;
                            item.LeamsUpdated = false;
                        }
                        __doorOfflineAuthorizationBLL.Save(doorOfflineAuthorizationList, ref tran, true);
                    }
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString().StartsWith("出错,") ? ex.ToString() : string.Format("出错,{0}", ex.ToString());
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
        public void GenerateEntityLog(OperateType operateType, UserDoorAuthorization originalEntity, UserDoorAuthorization curEntity, string operateIP, ref XTransaction tran, bool isSupress)
        {
            string title="", content = "", contentHTML = "", entityName = "UserDoorAuthorization", entityCnName = "门禁授权";
            GenerateEntityLogData(operateType, originalEntity, curEntity,out title, out content, out contentHTML, out entityName, out entityCnName);
            SystemLog systemLog = new SystemLog()
            {
                BusinessId = curEntity.Id,
                Id = Guid.NewGuid(),
                OperateEntityName = entityName,
                OperateEntityCnName = entityCnName,
                OperateTime = DateTime.Now,
                OperateTypeEnum = operateType,
                OperateIP = operateIP,
                LogContent = content,
                LogContentHTML = contentHTML,
                OperatorId = curEntity.Authorizer
            };
            __systemLogBLL.Add(new SystemLog[] { systemLog }, ref tran, isSupress);
        }
        public void GenerateEntityLogData(OperateType operateType, UserDoorAuthorization originalEntity, UserDoorAuthorization curEntity,out string title, out string content, out string contentHTML, out string entityName, out string entityCnName)
        {
            entityName = "UserDoorAuthorization";
            entityCnName = "门禁授权";
            title = string.Format("{0}用户[{1}]门禁[{2}]权限.",
                operateType.ToCnName(),
                curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName,
                curEntity == null || curEntity.Door == null ? "" : curEntity.Door.Name);
            StringBuilder sbContnet = new StringBuilder();
            sbContnet.AppendFormat("编码:【{0}】", curEntity.Id).Append("\r\n");
            sbContnet.AppendFormat("用户:【{0}】", curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName).Append("\r\n");
            sbContnet.AppendFormat("门禁:【{0}】", curEntity == null || curEntity.Door == null ? "" : curEntity.Door.Name).Append("\r\n");
            if (operateType == OperateType.Modified)
            {
                sbContnet.AppendFormat("开始时间:【{0}】→【{1}】", originalEntity == null || !originalEntity.StartDate.HasValue ? "" : originalEntity.StartDate.Value.ToString("yyyy-MM-dd HH:mm"), curEntity == null || !curEntity.StartDate.HasValue ? "" : curEntity.StartDate.Value.ToString("yyyy-MM-dd HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("结束时间:【{0}】→【{1}】", originalEntity == null || !originalEntity.EndDate.HasValue ? "" : originalEntity.EndDate.Value.ToString("yyyy-MM-dd HH:mm"), curEntity == null || !curEntity.EndDate.HasValue ? "" : curEntity.EndDate.Value.ToString("yyyy-MM-dd HH:mm")).Append("\r\n");
            }
            else
            {
                sbContnet.AppendFormat("开始时间:【{0}】", curEntity == null || !curEntity.StartDate.HasValue ? "" : curEntity.StartDate.Value.ToString("yyyy-MM-dd HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("结束时间:【{0}】", curEntity == null || !curEntity.EndDate.HasValue ? "" : curEntity.EndDate.Value.ToString("yyyy-MM-dd HH:mm")).Append("\r\n");
            }
            content = sbContnet.ToString();
            contentHTML = content.Replace("\r\n", "<br />");
        }
        public bool ManualDoorAuthorization(string loginName, string doorName, DateTime startTime, DateTime endTime, string note, out string errorMessage)
        {
            errorMessage = "";
            var loginNameDataParam = DataParameterFactory.CreateDataParameter("LoginName", loginName);
            var doorNameDataParam = DataParameterFactory.CreateDataParameter("DoorName", doorName);
            var startTimeDataParam = DataParameterFactory.CreateDataParameter("StartTime", startTime);
            var endTimeDataParam = DataParameterFactory.CreateDataParameter("EndTime", endTime);
            var noteDataParam = DataParameterFactory.CreateDataParameter("Note", note);
            var resultDataParam = DataParameterFactory.CreateDataParameter("Result", 0, ParameterDirection.Output);
            var remarkDataParam = DataParameterFactory.CreateDataParameter("Remark", "", 250, ParameterDirection.Output);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);

            List<IDataParameter> dataParams = new List<IDataParameter>() { loginNameDataParam, doorNameDataParam, startTimeDataParam, endTimeDataParam, noteDataParam, resultDataParam, remarkDataParam, returnValueDataParam };
            ProcedureAdapter.ExecuteProcedure("spManualDoorAuthorization", dataParams);
            errorMessage = (remarkDataParam.Value != null ? remarkDataParam.Value.ToString() : "");
            return (resultDataParam.Value == null || resultDataParam.Value.ToString() == "0") ? false : true;
        }
        public bool AutomaticDoorAuthorization(out string errorMessage)
        {
            errorMessage = "";
            var resultDataParam = DataParameterFactory.CreateDataParameter("Result", 0, ParameterDirection.Output);
            var remarkDataParam = DataParameterFactory.CreateDataParameter("Remark", "", 250, ParameterDirection.Output);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);

            List<IDataParameter> dataParams = new List<IDataParameter>() { resultDataParam, remarkDataParam, returnValueDataParam };
            ProcedureAdapter.ExecuteProcedure("spAutomaticDoorAuthorization", dataParams);
            errorMessage = (remarkDataParam.Value != null ? remarkDataParam.Value.ToString() : "");
            return (resultDataParam.Value == null || resultDataParam.Value.ToString() == "0") ? false : true;
        }
    }
}