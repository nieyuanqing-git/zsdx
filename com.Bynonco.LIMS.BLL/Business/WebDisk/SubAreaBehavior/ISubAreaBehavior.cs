using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL.SubAreaBehavior
{
    public interface ISubAreaBehavior
    {
        double GetTotalSizeBySubAreaId(Guid subAreaId);
        IList<SubArea> GetSubAreasByUserId(Guid userId);
        IList<SubArea> GetSubAreasByUserId(Guid userId, Guid subAreaCategoryId);
        IList<SubArea> GetSubAreasByCondition(string ownerName, string subAreaCategeryName, string subAreaName, string subAreaCategeryId);
        bool Add(IEnumerable<SubArea> entities, ref XTransaction tran, bool isSupress = false);
        bool Save(IEnumerable<SubArea> entities, ref XTransaction tran, bool isSupress = false);
        bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false);
    }
}
