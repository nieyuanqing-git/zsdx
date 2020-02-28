using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewMaterialOutputBLL : BLLBase<ViewMaterialOutput>, IViewMaterialOutputBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialOutputItemBLL __materialOutputItemBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;
        private IMaterialCategoryBLL _materialCategoryBLL;
        public ViewMaterialOutputBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialOutputItemBLL = BLLFactory.CreateMaterialOutputItemBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
            _materialCategoryBLL = BLLFactory.CreateMaterialCategoryBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialOutput> viewMaterialOutputList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialOutputList == null || viewMaterialOutputList.Count == 0) return;
            foreach (var viewMaterialOutput in viewMaterialOutputList)
            {
                viewMaterialOutput.IsSupressLazyLoad = false;
                viewMaterialOutput.InitOperate();
                OperateDecorate(userId, viewMaterialOutput);
                viewMaterialOutput.BuildOperate();
                viewMaterialOutput.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialOutput viewMaterialOutput)
        {
            if (viewMaterialOutput == null) throw new ArgumentException("耗材出库单为空");
            var materialOutputPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialOutputPrivilige(userId.Value, viewMaterialOutput.Id) : null;
            if (materialOutputPrivilige == null)
            {
                viewMaterialOutput.ModifyOperate = "";
                viewMaterialOutput.DeductOperate = "";
                viewMaterialOutput.DeleteOperate = "";
                viewMaterialOutput.CancelDeductOperate = "";
                viewMaterialOutput.NoteOperate = "";
                return;
            }
            if (!materialOutputPrivilige.IsEnableEdit) viewMaterialOutput.ModifyOperate = "";
            if (!materialOutputPrivilige.IsEnableDeduct) viewMaterialOutput.DeductOperate = "";
            if (!materialOutputPrivilige.IsEnableDelete) viewMaterialOutput.DeleteOperate = "";
            if (!materialOutputPrivilige.IsEnableCancelDeduct) viewMaterialOutput.CancelDeductOperate = "";
            if (!materialOutputPrivilige.IsEnableNote) viewMaterialOutput.NoteOperate = "";
            if (viewMaterialOutput.StatusEnum == MaterialOutputStatus.UnDeduct)
            {
                viewMaterialOutput.CancelDeductOperate = "";
            }
            if (viewMaterialOutput.StatusEnum == MaterialOutputStatus.Deduct)
            {
                viewMaterialOutput.DeductOperate = "";
                viewMaterialOutput.ModifyOperate = "";
                viewMaterialOutput.DeleteOperate = "";
            }
            if (viewMaterialOutput.StatusEnum == MaterialOutputStatus.ClosingAccount)
            {
                viewMaterialOutput.ModifyOperate = "";
                viewMaterialOutput.DeleteOperate = "";
                viewMaterialOutput.DeductOperate = "";
                viewMaterialOutput.CancelDeductOperate = "";
            }
        }
        public IList<ViewMaterialOutput> GetViewMaterialOutputListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialOutputList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialOutputList, isSupressLazyLoad);
            return viewMaterialOutputList;
        }
        public IList<ViewMaterialOutput> GetViewMaterialOutputListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialOutputList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialOutputList, isSupressLazyLoad);
            return viewMaterialOutputList;
        }
        public IList<ViewMaterialOutput> GetViewMaterialOutputListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialOutputList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialOutputList, isSupressLazyLoad);
            return viewMaterialOutputList;
        }
        public GridData<ViewMaterialOutput> GetGridViewMaterialOutputListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialOutputList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialOutputList == null ? null : viewMaterialOutputList.Data, isSupressLazyLoad);
            return viewMaterialOutputList;
        }
        public GridData<ViewMaterialOutput> GetGridViewMaterialOutputListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialOutputList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialOutputList == null ? null : viewMaterialOutputList.Data, isSupressLazyLoad);
            return viewMaterialOutputList;
        }

        public IList<ViewMaterialOutput> GetManageViewMaterialOutputList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialOutput(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        public int GetManageViewMaterialOutputCountByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialOutput(userId, ref dataGridSettings)) return 0;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return CountModelListByExpression(dataGridSettings.QueryExpression, mapping, isDistinct, outDistinctMapping);
        }

        public GridData<MaterialOutputStatData> GetGridViewMaterialOutputStatListByExpression(Guid? userId, DataGridSettings dataGridSettings, bool isDetail = false)
        {
            List<string> tableColumns;
            IDataParameter returnValueDataParameter;
            var results = GetMaterialOutputStatResults(dataGridSettings, isDetail, out tableColumns, out returnValueDataParameter);
            var datas = new List<MaterialOutputStatData>();
            MaterialOutputPrivilige privilige = null;
            if (userId.HasValue) privilige = PriviligeFactory.GetMaterialOutputPrivilige(userId.Value, Guid.Empty);
            foreach (var objectse in results)
            {
                var dict = new MaterialOutputStatData();
                for (int i = 0; i < objectse.Length; i++)
                {
                    dict.Add(tableColumns[i], objectse[i]);
                }
                if (!isDetail)
                {
                    if (userId.HasValue && privilige != null)
                    {
                        if (privilige.IsEnableDetailStatList)
                        {
                            dict.Add("Operate",
                                string.Format(
                                    "<a href='#' onclick=\"doMaterialOutputStatDetail('{0}');return false;\" title='查看明细信息' class='l-btn-color l-btn-color-green'>查看明细</a>",
                                    objectse[0]));

                        }
                        else
                        {
                            dict.Add("Operate", "");
                        }
                        if (privilige.IsEnableExportDetailStatList)
                        {
                            dict.Add("ExpOperate",
                                string.Format(
                                    "<a href='#' onclick=\"doExportMaterialOutputStatDetail('{0}');return false;\" title='生成明细列表' class='l-btn-color l-btn-color-blue'>导出明细</a>",
                                    objectse[0]));
                        }
                        else
                        {
                            dict.Add("ExpOperate", "");
                        }
                    }
                    else
                    {
                        dict.Add("Operate", "");
                        dict.Add("ExpOperate", "");
                    }
                }
                datas.Add(dict);
            }

            var gridData = new GridData<MaterialOutputStatData>();
            gridData.Data = datas;
            gridData.Count = Convert.ToInt32(returnValueDataParameter.Value);
            return gridData;
        }

        private IList<object[]> GetMaterialOutputStatResults(DataGridSettings dataGridSettings, bool isDetail, out List<string> tableColumns,
            out IDataParameter returnValueDataParameter)
        {
            var sql = string.Empty;
            var exp = dataGridSettings.QueryExpression;
            var fls = exp.Split('*');
            foreach (var fl in fls)
            {
                var ss = fl.Split('=');
                switch (ss[0])
                {
                    case "outusername":
                        sql += string.Format(" and us.UserName like '%{0}%' ", ss[1].Replace("\"", string.Empty));
                        break;
                    case "outusernameId":
                        sql += string.Format(" and us.UserId = '{0}' ", ss[1].Replace("\"", string.Empty));
                        break;
                    case "outputTimeStart":
                        sql += string.Format(" and m.OutputTime >= convert(datetime,'{0}') ", ss[1].Replace("\"", string.Empty));
                        break;
                    case "outputTimeEnd":
                        sql += string.Format(" and m.OutputTime < convert(datetime,'{0}') ",
                            DateTime.Parse(ss[1].Replace("\"", string.Empty)).AddDays(1).ToString("yyyy-MM-dd"));
                        break;
                }
            }


            var categories = _materialCategoryBLL.GetEntitiesByExpression(new DataGridSettings() { SortColumn = "Name" });
            tableColumns = new List<string>();
            tableColumns.Add("Id");
            tableColumns.Add("Name");
            foreach (var materialCategory in categories)
            {
                tableColumns.Add(materialCategory.Name);
            }
            tableColumns.Add("总金额");

            returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", null,
                ParameterDirection.ReturnValue);
            List<IDataParameter> dataParameter = new List<IDataParameter>()
            {
                DataParameterFactory.CreateDataParameter("queryExpression", sql, ParameterDirection.Input),
                DataParameterFactory.CreateDataParameter("pageIndex", dataGridSettings.PageIndex, ParameterDirection.Input),
                DataParameterFactory.CreateDataParameter("pageCount", dataGridSettings.PageSize, ParameterDirection.Input),
                returnValueDataParameter
            };
            var procedureName = isDetail ? "proGetMaterialOutputXsStat" : "proGetMaterialOutputStat";
            var results = (IList<object[]>)ProcedureAdapter.ExecuteProcedure(procedureName, dataParameter).Result;
            return results;
        }

        public IList<MaterialOutputStatData> GetViewMaterialOutputStatListByExpression(Guid? userId, DataGridSettings dataGridSettings, bool isDetail)
        {
            List<string> tableColumns;
            IDataParameter returnValueDataParameter;
            var results = GetMaterialOutputStatResults(dataGridSettings, isDetail, out tableColumns, out returnValueDataParameter); var datas = new List<MaterialOutputStatData>();
            foreach (var objectse in results)
            {
                var dict = new MaterialOutputStatData();
                for (int i = 0; i < objectse.Length; i++)
                {
                    dict.Add(tableColumns[i], objectse[i]);
                }
                datas.Add(dict);
            }
            return datas;
        }

        private bool IsManageMaterialOutput(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = "";
                var materialInfoIds = __viewMaterialInfoBLL.GetManageMaterialInfoIds(userId, new DataGridSettings() { QueryExpression = "" });
                if (materialInfoIds != null && materialInfoIds.Count() > 0)
                {
                    var materialOutputItemList = __materialOutputItemBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", materialInfoIds.ToFormatStr("MaterialInfoId")), null, "", true);
                    if (materialOutputItemList != null && materialOutputItemList.Count() > 0)
                        queryExpression = materialOutputItemList.Select(p => p.MaterialOutputId).Distinct().ToFormatStr();
                    else return false;
                }
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }


    }
}
