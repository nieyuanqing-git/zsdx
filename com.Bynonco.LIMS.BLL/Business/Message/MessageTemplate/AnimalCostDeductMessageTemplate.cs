using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.IBLL.View;

namespace com.Bynonco.LIMS.BLL
{
    public class AnimalCostDeductMessageTemplate : MessageTemplateBase
    {
        private IViewAnimalBLL __viewAnimalBLL;
        private ViewAnimal __viewAnimal;
        private AnimalCostDeduct __animalCostDeduct;
        public AnimalCostDeductMessageTemplate(MessageTemplateType messageTemplateType, AnimalCostDeduct animalCostDeduct, User sender)
            : base(messageTemplateType, sender)
        {
            __viewAnimalBLL = BLLFactory.CreateViewAnimalBLL();
            this.__animalCostDeduct = animalCostDeduct;
            this.__viewAnimal = __viewAnimalBLL.GetEntityById(animalCostDeduct.AnimalId);
        }
        protected override MessageContext GenerateMessageContext()
        {
            if (!__viewAnimal.OwnerId.HasValue) return null;
            var owner = _userBLL.GetEntityById(__viewAnimal.OwnerId.Value);
            string content = string.Format("$ReceiverInfo$的动物:{0},品系:{1},存放位置:{2},扣费金额:{3},从{4}~{5},共{6}天",
                __viewAnimal.Name, __viewAnimal.AnimalCategoryName, __viewAnimal.AnimalPos,
                __animalCostDeduct.Money, __animalCostDeduct.BeginDate.ToString("yyyy-MM-dd"), __animalCostDeduct.EndDate.ToString("yyyy-MM-dd"), (__animalCostDeduct.EndDate.Date - __animalCostDeduct.BeginDate.Date).TotalDays);
            return new MessageContext(Model.Enum.MsgType.DeductInform, "", "", "动物扣费消息通知", content, null, _sender,owner, new User[] { owner }, owner, __viewAnimal.Id, null);
        }
    }
}
