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
    public class ViewPunishActionBLL : BLLBase<ViewPunishAction>, IViewPunishActionBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewPunishActionBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }

        public void OperateDecorate(Guid? userId, ViewPunishAction viewPunishRecord, DelinquencyPrivilige delinquencyPrivilige)
        {
            if (viewPunishRecord == null) throw new ArgumentException("不良行为记录为空");
            if (delinquencyPrivilige == null)
            {
                viewPunishRecord.StopOperate = "";
                viewPunishRecord.SendMessageOperate = "";
                viewPunishRecord.ViewOperate = "";
                return;
            }
            if (!delinquencyPrivilige.IsEnableStopPunish || viewPunishRecord.IsStop)
            {
                viewPunishRecord.StopOperate = "";
            }
            if (!delinquencyPrivilige.IsEnableResend)
            {
                viewPunishRecord.SendMessageOperate = "";
            }
            if (!delinquencyPrivilige.IsEnablePunishDetail)
            {
                viewPunishRecord.ViewOperate = "";
            }
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewPunishAction> viewPunishActionList)
        {
            if (viewPunishActionList == null || viewPunishActionList.Count == 0 || !userId.HasValue) return;
            DelinquencyPrivilige delinquencyPrivilige = PriviligeFactory.GetDelinquencyPrivilige(userId.Value);
            foreach (var viewPunishAction in viewPunishActionList)
            {
                viewPunishAction.InitOperate();
                OperateDecorate(userId, viewPunishAction, delinquencyPrivilige);
                viewPunishAction.BuildOperate();
            }
        }
        public IList<ViewPunishAction> GetViewPunishActionListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewPunishActionList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
            ExcuteOperateDecorate(userId, viewPunishActionList);
            return viewPunishActionList;
        }
        public IList<ViewPunishAction> GetViewPunishActionByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewPunishActionList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
            ExcuteOperateDecorate(userId, viewPunishActionList);
            return viewPunishActionList;
        }
        public IList<ViewPunishAction> GetViewPunishActionListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            if (!IsManagePunishAction(userId, ref dataGridSettings)) return null;
            var viewPunishActionList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
            ExcuteOperateDecorate(userId, viewPunishActionList);
            return viewPunishActionList;
        }
        public GridData<ViewPunishAction> GetGridViewPunishActionListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManagePunishAction(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewPunishActionList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
            ExcuteOperateDecorate(userId, viewPunishActionList == null ? null : viewPunishActionList.Data);
            return viewPunishActionList;
        }
        public GridData<ViewPunishAction> GetGridViewPunishActionListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            if (!IsManagePunishAction(userId, ref dataGridSettings)) return null;
            var viewPunishActionList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
            ExcuteOperateDecorate(userId, viewPunishActionList == null ? null : viewPunishActionList.Data);
            return viewPunishActionList;
        }
        private bool IsManagePunishAction(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
                dataGridSettings.AppendAndQueryExpression(__workGroupModuleBLL.GetQueryManagerUserOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.PunishAction, "", "", "PunisherOrgId")));
            return true;
        }

        public IList<PunishTotalInfo> GetPunishTotalInfo(Guid? userId, DataGridSettings dataGridSettings)
        {
            var queryExpression  = dataGridSettings.QueryExpression;
            var viewPunishRecordList = GetViewPunishActionListByExpression(userId, dataGridSettings.AppendAndQueryExpression("DelinquencyRuleName=-null"), null, "", true);
            dataGridSettings.QueryExpression = queryExpression;
            var viewDelinquencyConfirmList = BLLFactory.CreateViewDelinquencyConfirmBLL().GetViewDelinquencyConfirmListByExpression(userId, dataGridSettings.AppendAndQueryExpression("HasRepeal=false"), null, "", true);
            IList<Guid> userIdList = new List<Guid>();
            if (viewPunishRecordList != null && viewPunishRecordList.Count() > 0)
            {
                var viewPunishRecordGroupList = viewPunishRecordList.GroupBy(p => p.PunisherId);
                if (viewPunishRecordGroupList != null && viewPunishRecordGroupList.Count() > 0)
                {
                    foreach (var item in viewPunishRecordGroupList)
                    {
                        if (item.Key.HasValue && !userIdList.Contains(item.Key.Value)) userIdList.Add(item.Key.Value);
                    }
                }
            }
            if (viewDelinquencyConfirmList != null && viewDelinquencyConfirmList.Count() > 0)
            {
                var viewDelinquencyConfirmGroupList = viewDelinquencyConfirmList.GroupBy(p => p.PunisherId);
                if (viewDelinquencyConfirmGroupList != null && viewDelinquencyConfirmGroupList.Count() > 0)
                {
                    foreach (var item in viewDelinquencyConfirmGroupList)
                    {
                        if (item.Key.HasValue && !userIdList.Contains(item.Key.Value)) userIdList.Add(item.Key.Value);
                    }
                }
            }
            if (userIdList.Count() == 0) return null;
            IList<PunishTotalInfo> lstPunishTotalInfo = new List<PunishTotalInfo>();
            foreach (var item in userIdList)
            {
                PunishTotalInfo punishTotalInfo = new PunishTotalInfo();
                var totalActionList = viewPunishRecordList == null ? null : viewPunishRecordList.Where(p => p.PunisherId == item).ToList();
                var totalDelinquencyList = viewDelinquencyConfirmList == null ? null : viewDelinquencyConfirmList.Where(p => p.PunisherId == item).ToList();

                punishTotalInfo.UserId = item;
                punishTotalInfo.UserName = totalActionList != null && totalActionList.Count() > 0 ? totalActionList[0].PunisherName : totalDelinquencyList[0].PunisherName;
                punishTotalInfo.TotalAction = totalActionList == null ? 0 : totalActionList.Count();
                punishTotalInfo.TotalDelinquency = totalDelinquencyList == null ? 0 : totalDelinquencyList.Count();
                punishTotalInfo.TotalYearAction = totalActionList == null ? 0 : totalActionList.Where(p => p.BeginAt.Year == DateTime.Now.Year && (!p.EndAt.HasValue || (p.EndAt.HasValue && p.EndAt.Value.Year == DateTime.Now.Year))).Count();
                punishTotalInfo.TotalYearDelinquency = totalDelinquencyList == null ? 0 : totalDelinquencyList.Where(p => p.DelinquencyAt.Year == DateTime.Now.Year).Count();
                punishTotalInfo.TotalStr = (punishTotalInfo.TotalAction + punishTotalInfo.TotalDelinquency + punishTotalInfo.TotalYearAction + punishTotalInfo.TotalYearDelinquency).ToString();
                lstPunishTotalInfo.Add(punishTotalInfo);
            }
            if (lstPunishTotalInfo != null) lstPunishTotalInfo = lstPunishTotalInfo.OrderBy(p => p.UserName).ToList();
            return lstPunishTotalInfo;
        }
        public GridData<ViewPunishAction> GetGridViewPunishActionListByExpression(Guid? userId, Guid punisherId, PunishTotalType punishTotalType, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false)
        {
            switch (punishTotalType)
            {
                case PunishTotalType.TotalAction:
                    dataGridSettings.AppendAndQueryExpression(string.Format("PunisherId=\"{0}\"", punisherId.ToString()));
                    break;
                case PunishTotalType.TotalYearAction:
                    DateTime beginAt = DateTime.Parse(string.Format("{0}-01-01", DateTime.Now.Year));
                    DateTime endAt = DateTime.Parse(string.Format("{0}-01-01", DateTime.Now.Year)).AddYears(1).AddSeconds(-1);
                    dataGridSettings.AppendAndQueryExpression(string.Format("PunisherId=\"{0}\"*BeginAt>\"{1}\"*(EndAt>\"{2}\"+EndAt=null)", punisherId.ToString(), beginAt.ToString("yyyy-MM-dd HH:mm:ss"), endAt.ToString("yyyy-MM-dd HH:mm:ss")));
                    break;
            }
            return GetGridViewPunishActionListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct);
        }

    }
}
