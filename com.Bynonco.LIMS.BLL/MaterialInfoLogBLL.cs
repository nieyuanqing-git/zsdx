using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;

namespace com.Bynonco.LIMS.BLL
{
    public class MaterialInfoLogBLL : BLLBase<MaterialInfoLog>, IMaterialInfoLogBLL
    {
        public bool AddInfoLog(Guid userId, Guid materialInfoId, double difValue, double stockValue, MaterialInfoLogOperateType operateType, MaterialInfoLogBusinessType businessType, Guid? businessId, string remark, ref XTransaction tran)
        {
            var materialInfoLog = new MaterialInfoLog();
            materialInfoLog.Id = Guid.NewGuid();
            materialInfoLog.MaterialInfoId = materialInfoId;
            materialInfoLog.DifValue = difValue;
            materialInfoLog.StockValue = stockValue;
            materialInfoLog.OperatorId = userId;
            materialInfoLog.OperateTime = DateTime.Now;
            materialInfoLog.OperateType = (int)operateType;
            materialInfoLog.BusinessType = (int)businessType;
            materialInfoLog.BusinessId = businessId;
            materialInfoLog.Remark = remark;
            return Add(new MaterialInfoLog[] { materialInfoLog }, ref tran, true);
        }
    }
}