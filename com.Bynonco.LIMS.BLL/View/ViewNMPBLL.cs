using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewNMPBLL : BLLBase<ViewNMP>, IViewNMPBLL
    {
        public void ExcuteOperateDecorate(Guid? userId, IList<ViewNMP> ViewNMPList, bool isSupressLazyLoad = false)
        {
            if (ViewNMPList == null || ViewNMPList.Count == 0) return;
            foreach (var ViewNMP in ViewNMPList)
            {
                ViewNMP.IsSupressLazyLoad = false;
                ViewNMP.InitOperate();
                OperateDecorate(userId, ViewNMP);
                ViewNMP.BuildOperate();
                ViewNMP.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }
        public void OperateDecorate(Guid? userId, ViewNMP ViewNMP)
        {
            if (ViewNMP == null) throw new ArgumentException("工位信息为空");
            NMPPrivilige nmpPrivilige =  null;
            if (userId.HasValue)
            {
                nmpPrivilige = PriviligeFactory.GetNMPPrivilige(userId.Value, ViewNMP.Id);
                ViewNMP.AppointmentWithoutLoginOperate = "";
            }
            else
            {
                ViewNMP.AppointmentOperate = "";
            }
            if (nmpPrivilige == null)
            {
                ViewNMP.ModifyOperate = "";
                return;
            }
            if (!nmpPrivilige.IsEnableEdit) ViewNMP.ModifyOperate = "";
            
        }
        public IList<ViewNMP> GetViewNMPListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var ViewNMPList = GetEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPList, isSupressLazyLoad);
            return ViewNMPList;
        }
        public IList<ViewNMP> GetViewNMPListByExpression(Guid? userId, string expression, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var ViewNMPList = GetEntitiesByExpression(expression, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPList, isSupressLazyLoad);
            return ViewNMPList;
        }
        public IList<ViewNMP> GetViewNMPListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var ViewNMPList = GetEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPList, isSupressLazyLoad);
            return ViewNMPList;
        }
        public GridData<ViewNMP> GetGridViewNMPListByExpression(Guid? userId, string expression, int pageindex, int pageSize, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var ViewNMPList = GetGridEntitiesByExpression(expression, pageindex, pageSize, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPList == null ? null : ViewNMPList.Data, isSupressLazyLoad);
            return ViewNMPList;
        }
        public GridData<ViewNMP> GetGridViewNMPListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isIniOperate = true)
        {
            var ViewNMPList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            if (isIniOperate) ExcuteOperateDecorate(userId, ViewNMPList == null ? null : ViewNMPList.Data, isSupressLazyLoad);
            return ViewNMPList;
        }
        public IList<ViewNMP> GetViewNMPListByEquipmentFilter(Guid? userId, DataGridSettings dataGridSettings, NMPFilter nmpFilter, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GetNMPFilterQueryExpression(nmpFilter, ref dataGridSettings);
            return GetViewNMPListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public GridData<ViewNMP> GetGridViewNMPListByEquipmentFilter(Guid? userId, DataGridSettings dataGridSettings, NMPFilter nmpFilter, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null, bool isManageList = true, bool isIniOperate = true)
        {
            GetNMPFilterQueryExpression(nmpFilter, ref dataGridSettings);
            return GetGridViewNMPListByExpression(userId, dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping, isIniOperate);
        }
        public void GetNMPFilterQueryExpression(NMPFilter nmpFilter, ref DataGridSettings dataGridSettings)
        {
            if (!string.IsNullOrEmpty(nmpFilter.Name)) dataGridSettings.AppendAndQueryExpression(string.Format("Name∽\"{0}\"", nmpFilter.Name.Trim()));
            if (!string.IsNullOrEmpty(nmpFilter.OrgXPath)) dataGridSettings.AppendAndQueryExpression(string.Format("OrgXPath→\"{0}\"", nmpFilter.OrgXPath.Trim()));
            if (!string.IsNullOrEmpty(nmpFilter.RoomXPath)) dataGridSettings.AppendAndQueryExpression(string.Format("RoomXPath→\"{0}\"", nmpFilter.RoomXPath.Trim()));
            if (!string.IsNullOrEmpty(nmpFilter.IP)) dataGridSettings.AppendAndQueryExpression(string.Format("IP∽\"{0}\"", nmpFilter.IP.Trim()));
            if (!string.IsNullOrWhiteSpace(nmpFilter.ContactPersonName)) dataGridSettings.AppendAndQueryExpression(string.Format("ContactPersonNameP∽\"{0}\"", nmpFilter.ContactPersonName.Trim()));
            if (!string.IsNullOrWhiteSpace(nmpFilter.ContactTelPhoneNo)) dataGridSettings.AppendAndQueryExpression(string.Format("ContactTelPhoneNo∽\"{0}\"", nmpFilter.ContactTelPhoneNo.Trim()));
            if (!string.IsNullOrWhiteSpace(nmpFilter.ContactEmail)) dataGridSettings.AppendAndQueryExpression(string.Format("ContactEmail∽\"{0}\"", nmpFilter.ContactEmail.Trim()));
           
        }
    }
}
