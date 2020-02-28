using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentRepairFundsCollegeAudit : EquipmentRepairFundsApplyBase
    {
        public EquipmentRepairFundsCollegeAudit(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentRepairFundsApply> GetMyGridViewEquipmentRepairFundsApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("(Status={0}+Status={1}+Status={2})*OrganizationDirectorId=\"{3}\"", (int)EquipmentRepairFundsApplyStatus.LabRoomAuditPassed, (int)EquipmentRepairFundsApplyStatus.CollegeAuditNoPassed, (int)EquipmentRepairFundsApplyStatus.CollegeAuditPassed, _operator.Id));
            return _viewEquipmentRepairFundsApplyBLL.GetGridViewEquipmentRepairFundsApplyListByExpression(_operatorId, dataGridSettings, _mapping, dataGridSettings.GetOrderExpression(), true, false, true, _outMapping);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableCollegeAuditPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.CollegeAuditPassed, auditContext);
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public override bool JudgeIsEnableAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            errorMessage = "";
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableCollegeAuditNoPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditNoPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.CollegeAuditNoPassed, auditContext);
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        protected override EquipmentRepairFundsApply GenerateAuditInfo(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus equipmentRepairFundsApplyStatus, EquipmentRepairFundsApplyAuditContext auditContext)
        {
            equipmentRepairFundsApply.CollegeAuditorId = _operatorId;
            equipmentRepairFundsApply.CollegeAuditRemark = auditContext.Remark;
            equipmentRepairFundsApply.CollegeAuditTime = DateTime.Now;
            equipmentRepairFundsApply.StatusEnum = equipmentRepairFundsApplyStatus;
            return equipmentRepairFundsApply;
        }

        protected override void OperateDecorate(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply) 
        {
            viewEquipmentRepairFundsApply.LabRoomAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.EquipmentDeptAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.EquipmentDeptAuditPassOperate = "";
        }
    }
}
