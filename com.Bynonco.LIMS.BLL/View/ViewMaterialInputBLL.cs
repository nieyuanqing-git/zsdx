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

namespace com.Bynonco.LIMS.BLL
{
    public class ViewMaterialInputBLL : BLLBase<ViewMaterialInput>, IViewMaterialInputBLL
    {
        private IUserBLL __userBLL;
        private IWorkGroupModuleBLL __workGroupModuleBLL;
        private IMaterialAdminBLL __materialAdminBLL;
        private IMaterialInputItemBLL __materialInputItemBLL;
        private IViewMaterialInfoBLL __viewMaterialInfoBLL;

        public ViewMaterialInputBLL()
        {
            __userBLL = BLLFactory.CreateUserBLL();
            __workGroupModuleBLL = BLLFactory.CreateWorkGroupModuleBLL();
            __materialAdminBLL = BLLFactory.CreateMaterialAdminBLL();
            __materialInputItemBLL = BLLFactory.CreateMaterialInputItemBLL();
            __viewMaterialInfoBLL = BLLFactory.CreateViewMaterialInfoBLL();
        }
        private void ExcuteOperateDecorate(Guid? userId, IList<ViewMaterialInput> viewMaterialInputList, bool isSupressLazyLoad = false)
        {
            if (viewMaterialInputList == null || viewMaterialInputList.Count == 0) return;
            foreach (var viewMaterialInput in viewMaterialInputList)
            {
                viewMaterialInput.IsSupressLazyLoad = false;
                viewMaterialInput.InitOperate();
                OperateDecorate(userId, viewMaterialInput);
                viewMaterialInput.BuildOperate();
                viewMaterialInput.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        private void OperateDecorate(Guid? userId, ViewMaterialInput viewMaterialInput)
        {
            if (viewMaterialInput == null) throw new ArgumentException("耗材进库单为空");
            var materialInputPrivilige = userId.HasValue ? PriviligeFactory.GetMaterialInputPrivilige(userId.Value, viewMaterialInput.Id) : null;
            if (materialInputPrivilige == null)
            {
                viewMaterialInput.ModifyOperate = "";
                viewMaterialInput.DeleteOperate = "";
                viewMaterialInput.NoteOperate = "";
                return;
            }
            if (!materialInputPrivilige.IsEnableEdit) viewMaterialInput.ModifyOperate = "";
            if (!materialInputPrivilige.IsEnableDelete) viewMaterialInput.DeleteOperate = "";
            if (!materialInputPrivilige.IsEnableNote) viewMaterialInput.NoteOperate = "";
        }
        public IList<ViewMaterialInput> GetViewMaterialInputListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInputList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInputList, isSupressLazyLoad);
            return viewMaterialInputList;
        }
        public IList<ViewMaterialInput> GetViewMaterialInputListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInputList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInputList, isSupressLazyLoad);
            return viewMaterialInputList;
        }
        public IList<ViewMaterialInput> GetViewMaterialInputListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInputList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInputList, isSupressLazyLoad);
            return viewMaterialInputList;
        }
        public GridData<ViewMaterialInput> GetGridViewMaterialInputListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInputList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInputList == null ? null : viewMaterialInputList.Data, isSupressLazyLoad);
            return viewMaterialInputList;
        }
        public GridData<ViewMaterialInput> GetGridViewMaterialInputListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var viewMaterialInputList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, viewMaterialInputList == null ? null : viewMaterialInputList.Data, isSupressLazyLoad);
            return viewMaterialInputList;
        }
      
        public IList<ViewMaterialInput> GetManageViewMaterialInputList(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            if (!IsManageMaterialInput(userId, ref dataGridSettings)) return null;
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            return GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
        }

        private bool IsManageMaterialInput(Guid? userId, ref DataGridSettings dataGridSettings)
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
                    var materialInputItemList = __materialInputItemBLL.GetEntitiesByExpression(string.Format("{0}*IsDelete=false", materialInfoIds.ToFormatStr("MaterialInfoId")), null, "", true);
                    if (materialInputItemList != null && materialInputItemList.Count() > 0)
                        queryExpression = materialInputItemList.Select(p => p.MaterialInputId).Distinct().ToFormatStr();
                    else return false;
                }
                dataGridSettings.AppendAndQueryExpression(queryExpression);
            }
            return true;
        }

        
    }
}
