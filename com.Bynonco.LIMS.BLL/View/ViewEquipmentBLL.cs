using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.BLL.Business.Customer.Factory;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewEquipmentBLL : BLLBase<ViewEquipment>, IViewEquipmentBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IEquipmentTranningBLL __equipmentTrainningBLL;
        private IUserEquipmentBLL __userEquipmentBLL;
        private IUserExaminationBLL __userExaminationBLL;
        private IViewEquipmentCategoryBindBLL __viewEquipmentCategoryBindBLL;
        private IViewLabOrganizationAdminBLL __viewLabOrganizationAdminBLL;
        private IViewEquipmentRelationBLL __viewEquipmentRelationBLL;
        private IEquipmentGroupAdminBLL __equipmentGroupAdminBLL;
        private IEquipmentGroupMemberBLL __equipmentGroupMemberBLL;

        public ViewEquipmentBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __equipmentTrainningBLL = BLLFactory.CreateEquipmentTranningBLL();
            __userEquipmentBLL = BLLFactory.CreateUserEquipmentBLL();
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
            __viewEquipmentCategoryBindBLL = BLLFactory.CreateViewEquipmentCategoryBindBLL();
            __viewLabOrganizationAdminBLL = BLLFactory.CreateViewLabOrganizationAdminBLL();
            __viewEquipmentRelationBLL = BLLFactory.CreateViewEquipmentRelationBLL();
            __equipmentGroupAdminBLL = BLLFactory.CreateEquipmentGroupAdminBLL();
            __equipmentGroupMemberBLL = BLLFactory.CreateEquipmentGroupMemberBLL();
        }
        public void ExcuteOperateDecorate(Guid? userId, IList<ViewEquipment> viewEquipmentList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentList == null || viewEquipmentList.Count == 0) return;
            foreach (var viewEquipment in viewEquipmentList)
            {
                viewEquipment.IsSupressLazyLoad = false;
                viewEquipment.InitOperate();
                OperateDecorate(userId, viewEquipment);
                viewEquipment.BuildOperate();
                viewEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        public void OperateDecorate(Guid? userId, ViewEquipment viewEquipment)
        {
            if (viewEquipment == null) throw new ArgumentException("设备信息为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipment.Id) : null;
           
            if (!viewEquipment.IsAcceptSampleTest.HasValue || !viewEquipment.IsAcceptSampleTest.Value || viewEquipment.StatusEnum != Model.Enum.EquipmentStatus.Normal)
            {
                viewEquipment.SampleApplyOperate = "";
                viewEquipment.SampleApplyWithoutLoginOperate = "";
            }
            if (viewEquipment.StatusEnum != Model.Enum.EquipmentStatus.Normal || viewEquipment.UseTypeEnum == Model.Enum.EquipmentUseType.Unavailable)
            {
                viewEquipment.AppointmentNoneOperate = "";
                viewEquipment.AppointmentOperate = "";
                viewEquipment.AppointmentWithoutLoginOperate = "";

            }
            else if(viewEquipment.StatusEnum != Model.Enum.EquipmentStatus.Normal)
            {
                viewEquipment.SampleApplyOperate = "";
                viewEquipment.SampleApplyWithoutLoginOperate = "";
            }
            if (viewEquipment.UseTypeEnum == EquipmentUseType.None)
            {
                viewEquipment.AppointmentOperate = "";
                viewEquipment.AppointmentWithoutLoginOperate = "";
                //viewEquipment.SampleApplyOperate = "";
                //viewEquipment.SampleApplyWithoutLoginOperate = "";
            }
            else viewEquipment.AppointmentNoneOperate = "";            
            if (equipmentPrivilige == null )
            {
                viewEquipment.MarkingOperate = "";
                viewEquipment.AppointmentOperate = "";
                viewEquipment.SampleApplyOperate = "";
                viewEquipment.TranningApplyOperate = "";
                viewEquipment.ReportBrokenOperate = "";
                viewEquipment.FocusOperate = "";
                viewEquipment.CancelFocusOperate = "";
                viewEquipment.ModifyOperate = "";
                viewEquipment.ExaminationManageOperate = "";
                viewEquipment.UserExaminationOperate = "";
                viewEquipment.UserReadingMaterialOperate = "";
                viewEquipment.CancelApplyOperate = "";
                viewEquipment.ApplyOperate="";
                viewEquipment.CameraPlayOperate = "";
                viewEquipment.CameraPlayRecordOperate = "";
                viewEquipment.ExportDocOperate = "";
                viewEquipment.OpenOperate = "";
                viewEquipment.CloseOperate = "";
                return;
            }
            if (!equipmentPrivilige.IsEnableExamination) viewEquipment.UserExaminationOperate = "";
            if (!equipmentPrivilige.IsEnableReadingMaterial) viewEquipment.UserReadingMaterialOperate = "";
            if (!equipmentPrivilige.IsEnableApplyTrainning) viewEquipment.TranningApplyOperate = "";
           
            if (!equipmentPrivilige.IsEnableEdit) viewEquipment.ModifyOperate = "";
            if (!equipmentPrivilige.IsEnableAddMarking) viewEquipment.MarkingOperate = "";
            if (!equipmentPrivilige.IsEnableAddBrokenReport) viewEquipment.ReportBrokenOperate = "";
            //var userEquipmentPrivilige = userId.HasValue ? PriviligeFactory.GetUserEquipmentPrivilige(userId.Value) : null;
            //if (userEquipmentPrivilige != null)
            //{
            //    if (!userEquipmentPrivilige.IsEnableCancelFocus) viewEquipment.CancelFocusOperate = "";
            //    if (!userEquipmentPrivilige.IsEnableFocusEquipment) viewEquipment.FocusOperate = "";
            //}
            if ((!viewEquipment.IsNeedTranningBeforeAppointment.HasValue ||  !viewEquipment.IsNeedTranningBeforeAppointment.Value) && (!viewEquipment.IsNeedTranningBeforeUse.HasValue || !viewEquipment.IsNeedTranningBeforeUse.Value))
                viewEquipment.TranningApplyOperate = "";
            if ((viewEquipment.IsNeedTranningBeforeAppointment.HasValue && viewEquipment.IsNeedTranningBeforeAppointment.Value) || (viewEquipment.IsNeedTranningBeforeUse.HasValue && viewEquipment.IsNeedTranningBeforeUse.Value))
            {
                var equipmentTrainningList = __equipmentTrainningBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*CreatorId=\"{1}\"", viewEquipment.Id.ToString(), userId.ToString()));
                if (equipmentTrainningList != null && equipmentTrainningList.Count > 0) viewEquipment.TranningApplyOperate = "";
            }
            var userEquipment= __userEquipmentBLL.GetFirstOrDefaultEntityByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", viewEquipment.Id.ToString(), userId.ToString()));
            if (userEquipment != null )
            {
                if (userEquipment.StatusEnum != Model.Enum.UserEquipmentStatus.Focus)
                {
                    viewEquipment.FocusOperate = "";
                    viewEquipment.CancelFocusOperate = "";
                    viewEquipment.ApplyOperate = "";
                }
                else
                {
                    viewEquipment.FocusOperate = "";
                    viewEquipment.CancelApplyOperate = "";
                }
            }
            else
            {
                viewEquipment.CancelFocusOperate = "";
                viewEquipment.CancelApplyOperate = "";
            }
            if (!equipmentPrivilige.IsEnableApplyEquipment)
            {
                viewEquipment.ApplyOperate = "";
            }
            if (!equipmentPrivilige.IsEanbleCancelApplyEquipment)
            {
                viewEquipment.CancelApplyOperate = "";
            }
            if (!equipmentPrivilige.IsEnableExaminationManage) viewEquipment.ExaminationManageOperate = "";
            if (!viewEquipment.IsNeedExaminationBeforeAppointment.HasValue || !viewEquipment.IsNeedExaminationBeforeAppointment.Value)
            {
                viewEquipment.UserExaminationOperate = "";
            }
            else
            {
                var maxExaminationTime = viewEquipment.MaxExaminationTime.HasValue ? viewEquipment.MaxExaminationTime.Value : 3;
                var userExaminationList = __userExaminationBLL.GetEntitiesByExpression(string.Format("BusinessId=\"{0}\"*UserId=\"{1}\"*IsStop=false*IsDelete=false", viewEquipment.Id.ToString(), userId.ToString()));
                if (userExaminationList != null && userExaminationList.Count() > 0 && (userExaminationList.Count() >= maxExaminationTime || userExaminationList.Where(p => p.IsPass == true).Count() > 0))
                    viewEquipment.UserExaminationOperate = "";
            }
            if (!equipmentPrivilige.IsEnableCameraPlay) viewEquipment.CameraPlayOperate = "";
            if (!equipmentPrivilige.IsEnableCameraVideoRecord) viewEquipment.CameraPlayRecordOperate = "";
            if (!equipmentPrivilige.IsEnableExportDoc) viewEquipment.ExportDocOperate = "";
            if (viewEquipment.IsOpen || !equipmentPrivilige.IsEnableOpen) viewEquipment.OpenOperate = "";
            if (!viewEquipment.IsOpen || !equipmentPrivilige.IsEnableClose) viewEquipment.CloseOperate = "";
            if (!viewEquipment.IsOpen)
            {
                viewEquipment.AppointmentOperate = "";
                viewEquipment.SampleApplyOperate = "";
            }
            if (viewEquipment.AppointmentNoneOperate != "" && viewEquipment.ApplyOperate != "")//随到随用不需要申请
                viewEquipment.ApplyOperate = "";
        }
        public IList<ViewEquipment> GetViewEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewEquipmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentList, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public IList<ViewEquipment> GetViewEquipmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewEquipmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentList, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public IList<ViewEquipment> GetViewEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewEquipmentList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentList, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public GridData<ViewEquipment> GetGridViewEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewEquipmentList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentList == null ? null : viewEquipmentList.Data, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public GridData<ViewEquipment> GetGridViewEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewEquipmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewEquipmentList == null ? null : viewEquipmentList.Data, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public IList<ViewEquipment> GetViewEquipmentListByEquipmentFilter(Guid? userId, DataGridSettings dataGridSettings, EquipmentFilter equipmentfilter, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            var isFuzzyQuery = dictCodeBLL.GetDictCodeBoolValueByCode("FuzzyQuery", "IsFuzzyQuery"); //是否模糊查询
            if (isFuzzyQuery.HasValue && isFuzzyQuery.Value)
                GetEquipmentFuzzyFilterQueryExpression(equipmentfilter, ref dataGridSettings);
            else
                GetEquipmentFilterQueryExpression(equipmentfilter, ref dataGridSettings);
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            return GetViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public GridData<ViewEquipment> GetGridViewEquipmentListByEquipmentFilter(Guid? userId, DataGridSettings dataGridSettings, EquipmentFilter equipmentfilter, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            IDictCodeBLL dictCodeBLL = BLLFactory.CreateDictCodeBLL();
            var isFuzzyQuery = dictCodeBLL.GetDictCodeBoolValueByCode("FuzzyQuery", "IsFuzzyQuery"); //是否模糊查询
            if (isFuzzyQuery.HasValue && isFuzzyQuery.Value)
                GetEquipmentFuzzyFilterQueryExpression(equipmentfilter, ref dataGridSettings);
            else
                GetEquipmentFilterQueryExpression(equipmentfilter, ref dataGridSettings);
            if (isManageList && !IsManageEquipment(userId, ref dataGridSettings)) return null;
            return GetGridViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public void GetEquipmentFuzzyFilterQueryExpression(EquipmentFilter equipmentfilter, ref DataGridSettings dataGridSettings)
        {
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentName)) dataGridSettings.AppendAndQueryExpression(string.Format("(Name∽\"{0}\"+Label∽\"{0}\"+InputStr∽\"{0}\"+Model∽\"{0}\"∽\"{0}\"+RoomName∽\"{0}\"+CategoryXPath∽\"{0}\")", equipmentfilter.EquipmentName));
            //if (!string.IsNullOrEmpty(equipmentfilter.ScopeOfApplication)) dataGridSettings.AppendAndQueryExpression(string.Format("ScopeOfApplicationNoHTML∽\"{0}\"", equipmentfilter.ScopeOfApplication));
            if (!string.IsNullOrEmpty(equipmentfilter.Qualification)) dataGridSettings.AppendAndQueryExpression(string.Format("QualificationNoHTML∽\"{0}\"", equipmentfilter.Qualification));
            if (!string.IsNullOrEmpty(equipmentfilter.OrgXPath))
            {
                var viewEquipmentRelationList = __viewEquipmentRelationBLL.GetEntitiesByExpression(string.Format("RelationOrgXPath→\"{0}\"*Status=\"{1}\"", equipmentfilter.OrgXPath, (int)EquipmentRelationStatus.AuditPass));
                var equipmentRelationQueryExpression = "";
                if (viewEquipmentRelationList != null && viewEquipmentRelationList.Count() > 0)
                {
                    equipmentRelationQueryExpression = "+" + viewEquipmentRelationList.Select(p => p.EquipmentId).ToFormatStr();
                }
                dataGridSettings.AppendAndQueryExpression(string.Format("OrgXPath→\"{0}\"{1}", equipmentfilter.OrgXPath, equipmentRelationQueryExpression));
            }
            //if (!string.IsNullOrEmpty(equipmentfilter.EquipmentCategoryXPath))
            //{
            //    var viewEquipmentCategoryBindList = __viewEquipmentCategoryBindBLL.GetEntitiesByExpression(string.Format("CategoryXPath→\"{0}\"*CategoryIsStop=false*CategoryIsDelete=false", equipmentfilter.EquipmentCategoryXPath));
            //    if (viewEquipmentCategoryBindList != null && viewEquipmentCategoryBindList.Count() > 0)
            //        dataGridSettings.AppendAndQueryExpression(viewEquipmentCategoryBindList.Select(p => p.EquipmentId).Distinct().ToFormatStr("Id"));
            //    else
            //        dataGridSettings.AppendAndQueryExpression("Id=null");
            //}            
            //if (!string.IsNullOrEmpty(equipmentfilter.RoomXPath)) dataGridSettings.AppendAndQueryExpression(string.Format("RoomXPath→\"{0}\"", equipmentfilter.RoomXPath));
            if (equipmentfilter.ControllerType.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("ControllerType={0}", equipmentfilter.ControllerType.Value.ToString()));
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentLabel)) dataGridSettings.AppendAndQueryExpression(string.Format("Label∽\"{0}\"", equipmentfilter.EquipmentLabel));
            if (equipmentfilter.BuyDateBegin.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("BuyDate>\"{0}\"", equipmentfilter.BuyDateBegin.Value.ToString("yyyy-MM-dd")));
            if (equipmentfilter.BuyDateEnd.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("BuyDate<\"{0}\"", equipmentfilter.BuyDateEnd.Value.AddDays(1).ToString("yyyy-MM-dd")));
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentIP)) dataGridSettings.AppendAndQueryExpression(string.Format("IP∽\"{0}\"", equipmentfilter.EquipmentIP));
            if (equipmentfilter.DirectorId.HasValue)
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", equipmentfilter.DirectorId.Value));
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    dataGridSettings.AppendAndQueryExpression(userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr());
                else
                    dataGridSettings.AppendAndQueryExpression("Id=null");
            }
            dataGridSettings.AppendAndQueryExpression(string.Format("IsOpen={0}", equipmentfilter.IsOpen.ToString().ToLower()));
            if (equipmentfilter.IsDutyFree)
                dataGridSettings.AppendAndQueryExpression(string.Format("IsDutyFree={0}", equipmentfilter.IsDutyFree.ToString().ToLower()));
        }
        public void GetEquipmentFilterQueryExpression(EquipmentFilter equipmentfilter, ref DataGridSettings dataGridSettings)
        {
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentName)) dataGridSettings.AppendAndQueryExpression(string.Format("(Name∽\"{0}\"+Label∽\"{0}\"+InputStr∽\"{0}\"+Model∽\"{0}\")", equipmentfilter.EquipmentName));
            if (!string.IsNullOrEmpty(equipmentfilter.ScopeOfApplication)) dataGridSettings.AppendAndQueryExpression(string.Format("ScopeOfApplicationNoHTML∽\"{0}\"", equipmentfilter.ScopeOfApplication));
            if (!string.IsNullOrEmpty(equipmentfilter.Qualification)) dataGridSettings.AppendAndQueryExpression(string.Format("QualificationNoHTML∽\"{0}\"", equipmentfilter.Qualification));
            if (!string.IsNullOrEmpty(equipmentfilter.OrgXPath))
            {
                var viewEquipmentRelationList = __viewEquipmentRelationBLL.GetEntitiesByExpression(string.Format("RelationOrgXPath→\"{0}\"*Status=\"{1}\"", equipmentfilter.OrgXPath, (int)EquipmentRelationStatus.AuditPass));
                var equipmentRelationQueryExpression = "";
                if (viewEquipmentRelationList != null && viewEquipmentRelationList.Count() > 0)
                {
                    equipmentRelationQueryExpression = "+" + viewEquipmentRelationList.Select(p => p.EquipmentId).ToFormatStr();
                }
                dataGridSettings.AppendAndQueryExpression(string.Format("OrgXPath→\"{0}\"{1}", equipmentfilter.OrgXPath, equipmentRelationQueryExpression));
            }
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentCategoryXPath))
            {
                var viewEquipmentCategoryBindList = __viewEquipmentCategoryBindBLL.GetEntitiesByExpression(string.Format("CategoryXPath→\"{0}\"*CategoryIsStop=false*CategoryIsDelete=false", equipmentfilter.EquipmentCategoryXPath));
                if (viewEquipmentCategoryBindList != null && viewEquipmentCategoryBindList.Count() > 0)
                    dataGridSettings.AppendAndQueryExpression(viewEquipmentCategoryBindList.Select(p => p.EquipmentId).Distinct().ToFormatStr("Id"));
                else
                    dataGridSettings.AppendAndQueryExpression("Id=null");
            }
            if (!string.IsNullOrEmpty(equipmentfilter.RoomXPath)) dataGridSettings.AppendAndQueryExpression(string.Format("RoomXPath→\"{0}\"", equipmentfilter.RoomXPath));
            if (equipmentfilter.ControllerType.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("ControllerType={0}", equipmentfilter.ControllerType.Value.ToString()));
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentLabel)) dataGridSettings.AppendAndQueryExpression(string.Format("Label∽\"{0}\"", equipmentfilter.EquipmentLabel));
            if (equipmentfilter.BuyDateBegin.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("BuyDate>\"{0}\"", equipmentfilter.BuyDateBegin.Value.ToString("yyyy-MM-dd")));
            if (equipmentfilter.BuyDateEnd.HasValue) dataGridSettings.AppendAndQueryExpression(string.Format("BuyDate<\"{0}\"", equipmentfilter.BuyDateEnd.Value.AddDays(1).ToString("yyyy-MM-dd")));
            if (!string.IsNullOrEmpty(equipmentfilter.EquipmentIP)) dataGridSettings.AppendAndQueryExpression(string.Format("IP∽\"{0}\"", equipmentfilter.EquipmentIP));
            if (equipmentfilter.DirectorId.HasValue)
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", equipmentfilter.DirectorId.Value));
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    dataGridSettings.AppendAndQueryExpression(userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr());
                else
                    dataGridSettings.AppendAndQueryExpression("Id=null");
            }
            if (!string.IsNullOrWhiteSpace(equipmentfilter.EquipmentAdminName))
            {
                var userList = __userBLL.GetEntitiesByExpression(string.Format("Label∽\"{0}\"+UserName∽\"{0}\"*IsDel=false", equipmentfilter.EquipmentAdminName));
                if (userList != null && userList.Count() > 0)
                {
                    var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(userList.Select(p=>p.Id).ToFormatStr("UserId"));
                    if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                        dataGridSettings.AppendAndQueryExpression(userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr());
                    else
                        dataGridSettings.AppendAndQueryExpression("Id=null");
                }
                else
                    dataGridSettings.AppendAndQueryExpression("Id=null");
            }
            dataGridSettings.AppendAndQueryExpression(string.Format("IsOpen={0}", equipmentfilter.IsOpen.ToString().ToLower()));
            if(equipmentfilter.IsDutyFree)
                dataGridSettings.AppendAndQueryExpression(string.Format("IsDutyFree={0}", equipmentfilter.IsDutyFree.ToString().ToLower()));
        }
        private bool IsManageEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            var queryExpression = "";
            if (!user.IsSuperAdmin || CustomerFactory.GetCustomer().GetIsSuperAdminOnlyShowAdminEqList())
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    queryExpression = userWorkScopeList.Select(p => p.EquipmentId).ToFormatStr();
            }
            if (!user.IsSuperAdmin)
            {
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "OrgId"), null);
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
            }
            if (!string.IsNullOrWhiteSpace(queryExpression)) dataGridSettings.AppendAndQueryExpression(queryExpression);
            return true;
        }
        public GridData<ViewEquipment> GetMyViewEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            var userEquipmentList = __userEquipmentBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"*Status=1", userId.Value));
            if (userEquipmentList == null || userEquipmentList.Count == 0) return null;
            dataGridSettings.AppendAndQueryExpression(userEquipmentList.Select(p => p.EquipmentId).ToFormatStr());
            return GetGridViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public GridData<ViewEquipment> GetUserManageViewEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!IsManageEquipment(userId, ref dataGridSettings)) return null;
            return GetGridViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public GridData<ViewEquipment> GetTutorViewEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            if (!userId.HasValue) return null;
            User user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return null;
            Guid tutorId = Guid.Empty;
            if (user.UserType.UserIdentityEnum == Model.Enum.UserIdentity.Tutor || user.UserType.UserIdentityEnum == Model.Enum.UserIdentity.OutTutor) tutorId = user.Id;
            else if (user.Tutor != null) tutorId = user.Tutor.Id;
            var userEquipmentList = __userEquipmentBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"*Status=1", tutorId.ToString()));
            if (userEquipmentList == null || userEquipmentList.Count == 0) return null;
            dataGridSettings.AppendAndQueryExpression(userEquipmentList.Select(p => p.EquipmentId).ToFormatStr());
            return GetGridViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public IList<ViewEquipment> GetAuthorizationViewEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageAuthorizationEquipment(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        public GridData<ViewEquipment> GetGridAuthorizationViewEquipmentList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageAuthorizationEquipment(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        private bool IsManageAuthorizationEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var viewLabOrganizationAdminList = __viewLabOrganizationAdminBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                if (viewLabOrganizationAdminList == null || viewLabOrganizationAdminList.Count() == 0) return false;
                string querySql = "";
                foreach (var item in viewLabOrganizationAdminList) querySql += (querySql == "" ? "" : "+") + string.Format("OrgXPath→\"{0}\"+RoomXPath→\"{0}\"", item.OrgXPath);
                querySql = "(" + querySql + ")";
                dataGridSettings.AppendAndQueryExpression(querySql);
            }
            return true;
        }

        public void ExcuteAuthorizationOperateDecorate(Guid? userId, IList<ViewEquipment> viewEquipmentList, bool isSupressLazyLoad = false)
        {
            if (viewEquipmentList == null || viewEquipmentList.Count == 0) return;
            foreach (var viewEquipment in viewEquipmentList)
            {
                viewEquipment.IsSupressLazyLoad = false;
                viewEquipment.InitAuthorizationOperate();
                AuthorizationOperateDecorate(userId, viewEquipment);
                viewEquipment.BuildAuthorizationOperate();
                viewEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        public void AuthorizationOperateDecorate(Guid? userId, ViewEquipment viewEquipment)
        {
            if (viewEquipment == null) throw new ArgumentException("设备信息为空");
            var equipmentPrivilige = userId.HasValue ? PriviligeFactory.GetEquipmentPrivilige(userId.Value, viewEquipment.Id) : null;

            if (equipmentPrivilige == null)
            {
                viewEquipment.ModifyOperate = "";
                viewEquipment.DeleteOperate = "";
                return;
            }
            if (!equipmentPrivilige.IsEnableAuthorizationEquipmentEdit) viewEquipment.ModifyOperate = "";
            if (!equipmentPrivilige.IsEnableAuthorizationEquipmentDelete) viewEquipment.DeleteOperate = "";
            if (!equipmentPrivilige.IsEnableAuthorizationEquipmentCameraPlay) viewEquipment.CameraPlayOperate = "";
            if (!equipmentPrivilige.IsEnableAuthorizationEquipmentCameraVideoRecord) viewEquipment.CameraPlayRecordOperate = "";
        }
        public GridData<ViewEquipment> GetGridAuthorizationViewEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!userId.HasValue || dataGridSettings.QueryExpression.IndexOf("OrgXPath→") == -1) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            var viewEquipmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteAuthorizationOperateDecorate(userId, viewEquipmentList == null ? null : viewEquipmentList.Data, isSupressLazyLoad);
            return viewEquipmentList;
        }
        public IList<ViewEquipment> GetEquipmentGroupViewEquipmentList(Guid? userId, Guid? equipmentGroupId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageEquipmentGroup(userId, equipmentGroupId, ref dataGridSettings)) return null;
            return GetViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        public GridData<ViewEquipment> GetGridEquipmentGroupViewEquipmentList(Guid? userId, Guid? equipmentGroupId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageEquipmentGroup(userId,equipmentGroupId, ref dataGridSettings)) return null;
            return GetGridViewEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        private bool IsManageEquipmentGroup(Guid? userId, Guid? equipmentGroupId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", user.Id));
                var queryExpression = "";
                IEnumerable<Guid> equipmentIdList = null;
                if (userWorkScopeList != null && userWorkScopeList.Count() > 0)
                    equipmentIdList = userWorkScopeList.Select(p => p.EquipmentId).Distinct();
                var equipmentGroupAdminList = __equipmentGroupAdminBLL.GetEntitiesByExpression(equipmentGroupId.HasValue ? string.Format("AdminId=\"{0}\"*EquipmentGroupId=\"{1}\"",user.Id, equipmentGroupId.Value) : string.Format("AdminId=\"{0}\"", user.Id));
                if (equipmentGroupAdminList != null && equipmentGroupAdminList.Count() > 0)
                {
                    var equipmentGroupMemberList = __equipmentGroupMemberBLL.GetEntitiesByExpression(equipmentGroupAdminList.Select(p=>p.EquipmentGroupId).Distinct().ToFormatStr("EquipmentGroupId"));
                    if (equipmentGroupMemberList != null && equipmentGroupMemberList.Count() > 0)
                    {
                        var memberEquipmentIdList = equipmentGroupMemberList.Select(p => p.EquipmentId).Distinct();
                        if (equipmentIdList == null) equipmentIdList = memberEquipmentIdList;
                        else equipmentIdList.Union(memberEquipmentIdList);
                    }
                }
                if(equipmentIdList != null && equipmentIdList.Count() > 0) queryExpression = equipmentIdList.ToFormatStr();
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.Equipment, "", "", "OrgId"), null);
                if ((queryOrgId == "Id=null" || queryOrgId == "") && queryExpression == "") queryExpression = "Id=null";
                else if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression += (queryExpression == "" ? "" : "+") + queryOrgId;
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }

        public IList<EquipmentStatistics> GetEquipmentStatistics(DataGridSettings dataGridSettings, Guid? userId, DateTime? startAt, DateTime? endAt, out int returnCount)
        {
            returnCount = 0;
            IList<EquipmentStatistics> lstEquipmentStatistics = new List<EquipmentStatistics>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            mapping["Id"] = "EquipmentId";
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            dataParams.Add(userId.HasValue ? DataParameterFactory.CreateDataParameter("userId", userId.Value) : DataParameterFactory.CreateDataParameter("userId", DBNull.Value));
            if (!string.IsNullOrWhiteSpace(dataGridSettings.SortColumn))
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", dataGridSettings.SortColumn));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr() == "desc" ? 1 : 0));
            }
            else
            {
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderName", DBNull.Value));
                dataParams.Add(DataParameterFactory.CreateDataParameter("orderType", DBNull.Value));
            }
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize));
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value.ToString("yyyy-MM-dd")) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value.ToString("yyyy-MM-dd 23:59:59")) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetEquipmentStatistics", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var equipmentId = new Guid(result[0].ToString());
                    var equipmentName = result[1] != null ? result[1].ToString() : "";
                    var equipmentOrgName = result[2] != null ? result[2].ToString() : "";
                    var totalAppointmentCount = Convert.ToInt32(result[3] == DBNull.Value ? 0 : result[3]);
                    var totalAppointmentHours = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                    var totalUsedCount = Convert.ToInt32(result[5] == DBNull.Value ? 0 : result[5]);
                    var totalUsedHours = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                    var totalCostDeductCount = Convert.ToInt32(result[7] == DBNull.Value ? 0 : result[7]);
                    var totalCurrency = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                    var totalCostUsedHour = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                    var totalCalcDuration = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                    var totalSampleApplyQuatity = Convert.ToInt32(result[11] == DBNull.Value ? 0 : result[11]);
                    var totalSampleQuatity = Convert.ToInt32(result[12] == DBNull.Value ? 0 : result[12]);
                    var totalSampleRealQuatity = Convert.ToInt32(result[13] == DBNull.Value ? 0 : result[13]);
                    var totalSampleRealCurrencyFee = Convert.ToInt32(result[14] == DBNull.Value ? 0 : result[14]);
                    var totalSampleVirtualCurrencyFee = Convert.ToInt32(result[15] == DBNull.Value ? 0 : result[15]);

                    EquipmentStatistics item = new EquipmentStatistics()
                    {
                        EquipmentId = equipmentId,
                        EquipmentName = equipmentName,
                        EquipmentOrgName = equipmentOrgName,
                        TotalAppointmentCount = totalAppointmentCount,
                        TotalAppointmentHours = totalAppointmentHours,
                        TotalUsedCount = totalUsedCount,
                        TotalUsedHours = totalUsedHours,
                        TotalCostDeductCount = totalCostDeductCount,
                        TotalCurrency = totalCurrency,
                        TotalCostUsedHour = totalCostUsedHour,
                        TotalCalcDuration = totalCalcDuration,
                        TotalSampleApplyQuatity = totalSampleApplyQuatity,
                        TotalSampleQuatity = totalSampleQuatity,
                        TotalSampleRealQuatity = totalSampleRealQuatity,
                        TotalSampleRealCurrencyFee = totalSampleRealCurrencyFee,
                        TotalSampleVirtualCurrencyFee = totalSampleVirtualCurrencyFee
                    };
                    lstEquipmentStatistics.Add(item);
                    returnCount = Convert.ToInt32(returnValueDataParameter.Value);
                }
            }
            return lstEquipmentStatistics;
        }
        public GridData<EquipmentStatistics> GetGridEquipmentStatistics(DataGridSettings dataGridSettings, Guid? userId, DateTime? startAt, DateTime? endAt)
        {
            int count = 0;
            var equipmentStatisticsList = GetEquipmentStatistics(dataGridSettings, userId, startAt, endAt, out count);
            return new GridData<EquipmentStatistics>()
            {
                Data = equipmentStatisticsList,
                Count = count
            };
        }
    }
}
