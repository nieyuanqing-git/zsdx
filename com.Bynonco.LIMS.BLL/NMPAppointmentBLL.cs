using com.august.DataLink;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPAppointmentBLL : BLLBase<NMPAppointment>, INMPAppointmentBLL
    {
        private IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        private IPunishActionBLL __punishActionBLL;
        private IPunishRecordBLL __punishRecordBLL;
        private INMPEquipmentBLL __nmpEquipmentBLL;
        private INMPBLL __nmpBLL;
        public NMPAppointmentBLL()
        {
            __delinquencyConfirmBLL = BLLFactory.CreateDelinquencyConfirmBLL();
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
            __nmpEquipmentBLL = BLLFactory.CreateNMPEquipmentBLL();
            __nmpBLL = BLLFactory.CreateNMPBLL();
        }
        public string GetPeriodPossiblyValidateNMPAppointmentQueryExpression(Guid? nmpEquipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = string.Format("BeginTime>\"{0}\"*BeginTime<\"{1}\"*Status={2}*AuditingStatus=-{3}*TutorAuditStatus=-{4}", beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.NotPass, (int)TutorAuditStatus.Refused);
            if (nmpEquipmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*NMPEquipmentId=\"{0}\"", nmpEquipmentId.Value);
            return periodValidateAppointmentQueryExpression;
        }
        public string GetPeriodValidateNMPAppointmentQueryExpression(Guid? nmpAppointmentId, Guid? nmpEquipmentId, DateTime beginAt, DateTime endAt)
        {
            var auditExpression = string.Format("*((IsNeedAudit=true*AuditingStatus={0}*IsNeedTutorAudit=true*TutorAuditStatus={1})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={0})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={1})+(IsNeedTutorAudit=false*IsNeedAudit=false))", (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed);
            var periodValidateAppointmentQueryExpression = string.Format("EndTime>\"{0}\"*BeginTime<\"{1}\"*Status=-{2}*Status=-{3}*Status=-{4}", beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Cancel, (int)AppointmentStatus.Changed, (int)AppointmentStatus.Fail);
            if (nmpAppointmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*Id=-\"{0}\"", nmpAppointmentId.Value);
            if (nmpEquipmentId.HasValue)
            {
                periodValidateAppointmentQueryExpression += string.Format("*NMPEquipmentId=\"{0}\"", nmpEquipmentId.Value);
                var nmpEquipment = __nmpEquipmentBLL.GetEntityById(nmpEquipmentId.Value);
                if (!nmpEquipment.NMP.IsEnableAppointmentBeforeTutorAudit && !nmpEquipment.NMP.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus=-{0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus=-{1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus=-{0}*AuditingStatus=-{1}))", (int)TutorAuditStatus.Refused, (int)AppointmentAuditStatus.NotPass);
                }
                if (!nmpEquipment.NMP.IsEnableAppointmentBeforeTutorAudit && nmpEquipment.NMP.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus=-{0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus=-{0}*AuditingStatus={1}))", (int)TutorAuditStatus.Refused, (int)AppointmentAuditStatus.Pass);
                }
                if (nmpEquipment.NMP.IsEnableAppointmentBeforeTutorAudit && !nmpEquipment.NMP.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus={0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus=-{1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus={0}*AuditingStatus=-{1}))", (int)TutorAuditStatus.Passed, (int)AppointmentAuditStatus.NotPass);
                }
            }
            periodValidateAppointmentQueryExpression += auditExpression;
            return periodValidateAppointmentQueryExpression;
        }
        private NMPAppointment GetPeriodValidateNMPAppointment(Guid? nmpAppointmentId, Guid nmpEquipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = GetPeriodValidateNMPAppointmentQueryExpression(nmpAppointmentId, nmpEquipmentId, beginAt, endAt);
            if (nmpAppointmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*Id=-\"{0}\"", nmpAppointmentId.Value);
            var nmpAppointment = GetFirstOrDefaultEntityByExpression(periodValidateAppointmentQueryExpression);
            return nmpAppointment;
        }
        public IList<NMPAppointment> GetPeriodValidateNMPAppointmentList(Guid nmpEquipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = GetPeriodValidateNMPAppointmentQueryExpression(null, nmpEquipmentId, beginAt, endAt);
            return GetEntitiesByExpression(periodValidateAppointmentQueryExpression);
        }
        public IList<NMPAppointment> GetTutorPeriodNMPAppointmentList(Guid nmpEquipmentId, Guid tutorId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateNMPAppointmentQueryExpression(nmpEquipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("PayerId=\"{0}\"*{1}", tutorId, periodPossiblyValidateAppointmentQueryExpression));
        }
        public IList<NMPAppointment> GetUserPeriodNMPAppointmentList(Guid nmpEquipmentId, Guid userId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateNMPAppointmentQueryExpression(nmpEquipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("UserId=\"{0}\"*{1}", userId, periodPossiblyValidateAppointmentQueryExpression));
        }
        public IList<NMPAppointment> GetSubjectPeriodNMPAppointmentList(Guid nmpEquipmentId, Guid subjectId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateNMPAppointmentQueryExpression(nmpEquipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*{1}", subjectId, periodPossiblyValidateAppointmentQueryExpression));
        }
        public bool TutorAuditAppointments(IEnumerable<Guid> appointmentIds, Guid operatorId, string operatorIP, TutorAuditStatus tutorAuditStatus, string remark, out string errorMesseage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMesseage = "";
            sucessCount = 0;
            failCount = 0;
            totalCount = 0;
            var messgaeHandler = new MessageHandler();
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            INMPBLL nmpBLL = BLLFactory.CreateNMPBLL();
            IViewNMPAppointmentBLL viewNMPAppointmentBLL = BLLFactory.CreateViewNMPAppointmentBLL();
            var operateUser = userBLL.GetEntityById(operatorId);
            try
            {
                var viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
                var viewNMPppointmentList = viewNMPAppointmentBLL.GetEntitiesByExpression(appointmentIds.ToFormatStr(), null, "", false, false, true, new string[] { "NMPAdminId", "ExperimentationContent" });
                if (viewNMPppointmentList == null || viewNMPppointmentList.Count == 0) throw new Exception("出错,工位预约信息为空");
                totalCount = viewNMPppointmentList.Count;
                var nmpList = nmpBLL.GetEntitiesByExpression(viewNMPppointmentList.Select(p => p.NMPId).ToFormatStr());
                foreach (var viewNMPAppointment in viewNMPppointmentList)
                {
                    var tran = SessionManager.Instance.BeginTransaction();
                    try
                    {
                        string errorMsg = "";
                        if (!JudgeIsEnableTutorAudit(true, tutorAuditStatus, new List<ViewNMPAppointment> { viewNMPAppointment }, nmpList, operatorId, out errorMsg)) throw new Exception(errorMsg);
                        string whyUnusable = "";
                        viewNMPAppointment.NMPAppointment.TutorAuditStatusEnum = tutorAuditStatus;
                        viewNMPAppointment.NMPAppointment.TutorAuditTime = DateTime.Now;
                        viewNMPAppointment.NMPAppointment.TutorAuditRemark = remark;
                        viewNMPAppointment.NMPAppointment.IsUseable = JudgeAppointmentIsUseable(viewNMPAppointment.NMPAppointment, viewNMPAppointment.NMPAppointment.NMPEquipment.NMP, out whyUnusable);
                        viewNMPAppointment.NMPAppointment.WhyUnuseable = whyUnusable;
                        Save(new NMPAppointment[] { viewNMPAppointment.NMPAppointment }, ref tran, true);
                        tran.CommitTransaction();
                        sucessCount++;
                        //消息通知
                        messgaeHandler.SendNMPAppointmentTutorAuditResultNotice(viewNMPAppointment, operateUser);
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        if (tran != null) tran.RollbackTransaction();
                        errorMesseage += ex.Message;
                        continue;
                    }
                    finally { if (tran != null)tran.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                errorMesseage = ex.Message; failCount++;
            }
            return true;
        }
        public bool AuditAppointments(IEnumerable<Guid> appointmentIds, Guid operatorId, string operateIP, AppointmentAuditStatus appointmentAuditStatus, string remark, out string errorMesseage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMesseage = "";
            sucessCount = 0;
            failCount = 0;
            totalCount = 0;
            var messgaeHandler = new MessageHandler();
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            INMPBLL nmpBLL = BLLFactory.CreateNMPBLL();
            IViewNMPAppointmentBLL viewNMPAppointmentBLL = BLLFactory.CreateViewNMPAppointmentBLL();
            var operateUser = userBLL.GetEntityById(operatorId);
            try
            {
                var viewNMPAppointmentList = viewNMPAppointmentBLL.GetEntitiesByExpression(appointmentIds.ToFormatStr(), null, "", false, false, true, new string[] { "NMPAdminId", "ExperimentationContent" });
                if (viewNMPAppointmentList == null || viewNMPAppointmentList.Count == 0) throw new Exception("出错,预约信息为空");
                var nmpList = nmpBLL.GetEntitiesByExpression(viewNMPAppointmentList.Select(p => p.NMPId).Distinct().ToFormatStr());
                var appointmentPriligeList = GetNMPAppointmentPriviliges(operatorId, viewNMPAppointmentList);
                totalCount = viewNMPAppointmentList.Count;
                foreach (var viewNMPAppointment in viewNMPAppointmentList)
                {
                    var tran = SessionManager.Instance.BeginTransaction();
                    try
                    {
                        string whyUnusable = "", errorMsg = "";
                        if (!JudgeIsEnableAudit(true, appointmentAuditStatus, new List<ViewNMPAppointment> { viewNMPAppointment }, appointmentPriligeList, nmpList, operatorId, out errorMsg)) throw new Exception(errorMsg);
                        viewNMPAppointment.NMPAppointment.AuditStatusEnum = appointmentAuditStatus;
                        viewNMPAppointment.NMPAppointment.AuditingUserId = operatorId;
                        viewNMPAppointment.NMPAppointment.AuditingNoPassWhy = remark;
                        viewNMPAppointment.NMPAppointment.IsUseable = JudgeAppointmentIsUseable(viewNMPAppointment.NMPAppointment, viewNMPAppointment.NMPAppointment.NMPEquipment.NMP, out whyUnusable);
                        viewNMPAppointment.NMPAppointment.WhyUnuseable = whyUnusable;
                        Save(new NMPAppointment[] { viewNMPAppointment.NMPAppointment }, ref tran, true);
                        tran.CommitTransaction();
                        sucessCount++;
                        //消息通知
                        messgaeHandler.SendNMPAppointmentAuditResultNotice(viewNMPAppointment, operateUser);
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        if (tran != null) tran.RollbackTransaction();
                        errorMesseage += ex.Message;
                        continue;
                    }
                    finally { if (tran != null)tran.Dispose(); }
                }
            }
            catch (Exception ex)
            {
                errorMesseage = ex.Message; failCount++;
            }
            return true;
        }
        public DelinquencyRule RegisterBreakAppointment(bool isAutoOperate, NMPAppointment nmpAppointment, Guid operatorId)
        {
            string errorMessage = "";
            var viewNMPAppointmentBLL = BLLFactory.CreateViewNMPAppointmentBLL();
            var viewNMPAppointmentList = viewNMPAppointmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", nmpAppointment.Id), null, "", false, false, true, new string[] { "NMPAdminId", "ExperimentationContent" });
            var nmpAppointmentPriviligeList = (IList<NMPAppointmentPrivilige>)GetNMPAppointmentPriviliges(operatorId, viewNMPAppointmentList);
            if (!JudgeIsEnableRegisterBreakAppointment(isAutoOperate, viewNMPAppointmentList, nmpAppointmentPriviligeList, operatorId, out errorMessage)) throw new Exception(errorMessage);
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                nmpAppointment.IsUseable = false;
                nmpAppointment.WhyUnuseable = "爽约";
                nmpAppointment.StatusEnum = Model.Enum.AppointmentStatus.Fail;
                Save(new NMPAppointment[] { nmpAppointment }, ref tran, true);
                var delinquencyConfirm = __delinquencyConfirmBLL.GenerateNMPEquipmentAppointmentDelinquencyConfirmData(nmpAppointment.Id, operatorId, nmpAppointment.EndTime.Value);
                var delinquencyRule = __delinquencyConfirmBLL.AddDelinquencyConfirm(delinquencyConfirm, nmpAppointment.User, ref tran);
                tran.CommitTransaction();
                return delinquencyRule;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                throw;
            }
            finally { tran.Dispose(); }
        }
        public DelinquencyRule RegisterBreakAppointment(bool isAutoOperate, Guid nmpAppointmentId, Guid operatorId)
        {
            var nmpAppointment = GetEntityById(nmpAppointmentId);
            if (nmpAppointment == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的工位预约信息", nmpAppointmentId.ToString()));
            return RegisterBreakAppointment(isAutoOperate, nmpAppointment, operatorId);
        }
        public void CancelRegisterBreakAppointment(Guid delinquencyConfirmId, string content, User user)
        {
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                string errorMessage = "";
                var viewNMPAppointmentBLL = BLLFactory.CreateViewNMPAppointmentBLL();
                var delinquencyConfirm = __delinquencyConfirmBLL.GetEntityById(delinquencyConfirmId);
                var nmpAppointment = GetEntityById(delinquencyConfirm.NMPAppointmentId.Value);
                if (nmpAppointment == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的工位预约信息", delinquencyConfirm.AppointmentId.Value.ToString()));
                nmpAppointment.StatusEnum = AppointmentStatus.Waitting;
                Save(new NMPAppointment[] { nmpAppointment }, ref tran, true);
                if (delinquencyConfirm != null && !delinquencyConfirm.HasRepeal)
                {
                    var viewNMPAppointmentList = viewNMPAppointmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", delinquencyConfirm.NMPAppointmentId.Value), null, "", false, false, true, new string[] { "NMPAdminId", "ExperimentationContent" });
                    var nmpAppointmentPriviligeList = (IList<NMPAppointmentPrivilige>)GetNMPAppointmentPriviliges(user.Id, viewNMPAppointmentList);
                    if (!JudgeIsEnableCancelRegisterBreakAppointment(viewNMPAppointmentList, nmpAppointmentPriviligeList, user.Id, out errorMessage)) throw new Exception(errorMessage);
                    __delinquencyConfirmBLL.Repeal(delinquencyConfirm, content, user, ref tran);
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                throw;
            }
            finally { tran.Dispose(); }
        }
        public bool JudgeAppointmentIsUseable(NMPAppointment nmpAppointment, NMP nmp, out string whyUnUsable)
        {
            INMPBLL nmpBLL = BLLFactory.CreateNMPBLL();
            whyUnUsable = "";
            StringBuilder sbReason = new StringBuilder();
            bool isAppointmentNeedTutorAuditPass = true, isAppointmentNeedAuditPass = true;
            if (nmp.IsAppointmentNeedTutorAudit && nmpAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
            {
                sbReason.Append(nmpBLL.GetTutorUnAuditMessage()).AppendLine();
                isAppointmentNeedTutorAuditPass = false;
            }
            if (nmp.IsAppointmentNeedAudit && nmp.IsAppointmentNeedAudit && nmpAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
            {
                sbReason.Append(nmpBLL.GetUnAuditMessage()).AppendLine();
                isAppointmentNeedAuditPass = false;
            }
            whyUnUsable = sbReason.ToString();
            return isAppointmentNeedAuditPass  && isAppointmentNeedTutorAuditPass;
        }
        public object GetNMPAppointmentPriviliges(Guid? userId, IList<ViewNMPAppointment> viewNMPAppointmentList)
        {
            IList<NMPAppointmentPrivilige> lstNMPAppointmentPriviliges = new List<NMPAppointmentPrivilige>();
            if (userId.HasValue && viewNMPAppointmentList != null && viewNMPAppointmentList.Count > 0)
            {
                foreach (ViewNMPAppointment viewNMPAppoinment in viewNMPAppointmentList)
                {
                    NMPAppointmentPrivilige nmpAppointmentPrivilige = lstNMPAppointmentPriviliges.FirstOrDefault(p => p.NMPAppointmentId.HasValue && p.NMPAppointmentId.Value == viewNMPAppoinment.Id);
                    if (nmpAppointmentPrivilige == null)
                    {
                        nmpAppointmentPrivilige = PriviligeFactory.GetNMPAppointmentPrivilige(userId.Value, viewNMPAppoinment);
                        lstNMPAppointmentPriviliges.Add(nmpAppointmentPrivilige);
                    }
                }
            }
            return lstNMPAppointmentPriviliges;
        }
        private bool JudgeIsEnableCancelorChange(ViewNMPAppointment viewNMPAppointment, NMPAppointmentPrivilige nmpAppointmentPrivilige, NMP nmp, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            if (viewNMPAppointment.StatusEnum != AppointmentStatus.Waitting)
            {
                errorMessager = string.Format("当前状态【{0}】不能进行取消操作", viewNMPAppointment.StatusEnum.ToCnName());
                return false;
            }
            if ((viewNMPAppointment.EndTime.Value < DateTime.Now))
            {
                errorMessager = "已结束,不能进行取消操作";
                return false;
            }
            if (!nmp.IsEnableCancelNotOverAppointment)
            {
                if (viewNMPAppointment.BeginTime.Value <= DateTime.Now)
                {
                    errorMessager = "该预约开始时间大于当前时间,不能进行取消操作";
                    return false;
                }
                if (viewNMPAppointment.BeginTime.Value < DateTime.Now.AddMinutes(nmp.MinAppointmentCancelTime))
                {
                    errorMessager = "过了提前改约时间,不能进行取消操作";
                    return false;
                }
            }
            if (!nmp.IsEnableCancelCheckedAppointment)
            {
                if (viewNMPAppointment.IsNeedAudit && viewNMPAppointment.AuditStatusEnum != AppointmentAuditStatus.UnAudit)
                {
                    errorMessager = "已审核,不能进行取消操作";
                    return false;
                }
            }
            return true;
        }
        private bool JudgeIsEnableAddUsedConfirm(IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, Guid? operatorId, bool isSelfOperate, out string errorMessager)
        {
            errorMessager = "";
            var lstNMPAppointmentPrivilige = (IList<NMPAppointmentPrivilige>)nmpAppointmentPriviliges;
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                if (viewNMPAppointment.HasClossingAccount)
                {
                    errorMessager = "已结算";
                    return false;
                }
                var nmpAppointmentPrivilige = lstNMPAppointmentPrivilige.FirstOrDefault(p => p.NMPAppointmentId.HasValue && p.NMPAppointmentId == viewNMPAppointment.Id);
                if (nmpAppointmentPrivilige == null || (!isSelfOperate && !nmpAppointmentPrivilige.IsEnableAddAppointmentUsedConfirm) || (isSelfOperate && !nmpAppointmentPrivilige.IsEnableSelfAddAppointmentUsedConfirm))
                {
                    errorMessager = "无操作权限";
                    return false;
                }

                if (viewNMPAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessager = string.Format("当前状态【{0}】不允许登记", viewNMPAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewNMPAppointment.EndTime.Value > DateTime.Now)
                {
                    errorMessager = string.Format("预约结束时间【{0}】大于当前时间【{1}】不允许登记", viewNMPAppointment.EndTimeStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    return false;
                }
                if (viewNMPAppointment.IsNeedTutorAudit && viewNMPAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才允许登记";
                    return false;
                }
                if (viewNMPAppointment.IsNeedAudit && viewNMPAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才允许登记";
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableCancel(IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, IEnumerable<NMP> nmps, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            if (viewNMPAppointments == null || viewNMPAppointments.Count() == 0)
            {
                errorMessager = "预约信息为空";
                return false;
            }
            var lstNMPAppointmentPrivilige = (IList<NMPAppointmentPrivilige>)nmpAppointmentPriviliges;
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                var nmp = nmps.First(p => p.Id == viewNMPAppointment.NMPId);
                var nmpAppointmentPrivilige = lstNMPAppointmentPrivilige.FirstOrDefault(p => p.NMPAppointmentId.Value == viewNMPAppointment.Id);
                if (operatorId.HasValue && nmpAppointmentPrivilige == null)
                {
                    nmpAppointmentPrivilige = PriviligeFactory.GetNMPAppointmentPrivilige(operatorId.Value, viewNMPAppointment);
                    if (nmpAppointmentPrivilige != null) lstNMPAppointmentPrivilige.Add(nmpAppointmentPrivilige);
                }
                if (!nmpAppointmentPrivilige.IsEnableCancelAppointment)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                var customer = CustomerFactory.GetCustomer();
                if (!customer.GetIsAdminEnableCancelAppointmentOfOtherUser() && operatorId.HasValue && operatorId.Value != viewNMPAppointment.UserId.Value)
                {
                    errorMessager = "不能取消非自己的预约";
                    return false;
                }
                if (!JudgeIsEnableCancelorChange(viewNMPAppointment, nmpAppointmentPrivilige, nmp, operatorId, out errorMessager))
                {
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableAudit(bool isRigidlyCheck, AppointmentAuditStatus? auditStatus, IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, IEnumerable<NMP> nmps, Guid? operatorId, out string errorMessage)
        {
            errorMessage = "";
            if (viewNMPAppointments == null || viewNMPAppointments.Count() == 0)
            {
                errorMessage = "预约信息为空";
                return false;
            }
            var lstAppointmentPrivilige = (IList<NMPAppointmentPrivilige>)nmpAppointmentPriviliges;
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                var appointmentInfo = string.Format("预约者【{0}】,预约工位设备【{1}】,预约时间【{2}~{3}】", viewNMPAppointment.UserName, viewNMPAppointment.NMPEquipmentName, viewNMPAppointment.BeginTimeStr, viewNMPAppointment.EndTimeStr);
                if (viewNMPAppointment.IsNeedTutorAudit && viewNMPAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessage = appointmentInfo + "需要导师审核通过才能进行审核";
                    return false;
                }
                if (viewNMPAppointment.HasClossingAccount)
                {
                    errorMessage = appointmentInfo + "已结算";
                    return false;
                }
                if (!viewNMPAppointment.IsNeedAudit)
                {
                    errorMessage = appointmentInfo + "无需审核";
                    return false;
                }
                if (viewNMPAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessage = appointmentInfo + string.Format(",预约状态【{0}】不能进行审核操作", viewNMPAppointment.StatusEnum.ToCnName());
                    return false;
                }
                var nmp = nmps.First(p => p.Id == viewNMPAppointment.NMPId);
                if (viewNMPAppointment.BeginTime.Value.AddMinutes(nmp.MinAppointmentCancelTime) < DateTime.Now)
                {
                    errorMessage = appointmentInfo + ",预约已经失效,不能进行审核操作";
                    return false;
                }
                var nmpAppointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.NMPAppointmentId.Value == viewNMPAppointment.Id);
                if (operatorId.HasValue && nmpAppointmentPrivilige == null)
                {
                    nmpAppointmentPrivilige = PriviligeFactory.GetNMPAppointmentPrivilige(operatorId.Value, viewNMPAppointment);
                    if (nmpAppointmentPrivilige != null) lstAppointmentPrivilige.Add(nmpAppointmentPrivilige);
                }
                if (nmpAppointmentPrivilige == null || !nmpAppointmentPrivilige.IsEnableAuditAppointment)
                {
                    errorMessage = appointmentInfo + ",无审核权限,不能进行审核操作";
                    return false;
                }
                if (isRigidlyCheck && auditStatus.HasValue && auditStatus.Value == AppointmentAuditStatus.Pass)
                {
                    var nmpAppointment = GetPeriodValidateNMPAppointment(viewNMPAppointment.Id, viewNMPAppointment.NMPEquipmentId.Value, viewNMPAppointment.BeginTime.Value, viewNMPAppointment.EndTime.Value);
                    if (nmpAppointment != null && DateTimeUtility.IsContain(viewNMPAppointment.BeginTime.Value, viewNMPAppointment.EndTime.Value, nmpAppointment.BeginTime.Value, nmpAppointment.EndTime.Value))
                    {
                        errorMessage = string.Format("出错,改时间段已经被{0}预约并生效", nmpAppointment.User.UserName);
                        return false;
                    }
                }
            }
            return true;
        }
        public bool JudgeIsEnableRegisterBreakAppointment(bool isAutoOperate, IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            var lstNMPAppointmentPrivilige = (IList<NMPAppointmentPrivilige>)nmpAppointmentPriviliges;
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                var appointmentPrivilige = lstNMPAppointmentPrivilige.FirstOrDefault(p => p.NMPAppointmentId.HasValue && p.NMPAppointmentId == viewNMPAppointment.Id);
                if (appointmentPrivilige == null || !appointmentPrivilige.IsEnableRegisterBreakAppointment)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                if (!isAutoOperate)
                {
                    if (viewNMPAppointment.StatusEnum != AppointmentStatus.Waitting)
                    {
                        errorMessager = string.Format("当前状态【{0}】不允许登记爽约", viewNMPAppointment.StatusEnum.ToCnName());
                        return false;
                    }
                    if (viewNMPAppointment.EndTime.Value > DateTime.Now)
                    {
                        errorMessager = string.Format("预约结束时间【{0}】小于当前时间【{1}】不允许登记爽约", viewNMPAppointment.EndTimeStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        return false;
                    }
                }
                else
                {
                    if (viewNMPAppointment.StatusEnum != AppointmentStatus.Cancel)
                    {
                        errorMessager = string.Format("当前状态【{0}】不允许登记爽约", viewNMPAppointment.StatusEnum.ToCnName());
                        return false;
                    }
                }
                if (viewNMPAppointment.IsNeedTutorAudit && viewNMPAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才登记爽约";
                    return false;
                }
                if (viewNMPAppointment.IsNeedAudit && viewNMPAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才登记爽约";
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableAddUsedConfirm(IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            return JudgeIsEnableAddUsedConfirm(viewNMPAppointments, nmpAppointmentPriviliges, operatorId, false, out errorMessager);
        }
        public bool JudgeIsEnableSelfAddUsedConfirm(IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            return JudgeIsEnableAddUsedConfirm(viewNMPAppointments, nmpAppointmentPriviliges, operatorId, true, out errorMessager);
        }
        public bool JudgeIsEnableCancelRegisterBreakAppointment(IEnumerable<ViewNMPAppointment> viewNMPAppointments, object nmpAppointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            var lstNMPAppointmentPrivilige = (IList<NMPAppointmentPrivilige>)nmpAppointmentPriviliges;
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                var nmpAppointmentPrivilige = lstNMPAppointmentPrivilige.FirstOrDefault(p => p.NMPAppointmentId.HasValue && p.NMPAppointmentId == viewNMPAppointment.Id);
                if (nmpAppointmentPrivilige == null || !nmpAppointmentPrivilige.IsEnableCancelRegisterBreakAppointmentOperate)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                if (viewNMPAppointment.StatusEnum != AppointmentStatus.Fail)
                {
                    errorMessager = string.Format("当前状态【{0}】不允许取消爽约", viewNMPAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewNMPAppointment.IsNeedTutorAudit && viewNMPAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才取消爽约";
                    return false;
                }
                if (viewNMPAppointment.IsNeedAudit && viewNMPAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才取消爽约";
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableTutorAudit(bool isRigidlyCheck, TutorAuditStatus? tutorAuditStatus, IEnumerable<ViewNMPAppointment> viewNMPAppointments, IEnumerable<NMP> nmps, Guid auditTutorId, out string errorMessage)
        {
            errorMessage = "";
            IUserBLL userBLL = BLLFactory.CreateUserBLL();
            var tutor = userBLL.GetEntityById(auditTutorId);
            if (tutor == null)
            {
                errorMessage = "出错,找不到导师信息";
                return false;
            }
            if (tutor.TutorId.HasValue)
            {
                errorMessage = string.Format("出错,用户【{0}】不是导师", tutor.UserName);
                return false;
            }
            foreach (var viewNMPAppointment in viewNMPAppointments)
            {
                var appointmentInfo = string.Format("预约信息:预约人:{0},工位设备:{1},预约时间:{2}", viewNMPAppointment.UserName, viewNMPAppointment.NMPEquipmentName, viewNMPAppointment.BeginTimeStr + "～" + viewNMPAppointment.EndTimeStr);
                if (!viewNMPAppointment.IsNeedTutorAudit)
                {
                    errorMessage = string.Format("出错,{0}无需导师审核", appointmentInfo);
                    return false;
                }
                if (viewNMPAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessage = string.Format("出错,{0},当前状态【{1}】,不能进行导师审核操作", appointmentInfo, viewNMPAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewNMPAppointment.TutorId != auditTutorId)
                {
                    errorMessage = string.Format("出错,您不能审核{0},您不是该申请人的导师", appointmentInfo);
                    return false;
                }
                if (viewNMPAppointment.IsNeedAudit && viewNMPAppointment.AuditStatusEnum != AppointmentAuditStatus.UnAudit)
                {
                    errorMessage = string.Format("出错,您不能审核{0},管理员已经审核", appointmentInfo);
                    return false;
                }
                var nmp = nmps.First(p => p.Id == viewNMPAppointment.NMPId);
                if (viewNMPAppointment.BeginTime.Value.AddMinutes( nmp.MinAppointmentCancelTime) < DateTime.Now)
                {
                    errorMessage = appointmentInfo + ",预约已经失效,不能进行审核操作";
                    return false;
                }
                if (isRigidlyCheck && tutorAuditStatus.HasValue && tutorAuditStatus.Value == TutorAuditStatus.Passed)
                {
                    var nmpAppointment = GetPeriodValidateNMPAppointment(viewNMPAppointment.Id, viewNMPAppointment.NMPEquipmentId.Value, viewNMPAppointment.BeginTime.Value, viewNMPAppointment.EndTime.Value);
                    if (nmpAppointment != null && DateTimeUtility.IsContain(viewNMPAppointment.BeginTime.Value, viewNMPAppointment.EndTime.Value, nmpAppointment.BeginTime.Value, nmpAppointment.EndTime.Value))
                    {
                        errorMessage = string.Format("出错,该时间段已经被{0}预约并生效", nmpAppointment.User.UserName);
                        return false;
                    }
                }

            }
            return true;
        }
    }
}
