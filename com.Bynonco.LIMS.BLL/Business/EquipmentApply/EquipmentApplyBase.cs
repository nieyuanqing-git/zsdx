using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.IBLL.View;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class EquipmentApplyBase
    {
        protected Guid _operatorId;
        protected IEquipmentApplyBLL _equipmentApplyBLL;
        protected IViewEquipmentApplyBLL _viewEquipmentApplyBLL;
        protected IEquipmentGroupServerBLL _equipmentGroupServerBLL;
        protected IEquipmentServiceHoursStatBLL _equipmentServiceHoursStatBLL;
        public EquipmentApplyBase(Guid operatorId)
        {
            _equipmentApplyBLL = BLLFactory.CreateEquipmentApplyBLL();
            _viewEquipmentApplyBLL = BLLFactory.CreateViewEquipmentApplyBLL();
            _equipmentGroupServerBLL = BLLFactory.CreateEquipmentGroupServerBLL();
            _equipmentServiceHoursStatBLL = BLLFactory.CreateEquipmentServiceHoursStatBLL();
            this._operatorId = operatorId;
        }
        protected abstract GridData<ViewEquipmentApply> GetMyGridViewEquipmentApplyList(DataGridSettings dataGridSettings);
        protected abstract void OperateDecorate(ViewEquipmentApply viewEquipmentApply);
        public GridData<ViewEquipmentApply> GetGridViewEquipmentApplyList(DataGridSettings dataGridSettings)
        {
            var viewEquipmentApplyList = GetMyGridViewEquipmentApplyList(dataGridSettings);
            if (viewEquipmentApplyList != null && viewEquipmentApplyList.Data != null && viewEquipmentApplyList.Data.Count > 0)
            {
                foreach (var viewEquipmentApply in viewEquipmentApplyList.Data)
                {
                    OperateDecorate(viewEquipmentApply);
                    viewEquipmentApply.BuildOperate();
                }
            }

            return viewEquipmentApplyList;
        }
        public virtual bool Apply(EquipmentApply equipmentApply, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                if (!_viewEquipmentApplyBLL.JudgeIsEnableApply(_operatorId, out errorMessage)) return false;
                tran = SessionManager.Instance.BeginTransaction();
                var equipmentApplyTemp = _equipmentApplyBLL.GetEntityById(equipmentApply.Id);
                if (equipmentApplyTemp != null)
                {
                    _equipmentApplyBLL.Save(new EquipmentApply[] { equipmentApply }, ref tran,true);
                    var equipmentGroupServerList = _equipmentGroupServerBLL.GetEntitiesByExpression(string.Format("EquipmentApplyId=\"{0}\"", equipmentApply.Id));
                    if (equipmentGroupServerList != null && equipmentGroupServerList.Count > 0)
                        _equipmentGroupServerBLL.Delete(equipmentGroupServerList.Select(p => p.Id), ref tran, true);
                    var equipmentServiceHoursStatList = _equipmentServiceHoursStatBLL.GetEntitiesByExpression(string.Format("EquipmentApplyId=\"{0}\"", equipmentApply.Id));
                    if (equipmentServiceHoursStatList != null && equipmentServiceHoursStatList.Count > 0)
                        _equipmentServiceHoursStatBLL.Delete(equipmentServiceHoursStatList.Select(p => p.Id), ref tran, true);
                }
                else
                {
                    equipmentApply.ApplicantId = _operatorId;
                    equipmentApply.ApplyTime = DateTime.Now;
                    _equipmentApplyBLL.Add(new EquipmentApply[] { equipmentApply }, ref tran,true);
                }
                if (equipmentApply.EquipmentGroupServers != null && equipmentApply.EquipmentGroupServers.Count > 0)
                    _equipmentGroupServerBLL.Add(equipmentApply.EquipmentGroupServers, ref tran, true);
                if (equipmentApply.EquipmentServiceHoursStats != null && equipmentApply.EquipmentServiceHoursStats.Count > 0)
                    _equipmentServiceHoursStatBLL.Add(equipmentApply.EquipmentServiceHoursStats, ref tran, true);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (tran != null) tran.RollbackTransaction();
                return false;
            }
            finally { if (tran != null) tran.Dispose(); }
            return true;
        }
        public virtual bool Cacel(Guid equipmentApplyId, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                var viewEquipmentApply = _viewEquipmentApplyBLL.GetEntityById(equipmentApplyId);
                if (!_viewEquipmentApplyBLL.JudgeIsEnableCancel(_operatorId, viewEquipmentApply, out errorMessage)) return false;
                var equipmentApply = _equipmentApplyBLL.GetEntityById(equipmentApplyId);
                if (equipmentApply == null) throw new Exception("找不到申请单信息");
                if (equipmentApply.StatusEnum != EquipmentApplyStatus.Applied)
                    throw new Exception(string.Format("当前申请状态【{0}】不行执行取消操作", equipmentApply.StatusEnum.ToCnName()));
                equipmentApply.StatusEnum = Model.Enum.EquipmentApplyStatus.Canceled;
                equipmentApply.CancelTime = DateTime.Now;
                equipmentApply.CancelOperatorId = _operatorId;
                _equipmentApplyBLL.Save(new EquipmentApply[] { equipmentApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public abstract bool JudgeIsEnableAuditPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage);
        protected abstract bool AuditPass(EquipmentApply equipmentApply, string remark, out string errorMessage);
        public virtual bool ExecAuditPass(ViewEquipmentApply viewEquipmentApply, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableAuditPass(viewEquipmentApply, out errorMessage)) return false;
            return AuditPass(viewEquipmentApply.EquipmentApply, remark, out errorMessage);
        }
        public virtual bool ExecAuditPass(Guid equipmentApplyId, string remark, out string errorMessage)
        {
            ViewEquipmentApply viewEquipmentApply = _viewEquipmentApplyBLL.GetEntityById(equipmentApplyId);
            return ExecAuditPass(viewEquipmentApply, remark, out errorMessage);
        }
        public virtual bool BatchExecAuditPass(IEnumerable<Guid> equipmentApplyIds, string remark, out string errorMessage,out int totalCount,out int sucessCount,out int failCount )
        {
            errorMessage = "";
            totalCount = 0;
            failCount = 0;
            sucessCount = 0;
            if (equipmentApplyIds == null || equipmentApplyIds.Count() == 0) return true;
            var viewEquipmentApplyList = _viewEquipmentApplyBLL.GetEntitiesByExpression(equipmentApplyIds.ToFormatStr());
            if (viewEquipmentApplyList == null || viewEquipmentApplyList.Count == 0) return true;
            totalCount = viewEquipmentApplyList.Count;
            StringBuilder sbErrorMessage = new StringBuilder();
            {
                foreach (var viewEquipmentApply in viewEquipmentApplyList)
                {
                    if (!ExecAuditPass(viewEquipmentApply, remark, out errorMessage))
                    {
                        sbErrorMessage.AppendFormat("设备{0}审核通过失败,原因:{1}", viewEquipmentApply.Name,errorMessage).AppendLine();
                        failCount++;
                        continue;
                    }
                    sucessCount++;
                }
            }
            errorMessage = sbErrorMessage.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
        public abstract bool JudgeIsEnableAuditNoPass(ViewEquipmentApply viewEquipmentApply, out string errorMessage);
        protected abstract bool AuditNoPass(EquipmentApply equipmentApply, string remark, out string errorMessage);
        public virtual bool ExecAuditNoPass(Guid equipmentApplyId, string remark, out string errorMessage)
        {
            var viewEquipmentApply = _viewEquipmentApplyBLL.GetEntityById(equipmentApplyId);
            return ExecAuditNoPass(viewEquipmentApply, remark, out errorMessage);
        }
        public virtual bool ExecAuditNoPass(ViewEquipmentApply viewEquipmentApply, string remark, out string errorMessage)
        {
            if (!JudgeIsEnableAuditNoPass(viewEquipmentApply, out errorMessage)) return false;
            return AuditNoPass(viewEquipmentApply.EquipmentApply, remark, out errorMessage);
        }
        public virtual bool BatchExecAuditNoPass(IEnumerable<Guid> equipmentApplyIds, string remark, out string errorMessage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMessage = "";
            errorMessage = "";
            totalCount = 0;
            failCount = 0;
            sucessCount = 0;
            if (equipmentApplyIds == null || equipmentApplyIds.Count() == 0) return true;
            var viewEquipmentApplyList = _viewEquipmentApplyBLL.GetEntitiesByExpression(equipmentApplyIds.ToFormatStr());
            if (viewEquipmentApplyList == null || viewEquipmentApplyList.Count == 0) return true;
            totalCount = viewEquipmentApplyList.Count;
            StringBuilder sbErrorMessage = new StringBuilder();
            {
                foreach (var viewEquipmentApply in viewEquipmentApplyList)
                {
                    if (!ExecAuditNoPass(viewEquipmentApply, remark, out errorMessage))
                    {
                        sbErrorMessage.AppendFormat("设备{0}审核不通过失败,原因:{1}", viewEquipmentApply.Name, errorMessage).AppendLine();
                        failCount++;
                        continue;
                    }
                    sucessCount++;
                }
            }
            errorMessage = sbErrorMessage.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
        protected abstract EquipmentApply GenerateAuditInfo(EquipmentApply equipmentApply, EquipmentApplyStatus equipmentApplyStatus, string remark);
    }
}
