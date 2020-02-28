using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;


namespace com.Bynonco.LIMS.BLL.SubAreaFileBehavior
{
    public class PersonalSubAreaFileBehavior : SubAreaFileBehaviorBase
    {
        public PersonalSubAreaFileBehavior(User operateUser)
            : base(operateUser)
        {
        }
        public override bool AddValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return true;
        }

        public override bool EditValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return true;
        }

        public override bool DeleteValidates(Model.SubAreaFile subAreaFile, ref string message)
        {
            return true;
        }
    }
}
