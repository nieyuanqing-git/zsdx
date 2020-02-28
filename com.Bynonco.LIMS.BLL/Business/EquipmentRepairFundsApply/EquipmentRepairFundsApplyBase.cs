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
using com.Bynonco.LIMS.BLL.Business;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class EquipmentRepairFundsApplyBase
    {
        protected Guid _operatorId;
        protected User _operator;
        protected IEquipmentRepairFundsApplyBLL _equipmentRepairFundsApplyBLL;
        protected IViewEquipmentRepairFundsApplyBLL _viewEquipmentRepairFundsApplyBLL;
        protected IEquipmentRepairFundsApplyAttachmentBLL _equipmentRepairFundsApplyAttachmentBLL;
        protected IUserBLL _userBLL;
        protected string[] _outMapping = new string[] { "RoomDirectorId", "OrganizationDirectorId" };
        protected Dictionary<string, string> _mapping = new Dictionary<string, string>();
        public EquipmentRepairFundsApplyBase(Guid operatorId)
        {
            _equipmentRepairFundsApplyBLL = BLLFactory.CreateEquipmentRepairFundsApplyBLL();
            _viewEquipmentRepairFundsApplyBLL = BLLFactory.CreateViewEquipmentRepairFundsApplyBLL();
            _equipmentRepairFundsApplyAttachmentBLL = BLLFactory.CreateEquipmentRepairFundsApplyAttachmentBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            this._operatorId = operatorId;
            _operator = _userBLL.GetEntityById(operatorId);
            _mapping["Id"] = "EquipmentRepairFundsApplyId";
        }
        protected abstract GridData<ViewEquipmentRepairFundsApply> GetMyGridViewEquipmentRepairFundsApplyList(DataGridSettings dataGridSettings);
        protected abstract void OperateDecorate(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply);
        public GridData<ViewEquipmentRepairFundsApply> GetGridViewEquipmentRepairFundsApplyList(DataGridSettings dataGridSettings)
        {
            var viewEquipmentRepairFundsApplyList = GetMyGridViewEquipmentRepairFundsApplyList(dataGridSettings);
            if (viewEquipmentRepairFundsApplyList != null && viewEquipmentRepairFundsApplyList.Data != null && viewEquipmentRepairFundsApplyList.Data.Count > 0)
            {
                foreach (var viewEquipmentRepairFundsApply in viewEquipmentRepairFundsApplyList.Data)
                {
                    OperateDecorate(viewEquipmentRepairFundsApply);
                    viewEquipmentRepairFundsApply.BuildOperate();
                }
            }

            return viewEquipmentRepairFundsApplyList;
        }
        public virtual bool Apply(EquipmentRepairFundsApply equipmentRepairFundsApply, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                if (!_viewEquipmentRepairFundsApplyBLL.JudgeIsEnableApply(_operatorId, out errorMessage)) return false;
                tran = SessionManager.Instance.BeginTransaction();
                var equipmentRepairFundsApplyTemp = _equipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApply.Id);
                if (equipmentRepairFundsApplyTemp != null)
                {
                    var equipmentRepairFundsApplyAttachmentsTemp = _equipmentRepairFundsApplyAttachmentBLL.GetEntitiesByExpression(string.Format("EquipmentRepairFundsApplyId=\"{0}\"", equipmentRepairFundsApply.Id));
                    if (equipmentRepairFundsApplyAttachmentsTemp != null && equipmentRepairFundsApplyAttachmentsTemp.Count > 0)
                    {
                        var willdelEquipmentRepairFundsApplyAttachments = equipmentRepairFundsApplyAttachmentsTemp;
                        if (equipmentRepairFundsApply.Attachments != null && equipmentRepairFundsApply.Attachments.Count > 0)
                            willdelEquipmentRepairFundsApplyAttachments = equipmentRepairFundsApplyAttachmentsTemp.Where(p => !equipmentRepairFundsApply.Attachments.Select(x => x.Id).Contains(p.Id)).ToList();
                        if (willdelEquipmentRepairFundsApplyAttachments != null && willdelEquipmentRepairFundsApplyAttachments.Count > 0)
                            _equipmentRepairFundsApplyAttachmentBLL.Delete(willdelEquipmentRepairFundsApplyAttachments.Select(p => p.Id).ToArray(), ref tran, true);
                    }
                    _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran, true);
                }
                else
                {
                    equipmentRepairFundsApply.ApplicantId = _operatorId;
                    equipmentRepairFundsApply.ApplyTime = DateTime.Now;
                    _equipmentRepairFundsApplyBLL.Add(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran, true);
                }
                if (equipmentRepairFundsApply.Attachments != null && equipmentRepairFundsApply.Attachments.Count > 0)
                {

                    foreach (var equipmentRepairFundsApplyAttachment in equipmentRepairFundsApply.Attachments)
                    {
                        var equipmentRepairFundsApplyAttachments = _equipmentRepairFundsApplyAttachmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", equipmentRepairFundsApplyAttachment.Id.ToString()));
                        if (equipmentRepairFundsApplyAttachments == null || equipmentRepairFundsApplyAttachments.Count == 0)
                            _equipmentRepairFundsApplyAttachmentBLL.Add(new EquipmentRepairFundsApplyAttachment[] { equipmentRepairFundsApplyAttachment }, ref tran, true);
                        else
                            _equipmentRepairFundsApplyAttachmentBLL.Save(new EquipmentRepairFundsApplyAttachment[] { equipmentRepairFundsApplyAttachment }, ref tran, true);
                    }
                }
                if (tran.HasTransaction) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (tran != null && tran.HasTransaction) tran.RollbackTransaction();
                errorMessage = ex.Message;
                return false;
            }
            finally { if (tran != null)tran.Dispose(); }
            return true;
        }
        public virtual bool Cacel(Guid equipmentRepairFundsApplyId, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                var viewEquipmentRepairFundsApply = _viewEquipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (!_viewEquipmentRepairFundsApplyBLL.JudgeIsEnableCancel(_operatorId, viewEquipmentRepairFundsApply, out errorMessage)) return false;
                var equipmentRepairFundsApply = _equipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (equipmentRepairFundsApply == null) throw new Exception("找不到申请单信息");
                equipmentRepairFundsApply.StatusEnum = EquipmentRepairFundsApplyStatus.Canceled;
                equipmentRepairFundsApply.CancelTime = DateTime.Now;
                equipmentRepairFundsApply.CancelOperatorId = _operatorId;
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public abstract bool JudgeIsEnableAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage);
        protected abstract bool AuditPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage);
        public virtual bool ExecAuditPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            if (!JudgeIsEnableAuditPass(viewEquipmentRepairFundsApply, out errorMessage)) return false;
            return AuditPass(viewEquipmentRepairFundsApply.EquipmentRepairFundsApply, auditContext, out errorMessage);
        }
        public virtual bool ExecAuditPass(Guid equipmentRepairFundsApplyId, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply = _viewEquipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
            return ExecAuditPass(viewEquipmentRepairFundsApply, auditContext, out errorMessage);
        }
        public virtual bool BatchExecAuditPass(IEnumerable<Guid> equipmentRepairFundsApplyIds,EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMessage = "";
            totalCount = 0;
            failCount = 0;
            sucessCount = 0;
            if (equipmentRepairFundsApplyIds == null || equipmentRepairFundsApplyIds.Count() == 0) return true;
            var viewEquipmentRepairFundsApplyList = _viewEquipmentRepairFundsApplyBLL.GetEntitiesByExpression(equipmentRepairFundsApplyIds.ToFormatStr(), _mapping, "", false, false, true, _outMapping);
            if (viewEquipmentRepairFundsApplyList == null || viewEquipmentRepairFundsApplyList.Count == 0) return true;
            totalCount = viewEquipmentRepairFundsApplyList.Count;
            StringBuilder sbErrorMessage = new StringBuilder();
            {
                foreach (var viewEquipmentRepairFundsApply in viewEquipmentRepairFundsApplyList)
                {
                    if (!ExecAuditPass(viewEquipmentRepairFundsApply, auditContext, out errorMessage))
                    {
                        sbErrorMessage.AppendFormat("设备{0}维修基金审核通过失败,原因:{1}", viewEquipmentRepairFundsApply.EquipmentName,errorMessage).AppendLine();
                        failCount++;
                        continue;
                    }
                    sucessCount++;
                }
            }
            errorMessage = sbErrorMessage.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
        public abstract bool JudgeIsEnableAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, out string errorMessage);
        protected abstract bool AuditNoPass(EquipmentRepairFundsApply equipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage);
        public virtual bool ExecAuditNoPass(Guid equipmentRepairFundsApplyId, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            var viewEquipmentRepairFundsApply = _viewEquipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
            return ExecAuditNoPass(viewEquipmentRepairFundsApply, auditContext, out errorMessage);
        }
        public virtual bool ExecAuditNoPass(ViewEquipmentRepairFundsApply viewEquipmentRepairFundsApply, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage)
        {
            if (!JudgeIsEnableAuditNoPass(viewEquipmentRepairFundsApply, out errorMessage)) return false;
            return AuditNoPass(viewEquipmentRepairFundsApply.EquipmentRepairFundsApply, auditContext, out errorMessage);
        }
        public virtual bool BatchExecAuditNoPass(IEnumerable<Guid> equipmentRepairFundsApplyIds, EquipmentRepairFundsApplyAuditContext auditContext, out string errorMessage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMessage = "";
            errorMessage = "";
            totalCount = 0;
            failCount = 0;
            sucessCount = 0;
            if (equipmentRepairFundsApplyIds == null || equipmentRepairFundsApplyIds.Count() == 0) return true;
            var viewEquipmentRepairFundsApplyList = _viewEquipmentRepairFundsApplyBLL.GetEntitiesByExpression(equipmentRepairFundsApplyIds.ToFormatStr(), _mapping, "", false, false, true, _outMapping);
            if (viewEquipmentRepairFundsApplyList == null || viewEquipmentRepairFundsApplyList.Count == 0) return true;
            totalCount = viewEquipmentRepairFundsApplyList.Count;
            StringBuilder sbErrorMessage = new StringBuilder();
            {
                foreach (var viewEquipmentRepairFundsApply in viewEquipmentRepairFundsApplyList)
                {
                    if (!ExecAuditNoPass(viewEquipmentRepairFundsApply, auditContext, out errorMessage))
                    {
                        sbErrorMessage.AppendFormat("设备维修基金{0}审核不通过失败,原因:{1}", viewEquipmentRepairFundsApply.EquipmentName, errorMessage).AppendLine();
                        failCount++;
                        continue;
                    }
                    sucessCount++;
                }
            }
            errorMessage = sbErrorMessage.ToString();
            return string.IsNullOrWhiteSpace(errorMessage);
        }
        public virtual bool FinishRepairRegister(Guid equipmentRepairFundsApplyId, string repairRecord, DateTime finishRepairTime, out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                var viewEquipmentRepairFundsApply = _viewEquipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (!_viewEquipmentRepairFundsApplyBLL.JudgeIsEnableFinishRepairRegister(_operatorId, viewEquipmentRepairFundsApply, out errorMessage)) return false;
                var equipmentRepairFundsApply = _equipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (equipmentRepairFundsApply == null) throw new Exception("找不到申请单信息");
                equipmentRepairFundsApply.StatusEnum = EquipmentRepairFundsApplyStatus.FinishRepaired;
                equipmentRepairFundsApply.IsFinishRepair = true;
                equipmentRepairFundsApply.FinishRepairRegisterOperatorId = _operatorId;
                equipmentRepairFundsApply.FinishRepairTime = finishRepairTime;
                equipmentRepairFundsApply.RepairRecord = repairRecord;
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public virtual bool FundsProvide(Guid equipmentRepairFundsApplyId, DateTime fundsProvideTime,out string errorMessage)
        {
            XTransaction tran = null;
            try
            {
                var viewEquipmentRepairFundsApply = _viewEquipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (!_viewEquipmentRepairFundsApplyBLL.JudgeIsEnableFundsProvide(_operatorId, viewEquipmentRepairFundsApply, out errorMessage)) return false;
                var equipmentRepairFundsApply = _equipmentRepairFundsApplyBLL.GetEntityById(equipmentRepairFundsApplyId);
                if (equipmentRepairFundsApply == null) throw new Exception("找不到申请单信息");
                equipmentRepairFundsApply.StatusEnum = EquipmentRepairFundsApplyStatus.FundsProvided;
                equipmentRepairFundsApply.FundsProvideOperatorId = _operatorId;
                equipmentRepairFundsApply.IsFundsProvide = true ;
                equipmentRepairFundsApply.FundsProvideTime = fundsProvideTime;
                _equipmentRepairFundsApplyBLL.Save(new EquipmentRepairFundsApply[] { equipmentRepairFundsApply }, ref tran);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        protected abstract EquipmentRepairFundsApply GenerateAuditInfo(EquipmentRepairFundsApply equipmentRepairFundsApply,Model.Enum.EquipmentRepairFundsApplyStatus equipmentRepairFundsApplyStatus,EquipmentRepairFundsApplyAuditContext auditContext);
    }
}
