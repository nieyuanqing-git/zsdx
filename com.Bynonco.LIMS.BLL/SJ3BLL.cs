using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class SJ3BLL : BLLBase<SJ3>, ISJ3BLL
    {
        private IDictCodeBLL __dictCodeBLL;
        public SJ3BLL()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        public override bool Add(IEnumerable<SJ3> entities, ref XTransaction tran, bool isSupress = false)
        {
            FitSJ3(ref entities);
            return base.Add(entities, ref tran, isSupress);
        }
        public override bool Save(IEnumerable<SJ3> entities, ref XTransaction tran, bool isSupress = false)
        {
            FitSJ3(ref entities);
            return base.Save(entities, ref tran, isSupress);
        }
        private void FitSJ3(ref IEnumerable<SJ3> entities)
        {
            string errorMessage = "";
            var schoolCode = __dictCodeBLL.GetDictCodeValueByCode("System", "SchoolCode");
            foreach (var item in entities)
            {
                if (string.IsNullOrWhiteSpace(item.SchoolCode)) item.SchoolCode = schoolCode;
                if (!item.UnitPrice.IsDoubleExtend("单价", "", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.UnitPrice = "0";
                item.UnitPrice = double.Parse(item.UnitPrice).ToString("F2");
                if (string.IsNullOrWhiteSpace(item.Model)) item.Model = "*";
                if (!item.UsedHourEducation.IsIntExtend("教学使用机时", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.UsedHourEducation = "0";
                if (!item.UsedHourScientificResearch.IsIntExtend("科研使用机时", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.UsedHourScientificResearch = "0";
                if (!item.UsedHourSocialServices.IsIntExtend("社会服务使用机时", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.UsedHourSocialServices = "0";
                if (!item.UsedHourOpen.IsIntExtend("开放使用机时", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.UsedHourOpen = "0";
                if (!item.SampleItemCount.IsIntExtend("测样数", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.SampleItemCount = "0";
                if (!item.TrainingStudent.IsIntExtend("培训人员数_学生", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.TrainingStudent = "0";
                if (!item.TrainingTutor.IsIntExtend("培训人员数_教师", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.TrainingTutor = "0";
                if (!item.TrainingOther.IsIntExtend("培训人员数_其他", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.TrainingOther = "0";
                if (!item.ProjectEducation.IsIntExtend("教学实验项目数", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.ProjectEducation = "0";
                if (!item.ProjectScientificResearch.IsIntExtend("科研项目数", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.ProjectScientificResearch = "0";
                if (!item.ProjectSocialServices.IsIntExtend("社会服务项目数", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.ProjectSocialServices = "0";
                if (!item.AwardsCountry.IsIntExtend("获奖等级_国家级", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.AwardsCountry = "0";
                if (!item.AwardsProvince.IsIntExtend("获奖等级_省部级", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.AwardsProvince = "0";
                if (!item.PatentTutor.IsIntExtend("发明专利_教师", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.PatentTutor = "0";
                if (!item.PatentStudent.IsIntExtend("发明专利_学生", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.PatentStudent = "0";
                if (!item.ThesisThreeSearch.IsIntExtend("论文情况_三大检索", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.ThesisThreeSearch = "0";
                if (!item.ThesisPublication.IsIntExtend("论文情况_核心刊物", null, null, NumericalRangeType.PositiveWithZero, out errorMessage)) item.ThesisPublication = "0";
                if (string.IsNullOrWhiteSpace(item.AdminName)) item.AdminName = "*";

                #region 字段长度控制
                item.SchoolCode = item.SchoolCode.CutLength(5);
                item.Label = item.Label.CutLength(8);
                item.CategoryCode = item.CategoryCode.CutLength(8);
                item.Name = item.Name.CutLength(30);
                item.UnitPrice = item.UnitPrice.CutLength(12);
                item.Model = item.Model.Trim().CutLength(20);
                item.Specifications = item.Specifications.CutLength(200);
                item.UsedHourEducation = item.UsedHourEducation.CutLength(4);
                item.UsedHourScientificResearch = item.UsedHourScientificResearch.CutLength(4);
                item.UsedHourSocialServices = item.UsedHourSocialServices.CutLength(4);
                item.UsedHourOpen = item.UsedHourOpen.Trim().CutLength(4);
                item.SampleItemCount = item.SampleItemCount.CutLength(6);
                item.TrainingStudent = item.TrainingStudent.CutLength(4);
                item.TrainingTutor = item.TrainingTutor.CutLength(4);
                item.TrainingOther = item.TrainingOther.CutLength(4);
                item.ProjectEducation = item.ProjectEducation.CutLength(3);
                item.ProjectScientificResearch = item.ProjectScientificResearch.CutLength(3);
                item.ProjectSocialServices = item.ProjectSocialServices.CutLength(3);
                item.AwardsCountry = item.AwardsCountry.CutLength(2);
                item.AwardsProvince = item.AwardsProvince.CutLength(2);
                item.PatentTutor = item.PatentTutor.CutLength(2);
                item.PatentStudent = item.PatentStudent.CutLength(2);
                item.ThesisThreeSearch = item.ThesisThreeSearch.CutLength(3);
                item.ThesisPublication = item.ThesisPublication.CutLength(3);
                item.AdminName = item.AdminName.CutLength(8);
                #endregion
            }
        }
    }
}