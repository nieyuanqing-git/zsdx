using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.IBLL;

namespace com.Bynonco.LIMS.BLL.Business.Privilige
{
    public class SampleApplyPrivilige : PriviligeBase
    {
        private UserWorkScope __userWorkScope = null;
        private IUserWorkScopeBLL __userWorkScopeBLL;
        public Guid? EquipmentId { get; private set; }
        public SampleApplyPrivilige(Guid userId)
            : base(userId)
        {
            __userWorkScopeBLL = BLLFactory.CreateUserWorkScopeBLL();
        }
        public SampleApplyPrivilige(Guid userId, Guid equipmentId)
            : this(userId)
        {
            EquipmentId = equipmentId;
            var userWorkScopeList = __userWorkScopeBLL.GetEntitiesByExpression(string.Format("EquipmentId=\"{0}\"*UserId=\"{1}\"", equipmentId.ToString(), userId.ToString()));
            if (userWorkScopeList != null && userWorkScopeList.Count > 0)
                __userWorkScope = userWorkScopeList.First();
        }
        protected override IList<Model.View.ViewUserWorkGroupModuleFunction> GetUserWorkGroupModuleList()
        {
            throw new NotImplementedException();
        }

        protected override void BuildPrivilige()
        {
            throw new NotImplementedException();
        }
    }
}
