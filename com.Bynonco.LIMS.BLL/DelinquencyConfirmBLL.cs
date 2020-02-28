using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.BLL.Business.Privilige;
using com.Bynonco.LIMS.Model;
using com.august.DataLink;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model.Business;

namespace com.Bynonco.LIMS.BLL
{
    public class DelinquencyConfirmBLL : BLLBase<DelinquencyConfirm>, IDelinquencyConfirmBLL
    {
        private IDelinquencyCategoryBLL __delinquencyCategoryBLL;
        private IPunishRecordBLL __punishRecordBLL;
        private IPunishActionBLL __punishActionBLL;
        private IDelinquencyRuleBLL __delinquencyRuleBLL;
        private IPunishActionDelinquencyBLL __punishActionDelinquencyBLL;
        private IRepealDelinquencyBLL __repealDelinquencyBLL;
        public DelinquencyConfirmBLL()
        {
            __delinquencyCategoryBLL = BLLFactory.CreateDelinquencyCategoryBLL();
            __punishRecordBLL = BLLFactory.CreatePunishRecordBLL();
            __punishActionBLL = BLLFactory.CreatePunishActionBLL();
            __delinquencyRuleBLL = BLLFactory.CreateDelinquencyRuleBLL();
            __punishActionDelinquencyBLL = BLLFactory.CreatePunishActionDelinquencyBLL();
            __repealDelinquencyBLL = BLLFactory.CreateRepealDelinquencyBLL();
        }
        /// <summary> 生成不良行为记录 </summary>
        /// <param name="appointmentId"></param>
        /// <param name="operatorId"></param>
        /// <param name="delinquencyAt"></param>
        /// <returns></returns>
        public DelinquencyConfirm GenerateEquipmentAppointmentDelinquencyConfirmData(Guid appointmentId, Guid operatorId, DateTime delinquencyAt)
        {
            DelinquencyConfirm delinquencyConfirm = GenerateDelinquencyConfirmData(operatorId, delinquencyAt);
            delinquencyConfirm.AppointmentId = appointmentId;
            return delinquencyConfirm;
        }
        public DelinquencyConfirm GenerateNMPEquipmentAppointmentDelinquencyConfirmData(Guid nmpAppointmentId, Guid operatorId, DateTime delinquencyAt)
        {
            DelinquencyConfirm delinquencyConfirm = GenerateDelinquencyConfirmData(operatorId, delinquencyAt);
            delinquencyConfirm.NMPAppointmentId = nmpAppointmentId;
            return delinquencyConfirm;
        }
        /// <summary> 生成不良行为记录 </summary>
        /// <param name="operatorId"></param>
        /// <param name="delinquencyAt"></param>
        /// <returns></returns>
        private DelinquencyConfirm GenerateDelinquencyConfirmData(Guid operatorId, DateTime delinquencyAt)
        {
            DelinquencyConfirm delinquencyConfirm = new DelinquencyConfirm() { Id = Guid.NewGuid() };
            delinquencyConfirm.CreaterId = operatorId;
            delinquencyConfirm.DelinquencyName = "爽约";
            delinquencyConfirm.Cause = "爽约";
            delinquencyConfirm.DelinquencyAt = delinquencyAt;
            delinquencyConfirm.CreateAt = DateTime.Now;
            return delinquencyConfirm;
        }
        public object GetDelinquencyPriviliges(Guid? userId, IList<ViewDelinquencyConfirm> viewDelinquencyConfirmList)
        {
            IList<DelinquencyPrivilige> lstDelinquencyPriviliges = new List<DelinquencyPrivilige>();
            if (userId.HasValue && viewDelinquencyConfirmList != null && viewDelinquencyConfirmList.Count > 0)
            {
                foreach (var viewDelinquencyConfirm in viewDelinquencyConfirmList)
                {
                    if (!viewDelinquencyConfirm.PunisherId.HasValue) continue;
                    DelinquencyPrivilige delinquencyPrivilige = lstDelinquencyPriviliges.FirstOrDefault(p => p.PunisherId.HasValue && p.PunisherId == viewDelinquencyConfirm.PunisherId);
                    if (delinquencyPrivilige == null)
                    {
                        delinquencyPrivilige = PriviligeFactory.GetDelinquencyPrivilige(userId.Value, viewDelinquencyConfirm.PunisherId.Value);
                        lstDelinquencyPriviliges.Add(delinquencyPrivilige);
                    }
                }
            }
            return lstDelinquencyPriviliges;
        }
        public DelinquencyConfirm GetDelinquencyConfirmByAppointmenId(Guid appointmentId)
        {
            return GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"*HasRepeal=false", appointmentId.ToString()));
        }
        public DelinquencyConfirm GetDelinquencyConfirmByNMPAppointmenId(Guid nmpAppointmentId)
        {
            return GetFirstOrDefaultEntityByExpression(string.Format("NMPAppointmentId=\"{0}\"*HasRepeal=false", nmpAppointmentId.ToString()));
        }
        /// <summary> 添加不良行为 </summary>
        /// <param name="delinquencyConfirm"></param>
        /// <param name="user"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public DelinquencyRule AddDelinquencyConfirm(DelinquencyConfirm delinquencyConfirm,User user, ref XTransaction tran)
        {
            bool isSupress = tran!=null;
            var delinquencyCategory = __delinquencyCategoryBLL.GetFirstOrDefaultEntityByExpression(string.Format("Name=\"{0}\"", delinquencyConfirm.DelinquencyName));
            if (delinquencyCategory == null) throw new ArgumentException(string.Format("出错,找不到{0}的严重程度定义", delinquencyConfirm.DelinquencyName));
            
            IBadBehaviorPunishManager badBehaviorPunishManager = new BadBehaviorPunishManager();
            DelinquencyRule delinquencyRule = null;
            PunishRecord curPunishRecord = null;
            int previousStatus = -1;
            if(!isSupress) tran = SessionManager.Instance.BeginTransaction();
            try
            {
                if (delinquencyConfirm.AppointmentId.HasValue)
                {
                    var delinquencyConfirmValidate = GetFirstOrDefaultEntityByExpression(string.Format("AppointmentId=\"{0}\"*HasRepeal=false", delinquencyConfirm.AppointmentId.Value.ToString()));
                    if (delinquencyConfirmValidate != null)
                        throw new Exception(string.Format("出错,该预约记录已经于【{0}】被【{1}】登记爽约,不能重复登记", delinquencyConfirmValidate.CreateAt.ToString("yyyy-MM-dd HH:mm"), delinquencyConfirmValidate.User.UserName));
                }
                delinquencyConfirm.SeverityValue = (int)delinquencyCategory.Severity;
                delinquencyConfirm.HasRepeal = false;
                var punishRecord = __punishRecordBLL.GetUserPubishRecord(user.Id);
                curPunishRecord = punishRecord;
                delinquencyRule =badBehaviorPunishManager.GetDelinquencyRule(delinquencyConfirm.SeverityValue);
                if (punishRecord == null)
                {
                    punishRecord = new PunishRecord() { Id = Guid.NewGuid(), PunisherId = user.Id, TotalSeverity = delinquencyConfirm.SeverityValue, Status = delinquencyRule == null ? 0 : delinquencyRule .Type};
                    __punishRecordBLL.Add(new PunishRecord[] { punishRecord }, ref tran, true);
                    curPunishRecord = punishRecord;
                }
                else 
                {
                    punishRecord.TotalSeverity += delinquencyConfirm.SeverityValue;
                    delinquencyRule = badBehaviorPunishManager.GetDelinquencyRule(punishRecord.TotalSeverity);
                    previousStatus = punishRecord.Status;//保存原来状态,为下面判断是否进行处罚准备
                    punishRecord.Status = delinquencyRule == null ? 0 : delinquencyRule.Type;
                    __punishRecordBLL.Save(new PunishRecord[] { punishRecord }, ref tran, true);
                }
                delinquencyConfirm.PunishRecordId = curPunishRecord.Id;
                Add(new DelinquencyConfirm[] { delinquencyConfirm }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
            if (previousStatus != -1) curPunishRecord.Status = previousStatus;
            var rule = badBehaviorPunishManager.GetDelinquencyRule(curPunishRecord.TotalSeverity);
            return rule != null && (curPunishRecord.Status < rule.Type || curPunishRecord.PunishTypeEnum == PunishType.Unusable) ? rule : null;
        }
        public void Punish(PunishRecord punishRecord, DelinquencyRule delinquencyRule, DateTime beginAt, DateTime? endAt, string content, Guid creatorId)
        {
            if (endAt.HasValue && endAt.Value < beginAt)
                throw new ArgumentException("出错,开始时间必须小于结束时间");
            XTransaction tran = SessionManager.Instance.BeginTransaction();
            try
            {
                PunishAction punishAction = new PunishAction() { Id = Guid.NewGuid(), PunishTypeEnum = delinquencyRule.TypeEnum, PunishRecordId = punishRecord.Id, BeginAt = beginAt, EndAt = endAt, Content = Encoding.Default.GetBytes(content) };
                punishAction.CreateAt = DateTime.Now;
                punishAction.CreaterId = creatorId;
                punishAction.TotalSeverity = punishRecord.TotalSeverity;
                punishAction.StandardSeverity = (int)delinquencyRule.TotalSeverity;
                __punishActionBLL.Add(new PunishAction[]{punishAction},ref tran,true);
                
                punishRecord.Status = delinquencyRule.Type;
                __punishRecordBLL.Save(new PunishRecord[] { punishRecord }, ref tran, true);
                if (punishRecord.DelinquencyConfirms != null && punishRecord.DelinquencyConfirms.Any(p => !p.HasRepeal))
                {
                    var delinquencyConfirmList = punishRecord.DelinquencyConfirms.Where(p => !p.HasRepeal);
                    foreach (var delinquencyConfirm in delinquencyConfirmList)
                    {
                        PunishActionDelinquency punishActionDelinquency = new PunishActionDelinquency() { DelinquencyConfirmId = delinquencyConfirm.Id, PunishActionId = punishAction.Id, Id = Guid.NewGuid() };
                        __punishActionDelinquencyBLL.Add(new PunishActionDelinquency[] { punishActionDelinquency }, ref tran, true); 
                    }
                }
                tran.CommitTransaction();
                //发送消息
                IMessageHandler messageHandler = new MessageHandler();
                messageHandler.SendPunishNotice(punishAction, punishAction.Creator);
            }
            catch (Exception ex)
            {
                tran.RollbackTransaction();
                throw;
            }
            finally { tran.Dispose(); }
        }
        public void StopPunish(PunishAction punishAction, Guid stoperId, ref XTransaction tran)
        {
            bool isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();      
            try
            {
                if (punishAction.HasStoped) throw new Exception("处罚已停止,不能重复停止");
                punishAction.StopAt = DateTime.Now;
                punishAction.StoperId = stoperId;
                punishAction.HasStoped = true;
                punishAction.EndAt = punishAction.EndAt ?? DateTime.Now;
                __punishActionBLL.Save(new PunishAction[] { punishAction }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
            }
            catch (Exception ex)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress) tran.Dispose(); }
        }
        public void Repeal(DelinquencyConfirm delinquencyConfirm, string content, User creator, ref XTransaction tran)
        {
            bool isSupress = tran != null;
            if (!isSupress) tran = SessionManager.Instance.BeginTransaction();
            RepealDelinquency repealDelinquency = null;
            try
            {
                if (delinquencyConfirm == null) throw new ArgumentNullException("出错,不良行为为空");
                if (delinquencyConfirm.HasRepeal) throw new ArgumentNullException("出错,不良行为已经撤销,不能重复撤销");
                var punishRecord = delinquencyConfirm.PunishRecord;
                int ruleServerity = punishRecord.TotalSeverity - delinquencyConfirm.SeverityValue;
                //if (punishRecord.Status != (int)PunishType.Unusable) punishRecord.TotalSeverity = ruleServerity;
                punishRecord.TotalSeverity = ruleServerity;
                if (delinquencyConfirm.GetPunishActions() == null || delinquencyConfirm.GetPunishActions().Count == 0)
                {
                    Delete(new Guid[] { delinquencyConfirm.Id }, ref tran, true);
                }
                else
                {
                    IBadBehaviorPunishManager badBehaviorPunishManager = new BadBehaviorPunishManager();
                    repealDelinquency = new RepealDelinquency() { Id = Guid.NewGuid(), Content = content, RepealerId = creator.Id, DelinquencyConfirmId = delinquencyConfirm.Id, RepealAt = DateTime.Now };
                    __repealDelinquencyBLL.Add(new RepealDelinquency[] { repealDelinquency }, ref tran, true);
                    delinquencyConfirm.HasRepeal = true;
                    var delinquencyRule = badBehaviorPunishManager.GetDelinquencyRule(ruleServerity);
                    var ruleType = delinquencyRule == null ? PunishType.None : delinquencyRule.TypeEnum;
                    if ((int)ruleType < punishRecord.Status) //有惩罚，通知
                    {
                        punishRecord.Status = (int)ruleType;
                        var punishActions = delinquencyConfirm.GetPunishActions() != null ? delinquencyConfirm.GetPunishActions().Where(x => !x.HasRepeal && x.PunishTypeEnum > ruleType) : null;
                        if (punishActions != null && punishActions.Count() > 0)
                        {
                            foreach (var punishAction in punishActions)
                            {
                                punishAction.RepealDelinquencyId = repealDelinquency.Id;
                                punishAction.HasRepeal = true;
                                __punishActionBLL.Save(new PunishAction[] { punishAction }, ref tran, true);
                            }
                        }
                    }
                    //有惩罚，不理//有惩罚，不通知
                    Save(new DelinquencyConfirm[] { delinquencyConfirm }, ref tran, true);
                }
                __punishRecordBLL.Save(new PunishRecord[] { punishRecord }, ref tran, true);
                if (!isSupress) tran.CommitTransaction();
                if (repealDelinquency != null && delinquencyConfirm.HasRepeal)
                {
                    IMessageHandler messageHandler = new MessageHandler();
                    messageHandler.SendRepealPunishNotice(repealDelinquency, creator);
                }
            }
            catch (Exception exception)
            {
                if (!isSupress) tran.RollbackTransaction();
                throw;
            }
            finally { if (!isSupress)tran.Dispose(); }
        }
    }
}