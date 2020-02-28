using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL
{
    public class AnimalRegisterDeathMessageTemplate : MessageTemplateBase
    {
        private ViewAnimal __viewAnimal;
        public AnimalRegisterDeathMessageTemplate(MessageTemplateType messageTemplateType, ViewAnimal viewAnimal,User sender)
            : base(messageTemplateType, sender)
        {
            this.__viewAnimal = viewAnimal;
        }
        protected override MessageContext GenerateMessageContext()
        {
            if (!__viewAnimal.OwnerId.HasValue) return null;
            var owner = _userBLL.GetEntityById(__viewAnimal.OwnerId.Value);
            string title = string.Format("$ReceiverInfo$的动物【{0}】,品系为【{1}】,死亡日期【{2}】,存放位置【{3}】,请登录系统进行死亡确认，若在【{4}】内未进行确认,系统将自动确认。",
                __viewAnimal.Name, __viewAnimal.AnimalCategoryName, __viewAnimal.DieTimeStr, __viewAnimal.AnimalPos, __viewAnimal.ConfirmDeathMaxDays);
            var content = string.Format("动物【{0}】,品系为【{1}】,死亡日期【{2}】,存放位置【{3}】,备注信息【{5}】,请登录系统进行死亡确认，若在【{4}】内未进行确认,系统将自动确认。",
                __viewAnimal.Name, __viewAnimal.AnimalCategoryName, __viewAnimal.DieTimeStr, __viewAnimal.AnimalPos, __viewAnimal.ConfirmDeathMaxDays, __viewAnimal.RegisterDeathRemark);
            return new MessageContext(Model.Enum.MsgType.AnimalRegisterDeath, "", "", title, content, null, _sender,owner, new User[] { owner }, owner, __viewAnimal.Id, null);
        }
    }
}
