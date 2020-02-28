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
    public class DoorBLL : BLLBase<Door>, IDoorBLL
    {
        private IUserDoorAuthorizationBLL __userDoorAuthorizationBLL;
        private IUserDoorContinuedAuthorizationBLL __userDoorContinuedAuthorizationBLL;
        private IEquipmentAuthorizationAndAppointmentBLL __equipmentAuthorizationAndAppointmentBLL;
        public DoorBLL()
        {
            __userDoorAuthorizationBLL = BLLFactory.CreateUserDoorAuthorizationBLL();
            __userDoorContinuedAuthorizationBLL = BLLFactory.CreateUserDoorContinuedAuthorizationBLL();
            __equipmentAuthorizationAndAppointmentBLL = BLLFactory.CreateEquipmentAuthorizationAndAppointmentBLL();
        }

        public bool DeleteDoor(Door door)
        {
            if (door == null) return false;
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var userDoorAuthorizationList = __userDoorAuthorizationBLL.GetEntitiesByExpression(string.Format("DoorID=\"{0}\"", door.Id));
                if (userDoorAuthorizationList != null && userDoorAuthorizationList.Count > 0)
                {
                    var equipmentAuthorizationAndAppointmentList = __equipmentAuthorizationAndAppointmentBLL.GetEntitiesByExpression(string.Format("DoorID=\"{0}\"", door.Id));
                    if (equipmentAuthorizationAndAppointmentList != null && equipmentAuthorizationAndAppointmentList.Count() > 0)
                    {
                        foreach (var item in equipmentAuthorizationAndAppointmentList)
                        {
                            item.Updated = 0;
                            item.IsDel = 1;
                            item.LeamsUpdated = false;
                        }
                        __equipmentAuthorizationAndAppointmentBLL.Save(equipmentAuthorizationAndAppointmentList, ref tran, true);
                    }
                    __userDoorAuthorizationBLL.Delete(userDoorAuthorizationList.Select(m => m.id), ref tran, true);
                }
                var userDoorContinuedAuthorizationList = __userDoorContinuedAuthorizationBLL.GetEntitiesByExpression(string.Format("DoorID=\"{0}\"", door.Id));
                if (userDoorContinuedAuthorizationList != null && userDoorContinuedAuthorizationList.Count > 0)
                    __userDoorContinuedAuthorizationBLL.Delete(userDoorContinuedAuthorizationList.Select(m => m.id), ref tran, true);
                door.IsDelete = true;
                door.IP = "";
                door.LeamsUpdated = false;
                Save(new Door[] { door }, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                return false;
            }
            finally { tran.Dispose(); }
            return true;
        }
    }
}