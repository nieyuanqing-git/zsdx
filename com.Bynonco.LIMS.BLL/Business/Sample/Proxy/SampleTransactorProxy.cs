using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleTransactorProxy: SampleProxy
    {
        public SampleTransactorProxy(Guid userId)
            : base(userId, new SampleTransactorSampleFactory(userId)) { }
        private IList<DictCodeType> __dictCodeTypes = null;
        public override Model.UserEquipmentPrivilige GetUserEquipmentPrivilige(Model.View.ViewSampleApply viewSampleApply)
        {
            var userSamplePrivilige = viewSampleApply == null ?
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(_user.Id) :
               _userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserItemId(_user.Id, viewSampleApply.SampleItemId);
            if (userSamplePrivilige == null) userSamplePrivilige = _samplePrivilige.ToUserEquipmentPrivilige();
            userSamplePrivilige.IsEnableHandleInner = true;
            userSamplePrivilige.IsEnableHandleOuter = true;
            return userSamplePrivilige;
        }
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply)
        {
            string errorMessage = "";
            if(__dictCodeTypes==null)__dictCodeTypes = _dictCodeTypeBLL.GetEntitiesByExpression("Code=WhichSampleStausDisableEdit");
            if (__dictCodeTypes != null && __dictCodeTypes.Count > 0 && !string.IsNullOrWhiteSpace(__dictCodeTypes.First().Value))
            {
                var statuses = __dictCodeTypes.First().Value.Trim().Replace("，", ",").Split(',');
                if (statuses.Contains(viewSampleApply.Status.ToString())) viewSampleApply.EditOperate = "";
            }

            if (!_viewSampleApplyBLL.JudgeIsEnableInputFinishedQuatity(_user.Id, viewSampleApply.SampleStatus, _samplePrivilige, out errorMessage))
            {
                viewSampleApply.ModifyAmountOperate = "";
            }
        }

        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            StringBuilder sbOperate = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ViewSampleApplyInfoOperate))
                sbOperate.Append(viewSampleApply.ViewSampleApplyInfoOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.SendMessageOperate))
                sbOperate.Append(viewSampleApply.SendMessageOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.EditOperate))
                sbOperate.Append(viewSampleApply.EditOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.AuditOperate))
                sbOperate.Append(viewSampleApply.AuditOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ChargeOperate))
                sbOperate.Append(viewSampleApply.ChargeOperate);
            if(!string.IsNullOrWhiteSpace(viewSampleApply.PrintOperate))
                sbOperate.Append(viewSampleApply.PrintOperate);
            if (!string.IsNullOrWhiteSpace(viewSampleApply.ModifyAmountOperate))
                sbOperate.Append(viewSampleApply.ModifyAmountOperate);
            //if (!string.IsNullOrWhiteSpace(viewSampleApply.PringQrCodeOperate)) 
            //    sbOperate.Append(viewSampleApply.PringQrCodeOperate);
            return sbOperate.ToString();
        }
    }
}
