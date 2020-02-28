using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class StatisticsPrivilige : PriviligeBase
    {
        public StatisticsPrivilige(Guid userId)
            : base(userId) { }
        protected override IList<ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            return _viewUserWorkGroupModuleFunctionBLL.GetModuleFunctionListByWorkGroupIdsControllerName(_workGroupIdList, "Statistics");
        }
        protected override void BuildPrivilige()
        {
            var viewModuleFunctionList = GetUserWorkGroupModuleList();
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count == 0) return;

            #region 教学科研仪器设备表(SJ1)
            IsEnableSJ1Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Manage");
            IsEnableSJ1StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsAdd");
            IsEnableSJ1StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsAudit");
            IsEnableSJ1StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsAuditPass");
            IsEnableSJ1StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsAuditReject");
            IsEnableSJ1StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsAuditWaitting");
            IsEnableSJ1StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsDelete");
            IsEnableSJ1StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsExportDoc");
            IsEnableSJ1StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1StatisticsExportTxt");
            IsEnableSJ1Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Add");
            IsEnableSJ1Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Edit");
            IsEnableSJ1Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Save");
            IsEnableSJ1Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Audit");
            IsEnableSJ1AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1AuditPass");
            IsEnableSJ1AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1AuditReject");
            IsEnableSJ1AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1AuditWaitting");
            IsEnableSJ1Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ1Delete");
            #endregion

            #region 教学科研仪器设备增减变动情况表(SJ2)
            IsEnableSJ2Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Manage");
            IsEnableSJ2StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsAdd");
            IsEnableSJ2StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsAudit");
            IsEnableSJ2StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsAuditPass");
            IsEnableSJ2StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsAuditReject");
            IsEnableSJ2StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsAuditWaitting");
            IsEnableSJ2StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsDelete");
            IsEnableSJ2StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsExportDoc");
            IsEnableSJ2StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2StatisticsExportTxt");
            IsEnableSJ2Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Add");
            IsEnableSJ2Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Edit");
            IsEnableSJ2Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Save");
            IsEnableSJ2Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Audit");
            IsEnableSJ2AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2AuditPass");
            IsEnableSJ2AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2AuditReject");
            IsEnableSJ2AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2AuditWaitting");
            IsEnableSJ2Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ2Delete");
            #endregion

            #region 贵重仪器设备表(SJ3)
            IsEnableSJ3Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Manage");
            IsEnableSJ3StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsAdd");
            IsEnableSJ3StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsAudit");
            IsEnableSJ3StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsAuditPass");
            IsEnableSJ3StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsAuditReject");
            IsEnableSJ3StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsAuditWaitting");
            IsEnableSJ3StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsDelete");
            IsEnableSJ3StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsExportDoc");
            IsEnableSJ3StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsExportTxt");
            IsEnableSJ3StatisticsExportExcel = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsExportExcel");
            IsEnableSJ3StatisticsImport = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3StatisticsImport");
            IsEnableSJ3Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Add");
            IsEnableSJ3Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Edit");
            IsEnableSJ3Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Save");
            IsEnableSJ3Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Audit");
            IsEnableSJ3AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3AuditPass");
            IsEnableSJ3AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3AuditReject");
            IsEnableSJ3AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3AuditWaitting");
            IsEnableSJ3Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ3Delete");
            #endregion

            #region 教学实验项目表(SJ4)
            IsEnableSJ4Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Manage");
            IsEnableSJ4StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsAdd");
            IsEnableSJ4StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsAudit");
            IsEnableSJ4StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsAuditPass");
            IsEnableSJ4StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsAuditReject");
            IsEnableSJ4StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsAuditWaitting");
            IsEnableSJ4StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsDelete");
            IsEnableSJ4StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsExportDoc");
            IsEnableSJ4StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4StatisticsExportTxt");
            IsEnableSJ4Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Add");
            IsEnableSJ4Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Edit");
            IsEnableSJ4Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Save");
            IsEnableSJ4Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Audit");
            IsEnableSJ4AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4AuditPass");
            IsEnableSJ4AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4AuditReject");
            IsEnableSJ4AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4AuditWaitting");
            IsEnableSJ4Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ4Delete");
            #endregion

            #region 专任实验室人员表(SJ5)
            IsEnableSJ5Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Manage");
            IsEnableSJ5StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsAdd");
            IsEnableSJ5StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsAudit");
            IsEnableSJ5StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsAuditPass");
            IsEnableSJ5StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsAuditReject");
            IsEnableSJ5StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsAuditWaitting");
            IsEnableSJ5StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsDelete");
            IsEnableSJ5StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsExportDoc");
            IsEnableSJ5StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5StatisticsExportTxt");
            IsEnableSJ5Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Add");
            IsEnableSJ5Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Edit");
            IsEnableSJ5Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Save");
            IsEnableSJ5Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Audit");
            IsEnableSJ5AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5AuditPass");
            IsEnableSJ5AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5AuditReject");
            IsEnableSJ5AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5AuditWaitting");
            IsEnableSJ5Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ5Delete");
            #endregion

            #region 实验室基本情况表(SJ6)
            IsEnableSJ6Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Manage");
            IsEnableSJ6StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsAdd");
            IsEnableSJ6StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsAudit");
            IsEnableSJ6StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsAuditPass");
            IsEnableSJ6StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsAuditReject");
            IsEnableSJ6StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsAuditWaitting");
            IsEnableSJ6StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsDelete");
            IsEnableSJ6StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsExportDoc");
            IsEnableSJ6StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6StatisticsExportTxt");
            IsEnableSJ6Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Add");
            IsEnableSJ6Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Edit");
            IsEnableSJ6Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Save");
            IsEnableSJ6Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Audit");
            IsEnableSJ6AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6AuditPass");
            IsEnableSJ6AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6AuditReject");
            IsEnableSJ6AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6AuditWaitting");
            IsEnableSJ6Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ6Delete");
            #endregion

            #region 实验室经费情况表(SJ7)
            IsEnableSJ7Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Manage");
            IsEnableSJ7StatisticsAdd = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsAdd");
            IsEnableSJ7StatisticsAudit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsAudit");
            IsEnableSJ7StatisticsAuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsAuditPass");
            IsEnableSJ7StatisticsAuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsAuditReject");
            IsEnableSJ7StatisticsAuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsAuditWaitting");
            IsEnableSJ7StatisticsDelete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsDelete");
            IsEnableSJ7StatisticsExportDoc = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsExportDoc");
            IsEnableSJ7StatisticsExportTxt = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7StatisticsExportTxt");
            IsEnableSJ7Add = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Add");
            IsEnableSJ7Edit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Edit");
            IsEnableSJ7Save = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Save");
            IsEnableSJ7Audit = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Audit");
            IsEnableSJ7AuditPass = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7AuditPass");
            IsEnableSJ7AuditReject = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7AuditReject");
            IsEnableSJ7AuditWaitting = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7AuditWaitting");
            IsEnableSJ7Delete = GetIsEnableOpearteExtend(viewModuleFunctionList, "SJ7Delete");
            #endregion

            #region 高等学校实验室综合信息表(SZ1)
            IsEnableSZ1Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SZ1Manage");
            #endregion
            #region 高等学校实验室综合信息表(SZ2)
            IsEnableSZ2Manage = GetIsEnableOpearteExtend(viewModuleFunctionList, "SZ2Manage");
            #endregion


        }
        private bool GetIsEnableOpearteExtend(IList<ViewUserWorkGroupModuleFunction> viewModuleFunctionList, string actionName)
        {
            return IsSuperAmin || viewModuleFunctionList.Any(p => p.ActionName == actionName);
        }
        #region 教学科研仪器设备表(SJ1)
        public bool IsEnableSJ1Manage { get; private set; }
        public bool IsEnableSJ1StatisticsAdd { get; private set; }
        public bool IsEnableSJ1StatisticsEdit { get; private set; }
        public bool IsEnableSJ1StatisticsSave { get; private set; }
        public bool IsEnableSJ1StatisticsAudit { get; private set; }
        public bool IsEnableSJ1StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ1StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ1StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ1StatisticsDelete { get; private set; }
        public bool IsEnableSJ1StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ1StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ1Add { get; private set; }
        public bool IsEnableSJ1Edit { get; private set; }
        public bool IsEnableSJ1Save { get; private set; }
        public bool IsEnableSJ1Audit { get; private set; }
        public bool IsEnableSJ1AuditPass { get; private set; }
        public bool IsEnableSJ1AuditReject { get; private set; }
        public bool IsEnableSJ1AuditWaitting { get; private set; }
        public bool IsEnableSJ1Delete { get; private set; }
        #endregion

        #region 教学科研仪器设备增减变动情况表(SJ2)
        public bool IsEnableSJ2Manage { get; private set; }
        public bool IsEnableSJ2StatisticsAdd { get; private set; }
        public bool IsEnableSJ2StatisticsEdit { get; private set; }
        public bool IsEnableSJ2StatisticsSave { get; private set; }
        public bool IsEnableSJ2StatisticsAudit { get; private set; }
        public bool IsEnableSJ2StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ2StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ2StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ2StatisticsDelete { get; private set; }
        public bool IsEnableSJ2StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ2StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ2Add { get; private set; }
        public bool IsEnableSJ2Edit { get; private set; }
        public bool IsEnableSJ2Save { get; private set; }
        public bool IsEnableSJ2Audit { get; private set; }
        public bool IsEnableSJ2AuditPass { get; private set; }
        public bool IsEnableSJ2AuditReject { get; private set; }
        public bool IsEnableSJ2AuditWaitting { get; private set; }
        public bool IsEnableSJ2Delete { get; private set; }
        #endregion

        #region 贵重仪器设备表(SJ3)
        public bool IsEnableSJ3Manage { get; private set; }
        public bool IsEnableSJ3StatisticsAdd { get; private set; }
        public bool IsEnableSJ3StatisticsEdit { get; private set; }
        public bool IsEnableSJ3StatisticsSave { get; private set; }
        public bool IsEnableSJ3StatisticsAudit { get; private set; }
        public bool IsEnableSJ3StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ3StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ3StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ3StatisticsDelete { get; private set; }
        public bool IsEnableSJ3StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ3StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ3StatisticsExportExcel { get; private set; }
        public bool IsEnableSJ3StatisticsImport { get; private set; }
        public bool IsEnableSJ3Add { get; private set; }
        public bool IsEnableSJ3Edit { get; private set; }
        public bool IsEnableSJ3Save { get; private set; }
        public bool IsEnableSJ3Audit { get; private set; }
        public bool IsEnableSJ3AuditPass { get; private set; }
        public bool IsEnableSJ3AuditReject { get; private set; }
        public bool IsEnableSJ3AuditWaitting { get; private set; }
        public bool IsEnableSJ3Delete { get; private set; }
        #endregion

        #region 教学实验项目表(SJ4)
        public bool IsEnableSJ4Manage { get; private set; }
        public bool IsEnableSJ4StatisticsAdd { get; private set; }
        public bool IsEnableSJ4StatisticsEdit { get; private set; }
        public bool IsEnableSJ4StatisticsSave { get; private set; }
        public bool IsEnableSJ4StatisticsAudit { get; private set; }
        public bool IsEnableSJ4StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ4StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ4StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ4StatisticsDelete { get; private set; }
        public bool IsEnableSJ4StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ4StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ4Add { get; private set; }
        public bool IsEnableSJ4Edit { get; private set; }
        public bool IsEnableSJ4Save { get; private set; }
        public bool IsEnableSJ4Audit { get; private set; }
        public bool IsEnableSJ4AuditPass { get; private set; }
        public bool IsEnableSJ4AuditReject { get; private set; }
        public bool IsEnableSJ4AuditWaitting { get; private set; }
        public bool IsEnableSJ4Delete { get; private set; }
        #endregion

        #region 专任实验室人员表(SJ5)
        public bool IsEnableSJ5Manage { get; private set; }
        public bool IsEnableSJ5StatisticsAdd { get; private set; }
        public bool IsEnableSJ5StatisticsEdit { get; private set; }
        public bool IsEnableSJ5StatisticsSave { get; private set; }
        public bool IsEnableSJ5StatisticsAudit { get; private set; }
        public bool IsEnableSJ5StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ5StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ5StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ5StatisticsDelete { get; private set; }
        public bool IsEnableSJ5StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ5StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ5Add { get; private set; }
        public bool IsEnableSJ5Edit { get; private set; }
        public bool IsEnableSJ5Save { get; private set; }
        public bool IsEnableSJ5Audit { get; private set; }
        public bool IsEnableSJ5AuditPass { get; private set; }
        public bool IsEnableSJ5AuditReject { get; private set; }
        public bool IsEnableSJ5AuditWaitting { get; private set; }
        public bool IsEnableSJ5Delete { get; private set; }
        #endregion

        #region 实验室基本情况表(SJ6)
        public bool IsEnableSJ6Manage { get; private set; }
        public bool IsEnableSJ6StatisticsAdd { get; private set; }
        public bool IsEnableSJ6StatisticsEdit { get; private set; }
        public bool IsEnableSJ6StatisticsSave { get; private set; }
        public bool IsEnableSJ6StatisticsAudit { get; private set; }
        public bool IsEnableSJ6StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ6StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ6StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ6StatisticsDelete { get; private set; }
        public bool IsEnableSJ6StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ6StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ6Add { get; private set; }
        public bool IsEnableSJ6Edit { get; private set; }
        public bool IsEnableSJ6Save { get; private set; }
        public bool IsEnableSJ6Audit { get; private set; }
        public bool IsEnableSJ6AuditPass { get; private set; }
        public bool IsEnableSJ6AuditReject { get; private set; }
        public bool IsEnableSJ6AuditWaitting { get; private set; }
        public bool IsEnableSJ6Delete { get; private set; }
        #endregion

        #region 实验室经费情况表(SJ7)
        public bool IsEnableSJ7Manage { get; private set; }
        public bool IsEnableSJ7StatisticsAdd { get; private set; }
        public bool IsEnableSJ7StatisticsEdit { get; private set; }
        public bool IsEnableSJ7StatisticsSave { get; private set; }
        public bool IsEnableSJ7StatisticsAudit { get; private set; }
        public bool IsEnableSJ7StatisticsAuditPass { get; private set; }
        public bool IsEnableSJ7StatisticsAuditReject { get; private set; }
        public bool IsEnableSJ7StatisticsAuditWaitting { get; private set; }
        public bool IsEnableSJ7StatisticsDelete { get; private set; }
        public bool IsEnableSJ7StatisticsExportDoc { get; private set; }
        public bool IsEnableSJ7StatisticsExportTxt { get; private set; }
        public bool IsEnableSJ7Add { get; private set; }
        public bool IsEnableSJ7Edit { get; private set; }
        public bool IsEnableSJ7Save { get; private set; }
        public bool IsEnableSJ7Audit { get; private set; }
        public bool IsEnableSJ7AuditPass { get; private set; }
        public bool IsEnableSJ7AuditReject { get; private set; }
        public bool IsEnableSJ7AuditWaitting { get; private set; }
        public bool IsEnableSJ7Delete { get; private set; }
        #endregion
        #region 高等学校实验室综合信息表(SZ1)
        public bool IsEnableSZ1Manage { get; private set; }
        #endregion
        #region 高等学校实验室综合信息表(SZ2)
        public bool IsEnableSZ2Manage { get; private set; }
        #endregion

    }
}
