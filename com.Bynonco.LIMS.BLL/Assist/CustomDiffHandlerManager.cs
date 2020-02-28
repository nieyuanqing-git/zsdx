using System;
using System.Linq;
using com.august.DataLink;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 客户差异处理管理 </summary>
    public static class CustomDiffHandlerManager
    {
        private static ICustomDiffHandler customDiffHandler = new HDSFDX(new DefaultCustomDiffHandler());
        /// <summary> 同步用户 </summary>
        /// <param name="label">用户登录账号</param>
        /// <returns>用户</returns>
        public static User SycnUser(string label)
        {
            return customDiffHandler.SycnUser(label);
        }

        /// <summary> 创建开放基金申请业务逻辑接口 </summary>
        /// <returns>开放基金申请业务逻辑接口</returns>
        public static IOpenFundApplyBLL CreateOpenFundApplyBLL()
        {
            return customDiffHandler.CreateOpenFundApplyBLL();
        }

        ///// <summary> 创建金额验证业务逻辑 </summary>
        ///// <returns>金额验证业务逻辑接口</returns>
        //public static IMoneyValidateBLL CreateMoneyValidateBLL()
        //{
        //    return customDiffHandler.CreateMoneyValidateBLL();
        //}
    }

    /// <summary> 客户差异处理接口（装饰者模式） </summary>
    public interface ICustomDiffHandler
    {
        /// <summary> 同步用户 </summary>
        /// <param name="label">用户登录账号</param>
        /// <returns>用户</returns>
        User SycnUser(string label);
        /// <summary> 创建开放基金申请业务逻辑 </summary>
        /// <returns>开放基金申请业务逻辑接口</returns>
        IOpenFundApplyBLL CreateOpenFundApplyBLL();
        ///// <summary> 创建金额验证业务逻辑 </summary>
        ///// <returns>金额验证业务逻辑接口</returns>
        //IMoneyValidateBLL CreateMoneyValidateBLL();
    }

    /// <summary> 默认客户差异处理 </summary>
    public class DefaultCustomDiffHandler : ICustomDiffHandler
    {
        /// <summary> 同步用户 </summary>
        /// <param name="label">用户登录账号</param>
        /// <returns>用户</returns>
        public virtual User SycnUser(string label)
        {
            return null;
        }

        /// <summary> 开放基金申请业务逻辑接口（内存） </summary>
        private IOpenFundApplyBLL openFundApplyBll;
        /// <summary> 创建开放基金申请业务逻辑 </summary>
        /// <returns>开放基金申请业务逻辑接口</returns>
        public virtual IOpenFundApplyBLL CreateOpenFundApplyBLL()
        {
            if (openFundApplyBll == null)
            {
                openFundApplyBll = new OpenFundApplyBLL();
            }
            return openFundApplyBll;
        }

        /// <summary> 金额验证业务逻辑接口（内存） </summary>
        private IMoneyValidateBLL moneyValidateBll;
        /// <summary> 创建金额验证业务逻辑接口 </summary>
        /// <returns>金额验证业务逻辑接口</returns>
        public IMoneyValidateBLL CreateMoneyValidateBLL()
        {
            if (moneyValidateBll == null)
            {
                moneyValidateBll = BLLFactory.CreateMoneyValidateBLL();
            }
            return moneyValidateBll;
        }
    }

    /// <summary> 华东师范大学 </summary>
    public class HDSFDX : ICustomDiffHandler
    {
        private readonly ICustomDiffHandler _customDiffHandler;
        public const string SchoolName = "华东师范大学";
        private bool? isHDSFDX;

        public HDSFDX(ICustomDiffHandler customDiffHandler)
        {
            _customDiffHandler = customDiffHandler;
        }

        /// <summary> 是否华东师范大学判断 </summary>
        private bool IsHDSFDX
        {
            get
            {
                if (!isHDSFDX.HasValue)
                {
                    var schoolName = BLLFactory.CreateDictCodeBLL().GetDictCodeByCode("System", "SchoolName");
                    isHDSFDX = (schoolName != null&&schoolName.Value == SchoolName);
                }
                return isHDSFDX.Value;
            }
        }

        /// <summary> 同步用户 </summary>
        /// <param name="label">用户登录账号</param>
        /// <returns>用户</returns>
        public virtual User SycnUser(string label)
        {
            return CheckCustom(() => GetHDSFDXUser(label), () => _customDiffHandler.SycnUser(label));
        }

        /// <summary> 创建开放基金申请业务逻辑 </summary>
        /// <returns>开放基金申请业务逻辑接口</returns>
        public virtual IOpenFundApplyBLL CreateOpenFundApplyBLL()
        {
            return CheckCustom(HDSFDXOpenFundApplyBLL, _customDiffHandler.CreateOpenFundApplyBLL);
        }

        ///// <summary> 创建金额验证业务逻辑 </summary>
        ///// <returns>金额验证业务逻辑接口</returns>
        //public IMoneyValidateBLL CreateMoneyValidateBLL()
        //{
        //    return CheckCustom(HDSFDXMoneyValidateBLL, _customDiffHandler.CreateMoneyValidateBLL);
        //}

        /// <summary> 检查是否华东师范大学客户 </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="handlerFunc">处理函数</param>
        /// <param name="defaultValueFunc">默认值函数</param>
        /// <returns>实体类</returns>
        public T CheckCustom<T>(Func<T> handlerFunc, Func<T> defaultValueFunc = null)
        {
            if (IsHDSFDX)
            {
                return handlerFunc();
            }
            return defaultValueFunc == null ? default(T) : defaultValueFunc();
        }

        /// <summary> 开放基金申请业务逻辑接口（内存） </summary>
        private IOpenFundApplyBLL openFundApplyBll;
        /// <summary> 华东师范大学开放基金申请业务逻辑 </summary>
        /// <returns></returns>
        private IOpenFundApplyBLL HDSFDXOpenFundApplyBLL()
        {
            if (openFundApplyBll == null)
            {
                openFundApplyBll = new HDSFDXOpenFundApplyBLL();
            }
            return openFundApplyBll;
        }

        ///// <summary> 金额验证业务逻辑接口（内存） </summary>
        //private IMoneyValidateBLL moneyValidateBll;
        ///// <summary> 创建金额验证业务逻辑 </summary>
        ///// <returns>金额验证业务逻辑接口</returns>
        //public IMoneyValidateBLL HDSFDXMoneyValidateBLL()
        //{
        //    if (moneyValidateBll == null)
        //    {
        //        moneyValidateBll = new HDSFDXMoneyValidateBLL();
        //    }
        //    return moneyValidateBll;
        //}

        /// <summary>获取华东师范大学用户</summary>
        /// <returns></returns>
        private User GetHDSFDXUser(string label)
        {
            var schoolUserBll = BLLFactory.CreateSchoolUserBLL();
            string errorMsg;
            int syncCountPerTime = 1, totalCount, successCount, failCount;
            //string queryExpression = string.Format("LoginName=\"{0}\"*IsHandled=false", label);
            string queryExpression = string.Format("LoginName=\"{0}\"", label);
            schoolUserBll.SyncSchoolUser(queryExpression, syncCountPerTime, null, out totalCount, out successCount, out failCount, out errorMsg);
            // 成功找到用户并处理，更新所属单位
            if (failCount == 0 && successCount > 0)
            {
                var userBLL = BLLFactory.CreateUserBLL();
                var user = userBLL.GetUserByLabel(label);
                SyncHDSFDXOranization(user);
                return user;
            }
            return null;
        }

        /// <summary> 同步用户所属单位 </summary>
        /// <param name="user">用户</param>
        private void SyncHDSFDXOranization(User user)
        {
            int syncCountPerTime = 1;
            // 用户已处理
            //string queryExpression = string.Format("LoginName=\"{0}\"*IsHandled=true", user.Label);
            string queryExpression = string.Format("LoginName=\"{0}\"", user.Label);
            IViewSchoolUserBLL viewSchoolUserBLL = BLLFactory.CreateViewSchoolUserBLL();
            var viewSchoolUser = viewSchoolUserBLL.GetUnsyncViewSchoolUserList(queryExpression, syncCountPerTime).FirstOrDefault();
            if (viewSchoolUser == null) return;
            var organizationName = viewSchoolUser.OrganizationName;
            // 强制更新单位名称
            user.OrganizationName = organizationName;
            var organization = BLLFactory.CreateLabOrganizationBLL().GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"", organizationName));
            if (organization != null)
            {
                user.OrganizationId = organization.Id;
            }
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var userBLL = BLLFactory.CreateUserBLL();
                userBLL.Save(new[] { user }, ref tran);
                tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
            }
            finally
            {
                tran.Dispose();
            }
            //OranizationSyncManager.GetInstance(syncCountPerTime, "").Sync(out totalCount, out successCount, out failCount, out skipCount, out errorMsg);
        }
    }
}