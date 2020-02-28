using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class UserEquipmentPriviligeBLL : BLLBase<UserEquipmentPrivilige>, IUserEquipmentPriviligeBLL
    {
        private IUserWorkGroupBLL __userWorkGroupBLL;
        private ISampleItemBLL __sampleItemBLL;
        public UserEquipmentPriviligeBLL()
        {
            __userWorkGroupBLL = BLLFactory.CreateUserWorkGroupBLL();
            __sampleItemBLL = BLLFactory.CreateSampleItemBLL();
        }

        public UserEquipmentPrivilige GetUserEquipmentPriviligeByUserId(Guid userid)
        {
            var userEquipmentPriviliges = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*IsStop=false*IsDelete=false", userid.ToString()));
            if (userEquipmentPriviliges != null && userEquipmentPriviliges.Count > 0)
            {
                var userEquipmentPrivilige = userEquipmentPriviliges.First();
                return userEquipmentPrivilige.Flag.HasValue && userEquipmentPrivilige.Flag .Value? new UserEquipmentPrivilige() : userEquipmentPrivilige;
            }
            var userWorkGroups = __userWorkGroupBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userid.ToString()));
            if (userWorkGroups != null && userWorkGroups.Count > 0)
            {
                userEquipmentPriviliges = GetEntitiesByExpression(string.Format("IsStop=false*IsDelete=false") + "*" + userWorkGroups.Select(p => p.WorkGroupId).ToFormatStr("WorkGroupId"));
                if (userEquipmentPriviliges != null && userEquipmentPriviliges.Count > 0)
                {
                    var userEquipmentPrivilige = userEquipmentPriviliges.First();
                    return userEquipmentPrivilige.Flag.HasValue && userEquipmentPrivilige.Flag.Value ? new UserEquipmentPrivilige() : userEquipmentPrivilige;
                }
            }
            return null;
        }
        public UserEquipmentPrivilige GetUserEquipmentPriviligeByUserItemId(Guid userid, Guid sampleItemId)
        {
            var sampleItems = __sampleItemBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", sampleItemId.ToString()));
            if (sampleItems == null || sampleItems.Count == 0) return null;
            var sampleItem = sampleItems.First();
            var priviliges = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*EquipmentId=\"{2}\"*SampleItemId=\"{1}\"*IsStop=false*IsDelete=false", userid.ToString(), sampleItemId.ToString(), sampleItem.EquipmentId.ToString()));

            if (priviliges != null && priviliges.Count > 0)
            {
                var privilige = priviliges.First();
                return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
            }
            priviliges = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"*WorkGroupId=null*SampleItemId=null*IsStop=false*IsDelete=false", userid.ToString(), sampleItem.EquipmentId.ToString()));
            if (priviliges != null && priviliges.Count > 0)
            {
                var privilige = priviliges.First();
                return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
            }
            priviliges = GetEntitiesByExpression(string.Format("UserId=\"{0}\"*EquipmentId=null*WorkGroupId=null*SampleItemId=null*IsStop=false*IsDelete=false", userid.ToString()));
            if (priviliges != null && priviliges.Count > 0)
            {
                var privilige = priviliges.First();
                return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
            }
            var userWorkGroups = __userWorkGroupBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userid.ToString()));
            if (userWorkGroups != null && userWorkGroups.Count > 0)
            {
                priviliges = GetEntitiesByExpression(string.Format("UserId=null*EquipmentId=\"{1}\"*SampleItemId=\"{0}\"*IsStop=false*IsDelete=false", sampleItemId.ToString(), sampleItem.EquipmentId.ToString()) + "*" + userWorkGroups.Select(p => p.WorkGroupId).ToFormatStr("WorkGroupId"));
                if (priviliges != null && priviliges.Count > 0)
                {
                    var privilige = priviliges.First();
                    return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
                }
                priviliges = GetEntitiesByExpression(string.Format("UserId=null*EquipmentId=\"{0}\"*SampleItemId=null*IsStop=false*IsDelete=false", sampleItem.EquipmentId.ToString()) + "*" + userWorkGroups.Select(p => p.WorkGroupId).ToFormatStr("WorkGroupId"));
                if (priviliges != null && priviliges.Count > 0)
                {
                    var privilige = priviliges.First();
                    return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
                }
                priviliges = GetEntitiesByExpression(string.Format("UserId=null*EquipmentId=\"{0}\"*SampleItemId=null*IsStop=false*IsDelete=false", sampleItem.EquipmentId.ToString()) + "*" + userWorkGroups.Select(p => p.WorkGroupId).ToFormatStr("WorkGroupId"));
                if (priviliges != null && priviliges.Count > 0)
                {
                    var privilige = priviliges.First();
                    return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
                }
                priviliges = GetEntitiesByExpression(string.Format("UserId=null*EquipmentId=null*SampleItemId=null*IsStop=false*IsDelete=false") + "*" + userWorkGroups.Select(p => p.WorkGroupId).ToFormatStr("WorkGroupId"));
                if (priviliges != null && priviliges.Count > 0)
                {
                    var privilige = priviliges.First();
                    return privilige.Flag.HasValue && privilige.Flag.Value ? new UserEquipmentPrivilige() : privilige;
                }
            }
            return null;
        }
    }
}