using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Model.Business.CERS.InstrusAndGroups;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class EquipmentTemperatureHumidityBLL : BLLBase<EquipmentTemperatureHumidity>, IEquipmentTemperatureHumidityBLL
    {
        public IList<EquipmentTemperatureHumidity> GetEquipmentTemperatureHumidityByCondition(string equipmentId)
        {
            try
            {
                string sqlStr = string.Format(@"SELECT TOP 1 * 
                                                FROM EquipmentTemperatureHumidity                                                                   
                                                WHERE EquipmentId ='{0}' 
                                                ORDER BY CreateTime desc", equipmentId == null ? "" : equipmentId);
                return GetEntitiesBySql(sqlStr, true, true);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}