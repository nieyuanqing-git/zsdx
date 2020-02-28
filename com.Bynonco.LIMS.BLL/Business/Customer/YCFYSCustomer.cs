using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Business
{
    //抗性基因资源与分子发育北京市重点实验室(别名:遗传发育所)
    public class YCFYSCustomer : DefaultCustomer
    {
        private readonly string __CODE = "YCFYS";
        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
        public override string GetHomeBanner(string xpath)
        {
            return __CODE + "Banner";
        }
        public override string GetHomeSkins(string xpath)
        {
            return __CODE + "Skins";
        }
    }
}
