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
    public class EquipmentRepairFundsEquipmentDeptAudit: EquipmentRepairFundsApplyBase
    {
        public EquipmentRepairFundsEquipmentDeptAudit(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentRepairFundsApply> GetMyGridViewEquipmentRepairFundsApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("Status={0}+Status={1}+Status={2}", (int)EquipmentRepairFundsApplyStatus.CollegeAuditPassed, (int)EquipmentRepairFundsApplyStatus.EquipmentDeptAuditNoPassed, (int)EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed));
            return _viewEquipmentRepairFundsApplyBLL.GetGridViewEquipmentRepairFundsApplyListByExpression(_operatorId, dataGridSettings, _mapping, dataGridSettings.GetOrderExpression(), true, false, true, _outMapping);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage)
        {
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableEquipmentDeptAuditPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed,auditContext);
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
            return _viewEquipmentRepairFundsApplyBLL.JudgeIsEnableEquipmentDeptAuditNoPass(_operatorId, viewEquipmentRepairFundsApply, out errorMessage);
        }

        protected override bool AuditNoPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentRepairFundsApply = GenerateAuditInfo(equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus.EquipmentDeptAuditNoPassed, auditContext);
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        protected override EquipmentRepairFundsApply GenerateAuditInfo(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyStatus equipmenRepairFundsApplyStatus, EquipmentRepairFundsApplyAuditContext auditContext)
        {
            equipmentRepairFundsApply.EquipmentDeptAuditorId = _operatorId;
            equipmentRepairFundsApply.EquipmentDeptAuditRemark = auditContext.Remark;
            equipmentRepairFundsApply.EquipmentDeptAuditPassBalance = auditContext.PassBalance;
            if (equipmenRepairFundsApplyStatus == EquipmentRepairFundsApplyStatus.EquipmentDeptAuditNoPassed)
            {
                equipmentRepairFundsApply.EquipmentDeptAuditPassBalance = 0;
                equipmentRepairFundsApply.ObtainedFee = 0d;
            }
            if (equipmenRepairFundsApplyStatus == EquipmentRepairFundsApplyStatus.EquipmentDeptAuditPassed)
            {
                equipmentRepairFundsApply.ObtainedFee = (equipmentRepairFundsApply.RepairTotalFee * (auditContext.PassBalance.HasValue ? auditContext.PassBalance.Value : 0d)) / 100;
            }
            equipmentRepairFundsApply.IsEquipmentInfoComplete = auditContext.IsEquipmentInfoComplete;
            equipmentRepairFundsApply.IsEquipmentLastYearPerformancePassed = auditContext.IsEquipmentLastYearPerformancePassed;
            equipmentRepairFundsApply.IsEquipmentUseRecordInputCorrect = auditContext.IsEquipmentUseRecordInputCorrect;
            equipmentRepairFundsApply.EquipmentDeptAuditTime = DateTime.Now;
            equipmentRepairFundsApply.StatusEnum = equipmenRepairFundsApplyStatus;
            equipmentRepairFundsApply.IsNeedSchoolMasterAudit = auditContext.IsNeedSchoolMasterAudit;
            equipmentRepairFundsApply.ApplyTypeId = auditContext.ApplyTypeId;
            return equipmentRepairFundsApply;
        }


        protected override void OperateDecorate(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply)
        {
            viewEquipmentRepairFundsApply.CollegeAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.ShareEAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.ShareEAuditPassOperate = "";
            viewEquipmentRepairFundsApply.SchoolAuditNoPassOperate = "";
            viewEquipmentRepairFundsApply.SchoolAuditPassOperate = "";
        }
    }
}
