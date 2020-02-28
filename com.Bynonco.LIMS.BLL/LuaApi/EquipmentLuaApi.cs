using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using NUnit.Framework;

namespace com.Bynonco.LIMS.BLL.LuaApi
{
    public class EquipmentLuaApi
    {
        private IEquipmentLabelBLL __equipmentLabelBLL;
        private IEquipmentBLL __equipmentBLL;
        private IAppointmentEquipmentUseConditionBLL __appointmentEquipmentUseConditionBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        public EquipmentLuaApi()
        {
            __equipmentLabelBLL = BLLFactory.CreateEquipmentLabelBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __appointmentEquipmentUseConditionBLL = BLLFactory.CreateAppointmentEquipmentUseConditionBLL();
        }
        [LuaFunction("IsCurUserBelongToTheLabelName")]
        public bool IsCurUserBelongToTheLabelName(string equipmentId, string userId,string labelName)
        {
            Guid? userIdTemp = null;
            if (!string.IsNullOrWhiteSpace(userId)) userIdTemp = new Guid(userId);
            return __equipmentLabelBLL.JudgeIsExistEquipmentLabelName(new Guid(equipmentId), userIdTemp, labelName);
        }
        [LuaFunction("IsAppointmentAsVirtualSpace")]
        public bool IsAppointmentAsVirtualSpace(string equipmentId)
        {
            var equipment = __equipmentBLL.GetEntityById(new Guid(equipmentId));
            return equipment.IsAppointmentAsVirtualSpace;
        }
        [LuaFunction("IsCurUserEquipmentAdmin")]
        public bool IsCurUserEquipmentAdmin(string equipmentId, string userId)
        {
             return __userWorkScopeBLL.JudgeIsEquipmentAdmin(new Guid(equipmentId),new Guid(userId));
        }
        [Test]
        public void DoTestIsCurUserBelongToTheLabelName()
        {
            LazyLoadSetUp.LazyLoadSetUp.SetUp();
            var result = IsCurUserBelongToTheLabelName("EC1A3E2A-1E9B-40D7-9713-A7D6C123C7DA", "3A4BE5D7-6E95-4A1E-BAD2-29DBCDC9D8BF", "管理员所在用户类型");
            Assert.IsTrue(result);
        }
    }
}
