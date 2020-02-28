using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public interface IBadBehaviorPunishManager
    {
         /// <summary>
        /// 轻微处罚操作
        /// </summary>
         PunishType MinorAction { get;  set; }
        /// <summary>
        /// 轻微处罚累计积分
        /// </summary>
         int MinorAccumulatePoints { get;  set; }
        /// <summary>
        /// 轻微处罚天数
        /// </summary>
         int? MinorPunishDays { get;  set; }
        /// <summary>
        /// 轻微邮件模板
        /// </summary>
         string MinorMailTemplate { get;  set; }


        /// <summary>
        /// 一般处罚操作
        /// </summary>
         PunishType CommonlyAction { get;  set; }
        /// <summary>
        /// 一般处罚累计积分
        /// </summary>
         int CommonlyAccumulatePoints { get;  set; }
        /// <summary>
        /// 一般处罚天数
        /// </summary>
         int? CommonlyPunishDays { get;  set; }
        /// <summary>
        /// 一般邮件模板
        /// </summary>
         string CommonlyMailTemplate { get;  set; }

        /// <summary>
        /// 严重处罚操作
        /// </summary>
         PunishType SeriousAction { get;  set; }
        /// <summary>
        /// 严重处罚累计积分
        /// </summary>
         int SeriousAccumulatePoints { get;  set; }
        /// <summary>
        /// 严重处罚天数
        /// </summary>
         int? SeriousPunishDays { get;  set; }
        /// <summary>
        /// 严重邮件模板
        /// </summary>
         string SeriousMailTemplate { get;  set; }


        /// <summary>
        /// 超级严重处罚操作
        /// </summary>
         PunishType VerySeriousAction { get; set; }
        /// <summary>
        /// 超级严重处罚累计积分
        /// </summary>
         int VerySeriousAccumulatePoints { get;  set; }
        /// <summary>
        /// 超级严重处罚天数
        /// </summary>
         int? VerySeriousPunishDays { get;  set; }
        /// <summary>
        /// 超级严重邮件模板
        /// </summary>
         string VerySeriousMailTemplate { get;  set; }

         DelinquencyRule GetDelinquencyRule(int totalSeverity);
         string MakePunishActionContent(DelinquencyRule delinquencyRule, string userName, PunishRecord punishRecord, DateTime begin, int? days);
         string MackRepealContent(DelinquencyConfirm delinquencyConfirm);
    }
}
