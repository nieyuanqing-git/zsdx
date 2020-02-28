using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.DAL;
using System.Data;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewCurrentUsingEquipmentBLL : BLLBase<ViewCurrentUsingEquipment>, IViewCurrentUsingEquipmentBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        public ViewCurrentUsingEquipmentBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewCurrentUsingEquipment> viewCurrentUsingEquipmentList, bool isSupressLazyLoad)
        {
            if (viewCurrentUsingEquipmentList == null || viewCurrentUsingEquipmentList.Count == 0) return;
            foreach (var viewCurrentUsingEquipment in viewCurrentUsingEquipmentList)
            {
                UsedConfirmPrivilige usedConfirmPrivilige = !userId.HasValue ? null : PriviligeFactory.GetUsedConfirmPrivilige(userId.Value, viewCurrentUsingEquipment.Id);
                viewCurrentUsingEquipment.IsSupressLazyLoad = false;
                viewCurrentUsingEquipment.InitOperate();
                OperateDecorate(userId, viewCurrentUsingEquipment, usedConfirmPrivilige);
                viewCurrentUsingEquipment.BuildOperate();
                viewCurrentUsingEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        private void OperateDecorate(Guid? userId, ViewCurrentUsingEquipment viewCurrentUsingEquipment, UsedConfirmPrivilige usedConfirmPrivilige)
        {
            if (viewCurrentUsingEquipment == null) throw new ArgumentException("设备信息为空");
            if (!viewCurrentUsingEquipment.SampleApplyId.HasValue)//当前正在使用的设备
            {
                if (usedConfirmPrivilige == null)
                {
                    viewCurrentUsingEquipment.ViewDetailOperate = "";
                    viewCurrentUsingEquipment.EditOperate = "";
                    viewCurrentUsingEquipment.DeleteOperate = "";
                    return;
                }
                if (!usedConfirmPrivilige.IsEnableViewUsedConfirmDetail)
                {
                    viewCurrentUsingEquipment.ViewDetailOperate = "";
                }
                if (!usedConfirmPrivilige.IsEnableEditUsedConfirm || viewCurrentUsingEquipment.SampleApplyId.HasValue || viewCurrentUsingEquipment.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    viewCurrentUsingEquipment.EditOperate = "";
                }
                if (!usedConfirmPrivilige.IsEnableDeleteUsedConfirm || viewCurrentUsingEquipment.SampleApplyId.HasValue || viewCurrentUsingEquipment.Status == (int)UsedConfirmStatus.Deduct || viewCurrentUsingEquipment.Status == (int)UsedConfirmStatus.ClosingAccount)
                {
                    viewCurrentUsingEquipment.DeleteOperate = "";
                }
            }
            else//当前正在测样的设备
            {

            }
        }
        public IList<ViewCurrentUsingEquipment> GetViewCurrentUsingEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageCurrentUsingEquipment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewCurrentUsingEquipmentList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewCurrentUsingEquipmentList, isSupressLazyLoad);
            return viewCurrentUsingEquipmentList;
        }
        public IList<ViewCurrentUsingEquipment> GetViewCurrentUsingEquipmentListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (!IsManageCurrentUsingEquipment(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var viewCurrentUsingEquipmentList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteOperateDecorate(userId, viewCurrentUsingEquipmentList, isSupressLazyLoad);
            return viewCurrentUsingEquipmentList;
        }
        public IList<ViewCurrentUsingEquipment> GetViewCurrentUsingEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            dataGridSettings.PageSize = 9999999;
            var gridData = GetGridViewCurrentUsingEquipmentListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (gridData == null || gridData.Data == null || gridData.Data.Count == 0) return null;
            return gridData.Data;
        }
        public GridData<ViewCurrentUsingEquipment> GetGridViewCurrentUsingEquipmentListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = GenerateDataGridSetting(expression, orderExpression, pageindex, pageSize, mapping);
            if (!IsManageCurrentUsingEquipment(userId, ref dataGridSettings)) return null;
            GridData<ViewCurrentUsingEquipment> gridData = new GridData<ViewCurrentUsingEquipment>();
            var sqlStr = ConverExpressionToSql(dataGridSettings.QueryExpression, null);
            var queryExpressionDataParam = DataParameterFactory.CreateDataParameter("queryExpression", sqlStr);
            var orderClomnNameDataParam = DataParameterFactory.CreateDataParameter("orderClomnName", dataGridSettings.SortColumn);
            var orderTypeDataParam = DataParameterFactory.CreateDataParameter("orderType", dataGridSettings.GetSortOrderStr());
            var pageIndexDataParam = DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex);
            var pageCountDataParam = DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize);
            var returnValueDataParam = DataParameterFactory.CreateDataParameter("returnValue", null, ParameterDirection.ReturnValue);
            List<IDataParameter> dataParams = new List<IDataParameter>()
             {
                 queryExpressionDataParam,
                 orderClomnNameDataParam,
                 orderTypeDataParam,
                 pageIndexDataParam,
                 pageCountDataParam,
                 returnValueDataParam
             };
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure("ProGetCurUsingEquipmentList", dataParams).Result;
            if (results != null && results.Count > 0)
            {
                IList<ViewCurrentUsingEquipment> viewCurrentUsingEquipments = new List<ViewCurrentUsingEquipment>();
                foreach (var result in results)
                {
                    ViewCurrentUsingEquipment viewCurrentUsingEquipment = new ViewCurrentUsingEquipment();
                    viewCurrentUsingEquipment.Id = new Guid(result[0].ToString());
                    viewCurrentUsingEquipment.UserId = new Guid(result[1].ToString());
                    viewCurrentUsingEquipment.UserName = result[2] != null ? result[2].ToString() : null;
                    viewCurrentUsingEquipment.PhoneNumber = result[3] != null ? result[3].ToString() : null;
                    viewCurrentUsingEquipment.UserOrgId = null;
                    if (result[4] != null && !string.IsNullOrWhiteSpace(result[4].ToString())) viewCurrentUsingEquipment.UserOrgId = new Guid(result[4].ToString());
                    viewCurrentUsingEquipment.TutorId = null;
                    if (result[5] != null && !string.IsNullOrWhiteSpace(result[5].ToString())) viewCurrentUsingEquipment.TutorId = new Guid(result[5].ToString());
                    viewCurrentUsingEquipment.TutorName = result[6] != null ? result[6].ToString() : null;
                    viewCurrentUsingEquipment.BeginAt = null;
                    if (result[7] != null && !string.IsNullOrWhiteSpace(result[7].ToString())) viewCurrentUsingEquipment.BeginAt = DateTime.Parse(result[7].ToString());
                    viewCurrentUsingEquipment.EndAt = null;
                    if (result[8] != null && !string.IsNullOrWhiteSpace(result[8].ToString())) viewCurrentUsingEquipment.EndAt = DateTime.Parse(result[8].ToString());
                    viewCurrentUsingEquipment.SubjectId = null;
                    if (result[9] != null && !string.IsNullOrWhiteSpace(result[9].ToString())) viewCurrentUsingEquipment.SubjectId = new Guid(result[9].ToString());
                    viewCurrentUsingEquipment.SubjectName = result[10] != null ? result[10].ToString() : null;
                    viewCurrentUsingEquipment.EquipmentId = viewCurrentUsingEquipment.EquipmentId = new Guid(result[11].ToString());
                    viewCurrentUsingEquipment.EquipmentName = result[12] != null ? result[12].ToString() : null;
                    viewCurrentUsingEquipment.RoomId = null;
                    if (result[13] != null && !string.IsNullOrWhiteSpace(result[13].ToString())) viewCurrentUsingEquipment.RoomId = new Guid(result[13].ToString());
                    viewCurrentUsingEquipment.EquipmentOrgId = null;
                    if (result[14] != null && !string.IsNullOrWhiteSpace(result[14].ToString())) viewCurrentUsingEquipment.EquipmentOrgId = new Guid(result[14].ToString());
                    viewCurrentUsingEquipment.RelativePic = result[15] != null ? result[15].ToString() : null;
                    viewCurrentUsingEquipment.LabRoomName = result[16] != null ? result[16].ToString() : null;
                    viewCurrentUsingEquipment.LabRoomXPath = result[17] != null ? result[17].ToString() : null;
                    viewCurrentUsingEquipment.CalcDuration = null;
                    if (result[18] != null && !string.IsNullOrWhiteSpace(result[18].ToString())) viewCurrentUsingEquipment.CalcDuration =double.Parse(result[18].ToString());
                    viewCurrentUsingEquipment.CreateAt = null;
                    if (result[19] != null && !string.IsNullOrWhiteSpace(result[19].ToString())) viewCurrentUsingEquipment.CreateAt = DateTime.Parse(result[19].ToString());
                    viewCurrentUsingEquipment.CreaterId = null;
                    if (result[20] != null && !string.IsNullOrWhiteSpace(result[20].ToString())) viewCurrentUsingEquipment.CreaterId = new Guid(result[20].ToString());
                    viewCurrentUsingEquipment.CreaterName = result[21] != null ? result[21].ToString() : null;
                    viewCurrentUsingEquipment.Status = int.Parse(result[22].ToString());
                    viewCurrentUsingEquipment.AppointmentId = null;
                    if (result[23] != null && !string.IsNullOrWhiteSpace(result[23].ToString())) viewCurrentUsingEquipment.AppointmentId = new Guid(result[23].ToString());
                    viewCurrentUsingEquipment.SampleApplyId = null;
                    if (result[24] != null && !string.IsNullOrWhiteSpace(result[24].ToString())) viewCurrentUsingEquipment.SampleApplyId = new Guid(result[24].ToString());
                    viewCurrentUsingEquipment.CurrentUsingType = int.Parse(result[25].ToString());
                    viewCurrentUsingEquipments.Add(viewCurrentUsingEquipment);
                }
                gridData.Data = viewCurrentUsingEquipments;
            }
            gridData.Count = returnValueDataParam.Value == null ? 0 : int.Parse(returnValueDataParam.Value.ToString());
            ExcuteOperateDecorate(userId, gridData == null ? null : gridData.Data, isSupressLazyLoad);
            return gridData;
        }
        public GridData<ViewCurrentUsingEquipment> GetGridViewCurrentUsingEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            return GetGridViewCurrentUsingEquipmentListByExpression(userId, dataGridSettings.QueryExpression, dataGridSettings.PageIndex, dataGridSettings.PageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }
        public int GetViewCurrentUsingEquipmentCountByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            return GetViewCurrentUsingEquipmentCountByExpression(userId,dataGridSettings, mapping, isDistinct, outDistinctMapping);
        }
        public int GetViewCurrentUsingEquipmentCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageCurrentUsingEquipment(userId, ref dataGridSettings)) return 0;
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }
        private bool IsManageCurrentUsingEquipment(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, new ManagerUserOrgId(ModuleBusinessType.UsedConfirm, "", "", "UserOrgId"), new ManagerEquipmentOrgId(ModuleBusinessType.UsedConfirm, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
    }
}
