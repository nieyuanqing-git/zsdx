using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary>成本扣费基类</summary>
    public abstract class CostDeductBase
    {
        /// 业务对象
        /// <summary>
        /// 业务对象
        /// </summary>
        protected object[] _businessObjects;
        /// 成本扣费业务逻辑
        /// <summary>
        /// 成本扣费业务逻辑
        /// </summary>
        protected ICostDeductBLL _costDeductBLL;
        /// 手工成本扣费业务逻辑
        /// <summary>
        /// 手工成本扣费业务逻辑
        /// </summary>
        protected IManualCostDeductBLL _manualCostDeductBLL;
        /// 耗材输出业务逻辑
        /// <summary>
        /// 耗材输出业务逻辑
        /// </summary>
        protected IMaterialOutputBLL _materialOutputBLL;
        /// 用户账号业务逻辑
        /// <summary>
        /// 用户账号业务逻辑
        /// </summary>
        protected IUserAccountBLL _userAccountBLL;
        /// 用户业务逻辑
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        protected IUserBLL _userBLL;
        /// 设备使用记录反馈业务逻辑
        /// <summary>
        /// 设备使用记录反馈业务逻辑
        /// </summary>
        protected IUsedConfirmFeedBackBLL _usedconfirmFeedBackBLL;
        /// 消息处理
        /// <summary>
        /// 消息处理
        /// </summary>
        protected IMessageHandler _messageHandler;
        /// 金额验证业务逻辑
        /// <summary>
        /// 金额验证业务逻辑
        /// </summary>
        protected IMoneyValidateBLL _moneyValidateBLL;
        /// 动物成本扣费业务逻辑
        /// <summary>
        /// 动物成本扣费业务逻辑
        /// </summary>
        protected IAnimalCostDeductBLL _animalCostDeductBLL;
        /// 水控成本扣费业务逻辑
        /// <summary>
        /// 水控成本扣费业务逻辑
        /// </summary>
        protected IWaterControlCostDeductBLL _waterControlCostDeductBLL;
        /// 成本扣费的考核评价业务逻辑
        /// <summary>
        /// 成本扣费的考核评价业务逻辑
        /// </summary>
        protected ICostDeductForStatisticsBLL _costDeductForStatisticsBLL;
        /// 操作员ID
        /// <summary>
        /// 操作员ID
        /// </summary>
        protected Guid? _operatorId;
        /// 操作员客户端IP
        /// <summary>
        /// 操作员客户端IP
        /// </summary>
        protected string _operateIP;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="businessObjects">业务对象数组</param>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="operateIP">操作员客户端IP</param>
        public CostDeductBase(object[] businessObjects, Guid? operatorId, string operateIP)
        {
            _costDeductBLL = BLLFactory.CreateCostDeductBLL();
            _manualCostDeductBLL = BLLFactory.CreateManualCostDeductBLL();
            _materialOutputBLL = BLLFactory.CreateMaterialOutputBLL();
            _userAccountBLL = BLLFactory.CreateUserAccountBLL();
            _usedconfirmFeedBackBLL = BLLFactory.CreateUsedConfirmFeedBackBLL();
            _animalCostDeductBLL = BLLFactory.CreateAnimalCostDeductBLL();
            _waterControlCostDeductBLL = BLLFactory.CreateWaterControlCostDeductBLL();
            _costDeductForStatisticsBLL = BLLFactory.CreateCostDeductForStatisticsBLL();
            _userBLL = BLLFactory.CreateUserBLL();
            _moneyValidateBLL = BLLFactory.CreateMoneyValidateBLL();
            _messageHandler = new MessageHandler();
            this._businessObjects = businessObjects;
            this._operateIP = operateIP;
            this._operatorId = operatorId;
        }
        /// 检查金额
        /// <summary>
        /// 检查金额
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckMoney()
        {
            return true;
        }
        /// 发送扣费消息
        /// <summary>
        /// 发送扣费消息
        /// </summary>
        /// <param name="userAccount">用户账号</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>是否成功</returns>
        protected virtual bool SendDeductMessage(UserAccount userAccount, out string errorMessage)
        {
            errorMessage = "";
            return true;
        }
        /// 扣费
        /// <summary>
        /// 扣费
        /// </summary>
        /// <param name="tran">事务</param>
        /// <returns>结果数组</returns>
        public abstract object[] Deduct(ref XTransaction tran);
        /// 取消扣费
        /// <summary>
        /// 取消扣费
        /// </summary>
        /// <param name="tran">事务</param>
        /// <returns>结果数组</returns>
        public abstract object[] CancelDeduct(ref XTransaction tran);
        /// 判断是否允许扣费
        /// <summary>
        /// 判断是否允许扣费
        /// </summary>
        /// <param name="subjectId">课题组</param>
        /// <param name="userAccount">用户账号</param>
        /// <param name="isDeduct">是否扣费</param>
        /// <param name="beginAt">开始时间</param>
        /// <param name="endAt">结束时间</param>
        /// <param name="chargeStandard">计费标准</param>
        /// <param name="calFeeTimeRule">计费时间规则</param>
        /// <param name="usedConfirmStatus">使用记录状态</param>
        /// <param name="minUnchangeMinutes">最小扣费时间分钟</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>是否允许扣费</returns>
        protected virtual bool JudgeIsEnableCostDeduct(Guid? subjectId, UserAccount userAccount, bool isDeduct, DateTime? beginAt, DateTime? endAt, IChargeStandard chargeStandard, ICalcFeeTimeRule calFeeTimeRule, UsedConfirmStatus usedConfirmStatus, int? minUnchangeMinutes, out string errorMessage)
        {
            errorMessage = "";
            if (!subjectId.HasValue)
            {
                errorMessage = "未绑定课题组,无法进行扣费";
                return false;
            }
            if (!isDeduct)
            {
                errorMessage = "设置管理员不进行扣费";
                return false;
            }
            if (userAccount == null)
            {
                errorMessage = "付费人帐户为空";
                return false;
            }
            if (beginAt.HasValue && !endAt.HasValue)
            {
                errorMessage = "只有开机记录，没有下机记录";
                return false;
            }
            if (chargeStandard.ChargeTypeEnum == ChargeType.ByHour && endAt.HasValue && (endAt.Value - beginAt.Value).TotalMinutes - (minUnchangeMinutes.HasValue ? minUnchangeMinutes.Value : 0) < 0d)
            {
                errorMessage = string.Format("使用时间【{0}分钟】小于等于最小扣费时间【{1}分钟】",
                                                             Math.Round((endAt.Value - beginAt.Value).TotalMinutes, 2),
                                                             (minUnchangeMinutes.HasValue ? minUnchangeMinutes.Value : 0));
                return false;
            }
            if (chargeStandard == null)
            {
                errorMessage = "计费标准为空";
                return false;
            }
            if (calFeeTimeRule == null)
            {
                errorMessage = "计费时间规则为空";
                return false;
            }
            if (usedConfirmStatus == UsedConfirmStatus.ClosingAccount)
            {
                errorMessage = "该使用记录已结算";
                return false;
            }
            return true;
        }
    }
}
