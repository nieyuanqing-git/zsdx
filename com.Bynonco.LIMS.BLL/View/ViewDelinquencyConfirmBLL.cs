using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business.Enum;
using System.Data.SqlClient;
using com.Bynonco.LIMS.DAL;
using com.august.DataLink;
using System.Data;


namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewDelinquencyConfirmBLL : BLLBase<ViewDelinquencyConfirm>, IViewDelinquencyConfirmBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IDelinquencyConfirmBLL __delinquencyConfirmBLL;
        public ViewDelinquencyConfirmBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __delinquencyConfirmBLL = BLLFactory.CreateDelinquencyConfirmBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        public void OperateDecorate(Guid? userId, ViewDelinquencyConfirm viewDelinquencyConfirm, DelinquencyPrivilige delinquencyPrivilige)
        {
            if (viewDelinquencyConfirm == null) throw new ArgumentException("不良行为为空");
            if (delinquencyPrivilige == null || !delinquencyPrivilige.IsEnableDeleteDelinquencyConfirm || viewDelinquencyConfirm.HasRepeal)
            {
                viewDelinquencyConfirm.DeleteOperate = "";
            }
            if (delinquencyPrivilige == null || !delinquencyPrivilige.IsEnableSendDelinquencyMsgNotice)
            {
                viewDelinquencyConfirm.SendMessageOperate = "";
                return;
            }
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewDelinquencyConfirm> viewDelinquencyConfirmList)
        {
            if (viewDelinquencyConfirmList == null || viewDelinquencyConfirmList.Count == 0 || !userId.HasValue) return;
            IList<DelinquencyPrivilige> lstDelinquencyPriviliges = (IList<DelinquencyPrivilige>)__delinquencyConfirmBLL.GetDelinquencyPriviliges(userId, viewDelinquencyConfirmList);
            foreach (var viewDelinquencyConfirm in viewDelinquencyConfirmList)
            {
                DelinquencyPrivilige delinquencyPrivilige = lstDelinquencyPriviliges.FirstOrDefault(p => p.PunisherId.HasValue && p.PunisherId == viewDelinquencyConfirm.PunisherId);
                viewDelinquencyConfirm.InitOperate();
                OperateDecorate(userId, viewDelinquencyConfirm, delinquencyPrivilige);
                viewDelinquencyConfirm.BuildOperate();
            }
        }
        public IList<ViewDelinquencyConfirm> GetViewDelinquencyConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDelinquencyConfirmList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewDelinquencyConfirmList);
            return viewDelinquencyConfirmList;
        }
        public IList<ViewDelinquencyConfirm> GetViewDelinquencyConfirmListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDelinquencyConfirmList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewDelinquencyConfirmList);
            return viewDelinquencyConfirmList;
        }
        public IList<ViewDelinquencyConfirm> GetViewDelinquencyConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManagePunishAction(userId, ref dataGridSettings)) return null;
            var viewDelinquencyConfirmList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewDelinquencyConfirmList);
            return viewDelinquencyConfirmList;
        }
        public GridData<ViewDelinquencyConfirm> GetGridViewDelinquencyConfirmListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewDelinquencyConfirmList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewDelinquencyConfirmList == null ? null : viewDelinquencyConfirmList.Data);
            return viewDelinquencyConfirmList;
        }
        public GridData<ViewDelinquencyConfirm> GetGridViewDelinquencyConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManagePunishAction(userId, ref dataGridSettings)) return null;
            var viewDelinquencyConfirmList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewDelinquencyConfirmList == null ? null : viewDelinquencyConfirmList.Data);
            return viewDelinquencyConfirmList;
        }
        private bool IsManagePunishAction(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.DelinquencyConfirm, "", "", "PunisherOrgId")));
            return true;
        }
        public GridData<ViewDelinquencyConfirm> GetGridViewDelinquencyConfirmListByExpression(Guid? userId, DataGridSettings dataGridSettings, Guid punisherId, PunishTotalType punishTotalType, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            switch (punishTotalType)
            {
                case PunishTotalType.TotalDelinquency:
                    dataGridSettings.AppendAndQueryExpression(string.Format("PunisherId=\"{0}\"", punisherId.ToString()));
                    break;
                case PunishTotalType.TotalYearDelinquency:
                    DateTime beginAt = DateTime.Parse(string.Format("{0}-01-01", DateTime.Now.Year));
                    DateTime endAt = DateTime.Parse(string.Format("{0}-01-01", DateTime.Now.Year)).AddYears(1).AddSeconds(-1);
                    dataGridSettings.AppendAndQueryExpression(string.Format("PunisherId=\"{0}\"*DelinquencyAt>\"{1}\"*DelinquencyAt<\"{2}\"", punisherId.ToString(), beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss")));
                    break;
            }
            return GetGridViewDelinquencyConfirmListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping,false);
        }
    }
}
