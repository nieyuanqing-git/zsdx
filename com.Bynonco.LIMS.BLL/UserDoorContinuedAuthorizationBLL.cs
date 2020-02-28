using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model;

using com.Bynonco.LIMS.Model.Business;
using com.august.DataLink;
namespace com.Bynonco.LIMS.BLL
{
    public class UserDoorContinuedAuthorizationBLL : BLLBase<UserDoorContinuedAuthorization>, IUserDoorContinuedAuthorizationBLL
    {
        private ISystemLogBLL __systemLogBLL;
        public UserDoorContinuedAuthorizationBLL()
        {
            __systemLogBLL = BLLFactory.CreateSystemLogBLL();
        }
        public void GenerateEntityLog(OperateType operateType, UserDoorContinuedAuthorization originalEntity, UserDoorContinuedAuthorization curEntity, string operateIP, ref XTransaction tran, bool isSupress)
        {
            string title="", content = "", contentHTML = "", entityName = "", entityCnName = "";
            GenerateEntityLogData(operateType, originalEntity, curEntity,out title, out content, out contentHTML, out entityName, out entityCnName);
            SystemLog systemLog = new SystemLog()
            {
                BusinessId = curEntity.Id,
                Id = Guid.NewGuid(),
                OperateEntityName = entityName,
                OperateEntityCnName = entityCnName,
                OperateTime = DateTime.Now,
                OperateTypeEnum = operateType,
                OperateIP = operateIP,
                LogContent = content,
                LogContentHTML = contentHTML,
                OperatorId = curEntity.Authorizer
            };
            __systemLogBLL.Add(new SystemLog[] { systemLog }, ref tran, isSupress);
        }
        public void GenerateEntityLogData(OperateType operateType, UserDoorContinuedAuthorization originalEntity, UserDoorContinuedAuthorization curEntity,out string title, out string content, out string contentHTML, out string entityName, out string entityCnName)
        {
            entityName = "UserDoorContinuedAuthorization";
            entityCnName = "门禁周期性授权";
            title = string.Format("{0}用户[{1}]门禁[{2}]周期性权限.",
                operateType.ToCnName(),
                curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName,
                curEntity == null || curEntity.Door == null ? "" : curEntity.Door.Name);

            StringBuilder sbContnet = new StringBuilder();
            sbContnet.AppendFormat("编码:【{0}】", curEntity.Id).Append("\r\n");
            sbContnet.AppendFormat("用户:【{0}】", curEntity == null || curEntity.User == null ? "" : curEntity.User.UserName).Append("\r\n");
            sbContnet.AppendFormat("门禁:【{0}】", curEntity == null || curEntity.Door == null ? "" : curEntity.Door.Name).Append("\r\n");
            if (operateType == OperateType.Modified)
            {
                sbContnet.AppendFormat("开始时间:【{0}】→【{1}】", originalEntity == null || !originalEntity.StartTime.HasValue ? "" : originalEntity.StartTime.Value.ToString("HH:mm"), curEntity == null || !curEntity.StartTime.HasValue ? "" : curEntity.StartTime.Value.ToString("HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("结束时间:【{0}】→【{1}】", originalEntity == null || !originalEntity.EndTime.HasValue ? "" : originalEntity.EndTime.Value.ToString("HH:mm"), curEntity == null || !curEntity.EndTime.HasValue ? "" : curEntity.EndTime.Value.ToString("HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("开始年份:【{0}】→【{1}】", originalEntity == null || !originalEntity.StartYear.HasValue ? "" : originalEntity.StartYear.Value.ToString(), curEntity == null || !curEntity.StartYear.HasValue ? "" : curEntity.StartYear.Value.ToString()).Append("\r\n");
                sbContnet.AppendFormat("结束年份:【{0}】→【{1}】", originalEntity == null || !originalEntity.EndYear.HasValue ? "" : originalEntity.EndYear.Value.ToString(), curEntity == null || !curEntity.EndYear.HasValue ? "" : curEntity.EndYear.Value.ToString()).Append("\r\n");
                sbContnet.AppendFormat("月份:【{0}】→【{1}】", originalEntity == null || string.IsNullOrWhiteSpace(originalEntity.Month) ? "" : originalEntity.Month, curEntity == null || string.IsNullOrWhiteSpace(curEntity.Month) ? "" : curEntity.Month).Append("\r\n");
                sbContnet.AppendFormat("星期:【{0}】→【{1}】", originalEntity == null || string.IsNullOrWhiteSpace(originalEntity.Week) ? "" : originalEntity.Week, curEntity == null || string.IsNullOrWhiteSpace(curEntity.Week) ? "" : curEntity.Week).Append("\r\n");
            }
            else
            {
                sbContnet.AppendFormat("开始时间:【{0}】", curEntity == null || !curEntity.StartTime.HasValue ? "" : curEntity.StartTime.Value.ToString("HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("结束时间:【{0}】", curEntity == null || !curEntity.EndTime.HasValue ? "" : curEntity.EndTime.Value.ToString("HH:mm")).Append("\r\n");
                sbContnet.AppendFormat("开始年份:【{0}】", curEntity == null || !curEntity.StartYear.HasValue ? "" : curEntity.StartYear.Value.ToString()).Append("\r\n");
                sbContnet.AppendFormat("结束年份:【{0}】", curEntity == null || !curEntity.EndYear.HasValue ? "" : curEntity.EndYear.Value.ToString()).Append("\r\n");
                sbContnet.AppendFormat("月份:【{0}】", curEntity == null || string.IsNullOrWhiteSpace(curEntity.Month) ? "" : curEntity.Month).Append("\r\n");
                sbContnet.AppendFormat("星期:【{0}】", curEntity == null || string.IsNullOrWhiteSpace(curEntity.Week) ? "" : curEntity.Week).Append("\r\n");
            }
            content = sbContnet.ToString();
            contentHTML = content.Replace("\r\n", "<br />");
        }
    }
}