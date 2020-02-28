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
    public class EquipmentRepairFundsShareEAudit: EquipmentRepairFundsApplyBase
    {
        public EquipmentRepairFundsShareEAudit(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentRepairFundsApply> GetMyGridViewEquipmentRepairFundsApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("(Status={0}+Status={1}+Status={2})*IsBigAmount=true", (int)EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed, (int)EquipmentRepairFundsApplyStatus.SharEAuditNoPassed, (int)EquipmentApplyStatus.SharEDirectorAuditPassed));
            return _viewEquipmentRepairFundsApplyBLL.GetGridViewEquipmentRepairFundsApplyListByExpression(_operatorId, dataGridSettings, _mapping, dataGridSettings.GetOrderExpression(), true, false, true, _outMapping);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableShareEAuditPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.SharEAuditPassed, auditContext);
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
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableShareEAuditNoPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditNoPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.SharEAuditNoPassed, auditContext);
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
            equipmentRepairFundsApply.ShareEAuditorId = _operatorId;
            equipmentRepairFundsApply.ShareEAuditRemark = auditContext.Remark;
            equipmentRepairFundsApply.ShareEAuditTime = DateTime.Now;
            equipmentRepairFundsApply.ShareEAuditPassBalance = auditContext.PassBalance;
            if (equipmentRepairFundsApplyStatus == EquipmentRepairFundsApplyStatus.SharEAuditNoPassed)
            {
                equipmentRepairFundsApply.ShareEAuditPassBalance = 0;
                equipmentRepairFundsApply.ObtainedFee = 0d;
            }
            if (equipmentRepairFundsApplyStatus == EquipmentRepairFundsApplyStatus.SharEAuditPassed)
            {
                equipmentRepairFundsApply.ObtainedFee = (equipmentRepairFundsApply.RepairTotalFee * (auditContext.PassBalance.HasValue ? auditContext.PassBalance.Value : 0d)) / 100;
            }
            equipmentRepairFundsApply.StatusEnum = equipmentRepairFundsApplyStatus;
            return equipmentRepairFundsApply;
        }

        protected override void OperateDecorate(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply)
        {
            viewEquipmentRepairFundsApply.EquipmentDeptAuditNoPassOperate = "";
        }
    }
}
