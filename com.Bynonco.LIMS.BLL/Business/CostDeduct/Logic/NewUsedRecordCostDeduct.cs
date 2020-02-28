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
    public class NewUsedRecordCostDeduct : UsedConfirmRelativeCostDeduct
    {

        private Guid? __usedConfirmId;
        private string __userLabel;
        private string __equipmentLabel;
        private DateTime __startAt;
        private DateTime __endAt;
        private bool __isDeduct;
        private string __remarkInfo;
        private string __subjectInfo;
        private UserAccount __userAccount;
        public NewUsedRecordCostDeduct(object[] businessObjects, Guid? operatorId, string operatorIP)
            : base(businessObjects,operatorId,operatorIP)
        {
            Guid usedConfirmId = Guid.Empty;
            if (businessObjects[0] != null && !string.IsNullOrWhiteSpace(businessObjects[0].ToString())
                && !Guid.TryParse(businessObjects[0].ToString(), out usedConfirmId))
            {
                throw new ArgumentException("使用记录ID不正确");
            }
            __usedConfirmId = usedConfirmId;
            if (businessObjects[1] != null) __userLabel = businessObjects[1].ToString();
            if (businessObjects[2] != null) __equipmentLabel = businessObjects[2].ToString();
            if (businessObjects[3] != null) __startAt = DateTime.Parse(businessObjects[3].ToString());
            if (businessObjects[4] != null) __endAt = DateTime.Parse(businessObjects[4].ToString());
            if (businessObjects[5] != null) __isDeduct = bool.Parse(businessObjects[5].ToString());
            if (businessObjects.Length >= 7) __subjectInfo = businessObjects[6] == null ? null : businessObjects[6].ToString();
            if (businessObjects.Length >= 8) __remarkInfo = businessObjects[7] == null ? null : businessObjects[7].ToString();
            if (businessObjects.Length >= 9) _usedConfirmFeedBack = businessObjects[8] == null ? null : (UsedConfirmFeedBack)businessObjects[8];
        }
        /// <summary>
        /// 构建新使用记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="equipment"></param>
        /// <param name="sampleCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private UsedConfirm GenerateNewUsedConfirm(User user,Equipment equipment,int? sampleCount,out string errorMessage)
        {
            errorMessage = "";
            UsedConfirm usedConfirm = null;
            PaymentMethod paymentMethod = PaymentMethod.SubjectDirectorPay;
            Guid? subjectId = null;
            // 获取用户所在课题组
            var subject = _subjectBLL.GetUserSelfJoinSubject(user.Id);
            if (subject != null)
            {
                subjectId = subject.Id;
                // 获取课题组支付账号、支付方式
                _userBLL.GetPayer(user.Id, subject.Id, out paymentMethod, out __userAccount);
            }
            // 获取新使用记录
            usedConfirm = _usedConfirmBLL.GetFirstOrDefaultEntityByExpression(string.Format("UserId=\"{0}\"*EquipmentId=\"{1}\"*BeginAt=\"{2}\"", user.Id.ToString(), equipment.Id.ToString(), __startAt.ToString("yyyy-MM-dd HH:mm:ss")));
            if (usedConfirm == null)
            {
                // 新建使用记录
                usedConfirm = _usedConfirmBLL.CreatedUsedConfirm(user.Id, subjectId, equipment.Id, paymentMethod, __startAt, __endAt, sampleCount, null, out errorMessage);
                if (!string.IsNullOrWhiteSpace(__subjectInfo) && !string.IsNullOrWhiteSpace(__remarkInfo))
                {
                    usedConfirm.Remark = "课题:" + __subjectInfo + " 备注:" + __remarkInfo + " ";
                }
            }
            return usedConfirm;
        }
        /// <summary>
        /// 构建使用记录实例
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected override Model.UsedConfirm CreateUseConfirm(out string errorMessage)
        {
            errorMessage = "";
            UsedConfirm usedConfirm = null;
            bool isOpenFundCostDeduct = false;
            int? sampleCount = null;
            if (_usedConfirmFeedBack != null && _usedConfirmFeedBack.SampleNum.HasValue && _usedConfirmFeedBack.SampleNum.Value > 0)
            {
                // 样品反馈，记录样品数量
                sampleCount = _usedConfirmFeedBack.SampleNum;
            }
            IList<CostDeductEquipmentOpenFund> costDeductEquipmentOpenFunds = null;
            // 检查使用用户及设备
            var user = _userBLL.GetUserByLabel(__userLabel);
            if (user == null) throw new ArgumentException(string.Format("找不到标识为【{0}】的用户信息", __userLabel));
            var equipment = _equipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("Label=\"{0}\"", __equipmentLabel));
            if (equipment == null) throw new ArgumentException(string.Format("找不到标识为【{0}】的设备信息", __equipmentLabel));
            if (__usedConfirmId == Guid.Empty)
            {
                usedConfirm = GenerateNewUsedConfirm(user,equipment,sampleCount,out errorMessage);
            }
            else
            {
                usedConfirm = _usedConfirmBLL.GetEntityById(__usedConfirmId.Value);
                if (usedConfirm == null)
                {
                    usedConfirm = GenerateNewUsedConfirm(user, equipment, sampleCount, out errorMessage);
                    usedConfirm.Id = __usedConfirmId.Value;
                }
                else
                {
                    _usedConfirmBLL.GenerateRelativeAppointmentUsedConfirm(usedConfirm.GetAppointment(), usedConfirm);
                }
            }
            usedConfirm.BeginAt = usedConfirm.BeginAt.HasValue ? usedConfirm.BeginAt < __startAt ? usedConfirm.BeginAt : __startAt : __startAt;
            usedConfirm.EndAt = usedConfirm.EndAt.HasValue ? usedConfirm.EndAt > __endAt ? usedConfirm.EndAt : __endAt : __endAt;
            if (usedConfirm.EndAt.HasValue && usedConfirm.BeginAt.HasValue && usedConfirm.EndAt.Value < usedConfirm.BeginAt.Value)
            {
                usedConfirm.EndAt = null;
            }
            if (sampleCount.HasValue) usedConfirm.SampleCount = sampleCount;
            //重新计算费用,处理之前的使用记录是小于最小扣费时间不进行扣费造成usedConfirm.CalFee为零的情况
            if (usedConfirm.BeginAt.HasValue && usedConfirm.EndAt.HasValue && usedConfirm.CostDeduct == null)
            {
                double calDuration = -1;
                double? unitPrice = null;
                double? realEquipmentOpenFundDiscout = null;
                usedConfirm.RealFee = _chargeStandardBLL.GetUsedConfirmCalFee(
                                        usedConfirm,
                                        out calDuration,
                                        out unitPrice,
                                        out isOpenFundCostDeduct,
                                        out costDeductEquipmentOpenFunds,out realEquipmentOpenFundDiscout);
                usedConfirm.CalcDuration = calDuration == -1d ? (usedConfirm.EndAt.Value - usedConfirm.BeginAt.Value).TotalHours : calDuration;
            }
            if (usedConfirm.RealFee == null) usedConfirm.RealFee = 0d;
            GenerateUsedConfirmRelativeFeedBack(usedConfirm);
            return usedConfirm;
        }

        public override object[] Deduct(ref august.DataLink.XTransaction tran)
        {
            string errorMessage = "";
            var isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            var usedConfirm = CreateUseConfirm(out errorMessage);
            if (usedConfirm.RealFee == null) usedConfirm.RealFee = 0d;
            usedConfirm.WhyNotCostDeduct += string.IsNullOrWhiteSpace(usedConfirm.WhyNotCostDeduct) ? errorMessage : "\r\n" + errorMessage;
            var user = _userBLL.GetUserByLabel(__userLabel);
            //没指定导师的学生用户不进行扣费，只产生使用记录
            if (user.UserType != null && user.UserType.UserIdentityEnum == UserIdentity.Student
                && (!user.TutorId.HasValue // 没指定导师
                && !user.IsUndergraduate() // 非本科生
                )) //本科生
            {
                __isDeduct = false;
            }
            try
            {
                if (__usedConfirmId == Guid.Empty)
                {
                    var usedConfirmTemp = _usedConfirmBLL.GetEntityById(usedConfirm.Id);
                    if (usedConfirmTemp == null)
                    {
                        usedConfirm = AddUsedConfirm(null,user, __userAccount, __startAt, __endAt, "", null, ref tran, __isDeduct);
                    }
                    else if (__endAt > __startAt)
                    {
                        usedConfirm = UpdateUseTime(null,usedConfirm, user, __startAt, __endAt, null, ref tran,_usedConfirmFeedBack ,__isDeduct);
                    }
                }
                else if (__endAt >= __startAt)
                {
                    if (usedConfirm.Status == (int)UsedConfirmStatus.UnComplete) usedConfirm.Status = (int)UsedConfirmStatus.Complete;
                    usedConfirm = UpdateUseTime(null,usedConfirm, user, __startAt, __endAt, null, ref tran,_usedConfirmFeedBack ,__isDeduct);
                }
                if (!isSupress && tran.HasTransaction) tran.CommitTransaction();
                //通知
                if (usedConfirm != null && usedConfirm.CostDeduct != null)
                {
                    _messageHandler.SendUsedConfirmCostDeductNotice(usedConfirm, null);
                    _messageHandler.SendDepositNotice(usedConfirm.CostDeduct, usedConfirm, null);
                }
                SendErrorChargedTypeNotice(usedConfirm);
            }
            catch (Exception ex)
            {
                if (!isSupress && tran.HasTransaction) tran.RollbackTransaction();
                errorMessage = ex.Message;
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
            return new object[] { usedConfirm };
        }
       
    }
}
