using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using com.Bynonco.LIMS.Model;
using Newtonsoft.Json;
using com.Bynonco.LIMS.BLL.Assist;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL
{
    /// <summary> 中山大学账户业务逻辑处理 </summary>
    public class ZsdxAccountBLL
    {
        private IDictCodeTypeBLL _dictCodeTypeBll;
        private string _xjApiUrl;
        private string _xjClientId;
        private string _xjClientSecret;
        private string _xjPassword;
        private string _xjUser;
        public ZsdxAccountBLL()
        {
            _dictCodeTypeBll = BLLFactory.CreateDictCodeTypeBLL();
            var dictCodeType = _dictCodeTypeBll.GetFirstOrDefaultEntityByExpression(string.Format("Code=ZdSynConfig"));
            if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
            {
                var dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "XjApiUrl");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value))
                {
                    _xjApiUrl = dictCode.Value;
                }
                dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "XjClientId");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value))
                {
                    _xjClientId = dictCode.Value;
                }
                dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "XjClientSecret");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value))
                {
                    _xjClientSecret = dictCode.Value;
                }
                dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "XjPassword");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value))
                {
                    _xjPassword = dictCode.Value;
                }
                dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "XjUser");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value))
                {
                    _xjUser = dictCode.Value;
                }
            }

        }

        /// <summary> 获取Token </summary>
        /// <returns></returns>
        private string GetOauthToken()
        {
            var url =
                string.Format(
                    "{0}api/oauth/token?client_id={1}&client_secret={2}&grant_type=password&username={3}&password={4}",
                    _xjApiUrl, _xjClientId, _xjClientSecret, _xjUser, _xjPassword);
            var body = HttpRequestAssitant.PerformHttpGet(url, true);
            var dicts = JavaScriptConvert.DeserializeObject<Dictionary<string, string>>(body);
            if (dicts.ContainsKey("access_token")) return dicts["access_token"];
            return null;
        }

        // POST数据 
        //{
        //    {
        //      "innerid": "INNERID",
        //      "equipmentid": "EQUIPMENTID",
        //      "userid": "USERID",
        //      "payerid": "PAYERID",
        //      "item": "扣费项目",
        //      "fee": 40,
        //      "usebegintime": "2015-02-12 10:02:30",
        //      "useendtime": "2015-02-12 12:30:39",
        //      "bookingbegintime": "2015-02-12 10:00",
        //      "bookingendtime": "2015-02-12 12:30",
        //      "subject": "课题名称",
        //      "samplecount": 10,
        //      "price": "20元/小时",
        //      "content": "实验内容",
        //      "otherinfo": "其它信息",
        //      "calcduration": 12.5,
        //      "usedduration": 12.5,
        //      "deductId": 12,
        //      "deductType": 1

        //    },
        //    {
        //      "innerid": "INNERID2",
        //      "equipmentid": " EQUIPMENTID ",
        //      "userid": " USERID ",
        //      "payerid": " PAYERID ",
        //      "item": "扣费项目",
        //      "fee": 40,
        //      "usebegintime": "2015-02-12 10:02:30",
        //      "useendtime": "2015-02-12 12:30:39",
        //      "bookingbegintime": "2015-02-12 10:00",
        //      "bookingendtime": "2015-02-12 12:30",
        //      "subject": "课题名称",
        //      "samplecount": 10,
        //      "price": "20元/小时",
        //      "content ": "实验内容",
        //      "otherinfo": "其它信息",
        //      "calcduration": 12.5,
        //      "usedduration": 12.5,
        //      "deductId": 12,
        //      "deductType": 1

        //    }
        //]

        //正常时的返回JSON数据包示例
        //[
        //    {
        //      "errcode": 0,
        //      "errmsg": " ok ",
        //      "innerid": "INNERID",
        //"success": true,
        //      "deductId": 1
        //    },
        //    {
        //      "errcode": 0,
        //      "errmsg": " ok ",
        //      "innerid": "INNERID2",
        //"success": true,
        //      "deductId": 10
        //    }
        // ]

        //失败时的返回JSON数据包示例
        // [
        //    { 
        //      "errcode": 0,     
        //      "errmsg": "",
        //"innerid": "INNERID",
        //  "success": true
        //    },
        //{
        //    "errcode": 44001,
        //      "errmsg": "找不到ID为USERID的用户",
        //      "innerid": "INNERID2",
        //      "success": false
        //    }
        // ]
        /// <summary> 扣费 </summary>
        /// <param name="data"> </param>
        /// <returns></returns>
        public DeductResult Deduct(DeductData data)
        {
#if(DEBUG)
            var json = @"
[
            {
              ""errcode"": 0,
              ""errmsg"": "" ok "",
              ""innerid"": ""INNERID"",
              ""success"": true,
              ""deductId"": 1
            },
            {
              ""errcode"": 0,
              ""errmsg"": "" ok "",
              ""innerid"": ""INNERID2"",
              ""success"": true,
              ""deductId"": 10
            }
         ]
";
            return JavaScriptConvert.DeserializeObject<List<DeductResult>>(json);
#else
            var url = string.Format("{0}api/deduct?access_token={1}", _xjApiUrl, GetOauthToken());
            var result = HttpRequestAssitant.WebHttpPost(url, data.ToNameValueCollection());
            return JavaScriptConvert.DeserializeObject<DeductResult>(result);
#endif
        }

        //学生用户账户信息
        //{
        //  "userid": "USERID",
        //  "status": 0,
        //  "list": [
        //    {
        //      "payerid": "PAYERID",
        //      "balance": 1000,
        //      "limit": 1000,
        //      "hasdebts": true,
        //      "outstanding": 2000,
        //      "faculty": "中山医学院"
        //    }
        //  ],
        //  "errcode": 0,
        //  "errmsg": "ok"
        //}

        //导师用户账户信息
        //{
        //  "userid": "USERID",
        //  "status": 0,
        //  "balance": 3000,
        //  "hasdebts": false,
        //  "outstanding": 0,
        //  "faculty": "",
        //  "list": [
        //    {
        //      "payerid": "PAYERID",
        //      "balance": 1000,
        //      "limit": 1000,
        //      "hasdebts": true,
        //      "outstanding": 2000,
        //      "faculty": "中山医学院"
        //    }
        //  ],
        //  "errcode": 0,
        //  "errmsg": "ok"
        //}

        //错误信息
        //{" errcode":44001,"errmsg":"找不到ID为USERID的用户"} 
        /// <summary> 获取帐户信息 </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public XjAccountResultBase GetAccount(string userId)
        {
#if(DEBUG)
            var json =
                @"{
          ""userid"": ""USERID"",
          ""status"": 0,
          ""balance"": 3000,
          ""hasdebts"": false,
          ""outstanding"": 0,
          ""faculty"": """",
          ""list"": [
            {
              ""payerid"": ""PAYERID"",
              ""balance"": 1000,
              ""limit"": 1000,
              ""hasdebts"": true,
              ""outstanding"": 2000,
              ""faculty"": ""中山医学院""
            }
          ],
          ""errcode"": 0,
          ""errmsg"": ""ok""
        }";
            return JavaScriptConvert.DeserializeObject<XjDsAccountResult>(json);
#else
            var url = string.Format("{0}api/getaccount?access_token={1}&userid={2}", _xjApiUrl, GetOauthToken(), System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId)));
            var result = HttpRequestAssitant.PerformHttpGet(url, true);
            return JavaScriptConvert.DeserializeObject<XjDsAccountResult>(result);
#endif
        }

        //正常时的返回JSON数据包示例
        //{
        //  "errcode": 0,
        //  "errmsg": "ok",
        //  "data": [
        //    {
        //      "faculty": "中山医学院",
        //      "equipment": "EQUIPMENT",
        //      "model": "设备规格型号",
        //      "auditdate": "2015-01-22 10:20:30",
        //      "count": 100,
        //      "hours": 200
        //    },
        //    {
        //      "faculty": "公共卫生学院",
        //      "equipment": "EQUIPMENT2",
        //      "model": "设备规格型号",
        //      "auditdate": "2015-01-22",
        //      "count": 100,
        //      "hours": 200
        //    }
        //  ]
        //}
        //失败时的返回JSON数据包示例
        //{" errcode":44001,"errmsg":"找不到ID为USERID的用户"} 
        /// <summary> 获取使用状态 </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public XjUserStat GetUsestat(string userId)
        {
#if(DEBUG)
            var json = @"
{
          ""errcode"": 0,
          ""errmsg"": ""ok"",
          ""data"": [
            {
              ""faculty"": ""中山医学院"",
              ""equipment"": ""EQUIPMENT"",
              ""model"": ""设备规格型号"",
              ""auditdate"": ""2015-01-22 10:20:30"",
              ""count"": 100,
              ""hours"": 200
            },
            {
              ""faculty"": ""公共卫生学院"",
              ""equipment"": ""EQUIPMENT2"",
              ""model"": ""设备规格型号"",
              ""auditdate"": ""2015-01-22"",
              ""count"": 100,
              ""hours"": 200
            }
          ]
        }";
            return JavaScriptConvert.DeserializeObject<XjUserStat>(json);
#else
            var url = string.Format("{0}api/getusestat?access_token={1}&userid={2}", _xjApiUrl, GetOauthToken(), System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userId)));
            var result = HttpRequestAssitant.PerformHttpGet(url, true);
            return JavaScriptConvert.DeserializeObject<XjUserStat>(result);
#endif
        }

        //POST数据
        //{
        //    {
        //      "deductId": "DEDUCTID",
        //      "innerid": " INNERID ",
        //      "equipmentid": " EQUIPMENTID ",
        //      "payerid": "PAYERID"
        //}
        //}

        //正常时的返回JSON数据包示例
        //[
        //{
        //      "errcode": 0,
        //      "errmsg": " ok ",
        //      "innerid": "INNERID",
        //      "success": true,
        //      "deductId": 1
        //    }
        // ]
        //失败时的返回JSON数据包示例
        // [
        //    { 
        //      "errcode": 0,     
        //      "errmsg": "",
        //      "innerid": "INNERID",
        //      "success": true
        //    }
        // ]
        /// <summary> 删除扣费记录 </summary>
        /// <param name="deduct"></param>
        /// <returns></returns>
        public DeductResult DeductDelect(DelDeductData deduct)
        {
#if(DEBUG)
            var json = @"
[
        {
              ""errcode"": 0,
              ""errmsg"": "" ok "",
              ""innerid"": ""INNERID"",
              ""success"": true,
              ""deductId"": 1
            }
         ]
        失败时的返回JSON数据包示例
         [
            { 
              ""errcode"": 0,     
              ""errmsg"": """",
              ""innerid"": ""INNERID"",
              ""success"": true
            }
         ]
