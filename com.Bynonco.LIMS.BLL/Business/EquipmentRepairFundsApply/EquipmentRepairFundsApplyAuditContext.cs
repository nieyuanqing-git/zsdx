using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentRepairFundsApplyAuditContext
    {
        public string Remark { get; private set; }
        public double? PassBalance { get; private set; }
        public bool IsEquipmentInfoComplete { get; private set; }
        public bool IsEquipmentUseRecordInputCorrect { get; private set; }
        public bool IsEquipmentLastYearPerformancePassed { get; private set; }
        public bool IsNeedSchoolMasterAudit { get; private set; }
        public Guid? ApplyTypeId { get; private set; }
        public EquipmentRepairFundsApplyAuditContext(string remark)
        {
            this.Remark = remark;
        }

        public EquipmentRepairFundsApplyAuditContext(string remark, double? passBalance, Guid? applyTypeId)
            : this(remark)
        {
            this.PassBalance = passBalance;
            this.ApplyTypeId = applyTypeId;
        }

        public EquipmentRepairFundsApplyAuditContext(string remark, double? passBalance, bool isEquipmentInfoComplete, bool isEquipmentUseRecordInputCorrect, bool isEquipmentLastYearPerformancePassed, bool isNeedSchoolMasterAudit, Guid? applyTypeId)
            : this(remark, passBalance, applyTypeId)
        {
            this.IsEquipmentInfoComplete = isEquipmentInfoComplete;
            this.IsEquipmentLastYearPerformancePassed = isEquipmentLastYearPerformancePassed;
            this.IsEquipmentUseRecordInputCorrect = isEquipmentUseRecordInputCorrect;
            this.IsNeedSchoolMasterAudit = isNeedSchoolMasterAudit;
        }
    }
}
