using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
   public class SampleTransactorSampleFactory:SampleFactory
    {
       public SampleTransactorSampleFactory(Guid userId) : base(userId) { }
        public override SamplePrivilige CreateSampePrivlige()
        {
            var privilige = __userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(__userId);
            if (privilige != null) return privilige.ToSamplePrivilige();
            return new SampleTransactorSamplePrivilige();
        }

        public override SampleManager CreateSampleManager()
        {
            return new SampleTransactorManager(__userId);
        }
    }
}
