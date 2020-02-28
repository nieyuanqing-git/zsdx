using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class TesterSampleFactory:SampleFactory
    {
        public TesterSampleFactory(Guid userId) : base(userId) { }
        public override SamplePrivilige CreateSampePrivlige()
        {
            var privilige = __userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(__userId);
            if (privilige != null)
            {
                var sampePrivilige = privilige.ToSamplePrivilige();
                sampePrivilige.IsEnableInputFinishedQuatity = true;
                return sampePrivilige;
            }
            return new TesterSamplePrivilige();
        }

        public override SampleManager CreateSampleManager()
        {
            return new SampleMTesteranager(__userId);
        }
    }
}
