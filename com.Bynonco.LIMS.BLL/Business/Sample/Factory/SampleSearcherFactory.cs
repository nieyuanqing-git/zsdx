﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class SampleSearcherFactory:SampleFactory
    {
        public SampleSearcherFactory(Guid userId) : base(userId) { }
        public override SamplePrivilige CreateSampePrivlige()
        {
            var privilige = __userEquipmentPriviligeBLL.GetUserEquipmentPriviligeByUserId(__userId);
            if (privilige != null) return privilige.ToSamplePrivilige();
            return new SampleSearcherPrivilige();
        }

        public override SampleManager CreateSampleManager()
        {
            return new SampleSearchManager(__userId);
        }
    }
}