";
            return JavaScriptConvert.DeserializeObject<List<DeductResult>>(json);
#else
            var url = string.Format("{0}api/deduct/delect?access_token={1}", _xjApiUrl, GetOauthToken());
            var result = HttpRequestAssitant.WebHttpPost(url, deduct.ToNameValueCollection());
            return JavaScriptConvert.DeserializeObject<DeductResult>(result);
#endif
        }
    }

    /// <summary> 结果状态 </summary>
    public class ResultStatus
    {
        /// <summary> 错误代码 </summary>
        public Errcode errcode { get; set; }
        /// <summary> 提示信息 </summary>
        public string errmsg { get; set; }
    }

    /// <summary> 删除扣费记录对象 </summary>
    public class DelDeductData
    {
        /// <summary> 院级平台对应的扣费记录编号 </summary>
        public string innerid { get; set; }
        /// <summary> 院级平台仪器编号，仪器在院级平台的唯一标识。 </summary>
        public string equipmentid { get; set; }
        /// <summary> 校级平台上ID </summary>
        public string deductId { get; set; }
        /// <summary> 付费人在院级平台的登录名 </summary>
        public string payerid { get; set; }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("innerid", innerid);
            nameValueCollection.Add("equipmentid", equipmentid);
            nameValueCollection.Add("deductId", deductId);
            nameValueCollection.Add("payerid", payerid);
            return nameValueCollection;
        }
    }

    /// <summary> 扣费记录对象 </summary>
    public class DeductData
    {
        public DeductData() { }

        /// <summary> 手工扣费实例化 </summary>
        /// <param name="usedConfirm"></param>
        /// <param name="moneyValidateResult"></param>
        public DeductData(UsedConfirm usedConfirm)
        {
            innerid = usedConfirm.CostDeduct.Id.ToString();
            equipmentid = usedConfirm.EquipmentId.ToString();
            userid = usedConfirm.User.Label;
            payerid = usedConfirm.CostDeduct.UserAccount.GetUser().Label;
            item = usedConfirm.Equipment.Name + "使用扣费";
            fee = (usedConfirm.CostDeduct.RealCurrency ?? 0d) + (usedConfirm.CostDeduct.VirtualCurrency ?? 0d);
            if (usedConfirm.BeginAt.HasValue)
                usebegintime = usedConfirm.BeginAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            if (usedConfirm.EndAt.HasValue)
                useendtime = usedConfirm.EndAt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            var appointment = usedConfirm.GetAppointment();
            if (appointment != null)
            {
                bookingbegintime = appointment.BeginTimeStr;
                bookingendtime = appointment.EndTimeStr;
            }
            subject = usedConfirm.Subject.Name;
            samplecount = usedConfirm.SampleCount.HasValue ? usedConfirm.SampleCount.Value : 0;
            price = usedConfirm.UnitPriceStr;
            content = usedConfirm.Target;
            otherinfo = usedConfirm.Remark;
            calcduration = (usedConfirm.CostDeduct.RealCurrency ?? 0d) + (usedConfirm.CostDeduct.VirtualCurrency ?? 0d);
            usedduration = usedConfirm.Duration;
            //deductId = 0;
            deductType = 0;
        }


        public DeductData(CostDeduct costDeduct)
        {
            IUserBLL _userBll = BLLFactory.CreateUserBLL();
            innerid = costDeduct.Id.ToString();
            equipmentid = string.Empty;
            payerid = costDeduct.UserAccount.GetUser().Label;
            //item = costDeduct.UserAccount.;
            fee = (costDeduct.RealCurrency ?? 0d) + (costDeduct.VirtualCurrency ?? 0d);


            //subject = costDeduct.Subject == null ? string.Empty : costDeduct.Subject.Name;
            samplecount = 0;
            //price = costDeduct.Money.ToString();
            //content = costDeduct.Name;
            //otherinfo = costDeduct.Remark;
            calcduration = (costDeduct.RealCurrency ?? 0d) + (costDeduct.VirtualCurrency ?? 0d);
            //usedduration = costDeduct.;
            //deductId = 0;
            deductType = 1;
            if (costDeduct.SampleApply != null)
            {
                item = costDeduct.SampleApply.TestDemand;
                samplecount = costDeduct.SampleApply.Quatity;
                subject = costDeduct.SampleApply.Subject.Name;
                item = costDeduct.SampleApply.Remark;
                if (costDeduct.SampleApply.CreatorId.HasValue)
                {
                    userid = _userBll.GetEntityById(costDeduct.SampleApply.CreatorId.Value).Label;
                }
                deductType = 2;
            }
            if (costDeduct.ManualCostDeduct != null)
            {
                item = costDeduct.ManualCostDeduct.Name;
                subject = costDeduct.ManualCostDeduct.Subject == null
                              ? string.Empty
                              : costDeduct.ManualCostDeduct.Subject.Name;
                userid = costDeduct.ManualCostDeduct.User.Label;
            }
            if (costDeduct.AnimalCostDeduct != null)
            {
                userid = costDeduct.AnimalCostDeduct.User.Label;
                item = costDeduct.AnimalCostDeduct.Animal.Name + "动物扣费";
            }
            if (costDeduct.Appointment != null)
            {
                userid = costDeduct.Appointment.User.Label;
                item = "预约扣费";
            }
        }

        /// <summary> 院级平台对应的扣费记录编号 </summary>
        public string innerid { get; set; }
        /// <summary> 院级平台仪器编号，仪器在院级平台的唯一标识。 </summary>
        public string equipmentid { get; set; }
        /// <summary> 使用者在院级平台的登录名 </summary>
        public string userid { get; set; }
        /// <summary> 付费人在院级平台的登录名 </summary>
        public string payerid { get; set; }
        /// <summary> 扣费项目 </summary>
        public string item { get; set; }
        /// <summary> 扣除金额 </summary>
        public double fee { get; set; }
        /// <summary> 使用开始时间 格式：2015-02-12 10:02:30 </summary>
        public string usebegintime { get; set; }
        /// <summary> 使用结束时间 格式：2015-02-12 10:02:30 </summary>
        public string useendtime { get; set; }
        /// <summary> 预约开始时间 格式：2015-02-12 10:02:30 </summary>
        public string bookingbegintime { get; set; }
        /// <summary> 预约结束时间 格式：2015-02-12 10:02:30 </summary>
        public string bookingendtime { get; set; }
        /// <summary> 课题名称 </summary>
        public string subject { get; set; }
        /// <summary> 样品数 </summary>
        public int samplecount { get; set; }
        /// <summary> 收费标准 </summary>
        public string price { get; set; }
        /// <summary> 实验内容 </summary>
        public string content { get; set; }
        /// <summary> 其它信息 </summary>
        public string otherinfo { get; set; }
        /// <summary> 收费标准 </summary>
        public double calcduration { get; set; }
        /// <summary> 使用时长 </summary>
        public double usedduration { get; set; }
        /// <summary> 校级平台上ID，如果有值，并且当前的id、innerid、equipmentid三个ID对得上，则表示更新校级平台的扣费记录 </summary>
        public string deductId { get; set; }
        /// <summary> 扣费类型，0：使用扣费，1：手工扣费，2：送样扣费 </summary>
        public int deductType { get; set; }

        public NameValueCollection ToNameValueCollection()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("innerid", innerid);
            nameValueCollection.Add("equipmentid", equipmentid);
            nameValueCollection.Add("userid", userid);
            nameValueCollection.Add("payerid", payerid);
            nameValueCollection.Add("item", item);
            nameValueCollection.Add("fee", fee.ToString());
            nameValueCollection.Add("usebegintime", usebegintime);
            nameValueCollection.Add("useendtime", useendtime);
            nameValueCollection.Add("bookingbegintime", bookingbegintime);
            nameValueCollection.Add("bookingendtime", bookingendtime);
            nameValueCollection.Add("subject", subject);
            nameValueCollection.Add("samplecount", samplecount.ToString());
            nameValueCollection.Add("price", price);
            nameValueCollection.Add("content", content);
            nameValueCollection.Add("otherinfo", otherinfo);
            nameValueCollection.Add("calcduration", calcduration.ToString());
            nameValueCollection.Add("usedduration", usedduration.ToString());
            nameValueCollection.Add("deductId", deductId);
            nameValueCollection.Add("deductType", deductType.ToString());
            return nameValueCollection;
        }
    }

    /// <summary> 扣费返回结果 </summary>
    public class DeductResult : ResultStatus
    {
        /// <summary> 院级平台对应的扣费记录编号 </summary>
        public string innerid { get; set; }
        /// <summary> 是否成功 </summary>
        public bool success { get; set; }
        /// <summary> 提交到校级平台成功后，需要返回并保存校级平台的扣费记录的Id </summary>
        public string deductId { get; set; }
    }

    /// <summary> 账户信息 </summary>
    public class XjAccountResultBase : ResultStatus
    {
        public XjAccountResultBase()
        {
            list = new List<AccountLimitItem>();
        }
        /// <summary> 用户在院级平台的登录名 </summary>
        public string userid { get; set; }
        /// <summary> 用户账号状态，正常：0，禁用：-1 </summary>
        public int status { get; set; }
        /// <summary> 用户限额列表，所属不同课题组可能有不同的扣费导师账户 </summary>
        public List<AccountLimitItem> list { get; set; }
    }

    /// <summary> 导师账户信息 </summary>
    public class XjDsAccountResult : XjAccountResultBase
    {
        /// <summary> 付费人账户余额 </summary>
        public double balance { get; set; }
        /// <summary> 是否有欠款，是：true，否：false </summary>
        public bool hasdebts { get; set; }
        /// <summary> 欠款金额 </summary>
        public double outstanding { get; set; }
        /// <summary> 欠款所属学院 </summary>
        public string faculty { get; set; }
    }

    /// <summary> 账户限额 </summary>
    public class AccountLimitItem
    {
        /// <summary> 付费人在院级平台的登录名 </summary>
        public string payerid { get; set; }
        /// <summary> 付费人账户余额 </summary>
        public double balance { get; set; }
        /// <summary> 用户限额 </summary>
        public double limit { get; set; }
        /// <summary> 是否有欠款，是：true，否：false </summary>
        public bool hasdebts { get; set; }
        /// <summary> 欠款金额 </summary>
        public double outstanding { get; set; }
        /// <summary> 欠款所属学院 </summary>
        public string faculty { get; set; }
    }

    /// <summary> 使用状态 </summary>
    public class XjUserStat : ResultStatus
    {
        /// <summary> 状态列表 </summary>
        public List<XjUseStatItem> data { get; set; }
    }

    /// <summary> 使用统计项 </summary>
    public class XjUseStatItem
    {
        /// <summary> 学院名称 </summary>
        public string faculty { get; set; }
        /// <summary> 设备名称 </summary>
        public string equipment { get; set; }
        /// <summary> 设备规格型号 </summary>
        public string model { get; set; }
        /// <summary> 审核通过时间 </summary>
        public DateTime auditdate { get; set; }
        /// <summary> 使用次数 </summary>
        public int count { get; set; }
        /// <summary> 使用时长 </summary>
        public double hours { get; set; }
    }

    public enum Errcode
    {
        系统繁忙 = -1,
        请求成功 = 0,
        系统错误 = 10001,
        服务暂停 = 10002,
        不合法的凭证类型 = 40001,
        不合法的凭证 = 40002,
        调用接口凭证超时 = 42001,
        需要GET请求 = 43001,
        需要POST请求 = 43002,
        需要DELETE请求 = 43003,
        找不到用户 = 44001,
        找不到付费人 = 44002,
        找不到设备 = 44003,
        扣费余额不足 = 45001,
        扣费账户欠款 = 45002,
        使用者被限制额度_无法扣费 = 45003,
        删除扣费记录失败_已缴费 = 46004,
        删除扣费记录失败_已结算 = 46005,
        找不到扣费记录 = 46001,
        扣费记录不存在 = 46002,
        记录不存在或无法删除 = 46003,
        不允许更新扣费记录 = 47001,
        不允许更新扣费记录_已缴费 = 47002,
        不允许更新扣费记录_已结算 = 47003
    }
}