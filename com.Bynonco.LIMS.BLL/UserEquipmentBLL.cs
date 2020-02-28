using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class UserEquipmentBLL : BLLBase<UserEquipment>, IUserEquipmentBLL
    {
        private IDictCodeBLL __dictCodeBLL;
        public UserEquipmentBLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public object GetUserEquipmentPriviliges(Guid? userId, IList<ViewUserEquipment> viewUserEquipmentList)
        {
            IList<com.Bynonco.LIMS.BLL.Business.Privilige.UserEquipmentPrivilige> lstUserEquipmentPriviliges = new List<com.Bynonco.LIMS.BLL.Business.Privilige.UserEquipmentPrivilige>();
            if (userId.HasValue && viewUserEquipmentList != null && viewUserEquipmentList.Count > 0)
            {
                foreach (ViewUserEquipment viewUserEquipment in viewUserEquipmentList)
                {
                    var userEquipmentPrivilige = lstUserEquipmentPriviliges.FirstOrDefault(p => p.UserEquipmentId.HasValue && p.UserEquipmentId.Value == viewUserEquipment.Id);
                    if (userEquipmentPrivilige == null)
                    {
                        userEquipmentPrivilige = PriviligeFactory.GetUserEquipmentPrivilige(userId.Value, viewUserEquipment.Id);
                        lstUserEquipmentPriviliges.Add(userEquipmentPrivilige);
                    }
                }
            }
            return lstUserEquipmentPriviliges;
        }
        public UserEquipment GetUserEquipmentInfo(Guid userId, Guid equipmentId)
        {
            var userEquipmentList = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"", userId.ToString(), equipmentId.ToString()));
            return userEquipmentList != null && userEquipmentList.Count > 0 ? userEquipmentList.First() : null;
        }
        public bool Validates(Guid equipmentId, Guid? userId, out string errorMessage)
        {
            errorMessage = "";
            if (!userId.HasValue) return true;
            var dictCode = __dictCodeBLL.GetDictCodeByCode("SelectEquipment", "SelectEquipmentMethod");
            if (dictCode == null || string.IsNullOrWhiteSpace(dictCode.Value) || dictCode.Value.Trim() != "1")
            {
                var count = (int)GetSingleResult(string.Format("SELECT count(*) FROM UserEquipment WHERE UserId='{0}' AND EquipmentId='{1}' AND Status=1", userId.Value.ToString(), equipmentId.ToString()));
                if (count == 0)
                {
                    errorMessage = "无权限";
                    return false;
                }
            }
            errorMessage = "有权限";
            return true;
        }
    }
}