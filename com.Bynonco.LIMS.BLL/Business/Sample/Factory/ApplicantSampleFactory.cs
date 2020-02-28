using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.Model.View;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.BLL.View;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.IBLL.View;
using com.august.DataLink;
using com.Bynonco.JqueryEasyUI.Core;

namespace com.Bynonco.LIMS.BLL
{
    public class ApplicantSampleFactory:SampleFactory
    {
        public ApplicantSampleFactory(Guid userId) : base(userId) { }
        public override SamplePrivilige CreateSampePrivlige()
        {
            var privilige = __userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(__userId);
            if (privilige != null) return privilige.ToSamplePrivilige();
            return new ApplicantSamplePrivilige();
        }

        public override SampleManager CreateSampleManager()
        {
            return new SampleApplicantManager(__userId);
        }
    }
}
