using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public interface IAutoDeductManager
    {
        void Deduct(out string errorMessage);
    }
}
