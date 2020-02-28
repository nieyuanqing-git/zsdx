using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.IBLL;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class AppointmentCostDeduct : UsedConfirmRelativeCostDeduct
    {
        private Appointment __appointment;
        private DateTime __beginTime;
        private DateTime __endTime;
        private bool __isDeduct = true;
        private CostDeduct __appointmentPredictCostDeduct;
        private IList<UsedConfirm> __appointmentUsedConfirmList;
        public AppointmentCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            __appointment = businessObjects[0] as Appointment;
            __beginTime = DateTime.Parse(businessObjects[1].ToString());
            __endTime = DateTime.Parse(businessObjects[2].ToString());
            _usedConfirmFeedBack = businessObjects[3] != null ? (UsedConfirmFeedBack)businessObjects[3] : null;
            __appointmentPredictCostDeduct = __appointment.PredictCostDeduct;
            __appointmentUsedConfirmList = _usedConfirmBLL.GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", __appointment.Id.ToString()));
            _preCostDeductEquipmentOpenFunds = __appointment.PredictCostDeduct != null ? __appointment.PredictCostDeduct.CostDeductEquipmentOpenFunds : null;
        }
        protected override UsedConfirm CreateUseConfirm(out string errorMessage)
        {
            var usedConfirm =  _usedConfirmBLL.CreateAppointmentUsedConfirm(__appointment, __beginTime, __endTime, true, null, out errorMessage);
            if (usedConfirm.RealFee == null) usedConfirm.RealFee = 0d;
            if (_usedConfirmFeedBack != null && _usedConfirmFeedBack.SampleNum.HasValue && _usedConfirmFeedBack.SampleNum.Value > 0)
            {
                usedConfirm.SampleCount = _usedConfirmFeedBack.SampleNum.Value;
            }
            if (!(!__appointment.IsPeriodCharge.HasValue||__appointment.IsPeriodCharge.Value)&&__appointmentUsedConfirmList != null && __appointmentUsedConfirmList.Count > 0)
            {
                usedConfirm.Id = __appointmentUsedConfirmList.First().Id;
            }
            GenerateUsedConfirmRelativeFeedBack(usedConfirm);
            return usedConfirm;
        }
        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            string errorMessage;
            UsedConfirm usedConfirm;
            double realCurrency, virtualCurrency;
            bool isPeriodCharge = __appointment.IsPeriodCharge.HasValue && __appointment.IsPeriodCharge.Value;
            UserAccount userAccount = _userAccountBLL.Deduct(false,__appointment.UserId.Value, __appointment.SubjectId.Value, __appointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
            IList<UsedConfirm> usedConfirmList = _usedConfirmBLL.GetEntitiesByExpression(string.Format("AppointmentId=\"{0}\"", __appointment.Id.ToString()));
            if (usedConfirmList == null || usedConfirmList.Count == 0)
            {
                usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
            }
            else
            {
                DateTime? minStartedDate, maxStartedDate, minUsedDate, maxUsedDate;
                var lstStartedConfirms = usedConfirmList.Where(p => !p.EndAt.HasValue).ToList();
                minStartedDate = lstStartedConfirms.Any() ? lstStartedConfirms.Select(p => p.BeginAt).Min() : null;
                maxStartedDate = lstStartedConfirms.Any() ? lstStartedConfirms.Select(p => p.BeginAt).Max() : null;

                var lstUsedConfirms = usedConfirmList.Where(p => p.BeginAt.HasValue && p.EndAt.HasValue).ToList();
                minUsedDate = lstUsedConfirms.Any() ? lstUsedConfirms.Select(p => p.BeginAt).Min() : null;
                maxUsedDate = lstUsedConfirms.Any() ? lstUsedConfirms.Select(p => p.EndAt).Max() : null;

                if (__beginTime > __endTime)//过来的开机信息
                {
                    usedConfirm = usedConfirmList.FirstOrDefault(p => p.BeginAt.Value == __beginTime);

                    if (usedConfirm != null) return new object[] { usedConfirm };//该开机记录已经处理

                    //lxm_2017-09-04 修改， 包含断电有脱机记录的情况
                    if (!isPeriodCharge && usedConfirmList.Count(c => c.BeginAt.HasValue) > 0) return null;//非分段计费,如果存在使用记录则不再处理开机记录,因为在Commit方法会删除多余的开机记录,所以不存一个使用记录多条开机记录的情况，这样可以避免刷卡3秒延迟的问题                                        

                    if (!isPeriodCharge &&
                        (minStartedDate != null && maxStartedDate != null &&
                        (minStartedDate.Value - __beginTime).TotalMilliseconds < 0 &&
                        (maxStartedDate.Value - __beginTime).TotalMilliseconds > 0)) return null;

                    if (!isPeriodCharge &&
                        (minUsedDate != null && maxUsedDate != null &&
                        (minUsedDate.Value - __beginTime).TotalMilliseconds < 0 &&
                        (maxUsedDate.Value - __beginTime).TotalMilliseconds > 0)) return null;

                    if ((minStartedDate == null || maxStartedDate == null ||
                        minStartedDate.Value == maxStartedDate.Value ||
                        (__beginTime - minStartedDate.Value).TotalMilliseconds < 0 ||
                        (__beginTime - maxStartedDate.Value).TotalMilliseconds > 0))
                    {
                        usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                    if (minUsedDate == null || maxUsedDate == null ||
                        (__beginTime - minUsedDate.Value).TotalMilliseconds < 0 ||
                        (__beginTime - maxUsedDate.Value).TotalMilliseconds > 0)
                    {
                        usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                    if (isPeriodCharge && minUsedDate != null || maxUsedDate != null &&
                        (__beginTime - minUsedDate.Value).TotalMilliseconds > 0 &&
                        (__beginTime - maxUsedDate.Value).TotalMilliseconds < 0)
                    {
                        usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                }
                else //过来的是使用记录
                {
                    usedConfirm = usedConfirmList.FirstOrDefault(p => p.BeginAt.Value == __beginTime && !p.EndAt.HasValue);
                    if (usedConfirm != null)//存在该使用记录的开机记录,合并
                    {
                        usedConfirm = UpdateUseTime(null, usedConfirm, __appointment.User, __beginTime, __endTime, null, ref tran, _usedConfirmFeedBack);//已经存在该开始时间的开机记录
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                    if (minUsedDate == null || maxUsedDate == null)//不存在任何使用记录,插入该使用记录
                    {
                        usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);//当前数据库没任何改预约对应该的使用记录则插入该使用记录
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                    if (!isPeriodCharge)
                    {
                        usedConfirm = usedConfirmList.First(p => p.EndAt.HasValue);
                        usedConfirm = UpdateUseTime(null, usedConfirm, __appointment.User, __beginTime, __endTime, null, ref tran, _usedConfirmFeedBack);//数据库已经存在使用记录,进行合并
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) }; ;
                    }
                    else
                    {
                        usedConfirm = AddUsedConfirm(null,__appointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(isPeriodCharge, usedConfirm, ref tran) };
                    }
                }
            }
            return new object[] { usedConfirm };
        }
        private UsedConfirm Commit(bool isPeriodCharge,UsedConfirm usedConfirm, ref XTransaction tran)
        {
            bool isDeletePredictCostDeduct = __appointmentPredictCostDeduct != null;
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (isDeletePredictCostDeduct)
                { 
                    __appointment.RealCurrency = 0d;
                    __appointment.VirtualCurrency = 0d;
                }
                if (usedConfirm.CostDeduct != null)
                {
                    __appointment.StatusEnum = AppointmentStatus.Success;
                    usedConfirm.CostDeduct.AppointmentForLog = __appointment;
                }
                if (isDeletePredictCostDeduct || usedConfirm.CostDeduct != null) 
                    _appointmentBLL.Save(new Appointment[] { __appointment }, ref tran, true);
                //非分段计费,删除多余的开机记录
                if (!isPeriodCharge)
                {
                    if (__appointmentUsedConfirmList != null && __appointmentUsedConfirmList.Count > 0)
                    {
                        var unFinishUsedConfirmList = __appointmentUsedConfirmList.Where(p => p.BeginAt.HasValue && !p.EndAt.HasValue && p.BeginAt.Value != usedConfirm.BeginAt.Value).ToList();
                        if (unFinishUsedConfirmList.Any() && (__appointmentUsedConfirmList.Any(p => p.IsFinsh) || usedConfirm.IsFinsh))
                            _usedConfirmBLL.Delete(unFinishUsedConfirmList.Select(p => p.Id), ref tran, true);
                    }
                }
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                throw;
            }
            finally { tran.Dispose(); }
            //消息通知
            if (usedConfirm.CostDeduct != null && usedConfirm != null)
            {
                _messageHandler.SendDepositNotice(usedConfirm.CostDeduct, usedConfirm, null);
                if (usedConfirm.EndAt.HasValue)
                    _messageHandler.SendUsedConfirmCostDeductNotice(usedConfirm, null);
            }
            if (usedConfirm.EndAt.HasValue && usedConfirm.EndAt.Value > __appointment.EndTime.Value)
                _messageHandler.SendUseEquipmentOutOfTimeNotice(__appointment, null);
            SendErrorChargedTypeNotice(usedConfirm);
            return usedConfirm;
        }
    }
}
