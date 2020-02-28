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
    public class EquipmentLabRoomDirectorAudit: EquipmentApplyBase
    {
        public EquipmentLabRoomDirectorAudit(Guid operatorId)
            : base(operatorId) { }
        protected override Logic.Model.GridData<Model.View.ViewEquipmentApply> GetMyGridViewEquipmentApplyList(JqueryEasyUI.Core.DataGridSettings dataGridSettings)
        {
            dataGridSettings.AppendAndQueryExpression(string.Format("Status={0}+Status={1}+Status={2}",(int)EquipmentApplyStatus.Applied,(int)EquipmentApplyStatus.LabRoomDirectotAuditNoPassed,(int)EquipmentApplyStatus.LabRoomDirectotAuditPassed));
            return _viewEquipmentApplyBLL.GetGridViewEquipmentApplyListByExpression(_operatorId,dataGridSettings);
        }

        public override bool JudgeIsEnableAuditPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            return _viewEquipmentApplyBLL.JudgeIsEnableLabRoomDirectorAuditPass(_operatorId, viewEquipmentApply, out errorMessage);
        }

        protected override bool AuditPass(EquipmentApply equipmentApply, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentApply = GenerateAuditInfo(equipmentApply, EquipmentApplyStatus.LabRoomDirectotAuditPassed, remark);
                _equipmentApplyBLL.Save(new EquipmentApply[] { equipmentApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        public override bool JudgeIsEnableAuditNoPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage)
        {
            errorMessage = "";
            return _viewEquipmentApplyBLL.JudgeIsEnableLabRoomDirectorAuditNoPass(_operatorId, viewEquipmentApply, out errorMessage);
        }

        protected override bool AuditNoPass(EquipmentApply  equipmentApply, string remark, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = null;
            try
            {
                equipmentApply = GenerateAuditInfo(equipmentApply,EquipmentApplyStatus.LabRoomDirectotAuditNoPassed,remark);
                _equipmentApplyBLL.Save(new EquipmentApply[] { equipmentApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }

        protected override EquipmentApply GenerateAuditInfo(EquipmentApply equipmentApply, EquipmentApplyStatus equipmentApplyStatus, string remark)
        {
            equipmentApply.LabRoomAuditDirectorId = _operatorId;
            equipmentApply.LabRoomDirectorAuditRemark = remark;
            equipmentApply.LabRoomDirectorAuditTime = DateTime.Now;
            equipmentApply.StatusEnum = equipmentApplyStatus;
            return equipmentApply;
        }

        protected override void OperateDecorate(ViewEquipmentApply viewEquipmentApply) 
        {
            viewEquipmentApply.SchoolDirectorAuditNoPassOperate = "";
            viewEquipmentApply.SchoolDirectorAuditPassOperate = "";
        }
    }
}
