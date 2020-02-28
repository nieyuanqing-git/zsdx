using System;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserSystemSettingBLL : BLLBase<ViewUserSystemSetting>, IViewUserSystemSettingBLL
    {
        /// <summary> 通过用户ID获取实体 </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ViewUserSystemSetting GetEntityByUserId(Guid userId)
        {
            var entities = GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId), null, "", true);
            if (entities == null || entities.Count == 0) return null;
            return entities[0];
        }
        /// <summary> 判断是否包含超级管理员字符的角色 </summary>
        /// <param name="setting">实例</param>
        public bool IsSuperAdminWorkGroup(ViewUserSystemSetting setting)
        {
            if (string.IsNullOrEmpty(setting.WorkGroupName))
            {
                var user = BLLFactory.CreateUserBLL().GetEntityById(setting.UserId);
                return user.IsSuperAdmin;
            }
            return setting.WorkGroupName.IndexOf("超级管理员") != -1;
        }
        /// <summary> 判断是否包含管理员字符的角色 </summary>
        /// <param name="setting">实例</param>
        public bool IsAnyAdminWorkGroup(ViewUserSystemSetting setting)
        {
            if (string.IsNullOrEmpty(setting.WorkGroupName))
            {
                var user = BLLFactory.CreateUserBLL().GetEntityById(setting.UserId);
                return user.IsAnyAdmin;
            }
            return setting.WorkGroupName.IndexOf("管理员") != -1;
        }

    }
}
