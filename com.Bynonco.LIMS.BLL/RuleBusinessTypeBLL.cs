using System;
using com.august.Core.XQuery;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using System.Linq;

namespace com.Bynonco.LIMS.BLL
{
    public class RuleBusinessTypeBLL
    {
        public bool IsUserInBusiness(User user, ICollectinData businessObj)
        {
            if (businessObj is User)
            {
                return businessObj.id == user.Id;
            }
            if (businessObj is Subject)
            {
                var subjects = user.GetRelevantSubjects();
                if (subjects == null)
                {
                    return false;
                }
                return subjects.Any(x => x.Id == businessObj.id);
            }
            if (businessObj is LabOrganization)
            {
                if (user.Organization == null)
                {
                    return false;
                }
                return user.Organization.XPath.StartsWith(((LabOrganization)businessObj).XPath);
            }
            if (businessObj is UserType)
            {
                if (user.UserType == null)
                {
                    return false;
                }
                return user.UserTypeId == businessObj.id;
            }
            if (businessObj is Tag)
            {
                if (user.Tag == null)
                {
                    return false;
                }
                return user.TagId == businessObj.id;
            }
            if (businessObj is WorkGroup)
            {
                if (user.WorkGroups == null)
                {
                    return false;
                }
                return user.WorkGroups.Any(x => x.Id == businessObj.id);
            }
            return false;
        }
    }
}