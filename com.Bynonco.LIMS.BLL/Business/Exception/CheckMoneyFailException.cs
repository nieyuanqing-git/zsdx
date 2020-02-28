using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class CheckMoneyFailException : Exception
    {
        private string __errorMessage = "";
        public CheckMoneyFailException(string errorMessage)
        {
            this.__errorMessage = errorMessage;
        }
        public override string Message
        {
            get
            {
                return __errorMessage;
            }
        }
    }
}
