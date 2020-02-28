using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using com.august.DataLink;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.august.Core.XQuery;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentBLL : BLLBase<Appointment>, IAppointmentBLL
    {
        private IUserBLL __userBLL;
        private IUserAccountBLL __userAccountBLL;
        private IEquipmentPartBLL __equipmentPartBLL;
        private IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        private ICostDeductBLL __costDeductBLL;
        private IEquipmentBLL __equipmentBLL;
        private IPunishActionBLL __punishActionBLL;
        private IPunishRecordBLL __punishRecordBLL;
        private IEquipmentBlackListBLL __equipmentBlackListBLL;
        private IEquipmentNoticeBLL __equipmentNoticeBLL;
        private IEquipmentTranningBLL __equipmentTranningBLL;
        private IUserEquipmentBLL __userEquipmentBLL;
        private IUsedConfirmBLL __usedConfirmBLL;
        private IUserExaminationBLL __userExaminationBLL;
        private IAppointmentEquipmentAddtionChargeItemBLL __appointmentEquipmentAddtionChargeItemBLL;
        private IAppointmentEquipmentPartBLL __appointmentEquipmentPartBLL;
        private IAppointmentEquipmentUseConditionBLL __appointmentEquipmentUseConditionBLL;
        private IViewAppointmentEquipmentUseConditionBLL __viewAppointmentEquipmentUseConditionBLL;

        public AppointmentBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __equipmentPartBLL = BLLFactory.CreateEquipmentPartBLL();
            __delinquencyConfirmBLL = BLLFactory.CreateDelinquencyConfirmBLL();
            __costDeductBLL = BLLFactory.CreateCostDeductBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
            __equipmentBlackListBLL = BLLFactory.CreateEquipmentBlackListBLL();
            __equipmentNoticeBLL = BLLFactory.CreateEquipmentNoticeBLL();
            __equipmentTranningBLL = BLLFactory.CreateEquipmentTranningBLL();
            __userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
            __appointmentEquipmentAddtionChargeItemBLL = BLLFactory.CreateAppointmentEquipmentAddtionChargeItemBLL();
            __appointmentEquipmentPartBLL = BLLFactory.CreateAppointmentEquipmentPartBLL();
            __appointmentEquipmentUseConditionBLL = BLLFactory.CreateAppointmentEquipmentUseConditionBLL();
            __viewAppointmentEquipmentUseConditionBLL = BLLFactory.CreateViewAppointmentEquipmentUseConditionBLL();
        }
        public override bool Add(IEnumerable<Appointment> entities, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null)tran = SessionManager.Instance.BeginTransaction();
                base.Add(entities, ref tran, isSupress);
                foreach (var entity in entities)
                {
                    if (entity.AppointmentEquipmentAddtionChargeItems != null && entity.AppointmentEquipmentAddtionChargeItems.Count > 0)
                        __appointmentEquipmentAddtionChargeItemBLL.Add(entity.AppointmentEquipmentAddtionChargeItems, ref tran, isSupress);
                    if (entity.AppointmentEquipmentParts != null && entity.AppointmentEquipmentParts.Count > 0)
                        __appointmentEquipmentPartBLL.Add(entity.AppointmentEquipmentParts, ref tran, isSupress);
                    if (entity.AppointmentEquipmentUseConditions != null && entity.AppointmentEquipmentUseConditions.Count > 0)
                        __appointmentEquipmentUseConditionBLL.Add(entity.AppointmentEquipmentUseConditions, ref tran, isSupress);
                       
                }
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public override bool Delete(IEnumerable<Guid> ids, ref XTransaction tran, bool isSupress = false)
        {
            try
            {
                if (!isSupress && tran != null)tran = SessionManager.Instance.BeginTransaction();
                var appointmentEquipmentAddtionChargeItemList = __appointmentEquipmentAddtionChargeItemBLL.GetEntitiesByExpression(ids.ToFormatStr("AppointmentId"));
                if (appointmentEquipmentAddtionChargeItemList != null && appointmentEquipmentAddtionChargeItemList.Count > 0)
                    __appointmentEquipmentAddtionChargeItemBLL.Delete(appointmentEquipmentAddtionChargeItemList.Select(p => p.Id), ref tran, isSupress);
                var appointmentEquipmentPartList = __appointmentEquipmentPartBLL.GetEntitiesByExpression(ids.ToFormatStr("AppointmentId"));
                if (appointmentEquipmentPartList != null && appointmentEquipmentPartList.Count > 0)
                    __appointmentEquipmentPartBLL.Delete(appointmentEquipmentPartList.Select(p => p.Id), ref tran, isSupress);
                var appointmentEquipmentUseConditionList = __appointmentEquipmentUseConditionBLL.GetEntitiesByExpression(ids.ToFormatStr("AppointmentId"));
                if (appointmentEquipmentUseConditionList != null && appointmentEquipmentUseConditionList.Count > 0)
                    __appointmentEquipmentUseConditionBLL.Delete(appointmentEquipmentUseConditionList.Select(p => p.Id), ref tran, isSupress);
                base.Delete(ids, ref tran, isSupress);
                if (!isSupress) tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); } 
        }
        public object GetAppointmentPriviliges(Guid? userId, IList<ViewAppointment> viewAppointmentList)
        {
            IList<AppointmentPrivilige> lstAppointmentPriviliges = new List<AppointmentPrivilige>();
            if (userId.HasValue && viewAppointmentList != null && viewAppointmentList.Count > 0)
            {
                foreach (ViewAppointment viewAppoinment in viewAppointmentList)
                {
                    AppointmentPrivilige appointmentPrivilige = lstAppointmentPriviliges.FirstOrDefault(p => p.AppointmentId.HasValue && p.AppointmentId.Value == viewAppoinment.Id);
                    if (appointmentPrivilige == null)
                    {
                        appointmentPrivilige = PriviligeFactory.GetAppointmentPrivilige(userId.Value,viewAppoinment);
                        lstAppointmentPriviliges.Add(appointmentPrivilige);
                    }
                }
            }
            return lstAppointmentPriviliges;
        }
        public bool JudgeIsExistTheEquipmentPartName(Guid[] equipmentPartIds,string equipmentPartName)
        {
            if (equipmentPartIds == null || equipmentPartIds.Count() == 0) return false;
            if (string.IsNullOrWhiteSpace(equipmentPartName)) return false;
            var equipmentPartList = __equipmentPartBLL.GetEntitiesByExpression(equipmentPartIds.ToFormatStr());
            if (equipmentPartList == null || equipmentPartList.Count == 0) return false;
            return equipmentPartList.Any(p => p.Name == equipmentPartName);
        }
        private bool JudgeIsEnableAppoitmentByPXOfSameRoom(DateTime beginTime, DateTime endTime, Equipment equipment, IList<ViewAppointmentEquipmentUseCondition> viewAppointmentEquipmentUseConditionList, IList<AppointmentEquipmentUseCondition> appointmentEquipmentUseConditionList, out string errorMessage)
        {
            errorMessage = "";
            if (viewAppointmentEquipmentUseConditionList == null || viewAppointmentEquipmentUseConditionList.Count == 0) return true;
            //处理同一房间独占情况
            var viewAppointmentEquipmentUseCondition = viewAppointmentEquipmentUseConditionList.FirstOrDefault(p => DateTimeUtility.IsContain(beginTime,endTime,p.AppointmentBeginTime.Value,p.AppointmentEndTime.Value) && p.IsAppointmentAsVirtualSpace.HasValue && p.IsAppointmentAsVirtualSpace.Value);
            if (viewAppointmentEquipmentUseCondition != null)
            {
                errorMessage = string.Format("用户【{0}】已经独占预约了【{1}】,预约时间从【{2}】到【{3}】,不能预约该时间段内【{1}】以及【{1}】下的设备",
                                                 viewAppointmentEquipmentUseCondition.AppointmentUserName,
                                                 viewAppointmentEquipmentUseCondition.EquipmentName,
                                                 viewAppointmentEquipmentUseCondition.AppointmentBeginTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                 viewAppointmentEquipmentUseCondition.AppointmentEndTime.Value.ToString("yyyy-MM-dd HH:mm")
                                                ); 
                return false;
            }
            //处理同一房间非独占情况
            if (appointmentEquipmentUseConditionList != null && appointmentEquipmentUseConditionList.Count > 0 && viewAppointmentEquipmentUseConditionList!=null && viewAppointmentEquipmentUseConditionList.Count>0)
            {
                foreach (var appointmentEquipmentUseCondition in appointmentEquipmentUseConditionList)
                {
                    if (!equipment.IsAppointmentAsVirtualSpace)
                    {
                        var result = JudgeIsEnableAppoitmentByPX(beginTime, endTime, appointmentEquipmentUseConditionList, viewAppointmentEquipmentUseConditionList, out errorMessage);
                        if (!result) return result;
                    }
                    else
                    {
                        viewAppointmentEquipmentUseCondition = viewAppointmentEquipmentUseConditionList.FirstOrDefault(p => DateTimeUtility.IsContain(beginTime, endTime, p.AppointmentBeginTime.Value, p.AppointmentEndTime.Value));
                        if (viewAppointmentEquipmentUseCondition != null)
                        {
                            errorMessage = string.Format("该时间段不能独占此空间【{1}】，因为该空间已存在用户【{0}】的预约,预约时间从【{2}】到【{3}】",
                                                     viewAppointmentEquipmentUseCondition.AppointmentUserName,
                                                     viewAppointmentEquipmentUseCondition.EquipmentName,
                                                     viewAppointmentEquipmentUseCondition.AppointmentBeginTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                     viewAppointmentEquipmentUseCondition.AppointmentEndTime.Value.ToString("yyyy-MM-dd HH:mm")
                                                    );
                        }
                        return false;
                    }
                }
            }
            return true;
        }
        private bool JudgeIsEnableAppoitmentByPX(DateTime beginTime, DateTime endTime, IList<AppointmentEquipmentUseCondition> appointmentEquipmentUseConditionList, IList<ViewAppointmentEquipmentUseCondition> viewAppointmentEquipmentUseConditionList, out string errorMessage)
        {
            errorMessage = "";
            if (appointmentEquipmentUseConditionList != null && appointmentEquipmentUseConditionList.Count > 0 &&
                viewAppointmentEquipmentUseConditionList != null && viewAppointmentEquipmentUseConditionList.Count > 0)
            {
                foreach (var appointmentEquipmentUseCondition in appointmentEquipmentUseConditionList)
                {
                    var viewAppointmentEquipmentUseCondition = viewAppointmentEquipmentUseConditionList.FirstOrDefault(p => DateTimeUtility.IsContain(beginTime, endTime, p.AppointmentBeginTime.Value, p.AppointmentEndTime.Value) &&
                          p.EquipmentUseConditionName == appointmentEquipmentUseCondition.EquipmentUseCondition.Name &&
                          p.EquipmentUseConditionVal != appointmentEquipmentUseCondition.Val);
                    if (viewAppointmentEquipmentUseCondition != null)
                    {
                        errorMessage = string.Format("用户【{0}】预约了【{1}】,预约时间从【{2}】到【{3}】,您当前的【{4}】为{5}与【{0}】的{6}不同,不能预约该时间段",
                                                       viewAppointmentEquipmentUseCondition.AppointmentUserName,
                                                       viewAppointmentEquipmentUseCondition.EquipmentName,
                                                       viewAppointmentEquipmentUseCondition.AppointmentBeginTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                       viewAppointmentEquipmentUseCondition.AppointmentEndTime.Value.ToString("yyyy-MM-dd HH:mm"),
                                                       viewAppointmentEquipmentUseCondition.EquipmentUseConditionName, appointmentEquipmentUseCondition.Val,
                                                       viewAppointmentEquipmentUseCondition.EquipmentUseConditionVal
                                                      );
                    }
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableAppoitmentByPX(Equipment equipment, DateTime beginTime, DateTime endTime, Guid roomId, Appointment appointment, out string errorMessage)
        {
            errorMessage = "";
            List<Guid> equipmentIds = new List<Guid>() { equipment.Id };
            var basicQueryExpression = string.Format("AppointmentEndTime>\"{0}\"*AppointmentBeginTime<\"{1}\"*AppointmentStatus={2}*((AppointmentIsNeedAudit=false+AppointmentIsNeedAudit=null)+(AppointmentIsNeedAudit=true*AppointmentAuditingStatus=-{3}))", beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.NotPass);
            var queryExpression = string.Format("EquipmentRoomId=\"{0}\"*",roomId)+basicQueryExpression;
            var viewAppointmentEquipmentUseConditionList = __viewAppointmentEquipmentUseConditionBLL.GetEntitiesByExpression(queryExpression, null, "AppointmentBeginTime");
            //处理同一房间情况
            var result = JudgeIsEnableAppoitmentByPXOfSameRoom(beginTime, endTime, equipment, viewAppointmentEquipmentUseConditionList, appointment != null ? appointment.AppointmentEquipmentUseConditions : null, out errorMessage);
            if (!result) return result;
            //处理不同房间情况
            if (appointment!=null && appointment.AppointmentEquipmentUseConditions != null && appointment.AppointmentEquipmentUseConditions.Count > 0)
            {
                queryExpression = string.Format("EquipmentRoomId=-\"{0}\"*AppointmentUserId=\"{1}\"*", roomId,appointment.UserId.Value) + basicQueryExpression + "*" + appointment.AppointmentEquipmentUseConditions.Select(p => p.EquipmentUseCondition.Name).ToFormatStr("EquipmentUseConditionName");
                viewAppointmentEquipmentUseConditionList = __viewAppointmentEquipmentUseConditionBLL.GetEntitiesByExpression(queryExpression, null, "AppointmentBeginTime");
                return JudgeIsEnableAppoitmentByPX(beginTime, endTime, appointment.AppointmentEquipmentUseConditions, viewAppointmentEquipmentUseConditionList, out errorMessage);
            }
            return true;
        }
        private bool JudgeIsEnableCancelorChange(ViewAppointment viewAppointment, AppointmentPrivilige appointmentPrivilige, Equipment equipment, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            if (viewAppointment.StatusEnum != AppointmentStatus.Waitting)
            {
                errorMessager = string.Format("当前状态【{0}】不能进行取消操作", viewAppointment.StatusEnum.ToCnName());
                return false;
            }
            if ((viewAppointment.EndTime.Value < DateTime.Now))
            {
                errorMessager = "已结束,不能进行取消操作";
                return false;
            }
            if (!equipment.IsEnableCancelNotOverAppointment)
            {
                if (viewAppointment.BeginTime.Value <= DateTime.Now)
                {
                    errorMessager = "该预约开始时间大于当前时间,不能进行取消操作";
                    return false;
                }
                if (viewAppointment.BeginTime.Value < DateTime.Now.AddMinutes(!equipment.MinAppointmentCancelTime.HasValue ? 0 : equipment.MinAppointmentCancelTime.Value))
                {
                    errorMessager = "过了提前改约时间,不能进行取消操作";
                    return false;
                }
            }
            if (!equipment.IsEnableCancelCheckedAppointment)
            {
                if (viewAppointment.IsNeedAudit.HasValue && viewAppointment.IsNeedAudit.Value && viewAppointment.AuditStatusEnum != AppointmentAuditStatus.UnAudit)
                {
                    errorMessager = "已审核,不能进行取消操作";
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableChange(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, IEnumerable<Equipment> equipments, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            if (viewAppointments == null || viewAppointments.Count() == 0)
            {
                errorMessager = "预约信息为空";
                return false;
            }
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                var equipment = equipments.First(p => p.Id == viewAppointment.EquipmentId.Value);
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.Value == viewAppointment.Id);
                if (operatorId.HasValue && appointmentPrivilige == null)
                {
                    appointmentPrivilige = PriviligeFactory.GetAppointmentPrivilige(operatorId.Value, viewAppointment);
                    if (appointmentPrivilige != null) lstAppointmentPrivilige.Add(appointmentPrivilige);
                }
                if (!appointmentPrivilige.IsEnabelChangeAppointment)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                var customer = CustomerFactory.GetCustomer();
                if (!customer.GetIsAdminEnableChangeAppointmentOfOtherUser() &&　operatorId.HasValue && operatorId.Value != viewAppointment.UserId.Value)
                {
                    errorMessager = "不能改约非自己的预约";
                    return false;
                }
                if (!JudgeIsEnableCancelorChange(viewAppointment, appointmentPrivilige, equipment, operatorId, out errorMessager))
                {
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableCancel(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, IEnumerable<Equipment> equipments, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            if (viewAppointments == null || viewAppointments.Count() == 0)
            {
                errorMessager = "预约信息为空";
                return false;
            }
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                var equipment = equipments.First(p => p.Id == viewAppointment.EquipmentId.Value);
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.Value == viewAppointment.Id);
                if (operatorId.HasValue && appointmentPrivilige == null)
                {
                    appointmentPrivilige = PriviligeFactory.GetAppointmentPrivilige(operatorId.Value, viewAppointment);
                    if (appointmentPrivilige != null) lstAppointmentPrivilige.Add(appointmentPrivilige);
                }
                if (!appointmentPrivilige.IsEnableCancelAppointment)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                var customer = CustomerFactory.GetCustomer();
                if (!customer.GetIsAdminEnableCancelAppointmentOfOtherUser()　&& operatorId.HasValue && operatorId.Value != viewAppointment.UserId.Value)
                {
                    errorMessager = "不能取消非自己的预约";
                    return false;
                }
                if (!JudgeIsEnableCancelorChange(viewAppointment, appointmentPrivilige, equipment, operatorId, out errorMessager))
                {
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableAudit(bool isRigidlyCheck, AppointmentAuditStatus? auditStatus, IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, IEnumerable<Equipment> equipments, Guid? operatorId, out string errorMessage)
        {
            errorMessage = "";
            if (viewAppointments == null || viewAppointments.Count()==0)
            {
                errorMessage = "预约信息为空";
                return false;
            }
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                var appointmentInfo = string.Format("预约者【{0}】,预约设备【{1}】,预约时间【{2}~{3}】", viewAppointment.UserName, viewAppointment.EquipmentName, viewAppointment.BeginTimeStr, viewAppointment.EndTimeStr);
                if (viewAppointment.IsNeedTutorAudit && viewAppointment.TutorAuditStatusEnum!= TutorAuditStatus.Passed)
                {
                    errorMessage = appointmentInfo + "需要导师审核通过才能进行审核";
                    return false;
                }
                if (viewAppointment.HasClossingAccount)
                {
                    errorMessage = appointmentInfo + "已结算";
                    return false;
                }
                if (!viewAppointment.IsNeedAudit.HasValue || !viewAppointment.IsNeedAudit.Value)
                {
                    errorMessage = appointmentInfo + "无需审核"; 
                    return false;
                }
                if (viewAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessage = appointmentInfo + string.Format(",预约状态【{0}】不能进行审核操作", viewAppointment.StatusEnum.ToCnName());
                    return false;
                }
                var equipment = equipments.First(p => p.Id == viewAppointment.EquipmentId.Value);
                var customer = CustomerFactory.GetCustomer();
                var isCheckAppointmentCancelTime = customer.GetIsCheckAppointmentCancelTime();
                if (isCheckAppointmentCancelTime && (viewAppointment.BeginTime.Value.AddMinutes(!equipment.MinAppointmentCancelTime.HasValue ? 0 : equipment.MinAppointmentCancelTime.Value) < DateTime.Now))
                {
                    errorMessage = appointmentInfo + ",预约已经失效,不能进行审核操作";
                    return false;
                }
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.Value == viewAppointment.Id);
                if (operatorId.HasValue && appointmentPrivilige == null)
                {
                    appointmentPrivilige = PriviligeFactory.GetAppointmentPrivilige(operatorId.Value, viewAppointment);
                    if (appointmentPrivilige != null)lstAppointmentPrivilige.Add(appointmentPrivilige);
                }
                if (appointmentPrivilige == null || !appointmentPrivilige.IsEnableAuditAppointment)
                {
                    errorMessage = appointmentInfo + ",无审核权限,不能进行审核操作";
                    return false;
                }
                if (isRigidlyCheck && auditStatus.HasValue && auditStatus.Value== AppointmentAuditStatus.Pass)
                {
                    var appointment = GetPeriodValidateAppointment(viewAppointment.Id, equipment.Id, viewAppointment.BeginTime.Value, viewAppointment.EndTime.Value);
                    if (appointment != null && DateTimeUtility.IsContain(viewAppointment.BeginTime.Value,viewAppointment.EndTime.Value,appointment.BeginTime.Value,appointment.EndTime.Value))
                    {
                        errorMessage = string.Format("出错,该时间段已经被{0}预约并生效", appointment.User.UserName);
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary> 判断是否允许爽约 </summary>
        /// <param name="isAutoOperate"></param>
        /// <param name="viewAppointments"></param>
        /// <param name="appointmentPriviliges"></param>
        /// <param name="operatorId"></param>
        /// <param name="errorMessager"></param>
        /// <returns></returns>
        public bool JudgeIsEnableRegisterBreakAppointment(bool isAutoOperate,IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.HasValue && p.AppointmentId == viewAppointment.Id);
                if (appointmentPrivilige == null || !appointmentPrivilige.IsEnableRegisterBreakAppointment)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                if (!isAutoOperate)
                {
                    if (viewAppointment.StatusEnum != AppointmentStatus.Waitting)
                    {
                        errorMessager = string.Format("当前状态【{0}】不允许登记爽约", viewAppointment.StatusEnum.ToCnName());
                        return false;
                    }
                    if (viewAppointment.EndTime.Value > DateTime.Now)
                    {
                        errorMessager = string.Format("预约结束时间【{0}】小于当前时间【{1}】不允许登记爽约", viewAppointment.EndTimeStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        return false;
                    }
                }
                else
                {
                    if (viewAppointment.StatusEnum != AppointmentStatus.Cancel)
                    {
                        errorMessager = string.Format("当前状态【{0}】不允许登记爽约", viewAppointment.StatusEnum.ToCnName());
                        return false;
                    }
                }
                if (viewAppointment.IsNeedTutorAudit && viewAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才登记爽约";
                    return false;
                }
                if (viewAppointment.IsNeedAudit.HasValue && viewAppointment.IsNeedAudit.Value && viewAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才登记爽约";
                    return false;
                }
            }
            return true;
        }
        private bool JudgeIsEnableAddUsedConfirm(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, Guid? operatorId, bool isSelfOperate,out string errorMessager)
        {
            errorMessager = "";
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                if (viewAppointment.HasClossingAccount)
                {
                    errorMessager = "已结算";
                    return false;
                }
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.HasValue && p.AppointmentId == viewAppointment.Id);
                if (appointmentPrivilige == null || (!isSelfOperate && !appointmentPrivilige.IsEnableAddAppointmentUsedConfirm) ||  (isSelfOperate && !appointmentPrivilige.IsEnableSelfAddAppointmentUsedConfirm))
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                
                if (viewAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessager = string.Format("当前状态【{0}】不允许登记", viewAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAppointment.EndTime.Value > DateTime.Now)
                {
                    errorMessager = string.Format("预约结束时间【{0}】大于当前时间【{1}】不允许登记", viewAppointment.EndTimeStr, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    return false;
                }
                if (viewAppointment.IsNeedTutorAudit && viewAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才允许登记";
                    return false;
                }
                if (viewAppointment.IsNeedAudit.HasValue && viewAppointment.IsNeedAudit.Value && viewAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才允许登记";
                    return false;
                }
            }
            return true;
        } 
        public bool JudgeIsEnableAddUsedConfirm(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            return JudgeIsEnableAddUsedConfirm(viewAppointments, appointmentPriviliges, operatorId, false,out errorMessager);
        }
        public bool JudgeIsEnableSelfAddUsedConfirm(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            return JudgeIsEnableAddUsedConfirm(viewAppointments, appointmentPriviliges, operatorId, true, out errorMessager);
        }
        public bool JudgeIsEnableCancelRegisterBreakAppointment(IEnumerable<ViewAppointment> viewAppointments, object appointmentPriviliges, Guid? operatorId, out string errorMessager)
        {
            errorMessager = "";
            var lstAppointmentPrivilige = (IList<AppointmentPrivilige>)appointmentPriviliges;
            foreach (var viewAppointment in viewAppointments)
            {
                var appointmentPrivilige = lstAppointmentPrivilige.FirstOrDefault(p => p.AppointmentId.HasValue && p.AppointmentId == viewAppointment.Id);
                if (appointmentPrivilige == null || !appointmentPrivilige.IsEnableCancelRegisterBreakAppointmentOperate)
                {
                    errorMessager = "无操作权限";
                    return false;
                }
                if (viewAppointment.StatusEnum != AppointmentStatus.Fail)
                {
                    errorMessager = string.Format("当前状态【{0}】不允许取消爽约", viewAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if (viewAppointment.IsNeedTutorAudit && viewAppointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
                {
                    errorMessager = "预约导师需要审核通过才取消爽约";
                    return false;
                }
                if (viewAppointment.IsNeedAudit.HasValue && viewAppointment.IsNeedAudit.Value && viewAppointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
                {
                    errorMessager = "预约需要管理员审核通过才取消爽约";
                    return false;
                }
            }
            return true;
        }
        public bool JudgeIsEnableTutorAudit(bool isRigidlyCheck, TutorAuditStatus? tutorAuditStatus, IEnumerable<ViewAppointment> viewAppointments, IEnumerable<Equipment> equipments, Guid auditTutorId, out string errorMessage)
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
            foreach (var viewAppointment in viewAppointments)
            {
                var appointmentInfo = string.Format("预约信息:预约人:{0},设备:{1},预约时间:{2}", viewAppointment.UserName, viewAppointment.EquipmentName, viewAppointment.BeginTimeStr + "～" + viewAppointment.EndTimeStr);
                if (!viewAppointment.IsNeedTutorAudit)
                {
                    errorMessage = string.Format("出错,{0}无需导师审核", appointmentInfo);
                    return false;
                }
                if (viewAppointment.StatusEnum != AppointmentStatus.Waitting)
                {
                    errorMessage = string.Format("出错,{0},当前状态【{1}】,不能进行导师审核操作", appointmentInfo, viewAppointment.StatusEnum.ToCnName());
                    return false;
                }
                if ( viewAppointment.TutorId != auditTutorId)
                {
                    errorMessage = string.Format("出错,您不能审核{0},您不是该申请人的导师", appointmentInfo);
                    return false;
                }
                if (viewAppointment.IsNeedAudit.HasValue && viewAppointment.IsNeedAudit.Value && viewAppointment.AuditStatusEnum != AppointmentAuditStatus.UnAudit)
                {
                    errorMessage = string.Format("出错,您不能审核{0},管理员已经审核", appointmentInfo);
                    return false;
                }
                //if (viewAppointment.TutorAuditStatusEnum != TutorAuditStatus.WaitingAudit)
                //{
                //    errorMessage = string.Format("出错,{0}已经审核过，不能重复审核", appointmentInfo);
                //    return false;
                //}
                var equipment = equipments.First(p => p.Id == viewAppointment.EquipmentId.Value);
                if (viewAppointment.BeginTime.Value.AddMinutes(!equipment.MinAppointmentCancelTime.HasValue ? 0 : equipment.MinAppointmentCancelTime.Value) < DateTime.Now)
                {
                    errorMessage = appointmentInfo + ",预约已经失效,不能进行审核操作";
                    return false;
                }
                if (isRigidlyCheck && tutorAuditStatus.HasValue && tutorAuditStatus.Value== TutorAuditStatus.Passed)
                {
                    var appointment = GetPeriodValidateAppointment(viewAppointment.Id, equipment.Id, viewAppointment.BeginTime.Value, viewAppointment.EndTime.Value);
                    if (appointment != null && DateTimeUtility.IsContain(viewAppointment.BeginTime.Value, viewAppointment.EndTime.Value, appointment.BeginTime.Value, appointment.EndTime.Value))
                    {
                        errorMessage = string.Format("出错,该时间段已经被{0}预约并生效", appointment.User.UserName);
                        return false;
                    }
                }

            }
            return true;
        }
        private Appointment GetPeriodValidateAppointment(Guid? appointmentId, Guid equipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = GetPeriodValidateAppointmentQueryExpression(appointmentId, equipmentId, beginAt, endAt);
            if (appointmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*Id=-\"{0}\"", appointmentId.Value);
            var appointment = GetFirstOrDefaultEntityByExpression(periodValidateAppointmentQueryExpression);
            return appointment;
        }
        public string GetPeriodPossiblyValidateAppointmentQueryExpression(Guid? equipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = string.Format("BeginTime>\"{0}\"*BeginTime<\"{1}\"*Status={2}*AuditingStatus=-{3}*TutorAuditStatus=-{4}", beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Waitting, (int)AppointmentAuditStatus.NotPass, (int)TutorAuditStatus.Refused);
            if (equipmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*EquipmentId=\"{0}\"", equipmentId.Value);
            return periodValidateAppointmentQueryExpression;
        }
        public string GetPeriodValidateAppointmentQueryExpression(Guid? appointmentId, Guid? equipmentId, DateTime beginAt, DateTime endAt)
        {
            var auditExpression = string.Format("*((IsNeedAudit=true*AuditingStatus={0}*IsNeedTutorAudit=true*TutorAuditStatus={1})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={0})+(IsNeedAudit=false*IsNeedTutorAudit=true*TutorAuditStatus={1})+(IsNeedTutorAudit=false*IsNeedAudit=false))", (int)AppointmentAuditStatus.Pass, (int)TutorAuditStatus.Passed);
            var periodValidateAppointmentQueryExpression = string.Format("EndTime>\"{0}\"*BeginTime<\"{1}\"*Status=-{2}*Status=-{3}*Status=-{4}",beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss"), (int)AppointmentStatus.Cancel, (int)AppointmentStatus.Changed, (int)AppointmentStatus.Fail);
            if (appointmentId.HasValue) periodValidateAppointmentQueryExpression += string.Format("*Id=-\"{0}\"", appointmentId.Value);
            if (equipmentId.HasValue)
            {
                periodValidateAppointmentQueryExpression += string.Format("*EquipmentId=\"{0}\"", equipmentId.Value);
                var equipment = __equipmentBLL.GetEntityById(equipmentId.Value);
                if (!equipment.IsEnableAppointmentBeforeTutorAudit && !equipment.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus=-{0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus=-{1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus=-{0}*AuditingStatus=-{1}))", (int)TutorAuditStatus.Refused, (int)AppointmentAuditStatus.NotPass);
                }
                if (!equipment.IsEnableAppointmentBeforeTutorAudit && equipment.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus=-{0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus={1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus=-{0}*AuditingStatus={1}))", (int)TutorAuditStatus.Refused, (int)AppointmentAuditStatus.Pass);
                }
                if (equipment.IsEnableAppointmentBeforeTutorAudit && !equipment.IsEnableAppointmentBeforeAdminAudit)
                {
                    auditExpression = string.Format("*((IsNeedAudit=false*IsNeedTutorAudit=false)+(IsNeedTutorAudit=true*IsNeedAudit=false*TutorAuditStatus={0})+(IsNeedTutorAudit=false*IsNeedAudit=true*AuditingStatus=-{1})+(IsNeedAudit=true*IsNeedTutorAudit=true*TutorAuditStatus={0}*AuditingStatus=-{1}))", (int)TutorAuditStatus.Passed, (int)AppointmentAuditStatus.NotPass);
                }
            }
            periodValidateAppointmentQueryExpression += auditExpression;
            return periodValidateAppointmentQueryExpression;
        }
        public IList<Appointment> GetPeriodValidateAppointmentList(Guid equipmentId, DateTime beginAt, DateTime endAt)
        {
            var periodValidateAppointmentQueryExpression = GetPeriodValidateAppointmentQueryExpression(null,equipmentId, beginAt, endAt);
            return GetEntitiesByExpression(periodValidateAppointmentQueryExpression);
        }
        public IList<Appointment> GetTutorPeriodAppointmentList(Guid equipmentId, Guid tutorId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateAppointmentQueryExpression(equipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("PayerId=\"{0}\"*{1}", tutorId, periodPossiblyValidateAppointmentQueryExpression));
        }
        public IList<Appointment> GetUserPeriodAppointmentList(Guid equipmentId, Guid userId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateAppointmentQueryExpression(equipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("UserId=\"{0}\"*{1}", userId, periodPossiblyValidateAppointmentQueryExpression));
        }
        public IList<Appointment> GetSubjectPeriodAppointmentList(Guid equipmentId, Guid subjectId, DateTime beginAt, DateTime endAt)
        {
            var periodPossiblyValidateAppointmentQueryExpression = GetPeriodPossiblyValidateAppointmentQueryExpression(equipmentId, beginAt, endAt);
            return GetEntitiesByExpression(string.Format("SubjectId=\"{0}\"*{1}", subjectId, periodPossiblyValidateAppointmentQueryExpression));
        }
        /// <summary> 爽约 </summary>
        /// <param name="isAutoOperate"></param>
        /// <param name="appointment"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public DelinquencyRule RegisterBreakAppointment(bool isAutoOperate,Appointment appointment, Guid operatorId)
        {
            // chao: 爽约，是否需要在预扣费模式下作费用对冲（目前不对冲），涉及撤销爽约
            string errorMessage = "";
            var viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
            var viewAppointmentList = viewAppointmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", appointment.Id), null, "", false, false, true, new string[] { "EquipmentAdminId", "ExperimentationContent" });
            var appointmentPriviligeList = (IList<AppointmentPrivilige>)GetAppointmentPriviliges(operatorId, viewAppointmentList);
            if (!JudgeIsEnableRegisterBreakAppointment(isAutoOperate,viewAppointmentList, appointmentPriviligeList, operatorId, out errorMessage)) throw new Exception(errorMessage);
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                appointment.IsUseable = false;
                appointment.WhyUnuseable = "爽约";
                appointment.StatusEnum = Model.Enum.AppointmentStatus.Fail;
                Save(new Appointment[] { appointment }, ref tran, true);
                var delinquencyConfirm = __delinquencyConfirmBLL.GenerateEquipmentAppointmentDelinquencyConfirmData(appointment.Id, operatorId, appointment.EndTime.Value);
                var delinquencyRule = __delinquencyConfirmBLL.AddDelinquencyConfirm(delinquencyConfirm, appointment.User, ref tran);
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
        /// <summary> 爽约 </summary>
        /// <param name="isAutoOperate"></param>
        /// <param name="appointmentId"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public DelinquencyRule RegisterBreakAppointment(bool isAutoOperate, Guid appointmentId, Guid operatorId)
        {
            var appointment = GetEntityById(appointmentId);
            if (appointment == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的预约信息", appointmentId.ToString()));
            return RegisterBreakAppointment(isAutoOperate,appointment, operatorId);
        }
        /// <summary> 取消爽约 </summary>
        /// <param name="delinquencyConfirmId"></param>
        /// <param name="content"></param>
        /// <param name="user"></param>
        public void CancelRegisterBreakAppointment(Guid delinquencyConfirmId, string content, User user)
        {
            XTransaction tran =SessionManager.Instance.BeginTransaction();
            try
            {
                string errorMessage = "";
                var viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
                var delinquencyConfirm = __delinquencyConfirmBLL.GetEntityById(delinquencyConfirmId);
                var appointment = GetEntityById(delinquencyConfirm.AppointmentId.Value);
                if (appointment == null) throw new ArgumentException(string.Format("出错,找不到编码为【{0}】的预约信息", delinquencyConfirm.AppointmentId.Value.ToString()));
                appointment.StatusEnum = AppointmentStatus.Waitting;
                Save(new Appointment[] { appointment }, ref tran, true);
                if (delinquencyConfirm != null && !delinquencyConfirm.HasRepeal)
                {
                    var viewAppointmentList = viewAppointmentBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"", delinquencyConfirm.AppointmentId.Value),null,"",false,false,true,new string[]{"EquipmentAdminId","ExperimentationContent"});
                    var appointmentPriviligeList = (IList<AppointmentPrivilige>)GetAppointmentPriviliges(user.Id, viewAppointmentList);
                    if (!JudgeIsEnableCancelRegisterBreakAppointment(viewAppointmentList, appointmentPriviligeList, user.Id, out errorMessage)) throw new Exception(errorMessage);
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
        public bool TutorAuditAppointments(IEnumerable<Guid> appointmentIds,Guid operatorId, string operatorIP, TutorAuditStatus tutorAuditStatus, string remark, out string errorMesseage, out int totalCount, out int sucessCount, out int failCount)
        {
            errorMesseage = "";
            sucessCount = 0;
            failCount = 0;
            totalCount = 0;
            var messgaeHandler = new MessageHandler();
            var operateUser = __userBLL.GetEntityById(operatorId);
            try
            {
                var viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
                var viewAppointmentList = viewAppointmentBLL.GetEntitiesByExpression(appointmentIds.ToFormatStr(), null, "", false, false, true, new string[] { "EquipmentAdminId", "ExperimentationContent" });
                if (viewAppointmentList == null || viewAppointmentList.Count == 0) throw new Exception("出错,预约信息为空");
                totalCount = viewAppointmentList.Count;
                var equipmentList = __equipmentBLL.GetEntitiesByExpression(viewAppointmentList.Select(p=>p.EquipmentId.Value).ToFormatStr());
                foreach (var viewAppointment in viewAppointmentList)
                {
                    var tran = SessionManager.Instance.BeginTransaction();
                    try
                    {
                        string errorMsg = "";
                        if (!JudgeIsEnableTutorAudit(true, tutorAuditStatus, new List<ViewAppointment> { viewAppointment }, equipmentList, operatorId, out errorMsg)) throw new Exception(errorMsg);
                        string whyUnusable = "";
                        viewAppointment.Appointment.TutorAuditStatusEnum = tutorAuditStatus;
                        viewAppointment.Appointment.TutorAuditTime = DateTime.Now;
                        viewAppointment.Appointment.TutorAuditRemark = remark;
                        viewAppointment.Appointment.IsUseable = JudgeAppointmentIsUseable(viewAppointment.Appointment, viewAppointment.Appointment.Equipment, out whyUnusable);
                        viewAppointment.Appointment.WhyUnuseable = whyUnusable;
                        viewAppointment.Appointment.BackUpSampleApplyId = viewAppointment.Appointment.SampleApplyId.HasValue ? viewAppointment.Appointment.SampleApplyId : viewAppointment.Appointment.BackUpSampleApplyId;
                        AuditAppointments(viewAppointment, AppointmentAuditType.TutorAudit, tutorAuditStatus == TutorAuditStatus.Refused ? AuditStatus.NotPassed : AuditStatus.Passed, operatorId, operatorIP, ref tran);
                        Save(new Appointment[] { viewAppointment.Appointment }, ref tran, true);
                        tran.CommitTransaction();
                        sucessCount++;
                        //消息通知
                        messgaeHandler.SendAppointmentTutorAuditResultNotice(viewAppointment, operateUser);
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
            var operateUser = __userBLL.GetEntityById(operatorId);
            try
            {
                var viewAppointmentBLL = BLLFactory.CreateViewAppointmentBLL();
                var viewAppointmentList = viewAppointmentBLL.GetEntitiesByExpression(appointmentIds.ToFormatStr(), null, "", false, false, true, new string[] { "EquipmentAdminId", "ExperimentationContent" });
                if (viewAppointmentList == null || viewAppointmentList.Count == 0) throw new Exception("出错,预约信息为空");
                var equipmentList = __equipmentBLL.GetEntitiesByExpression(viewAppointmentList.Select(p => p.EquipmentId.Value).Distinct().ToFormatStr());
                var appointmentPriligeList = GetAppointmentPriviliges(operatorId, viewAppointmentList);
                totalCount = viewAppointmentList.Count;
                foreach (var viewAppointment in viewAppointmentList)
                {
                    var tran = SessionManager.Instance.BeginTransaction();
                    try
                    {
                        string whyUnusable = "", errorMsg = "";
                        if (!JudgeIsEnableAudit(true, appointmentAuditStatus, new List<ViewAppointment> { viewAppointment }, appointmentPriligeList, equipmentList, operatorId, out errorMsg)) throw new Exception(errorMsg);
                        viewAppointment.Appointment.AuditStatusEnum = appointmentAuditStatus;
                        viewAppointment.Appointment.AuditingUserId = operatorId;
                        viewAppointment.Appointment.AuditingNoPassWhy = remark;
                        viewAppointment.Appointment.IsUseable = JudgeAppointmentIsUseable(viewAppointment.Appointment, viewAppointment.Appointment.Equipment, out whyUnusable);
                        viewAppointment.Appointment.WhyUnuseable = whyUnusable;
                        viewAppointment.Appointment.BackUpSampleApplyId = viewAppointment.Appointment.SampleApplyId.HasValue ? viewAppointment.Appointment.SampleApplyId : viewAppointment.Appointment.BackUpSampleApplyId;
                        AuditAppointments(viewAppointment, AppointmentAuditType.AdminAudit, appointmentAuditStatus == AppointmentAuditStatus.NotPass ? AuditStatus.NotPassed : AuditStatus.Passed, operatorId, operateIP, ref tran);
                        Save(new Appointment[] { viewAppointment.Appointment }, ref tran, true);
                        tran.CommitTransaction();
                        sucessCount++;
                        //消息通知
                        messgaeHandler.SendAppointmentAuditResultNotice(viewAppointment, operateUser);
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
        private void AuditAppointments(ViewAppointment viewAppointment, AppointmentAuditType appointmentAuditType, AuditStatus auditStatus, Guid operatorId, string operatorIP, ref XTransaction tran)
        {
            var costDeductManager = new CostDeductManager();
            string errorMsg = "";
            double realCurrency = 0d, virtualCurrency = 0d;
            if (auditStatus == AuditStatus.NotPassed)
            {
                viewAppointment.Appointment.SampleApplyId = null;
            }
            if (auditStatus == AuditStatus.Passed &&
                viewAppointment.Appointment.SampleApplyId == null &&
                viewAppointment.Appointment.BackUpSampleApplyId.HasValue)
            {
                var countAppointment = CountModelListByExpression(string.Format("SampleApplyId=\"{0}\"*Id=-\"{1}\"", viewAppointment.Appointment.BackUpSampleApplyId.Value, viewAppointment.Appointment.Id));
                if (countAppointment == 0)
                {
                    viewAppointment.Appointment.SampleApplyId = viewAppointment.Appointment.BackUpSampleApplyId;
                }
            }
            if (viewAppointment.IsPredictCostDeduct.HasValue && viewAppointment.IsPredictCostDeduct.Value)
            {
                var predictCostDeduct = __costDeductBLL.GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"", viewAppointment.Id.ToString()));
                if (auditStatus == AuditStatus.NotPassed && predictCostDeduct != null)
                {
                    costDeductManager.CancelAppointmentAuditDeduct(viewAppointment.Appointment, ref tran, operatorId, operatorIP);
                }
                if (auditStatus == AuditStatus.Passed && predictCostDeduct == null)
                {
                    if (appointmentAuditType == AppointmentAuditType.AdminAudit || (appointmentAuditType == AppointmentAuditType.TutorAudit && (viewAppointment.IsNeedAudit == null || !viewAppointment.IsNeedAudit.Value)))
                    {
                        UserAccount userAccount = __userAccountBLL.Deduct(viewAppointment.Appointment.IsOpenFundCostDeduct, viewAppointment.UserId.Value, viewAppointment.SubjectId.Value, viewAppointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMsg, false, false);
                        costDeductManager.AppointmentAuditDeduct(viewAppointment.Appointment, userAccount, ref tran, operatorId, operatorIP);
                    }
                }
            }
        }
        public AppointmentBeforeValidate GetAppointmentBeforeValidate(Guid equipmentId, Guid userId)
        {
            AppointmentBeforeValidate appointmentBeforeValidate = new AppointmentBeforeValidate();
            string errorMessage = "";
            appointmentBeforeValidate.IsUserStatusValidate = __userBLL.CheckUserStatus(userId, out errorMessage);
            appointmentBeforeValidate.MessageUserStatusValidate = errorMessage;

            appointmentBeforeValidate.IsDelinquencyValidate = __punishActionBLL.Validates(userId, out errorMessage);
            appointmentBeforeValidate.MessageDelinquencyValidate = errorMessage;

            appointmentBeforeValidate.IsEquipmentBlackListValidate = __equipmentBlackListBLL.Validates(equipmentId, userId, out errorMessage);
            appointmentBeforeValidate.MessageEquipmentBlackListValidate = errorMessage;
            
            appointmentBeforeValidate.IsEquipmentNoticValidate = __equipmentNoticeBLL.Validates(equipmentId, userId, out errorMessage);
            appointmentBeforeValidate.MessageEquipmentNoticValidate = errorMessage;

            appointmentBeforeValidate.IsTranningValidate = __equipmentTranningBLL.Validates(equipmentId, userId, out errorMessage);
            appointmentBeforeValidate.MessageTranningValidate = errorMessage;

            appointmentBeforeValidate.IsUserEquipmentValidate = __userEquipmentBLL.Validates(equipmentId, userId, out errorMessage);;
            appointmentBeforeValidate.MessageUserEquipmentValidate = errorMessage;

            appointmentBeforeValidate.IsUsedConfirmFeedBackValidate = __usedConfirmBLL.Validates(equipmentId, userId, out errorMessage);
            appointmentBeforeValidate.MessageUsedConfirmFeedBackValidate = errorMessage;

            appointmentBeforeValidate.IsEquipmentUserExaminationValidate = __userExaminationBLL.Validates(equipmentId,  userId, TrainningExaminationBusinessType.Equipment, out errorMessage);
            appointmentBeforeValidate.MessageEquipmentUserExaminationValidate = errorMessage;

            appointmentBeforeValidate.IsOrgUserExaminationValidate = __userExaminationBLL.Validates(equipmentId, userId, TrainningExaminationBusinessType.LabOrganization, out errorMessage);
            appointmentBeforeValidate.MessageOrgUserExaminationValidate = errorMessage;

            appointmentBeforeValidate.IsAllValidate = appointmentBeforeValidate.IsUserStatusValidate 
                && appointmentBeforeValidate.IsDelinquencyValidate 
                && appointmentBeforeValidate.IsEquipmentBlackListValidate
                && appointmentBeforeValidate.IsEquipmentNoticValidate
                && appointmentBeforeValidate.IsTranningValidate
                && appointmentBeforeValidate.IsUserEquipmentValidate
                && appointmentBeforeValidate.IsUsedConfirmFeedBackValidate
                && appointmentBeforeValidate.IsEquipmentUserExaminationValidate
                && appointmentBeforeValidate.IsOrgUserExaminationValidate;

            return appointmentBeforeValidate;
        }
        public bool Validate(Appointment appointment, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                if (appointment.GetPaymentUser() == null)
                {
                    errorMessage = "付费人为空";
                    return false;
                }
                if (appointment.GetPaymentUser().UserAccount == null)
                {
                    errorMessage = "付费人账户为空";
                    return false;
                }
                if (appointment.GetPaymentUser().UserAccount.IsUnUseable)
                {
                    errorMessage = string.Format("付费人【{0}】帐户余额不足,达到信用额度的禁用值", appointment.GetPaymentUser().UserName);
                    return false;
                }
                if (appointment.PaymentMethodEnum == PaymentMethod.SubjectDirectorPay &&
                    appointment.Subject.Director.Id != appointment.User.Id)
                {
                    var subjectExperimenter = appointment.Subject.Experiments.FirstOrDefault(x => x.Experimenter.Id == appointment.User.Id);
                    if (subjectExperimenter.IsUnUseable)
                    {
                        errorMessage = "预约者课题经费不足,达到信用额度的禁用值";
                        return false;
                    }
                }
                var punishRecord = __punishRecordBLL.GetUserPubishRecord(appointment.UserId.Value);
                if (punishRecord != null && !punishRecord.HasFinished && punishRecord.IsUnusable)
                {
                    errorMessage = "不良行为程度累积到禁用设备";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            return true;
        }
        public int GetAppointmentSampleQuatityByEquipmentId(Guid equipmentId)
        {
            var obj = GetSingleResult(String.Format("SELECT sum(Appointment.SampleCount) FROM Appointment WHERE Status={0} AND EquipmentId = '{1}'", (int)AppointmentStatus.Success, equipmentId));
            return obj == null ? 0 : int.Parse(obj.ToString());
        }
        public int GetAppointmentSampleQuatityByAppliantId(Guid appliantId)
        {
            var obj = GetSingleResult(String.Format("SELECT sum(Appointment.SampleCount) FROM Appointment WHERE Status={0} AND SubscriberId = '{1}'", (int)AppointmentStatus.Success, appliantId));
            return obj == null ? 0 : int.Parse(obj.ToString());
        }
        public bool JudgeAppointmentIsUseable(Appointment appointment,Equipment equipment,out string whyUnUsable)
        {
            IEquipmentTranningBLL equipmentTrainningBLL = BLLFactory.CreateEquipmentTranningBLL();
            whyUnUsable = "";
            StringBuilder sbReason = new StringBuilder();
            bool isAppointmentNeedTutorAuditPass = true,isAppointmentNeedAuditPass = true, isIsNeedTranningBeforeUsePass = true;
            if (equipment.IsAppointmentNeedTutorAudit && appointment.TutorAuditStatusEnum != TutorAuditStatus.Passed)
            {
                sbReason.Append(__equipmentBLL.GetTutorUnAuditMessage()).AppendLine();
                isAppointmentNeedTutorAuditPass = false;
            }
            if (equipment.IsAppointmentNeedAudit.HasValue && equipment.IsAppointmentNeedAudit.Value && appointment.AuditStatusEnum != AppointmentAuditStatus.Pass)
            {
                sbReason.Append(__equipmentBLL.GetUnAuditMessage()).AppendLine();
                isAppointmentNeedAuditPass = false;
            }
            if (equipment.IsNeedTranningBeforeUse.HasValue && equipment.IsNeedTranningBeforeUse.Value)
            {
                var equipmentTrainning = __equipmentTranningBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*CreatorId=\"{1}\"*Status=\"{2}\"",
                                          equipment.Id, appointment.UserId.Value, (int)EquipmentTrainningStatus.Finished));
                if (equipmentTrainning == null)
                {
                    sbReason.Append(__equipmentBLL.GetUnTrainningMessage()).AppendLine();
                    isIsNeedTranningBeforeUsePass = false;
                }
            }
            whyUnUsable = sbReason.ToString();
            return isAppointmentNeedAuditPass && isIsNeedTranningBeforeUsePass && isAppointmentNeedTutorAuditPass;
        }
        public void UpdateAppointmentIsUsable(IEnumerable<Guid> userIds,Equipment equipment, bool? isUsable, string whyUnusable,string additionQueryExpression ,ref XTransaction tran)
        {
            var queryExpression = string.Format("BeginTime>\"{0}\"*Status={1}*EquipmentId=\"{2}\"", DateTime.Now.AddSeconds(1), (int)AppointmentStatus.Waitting, equipment.Id);
            if (!string.IsNullOrWhiteSpace(additionQueryExpression)) queryExpression += "*" + additionQueryExpression;
            if (userIds != null && userIds.Count() > 0)
                queryExpression += "*" + userIds.ToFormatStr("UserId");
            var appointmentList = GetEntitiesByExpression(queryExpression);
            if (appointmentList != null && appointmentList.Count > 0)
            {
                foreach (var appointment in appointmentList)
                {
                    bool isAuditPass = !appointment.IsNeedAudit.HasValue || !appointment.IsNeedAudit.Value || (appointment.IsNeedAudit.HasValue && appointment.IsNeedAudit.Value && appointment.AuditStatusEnum== AppointmentAuditStatus.Pass);
                    appointment.IsUseable = isUsable.HasValue ? isUsable : JudgeAppointmentIsUseable(appointment,equipment ,out whyUnusable);
                    appointment.WhyUnuseable = whyUnusable;
                    appointment.IsUseable = appointment.IsUseable.Value && isAuditPass;
                    if (!isAuditPass && appointment.WhyUnuseable.IndexOf(__equipmentBLL.GetUnAuditMessage()) == -1)
                        appointment.WhyUnuseable = new StringBuilder(appointment.WhyUnuseable).AppendLine().Append(__equipmentBLL.GetUnAuditMessage()).ToString();
                }
                Save(appointmentList, ref tran, true);
            }
        }
        public void UpdateAppointmentAuditStatus(Equipment equipment,ref XTransaction tran)
        {
            var queryExpression = string.Format("BeginTime>\"{0}\"*Status={1}*EquipmentId=\"{2}\"", DateTime.Now.AddSeconds(1), (int)AppointmentStatus.Waitting, equipment.Id);
            var appointmentList = GetEntitiesByExpression(queryExpression);
            if (appointmentList != null && appointmentList.Count > 0)
            {
                foreach (var appointment in appointmentList)
                {
                    appointment.IsNeedAudit = equipment.IsAppointmentNeedAudit.HasValue?equipment.IsAppointmentNeedAudit.Value:false;
                    if (!equipment.IsAppointmentNeedAudit.HasValue ||  !equipment.IsAppointmentNeedAudit.Value)
                    {
                        appointment.AuditStatusEnum = AppointmentAuditStatus.UnAudit;
                        appointment.AuditingUserId = null;
                        appointment.AuditingNoPassWhy = "";
                    }
                }
                Save(appointmentList, ref tran, true);
            }
        }
    }
}