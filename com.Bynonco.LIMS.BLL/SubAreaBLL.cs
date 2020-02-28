using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaBLL : BLLBase<SubArea>, ISubAreaBLL
    {
        public IList<SubArea> GetSubAreasByCondition(string ownerName, string subAreaCategeryName, string subAreaName, string subAreaCategeryId)
        {
            try
            {
                string sqlStr = string.Format(@"SELECT SubArea.*
                                                FROM SubArea 
                                                INNER JOIN  SubAreaCategory ON SubAreaCategory.SubAreaCategoryId = SubArea.SubAreaCategoryId
                                                INNER JOIN [User] ON  [User].UserId = SubArea.OwnerId
                                                WHERE [User].Name LIKE '%{0}%' 
                                                AND SubArea.Name LIKE '%{1}%'
                                                AND SubAreaCategory.Name LIKE '%{2}%'  AND SubArea.SubAreaCategoryId='{3}'", ownerName == null ? "" : ownerName, subAreaName == null ? "" : subAreaName, subAreaCategeryName == null ? "" : subAreaCategeryName, subAreaCategeryId);
                return GetEntitiesBySql(sqlStr, true, true);
            }
            catch(Exception ex){return null; }
        }

        public double GetTotalSizeBySubAreaId(Guid subAreaId)
        {
            try
            {
                return Convert.ToDouble(GetSingleResult(string.Format(@"SELECT sum(SubAreaFile.FileSize) 
                                                        FROM SubAreaFile 
                                                        WHERE SubAreaId='{0}'", subAreaId.ToString())));
            }
            catch (Exception ex) { }
            return 0d; ;
        }
        public IList<SubArea> GetSubAreasByUserId(Guid userId)
        {
            try
            {
                string sqllStr = string.Format(@"SELECT SubArea.*
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
                return GetEntitiesBySql(sqllStr, true, true);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "GetSubAreasByUserId", ex.Message));
                return null;
            }
        }

        public IList<SubArea> GetSubAreasByUserId(Guid userId, Guid subAreaCategoryId)
        {
            try
            {
                string sqllStr = string.Format(@"SELECT SubArea.*
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
                                                AND SubArea.SubAreaCategoryId='{1}' AND SubArea.IsStop=0", userId.ToString(), subAreaCategoryId.ToString());
                return GetEntitiesBySql(sqllStr, true);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "GetSubAreasByUserId", ex.Message));
                return null;
            }
        }
    }
}
