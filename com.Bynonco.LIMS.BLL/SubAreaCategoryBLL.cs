using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaCategoryBLL : BLLBase<SubAreaCategory>, ISubAreaCategoryBLL
    {
        public IList<SubAreaCategory> GetSubAreaCategoriesByUserId(Guid userId)
        {
            try
            {
                string sqllStr = string.Format(@"SELECT SubAreaCategory.*
                                                FROM SubAreaCategory 
                                                INNER JOIN SubArea ON SubAreaCategory.SubAreaCategoryId=SubArea.SubAreaCategoryId
                                                WHERE 
                                                (
	                                                SubArea.SubAreaId IN 
	                                                (
		                                                SELECT SubAreaTagPower.SubAreaId FROM SubAreaTagPower 
		                                                WHERE SubAreaTagPower.TagId=(SELECT TOP 1 [User].TagId FROM [User] WHERE [User].UserId='{0}')
                                                        AND SubAreaTagPower.IsStop=0
	                                                )
	                                                OR
	                                                SubArea.SubAreaId  IN 
	                                                (
		                                                SELECT SubAreaUerPower.SubAreaId FROM SubAreaUerPower 
		                                                WHERE SubAreaUerPower.UserId=(SELECT TOP 1 [User].UserId FROM [User] WHERE [User].UserId='{0}')
                                                        AND SubAreaUerPower.IsStop=0
	                                                )
                                                )
                                                AND 
                                                SubArea.SubAreaId NOT IN 
                                                (
	                                                SELECT SubAreaUserWithoutPower.SubAreaId FROM SubAreaUserWithoutPower 
	                                                WHERE UserId=(SELECT TOP 1 [User].UserId FROM [User] WHERE [User].UserId='{0}')
                                                    AND SubAreaUserWithoutPower.IsStop=0
                                                )
                                                AND SubArea.IsStop=0
                                                ", userId.ToString());
                var subAreaCategories =  GetEntitiesBySql(sqllStr, true,false).Distinct().ToList();
                foreach (var subAreaCategory in subAreaCategories)
                {
                    subAreaCategory.SetOwnerId(userId);
                    subAreaCategory.LoadReference();
                }
                return subAreaCategories;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "GetSubAreaCategoriesByUserId", ex.Message));
                return null;
            }
        }
    }
}
