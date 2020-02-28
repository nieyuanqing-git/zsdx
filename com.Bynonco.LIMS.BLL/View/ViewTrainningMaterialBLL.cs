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
    public class ViewTrainningMaterialBLL : BLLBase<ViewTrainningMaterial>, IViewTrainningMaterialBLL
    {
        private ITrainningTypeBLL __trainningTypeBLL;

        private ITrainningExaminationBLL __trainningExaminationBLL;
        private ITrainningExaminationMaterialBLL __trainningExaminationMaterialBLL;
        private IUserExaminationBLL __userExaminationBLL;
        
        public ViewTrainningMaterialBLL()
        {
            __trainningTypeBLL = BLLFactory.CreateTrainningTypeBLL();
            __trainningExaminationBLL = BLLFactory.CreateTrainningExaminationBLL();
            __trainningExaminationMaterialBLL = BLLFactory.CreateTrainningExaminationMaterialBLL();
            __userExaminationBLL = BLLFactory.CreateUserExaminationBLL();
        }

        public IList<ViewTrainningMaterial> GetViewTrainningMaterialListByTrainningExaminationId(Guid trainningExaminationId)
        {
            IList<ViewTrainningMaterial> vireTrainningMaterialList = null;
            var trainningExaminationMaterialList = __trainningExaminationMaterialBLL.GetEntitiesByExpression(string.Format("TrainningExaminationId=\"{0}\"", trainningExaminationId),null,"",true);
            if (trainningExaminationMaterialList != null && trainningExaminationMaterialList.Count() > 0)
            {
                var queryExpression = trainningExaminationMaterialList.Select(p => p.TrainningMaterialId).ToFormatStr();
                queryExpression += (queryExpression == "" ? "" : "*") + "IsDelete=false*IsStop=false";
                vireTrainningMaterialList = GetEntitiesByExpression(queryExpression, null, "TrainningTypeXPath A,XPath A");
            }
            return vireTrainningMaterialList;
        }
        public IList<ViewTrainningMaterial> GetViewTrainningMaterialList(Guid trainningTypeId)
        {
            IList<ViewTrainningMaterial> viewTrainningMaterialList = null;
            var trainningType = __trainningTypeBLL.GetEntityById(trainningTypeId);
            if (trainningType != null)
            {
                DataGridSettings dataGridSettings = new DataGridSettings() { QueryExpression = "IsDelete=false*IsStop=false" };
                if (!trainningType.IsGeneral)
                {
                    dataGridSettings.AppendAndQueryExpression(string.Format("(TrainningTypeIsGeneral=true+TrainningTypeXPath→\"{0}\")", trainningType.XPath));
                }
                viewTrainningMaterialList = GetEntitiesByExpression(dataGridSettings, null, "TrainningTypeXPath A,XPath A");
            }
            return viewTrainningMaterialList;
        }

        public GridData<ViewTrainningMaterial> GetGridUserExaminationMaterialList(DataGridSettings dataGridSettings, int businessType, Guid businessId, Guid? trainningExaminationId, out string errorMessage)
        {
            errorMessage = "";
            IList<ViewTrainningMaterial> viewTrainningMaterialList = null;
            if (businessType == -1)
            {
                errorMessage = "出错,考试所属为空";
                return null;
            }
            IList<Guid> trainningTypeIdList = new List<Guid>();
            viewTrainningMaterialList = new List<ViewTrainningMaterial>();
            if (trainningExaminationId.HasValue)
            {
                var trainningExamination = __trainningExaminationBLL.GetFirstOrDefaultEntityByExpression(string.Format("Id=\"{0}\"*IsStop=false*IsDelete=false", trainningExaminationId.Value));
                if (trainningExamination == null)
                {
                    errorMessage = "出错,考试试卷不存在";
                    return null;
                }
                trainningTypeIdList.Add(trainningExamination.TrainningTypeId);
                var queryExpression = "";
                if (trainningExamination.TrainningMaterialList != null && trainningExamination.TrainningMaterialList.Count() > 0)
                {
                    queryExpression += (queryExpression == "" ? "" : "+") + trainningExamination.TrainningMaterialList.Select(p => p.Id).ToFormatStr();
                }
                if (queryExpression != "")
                    viewTrainningMaterialList = GetEntitiesByExpression(queryExpression, null, "TrainningTypeXPath A,XPath A", true);
            }
            else
            {
                var examinationBusiness = __userExaminationBLL.GetExaminationBusiness(null,businessType, businessId, out errorMessage);
                if (examinationBusiness == null)
                {
                    errorMessage = "出错,考试所属无效";
                    return null;
                }
                if (examinationBusiness.TrainningTypeId.HasValue) trainningTypeIdList.Add(examinationBusiness.TrainningTypeId.Value);
            }
            
            if (viewTrainningMaterialList.Count() == 0 && trainningTypeIdList.Count() > 0)
            {
                var trainningTypeList = __trainningTypeBLL.GetEntitiesByExpression(trainningTypeIdList.ToFormatStr(), null, "", true);
                if (trainningTypeList != null && trainningTypeList.Count() > 0)
                {
                    var queryExpression = "";
                    bool isGeneral = false;
                    foreach (var item in trainningTypeList)
                    {
                        if (item.IsGeneral)
                        {
                            isGeneral = true;
                            break;
                        }
                        queryExpression += (queryExpression == "" ? "" : "+") + string.Format("TrainningTypeXPath→\"{0}\"", item.XPath);
                    }
                    if (isGeneral)
                        viewTrainningMaterialList = GetEntitiesByExpression("IsDelete=false*IsStop=false", null, "TrainningTypeXPath A,XPath A", true);
                    else if (queryExpression != "")
                    {
                        queryExpression = "IsDelete=false*IsStop=false*(TrainningTypeIsGeneral=true+" + queryExpression + ")";
                        viewTrainningMaterialList = GetEntitiesByExpression(queryExpression, null, "TrainningTypeXPath A,XPath A", true);
                    }
                }
            }
            if (viewTrainningMaterialList == null)
            {
                errorMessage = "出错,考试培训材料为空";
                return null;
            }
            var count = viewTrainningMaterialList.Count;
            viewTrainningMaterialList = viewTrainningMaterialList.Skip((dataGridSettings.PageIndex - 1) * dataGridSettings.PageSize).Take(dataGridSettings.PageSize).ToList();
            return new GridData<ViewTrainningMaterial>()
            {
                Data = viewTrainningMaterialList,
                Count = count
            };
        }
    }
}
