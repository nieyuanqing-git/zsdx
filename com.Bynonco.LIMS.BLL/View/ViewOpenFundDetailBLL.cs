using System;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using System.Collections.Generic;

namespace com.Bynonco.LIMS.BLL.View
{
    /// <summary> 开放基金设备明细业务逻辑 </summary>
    public class ViewOpenFundDetailBLL : BLLBase<ViewOpenFundDetail>, IViewOpenFundDetailBLL
    {
        /// <summary> 构建基础查询条件 </summary>
        /// <param name="equipmentIds">设备</param>
        /// <param name="payerId">付款人</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        private string GenerateBasicQueryExpression(IEnumerable<Guid> equipmentIds, Guid payerId, DateTime endTime)
        {
            var queryExpression = string.Format("IsOpenFundApplyEquipmentDelete=false*IsOpenFundApplyDelete=false*Status={0}*UserId=\"{1}\"*ValidTime>\"{2}\"", (int)OpenFundApplyStatus.AuditPass, payerId, endTime);
            if (equipmentIds != null)
                queryExpression = queryExpression + "*" + equipmentIds.ToFormatStr("EquipmentId");
            return queryExpression;
        }
        /// <summary> 获取开放基金设备明细 </summary>
        /// <param name="equipmentId"></param>
        /// <param name="payerId"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IList<ViewOpenFundDetail> GetEquipmentViewOpenFundDetail(Guid equipmentId, Guid payerId, DateTime endTime)
        {
            return GetEquipmentViewOpenFundDetail(new Guid[] { equipmentId }, payerId, endTime);
        }
        public IList<ViewOpenFundDetail> GetEquipmentViewOpenFundDetail(IEnumerable<Guid> equipmentIds, Guid payerId, DateTime endTime)
        {
            var queryExpression = GenerateBasicQueryExpression(equipmentIds, payerId, endTime);
            return GetEntitiesByExpression(queryExpression, null, "ValidTime A");
        }
        public int GetCountEquipmentViewOpenFundDetail(IEnumerable<Guid> equipmentIds, Guid payerId, DateTime endTime)
        {
            var queryExpression = GenerateBasicQueryExpression(equipmentIds, payerId, endTime);
            return CountModelListByExpression(queryExpression);
        }
    }
}
