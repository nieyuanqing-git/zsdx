using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.JqueryEasyUI.Core;
using System.Collections;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewEquipmentSampleItemQueryBLL : BLLBase<ViewEquipmentSampleItemQuery>, IViewEquipmentSampleItemQueryBLL
    {
        private IEquipmentBLL __equipmentBLL;
        private ISampleStatusBLL __sampleStatusBLL;
        public ViewEquipmentSampleItemQueryBLL()
        {
            __equipmentBLL = BLLFactory.CreateEquipmentBLL();
            __sampleStatusBLL = BLLFactory.CreateSampleStatusBLL();
        }
        public IList<Equipment> GetEquipmentSampleItems(DataGridSettings dataGridSetting,Guid? queryEquipmentId)
        {
            var whereStr = " ((DefaultAcceptSampleTest= 1 and  UseDefaultIsAcceptSampleTest=1 ) or ((UseDefaultIsAcceptSampleTest=0 or UseDefaultIsAcceptSampleTest is null) and IsAcceptSampleTest=1))";
            if (queryEquipmentId.HasValue) whereStr += string.Format(" and EquipmentId='{0}' ", queryEquipmentId.Value);
            if (!string.IsNullOrWhiteSpace(dataGridSetting.QueryExpression))
            {
                whereStr += " and " +  com.august.Core.XQuery.NHibernate.NHibernateQueryExpressionConverter<ViewEquipmentSampleItemQuery>.ToSqlStr(dataGridSetting.QueryExpression, null);
            }
            var results = (ArrayList)GetMutiResult(string.Format("select sum(quatity) from (select 1 as quatity from viewEquipmentsampleItemQuery where {0} group by equipmentid) t", whereStr));
            dataGridSetting.Total = results[0] == null ? 0 : (int)results[0];
            string sql = string.Format("select top {0} equipmentid from viewEquipmentsampleItemQuery where {1}  group by equipmentid", (dataGridSetting.PageIndex * dataGridSetting.PageSize).ToString(), whereStr);
            var equipmentIds = (ArrayList)GetMutiResult(sql);
            IList<Equipment> equipments = null;
            if (equipmentIds != null && equipmentIds.Count > 0)
            {
                StringBuilder sbQueryExpression = new StringBuilder("(");
                foreach (var equipmentId in equipmentIds)
                    sbQueryExpression.Append(string.Format("{0}Id=\"{1}\"", sbQueryExpression.ToString() == "(" ? "" : "+", equipmentId.ToString()));
                sbQueryExpression.Append(")");
                equipments = __equipmentBLL.GetEntitiesByExpression(sbQueryExpression.ToString()).Skip((dataGridSetting.PageIndex - 1) * dataGridSetting.PageSize).Take(dataGridSetting.PageSize).ToList();
            }
            return equipments;
        }

        public IList<SampleStatus> GetSampleStatusSampleItems(DataGridSettings dataGridSetting)
        {
            var whereStr = string.IsNullOrWhiteSpace(dataGridSetting.QueryExpression) ? "1=1" : com.august.Core.XQuery.NHibernate.NHibernateQueryExpressionConverter<ViewEquipmentSampleItemQuery>.ToSqlStr(dataGridSetting.QueryExpression, null);
            var results = (ArrayList)GetMutiResult(string.Format("select sum(quatity) from (select 1 as quatity from viewEquipmentsampleItemQuery where {0} group by samplestatusid) t", whereStr));
            dataGridSetting.Total = results[0] == null ? 0 : (int)results[0];
            string sql = string.Format("select top {0} samplestatusid from viewEquipmentsampleItemQuery where {1} group by samplestatusid", (dataGridSetting.PageIndex * dataGridSetting.PageSize).ToString(), whereStr);
            var sampleStatusIds = (ArrayList)GetMutiResult(sql);
            IList<SampleStatus> sampleStatus = null;
            if (sampleStatusIds != null && sampleStatusIds.Count > 0)
            {
                StringBuilder sbQueryExpression = new StringBuilder("(");
                foreach (var sampleStatusId in sampleStatusIds)
                    sbQueryExpression.Append(string.Format("{0}Id=\"{1}\"", sbQueryExpression.ToString() == "(" ? "" : "+", sampleStatusId.ToString()));
                sbQueryExpression.Append(")");
                sampleStatus = __sampleStatusBLL.GetEntitiesByExpression(sbQueryExpression.ToString()).Skip((dataGridSetting.PageIndex - 1) * dataGridSetting.PageSize).Take(dataGridSetting.PageSize).ToList(); ;
            }
            return sampleStatus;
        }
    }
}
