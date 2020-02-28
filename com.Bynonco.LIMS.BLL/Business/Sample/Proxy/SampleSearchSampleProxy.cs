using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleSearchSampleProxy: SampleProxy
    {
        public SampleSearchSampleProxy(Guid userId)
            : base(userId, new SampleSearcherFactory(userId)) { }
        public override void DoPriviligeFacade(Model.View.ViewSampleApply viewSampleApply) { }

        public override string BuildOperate(Model.View.ViewSampleApply viewSampleApply)
        {
            return viewSampleApply.ViewSampleApplyInfoOperate;
        }
    }
}
