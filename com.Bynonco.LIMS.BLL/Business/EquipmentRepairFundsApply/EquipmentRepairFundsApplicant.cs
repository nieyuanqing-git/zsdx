using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentRepairFundsApplicant : EquipmentRepairFundsApplyBase
    {
        public EquipmentRepairFundsApplicant(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentRepairFundsApply> GetMyGridViewEquipmentRepairFundsApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("ApplicantId=\"{0}\"", _operatorId));
            return _viewEquipmentRepairFundsApplyBLL.GetGridViewEquipmentRepairFundsApplyListByExpression(_operatorId, dataGridSettings, _mapping, dataGridSettings.GetOrderExpression(), true, false, true, _outMapping);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override bool AuditPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        public override bool JudgeIsEnableAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override bool AuditNoPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "无操作权限";
            return false;
        }

        protected override EquipmentRepairFundsApply GenerateAuditInfo(EquipmentRepairFundsApply equipmentRepairFundsApply, Model.Enum.EquipmentRepairFundsApplyStatus equipmentRepairFundsApplyStatus, EquipmentRepairFundsApplyAuditContext auditContext)
        {
            return equipmentRepairFundsApply;
        }


        protected override void OperateDecorate(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply) 
        {
            viewEquipmentRepairFundsApply.LabRoomAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.LabRoomAuditPassOperate = "";
            viewEquipmentRepairFundsApply.CollegeAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.CollegeAuditPassOperate = "";
            viewEquipmentRepairFundsApply.EquipmentDeptAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.EquipmentDeptAuditPassOperate = "";
            viewEquipmentRepairFundsApply.ShareEAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.ShareEAuditPassOperate = "";
            viewEquipmentRepairFundsApply.SchoolAuditPassOperate = "";
            viewEquipmentRepairFundsApply.SchoolAuditNoPassOperate = "";
        }
    }
}
