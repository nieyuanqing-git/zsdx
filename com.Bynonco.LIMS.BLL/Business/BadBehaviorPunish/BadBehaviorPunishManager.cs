using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    public class BadBehaviorPunishManager:IBadBehaviorPunishManager
    {
         IDelinquencyRuleBLL __delinquencyRuleBLL = BLLFactory.CreateDelinquencyRuleBLL();
         IList<DelinquencyRule> __delinquencyRules;
        public BadBehaviorPunishManager()
        {
            var delinquencyRuleList = __delinquencyRuleBLL.GetEntitiesByExpression("");
            __delinquencyRules = delinquencyRuleList;
            Generate(delinquencyRuleList);
        }
        public BadBehaviorPunishManager(IList<DelinquencyRule> delinquencyRules)
        {
            this.__delinquencyRules = delinquencyRules;
            Generate(delinquencyRules);
        }
        private void Generate(IList<DelinquencyRule> delinquencyRules)
        {
            var minorDelinquencyRule = delinquencyRules.First(p => p.TypeEnum == PunishType.Warning);
            this.MinorAction = minorDelinquencyRule.TypeEnum;
            this.MinorAccumulatePoints = (int)minorDelinquencyRule.TotalSeverity;
            this.MinorPunishDays = minorDelinquencyRule.PersisDays;
            this.MinorMailTemplate = minorDelinquencyRule.MessageTemplate;

            var commonlyDelinquencyRule = delinquencyRules.First(p => p.TypeEnum == PunishType.TutorWarning);
            this.CommonlyAction = commonlyDelinquencyRule.TypeEnum;
            this.CommonlyAccumulatePoints = (int)commonlyDelinquencyRule.TotalSeverity;
            this.CommonlyMailTemplate = commonlyDelinquencyRule.MessageTemplate;
            this.CommonlyPunishDays = commonlyDelinquencyRule.PersisDays;

            var seriousDelinquencyRule = delinquencyRules.First(p => p.TypeEnum == PunishType.Unappointment);
            this.SeriousAccumulatePoints = (int)seriousDelinquencyRule.TotalSeverity;
            this.SeriousAction = seriousDelinquencyRule.TypeEnum;
            this.SeriousMailTemplate = seriousDelinquencyRule.MessageTemplate;
            this.SeriousPunishDays = seriousDelinquencyRule.PersisDays;

            var verySeriousDelinquencyRule = delinquencyRules.First(p => p.TypeEnum == PunishType.Unusable);
            this.VerySeriousAccumulatePoints = (int)verySeriousDelinquencyRule.TotalSeverity;
            this.VerySeriousAction = verySeriousDelinquencyRule.TypeEnum;
            this.VerySeriousMailTemplate = verySeriousDelinquencyRule.MessageTemplate;
            this.VerySeriousPunishDays = verySeriousDelinquencyRule.PersisDays;
        }
        /// <summary>
        /// 轻微处罚操作
        /// </summary>
        public PunishType MinorAction { get;  set; }
        /// <summary>
        /// 轻微处罚累计积分
        /// </summary>
        public int MinorAccumulatePoints { get;  set; }
        /// <summary>
        /// 轻微处罚天数
        /// </summary>
        public int? MinorPunishDays { get;  set; }
        /// <summary>
        /// 轻微邮件模板
        /// </summary>
        public string MinorMailTemplate { get;  set; }


        /// <summary>
        /// 一般处罚操作
        /// </summary>
        public PunishType CommonlyAction { get;  set; }
        /// <summary>
        /// 一般处罚累计积分
        /// </summary>
        public int CommonlyAccumulatePoints { get;  set; }
        /// <summary>
        /// 一般处罚天数
        /// </summary>
        public int? CommonlyPunishDays { get;  set; }
        /// <summary>
        /// 一般邮件模板
        /// </summary>
        public string CommonlyMailTemplate { get;  set; }

        /// <summary>
        /// 严重处罚操作
        /// </summary>
        public PunishType SeriousAction { get;  set; }
        /// <summary>
        /// 严重处罚累计积分
        /// </summary>
        public int SeriousAccumulatePoints { get;  set; }
        /// <summary>
        /// 严重处罚天数
        /// </summary>
        public int? SeriousPunishDays { get;  set; }
        /// <summary>
        /// 严重邮件模板
        /// </summary>
        public string SeriousMailTemplate { get;  set; }


        /// <summary>
        /// 超级严重处罚操作
        /// </summary>
        public PunishType VerySeriousAction { get;  set; }
        /// <summary>
        /// 超级严重处罚累计积分
        /// </summary>
        public int VerySeriousAccumulatePoints { get;  set; }
        /// <summary>
        /// 超级严重处罚天数
        /// </summary>
        public int? VerySeriousPunishDays { get;  set; }
        /// <summary>
        /// 超级严重邮件模板
        /// </summary>
        public string VerySeriousMailTemplate { get;  set; }


        public DelinquencyRule GetDelinquencyRule(int totalSeverity)
        {
            return __delinquencyRules.Where(p => p.TotalSeverity <= totalSeverity).OrderByDescending(p => p.TotalSeverity).FirstOrDefault();
        }

        public string MackRepealContent(DelinquencyConfirm delinquencyConfirm)
        {
            if (delinquencyConfirm == null) throw new ArgumentNullException("不良行为为空");
            if (delinquencyConfirm.HasRepeal) throw new Exception("重复撤销不良行为");
            var punishRecord = delinquencyConfirm.PunishRecord;
            StringBuilder stringBuilder = new StringBuilder();
            var originalDelinquencyRule = GetDelinquencyRule(punishRecord.TotalSeverity );
            var curDelinquencyRule = GetDelinquencyRule(punishRecord.TotalSeverity - delinquencyConfirm.SeverityValue);
            if (curDelinquencyRule == null) curDelinquencyRule = new DelinquencyRule() { TypeEnum = PunishType.None };
            stringBuilder.Append("敬爱的用户您好：\n");
            stringBuilder.AppendFormat("您于{0:yyyy-MM-dd}的不良行为‘{1}’，现已被管理员撤消。\n", delinquencyConfirm.DelinquencyAt, delinquencyConfirm.Cause);
            if (originalDelinquencyRule != null && originalDelinquencyRule.TypeEnum > curDelinquencyRule.TypeEnum)
            {

                stringBuilder.Append("同时，撤消以下：\n");
                var punishActions = delinquencyConfirm.GetPunishActions() == null ? null : delinquencyConfirm.GetPunishActions().Where(x => !x.HasRepeal && x.PunishTypeEnum > curDelinquencyRule.TypeEnum);
                if (punishActions != null && punishActions.Count() > 0)
                {
                    foreach (var punishAction in punishActions)
                    {
                        stringBuilder.AppendFormat("{0:yyyy-MM-dd}的‘{1}’惩罚；\n", punishAction.BeginAt, punishAction.PunishTypeEnum.ToCnName());
                    }
                }
                if (punishRecord.PunishTypeEnum != PunishType.Unusable)
                    stringBuilder.AppendFormat("您当前的处罚状态是：{0}\n", curDelinquencyRule.TypeEnum.ToCnName());
            }
            return stringBuilder.ToString();
        }

        public string MakePunishActionContent(DelinquencyRule delinquencyRule, string userName, PunishRecord punishRecord, DateTime begin, int? days)
        {
            string[] Keys = { "{user}", "{total}", "{value}", "{detail}", "{rule}", "{begin}", "{days}" };
            var confirms = punishRecord.DelinquencyConfirms.Where(x => !x.HasRepeal).Distinct().ToList();
            var detail = new StringBuilder();
            detail.AppendLine("<table border='0' class='edit-table' style='width:100%'><tr><td>时间</td><td>不良行为种类</td><td>严重程度值</td><td>描述</td></tr>");
            confirms.RemoveAt(0);
            foreach (var delinquencyConfirm in confirms)
            {
                if (delinquencyConfirm.HasRepeal) continue;
                detail.AppendLine(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>",
                                                delinquencyConfirm.DelinquencyAt.ToString("yyyy-MM-dd"), delinquencyConfirm.DelinquencyName,
                                                delinquencyConfirm.SeverityValue,
                                                delinquencyConfirm.Cause));
            }
            detail.AppendLine("</table><br/>");
            var rule = new StringBuilder();
            rule.AppendLine("<table border='0' class='edit-table' style='width:100%'><tr><td>处罚类型</td><td>严重程度累计</td><td>处罚持续天数</td></tr>");
            rule.AppendLine(string.Format("<tr><td>{1}</td><td>{0}</td><td></td></tr>", MinorAccumulatePoints, MinorAction.ToCnName()));
            rule.AppendLine(string.Format("<tr><td>{1} </td><td>{0}</td><td></td></tr>", CommonlyAccumulatePoints, CommonlyAction.ToCnName()));
            rule.AppendLine(string.Format("<tr><td>{1}</td><td>{0}</td><td>{2}</td></tr>", SeriousAccumulatePoints, SeriousAction.ToCnName(), SeriousPunishDays.HasValue ? SeriousPunishDays.Value.ToString() : ""));
            rule.AppendLine(string.Format("<tr><td>{1}</td><td>{0}</td><td>{2}</td></tr>", VerySeriousAccumulatePoints, VerySeriousAction.ToCnName(), VerySeriousPunishDays.HasValue ? SeriousPunishDays.Value.ToString() : ""));
            rule.AppendLine("</table>");
            string[] valus = {
                                 userName, delinquencyRule.TotalSeverity.ToString(), punishRecord.TotalSeverity.ToString(),
                                 detail.ToString(),
                                 rule.ToString(),
                                 begin.ToString("yyyy-MM-dd"), 
                                 days.HasValue ? days + "天" : "未定期"
                             };
            int size = 0;
            if (delinquencyRule.TypeEnum == PunishType.TutorWarning || delinquencyRule.TypeEnum == PunishType.Warning)
            {
                size = 5;
            }
            else if (delinquencyRule.TypeEnum == PunishType.Unappointment || delinquencyRule.TypeEnum == PunishType.Unusable)
            {
                size = Keys.Length;
            }
            else return string.Empty;
            string msg = delinquencyRule.MessageTemplate;
            for (int i = 0; i < size; i++)
                msg = msg.Replace(Keys[i], valus[i]);
            return msg;
        }
    }
}