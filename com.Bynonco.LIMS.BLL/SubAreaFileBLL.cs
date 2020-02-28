using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Base;

namespace com.Bynonco.LIMS.BLL
{
    public class SubAreaFileBLL : BLLBase<SubAreaFile>, ISubAreaFileBLL
    {
        public int GetMaxRowNo(SubAreaFile entity)
        {

            int maxRowNo = 1;
            try
            {
                if (entity.Parent != null)
                    maxRowNo = entity.Parent.Childrens.Count + 1;
                else
                    maxRowNo = BLLFactory.CreateSubAreaFileBLL().GetEntitiesByExpression(string.Format("SubAreaId=\"{0}\"*ParentId=\"{1}\"", entity.SubAreaId, (new Guid()).ToString())).Count + 1;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "GetMaxRowNo", ex.Message));
                return maxRowNo;
            }
            return maxRowNo;
        }
        /// <summary>
        /// 获取文件所有子文件容量之和
        /// </summary>
        /// <param name="subAreaId"></param>
        /// <returns></returns>
        public double GetChildrenTotalSize(Guid subAreaId)
        {
            try
            {
                object result = GetSingleResult(string.Format(@"SELECT SUM(SubAreaFile.FileSize) FROM SubAreaFile WHERE ParentId='{0}'", subAreaId.ToString()));
                return result == null ? 0d : Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}:{1}", "GetMaxRowNo", ex.Message));
                return 0d;
            }
        }
    }
}
