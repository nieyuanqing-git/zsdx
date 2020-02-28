using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using System.Data;
using com.Bynonco.LIMS.DAL;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewJudgeEquipmentRecordBLL : BLLBase<ViewJudgeEquipmentRecord>, IViewJudgeEquipmentRecordBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IViewJudgeProjectBLL __viewJudgeProjectBLL;
        private IViewJudgeProjectContentBLL __viewJudgeProjectContentBLL;
        private IViewJudgeProjectRecordBLL __viewJudgeProjectRecordBLL;
        private IViewJudgeProjectContentRecordBLL __viewJudgeProjectContentRecordBLL;
        
        public ViewJudgeEquipmentRecordBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __viewJudgeProjectBLL = BLLFactory.CreateViewJudgeProjectBLL();
            __viewJudgeProjectContentBLL = BLLFactory.CreateViewJudgeProjectContentBLL();
            __viewJudgeProjectRecordBLL = BLLFactory.CreateViewJudgeProjectRecordBLL();
            __viewJudgeProjectContentRecordBLL = BLLFactory.CreateViewJudgeProjectContentRecordBLL();
        }
        private void ExcuteOperateDecorateStatistics(Guid? userId, IList<JudgeEquipmentRecordStatistics> judgeEquipmentRecordStatisticsList)
        {
            if (judgeEquipmentRecordStatisticsList == null || judgeEquipmentRecordStatisticsList.Count == 0) return;
            IList<JudgeEquipmentRecordPrivilige> lstJudgeEquipmentRecordPrivilige = new List<JudgeEquipmentRecordPrivilige>();
            if (userId.HasValue)
            {
                foreach (var item in judgeEquipmentRecordStatisticsList)
                {
                    if(!item.JudgeEquipmentRecordId.HasValue) continue;
                    JudgeEquipmentRecordPrivilige judgeEquipmentRecordPrivilige = lstJudgeEquipmentRecordPrivilige.FirstOrDefault(p => p.JudgeEquipmentRecordId.HasValue && p.JudgeEquipmentRecordId.Value == item.JudgeEquipmentRecordId.Value);
                    if (judgeEquipmentRecordPrivilige == null)
                    {
                        judgeEquipmentRecordPrivilige = PriviligeFactory.GetJudgeEquipmentRecordPrivilige(userId.Value, item.JudgeEquipmentRecordId.Value);
                        lstJudgeEquipmentRecordPrivilige.Add(judgeEquipmentRecordPrivilige);
                    }
                }
            }

            foreach (var item in judgeEquipmentRecordStatisticsList)
            {
                var itemPrivilige = lstJudgeEquipmentRecordPrivilige.FirstOrDefault(p => p.JudgeEquipmentRecordId.HasValue && p.JudgeEquipmentRecordId == item.JudgeEquipmentRecordId);
                item.InitOperate();
                OperateDecorateStatistics(userId, item, itemPrivilige);
                item.BuildOperate();
            }
        }
        private void OperateDecorateStatistics(Guid? userId, JudgeEquipmentRecordStatistics judgeEquipmentRecordStatistics, JudgeEquipmentRecordPrivilige judgeEquipmentRecordPrivilige)
        {
            if (judgeEquipmentRecordStatistics == null) throw new ArgumentException("设备考核评价为空");
            if (judgeEquipmentRecordPrivilige == null)
            {
                judgeEquipmentRecordStatistics.ModifyOperate = "";
                judgeEquipmentRecordStatistics.DeleteOperate = "";
                judgeEquipmentRecordStatistics.ViewOperate = "";
                return;
            }
            if (!judgeEquipmentRecordPrivilige.IsEnableEdit) judgeEquipmentRecordStatistics.ModifyOperate = "";
            if (!judgeEquipmentRecordPrivilige.IsEnableDelete) judgeEquipmentRecordStatistics.DeleteOperate = "";
            if (!judgeEquipmentRecordPrivilige.IsEnableViewRecord) judgeEquipmentRecordStatistics.ViewOperate = "";
            
        }
        public IList<JudgeEquipmentRecordStatistics> GetJudgeEquipmentRecordStatisticsByExpression(Guid? userId, string expression, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageJudgeEquipmentRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            int returnValue;
            var judgeEquipmentRecordStatisticsList = GetJudgeEquipmentRecordStatistics(expression, out returnValue);
            if (isIniOperate) ExcuteOperateDecorateStatistics(userId, judgeEquipmentRecordStatisticsList);
            return judgeEquipmentRecordStatisticsList;
        }
        public IList<JudgeEquipmentRecordStatistics> GetJudgeEquipmentRecordStatisticsListByExpression(Guid? userId, DataGridSettings dataGridSettings, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageJudgeEquipmentRecord(userId, ref dataGridSettings)) return null;
            int returnValue;
            var judgeEquipmentRecordStatisticsList = GetJudgeEquipmentRecordStatistics(dataGridSettings.QueryExpression, out returnValue);
            if (isIniOperate) ExcuteOperateDecorateStatistics(userId, judgeEquipmentRecordStatisticsList);
            return judgeEquipmentRecordStatisticsList;
        }
        public GridData<JudgeEquipmentRecordStatistics> GetGridJudgeEquipmentRecordStatisticsListByExpression(Guid? userId, string expression, int pageindex, int pageSize, bool isManageList = true, bool isIniOperate = true)
        {
            DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = expression };
            if (isManageList && !IsManageJudgeEquipmentRecord(userId, ref dataGridSettings)) return null;
            expression = dataGridSettings.QueryExpression;
            var judgeEquipmentRecordStatisticsList = GetGridJudgeEquipmentRecordStatistics(expression, pageindex, pageSize);
            if(isIniOperate && judgeEquipmentRecordStatisticsList != null && judgeEquipmentRecordStatisticsList.Data != null)
                ExcuteOperateDecorateStatistics(userId, judgeEquipmentRecordStatisticsList.Data);
            return judgeEquipmentRecordStatisticsList;
        }
        public GridData<JudgeEquipmentRecordStatistics> GetGridJudgeEquipmentRecordStatisticsListByExpression(Guid? userId, DataGridSettings dataGridSettings, bool isManageList = true, bool isIniOperate = true)
        {
            if (isManageList && !IsManageJudgeEquipmentRecord(userId, ref dataGridSettings)) return null;
            var judgeEquipmentRecordStatisticsList = GetGridJudgeEquipmentRecordStatistics(dataGridSettings.QueryExpression, dataGridSettings.PageIndex, dataGridSettings.PageSize);
            if (isIniOperate && judgeEquipmentRecordStatisticsList != null && judgeEquipmentRecordStatisticsList.Data != null)
                ExcuteOperateDecorateStatistics(userId, judgeEquipmentRecordStatisticsList.Data);
            return judgeEquipmentRecordStatisticsList;
        }
        private bool IsManageJudgeEquipmentRecord(Guid? userId, ref DataGridSettings dataGridSettings)
        {
            if (!userId.HasValue) return false;
            var user = __userBLL.GetEntityById(userId.Value);
            if (user == null) return false;
            if (!user.IsSuperAdmin)
            {
                var queryExpression = string.Format("EquipmentAdminId=\"{0}\"", user.Id);
                var queryOrgId = __workGroupModuleBLL.GetQueryManagerOrgIds(user.Id, null, new ManagerEquipmentOrgId(ModuleBusinessType.JudgeEquipmentRecord, "", "", "EquipmentOrgId"), null);
                if (queryOrgId != "Id=null" && queryOrgId != "") queryExpression = "(" + queryExpression + "+" + queryOrgId + ")";
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }
        private GridData<JudgeEquipmentRecordStatistics> GetGridJudgeEquipmentRecordStatistics(string queryExpression, int pageIndex = 0, int pageCount = 10)
        {
            int returnValue;
            var judgeEquipmentRecordStatisticsList = GetJudgeEquipmentRecordStatistics(queryExpression,out returnValue, pageIndex,pageCount);
            return new GridData<JudgeEquipmentRecordStatistics>()
            {
                Data = judgeEquipmentRecordStatisticsList,
                Count = returnValue
            };
        }
        private IList<JudgeEquipmentRecordStatistics> GetJudgeEquipmentRecordStatistics(string queryExpression, out int returnValue, int pageIndex = 0, int pageCount = 10)
        {
            returnValue = 0;
            var judgeEquipmentRecordStatisticsList = new List<JudgeEquipmentRecordStatistics>();
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            mapping["Id"] = "JudgeEquipmentRecordId";
            queryExpression = !string.IsNullOrWhiteSpace(queryExpression) && queryExpression.IndexOf("+Id=-null") != -1 ? queryExpression.Replace("+Id=-null", "+JudgeEquipmentRecordId=-null") : queryExpression;
            var sqlStr = ConverExpressionToSql(queryExpression, mapping);
            sqlStr = sqlStr.Replace("EquipmentAdminId", "UserWorkScope.UserId");
            sqlStr = sqlStr.Replace("EquipmentId", "JudgeEquipmentRecord.EquipmentId");
            sqlStr = sqlStr.Replace("EquipmentName", "Equipment.Name");
            sqlStr = sqlStr.Replace("EquipmentModel", "Equipment.Model");
            sqlStr = sqlStr.Replace("EquipmentLabel", "Equipment.Label");
            sqlStr = sqlStr.Replace("EquipmentOrgXPath", "LabOrganization.XPath");
            var dataParams = new List<IDataParameter>();
            dataParams.Add(DataParameterFactory.CreateDataParameter("queryExpression", sqlStr));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageIndex", pageIndex));
            dataParams.Add(DataParameterFactory.CreateDataParameter("pageCount", pageCount));
            IDataParameter returnValueDataParameter = DataParameterFactory.CreateDataParameter("returnValue", returnValue, ParameterDirection.ReturnValue);
            dataParams.Add(returnValueDataParameter);
            var execResult = ProcedureAdapter.ExecuteProcedure("ProGetJudgeEquipmentRecordStatistics", dataParams);
            var results = (IList<object[]>)execResult.Result;
            var columnNames = (object[])execResult.ColumnName;
            if (results != null && results.Count > 0 && columnNames != null && columnNames.Length > 0)
            {
                var viewJudgeProjectList = __viewJudgeProjectBLL.GetEntitiesByExpression("IsDelete=false", null, "XPath");
                foreach (var result in results)
                {
                    var recordNum = result[0] == DBNull.Value ? "" : result[0].ToString();
                    Guid? equipmentId = null;
                    if (result[1] != DBNull.Value) equipmentId = new Guid(result[1].ToString());
                    var equipmentName = result[2] == DBNull.Value ? "" : result[2].ToString();
                    var equipmentLabel = result[3] == DBNull.Value ? "" : result[3].ToString();
                    var equipmentModel = result[4] == DBNull.Value ? "" : result[4].ToString();
                    var equipmentRelativePic = result[5] == DBNull.Value ? "" : result[5].ToString();
                    Guid? equipmentOrgId = null;
                    if (result[6] != DBNull.Value) equipmentOrgId = new Guid(result[6].ToString());
                    var equipmentOrgName = result[7] == DBNull.Value ? "" : result[7].ToString();
                    var equipmentOrgXPath = result[8] == DBNull.Value ? "" : result[8].ToString();
                    DateTime? judgeTime = null;
                    if(result[9] != DBNull.Value) judgeTime = DateTime.Parse(result[9].ToString());
                    Guid? createrId = null;
                    if (result[10] != DBNull.Value) createrId = new Guid(result[10].ToString());
                    var createrName = result[11] == DBNull.Value ? "" : result[11].ToString();
                    DateTime? createTime = null;
                    if (result[12] != DBNull.Value) createTime = DateTime.Parse(result[12].ToString());
                    Guid? updateUserId = null;
                    if (result[13] != DBNull.Value) updateUserId = new Guid(result[13].ToString());
                    var updateName = result[14] == DBNull.Value ? "" : result[14].ToString();
                    DateTime? updateTime = null;
                    if (result[15] != DBNull.Value) updateTime = DateTime.Parse(result[15].ToString());
                    Guid? judgeEquipmentRecordId = null;
                    if (result[16] != DBNull.Value) judgeEquipmentRecordId = new Guid(result[16].ToString());
                    
                    JudgeEquipmentRecordStatistics judgeEquipmentRecordStatistics = new JudgeEquipmentRecordStatistics()
                    {
                        RecordNum = recordNum,
                        EquipmentId = equipmentId,
                        EquipmentName = equipmentName,
                        EquipmentLabel = equipmentLabel,
                        EquipmentModel = equipmentModel,
                        EquipmentRelativePic = equipmentRelativePic,
                        EquipmentOrgId = equipmentOrgId,
                        EquipmentOrgName = equipmentOrgName,
                        EquipmentOrgXPath = equipmentOrgXPath,
                        JudgeTime = judgeTime,
                        CreaterId = createrId,
                        CreaterName = createrName,
                        CreateTime = createTime,
                        UpdateUserId = updateUserId,
                        UpdateName = updateName,
                        UpdateTime = updateTime,
                        JudgeEquipmentRecordId = judgeEquipmentRecordId
                    };
                    if(viewJudgeProjectList != null && viewJudgeProjectList.Count() > 0)
                    {
                        judgeEquipmentRecordStatistics.JudgeProjectRecordStatisticsList = new List<JudgeProjectRecordStatistics>();
                        foreach (var item in viewJudgeProjectList)
                        {
                            var judgeProjectRecordStatistics = new JudgeProjectRecordStatistics();
                            judgeProjectRecordStatistics.JudgeProjectId = item.JudgeProjectId;
                            judgeProjectRecordStatistics.JudgeProjectName = item.Name;
                            judgeProjectRecordStatistics.JudgeProjectWeight = item.Weight;
                            judgeProjectRecordStatistics.JudgeProjectProjectCent = item.ProjectCent;
                            judgeProjectRecordStatistics.XPath = item.XPath;
                            if (result.Length > 18 && columnNames.Length == result.Length)
                            {
                                for (int i = 17; i < result.Length; i++)
                                {
                                    var colValue = columnNames[i] == null ? "" : columnNames[i].ToString();
                                    if (colValue.IsGuid() && new Guid(colValue) == item.JudgeProjectId)
                                    {
                                        int? itemCent = null;
                                        if (result[i] != DBNull.Value && result[i].ToString().IsInt()) itemCent = int.Parse(result[i].ToString());
                                        judgeProjectRecordStatistics.ItemCent = itemCent;
                                    }
                                }
                            }
                            judgeEquipmentRecordStatistics.JudgeProjectRecordStatisticsList.Add(judgeProjectRecordStatistics);
                        }
                    }
                    judgeEquipmentRecordStatisticsList.Add(judgeEquipmentRecordStatistics);
                }
                returnValue = Convert.ToInt32(returnValueDataParameter.Value);
            }
            return judgeEquipmentRecordStatisticsList;
        }

        public JudgeEquipmentRecordStatistics GetJudgeEquipmentRecordStatisticsInfo(Guid? id)
        {
            var judgeEquipmentRecordStatistics = new JudgeEquipmentRecordStatistics();
            var viewJudgeProjectList = __viewJudgeProjectBLL.GetEntitiesByExpression("IsDelete=false", null, "XPath");
            var viewJudgeProjectContentList = __viewJudgeProjectContentBLL.GetEntitiesByExpression("IsDelete=false", null, "XPath");
            IList<ViewJudgeProjectRecord> viewJudgeProjectRecordList = null;
            if (id.HasValue)
            {
                var viewJudgeEquipmentRecord = GetEntityById(id.Value);
                if (viewJudgeEquipmentRecord != null)
                {
                    judgeEquipmentRecordStatistics.JudgeEquipmentRecordId = viewJudgeEquipmentRecord.Id;
                    judgeEquipmentRecordStatistics.RecordNum = viewJudgeEquipmentRecord.RecordNum;
                    judgeEquipmentRecordStatistics.EquipmentId = viewJudgeEquipmentRecord.EquipmentId;
                    judgeEquipmentRecordStatistics.EquipmentName = viewJudgeEquipmentRecord.EquipmentName;
                    judgeEquipmentRecordStatistics.EquipmentLabel = viewJudgeEquipmentRecord.EquipmentLabel;
                    judgeEquipmentRecordStatistics.EquipmentModel = viewJudgeEquipmentRecord.EquipmentModel;
                    judgeEquipmentRecordStatistics.EquipmentRelativePic = viewJudgeEquipmentRecord.EquipmentRelativePic;
                    judgeEquipmentRecordStatistics.EquipmentOrgId = viewJudgeEquipmentRecord.EquipmentOrgId;
                    judgeEquipmentRecordStatistics.EquipmentOrgName = viewJudgeEquipmentRecord.EquipmentOrgName;
                    judgeEquipmentRecordStatistics.EquipmentOrgXPath = viewJudgeEquipmentRecord.EquipmentOrgXPath;
                    judgeEquipmentRecordStatistics.JudgeTime = viewJudgeEquipmentRecord.JudgeTime;
                    judgeEquipmentRecordStatistics.CreaterId = viewJudgeEquipmentRecord.CreaterId;
                    judgeEquipmentRecordStatistics.CreaterName = viewJudgeEquipmentRecord.CreaterName;
                    judgeEquipmentRecordStatistics.CreateTime = viewJudgeEquipmentRecord.CreateTime;
                    judgeEquipmentRecordStatistics.UpdateUserId = viewJudgeEquipmentRecord.UpdateUserId;
                    judgeEquipmentRecordStatistics.UpdateName = viewJudgeEquipmentRecord.UpdateName;
                    judgeEquipmentRecordStatistics.UpdateTime = viewJudgeEquipmentRecord.UpdateTime;
                    viewJudgeProjectRecordList = __viewJudgeProjectRecordBLL.GetEntitiesByExpression(string.Format("JudgeEquipmentRecordId=\"{0}\"*IsDelete=false", viewJudgeEquipmentRecord.Id), null, "XPath");
                }
            }
            if (viewJudgeProjectList != null && viewJudgeProjectList.Count() > 0)
            {
                judgeEquipmentRecordStatistics.JudgeProjectRecordStatisticsList = new List<JudgeProjectRecordStatistics>();
                foreach (var item in viewJudgeProjectList)
                {
                    var judgeProjectRecordStatistics = new JudgeProjectRecordStatistics();
                    judgeProjectRecordStatistics.JudgeProjectId = item.JudgeProjectId;
                    judgeProjectRecordStatistics.JudgeProjectName = item.Name;
                    judgeProjectRecordStatistics.JudgeProjectWeight = item.Weight;
                    judgeProjectRecordStatistics.JudgeProjectProjectCent = item.ProjectCent;
                    judgeProjectRecordStatistics.XPath = item.XPath;
                    ViewJudgeProjectRecord viewJudgeProjectRecord = null;
                    if (viewJudgeProjectRecordList != null && viewJudgeProjectRecordList.Count() > 0)
                        viewJudgeProjectRecord = viewJudgeProjectRecordList.Where(p => p.JudgeProjectId == item.JudgeProjectId).FirstOrDefault();
                    if (viewJudgeProjectRecord != null)
                    {
                        judgeProjectRecordStatistics.JudgeProjectRecordId = viewJudgeProjectRecord.Id;
                        judgeProjectRecordStatistics.ItemWeightCent = viewJudgeProjectRecord.ItemWeightCent;
                        judgeProjectRecordStatistics.ItemCent = viewJudgeProjectRecord.ItemCent;
                    }
                    if (viewJudgeProjectContentList != null && viewJudgeProjectContentList.Count() > 0)
                    {
                        var itemContentList = viewJudgeProjectContentList.Where(p => p.JudgeProjectId == item.Id).OrderBy(p => p.XPath).ToList();
                        if (itemContentList != null && itemContentList.Count() > 0)
                        {
                            judgeProjectRecordStatistics.JudgeProjectContentRecordStatisticsList = new List<JudgeProjectContentRecordStatistics>();
                            foreach (var contentItem in itemContentList)
                            {
                                ViewJudgeProjectContentRecord viewJudgeProjectContentRecord = null;
                                if (viewJudgeProjectRecord != null && viewJudgeProjectRecord.ViewJudgeProjectContentRecordList != null && viewJudgeProjectRecord.ViewJudgeProjectContentRecordList.Count() > 0)
                                    viewJudgeProjectContentRecord = viewJudgeProjectRecord.ViewJudgeProjectContentRecordList.Where(p => p.JudgeProjectContentId == contentItem.Id).FirstOrDefault();

                                var judgeProjectContentRecordStatistics = new JudgeProjectContentRecordStatistics();
                                judgeProjectContentRecordStatistics.JudgeProjectContentId = contentItem.Id;
                                judgeProjectContentRecordStatistics.ContentName = contentItem.Name;
                                judgeProjectContentRecordStatistics.JudgeStandard = contentItem.JudgeStandard;
                                judgeProjectContentRecordStatistics.XPath = contentItem.XPath;
                                if(viewJudgeProjectContentRecord != null)
                                {
                                    judgeProjectContentRecordStatistics.JudgeProjectContentRecordId = viewJudgeProjectContentRecord.Id;
                                    judgeProjectContentRecordStatistics.ContentCent = viewJudgeProjectContentRecord.ContentCent;
                                    judgeProjectContentRecordStatistics.Remark = viewJudgeProjectContentRecord.Remark;
                                }
                                judgeProjectRecordStatistics.JudgeProjectContentRecordStatisticsList.Add(judgeProjectContentRecordStatistics);
                            }
                        }
                    }
                    judgeEquipmentRecordStatistics.JudgeProjectRecordStatisticsList.Add(judgeProjectRecordStatistics);
                }
            }
            return judgeEquipmentRecordStatistics;
        }
    }
}
