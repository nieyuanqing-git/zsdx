using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.DAL;
using System.Data;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewUsedConfirmUseConditionBLL : BLLBase<ViewUsedConfirmUseCondition>, IViewUsedConfirmUseConditionBLL
    {
        public override IList<ViewUsedConfirmUseCondition> GetEntitiesByExpression(JqueryEasyUI.Core.DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (mapping == null)
                mapping = new Dictionary<string, string>();
            mapping["Id"] = "UsedConfirmId";

            IList<ViewUsedConfirmUseCondition> lstViewUsedConfirmUseCondition = new List<ViewUsedConfirmUseCondition>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetUsedConfirmUseConditionList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    ViewUsedConfirmUseCondition viewUsedConfirmUseCondition = new ViewUsedConfirmUseCondition();
                    viewUsedConfirmUseCondition.Id = new Guid(result[0].ToString());
                    if (result[1] != null && !string.IsNullOrWhiteSpace(result[1].ToString()))
                        viewUsedConfirmUseCondition.CostDeductId = new Guid(result[1].ToString());
                    if (result[2] != null && !string.IsNullOrWhiteSpace(result[2].ToString()))
                        viewUsedConfirmUseCondition.DeductAt = DateTime.Parse(result[2].ToString());
                    if (result[3] != null && !string.IsNullOrWhiteSpace(result[3].ToString()))
                        viewUsedConfirmUseCondition.HasClossingAccount = bool.Parse(result[3].ToString());
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString()))
                        viewUsedConfirmUseCondition.VirtualCurrency = double.Parse(result[4].ToString());
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString()))
                        viewUsedConfirmUseCondition.RealCurrency = double.Parse(result[5].ToString());
                    if (result[6] != null && !string.IsNullOrWhiteSpace(result[6].ToString()))
                        viewUsedConfirmUseCondition.TotalCurrency = double.Parse(result[6].ToString());
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString()))
                        viewUsedConfirmUseCondition.CostDeductType = int.Parse(result[7].ToString());
                    if (result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString()))
                        viewUsedConfirmUseCondition.PayerId = new Guid(result[8].ToString());
                    viewUsedConfirmUseCondition.PayerName = result[9] != null ? result[9].ToString() : "";
                    if (result[10] != null && !string.IsNullOrWhiteSpace(result[10].ToString()))
                        viewUsedConfirmUseCondition.EquipmentId = new Guid(result[10].ToString());
                    viewUsedConfirmUseCondition.EquipmentName = result[11] != null ? result[11].ToString() : "";
                    if (result[12] != null && !string.IsNullOrWhiteSpace(result[12].ToString()))
                        viewUsedConfirmUseCondition.EquipmentOrgId = new Guid(result[12].ToString());
                    viewUsedConfirmUseCondition.EquipmentOrgName = result[13] != null ? result[13].ToString() : "";
                    viewUsedConfirmUseCondition.LabRoomName = result[14] != null ? result[14].ToString() : "";
                    viewUsedConfirmUseCondition.LabRoomXPath = result[15] != null ? result[15].ToString() : "";
                    if (result[16] != null && !string.IsNullOrWhiteSpace(result[16].ToString()))
                        viewUsedConfirmUseCondition.SubjectId = new Guid(result[16].ToString());
                    viewUsedConfirmUseCondition.SubjectName = result[17] != null ? result[17].ToString() : "";
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString()))
                        viewUsedConfirmUseCondition.SubjectDirectorId = new Guid(result[18].ToString());
                    if (result[19] != null && !string.IsNullOrWhiteSpace(result[19].ToString()))
                        viewUsedConfirmUseCondition.PaymentMethod = int.Parse(result[19].ToString());
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString()))
                        viewUsedConfirmUseCondition.UserId = new Guid(result[20].ToString());
                    viewUsedConfirmUseCondition.UserName = result[21] != null ? result[21].ToString() : "";
                    if (result[22] != null && !string.IsNullOrWhiteSpace(result[22].ToString()))
                        viewUsedConfirmUseCondition.RealFee = double.Parse(result[22].ToString());
                    if (result[23] != null && !string.IsNullOrWhiteSpace(result[23].ToString()))
                        viewUsedConfirmUseCondition.CalcFee = double.Parse(result[23].ToString());
                    if (result[24] != null && !string.IsNullOrWhiteSpace(result[24].ToString()))
                        viewUsedConfirmUseCondition.BeginAt = DateTime.Parse(result[24].ToString());
                    if (result[25] != null && !string.IsNullOrWhiteSpace(result[25].ToString()))
                        viewUsedConfirmUseCondition.EndAt = DateTime.Parse(result[25].ToString());
                    if (result[26] != null && !string.IsNullOrWhiteSpace(result[26].ToString()))
                        viewUsedConfirmUseCondition.EquipmentUseConditionId = new Guid(result[26].ToString());
                    viewUsedConfirmUseCondition.EquipmentUseConditionName = result[27] != null ? result[27].ToString() : "";
                    viewUsedConfirmUseCondition.Val = result[28] != null ? result[28].ToString() : "";
                    viewUsedConfirmUseCondition.BeforeUseStatus = (int)UsedConfirmFeedBackStatus.NoFeedBack;
                    if (result[29] != null && !string.IsNullOrWhiteSpace(result[29].ToString()))
                        viewUsedConfirmUseCondition.BeforeUseStatus = int.Parse(result[29].ToString());
                    viewUsedConfirmUseCondition.FinishUseStatus = (int)UsedConfirmFeedBackStatus.NoFeedBack;
                    if (result[30] != null && !string.IsNullOrWhiteSpace(result[30].ToString()))
                        viewUsedConfirmUseCondition.FinishUseStatus = int.Parse(result[30].ToString());
                    viewUsedConfirmUseCondition.SampleNo = result[31] != null ? result[31].ToString() : "";

                    lstViewUsedConfirmUseCondition.Add(viewUsedConfirmUseCondition);
                }
            }
            return lstViewUsedConfirmUseCondition;
        }
    }
}
