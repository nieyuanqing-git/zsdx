using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Utility;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class WorkGroupModuleBLL : BLLBase<WorkGroupModule>, IWorkGroupModuleBLL
    {
        private IUserBLL __userBLL;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        private IUserSystemSettingBLL __userSystemSettingBLL;
        private IEquipmentBLL __equipmentBLL;
        private ILabOrganizationAuthorizedBLL __labOrganizationAuthorizedBLL;
        private INMPBLL __nmpBLL;
        private INMPAdminBLL __nmpAdminBLL;
        public WorkGroupModuleBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
            __userSystemSettingBLL = BLLFactory.CreateUserSystemSettingBLL();
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __labOrganizationAuthorizedBLL = BLLFactory.CreateLabOrganizationAuthorizedBLL();
            __nmpBLL = BLLFactory.CreateNMPBLL();
            __nmpAdminBLL = BLLFactory.CreateNMPAdminBLL();
        }
        public string GetQueryManagerUserOrgIds(Guid userId, ManagerUserOrgId managerUserOrgId)
        {
            return GetQueryManagerOrgIds(userId, managerUserOrgId, null,null);
        }
        public string GetQueryManagerEquipmentOrgIds(Guid userId, ManagerEquipmentOrgId managerEquipmentOrgId)
        {
            return GetQueryManagerOrgIds(userId, null, managerEquipmentOrgId, null);
        }
        public string GetQueryManagerNMPOrgIds(Guid userId, ManagerNMPOrgId managerNMPOrgId)
        {
            return GetQueryManagerOrgIds(userId, null,null, managerNMPOrgId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="managerUserOrgId"></param>
        /// <param name="managerEquipmentOrgId"></param>
        /// <returns>"Id=null"无任何权限; "Id=-null"拥有所有权限</returns>
        public string GetQueryManagerOrgIds(Guid userId, ManagerUserOrgId managerUserOrgId, ManagerEquipmentOrgId managerEquipmentOrgId, ManagerNMPOrgId managerNMPOrgId)
        {
            var viewUserWorkGroupModuleFunctionBLL = BLLFactory.CreateViewUserWorkGroupModuleFunctionBLL();
            var user = __userBLL.GetEntityById(userId);
            if (user == null && managerUserOrgId == null && managerEquipmentOrgId == null) return "Id=null";

            string controllerName = "";
            string actionName = "";
            IEnumerable<Guid> distinctUserOrgIds = null;
            IEnumerable<Guid> distinctEquipmentOrgIds = null;
            IEnumerable<Guid> distinctNMPOrgIds = null;
            if(managerUserOrgId != null)
            {
                controllerName = managerUserOrgId.ControllerName;
                actionName =managerUserOrgId.ActionName;
                distinctUserOrgIds = GetUserOrgIdByBusinessType(managerUserOrgId.ModuleBusinessType, ref controllerName, ref actionName);
                managerUserOrgId.ControllerName = controllerName.Trim() != "" ? controllerName : "User";
                managerUserOrgId.ActionName = actionName.Trim() != "" ? actionName : "Index";
            }
            if (managerEquipmentOrgId != null)
            {
                controllerName = managerEquipmentOrgId.ControllerName;
                actionName = managerEquipmentOrgId.ActionName;
                distinctEquipmentOrgIds = GetEquipmentOrgIdByBusinessType(managerEquipmentOrgId.ModuleBusinessType, ref controllerName, ref actionName);
                managerEquipmentOrgId.ControllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                managerEquipmentOrgId.ActionName = actionName.Trim() != "" ? actionName : "Index";
            }
            if (managerNMPOrgId != null)
            {
                controllerName = managerNMPOrgId.ControllerName;
                actionName = managerNMPOrgId.ActionName;
                distinctNMPOrgIds = GetNMPOrgIdByBusinessType(managerNMPOrgId.ModuleBusinessType, ref controllerName, ref actionName);
                managerNMPOrgId.ControllerName = controllerName.Trim() != "" ? controllerName : "NMP";
                managerNMPOrgId.ActionName = actionName.Trim() != "" ? actionName : "Index";
            }
            var userSystemSettingList = __userSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            IEnumerable<Guid> workGroupIds = null;
            if (userSystemSettingList != null && userSystemSettingList.Count() > 0 && userSystemSettingList.First().WorkGroupId.HasValue)
                workGroupIds = new Guid[] { userSystemSettingList.First().WorkGroupId.Value };
            else if (user.WorkGroups != null && user.WorkGroups.Where(p => p.IsDelete == false) != null && user.WorkGroups.Where(p => p.IsDelete == false).Count() > 0)
                workGroupIds = user.WorkGroups.Where(p => p.IsDelete == false).Select(p => p.Id);
            if (workGroupIds == null || workGroupIds.Count() == 0) return "Id=null";
            var viewModuleFunctionList = viewUserWorkGroupModuleFunctionBLL.GetEntitiesByExpression(string.Format("{0}*ControllerName=\"{1}\"*ActionName=\"{2}\"*IsDelete=false*IsStop=false", workGroupIds.ToFormatStr("WorkGroupId"), controllerName, actionName), null, "", true);
            if (viewModuleFunctionList == null || viewModuleFunctionList.Count() == 0) return "Id=null";
            string queryExpressionUserOrgId = "";
            string queryExpressionEquipmentOrgId = "";
            string queryExpressionNMPOrgId = "";
            bool isAllowRuleLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowRuleLimit);
            bool isAllowUserOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowUserOrgLimit);
            bool isAllowEquipmentOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowEquipmentOrgLimit);
            bool isAllowNMPOrgLimit = viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsAllowNMPOrgLimit);
            bool isNoRulePass = isAllowRuleLimit
                   && viewModuleFunctionList.Any(p => p.IsNoRuleLimit.HasValue && p.IsNoRuleLimit.Value
                           && (!p.IsUserOrgLimit.HasValue || !p.IsUserOrgLimit.Value)
                           && (!p.IsEquipmentOrgLimit.HasValue || !p.IsEquipmentOrgLimit.Value)
                           && (!p.IsNMPOrgLimit.HasValue || !p.IsNMPOrgLimit.Value));
            bool isUserOrgLimit = managerUserOrgId != null && isAllowUserOrgLimit
                && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsUserOrgLimit.HasValue && p.IsUserOrgLimit.Value);
            bool isEquipmentOrgLimit = managerEquipmentOrgId != null && isAllowEquipmentOrgLimit
                && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsEquipmentOrgLimit.HasValue && p.IsEquipmentOrgLimit.Value);


            bool isNMPOrgLimit = managerNMPOrgId != null && isAllowNMPOrgLimit
               && viewModuleFunctionList.Any(p => p.ActionName == actionName && p.IsNMPOrgLimit.HasValue && p.IsNMPOrgLimit.Value);
            
            
            if (isNoRulePass) return "Id=-null";
            if (isUserOrgLimit)
            {
                var labOrganizationIdList = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId, managerUserOrgId.LabOrganizationAuthorizedType);
                if (labOrganizationIdList != null && labOrganizationIdList.Count() > 0)
                {
                    if (managerUserOrgId.ModuleBusinessType == ModuleBusinessType.LabOrganization)
                        queryExpressionUserOrgId = labOrganizationIdList.ToFormatStr(managerUserOrgId.IdName, managerUserOrgId.LogicRelation);
                    else if (distinctUserOrgIds != null && distinctUserOrgIds.Count() > 0)
                        queryExpressionUserOrgId = labOrganizationIdList.Where(p => distinctUserOrgIds.Contains(p)).ToFormatStr(managerUserOrgId.IdName, managerUserOrgId.LogicRelation);
                }
                if(queryExpressionUserOrgId == "" ) queryExpressionUserOrgId = "Id=null";
            }
            if (isEquipmentOrgLimit)
            {
                var labOrganizationIdList = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId, managerEquipmentOrgId.LabOrganizationAuthorizedType);
                if (labOrganizationIdList != null && labOrganizationIdList.Count() > 0)
                {
                    if (managerEquipmentOrgId.ModuleBusinessType == ModuleBusinessType.LabOrganization)
                        queryExpressionEquipmentOrgId = labOrganizationIdList.ToFormatStr(managerEquipmentOrgId.IdName, managerEquipmentOrgId.LogicRelation);
                    else if (distinctEquipmentOrgIds != null && distinctEquipmentOrgIds.Count() > 0)
                        queryExpressionEquipmentOrgId = labOrganizationIdList.Where(p => distinctEquipmentOrgIds.Contains(p)).ToFormatStr(managerEquipmentOrgId.IdName, managerEquipmentOrgId.LogicRelation);
                }
                if (queryExpressionEquipmentOrgId == "") queryExpressionEquipmentOrgId = "Id=null";
            }

            if (isNMPOrgLimit)
            {
                var labOrganizationIdList = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId, managerNMPOrgId.LabOrganizationAuthorizedType);
                if (labOrganizationIdList != null && labOrganizationIdList.Count() > 0)
                {
                    if (managerNMPOrgId.ModuleBusinessType == ModuleBusinessType.LabOrganization)
                        queryExpressionNMPOrgId = labOrganizationIdList.ToFormatStr(managerNMPOrgId.IdName, managerNMPOrgId.LogicRelation);
                    else if (distinctNMPOrgIds != null && distinctNMPOrgIds.Count() > 0)
                        queryExpressionNMPOrgId = labOrganizationIdList.Where(p => distinctNMPOrgIds.Contains(p)).ToFormatStr(managerNMPOrgId.IdName, managerNMPOrgId.LogicRelation);
                }
                if (queryExpressionNMPOrgId == "") queryExpressionNMPOrgId = "Id=null";
            }
            if ((managerUserOrgId == null || (isAllowUserOrgLimit && !isUserOrgLimit)) && (managerEquipmentOrgId == null || (isAllowEquipmentOrgLimit && !isEquipmentOrgLimit)) && (managerNMPOrgId == null || (isAllowNMPOrgLimit && !isNMPOrgLimit))) return "Id=null";
            string queryExprsession = "";
            if (queryExpressionUserOrgId != "" && queryExpressionEquipmentOrgId != "" && queryExpressionNMPOrgId != "") queryExprsession = "(" + queryExpressionUserOrgId + "+" + queryExpressionEquipmentOrgId + "+" + queryExpressionNMPOrgId + ")";
            else
            {
                queryExprsession = queryExpressionUserOrgId;
                if (!string.IsNullOrWhiteSpace(queryExpressionEquipmentOrgId))
                {
                    queryExprsession += string.IsNullOrWhiteSpace(queryExprsession) ? queryExpressionEquipmentOrgId : "+" + queryExpressionEquipmentOrgId;
                }
                if (!string.IsNullOrWhiteSpace(queryExpressionNMPOrgId))
                    queryExprsession += string.IsNullOrWhiteSpace(queryExprsession) ? queryExpressionNMPOrgId : "+" + queryExpressionNMPOrgId;
            }
            return queryExprsession;
        }
        public IEnumerable<Guid> GetManagerEquipmentIds(Guid userId, ModuleBusinessType moduleBusinessType, string controllerName = "", string actionName = "")
        {
            var viewUserWorkGroupModuleFunctionBLL = BLLFactory.CreateViewUserWorkGroupModuleFunctionBLL();
            IEnumerable<Guid> allEquipmentIdList = null;
            IEnumerable<Guid> powerEquipmentIdList = null;
            IEnumerable<Guid> userWorkScopeEquipmentIdList = null;
            IEnumerable<Guid> distinctIds = null;
            if (moduleBusinessType != ModuleBusinessType.Equipment) distinctIds = GetEquipmentIdsByBusinessType(moduleBusinessType, ref controllerName, ref actionName);
            controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
            actionName = actionName.Trim() != "" ? actionName : "Edit";
            var user = __userBLL.GetEntityById(userId);
            if (user == null) return null;
            var userSystemSettingList = __userSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            IEnumerable<Guid> workGroupIds = null;
            if (userSystemSettingList != null && userSystemSettingList.Count() > 0 && userSystemSettingList.First().WorkGroupId.HasValue)
                workGroupIds = new Guid[] { userSystemSettingList.First().WorkGroupId.Value };
            else if (user.WorkGroups != null && user.WorkGroups.Count() > 0)
                workGroupIds = user.WorkGroups.Select(p => p.Id);
            if (workGroupIds != null && workGroupIds.Count() > 0)
            {
                var viewModuleFunctionList = viewUserWorkGroupModuleFunctionBLL.GetEntitiesByExpression(string.Format("{0}*ControllerName=\"{1}\"*ActionName=\"{2}\"*IsDelete=false*IsStop=false", workGroupIds.ToFormatStr("WorkGroupId"), controllerName, actionName), null, "", true);
                if (viewModuleFunctionList != null && viewModuleFunctionList.Count != 0)
                {
                    string queryExpression = "IsDelete=false";
                    if (viewModuleFunctionList.Any(p => p.IsNoRuleLimit.HasValue && p.IsNoRuleLimit.Value && (!p.IsEquipmentOrgLimit.HasValue || !p.IsEquipmentOrgLimit.Value)))
                        powerEquipmentIdList = __equipmentBLL.GetEntitiesByExpression(queryExpression, null, "", true).Select(p => p.Id);
                    else if (viewModuleFunctionList.Any(p => p.IsEquipmentOrgLimit.HasValue && p.IsEquipmentOrgLimit.Value))
                    {
                        var labOrganizationIdList = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId,LabOrganizationAuthorizedType.Equipment);
                        if (labOrganizationIdList != null && labOrganizationIdList.Count() > 0)
                        {
                            queryExpression += "*" + labOrganizationIdList.ToFormatStr("OrgId");
                            powerEquipmentIdList = __equipmentBLL.GetEntitiesByExpression(queryExpression, null, "", true).Select(p => p.Id);
                        }
                    }
                    if (powerEquipmentIdList != null && powerEquipmentIdList.Count() > 0)
                        allEquipmentIdList = powerEquipmentIdList;
                }
            }
            var userWorkScopeList =__userWorkScopeBLL.GetUserWorkScopeListByUserId(userId);
            if (userWorkScopeList == null || userWorkScopeList.Count == 0) return allEquipmentIdList;
            userWorkScopeEquipmentIdList = userWorkScopeList.Select(p => p.EquipmentId);
            allEquipmentIdList = allEquipmentIdList == null ? userWorkScopeEquipmentIdList : allEquipmentIdList.Union(userWorkScopeEquipmentIdList).Distinct();
            if (allEquipmentIdList != null && allEquipmentIdList.Count() > 0)
            {
                if (moduleBusinessType != ModuleBusinessType.Equipment &&  distinctIds != null && distinctIds.Count() > 0)
                    allEquipmentIdList = allEquipmentIdList.Where(p => distinctIds.Contains(p));
            }
            return allEquipmentIdList;
        }
        public IEnumerable<Guid> GetManagerNMPIds(Guid userId, ModuleBusinessType moduleBusinessType, string controllerName = "", string actionName = "")
        {
            var viewUserWorkGroupModuleFunctionBLL = BLLFactory.CreateViewUserWorkGroupModuleFunctionBLL();
            IEnumerable<Guid> allNMPIdList = null;
            IEnumerable<Guid> powerNMPIdList = null;
            IEnumerable<Guid> userAdminNMPIdList = null;
            IEnumerable<Guid> distinctIds = null;
            if (moduleBusinessType != ModuleBusinessType.NMP) distinctIds = GetNMPIdsByBusinessType(moduleBusinessType, ref controllerName, ref actionName);
            controllerName = controllerName.Trim() != "" ? controllerName : "NMP";
            actionName = actionName.Trim() != "" ? actionName : "Edit";
            var user = __userBLL.GetEntityById(userId);
            if (user == null) return null;
            var userSystemSettingList = __userSystemSettingBLL.GetEntitiesByExpression(string.Format("UserId=\"{0}\"", userId.ToString()));
            IEnumerable<Guid> workGroupIds = null;
            if (userSystemSettingList != null && userSystemSettingList.Count() > 0 && userSystemSettingList.First().WorkGroupId.HasValue)
                workGroupIds = new Guid[] { userSystemSettingList.First().WorkGroupId.Value };
            else if (user.WorkGroups != null && user.WorkGroups.Count() > 0)
                workGroupIds = user.WorkGroups.Select(p => p.Id);
            if (workGroupIds != null && workGroupIds.Count() > 0)
            {
                var viewModuleFunctionList = viewUserWorkGroupModuleFunctionBLL.GetEntitiesByExpression(string.Format("{0}*ControllerName=\"{1}\"*ActionName=\"{2}\"*IsDelete=false*IsStop=false", workGroupIds.ToFormatStr("WorkGroupId"), controllerName, actionName), null, "", true);
                if (viewModuleFunctionList != null && viewModuleFunctionList.Count != 0)
                {
                    string queryExpression = "IsDelete=false";
                    if (viewModuleFunctionList.Any(p => p.IsNoRuleLimit.HasValue && p.IsNoRuleLimit.Value && (!p.IsNMPOrgLimit.HasValue || !p.IsNMPOrgLimit.Value)))
                        powerNMPIdList = __nmpBLL.GetEntitiesByExpression(queryExpression, null, "", true).Select(p => p.Id);
                    else if (viewModuleFunctionList.Any(p => p.IsNMPOrgLimit.HasValue && p.IsNMPOrgLimit.Value))
                    {
                        var labOrganizationIdList = __labOrganizationAuthorizedBLL.GetAuthorizedLabOrganizationIds(userId, LabOrganizationAuthorizedType.NMP);
                        if (labOrganizationIdList != null && labOrganizationIdList.Count() > 0)
                        {
                            queryExpression += "*" + labOrganizationIdList.ToFormatStr("OrgId");
                            powerNMPIdList = __nmpBLL.GetEntitiesByExpression(queryExpression, null, "", true).Select(p => p.Id);
                        }
                    }
                    if (powerNMPIdList != null && powerNMPIdList.Count() > 0)
                        allNMPIdList = powerNMPIdList;
                }
            }
            var nmpAdminList = __nmpAdminBLL.GetNMPAdminListByAdminId(userId);
            if (nmpAdminList == null || nmpAdminList.Count == 0) return allNMPIdList;
            userAdminNMPIdList = nmpAdminList.Select(p => p.NMPId);
            allNMPIdList = allNMPIdList == null ? userAdminNMPIdList : allNMPIdList.Union(userAdminNMPIdList).Distinct();
            if (allNMPIdList != null && allNMPIdList.Count() > 0)
            {
                if (moduleBusinessType != ModuleBusinessType.NMP && distinctIds != null && distinctIds.Count() > 0)
                    allNMPIdList = allNMPIdList.Where(p => distinctIds.Contains(p));
            }
            return allNMPIdList;
        }
        private IEnumerable<Guid> GetUserOrgIdByBusinessType(ModuleBusinessType moduleBusinessType, ref string controllerName, ref string actionName)
        {
            object obj = null;
            switch (moduleBusinessType)
            {
                case ModuleBusinessType.User:
                    controllerName = controllerName.Trim() != "" ? controllerName : "User";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrganizationId from [User] where OrganizationId is not null");
                    break;
                case ModuleBusinessType.LabOrganization:
                    controllerName = controllerName.Trim() != "" ? controllerName : "LabOrganization";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    break;
                case ModuleBusinessType.Subject:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Subject";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrgId from [ViewSubject] where OrgId is not null");
                    break;
                case ModuleBusinessType.EquipmentTrainning:
                    controllerName = controllerName.Trim() != "" ? controllerName : "EquipmentTrainning";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct CreatorOrgId from [ViewEquipmentTrainning] where CreatorOrgId is not null");
                    break;
                case ModuleBusinessType.UserEquipment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserEquipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewUserEquipment] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.Appointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Appointment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewAppointment] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.UsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewUsedConfirm] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.CostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct PayerOrgId from [ViewCostDeduct] where PayerOrgId is not null");
                    break;
                case ModuleBusinessType.BalanceAccount:
                    controllerName = controllerName.Trim() != "" ? controllerName : "BalanceAccount";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct CreateOrgId from [ViewBalanceAccount] where CreateOrgId is not null");
                    break;
                case ModuleBusinessType.DepositRecord:
                    controllerName = controllerName.Trim() != "" ? controllerName : "DepositRecord";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewDepositRecord] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.ArticleCategory:
                    controllerName = controllerName.Trim() != "" ? controllerName : "ArticleCategory";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrgId from [ArticleCategory] where OrgId is not null");
                    break;   
                case ModuleBusinessType.Question:
                    controllerName = controllerName.Trim() != "" ? controllerName : "QA";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrgId from [Question] where OrgId is not null");
                    break;
                case ModuleBusinessType.DelinquencyConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Delinquency";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct PunisherOrgId from [ViewDelinquencyConfirm] where PunisherOrgId is not null");
                    break;
                case ModuleBusinessType.PunishAction:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Delinquency";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct PunisherOrgId from [ViewPunishAction] where PunisherOrgId is not null");
                    break;
                case ModuleBusinessType.DoorAccessRecord:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Door";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewDoorAccessRecord] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.PowerOperation:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewPowerOperation] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.UserEquipmentExamination:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserExamination";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct TrainningTypeOrgId from [ViewUserEquipmentExamination] where TrainningTypeOrgId is not null");
                    break;
                case ModuleBusinessType.UserLabOrganizationExamination:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserExamination";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct TrainningTypeOrgId from [ViewUserLabOrganizationExamination] where TrainningTypeOrgId is not null");
                    break;
                case ModuleBusinessType.SubjectAchievement:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewSubjectAchievement] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.Thesis:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewThesis] where UserOrgId is not null");
                    break;    
                case ModuleBusinessType.Publication:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewPublication] where UserOrgId is not null");
                    break;         
                case ModuleBusinessType.Patent:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewPatent] where UserOrgId is not null");
                    break;        
                case ModuleBusinessType.Awards:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewAwards] where UserOrgId is not null");
                    break;         
                case ModuleBusinessType.AcademicExchanges:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewAcademicExchanges] where UserOrgId is not null");
                    break;            
                case ModuleBusinessType.AchievementStudent:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewAchievementStudent] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.AcademicPosition:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Achievement";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewAcademicPosition] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.SampleStatistics:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "Statistics";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewSampleStatistics] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.SampleApplyQuery:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "GetSampleApplyListInfo";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewSampleApply] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.SampleApply:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleManage";
                    actionName = actionName.Trim() != "" ? actionName : "SampleApplyAuditIndex";
                     obj = GetMutiResult("select distinct UserOrgId from [ViewSampleApply] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.DepositRecordOpenFund:
                    controllerName = controllerName.Trim() != "" ? controllerName : "DepositRecord";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewDepositRecordOpenFund] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.NMP:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMP";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrganizationId from [NMP] where OrganizationId is not null");
                    break;
                case ModuleBusinessType.NMPAppointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPAppointment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewNMPAppointment] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.NMPUsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPUsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "UnDeductNMPUsedConfirmListContainer";
                    obj = GetMutiResult("select distinct UserOrgId from [ViewNMPUsedConfirm] where UserOrgId is not null");
                    break;
                case ModuleBusinessType.NMPCostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "CostDeduct";
                    actionName = actionName.Trim() != "" ? actionName : "NMPCostDeductListContainer";
                    obj = GetMutiResult("select distinct PayerOrgId from [ViewCostDeduct] where PayerOrgId is not null");
                    break;
               
            }
            if (obj != null)
            {
                IList<Guid> ids = new List<Guid>();
                foreach (var id in (System.Collections.ArrayList)obj)
                    ids.Add(new Guid(id.ToString()));
                return ids.AsEnumerable();
            }
            return null;
        }
        private IEnumerable<Guid> GetNMPOrgIdByBusinessType(ModuleBusinessType moduleBusinessType, ref string controllerName, ref string actionName)
        {
            object obj = null;
            switch (moduleBusinessType)
            {
                case ModuleBusinessType.NMP:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMP";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrganizationId from [NMP] where OrganizationId is not null");
                    break;
                case ModuleBusinessType.NMPAppointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPAppointment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct NMPOrgId from [ViewNMPAppointment] where NMPOrgId is not null");
                    break;
                case ModuleBusinessType.NMPUsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPUsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "UnDeductNMPUsedConfirmListContainer";
                    obj = GetMutiResult("select distinct NMPOrgId from [ViewNMPUsedConfirm] where NMPOrgId is not null");
                    break;
                case ModuleBusinessType.NMPCostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "CostDeduct";
                    actionName = actionName.Trim() != "" ? actionName : "NMPCostDeductListContainer";
                    obj = GetMutiResult("select distinct PayerOrgId from [ViewCostDeduct] where PayerOrgId is not null");
                    break;
               
            }
            if (obj != null)
            {
                IList<Guid> ids = new List<Guid>();
                foreach (var id in (System.Collections.ArrayList)obj)
                    ids.Add(new Guid(id.ToString()));
                return ids.AsEnumerable();
            }
            return null;
        }
        private IEnumerable<Guid> GetEquipmentOrgIdByBusinessType(ModuleBusinessType moduleBusinessType, ref string controllerName, ref string actionName)
        {
            object obj = null;
            switch (moduleBusinessType)
            {
                case ModuleBusinessType.Equipment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct OrgId from [Equipment] where OrgId is not null");
                    break;
                case ModuleBusinessType.UserEquipment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserEquipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewUserEquipment] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.Appointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Appointment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewAppointment] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.UsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewUsedConfirm] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.CostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewCostDeduct] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.EquipmentTrainning:
                    controllerName = controllerName.Trim() != "" ? controllerName : "EquipmentTrainning";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewEquipmentTrainning] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.UserEquipmentExamination:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserExamination";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct TrainningTypeOrgId from [ViewUserEquipmentExamination] where TrainningTypeOrgId is not null");
                    break;
                case ModuleBusinessType.UserLabOrganizationExamination:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserExamination";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct TrainningTypeOrgId from [ViewUserLabOrganizationExamination] where TrainningTypeOrgId is not null");
                    break;
                case ModuleBusinessType.Door:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Door";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct LabOrganizationID from [Door] where LabOrganizationID is not null");
                    break;
                case ModuleBusinessType.DoorAccessRecord:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Door";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct DoorOrgId from [ViewDoorAccessRecord] where DoorOrgId is not null");
                    break;
                case ModuleBusinessType.PowerOperation:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewPowerOperation] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.Material:
                    controllerName = controllerName.Trim() != "" ? controllerName : "System";
                    actionName = actionName.Trim() != "" ? actionName : "MaterialSettingIndex";
                    obj = GetMutiResult("select distinct OrganizationId from [ViewMaterialInfo] where OrganizationId is not null");
                    break;
                case ModuleBusinessType.JudgeEquipmentRecord:
                    controllerName = controllerName.Trim() != "" ? controllerName : "JudgeEquipmentRecord";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewJudgeEquipmentRecord] where EquipmentOrgId is not null");
                    break;
                
                case ModuleBusinessType.SampleStatistics:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "Statistics";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSampleStatistics] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SampleApplyQuery:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "GetSampleApplyListInfo";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSampleApply] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SampleItem:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleItem";
                    actionName = actionName.Trim() != "" ? actionName : "GetGridViewSampleItemList";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSampleItem] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.OpenFundApplyEquipment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "OpenFundApply";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewOpenFundApplyEquipment] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.EquipmentAlarm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewEquipmentAlarm] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SJ1:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSJ1] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SJ3:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSJ3] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.UserEquipmentPrivilige:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserEquipmentPrivilige";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewUserEquipmentPrivilige] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SampleItemLabel:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleItemLabel";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSampleItemLabel] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.EquipmentSemesterCost:
                    controllerName = controllerName.Trim() != "" ? controllerName : "EquipmentSemesterCost";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewEquipmentSemesterCost] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.EquipmentBrokenReport:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Equipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewEquipmentBrokenReport] where EquipmentOrgId is not null");
                    break;
                case ModuleBusinessType.SampleApply:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleManage";
                    actionName = actionName.Trim() != "" ? actionName : "SampleApplyAuditIndex";
                    obj = GetMutiResult("select distinct EquipmentOrgId from [ViewSampleApply] where EquipmentOrgId is not null");
                    break;
            }
            if (obj != null)
            {
                IList<Guid> ids = new List<Guid>();
                foreach (var id in (System.Collections.ArrayList)obj)
                    ids.Add(new Guid(id.ToString()));
                return ids.AsEnumerable();
            }
            return null;
        }
        private IEnumerable<Guid> GetEquipmentIdsByBusinessType(ModuleBusinessType moduleBusinessType, ref string controllerName, ref string actionName)
        {
            object obj = null;
            switch (moduleBusinessType)
            {
                case ModuleBusinessType.EquipmentTrainning:
                    controllerName = controllerName.Trim() != "" ? controllerName : "EquipmentTrainning";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewEquipmentTrainning]");
                    break;
                case ModuleBusinessType.UserEquipment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserEquipment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentId from [UserEquipment]");
                    break;
                case ModuleBusinessType.Appointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "Appointment";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentId from [Appointment]");
                    break;
                case ModuleBusinessType.UsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentId from [UsedConfirm]");
                    break;
                case ModuleBusinessType.CostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewCostDeduct] where EquipmentId is not null");
                    break;
                case ModuleBusinessType.UserEquipmentExamination:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserExamination";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentId from [UserExamination] where EquipmentId is not null");
                    break;
                case ModuleBusinessType.SampleStatistics:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "Statistics";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewSampleStatistics]");
                    break;
                case ModuleBusinessType.SampleApplyQuery:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleStatistics";
                    actionName = actionName.Trim() != "" ? actionName : "GetSampleApplyListInfo";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewSampleApply]");
                    break;
                case ModuleBusinessType.SampleItem:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleItem";
                    actionName = actionName.Trim() != "" ? actionName : "GetGridViewSampleItemList";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewSampleItem]");
                    break;
                case ModuleBusinessType.UserEquipmentPrivilige:
                    controllerName = controllerName.Trim() != "" ? controllerName : "UserEquipmentPrivilige";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewUserEquipmentPrivilige]");
                    break;
                case ModuleBusinessType.SampleItemLabel:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleItemLabel";
                    actionName = actionName.Trim() != "" ? actionName : "Manage";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewSampleItemLabel]");
                    break;
                case ModuleBusinessType.SampleApply:
                    controllerName = controllerName.Trim() != "" ? controllerName : "SampleManage";
                    actionName = actionName.Trim() != "" ? actionName : "SampleApplyAuditIndex";
                    obj = GetMutiResult("select distinct EquipmentId from [ViewSampleApply]");
                    break;
            }
            if (obj != null)
            {
                IList<Guid> ids = new List<Guid>();
                var idArray = (System.Collections.ArrayList)obj;
                foreach (var id in idArray)
                    ids.Add(new Guid(id.ToString()));
                return ids.AsEnumerable();
            }
            return null;
        }
        private IEnumerable<Guid> GetNMPIdsByBusinessType(ModuleBusinessType moduleBusinessType, ref string controllerName, ref string actionName)
        {
            object obj = null;
            switch (moduleBusinessType)
            {
                case ModuleBusinessType.NMP:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMP";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct NMPId from [NMP]");
                    break;
                case ModuleBusinessType.NMPAppointment:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPAppointment";
                    actionName = actionName.Trim() != "" ? actionName : "Index";
                    obj = GetMutiResult("select distinct NMPId from [ViewNMPAppointment] where NMPId is not null");
                    break;
                case ModuleBusinessType.NMPUsedConfirm:
                    controllerName = controllerName.Trim() != "" ? controllerName : "NMPUsedConfirm";
                    actionName = actionName.Trim() != "" ? actionName : "UnDeductNMPUsedConfirmListContainer";
                    obj = GetMutiResult("select distinct NMPId from [ViewNMPUsedConfirm] where NMPId is not null");
                    break;
                case ModuleBusinessType.NMPCostDeduct:
                    controllerName = controllerName.Trim() != "" ? controllerName : "CostDeduct";
                    actionName = actionName.Trim() != "" ? actionName : "NMPCostDeductListContainer";
                    obj = GetMutiResult("select distinct NMPId from [ViewCostDeduct] where NMPId is not null");
                    break;
            }
            if (obj != null)
            {
                IList<Guid> ids = new List<Guid>();
                var idArray = (System.Collections.ArrayList)obj;
                foreach (var id in idArray)
                    ids.Add(new Guid(id.ToString()));
                return ids.AsEnumerable();
            }
            return null;
        }
    }
}