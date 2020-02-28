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
    public class NMPAppointmentCostDeduct : NMPUsedConfirmRelativeCostDeduct
    {
        private NMPAppointment __nmpAppointment;
        private DateTime __beginTime;
        private DateTime __endTime;
        private bool __isDeduct = true;
        private IList<NMPUsedConfirm> __appointmentNMPUsedConfirmList;
        public NMPAppointmentCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            __nmpAppointment = businessObjects[0] as NMPAppointment;
            __beginTime = DateTime.Parse(businessObjects[1].ToString());
            __endTime = DateTime.Parse(businessObjects[2].ToString());
            _nmpUsedConfirmFeedBack = businessObjects[3] != null ? (NMPUsedConfirmFeedBack)businessObjects[3] : null;
            __appointmentNMPUsedConfirmList = _nmpUsedConfirmBLL.GetEntitiesByExpression(string.Format("NMPAppointmentId=\"{0}\"", __nmpAppointment.Id));
        }
        protected override NMPUsedConfirm CreateNMPUseConfirm(out string errorMessage)
        {
            var nmpUsedConfirm = _nmpUsedConfirmBLL.CreateAppointmentNMPUsedConfirm(__nmpAppointment, __beginTime, __endTime, true, null, out errorMessage);
            if (nmpUsedConfirm.RealFee == null) nmpUsedConfirm.RealFee = 0d;
            if (__appointmentNMPUsedConfirmList != null && __appointmentNMPUsedConfirmList.Count > 0)
            {
                nmpUsedConfirm.Id = __appointmentNMPUsedConfirmList.First().Id;
            }
            GenerateNMPUsedConfirmRelativeFeedBack(nmpUsedConfirm);
            return nmpUsedConfirm;
        }
        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            string errorMessage = "";
            NMPUsedConfirm nmpUsedConfirm = null;
            double realCurrency = 0d, virtualCurrency = 0d;
            UserAccount userAccount = _userAccountBLL.Deduct(false,__nmpAppointment.UserId.Value, __nmpAppointment.SubjectId.Value, __nmpAppointment.PaymentMethodEnum, 0, out realCurrency, out virtualCurrency, ref tran, out errorMessage, false);
            IList<NMPUsedConfirm> nmpUsedConfirmList = _nmpUsedConfirmBLL.GetEntitiesByExpression(string.Format("NMPAppointmentId=\"{0}\"", __nmpAppointment.Id.ToString()));
            if (nmpUsedConfirmList == null || nmpUsedConfirmList.Count == 0)
            {
                nmpUsedConfirm = AddNMPUsedConfirm(null, __nmpAppointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                return new object[] { Commit(nmpUsedConfirm, ref tran) };
            }
            else
            {
                DateTime? minStartedDate, maxStartedDate, minUsedDate, maxUsedDate;
                var lstStartedConfirms = nmpUsedConfirmList.Where(p => !p.EndAt.HasValue);
                minStartedDate = lstStartedConfirms.Count() > 0 ? lstStartedConfirms.Select(p => p.BeginAt).Min() : null;
                maxStartedDate = lstStartedConfirms.Count() > 0 ? lstStartedConfirms.Select(p => p.BeginAt).Max() : null;

                var lstUsedConfirms = nmpUsedConfirmList.Where(p => p.BeginAt.HasValue && p.EndAt.HasValue);
                minUsedDate = lstUsedConfirms.Count() > 0 ? lstUsedConfirms.Select(p => p.BeginAt).Min() : null;
                maxUsedDate = lstUsedConfirms.Count() > 0 ? lstUsedConfirms.Select(p => p.EndAt).Max() : null;

                if (__beginTime > __endTime)//过来的开机信息
                {
                    nmpUsedConfirm = nmpUsedConfirmList.FirstOrDefault(p => p.BeginAt.Value == __beginTime);

                    if (nmpUsedConfirm != null) return new object[] { nmpUsedConfirm };//该开机记录已经处理

                    if (maxUsedDate != null && minUsedDate != null) return null;//非分段计费,如果存在使用记录则不再处理开机记录,因为在Commit方法会删除多余的开机记录,所以不存一个使用记录多条开机记录的情况，这样可以避免刷卡3秒延迟的问题

                    if ((minStartedDate != null && maxStartedDate != null &&
                        (minStartedDate.Value - __beginTime).TotalMilliseconds < 0 &&
                        (maxStartedDate.Value - __beginTime).TotalMilliseconds > 0)) return null;

                    if ((minUsedDate != null && maxUsedDate != null &&
                        (minUsedDate.Value - __beginTime).TotalMilliseconds < 0 &&
                        (maxUsedDate.Value - __beginTime).TotalMilliseconds > 0)) return null;

                    if ((minStartedDate == null || maxStartedDate == null ||
                        minStartedDate.Value == maxStartedDate.Value ||
                        (__beginTime - minStartedDate.Value).TotalMilliseconds < 0 ||
                        (__beginTime - maxStartedDate.Value).TotalMilliseconds > 0))
                    {
                        nmpUsedConfirm = AddNMPUsedConfirm(null, __nmpAppointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(nmpUsedConfirm, ref tran) };
                    }
                    if (minUsedDate == null || maxUsedDate == null ||
                        (__beginTime - minUsedDate.Value).TotalMilliseconds < 0 ||
                        (__beginTime - maxUsedDate.Value).TotalMilliseconds > 0)
                    {
                        nmpUsedConfirm = AddNMPUsedConfirm(null, __nmpAppointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);
                        return new object[] { Commit(nmpUsedConfirm, ref tran) };
                    }
                }
                else //过来的是使用记录
                {
                    nmpUsedConfirm = nmpUsedConfirmList.FirstOrDefault(p => p.BeginAt.Value == __beginTime && !p.EndAt.HasValue);
                    if (nmpUsedConfirm != null)//存在该使用记录的开机记录,合并
                    {
                        nmpUsedConfirm = UpdateUseTime(null, nmpUsedConfirm, __nmpAppointment.User, __beginTime, __endTime, null, ref tran, _nmpUsedConfirmFeedBack);//已经存在该开始时间的开机记录
                        return new object[] { Commit(nmpUsedConfirm, ref tran) };
                    }
                    if (minUsedDate == null || maxUsedDate == null)//不存在任何使用记录,插入该使用记录
                    {
                        nmpUsedConfirm = AddNMPUsedConfirm(null,__nmpAppointment.User, userAccount, __beginTime, __endTime, "", null, ref tran, __isDeduct);//当前数据库没任何改预约对应该的使用记录则插入该使用记录
                        return new object[] { Commit(nmpUsedConfirm, ref tran) };
                    }
                    nmpUsedConfirm = nmpUsedConfirmList.First(p => p.EndAt.HasValue);
                    nmpUsedConfirm = UpdateUseTime(null, nmpUsedConfirm, __nmpAppointment.User, __beginTime, __endTime, null, ref tran, _nmpUsedConfirmFeedBack);//数据库已经存在使用记录,进行合并
                    return new object[] { Commit(nmpUsedConfirm, ref tran) }; ;
                }
            }
            return new object[] { nmpUsedConfirm };
        }
        private NMPUsedConfirm Commit(NMPUsedConfirm nmpUsedConfirm, ref XTransaction tran) 
        {
            if (tran == null) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (nmpUsedConfirm.CostDeduct != null)
                {
                    __nmpAppointment.StatusEnum = AppointmentStatus.Success;
                    _nmpAppointmentBLL.Save(new NMPAppointment[] { __nmpAppointment }, ref tran, true);
                }
                if (__appointmentNMPUsedConfirmList != null && __appointmentNMPUsedConfirmList.Count > 0)
                {
                    var unFinishUsedConfirmList = __appointmentNMPUsedConfirmList.Where(p => p.BeginAt.HasValue && !p.EndAt.HasValue && p.BeginAt.Value != nmpUsedConfirm.BeginAt.Value);
                    if (unFinishUsedConfirmList != null && unFinishUsedConfirmList.Count() > 0 && (__appointmentNMPUsedConfirmList.Any(p => p.IsFinsh) || nmpUsedConfirm.IsFinsh))
                        _nmpUsedConfirmBLL.Delete(unFinishUsedConfirmList.Select(p => p.Id), ref tran, true);
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
            if (nmpUsedConfirm.CostDeduct != null && nmpUsedConfirm != null)
            {
                _messageHandler.SendDepositNotice(nmpUsedConfirm.CostDeduct,nmpUsedConfirm, null);
                if (nmpUsedConfirm.EndAt.HasValue)
                    _messageHandler.SendNMPUsedConfirmCostDeductNotice(nmpUsedConfirm, null);
            }
            return nmpUsedConfirm;
        }
    }
}
