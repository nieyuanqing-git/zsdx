using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Business;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.august.DataLink;
using com.Bynonco.LIMS.Utility;

namespace com.Bynonco.LIMS.BLL
{
    public class JudgeEquipmentRecordBLL : BLLBase<JudgeEquipmentRecord>, IJudgeEquipmentRecordBLL 
    {
        private IJudgeProjectRecordBLL __judgeProjectRecordBLL;
        private IJudgeProjectContentRecordBLL __judgeProjectContentRecordBLL;

        public JudgeEquipmentRecordBLL()
        {
            __judgeProjectRecordBLL = BLLFactory.CreateJudgeProjectRecordBLL();
            __judgeProjectContentRecordBLL = BLLFactory.CreateJudgeProjectContentRecordBLL();
        }
        public bool SaveJudgeEquipmentRecordStatistics(Guid? userId, JudgeEquipmentRecordStatistics model, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                if (model == null) throw new Exception("设备审核评价为空");
                JudgeEquipmentRecord judgeEquipmentRecord = null;
                bool isNew = false;
                if (model.JudgeEquipmentRecordId.HasValue) judgeEquipmentRecord = GetEntityById(model.JudgeEquipmentRecordId.Value);
                else
                {
                    isNew = true;
                    judgeEquipmentRecord = new JudgeEquipmentRecord();
                    judgeEquipmentRecord.Id = Guid.NewGuid();
                    judgeEquipmentRecord.CreaterId = userId.Value;
                    judgeEquipmentRecord.CreateTime = DateTime.Now;
                }
                if (judgeEquipmentRecord == null) throw new Exception("设备审核评价记录为空");
                judgeEquipmentRecord.EquipmentId = model.EquipmentId.Value;
                judgeEquipmentRecord.RecordNum = model.RecordNum;
                judgeEquipmentRecord.JudgeTime = model.JudgeTime.Value;
                judgeEquipmentRecord.UpdateUserId = userId.Value;
                judgeEquipmentRecord.UpdateTime = DateTime.Now;
                judgeEquipmentRecord.IsDelete = false;
                if (isNew) Add(new JudgeEquipmentRecord[] { judgeEquipmentRecord }, ref tran, true);
                else Save(new JudgeEquipmentRecord[] { judgeEquipmentRecord }, ref tran, true);
                IList<JudgeProjectRecord> judgeProjectRecordList = null;
                IList<JudgeProjectContentRecord> judgeProjectContentRecordList = null;

                if (!isNew) judgeProjectRecordList = new List<JudgeProjectRecord>();
                if (model.JudgeProjectRecordStatisticsList != null && model.JudgeProjectRecordStatisticsList.Count() > 0)
                {
                    foreach (var item in model.JudgeProjectRecordStatisticsList)
                    {
                        bool isItemNew = false;
                        JudgeProjectRecord judgeProjectRecord = item.JudgeProjectRecordId.HasValue ? __judgeProjectRecordBLL.GetEntityById(item.JudgeProjectRecordId.Value) : null;
                        if (judgeProjectRecord == null)
                        {
                            isItemNew = true;
                            judgeProjectRecord = new JudgeProjectRecord();
                            judgeProjectRecord.Id = Guid.NewGuid();
                            judgeProjectRecord.JudgeEquipmentRecordId = judgeEquipmentRecord.Id;
                            judgeProjectRecord.JudgeProjectId = item.JudgeProjectId;
                            judgeProjectRecord.CreaterId = userId.Value;
                            judgeProjectRecord.CreateTime = DateTime.Now;

                        }
                        judgeProjectRecord.ItemCent = item.ItemCent.Value;
                        judgeProjectRecord.ItemWeightCent = item.JudgeProjectWeight;
                        judgeProjectRecord.UpdateUserId = userId.Value;
                        judgeProjectRecord.UpdateTime = DateTime.Now;
                        judgeProjectRecord.IsDelete = false;
                        if (isItemNew) __judgeProjectRecordBLL.Add(new JudgeProjectRecord[] { judgeProjectRecord }, ref tran, true);
                        else __judgeProjectRecordBLL.Save(new JudgeProjectRecord[] { judgeProjectRecord }, ref tran, true);
                        if (!isNew) judgeProjectRecordList.Add(judgeProjectRecord);
                        if (!isNew && !isItemNew) judgeProjectContentRecordList = new List<JudgeProjectContentRecord>();
                        if (item.JudgeProjectContentRecordStatisticsList != null && item.JudgeProjectContentRecordStatisticsList.Count() > 0)
                        {
                            foreach (var itemContent in item.JudgeProjectContentRecordStatisticsList)
                            {
                                bool isItemContentNew = false;
                                JudgeProjectContentRecord judgeProjectContentRecord = itemContent.JudgeProjectContentRecordId.HasValue ? __judgeProjectContentRecordBLL.GetEntityById(itemContent.JudgeProjectContentRecordId.Value) : null;
                                if (judgeProjectContentRecord == null)
                                {
                                    isItemContentNew = true;
                                    judgeProjectContentRecord = new JudgeProjectContentRecord();
                                    judgeProjectContentRecord.Id = Guid.NewGuid();
                                    judgeProjectContentRecord.JudgeProjectRecordId = judgeProjectRecord.Id;
                                    judgeProjectContentRecord.JudgeProjectContentId = itemContent.JudgeProjectContentId;
                                    judgeProjectContentRecord.CreaterId = userId.Value;
                                    judgeProjectContentRecord.CreateTime = DateTime.Now;
                                }
                                judgeProjectContentRecord.ContentCent = itemContent.ContentCent;
                                judgeProjectContentRecord.Remark = itemContent.Remark;
                                judgeProjectContentRecord.UpdateUserId = userId.Value;
                                judgeProjectContentRecord.UpdateTime = DateTime.Now;
                                judgeProjectContentRecord.IsDelete = false;
                                if (isItemContentNew) __judgeProjectContentRecordBLL.Add(new JudgeProjectContentRecord[] { judgeProjectContentRecord }, ref tran, true);
                                else __judgeProjectContentRecordBLL.Save(new JudgeProjectContentRecord[] { judgeProjectContentRecord }, ref tran, true);
                                if (!isNew && !isItemNew) judgeProjectContentRecordList.Add(judgeProjectContentRecord);
                            }
                        }
                        if (!isItemNew)
                        {
                            var delJudgeProjectContentRecordList = __judgeProjectContentRecordBLL.GetEntitiesByExpression(string.Format("JudgeProjectRecordId=\"{0}\"*IsDelete=false{1}", judgeProjectRecord.Id, judgeProjectContentRecordList != null && judgeProjectContentRecordList.Count() > 0 ? "*" + judgeProjectContentRecordList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot) : ""));
                            if (delJudgeProjectContentRecordList != null && delJudgeProjectContentRecordList.Count() > 0)
                            {
                                foreach (var delItemContent in delJudgeProjectContentRecordList)
                                {
                                    delItemContent.IsDelete = true;
                                    delItemContent.UpdateUserId = userId.Value;
                                    delItemContent.UpdateTime = DateTime.Now;
                                }
                                __judgeProjectContentRecordBLL.Save(delJudgeProjectContentRecordList, ref tran, true);
                            }
                        }

                    }
                    if (!isNew)
                    {
                        var delJudgeProjectRecordList = __judgeProjectRecordBLL.GetEntitiesByExpression(string.Format("JudgeEquipmentRecordId=\"{0}\"*IsDelete=false{1}", judgeEquipmentRecord.Id, judgeProjectRecordList != null && judgeProjectRecordList.Count() > 0 ? "*" + judgeProjectRecordList.Select(p => p.Id).ToFormatStr("Id", LogicRelation.AndNot): ""));
                        if (delJudgeProjectRecordList != null && delJudgeProjectRecordList.Count() > 0)
                        {
                            foreach (var delItem in delJudgeProjectRecordList)
                            {
                                delItem.IsDelete = true;
                                delItem.UpdateUserId = userId.Value;
                                delItem.UpdateTime = DateTime.Now;
                            }
                            __judgeProjectRecordBLL.Save(delJudgeProjectRecordList, ref tran, true);
                        }
                    }
                }
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }
        public bool DeleteJudgeEquipmentRecord(Guid? userId, Guid judgeEquipmentRecordId, out string errorMessage)
        {
            errorMessage = "";
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (!userId.HasValue) throw new Exception("操作用户为空");
                var judgeEquipmentRecord = GetEntityById(judgeEquipmentRecordId);
                if (judgeEquipmentRecord == null) throw new Exception("设备审核评价记录为空");
                judgeEquipmentRecord.IsDelete = true;
                judgeEquipmentRecord.UpdateUserId = userId.Value;
                judgeEquipmentRecord.UpdateTime = DateTime.Now;
                if (judgeEquipmentRecord.JudgeProjectRecordList != null && judgeEquipmentRecord.JudgeProjectRecordList.Count() > 0)
                {
                    foreach (var item in judgeEquipmentRecord.JudgeProjectRecordList)
                    {
                        item.IsDelete = true;
                        item.UpdateUserId = userId.Value;
                        item.UpdateTime = DateTime.Now;
                        if (item.JudgeProjectContentRecordList != null && item.JudgeProjectContentRecordList.Count() > 0)
                        {
                            foreach (var itemContent in item.JudgeProjectContentRecordList)
                            {
                                itemContent.IsDelete = true;
                                itemContent.UpdateUserId = userId.Value;
                                itemContent.UpdateTime = DateTime.Now;
                            }
                            __judgeProjectContentRecordBLL.Save(item.JudgeProjectContentRecordList, ref tran, true);
                        }
                    }
                    __judgeProjectRecordBLL.Save(judgeEquipmentRecord.JudgeProjectRecordList, ref tran, true);
                }
                Save(new JudgeEquipmentRecord[] { judgeEquipmentRecord }, ref tran, true);
                tran.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                errorMessage = ex.Message.IndexOf("出错") == -1 ? "出错," + ex.Message : ex.Message;
                return false;
            }
            finally { tran.Dispose(); }
        }
    }
}
