using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;
namespace com.Bynonco.LIMS.BLL
{
    public class UserTypeBLL : BLLBase<UserType>, IUserTypeBLL
    {
        public bool IsTutorUserType(Guid id)
        {
            bool isOk = false;
            var userType = GetEntityById(id);
            if (userType != null && userType.UserIdentity.HasValue)
                isOk = userType.UserIdentity == (int)UserIdentity.Tutor || userType.UserIdentity == (int)UserIdentity.OutTutor;
            return isOk;
        }
        public bool IsStudentUserType(Guid id)
        {
            bool isOk = false;
            var userType = GetEntityById(id);
            if (userType != null && userType.UserIdentity.HasValue)
                isOk = userType.UserIdentity == (int)UserIdentity.Student;
            return isOk;
        }
        public UserType GenerateDefaultUserType(string userTypeName, UserIdentity userIdentity, ref XTransaction tran)
        {
            IWorkGroupBLL workGroupBLL = BLLFactory.CreateWorkGroupBLL();
            var userType = GetFirstOrDefaultEntityByExpression(string.Format("IsDelete=false*UserIdentity={0}*Name=\"{1}\"", (int)userIdentity, userTypeName.Trim()));
            if (userType != null) return userType;
            Guid? parentId = null;
            var parentUserType = GetFirstOrDefaultEntityByExpression(string.Format("IsDelete=false*UserIdentity={0}*ParentId=null", (int)userIdentity), null, "XPath D");//优先“其他”节点
            if (parentUserType != null) parentId = parentUserType.Id;
            var workGroup = workGroupBLL.GenerateDefaultWorkGroup(userIdentity.ToCnName(), ref tran);
            userType = new UserType()
            {
                Id = Guid.NewGuid(),
                InputStr = ShortcutStringUtility.GetFirstPYLetter(userTypeName.Trim()),
                IsDelete = false,
                IsStop = false,
                Name = userTypeName.Trim(),
                ParentId = parentId,
                WorkGroupId = workGroup.Id,
                UserIdentity = (int)userIdentity,
                XPath = XPathUtility.GenerateXPath(null, parentId,
                   (entityId) => { return GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                   (parentEntityId) => { return GetEntitiesByExpression(string.Format("ParentId=\"{0}\"*IsDelete=false", parentEntityId.ToString())); },
                   () => { return GetEntitiesByExpression("ParentId=null*IsDelete=false"); })
            };
            Add(new UserType[] { userType }, ref tran, true);
            return userType;
        }
    }
}