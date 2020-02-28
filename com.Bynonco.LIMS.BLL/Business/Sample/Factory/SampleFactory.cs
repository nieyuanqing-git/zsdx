using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public abstract class SampleFactory
    {
        protected IUserEquipmentPriviligeBLL __userEquipmentPriviligeBLL;
        protected Guid __userId;
        public SampleFactory(Guid userId)
        {
            this.__userId = userId;
            __userEquipmentPriviligeBLL = BLLFactory.CreateUserEquipmentPriviligeBLL();
        }
        public abstract SamplePrivilige CreateSampePrivlige();
        public abstract SampleManager CreateSampleManager();
    }
}
