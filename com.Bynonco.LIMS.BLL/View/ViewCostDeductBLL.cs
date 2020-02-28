using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using System.Data;
using com.Bynonco.LIMS.DAL;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewCostDeductBLL : BLLBase<ViewCostDeduct>, IViewCostDeductBLL
    {
        private ICostDeductBLL __costDeductBLL;
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IUsedConfirmBLL __usedConfirmBLL;
        private IDepositRecordBLL __depositRecordBLL;
        private IUserAccountBLL __userAccountBLL;
        private IUserTypeBLL __userTypeBLL;
        private ITagBLL __tagBLL;
        private IViewMaterialDepositRecordBLL __viewMaterialDepositRecordBLL;
        private Dictionary<string, string> __mapping = new Dictionary<string, string>();
        public ViewCostDeductBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __costDeductBLL = BLLFactory.CreateCostDeductBLL();
            __usedConfirmBLL = BLLFactory.CreateUsedConfirmBLL();
            __depositRecordBLL = BLLFactory.CreateDepositRecordBLL();
            __userAccountBLL = BLLFactory.CreateUserAccountBLL();
            __userTypeBLL = BLLFactory.CreateUserTypeBLL();
            __tagBLL = BLLFactory.CreateTagBLL();
            __viewMaterialDepositRecordBLL = BLLFactory.CreateViewMaterialDepositRecordBLL();
            __mapping["Id"] = "CostDeductId";

        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewCostDeduct> viewCostDeductList, bool isSupressLazyLoad, bool isforBalanceAccount = false)
        {
            if (viewCostDeductList == null || viewCostDeductList.Count == 0) return;
            var costDeductPriviligeList = (IList<CostDeductPrivilige>)__costDeductBLL.GetCostDeductPriviliges(userId, viewCostDeductList);
            foreach (var viewCostDeduct in viewCostDeductList)
            {
                CostDeductPrivilige costDeductPrivilige = viewCostDeduct.EquipmentId.HasValue ?
                    costDeductPriviligeList.FirstOrDefault(P => P.CostDeductId.HasValue && P.CostDeductId == viewCostDeduct.Id) :
                    costDeductPriviligeList.FirstOrDefault(P => P.CostDeductId.HasValue);
                viewCostDeduct.IsSupressLazyLoad = false;
                viewCostDeduct.InitOperate();
                OperateDecorate(userId, viewCostDeduct, costDeductPrivilige, isforBalanceAccount);
                viewCostDeduct.BuildOperate();
                viewCostDeduct.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        public void OperateDecorate(Guid? userId, ViewCostDeduct viewCostDeduct, CostDeductPrivilige costDeductPrivilige, bool isforBalanceAccount = false)
        {
            if (viewCostDeduct == null) throw new ArgumentException("扣费信息为空");
            if (costDeductPrivilige == null)
            {
                viewCostDeduct.ViewCostDeductInfoOperate = "";
                viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                viewCostDeduct.EditCostDeductOperate = "";
                viewCostDeduct.DeleteCostDeductOperate = "";
                viewCostDeduct.EditManualCostDeductOperate = "";
                viewCostDeduct.DeleteManualCostDeductOperate = "";
                viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                viewCostDeduct.AddFeedBackOperate = "";
                viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                viewCostDeduct.EditNMPCostDeductOperate = "";
                viewCostDeduct.DeleteNMPCostDeductOperate = "";
                viewCostDeduct.AddNMPFeedBackOperate = "";
                viewCostDeduct.PrintOperate = "";
                return;
            }
            if (viewCostDeduct.HasClossingAccount)
            {
                viewCostDeduct.EditCostDeductOperate = "";
                viewCostDeduct.DeleteCostDeductOperate = "";
                viewCostDeduct.EditManualCostDeductOperate = "";
                viewCostDeduct.DeleteManualCostDeductOperate = "";
                viewCostDeduct.AddFeedBackOperate = "";
                viewCostDeduct.EditNMPCostDeductOperate = "";
                viewCostDeduct.DeleteNMPCostDeductOperate = "";
                viewCostDeduct.AddNMPFeedBackOperate = "";
            }
            switch (viewCostDeduct.CostDeductTypeEnum)
            {
                case CostDeductType.AppointmentPredictCostDeduct:
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    break;
                case CostDeductType.ManualCostDeduct:
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    break;
                case CostDeductType.SampleCostDeduct:
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    break;
                case CostDeductType.UsedCostDeduct:
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    if (!costDeductPrivilige.IsEnableViewCostDeduct)
                    {
                        viewCostDeduct.ViewCostDeductInfoOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnableSaveCosetDeduct && !costDeductPrivilige.IsEnableReCosetDeduct)
                    {
                        viewCostDeduct.EditCostDeductOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnabelDeleteCostDeduct)
                    {
                        viewCostDeduct.DeleteCostDeductOperate = "";
                    }
                    if (viewCostDeduct.HasClossingAccount)
                    {
                        viewCostDeduct.EditCostDeductOperate = "";
                        viewCostDeduct.DeleteCostDeductOperate = "";
                        viewCostDeduct.AddFeedBackOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnableAddFeedBackOperate)
                    {
                        viewCostDeduct.AddFeedBackOperate = "";
                    }
                    break;
                case CostDeductType.MaterialCostDeduct:
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    break;
                case CostDeductType.Animal:
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    break;
                case CostDeductType.WaterControlCostDeduct:
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.PrintOperate = "";
                    viewCostDeduct.AddNMPFeedBackOperate = "";
                    viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    viewCostDeduct.EditNMPCostDeductOperate = "";
                    viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    break;
                case CostDeductType.NMPUsedCostDeduct:
                    viewCostDeduct.ViewAppointmentPreCostDeductInfoOperate = "";
                    viewCostDeduct.ViewManualCostDeductInfoOperate = "";
                    viewCostDeduct.ViewCostDeductInfoOperate = "";
                    viewCostDeduct.EditManualCostDeductOperate = "";
                    viewCostDeduct.DeleteManualCostDeductOperate = "";
                    viewCostDeduct.ViewSampleApplyDeductInfoOperate = "";
                    viewCostDeduct.ViewMaterialOutputInfoOperate = "";
                    viewCostDeduct.ViewAnimalCostDeductInfoOperate = "";
                    viewCostDeduct.ViewWaterControlCostDeductInfoOperate = "";
                    viewCostDeduct.EditWaterControlCostDeductOperate = "";
                    viewCostDeduct.DeleteWaterControlCostDeductOperate = "";
                    viewCostDeduct.AddFeedBackOperate = "";
                    viewCostDeduct.EditCostDeductOperate = "";
                    viewCostDeduct.DeleteCostDeductOperate = "";
                    if (!costDeductPrivilige.IsEnableViewNMPCostDeduct)
                    {
                        viewCostDeduct.ViewNMPCostDeductInfoOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnableSaveNMPCosetDeduct && !costDeductPrivilige.IsEnableReNMPCosetDeduct)
                    {
                        viewCostDeduct.EditNMPCostDeductOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnabelDeleteNMPCostDeduct)
                    {
                        viewCostDeduct.DeleteNMPCostDeductOperate = "";
                    }
                    if (viewCostDeduct.HasClossingAccount)
                    {
                        viewCostDeduct.EditNMPCostDeductOperate = "";
                        viewCostDeduct.DeleteNMPCostDeductOperate = "";
                        viewCostDeduct.AddNMPFeedBackOperate = "";
                    }
                    if (!costDeductPrivilige.IsEnableAddFeedBackOperate)
                    {
                        viewCostDeduct.AddNMPFeedBackOperate = "";
                    }
                    break;
            }
            if (isforBalanceAccount)//结算只能查看信息
            {
                viewCostDeduct.EditCostDeductOperate = "";
                viewCostDeduct.DeleteCostDeductOperate = "";
                viewCostDeduct.EditManualCostDeductOperate = "";
                viewCostDeduct.DeleteManualCostDeductOperate = "";
                viewCostDeduct.EditNMPCostDeductOperate = "";
                viewCostDeduct.DeleteNMPCostDeductOperate = "";
            }
        }
        public string GenerateQueryExpression(Guid? userId, string expression, CostDeductType? costDeductType = null, bool isManageList = true, string controllerName = "", string actionName = "")
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageCostDeduct(userId, costDeductType, ref dataGridSettings, connectionName, actionName))
            {
                return "Id=null";
            }
            return dataGridSettings.QueryExpression;
        }

        public IList<ViewCostDeduct> GetViewCostDeductListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, CostDeductType? costDeductType = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, costDeductType, isManageList);
            IList<ViewCostDeduct> viewCostDeductList = null;
            if (costDeductType.HasValue)
            {
                var dataGirdSetings = GenerateDataGridSetting(expression, orderExpression, 1, int.MaxValue, __mapping);
                var gridData = GenerateViewCostDedudctListByCostDeductType(costDeductType.Value, dataGirdSetings, isSupressLazyLoad);
                if (gridData != null) viewCostDeductList = gridData.Data;
            }
            else
            {
                viewCostDeductList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            }
            if (isIniOperate) ExcuteOperateDecorate(userId, viewCostDeductList, isSupressLazyLoad, isforBalanceAccount);
            return viewCostDeductList;
        }
        public IList<ViewCostDeduct> GetViewCostDeductListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, CostDeductType? costDeductType = null, bool isManageList = true, bool isIniOperate = true)
        {
            expression = GenerateQueryExpression(userId, expression, costDeductType, isManageList);
            IList<ViewCostDeduct> viewCostDeductList = null;
            if (costDeductType.HasValue)
            {
                var dataGirdSetings = GenerateDataGridSetting(expression, orderExpression, 1, int.MaxValue, __mapping);
                var gridData = GenerateViewCostDedudctListByCostDeductType(costDeductType.Value, dataGirdSetings, isSupressLazyLoad);
                if (gridData != null) viewCostDeductList = gridData.Data;
            }
            else
            {
                viewCostDeductList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            }
            if (isIniOperate) ExcuteOperateDecorate(userId, viewCostDeductList, isSupressLazyLoad, isforBalanceAccount);
            return viewCostDeductList;
        }
        public IList<ViewCostDeduct> GetViewCostDeductListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, CostDeductType? costDeductType = null, bool isManageList = true, bool isIniOperate = true)
        {
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            IList<ViewCostDeduct> viewCostDeductList = null;
            if (costDeductType.HasValue)
            {
                var gridData = GenerateViewCostDedudctListByCostDeductType(costDeductType.Value, dataGridSettings, isSupressLazyLoad);
                if (gridData != null) viewCostDeductList = gridData.Data;
            }
            else
            {
                viewCostDeductList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            }
            if (isIniOperate) ExcuteOperateDecorate(userId, viewCostDeductList, isSupressLazyLoad, isforBalanceAccount);
            return viewCostDeductList;
        }
        public GridData<ViewCostDeduct> GetGridViewCostDeductListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, CostDeductType? costDeductType = null, bool isManageList = true, bool isIniOperate = true)
        {
            GridData<ViewCostDeduct> gridViewCostDeductList = null;
            expression = GenerateQueryExpression(userId, expression, costDeductType, isManageList);
            if (costDeductType.HasValue)
            {
                var dataGirdSetings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, __mapping);
                gridViewCostDeductList = GenerateViewCostDedudctListByCostDeductType(costDeductType.Value, dataGirdSetings, isSupressLazyLoad);
            }
            else
            {
                gridViewCostDeductList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            }
            if (isIniOperate) ExcuteOperateDecorate(userId, gridViewCostDeductList == null ? null : gridViewCostDeductList.Data, isSupressLazyLoad, isforBalanceAccount);
            return gridViewCostDeductList;
        }
        public GridData<ViewCostDeduct> GetGridViewCostDeductListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, CostDeductType? costDeductType = null, bool isManageList = true, bool isIniOperate = true)
        {
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            GridData<ViewCostDeduct> gridViewCostDeductList = null;
            if (costDeductType.HasValue)
            {
                gridViewCostDeductList = GenerateViewCostDedudctListByCostDeductType(costDeductType.Value, dataGridSettings, isSupressLazyLoad);
            }
            else
            {
                gridViewCostDeductList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            }
            if (isIniOperate) ExcuteOperateDecorate(userId, gridViewCostDeductList == null ? null : gridViewCostDeductList.Data, isSupressLazyLoad, isforBalanceAccount);
            return gridViewCostDeductList;
        }
        public GridData<ViewCostDeduct> GetPayerGridViewCostDeductListByExpression(Guid? userId, Guid payerId, CostDeductType costDeductType, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, bool isManageList = true, bool isIniOperate = true)
        {
            if (payerId != Guid.Empty)
            {
                dataGridSettings.AppendAndQueryExpression(string.Format("PayerId=\"{0}\"", payerId));
            }
            var gridViewCostDeductList = GenerateViewCostDedudctListByCostDeductType(costDeductType, dataGridSettings, isSupressLazyLoad);
            if (isIniOperate) ExcuteOperateDecorate(userId, gridViewCostDeductList == null ? null : gridViewCostDeductList.Data, isSupressLazyLoad, isforBalanceAccount);
            return gridViewCostDeductList;
        }
        public IList<ViewCostDeduct> GetPayerViewCostDeductListByExpression(Guid? userId, Guid payerId, CostDeductType costDeductType, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isforBalanceAccount = false, bool isManageList = true, bool isIniOperate = true)
        {
            if (payerId != Guid.Empty)
            {
                dataGridSettings.AppendAndQueryExpression(string.Format("PayerId=\"{0}\"", payerId));
            }
            IList<ViewCostDeduct> viewCostDeductList = null;
            var gridData = GenerateViewCostDedudctListByCostDeductType(costDeductType, dataGridSettings, isSupressLazyLoad);
            if (gridData != null) viewCostDeductList = gridData.Data;
            if (isIniOperate) ExcuteOperateDecorate(userId, viewCostDeductList, isSupressLazyLoad, isforBalanceAccount);
            return viewCostDeductList;
        }
        public IList<CostDeductPayerStatistics> GetViewCostDeductPayerStatistics(Guid? userId, DataGridSettings dataGridSettings, out int returnCount, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            returnCount = 0;
            IList<CostDeductPayerStatistics> lstCostDeductPayerStatistics = new List<CostDeductPayerStatistics>();
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            DateTime? startAt = null;
            DateTime? endAt = null;
            GetCostDeductStatisticsTimeParameter(dataGridSettings, ref startAt, ref endAt);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
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
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetCostDeductPayerStatistics", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var payerId = new Guid(result[0].ToString());
                    var payerName = result[1] != null ? result[1].ToString() : "";
                    var payerRelativePic = result[2] != null ? result[2].ToString() : "";
                    var usedCount = Convert.ToInt32(result[3] == DBNull.Value ? 0 : result[3]);
                    var usedHour = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                    var calcDuration = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                    var costRealCurrency = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                    var costVirtualCurrency = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                    var costOpenFundRealCurrency = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                    var costSubsidiesCurrency = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                    var depositRealSum = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                    var depositVirtualSum = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                    var openFundDepositSum = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                    var openFundOpenFundSum = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                    var materialDepositSum = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                    var realCurrency = Convert.ToDouble(result[15] == DBNull.Value ? 0 : result[15]);
                    var virtualCurrency = Convert.ToDouble(result[16] == DBNull.Value ? 0 : result[16]);
                    var balanceMoneyCurrency = Convert.ToDouble(result[17] == DBNull.Value ? 0 : result[17]);
                    var openFundClosingSum = Convert.ToDouble(result[18] == DBNull.Value ? 0 : result[18]);
                    var openFundClosedSum = Convert.ToDouble(result[19] == DBNull.Value ? 0 : result[19]);
                    var payerOrgName = result[20] != null ? result[20].ToString() : "";

                    CostDeductPayerStatistics costDeductPayerStatistics = new CostDeductPayerStatistics()
                    {
                        PayerId = payerId,
                        PayerName = payerName,
                        PayerRelativePic = payerRelativePic,
                        UsedCount = usedCount,
                        UsedHour = usedHour,
                        CalcDuration = calcDuration,
                        CostRealCurrency = costRealCurrency,
                        CostVirtualCurrency = costVirtualCurrency,
                        CostOpenFundRealCurrency = costOpenFundRealCurrency,
                        CostSubsidiesCurrency = costSubsidiesCurrency,
                        DepositRealSum = depositRealSum,
                        DepositVirtualSum = depositVirtualSum,
                        OpenFundDepositSum = openFundDepositSum,
                        OpenFundOpenFundSum = openFundOpenFundSum,
                        MaterialDepositSum = materialDepositSum,
                        RealCurrency = realCurrency,
                        VirtualCurrency = virtualCurrency,
                        BalanceMoneyCurrency = balanceMoneyCurrency,
                        OpenFundClosingSum = openFundClosingSum,
                        OpenFundClosedSum = openFundClosedSum,
                        PayerOrgName = payerOrgName
                    };
                    costDeductPayerStatistics.InitOperate();
                    costDeductPayerStatistics.BuildOperate();
                    lstCostDeductPayerStatistics.Add(costDeductPayerStatistics);
                    returnCount = Convert.ToInt32(returnValueDataParameter.Value);
                }
            }
            return lstCostDeductPayerStatistics;
        }
        public GridData<CostDeductPayerStatistics> GetGridViewCostDeductPayerStatistics(Guid? userId, DataGridSettings dataGridSettings, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            int count = 0;
            var costDeductPayerStatisticsList = GetViewCostDeductPayerStatistics(userId, dataGridSettings, out count, costDeductType, isManageList);
            return new GridData<CostDeductPayerStatistics>()
            {
                Data = costDeductPayerStatisticsList,
                Count = count
            };
        }
        public CostDeductPayerStatistics GetCostDeductPayerStatisticsSum(Guid? userId, DataGridSettings dataGridSettings, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            DateTime? startAt = null;
            DateTime? endAt = null;
            GetCostDeductStatisticsTimeParameter(dataGridSettings, ref startAt, ref endAt);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int payerCount = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", payerCount, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetCostDeductPayerStatisticsSum", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                var result = results[0];
                var usedCount = Convert.ToInt32(result[0] == DBNull.Value ? 0 : result[0]);
                var usedHour = Convert.ToDouble(result[1] == DBNull.Value ? 0 : result[1]);
                var calcDuration = Convert.ToDouble(result[2] == DBNull.Value ? 0 : result[2]);
                var costRealCurrency = Convert.ToDouble(result[3] == DBNull.Value ? 0 : result[3]);
                var costVirtualCurrency = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                var costOpenFundRealCurrency = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                var costSubsidiesCurrency = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                var depositRealSum = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                var depositVirtualSum = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                var openFundDepositSum = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                var openFundOpenFundSum = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                var materialDepositSum = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                var realCurrency = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                var virtualCurrency = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                var balanceMoneyCurrency = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                var openFundClosingSum = Convert.ToDouble(result[15] == DBNull.Value ? 0 : result[15]);
                var openFundClosedSum = Convert.ToDouble(result[16] == DBNull.Value ? 0 : result[16]);
                payerCount = Convert.ToInt32(returnValueDataParameter.Value);

                CostDeductPayerStatistics costDeductPayerStatistics = new CostDeductPayerStatistics()
                {
                    PayerId = Guid.Empty,
                    PayerName = payerCount.ToString(),
                    PayerNameStr = payerCount.ToString(),
                    UsedCount = usedCount,
                    UsedHour = usedHour,
                    CalcDuration = calcDuration,
                    CostRealCurrency = costRealCurrency,
                    CostVirtualCurrency = costVirtualCurrency,
                    CostOpenFundRealCurrency = costOpenFundRealCurrency,
                    CostSubsidiesCurrency = costSubsidiesCurrency,
                    DepositRealSum = depositRealSum,
                    DepositVirtualSum = depositVirtualSum,
                    OpenFundDepositSum = openFundDepositSum,
                    OpenFundOpenFundSum = openFundOpenFundSum,
                    MaterialDepositSum = materialDepositSum,
                    RealCurrency = realCurrency,
                    VirtualCurrency = virtualCurrency,
                    BalanceMoneyCurrency = balanceMoneyCurrency,
                    OpenFundClosingSum = openFundClosingSum,
                    OpenFundClosedSum = openFundClosedSum
                };
                return costDeductPayerStatistics;
            }
            return null;

        }
        public IList<CostDeductEquipmentStatistics> GetViewCostDeductEquipmentStatistics(Guid? userId, DataGridSettings dataGridSettings, out int returnCount, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            returnCount = 0;
            IList<CostDeductEquipmentStatistics> lstCostDeductEquipmentStatistics = new List<CostDeductEquipmentStatistics>();
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            DateTime? startAt = null;
            DateTime? endAt = null;
            GetCostDeductStatisticsTimeParameter(dataGridSettings, ref startAt, ref endAt);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
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
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int returnValue = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetCostDeductEquipmentStatistics", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                foreach (var result in results)
                {
                    var equipmentId = new Guid(result[0].ToString());
                    var equipmentName = result[1] != null ? result[1].ToString() : "";
                    var equipmentRelativePic = result[2] != null ? result[2].ToString() : "";
                    var userCount = Convert.ToInt32(result[3] == DBNull.Value ? 0 : result[3]);
                    var payerCount = Convert.ToInt32(result[4] == DBNull.Value ? 0 : result[4]);
                    var usedCount = Convert.ToInt32(result[5] == DBNull.Value ? 0 : result[5]);
                    var usedHour = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                    var calcDuration = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                    var costRealCurrency = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                    var costVirtualCurrency = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                    var costOpenFundRealCurrency = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                    var costSubsidiesCurrency = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                    var costAppointmentRealCurrency = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                    var costAppointmentVirtualCurrency = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                    var costAppointmentOpenFundRealCurrency = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                    var costAppointmentSubsidiesCurrency = Convert.ToDouble(result[15] == DBNull.Value ? 0 : result[15]);
                    var costSampleApplyRealCurrency = Convert.ToDouble(result[16] == DBNull.Value ? 0 : result[16]);
                    var costSampleApplyVirtualCurrency = Convert.ToDouble(result[17] == DBNull.Value ? 0 : result[17]);
                    var costSampleApplyOpenFundRealCurrency = Convert.ToDouble(result[18] == DBNull.Value ? 0 : result[18]);
                    var costSampleApplySubsidiesCurrency = Convert.ToDouble(result[19] == DBNull.Value ? 0 : result[19]);
                    var deductType = Convert.ToInt32(result[20] == DBNull.Value ? 0 : result[20]);
                    var studentCount = Convert.ToInt32(result[21] == DBNull.Value ? 0 : result[21]);
                    var tutorCount = Convert.ToInt32(result[22] == DBNull.Value ? 0 : result[22]);
                    var otherCount = Convert.ToInt32(result[23] == DBNull.Value ? 0 : result[23]);
                    var outcommerCount = Convert.ToInt32(result[24] == DBNull.Value ? 0 : result[24]);
                    var outTutorCount = Convert.ToInt32(result[25] == DBNull.Value ? 0 : result[25]);
                    var tagCounts = Convert.ToString(result[26] == DBNull.Value ? "" : result[26]);
                    var equipmentOrgName = result[27] != null ? result[27].ToString() : "";
                    CostDeductEquipmentStatistics costDeductEquipmentStatistics = new CostDeductEquipmentStatistics()
                    {
                        EquipmentId = equipmentId,
                        EquipmentName = equipmentName,
                        EquipmentOrgName = equipmentOrgName,
                        EquipmentRelativePic = equipmentRelativePic,
                        UserCount = userCount,
                        PayerCount = payerCount,
                        UsedCount = usedCount,
                        UsedHour = usedHour,
                        CalcDuration = calcDuration,
                        CostRealCurrency = costRealCurrency,
                        CostVirtualCurrency = costVirtualCurrency,
                        CostOpenFundRealCurrency = costOpenFundRealCurrency,
                        CostSubsidiesCurrency = costSubsidiesCurrency,
                        CostAppointmentRealCurrency = costAppointmentRealCurrency,
                        CostAppointmentVirtualCurrency = costAppointmentVirtualCurrency,
                        CostAppointmentOpenFundRealCurrency = costAppointmentOpenFundRealCurrency,
                        CostAppointmentSubsidiesCurrency = costAppointmentSubsidiesCurrency,
                        CostSampleApplyRealCurrency = costSampleApplyRealCurrency,
                        CostSampleApplyVirtualCurrency = costSampleApplyVirtualCurrency,
                        CostSampleApplyOpenFundRealCurrency = costSampleApplyOpenFundRealCurrency,
                        CostSampleApplySubsidiesCurrency = costSampleApplySubsidiesCurrency,
                        DeductType = deductType,
                        UserIdentityInfoList = new List<UserIdentityStruct>(),
                        PayerTagInfoList = new List<UserTagStruct>()
                    };
                    if (costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.ManualCostDeduct
                        || costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.MaterialCostDeduct
                        || costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.SampleCostDeduct)
                    {
                        costDeductEquipmentStatistics.UsedHour = null;
                        costDeductEquipmentStatistics.CalcDuration = null;
                    }

                    IList<UserIdentityStruct> userIdentityStructList = new List<UserIdentityStruct>();
                    foreach (var userIdentity in System.Enum.GetValues(typeof(UserIdentity)))
                    {
                        UserIdentityStruct userIdentityStruct = new UserIdentityStruct();
                        userIdentityStruct.UserIdentity = (int)userIdentity;
                        userIdentityStruct.UserIdentityName = ((UserIdentity)userIdentity).ToCnName();
                        switch ((UserIdentity)userIdentity)
                        {
                            case UserIdentity.Student:
                                userIdentityStruct.StatisticsCount = studentCount;
                                break;
                            case UserIdentity.Tutor:
                                userIdentityStruct.StatisticsCount = tutorCount;
                                break;
                            case UserIdentity.Other:
                                userIdentityStruct.StatisticsCount = otherCount;
                                break;
                            case UserIdentity.Outcommer:
                                userIdentityStruct.StatisticsCount = outcommerCount;
                                break;
                            case UserIdentity.OutTutor:
                                userIdentityStruct.StatisticsCount = outTutorCount;
                                break;
                            default:
                                userIdentityStruct.StatisticsCount = 0;
                                break;
                        }
                        costDeductEquipmentStatistics.UserIdentityInfoList.Add(userIdentityStruct);
                    }
                    var tagList = __tagBLL.GetEntitiesByExpression(string.Format("IsDelete=false"), null, "XPath");
                    string[] tagCountList = tagCounts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tagList != null && tagList.Count() > 0 && tagCountList != null && tagCountList.Count() == tagList.Count())
                    {
                        int i = 0;
                        foreach (var tag in tagList)
                        {
                            UserTagStruct userTagStruct = new UserTagStruct();
                            userTagStruct.TagId = tag.Id;
                            userTagStruct.TagName = tag.Name;
                            userTagStruct.StatisticsCount = Int32.Parse(tagCountList[i]);
                            costDeductEquipmentStatistics.PayerTagInfoList.Add(userTagStruct);
                            i++;
                        }
                    }

                    costDeductEquipmentStatistics.InitOperate();
                    costDeductEquipmentStatistics.BuildOperate();
                    lstCostDeductEquipmentStatistics.Add(costDeductEquipmentStatistics);
                    returnCount = Convert.ToInt32(returnValueDataParameter.Value);
                }
            }
            return lstCostDeductEquipmentStatistics;
        }
        public GridData<CostDeductEquipmentStatistics> GetGridViewCostDeductEquipmentStatistics(Guid? userId, DataGridSettings dataGridSettings, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            int count = 0;
            var costDeductEquipmentStatisticsList = GetViewCostDeductEquipmentStatistics(userId, dataGridSettings, out count, costDeductType, isManageList);
            return new GridData<CostDeductEquipmentStatistics>()
            {
                Data = costDeductEquipmentStatisticsList,
                Count = count
            };
        }
        public CostDeductEquipmentStatistics GetCostDeductEquipmentStatisticsSum(Guid? userId, DataGridSettings dataGridSettings, CostDeductType? costDeductType = null, bool isManageList = true)
        {
            dataGridSettings.QueryExpression = GenerateQueryExpression(userId, dataGridSettings.QueryExpression, costDeductType, isManageList);
            DateTime? startAt = null;
            DateTime? endAt = null;
            GetCostDeductStatisticsTimeParameter(dataGridSettings, ref startAt, ref endAt);
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            dataParams.Add(startAt.HasValue ? DataParameterFactory.CreateDataParameter("startAt", startAt.Value) : DataParameterFactory.CreateDataParameter("startAt", DBNull.Value));
            dataParams.Add(endAt.HasValue ? DataParameterFactory.CreateDataParameter("endAt", endAt.Value) : DataParameterFactory.CreateDataParameter("endAt", DBNull.Value));
            int equipmentCount = 0;
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", equipmentCount, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetCostDeductEquipmentStatisticsSum", dataParams);
            var results = (IList<object[]>)execResult.Result;
            if (results != null && results.Count > 0)
            {
                var result = results[0];
                var userCount = Convert.ToInt32(result[0] == DBNull.Value ? 0 : result[0]);
                var payerCount = Convert.ToInt32(result[1] == DBNull.Value ? 0 : result[1]);
                var usedCount = Convert.ToInt32(result[2] == DBNull.Value ? 0 : result[2]);
                var usedHour = Convert.ToDouble(result[3] == DBNull.Value ? 0 : result[3]);
                var calcDuration = Convert.ToDouble(result[4] == DBNull.Value ? 0 : result[4]);
                var costRealCurrency = Convert.ToDouble(result[5] == DBNull.Value ? 0 : result[5]);
                var costVirtualCurrency = Convert.ToDouble(result[6] == DBNull.Value ? 0 : result[6]);
                var costOpenFundRealCurrency = Convert.ToDouble(result[7] == DBNull.Value ? 0 : result[7]);
                var costSubsidiesCurrency = Convert.ToDouble(result[8] == DBNull.Value ? 0 : result[8]);
                var costAppointmentRealCurrency = Convert.ToDouble(result[9] == DBNull.Value ? 0 : result[9]);
                var costAppointmentVirtualCurrency = Convert.ToDouble(result[10] == DBNull.Value ? 0 : result[10]);
                var costAppointmentOpenFundRealCurrency = Convert.ToDouble(result[11] == DBNull.Value ? 0 : result[11]);
                var costAppointmentSubsidiesCurrency = Convert.ToDouble(result[12] == DBNull.Value ? 0 : result[12]);
                var costSampleApplyRealCurrency = Convert.ToDouble(result[13] == DBNull.Value ? 0 : result[13]);
                var costSampleApplyVirtualCurrency = Convert.ToDouble(result[14] == DBNull.Value ? 0 : result[14]);
                var costSampleApplyOpenFundRealCurrency = Convert.ToDouble(result[15] == DBNull.Value ? 0 : result[15]);
                var costSampleApplySubsidiesCurrency = Convert.ToDouble(result[16] == DBNull.Value ? 0 : result[16]);
                var studentCount = Convert.ToInt32(result[17] == DBNull.Value ? 0 : result[17]);
                var tutorCount = Convert.ToInt32(result[18] == DBNull.Value ? 0 : result[18]);
                var otherCount = Convert.ToInt32(result[19] == DBNull.Value ? 0 : result[19]);
                var outcommerCount = Convert.ToInt32(result[20] == DBNull.Value ? 0 : result[20]);
                var outTutorCount = Convert.ToInt32(result[21] == DBNull.Value ? 0 : result[21]);
                var tagCounts = Convert.ToString(result[22] == DBNull.Value ? "" : result[22]);
                equipmentCount = Convert.ToInt32(returnValueDataParameter.Value);

                CostDeductEquipmentStatistics costDeductEquipmentStatistics = new CostDeductEquipmentStatistics()
                {
                    EquipmentId = Guid.Empty,
                    EquipmentName = equipmentCount.ToString(),
                    EquipmentNameStr = equipmentCount.ToString(),
                    UserCount = userCount,
                    PayerCount = payerCount,
                    UsedCount = usedCount,
                    UsedHour = usedHour,
                    CalcDuration = calcDuration,
                    CostRealCurrency = costRealCurrency,
                    CostVirtualCurrency = costVirtualCurrency,
                    CostOpenFundRealCurrency = costOpenFundRealCurrency,
                    CostSubsidiesCurrency = costSubsidiesCurrency,
                    CostAppointmentRealCurrency = costAppointmentRealCurrency,
                    CostAppointmentVirtualCurrency = costAppointmentVirtualCurrency,
                    CostAppointmentOpenFundRealCurrency = costAppointmentOpenFundRealCurrency,
                    CostAppointmentSubsidiesCurrency = costAppointmentSubsidiesCurrency,
                    CostSampleApplyRealCurrency = costSampleApplyRealCurrency,
                    CostSampleApplyVirtualCurrency = costSampleApplyVirtualCurrency,
                    CostSampleApplyOpenFundRealCurrency = costSampleApplyOpenFundRealCurrency,
                    CostSampleApplySubsidiesCurrency = costSampleApplySubsidiesCurrency,
                    UserIdentityInfoList = new List<UserIdentityStruct>(),
                    PayerTagInfoList = new List<UserTagStruct>()
                };
                if (costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.ManualCostDeduct
                    || costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.MaterialCostDeduct
                    || costDeductEquipmentStatistics.DeductTypeEnum == CostDeductType.SampleCostDeduct)
                {
                    costDeductEquipmentStatistics.UsedHour = null;
                    costDeductEquipmentStatistics.CalcDuration = null;
                }

                IList<UserIdentityStruct> userIdentityStructList = new List<UserIdentityStruct>();
                foreach (var userIdentity in System.Enum.GetValues(typeof(UserIdentity)))
                {
                    UserIdentityStruct userIdentityStruct = new UserIdentityStruct();
                    userIdentityStruct.UserIdentity = (int)userIdentity;
                    userIdentityStruct.UserIdentityName = ((UserIdentity)userIdentity).ToCnName();
                    switch ((UserIdentity)userIdentity)
                    {
                        case UserIdentity.Student:
                            userIdentityStruct.StatisticsCount = studentCount;
                            break;
                        case UserIdentity.Tutor:
                            userIdentityStruct.StatisticsCount = tutorCount;
                            break;
                        case UserIdentity.Other:
                            userIdentityStruct.StatisticsCount = otherCount;
                            break;
                        case UserIdentity.Outcommer:
                            userIdentityStruct.StatisticsCount = outcommerCount;
                            break;
                        case UserIdentity.OutTutor:
                            userIdentityStruct.StatisticsCount = outTutorCount;
                            break;
                        default:
                            userIdentityStruct.StatisticsCount = 0;
                            break;
                    }
                    costDeductEquipmentStatistics.UserIdentityInfoList.Add(userIdentityStruct);
                }
                var tagList = __tagBLL.GetEntitiesByExpression(string.Format("IsDelete=false"), null, "XPath");
                string[] tagCountList = tagCounts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (tagList != null && tagList.Count() > 0 && tagCountList != null && tagCountList.Count() == tagList.Count())
                {
                    int i = 0;
                    foreach (var tag in tagList)
                    {
                        UserTagStruct userTagStruct = new UserTagStruct();
                        userTagStruct.TagId = tag.Id;
                        userTagStruct.TagName = tag.Name;
                        userTagStruct.StatisticsCount = Int32.Parse(tagCountList[i]);
                        costDeductEquipmentStatistics.PayerTagInfoList.Add(userTagStruct);
                        i++;
                    }
                }
                return costDeductEquipmentStatistics;
            }
            return null;
        }
        public void GetCostDeductStatisticsTimeParameter(DataGridSettings dataGridSettings, ref DateTime? startAt, ref DateTime? endAt)
        {

            if (!string.IsNullOrWhiteSpace(dataGridSettings.QueryExpression) && dataGridSettings.QueryExpression.IndexOf("BeginAt") != -1)
            {
                var groupsStartAt = new System.Text.RegularExpressions.Regex("BeginAt>\"([^\"]*)\"").Match(dataGridSettings.QueryExpression).Groups;
                if (groupsStartAt.Count > 1)
                {
                    var tempTime = groupsStartAt[1].Value.ToString();
                    DateTime time;
                    if (!string.IsNullOrWhiteSpace(tempTime) && DateTime.TryParse(tempTime, out time)) startAt = time;
                }
                var groupsEndAt = new System.Text.RegularExpressions.Regex("BeginAt<\"([^\"]*)\"").Match(dataGridSettings.QueryExpression).Groups;
                if (groupsEndAt.Count > 1)
                {
                    var tempTime = groupsEndAt[1].Value.ToString();
                    DateTime time;
                    if (!string.IsNullOrWhiteSpace(tempTime) && DateTime.TryParse(tempTime, out time)) endAt = time;
                }
            }
        }
        public override GridData<ViewCostDeduct> GetGridEntitiesByExpression(string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (mapping == null)
                mapping = new Dictionary<string, string>();
            mapping["Id"] = "CostDeductId";
            var dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, mapping);
            GridData<ViewAppointment> gridData = new GridData<ViewAppointment>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, mapping);
            List<IDataParameter> dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
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
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetViewCostDeductList", dataParams).Result;
            var count = returnValueDataParameter.Value == null ? 0 : int.Parse(returnValueDataParameter.Value.ToString());
            return FillGridDataViewCostDeduct(results, count, isSupressLazyLoad);
        }
        private GridData<ViewCostDeduct> GenerateViewCostDedudctListByCostDeductType(CostDeductType costDeductType, DataGridSettings dataGridSettings, bool isSupressLazyLoad)
        {
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, __mapping);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var costDeductTypeDataParam = DataParameterFactory.CreateDataParameter("costDeductType", (int)costDeductType);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var pageIndexDataParam = DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex);
            var pageCountDataParam = DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 costDeductTypeDataParam,
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 pageIndexDataParam,
                 pageCountDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetCostDeductList", dataParams).Result;
            var count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            return FillGridDataViewCostDeduct(results, count, isSupressLazyLoad);
        }
        private GridData<ViewCostDeduct> FillGridDataViewCostDeduct(IList<object[]> results, int count, bool isSupressLazyLoad)
        {
            var gridData = new GridData<ViewCostDeduct>();
            if (results != null && results.Count > 0)
            {
                IList<ViewCostDeduct> viewCostDeducts = new List<ViewCostDeduct>();
                foreach (var result in results)
                {
                    ViewCostDeduct viewCostDeduct = new ViewCostDeduct();
                    viewCostDeduct.Id = new Guid(result[0].ToString());
                    viewCostDeduct.Title = result[1] != null ? result[1].ToString() : null;
                    viewCostDeduct.SampleItemName = result[2] != null ? result[2].ToString() : null;
                    viewCostDeduct.DeductAt = null;
                    if (result[3] != null && !string.IsNullOrWhiteSpace(result[3].ToString())) viewCostDeduct.DeductAt = DateTime.Parse(result[3].ToString());
                    viewCostDeduct.HasClossingAccount = Convert.ToBoolean(result[4].ToString());
                    viewCostDeduct.VirtualCurrency = null;
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) viewCostDeduct.VirtualCurrency = double.Parse(result[5].ToString());
                    viewCostDeduct.RealCurrency = null;
                    if (result[6] != null && !string.IsNullOrWhiteSpace(result[6].ToString())) viewCostDeduct.RealCurrency = double.Parse(result[6].ToString());
                    viewCostDeduct.TotalCurrency = null;
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString())) viewCostDeduct.TotalCurrency = double.Parse(result[7].ToString());
                    viewCostDeduct.UserAccountId = new Guid(result[8].ToString());
                    viewCostDeduct.PayerId = null;
                    if (result[9] != null && !string.IsNullOrWhiteSpace(result[9].ToString())) viewCostDeduct.PayerId = new Guid(result[9].ToString());
                    viewCostDeduct.PayerName = result[10] != null ? result[10].ToString() : null;
                    viewCostDeduct.PayerRelativePic = result[11] != null ? result[11].ToString() : null;
                    viewCostDeduct.PayerUserTypeId = null;
                    if (result[12] != null && !string.IsNullOrWhiteSpace(result[12].ToString())) viewCostDeduct.PayerUserTypeId = new Guid(result[12].ToString());
                    viewCostDeduct.PayerTagId = null;
                    if (result[13] != null && !string.IsNullOrWhiteSpace(result[13].ToString())) viewCostDeduct.PayerTagId = new Guid(result[13].ToString());
                    viewCostDeduct.PayerOrgId = null;
                    if (result[14] != null && !string.IsNullOrWhiteSpace(result[14].ToString())) viewCostDeduct.PayerOrgId = new Guid(result[14].ToString());
                    viewCostDeduct.PayerOrgName = result[15] != null ? result[15].ToString() : null;
                    viewCostDeduct.PayerOrgXPath = result[16] != null ? result[16].ToString() : null;
                    viewCostDeduct.CreaterId = null;
                    if (result[17] != null && !string.IsNullOrWhiteSpace(result[17].ToString())) viewCostDeduct.CreaterId = new Guid(result[17].ToString());
                    viewCostDeduct.CreaterName = result[18] != null ? result[18].ToString() : null;
                    viewCostDeduct.CostDeductType = int.Parse(result[19].ToString());
                    viewCostDeduct.Duration = null;
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString())) viewCostDeduct.Duration = double.Parse(result[20].ToString());
                    viewCostDeduct.CalcDuration = null;
                    if (result[21] != null && !string.IsNullOrWhiteSpace(result[21].ToString())) viewCostDeduct.CalcDuration = double.Parse(result[21].ToString());
                    viewCostDeduct.BalanceAccountItemId = null;
                    if (result[22] != null && !string.IsNullOrWhiteSpace(result[22].ToString())) viewCostDeduct.BalanceAccountItemId = new Guid(result[22].ToString());
                    viewCostDeduct.Status = int.Parse(result[23].ToString());
                    viewCostDeduct.BusinessId = null;
                    if (result[24] != null && !string.IsNullOrWhiteSpace(result[24].ToString())) viewCostDeduct.BusinessId = new Guid(result[24].ToString());
                    viewCostDeduct.EquipmentId = null;
                    if (result[25] != null && !string.IsNullOrWhiteSpace(result[25].ToString())) viewCostDeduct.EquipmentId = new Guid(result[25].ToString());
                    viewCostDeduct.EquipmentName = result[26] != null ? result[26].ToString() : null;
                    viewCostDeduct.RelativePic = result[27] != null ? result[27].ToString() : null;
                    viewCostDeduct.EquipmentOrgId = null;
                    if (result[28] != null && !string.IsNullOrWhiteSpace(result[28].ToString())) viewCostDeduct.EquipmentOrgId = new Guid(result[28].ToString());

                    viewCostDeduct.LabRoomName = result[29] != null ? result[29].ToString() : null;
                    viewCostDeduct.SubjectId = null;
                    if (result[30] != null && !string.IsNullOrWhiteSpace(result[30].ToString())) viewCostDeduct.SubjectId = new Guid(result[30].ToString());
                    viewCostDeduct.SubjectName = result[31] != null ? result[31].ToString() : null;
                    viewCostDeduct.PaymentMethod = null;
                    if (result[32] != null && !string.IsNullOrWhiteSpace(result[32].ToString())) viewCostDeduct.PaymentMethod = int.Parse(result[32].ToString());
                    viewCostDeduct.UserId = null;
                    if (result[33] != null && !string.IsNullOrWhiteSpace(result[33].ToString())) viewCostDeduct.UserId = new Guid(result[33].ToString());
                    viewCostDeduct.UserName = result[34] != null ? result[34].ToString() : null;
                    viewCostDeduct.UserRelativePic = result[35] != null ? result[35].ToString() : null;
                    viewCostDeduct.UserUserTypeId = null;
                    if (result[36] != null && !string.IsNullOrWhiteSpace(result[36].ToString())) viewCostDeduct.UserUserTypeId = new Guid(result[36].ToString());
                    viewCostDeduct.UserTagId = null;
                    if (result[37] != null && !string.IsNullOrWhiteSpace(result[37].ToString())) viewCostDeduct.UserTagId = new Guid(result[37].ToString());
                    viewCostDeduct.RealFee = null;
                    if (result[38] != null && !string.IsNullOrWhiteSpace(result[38].ToString())) viewCostDeduct.RealFee = double.Parse(result[38].ToString());
                    viewCostDeduct.CalcFee = null;
                    if (result[39] != null && !string.IsNullOrWhiteSpace(result[39].ToString())) viewCostDeduct.CalcFee = double.Parse(result[39].ToString());
                    viewCostDeduct.BeginAt = null;
                    if (result[40] != null && !string.IsNullOrWhiteSpace(result[40].ToString())) viewCostDeduct.BeginAt = DateTime.Parse(result[40].ToString());
                    viewCostDeduct.EndAt = null;
                    if (result[41] != null && !string.IsNullOrWhiteSpace(result[41].ToString())) viewCostDeduct.EndAt = DateTime.Parse(result[41].ToString());
                    viewCostDeduct.UnitPrice = null;
                    if (result[42] != null && !string.IsNullOrWhiteSpace(result[42].ToString())) viewCostDeduct.UnitPrice = double.Parse(result[42].ToString());
                    viewCostDeduct.ChargeType = null;
                    if (result[43] != null && !string.IsNullOrWhiteSpace(result[43].ToString())) viewCostDeduct.ChargeType = int.Parse(result[43].ToString());
                    viewCostDeduct.BalanceAccountId = null;
                    if (result[44] != null && !string.IsNullOrWhiteSpace(result[44].ToString())) viewCostDeduct.BalanceAccountId = new Guid(result[44].ToString());
                    viewCostDeduct.SubjectProjectId = null;
                    if (result[45] != null && !string.IsNullOrWhiteSpace(result[45].ToString())) viewCostDeduct.SubjectProjectId = new Guid(result[45].ToString());
                    viewCostDeduct.SubjectProjectName = result[46] != null ? result[46].ToString() : null;
                    viewCostDeduct.UserWorkTeam = result[47] != null ? result[47].ToString() : null;
                    viewCostDeduct.SampleCount = null;
                    if (result[48] != null && !string.IsNullOrWhiteSpace(result[48].ToString())) viewCostDeduct.SampleCount = int.Parse(result[48].ToString());
                    viewCostDeduct.Remark = result[49] != null ? result[49].ToString() : null;
                    if (result[50] != null && !string.IsNullOrWhiteSpace(result[50].ToString())) viewCostDeduct.IsOpenFundCostDeduct = bool.Parse(result[50].ToString());
                    if (result[51] != null && !string.IsNullOrWhiteSpace(result[51].ToString())) viewCostDeduct.AnimalId = new Guid(result[51].ToString());
                    viewCostDeduct.AnimalName = result[52] != null ? result[52].ToString() : null;
                    if (result[53] != null && !string.IsNullOrWhiteSpace(result[53].ToString())) viewCostDeduct.AnimalCategoryId = new Guid(result[53].ToString());
                    viewCostDeduct.AnimalCategoryName = result[54] != null ? result[54].ToString() : null;
                    viewCostDeduct.EquipmentOrgName = result[55] != null ? result[55].ToString() : null;
                    viewCostDeduct.LabRoomXPath = result[56] != null ? result[56].ToString() : null;
                    if (result[57] != null && !string.IsNullOrWhiteSpace(result[57].ToString())) viewCostDeduct.OpenFundCurrency = double.Parse(result[57].ToString());

                    viewCostDeduct.NMPId = null;
                    if (result[58] != null && !string.IsNullOrWhiteSpace(result[58].ToString())) viewCostDeduct.NMPId = new Guid(result[58].ToString());
                    viewCostDeduct.NMPName = result[59] != null ? result[59].ToString() : null;
                    viewCostDeduct.NMPRelativePic = result[60] != null ? result[60].ToString() : null;
                    viewCostDeduct.NMPOrgId = null;
                    if (result[61] != null && !string.IsNullOrWhiteSpace(result[61].ToString())) viewCostDeduct.NMPOrgId = new Guid(result[61].ToString());
                    viewCostDeduct.NMPOrgName = result[62] != null ? result[62].ToString() : null;
                    viewCostDeduct.NMPEquipmentId = null;
                    if (result[63] != null && !string.IsNullOrWhiteSpace(result[63].ToString())) viewCostDeduct.NMPEquipmentId = new Guid(result[63].ToString());
                    viewCostDeduct.NMPEquipmentName = result[64] != null ? result[64].ToString() : null;
                    viewCostDeduct.UserTypeName = result[65] != null ? result[65].ToString() : null;
                    viewCostDeduct.IsSupressLazyLoad = isSupressLazyLoad;
                    viewCostDeducts.Add(viewCostDeduct);
                }
                gridData.Data = viewCostDeducts;
            }
            gridData.Count = count;
            return gridData;
        }
        private bool IsManageCostDeduct(Guid? userId, CostDeductType? costDeductType, ref DataGridSettings dataGridSettings, string controllerName, string actionName)
        {
            if (!userId.HasValue) return false;
            //var user = __userBLL.GetEntityById(userId.Value);
            //if (user == null) return false;
            //if (!user.IsSuperAdmin)
            var userIdValue = userId.Value;
            var viewUserSystemSettingBLL = BLLFactory.CreateViewUserSystemSettingBLL();
            var viewUserSystemSetting = viewUserSystemSettingBLL.GetEntityByUserId(userIdValue);
            if (viewUserSystemSetting != null && !viewUserSystemSettingBLL.IsSuperAdminWorkGroup(viewUserSystemSetting))
            {
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(userIdValue, new ManagerUserOrgId(ModuleBusinessType.CostDeduct, controllerName, actionName, "PayerOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.CostDeduct, controllerName, actionName, "EquipmentOrgId"), null);
                var queryUserOrgId = __workGroupModuleBLL.GetQueryManagerUserOrgIds(userIdValue, new ManagerUserOrgId(ModuleBusinessType.CostDeduct, controllerName, actionName, "PayerOrgId"));
                if (costDeductType != null)
                {
                    dataGridSettings.AppendAndQueryExpression(string.Format("CostDeductType=\"{0}\"", (int)costDeductType.Value));
                    if (costDeductType.Value != CostDeductType.ManualCostDeduct)
                    {
                        var queryManageEquipment = string.Format("EquipmentAdminId=\"{0}\"", userIdValue);
                        if (queryOrgId != "Id=null" && queryOrgId != "") queryManageEquipment = "(" + queryManageEquipment + "+" + queryOrgId + ")";
                        dataGridSettings.AppendAndQueryExpression(queryManageEquipment);
                    }
                    else if (queryUserOrgId != "")
                        dataGridSettings.AppendAndQueryExpression(queryUserOrgId);
                }
                else
                {
                    var queryCostDeduct = "";
                    var queryManualCostDeduct = "";
                    if (queryUserOrgId != "") queryManualCostDeduct = string.Format("(CostDeductType=\"{0}\"*{1})", (int)CostDeductType.ManualCostDeduct, queryUserOrgId);
                    else queryManualCostDeduct = string.Format("CostDeductType=\"{0}\"", (int)CostDeductType.ManualCostDeduct);
                    queryCostDeduct = string.Format("EquipmentAdminId=\"{0}\"", userIdValue);
                    if (queryOrgId != "Id=null" && queryOrgId != "") queryCostDeduct = "(" + queryCostDeduct + "+" + queryOrgId + ")";
                    queryCostDeduct = string.Format("(CostDeductType=-\"{0}\"*{1})", (int)CostDeductType.ManualCostDeduct, queryCostDeduct);
                    dataGridSettings.AppendAndQueryExpression(string.Format("({0}+{1}", queryManualCostDeduct, queryCostDeduct));
                }
            }
            else if (costDeductType != null)
                dataGridSettings.AppendAndQueryExpression(string.Format("CostDeductType=\"{0}\"", (int)costDeductType.Value));
            return true;
        }
    }
}
