using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL.View;
using com.Bynonco.LIMS.BLL.Base;
using com.Bynonco.LIMS.Model.View;

namespace com.Bynonco.LIMS.BLL.View
{
    public class ViewSssJBLL : BLLBase<ViewSssJ>, IViewSssJBLL
    {
        public ViewSssJBLL(string connectionName) : base(connectionName) 
        {
        }
    }
}