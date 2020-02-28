using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewUserMaterialReadingTimeBLL : BLLBase<ViewUserMaterialReadingTime>, IViewUserMaterialReadingTimeBLL
    {
        private ITrainningTypeBLL __trainningTypeBLL;
        public ViewUserMaterialReadingTimeBLL()
        {
            __trainningTypeBLL = BLLFactory.CreateTrainningTypeBLL();
        }
        public double GetUserTotalReadingHours(Guid userId, Guid trainningTypeId)
        {
            double totalReadingHours = 0;
            var trainningType = __trainningTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"",trainningTypeId));
            if (trainningType != null)
            {
                string queryExpression = "";
                if (!trainningType.IsGeneral)
                    queryExpression += string.Format("(TrainningTypeIsGeneral=true+TrainningTypeXPath→\"{0}\")", trainningType.XPath);
                else queryExpression = "Id=-null";
                var viewUserMaterialReadingTimeList = GetEntitiesByExpression(queryExpression);
                if (viewUserMaterialReadingTimeList != null && viewUserMaterialReadingTimeList.Count() > 0)
                {
                    totalReadingHours = viewUserMaterialReadingTimeList.Where(p => p.TotalSeconds.HasValue).Sum(p => p.TotalSeconds.Value);
                }
            }
            return totalReadingHours/3600;
                 
        }
        
    }
}
