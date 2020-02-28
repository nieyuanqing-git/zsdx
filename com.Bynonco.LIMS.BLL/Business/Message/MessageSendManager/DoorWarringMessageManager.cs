using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Model.Enum;

namespace com.Bynonco.LIMS.BLL
{
    public class DoorWarringMessageManager:GlobalMessageManager
    {
        private string __doorId;
        private DateTime __optTime;
        private string __doorName;
        private string __labName;
        private string __labAddress;
        private string __receiveUserLabels;
        private string __openDoorUserLabel;
        public DoorWarringMessageManager(object[] businessObjs, User sender)
            : base(businessObjs,sender) 
        {
            __doorId = this._businessObjects[0] == null ? null : this._businessObjects[0].ToString();
            __optTime = DateTime.Parse(this._businessObjects[1].ToString());
            __doorName = this._businessObjects[2] == null ? null : this._businessObjects[2].ToString();
            __labName = this._businessObjects[3] == null ? null : this._businessObjects[3].ToString();
            __labAddress = this._businessObjects[4] == null ? null : this._businessObjects[4].ToString();
            __receiveUserLabels = this._businessObjects[5] == null ? null : this._businessObjects[5].ToString();
            __openDoorUserLabel = this._businessObjects[6] == null ? null : this._businessObjects[6].ToString();
        }
        
        protected override MessageTemplateBase CreateMessageTemplate()
        {

            return new DoorWarringMessageTemplate(MessageTemplateType.HTML, __doorId, __optTime, __doorName, __labName, __labAddress, __receiveUserLabels, __openDoorUserLabel, _sender);
        }

        protected override MessageTemplateBase CreateNoHTMLMessageTemplate()
        {
            return new DoorWarringMessageTemplate(MessageTemplateType.NoHTML, __doorId, __optTime, __doorName, __labName, __labAddress, __receiveUserLabels, __openDoorUserLabel, _sender);
        }
    }
}
