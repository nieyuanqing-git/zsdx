using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class TrainningQuestionBLL : BLLBase<TrainningQuestion>, ITrainningQuestionBLL
    {
        private ITrainningQuestionItemBLL __trainningQuestionItemBLL;
        public TrainningQuestionBLL()
        {
            __trainningQuestionItemBLL = BLLFactory.CreateTrainningQuestionItemBLL();
        }
        public string AutoSetQuestionAnswer(Guid TrainningQuestionId)
        {
            string answer = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var trainningQuestion = GetEntityById(TrainningQuestionId);
                if (trainningQuestion != null)
                {
                    var trainningQuestionItemList = __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("IsStop=false*IsDelete=false*TrainningQuestionId=\"{0}\"", TrainningQuestionId), null, "XPath");
                    if (trainningQuestionItemList != null && trainningQuestionItemList.Count() > 0)
                    {
                       
                        int i = 0;
                        byte byteItemValueStart = System.Text.Encoding.Default.GetBytes("A")[0];
                        foreach (var item in trainningQuestionItemList)
                        {
                            if (i == 26) break;//只有26个字母
                            if (trainningQuestion.QuestionType != (int)QuestionType.YesNo)  item.ItemValue = Convert.ToChar('A' + i).ToString();
                            if (item.IsCorrect) answer += item.ItemValue;
                            i++;
                        }
                        trainningQuestion.Answer = answer;
                        __trainningQuestionItemBLL.Save(trainningQuestionItemList.ToArray(), ref tran, true);
                        Save(new TrainningQuestion[]{trainningQuestion},ref tran, true);
                        tran.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
            }
            finally { tran.Dispose(); }
            return answer;
        }
        public string AutoSetQuestionItem(Guid TrainningQuestionId)
        {
            string answer = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                var trainningQuestion = GetEntityById(TrainningQuestionId);
                if (trainningQuestion != null)
                {
                    if (trainningQuestion.QuestionType == (int)QuestionType.YesNo)
                    {
                        #region 判断题
                        var trainningQuestionItemOtherList = __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("ItemValue=-\"对\"*ItemValue=-\"错\"*IsStop=false*IsDelete=false*TrainningQuestionId=\"{0}\"", TrainningQuestionId), null, "XPath");
                        if (trainningQuestionItemOtherList != null && trainningQuestionItemOtherList.Count() > 0)
                        {
                            foreach (var itemOther in trainningQuestionItemOtherList)
                            {
                                itemOther.IsStop = true;
                                itemOther.ItemValue = "";
                            }
                            __trainningQuestionItemBLL.Save(trainningQuestionItemOtherList.ToArray(), ref tran, true);
                        }
                        var trainningQuestionItemYesNo = __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("(ItemValue=\"对\"+ItemValue=\"错\")*IsDelete=false*TrainningQuestionId=\"{0}\"", TrainningQuestionId), null, "XPath");
                        TrainningQuestionItem itemYes = null;
                        TrainningQuestionItem itemNo = null;
                        if (trainningQuestionItemYesNo != null && trainningQuestionItemYesNo.Count() > 0)
                        {
                            foreach (var item in trainningQuestionItemYesNo) item.IsStop = true;
                            if (trainningQuestionItemYesNo.Where(p => p.ItemValue == "对").Count() > 0)
                            {
                                itemYes = trainningQuestionItemYesNo.Where(p => p.ItemValue == "对").First();
                                itemYes.IsStop = false;
                            }
                            if (trainningQuestionItemYesNo.Where(p => p.ItemValue == "错").Count() > 0)
                            {
                                itemNo = trainningQuestionItemYesNo.Where(p => p.ItemValue == "错").First();
                                itemNo.IsStop = false;
                            }
                            if (itemYes != null && itemNo != null)
                            {
                                if (itemYes.IsCorrect && itemNo.IsCorrect)
                                {
                                    itemYes.IsCorrect = false;
                                    itemNo.IsCorrect = false;
                                }
                                else if (itemYes.IsCorrect) answer = itemYes.ItemValue;
                                else if (itemNo.IsCorrect) answer = itemNo.ItemValue;
                            }
                            __trainningQuestionItemBLL.Save(trainningQuestionItemYesNo.ToArray(), ref tran, true);
                        }
                        if (itemYes == null || itemNo == null)
                        {
                            Guid? tempId = null;
                            Guid? tempParentId = null;
                            var newXPath = XPathUtility.GenerateXPath(tempId, tempParentId,
                                (entityId) => { return __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("Id=\"{0}\"*IsDelete=false", entityId.ToString())).First(); },
                                null,
                                () => { return __trainningQuestionItemBLL.GetEntitiesByExpression("IsDelete=false"); }, 9);
                            if (itemYes == null)
                            {
                                itemYes = new TrainningQuestionItem();
                                itemYes.Id = Guid.NewGuid();
                                itemYes.TrainningQuestionId = TrainningQuestionId;
                                itemYes.XPath = newXPath;
                                newXPath = XPathUtility.GenerateNewXPath(newXPath, 9);
                                itemYes.IsStop = false;
                                itemYes.IsCorrect = false;
                                itemYes.IsDelete = false;
                                itemYes.ItemTextHTML = "";
                                itemYes.ItemTextNoHTML = "";
                                itemYes.ItemValue = "对";
                                __trainningQuestionItemBLL.Add(new TrainningQuestionItem[]{ itemYes }, ref tran, true);
                            }
                            if(itemNo == null)
                            {
                                itemNo = new TrainningQuestionItem();
                                itemNo.Id = Guid.NewGuid();
                                itemNo.TrainningQuestionId = TrainningQuestionId;
                                itemNo.XPath = newXPath;
                                itemNo.IsStop = false;
                                itemNo.IsCorrect = false;
                                itemNo.IsDelete = false;
                                itemNo.ItemTextHTML = "";
                                itemNo.ItemTextNoHTML = "";
                                itemNo.ItemValue = "错";
                                __trainningQuestionItemBLL.Add(new TrainningQuestionItem[] { itemNo }, ref tran, true);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 单选题/多选题
                        var trainningQuestionItemYesNo = __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("(ItemValue=\"对\"+ItemValue=\"错\")*IsStop=false*IsDelete=false*TrainningQuestionId=\"{0}\"", TrainningQuestionId), null, "XPath");
                        if (trainningQuestionItemYesNo != null && trainningQuestionItemYesNo.Count() > 0)
                        {
                            foreach (var itemYesNo in trainningQuestionItemYesNo)
                            {
                                itemYesNo.IsStop = true;
                                itemYesNo.ItemTextHTML = itemYesNo.ItemValue;
                                itemYesNo.ItemTextNoHTML = itemYesNo.ItemValue;
                                itemYesNo.ItemValue = "";
                            }
                            __trainningQuestionItemBLL.Save(trainningQuestionItemYesNo.ToArray(), ref tran, true);
                        }
                        var trainningQuestionItemList = __trainningQuestionItemBLL.GetEntitiesByExpression(string.Format("ItemValue=-\"对\"*ItemValue=-\"错\"*IsStop=false*IsDelete=false*TrainningQuestionId=\"{0}\"", TrainningQuestionId), null, "XPath");
                        if (trainningQuestionItemList != null && trainningQuestionItemList.Count() > 0)
                        {
                            if (trainningQuestion.QuestionType == (int)QuestionType.Radio)
                            {
                                if (trainningQuestionItemList.Where(p => p.IsCorrect == true).Count() > 1)
                                {
                                    foreach (var item in trainningQuestionItemList) item.IsCorrect = false;
                                    __trainningQuestionItemBLL.Save(trainningQuestionItemList.ToArray(), ref tran, true);
                                }
                                else
                                    answer = trainningQuestionItemList.Where(p => p.IsCorrect == true).First().ItemValue;
                            }
                            else if (trainningQuestion.QuestionType == (int)QuestionType.CheckBox)
                            {
                                foreach (var item in trainningQuestionItemList)
                                    if (item.IsCorrect) answer += item.ItemValue;
                            }
                        }
                        #endregion
                    }
                    trainningQuestion.Answer = answer;
                    Save(new TrainningQuestion[] { trainningQuestion }, ref tran, true);
                    tran.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
            }
            finally { tran.Dispose(); }
            return answer;
        }

    }
}
