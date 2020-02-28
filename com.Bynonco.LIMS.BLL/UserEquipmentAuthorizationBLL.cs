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
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class UserEquipmentAuthorizationBLL : BLLBase<UserEquipmentAuthorization>, IUserEquipmentAuthorizationBLL
    {
        private IEquipmentAuthorizationAndAppointmentBLL __equipmentAuthorizationAndAppointmentBLL;
        private IDoorOfflineAuthorizationBLL __doorOfflineAuthorizationBLL;
        private IViewDoorEquipmentBLL __viewDoorEquipmentBLL;
        private ISystemLogBLL __systemLogBLL;
        public UserEquipmentAuthorizationBLL()
        {
            __equipmentAuthorizationAndAppointmentBLL = BLLFactory.CreateEquipmentAuthorizationAndAppointmentBLL();
            __doorOfflineAuthorizationBLL = BLLFactory.CreateDoorOfflineAuthorizationBLL();
            __viewDoorEquipmentBLL = BLLFactory.CreateViewDoorEquipmentBLL();
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
        }

        public bool SaveUserEquipmentAuthorization(IList<UserEquipmentAuthorization> userEquipmentAuthorizationList, string ip, out string errorMessage)
        {
            errorMessage = "";
            if (userEquipmentAuthorizationList == null || userEquipmentAuthorizationList.Count() == 0)
            {
                errorMessage = "出错, 操作记录为空.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var viewDoorEquipmentList = __viewDoorEquipmentBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", userEquipmentAuthorizationList.Select(p => p.EquipmentID.Value).ToFormatStr("EquipmentId")));

                foreach (var item in userEquipmentAuthorizationList)
                {
                    var originalEntity = GetEntityById(item.Id);
                    if (originalEntity == null)
                    {
                        Add(new UserEquipmentAuthorization[] { item }, ref tran, true);
                        GenerateEntityLog(OperateType.New, originalEntity, item, ip, ref tran, true);
                    }
                    else
                    {
                        Save(new UserEquipmentAuthorization[] { item }, ref tran, true);
                        GenerateEntityLog(OperateType.Modified, originalEntity, item, ip, ref tran, true);
                    }
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
                        equipmentAuthorizationAndAppointment.EquipmentID = item.EquipmentID;
                        equipmentAuthorizationAndAppointment.UserID = item.UserID;
                        equipmentAuthorizationAndAppointment.StartDate = item.StartDate;
                        equipmentAuthorizationAndAppointment.EndDate = item.EndDate;
                        equipmentAuthorizationAndAppointment.IsDel = 0;
                        equipmentAuthorizationAndAppointment.Updated = 0;
                        equipmentAuthorizationAndAppointment.LeamsUpdated = false;
                        __equipmentAuthorizationAndAppointmentBLL.Add(new EquipmentAuthorizationAndAppointment[] { equipmentAuthorizationAndAppointment }, ref tran, true);
                    }
                    var doorOfflineAuthorizationList = __doorOfflineAuthorizationBLL.GetEntitiesByExpression(string.Format("AuthorizationId=\"{0}\"", item.Id));
                    if (doorOfflineAuthorizationList != null && doorOfflineAuthorizationList.Count() > 0)
                    {
                        foreach (var doorOfflineItem in doorOfflineAuthorizationList)
                        {
                            doorOfflineItem.IsDel = true;
                            doorOfflineItem.Updated = false;
                            doorOfflineItem.LeamsUpdated = false;
                        }
                    }
                    var itemDoorEquipmentList = viewDoorEquipmentList == null ? null : viewDoorEquipmentList.Where(p => p.EquipmentId == item.EquipmentID.Value);
                    if (itemDoorEquipmentList != null && itemDoorEquipmentList.Count() > 0)
                    {
                        foreach (var doorItem in itemDoorEquipmentList)
                        {
                            DoorOfflineAuthorization doorOfflineItem = null;
                            bool isNewOfflineItem = false;
                            var oldOfflineList = doorOfflineAuthorizationList == null ? null : doorOfflineAuthorizationList.Where(p => p.DoorId == doorItem.DoorID);
                            if (oldOfflineList != null && oldOfflineList.Count() > 0) doorOfflineItem = oldOfflineList.First();
                            else
                            {
                                doorOfflineItem = new DoorOfflineAuthorization();
                                doorOfflineItem.Id = Guid.NewGuid();
                                isNewOfflineItem = true;
                            }
                            doorOfflineItem.DoorId = doorItem.DoorID;
                            doorOfflineItem.UserId = item.UserID.Value;
                            doorOfflineItem.StartDate = item.StartDate.Value;
                            doorOfflineItem.EndDate = item.EndDate.Value;
                            doorOfflineItem.Updated = false;
                            doorOfflineItem.IsDel = false;
                            doorOfflineItem.LeamsUpdated = false;
                            doorOfflineItem.EquipmentId = item.EquipmentID.Value;
                            doorOfflineItem.AuthorizationId = item.Id;
                            if (isNewOfflineItem) __doorOfflineAuthorizationBLL.Add(new DoorOfflineAuthorization[] { doorOfflineItem }, ref tran, true);
                        }
                    }
                    if (doorOfflineAuthorizationList != null && doorOfflineAuthorizationList.Count() > 0)
                        __doorOfflineAuthorizationBLL.Save(doorOfflineAuthorizationList, ref tran, true);
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
        public bool DeleteUserEquipmentAuthorization(IList<UserEquipmentAuthorization> userEquipmentAuthorizationList, string ip, out string errorMessage)
        {
            errorMessage = "";
            if (userEquipmentAuthorizationList == null || userEquipmentAuthorizationList.Count() == 0)
            {
                errorMessage = "出错, 操作记录为空.";
                return false;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                foreach (var userEquipmentAuthorization in userEquipmentAuthorizationList)
                {
                    Delete(new Guid[] { userEquipmentAuthorization.Id }, ref tran, true);
                    GenerateEntityLog(OperateType.Deleted, userEquipmentAuthorization, userEquipmentAuthorization, ip, ref tran, true);
                    var equipmentAuthorizationAndAppointment = __equipmentAuthorizationAndAppointmentBLL.GetEntityById(userEquipmentAuthorization.Id);
                    if (equipmentAuthorizationAndAppointment != null)
                    {
                        equipmentAuthorizationAndAppointment.IsDel = 1;
                        equipmentAuthorizationAndAppointment.Updated = 0;
                        equipmentAuthorizationAndAppointment.LeamsUpdated = false;
                        __equipmentAuthorizationAndAppointmentBLL.Save(new EquipmentAuthorizationAndAppointment[] { equipmentAuthorizationAndAppointment }, ref tran, true);
                    }
                    var doorOfflineAuthorizationList = __doorOfflineAuthorizationBLL.GetEntitiesByExpression(string.Format("AuthorizationId=\"{0}\"", userEquipmentAuthorization.Id));
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
        public void GenerateEntityLog(OperateType operateType, UserEquipmentAuthorization originalEntity, UserEquipmentAuthorization curEntity, string operateIP, ref XTransaction tran, bool isSupress)
        {
            string title = "", content = "", contentHTML = "", entityName = "", entityCnName = "";
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
        public void GenerateEntityLogData(OperateType operateType, UserEquipmentAuthorization originalEntity, UserEquipmentAuthorization curEntity, out string title, out string content, out string contentHTML, out string entityName, out string entityCnName)
        {
            entityName = "UserEquipmentAuthorization";
            entityCnName = "设备授权";
            title = string.Format("{0}用户[{1}]仪器设备[{2}]权限.",
                operateType.ToCnName(),
                curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName,
                curEntity == null || curEntity.Equipment == null ? "" : curEntity.Equipment.Name);
            StringBuilder sbContnet = new StringBuilder();
            sbContnet.AppendFormat("编码:【{0}】", curEntity.Id).Append("\r\n");
            sbContnet.AppendFormat("用户:【{0}】", curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName).Append("\r\n");
            sbContnet.AppendFormat("设备:【{0}】", curEntity == null || curEntity.Equipment == null ? "" : curEntity.Equipment.Name).Append("\r\n");
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
    }
}