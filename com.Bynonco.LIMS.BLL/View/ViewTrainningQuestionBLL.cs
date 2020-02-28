using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.Logic.Model;
using com.Bynonco.JqueryEasyUI.Core;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class ViewTrainningQuestionBLL : BLLBase<ViewTrainningQuestion>, IViewTrainningQuestionBLL
    {
        private ITrainningTypeBLL __trainningTypeBLL;
        private ITrainningExaminationQuestionBLL __trainningExaminationQuestionBLL;
        
        public ViewTrainningQuestionBLL()
        {
            __trainningTypeBLL = BLLFactory.CreateTrainningTypeBLL();
            __trainningExaminationQuestionBLL = BLLFactory.CreateTrainningExaminationQuestionBLL();
        }

        public IList<ViewTrainningQuestion> GetViewTrainningQuestionListByTrainningExaminationId(Guid trainningExaminationId)
        {
            IList<ViewTrainningQuestion> vireTrainningQuestionList = null;
            var trainningExaminationQuestionList = __trainningExaminationQuestionBLL.GetEntitiesByExpression(string.Format("TrainningExaminationId=\"{0}\"", trainningExaminationId),null,"",true);
            if (trainningExaminationQuestionList != null && trainningExaminationQuestionList.Count() > 0)
            {
                var queryExpression = trainningExaminationQuestionList.Select(p => p.TrainningQuestionId).ToFormatStr();
                queryExpression += (queryExpression == "" ? "" : "*") + "IsDelete=false*IsStop=false";
                vireTrainningQuestionList = GetEntitiesByExpression(queryExpression, null, "TrainningTypeXPath A,XPath A");
            }
            return vireTrainningQuestionList;
        }
        public IList<ViewTrainningQuestion> GetRandomViewTrainningQuestionList(Guid trainningTypeId, int randomCount)
        {
            IList<ViewTrainningQuestion> viewTrainningQuestionRandomList = null;
            var trainningType = __trainningTypeBLL.GetEntityById(trainningTypeId);
            if (trainningType != null)
            {
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "IsDelete=false*IsStop=false" };
                if (!trainningType.IsGeneral)
                {
                    dataGridSettings.AppendAndQueryExpression(string.Format("(TrainningTypeIsGeneral=true+TrainningTypeXPath→\"{0}\")", trainningType.XPath));
                }
                var viewTrainningQuestionList = GetEntitiesByExpression(dataGridSettings, null, "TrainningTypeXPath A,XPath A");
                if (viewTrainningQuestionList != null && viewTrainningQuestionList.Count > 0)
                    viewTrainningQuestionRandomList = GetRandomList(viewTrainningQuestionList.ToList(), randomCount);
            }
            return viewTrainningQuestionRandomList;
        }
    }
}
