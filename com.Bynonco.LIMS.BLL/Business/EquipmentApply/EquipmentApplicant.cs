using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentApplicant : EquipmentApplyBase
    {
        public EquipmentApplicant(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentApply> GetMyGridViewEquipmentApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("ApplicantId=\"{0}\"", _operatorId));
            return _viewEquipmentApplyBLL.GetGridViewEquipmentApplyListByExpression(_operatorId,dataGridSettings);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override bool AuditPass(EquipmentApply equipmentApply, string remark, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        public override bool JudgeIsEnableAuditNoPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override bool AuditNoPass(EquipmentApply equipmentApply, string remark, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override EquipmentApply GenerateAuditInfo(EquipmentApply equipmentApply, Model.Enum.EquipmentApplyStatus equipmentApplyStatus, string remark)
        {
            return equipmentApply;
        }


        protected override void OperateDecorate(ViewEquipmentApply viewEquipmentApply) { }
    }
}
