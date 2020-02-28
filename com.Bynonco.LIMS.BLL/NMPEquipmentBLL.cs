using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class NMPEquipmentBLL : BLLBase<NMPEquipment>, INMPEquipmentBLL
    {
        public IList<NMPEquipment> GetNMPEquipmentListByLabels(string[] labels)
        {
            if (labels == null || labels.Length == 0) return null;
            string queryExpression = "";
            foreach (var label in labels)
            {
                if (string.IsNullOrWhiteSpace(label)) continue;
                queryExpression += string.IsNullOrWhiteSpace(queryExpression) ? string.Format("Label=\"{0}\"", label.Trim()) : string.Format("+Label=\"{0}\"", label.Trim());
            }
            if (queryExpression == "") return null;
            return GetEntitiesByExpression(queryExpression);
        }
        public GridData<NMPEquipment> GetGridAuthorizationNMPEquipmentListByExpression(Guid? userId, DataGridSettings dataGridSettings, Dictionary<string, string> mapping = null, string orderExpression = "", bool isSupressLazyLoad = false, bool isLoadReference = false, bool isDistinct = false, string[] outDistinctMapping = null)
        {
            dataGridSettings.AppendAndQueryExpression("IsDelete=false");
            var NMPEquipmentList = GetGridEntitiesByExpression(dataGridSettings, mapping, orderExpression, isSupressLazyLoad, isLoadReference, isDistinct, outDistinctMapping);
            ExcuteAuthorizationOperateDecorate(userId, NMPEquipmentList == null ? null : NMPEquipmentList.Data, isSupressLazyLoad);
            return NMPEquipmentList;
        }

        public void ExcuteAuthorizationOperateDecorate(Guid? userId, IList<NMPEquipment> NMPEquipmentList, bool isSupressLazyLoad = false)
        {
            if (NMPEquipmentList == null || NMPEquipmentList.Count == 0) return;
            foreach (var nmpEquipment in NMPEquipmentList)
            {
                nmpEquipment.IsSupressLazyLoad = false;
                nmpEquipment.InitAuthorizationOperate();
                AuthorizationOperateDecorate(userId, nmpEquipment);
                nmpEquipment.BuildAuthorizationOperate();
                nmpEquipment.IsSupressLazyLoad = isSupressLazyLoad;
            }
        }

        public void AuthorizationOperateDecorate(Guid? userId, NMPEquipment nmpEquipment)
        {
            if (nmpEquipment == null) throw new ArgumentException("工位设备信息为空");
            var nmpPrivilige = userId.HasValue ? PriviligeFactory.GetNMPPrivilige(userId.Value, nmpEquipment.Id) : null;

            if (nmpPrivilige == null)
            {
                nmpEquipment.AuthorizationOperate = "";
                return;
            }
            if (!nmpPrivilige.IsEnableNMPEquipmentAuthorization) nmpEquipment.AuthorizationOperate = "";
        }
    }
}
