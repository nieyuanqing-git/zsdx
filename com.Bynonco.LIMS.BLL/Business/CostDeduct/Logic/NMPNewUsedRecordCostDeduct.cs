using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPNewUsedRecordCostDeduct : NMPUsedConfirmRelativeCostDeduct
    {

        private Guid? __nmpUsedConfirmId;
        private string __userLabel;
        private string __nmpeEquipmentLabel;
        private DateTime __startAt;
        private DateTime __endAt;
        private bool __isDeduct;
        private string __remarkInfo;
        private string __subjectInfo;
        private UserAccount __userAccount;
        public NMPNewUsedRecordCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            Guid usedConfirmId = Guid.Empty;
            if (businessObjects[0] != null && !string.IsNullOrWhiteSpace(businessObjects[0].ToString())
                && !Guid.TryParse(businessObjects[0].ToString(), out usedConfirmId))
            {
                throw new ArgumentException("工位使用记录ID不正确");
            }
            __nmpUsedConfirmId = usedConfirmId;
            if (businessObjects[1] != null) __userLabel = businessObjects[1].ToString();
            if (businessObjects[2] != null) __nmpeEquipmentLabel = businessObjects[2].ToString();
            if (businessObjects[3] != null) __startAt = DateTime.Parse(businessObjects[3].ToString());
            if (businessObjects[4] != null) __endAt = DateTime.Parse(businessObjects[4].ToString());
            if (businessObjects[5] != null) __isDeduct = bool.Parse(businessObjects[5].ToString());
            if (businessObjects.Length >= 7) __subjectInfo = businessObjects[6] == null ? null : businessObjects[6].ToString();
            if (businessObjects.Length >= 8) __remarkInfo = businessObjects[7] == null ? null : businessObjects[7].ToString();
            if (businessObjects.Length >= 9) _nmpUsedConfirmFeedBack = businessObjects[8] == null ? null : (NMPUsedConfirmFeedBack)businessObjects[8];
        }
        private NMPUsedConfirm GenerateNewNMPUsedConfirm(User user, NMPEquipment nmpEquipment, out string errorMessage)
        {
            errorMessage = "";
            NMPUsedConfirm nmpUsedConfirm = null;
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            Guid? subjectId = null;
            var subject = _subjectBLL.GetUserSelfJoinSubject(user.Id);
            if (subject != null)
            {
                subjectId = subject.Id;
                _userBLL.GetPayer(user.Id, subject.Id, out paymentMethod, out __userAccount);
            }
            nmpUsedConfirm = _nmpUsedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*NMPEquipmentId=\"{1}\"*BeginAt=\"{2}\"", user.Id, nmpEquipment.Id, __startAt.ToString("yyyy-MM-dd HH:mm:ss")));
            if (nmpUsedConfirm == null)
            {
                nmpUsedConfirm = _nmpUsedConfirmBLL.CreatedNMPUsedConfirm(user.Id, subjectId, nmpEquipment.Id, paymentMethod, __startAt, __endAt, null, out errorMessage);
                if (!string.IsNullOrWhiteSpace(__subjectInfo) && !string.IsNullOrWhiteSpace(__remarkInfo))
                {
                    nmpUsedConfirm.Remark = "课题:" + __subjectInfo + " 备注:" + __remarkInfo + " ";
                }
            }
            return nmpUsedConfirm;
        }
        protected override Model.NMPUsedConfirm CreateNMPUseConfirm(out string errorMessage)
        {
            errorMessage = "";
            NMPUsedConfirm nmpUsedConfirm = null;
            var user = _userBLL.GetUserByLabel(__userLabel);
            if (user == null) throw new ArgumentException(string.Format("找不到标识为【{0}】的用户信息", __userLabel));
            var nmpEquipment = _nmpEquipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", __nmpeEquipmentLabel));
            if (nmpEquipment == null) throw new ArgumentException(string.Format("找不到标识为【{0}】的工位设备信息", __nmpeEquipmentLabel));
            if (__nmpUsedConfirmId == Guid.Empty)
            {
                nmpUsedConfirm = GenerateNewNMPUsedConfirm(user,nmpEquipment,out errorMessage);
            }
            else
            {
                nmpUsedConfirm = _nmpUsedConfirmBLL.GetEntityById(__nmpUsedConfirmId.Value);
                if (nmpUsedConfirm == null)
                {
                    nmpUsedConfirm = GenerateNewNMPUsedConfirm(user, nmpEquipment, out errorMessage);
                    nmpUsedConfirm.Id = __nmpUsedConfirmId.Value;
                }
                else
                {
                    _nmpUsedConfirmBLL.GenerateRelativeAppointmentNMPUsedConfirm(nmpUsedConfirm.GetNMPAppointment(), nmpUsedConfirm);
                }
            }
            nmpUsedConfirm.BeginAt = nmpUsedConfirm.BeginAt.HasValue ? nmpUsedConfirm.BeginAt < __startAt ? nmpUsedConfirm.BeginAt : __startAt : __startAt;
            nmpUsedConfirm.EndAt = nmpUsedConfirm.EndAt.HasValue ? nmpUsedConfirm.EndAt > __endAt ? nmpUsedConfirm.EndAt : __endAt : __endAt;
            if (nmpUsedConfirm.EndAt.HasValue && nmpUsedConfirm.BeginAt.HasValue && nmpUsedConfirm.EndAt.Value < nmpUsedConfirm.BeginAt.Value)
            {
                nmpUsedConfirm.EndAt = null;
            }
            //重新计算费用,处理之前的使用记录是小于最小扣费时间不进行扣费造成usedConfirm.CalFee为零的情况
            if (nmpUsedConfirm.BeginAt.HasValue && nmpUsedConfirm.EndAt.HasValue && nmpUsedConfirm.CostDeduct == null)
            {
                double calDuration = -1;
                double? unitPrice = null;
                nmpUsedConfirm.RealFee = _nmpChargeStandardBLL.GetNMPUsedConfirmCalFee(
                                        nmpUsedConfirm,
                                        out calDuration,
                                        out unitPrice);
                nmpUsedConfirm.CalcDuration = calDuration == -1d ? (nmpUsedConfirm.EndAt.Value - nmpUsedConfirm.BeginAt.Value).TotalHours : calDuration;
            }
            if (nmpUsedConfirm.RealFee == null) nmpUsedConfirm.RealFee = 0d;
            GenerateNMPUsedConfirmRelativeFeedBack(nmpUsedConfirm);
            return nmpUsedConfirm;
        }

        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            var nmpUsedConfirm = CreateNMPUseConfirm(out errorMessage);
            if (nmpUsedConfirm.RealFee == null) nmpUsedConfirm.RealFee = 0d;
            nmpUsedConfirm.WhyNotCostDeduct += string.IsNullOrWhiteSpace(nmpUsedConfirm.WhyNotCostDeduct) ? errorMessage : "\r\n" + errorMessage;
            var user = _userBLL.GetUserByLabel(__userLabel);
            //没指定导师的学生用户不进行扣费，只产生使用记录
            if (user.UserType != null && user.UserType.UserIdentityEnum == UserIdentity.Student && !user.TutorId.HasValue)
            {
                __isDeduct = false;
            }
            try
            {
                if (__nmpUsedConfirmId == Guid.Empty)
                {
                    var nmpUsedConfirmTemp = _nmpUsedConfirmBLL.GetEntityById(nmpUsedConfirm.Id);
                    if (nmpUsedConfirmTemp == null)
                    {
                        nmpUsedConfirm = AddNMPUsedConfirm(null, user, __userAccount, __startAt, __endAt, "", null, ref tran, __isDeduct);
                    }
                    else if (__endAt > __startAt)
                    {
                        nmpUsedConfirm = UpdateUseTime(null,nmpUsedConfirm, user, __startAt, __endAt, null, ref tran,_nmpUsedConfirmFeedBack ,__isDeduct);
                    }
                }
                else if (__endAt >= __startAt)
                {
                    if (nmpUsedConfirm.Status == (int)UsedConfirmStatus.UnComplete) nmpUsedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    nmpUsedConfirm = UpdateUseTime(null, nmpUsedConfirm, user, __startAt, __endAt, null, ref tran, _nmpUsedConfirmFeedBack, __isDeduct);
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
                //通知
                if (nmpUsedConfirm != null && nmpUsedConfirm.CostDeduct != null)
                {
                    _messageHandler.SendNMPUsedConfirmCostDeductNotice(nmpUsedConfirm, null);
                    _messageHandler.SendDepositNotice(nmpUsedConfirm.CostDeduct, nmpUsedConfirm, null);
                }
            }
            catch (Exception ex)
            {
                if (!isSupress && tran.HasTransaction) tran.RollbackTransaction();
                errorMessage = ex.Message;
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
            return new object[] { nmpUsedConfirm };
        }
       
    }
}
