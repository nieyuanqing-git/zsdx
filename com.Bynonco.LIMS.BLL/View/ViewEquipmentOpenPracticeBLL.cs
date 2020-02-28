using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentOpenPracticeBLL : BLLBase<ViewEquipmentOpenPractice>, IViewEquipmentOpenPracticeBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IEquipmentOpenPracticeTeacherBLL __equipmentOpenPracticeTeacherBLL;
        public ViewEquipmentOpenPracticeBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __equipmentOpenPracticeTeacherBLL = BLLFactory.CreateEquipmentOpenPracticeTeacherBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipmentOpenPractice> viewEquipmentOpenPracticeList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentOpenPracticeList == null || viewEquipmentOpenPracticeList.Count == 0) return;
            foreach (var viewEquipmentOpenPractice in viewEquipmentOpenPracticeList)
            {
                viewEquipmentOpenPractice.IsSupressLazyLoad = false;
                viewEquipmentOpenPractice.InitOperate();
                OperateDecorate(userId, viewEquipmentOpenPractice);
                viewEquipmentOpenPractice.BuildOperate();
                viewEquipmentOpenPractice.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        public void OperateDecorate(Guid? userId, ViewEquipmentOpenPractice viewEquipmentOpenPractice)
        {
            if (viewEquipmentOpenPractice == null) throw new ArgumentException("实践研究项目信息为空");
            var equipmentOpenPracticePrivilige = userId.HasValue ?  PriviligeFactory.GetEquipmentOpenPracticePrivilige(userId.Value) : null;
            var userPrivilige = userId.HasValue ? PriviligeFactory.GetUserPrivilige(userId.Value, viewEquipmentOpenPractice.ApplyUserId) : null;
            //权限不存在时处理
            if (equipmentOpenPracticePrivilige == null)
            {
                viewEquipmentOpenPractice.ViewOperate = "";
                viewEquipmentOpenPractice.ClosedViewOperate = "";
                viewEquipmentOpenPractice.ModifyOperate = "";
                viewEquipmentOpenPractice.TeacherAuditOperate = "";
                viewEquipmentOpenPractice.CollegeAuditOperate = "";
                viewEquipmentOpenPractice.ManageAuditOperate = "";
                viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                viewEquipmentOpenPractice.DeleteOperate = "";
                viewEquipmentOpenPractice.ClosedOperate = "";
                viewEquipmentOpenPractice.ExportWord = "";
                viewEquipmentOpenPractice.ExportClosedWord = "";
                return;
            }
            //判断权限
            if (!equipmentOpenPracticePrivilige.IsEnableView) viewEquipmentOpenPractice.ViewOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableEdit) viewEquipmentOpenPractice.ModifyOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableExportWord) viewEquipmentOpenPractice.ExportWord = "";
            if (!equipmentOpenPracticePrivilige.IsEnableExportClosedWord) viewEquipmentOpenPractice.ExportClosedWord = "";
            if (!equipmentOpenPracticePrivilige.IsEnableDelete) viewEquipmentOpenPractice.DeleteOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableTeacherAudit) viewEquipmentOpenPractice.TeacherAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableCollegeAudit) viewEquipmentOpenPractice.CollegeAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableManageAudit) viewEquipmentOpenPractice.ManageAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableManagePreAudit) viewEquipmentOpenPractice.ManagePreAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableAllocatingFund) viewEquipmentOpenPractice.AllocatingFundOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableClosed) viewEquipmentOpenPractice.ClosedOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableViewClosed) viewEquipmentOpenPractice.ClosedViewOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableClosedTeacherAudit) viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableClosedCollegeAudit) viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
            if (!equipmentOpenPracticePrivilige.IsEnableClosedManageAudit) viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
            
            bool isAdminPower = true;
            if (!equipmentOpenPracticePrivilige.IsEnableManageAudit && !userPrivilige.IsEnableEdit)
                isAdminPower = false;
            //非草稿状态不能编辑删除
            if (viewEquipmentOpenPractice.StatusEnum != EquipmentOpenPracticeStatus.Draft
                & viewEquipmentOpenPractice.StatusEnum != EquipmentOpenPracticeStatus.TeacherAuditReject
                & viewEquipmentOpenPractice.StatusEnum != EquipmentOpenPracticeStatus.CollegeAuditReject
                & viewEquipmentOpenPractice.StatusEnum != EquipmentOpenPracticeStatus.ManageAuditReject)
            {
                viewEquipmentOpenPractice.ModifyOperate = "";
                viewEquipmentOpenPractice.DeleteOperate = "";
            }
            //非申请人不能编辑
            if (viewEquipmentOpenPractice.ApplyUserId != userId.Value) viewEquipmentOpenPractice.ModifyOperate = "";
            ////用户没有编辑权限不能进行提交操作？
            //if (!userPrivilige.IsEnableEdit)
            //{
            //    viewEquipmentOpenPractice.CollegeAuditOperate = "";
            //    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
            //}
            var isTeacher = __equipmentOpenPracticeTeacherBLL.CountModelListByExpression(string.Format("EquipmentOpenPracticeId=\"{0}\"*UserId=\"{1}\"*IsDelete=false", viewEquipmentOpenPractice.Id, userId.Value)) > 0;
           //只有申请里的导师才能进行导师审核及结题
            if (!isTeacher)
            {
                viewEquipmentOpenPractice.TeacherAuditOperate = "";
                viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
            }
            //各流程状态下的操作判断
            switch (viewEquipmentOpenPractice.StatusEnum)
            {
                case EquipmentOpenPracticeStatus.Draft:
                    //导师、管理员或申请人可以在草稿状态下删除申请
                    if (!isTeacher && !isAdminPower && viewEquipmentOpenPractice.ApplyUserId != userId.Value) viewEquipmentOpenPractice.DeleteOperate = "";
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.WaitingAudit:
                    viewEquipmentOpenPractice.DeleteOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.TeacherAuditPass:
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    break;
                case EquipmentOpenPracticeStatus.TeacherAuditReject:
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.CollegeAuditPass:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.CollegeAuditReject:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ManagePreAuditPass:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ManagePreAuditReject:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.AllocatingFundSubmit:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ManageAuditPass:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    if (viewEquipmentOpenPractice.ApplyUserId != userId.Value) viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ManageAuditReject:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedViewOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ClosedDraft:
                case EquipmentOpenPracticeStatus.ClosedTeacherAuditReject:
                case EquipmentOpenPracticeStatus.ClosedCollegeAuditReject:
                case EquipmentOpenPracticeStatus.ClosedManageAuditReject:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    if (viewEquipmentOpenPractice.ApplyUserId != userId.Value) viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.Closed:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    if (viewEquipmentOpenPractice.ApplyUserId != userId.Value) viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                case EquipmentOpenPracticeStatus.ClosedTeacherAuditPass:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                //case EquipmentOpenPracticeStatus.ClosedTeacherAuditReject:
                //    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                //    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                //    viewEquipmentOpenPractice.ManageAuditOperate = "";
                //    viewEquipmentOpenPractice.ClosedOperate = "";
                //    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                //    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                //    viewEquipmentOpenPractice.ExportClosedWord = "";
                //    viewEquipmentOpenPractice.ExportWord = "";
                //    break;
                case EquipmentOpenPracticeStatus.ClosedCollegeAuditPass:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
                //case EquipmentOpenPracticeStatus.ClosedCollegeAuditReject:
                //    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                //    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                //    viewEquipmentOpenPractice.ManageAuditOperate = "";
                //    viewEquipmentOpenPractice.ClosedOperate = "";
                //    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                //    viewEquipmentOpenPractice.ClosedManageAuditOperate = "";
                //    viewEquipmentOpenPractice.ExportClosedWord = "";
                //    viewEquipmentOpenPractice.ExportWord = "";
                //    break;
                case EquipmentOpenPracticeStatus.ClosedManageAuditPass:
                //case EquipmentOpenPracticeStatus.ClosedManageAuditReject:
                    viewEquipmentOpenPractice.TeacherAuditOperate = "";
                    viewEquipmentOpenPractice.CollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ManagePreAuditOperate = "";
                    viewEquipmentOpenPractice.AllocatingFundOperate = "";
                    viewEquipmentOpenPractice.ManageAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedOperate = "";
                    viewEquipmentOpenPractice.ClosedTeacherAuditOperate = "";
                    viewEquipmentOpenPractice.ClosedCollegeAuditOperate = "";
                    viewEquipmentOpenPractice.ExportClosedWord = "";
                    viewEquipmentOpenPractice.ExportWord = "";
                    break;
            }
        }
        public IList<ViewEquipmentOpenPractice> GetViewEquipmentOpenPracticeListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentOpenPractice(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenPracticeList = GetEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenPracticeList, isSupressLazyLoad);
            return viewEquipmentOpenPracticeList;
        }
        public IList<ViewEquipmentOpenPractice> GetViewEquipmentOpenPracticeListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentOpenPractice(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenPracticeList = GetEntitiesByExpression(dataGridSettings.QueryExpression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenPracticeList, isSupressLazyLoad);
            return viewEquipmentOpenPracticeList;
        }
        public IList<ViewEquipmentOpenPractice> GetViewEquipmentOpenPracticeListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipmentOpenPractice(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenPracticeList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenPracticeList, isSupressLazyLoad);
            return viewEquipmentOpenPracticeList;
        }
        public GridData<ViewEquipmentOpenPractice> GetGridViewEquipmentOpenPracticeListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageEquipmentOpenPractice(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenPracticeList = GetGridEntitiesByExpression(dataGridSettings.QueryExpression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenPracticeList == null ? null : viewEquipmentOpenPracticeList.Data, isSupressLazyLoad);
            return viewEquipmentOpenPracticeList;
        }
        public GridData<ViewEquipmentOpenPractice> GetGridViewEquipmentOpenPracticeListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageEquipmentOpenPractice(userId, ref dataGridSettings)) return null;
            var viewEquipmentOpenPracticeList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentOpenPracticeList == null ? null : viewEquipmentOpenPracticeList.Data, isSupressLazyLoad);
            return viewEquipmentOpenPracticeList;
        }
        private bool IsManageEquipmentOpenPractice(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            var equipmentOpenPracticePrivilige = PriviligeFactory.GetEquipmentOpenPracticePrivilige(userId.Value);
            if (!user.IsSuperAdmin && (equipmentOpenPracticePrivilige == null || !equipmentOpenPracticePrivilige.IsEnableManageAudit))
            {
                var queryExpression = "";
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.User), null, null);
                if (queryOrgId == "Id=null" || queryOrgId == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
