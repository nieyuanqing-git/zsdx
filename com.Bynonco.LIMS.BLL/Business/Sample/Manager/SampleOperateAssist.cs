using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public partial class SampleManager
    {
        private static bool? __isEnbaleEditOperate = null;
        private bool JudgeIsEnableOperate(int status)
        {
            if (__isEnbaleEditOperate != null) return __isEnbaleEditOperate.Value;
            var __dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=WhichSampleStausDisableEdit");
            if (__dictCodeTypes != null && __dictCodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(__dictCodeTypes.First().Value))
            {
                var statuses = __dictCodeTypes.First().Value.Trim().Replace("，", ",").Split(',');
                return !statuses.Contains(status.ToString());
            }
            return true;
        }
        /// <summary>
        /// 对申请单的编辑情况进行撤销，只保存申请单状态信息
        /// </summary>
        /// <param name="sampleApplyId"></param>
        /// <param name="sampleStatus"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private SampleApply RejectChange(Guid sampleApplyId,int sampleStatus,out string errorMessage)
        {
            errorMessage = "";
            try
            {
                var sampleApplies = _sampleApplyBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", sampleApplyId.ToString()));
                if (sampleApplies != null && sampleApplies.Count > 0)
                {
                    var sampleApplyTemp = sampleApplies.First();
                    sampleApplyTemp.EditId = sampleApplyTemp.Id;
                    sampleApplyTemp.Status = sampleStatus;
                    sampleApplyTemp.LoadReference();
                    return sampleApplyTemp;
                }
                throw new Exception(string.Format("出错,申请单信息编辑撤销失败,找不到编码为【{0}】的申请单信息", sampleApplyId.ToString()));
            }
            catch (Exception ex)
            {
                errorMessage = "出错," + ex.Message;
                return null;
            }
        }
    }
}
