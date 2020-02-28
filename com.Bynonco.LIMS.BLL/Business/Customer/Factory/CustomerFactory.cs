using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.Bynonco.LIMS.IBLL;
using com.Bynonco.LIMS.Model;
using com.Bynonco.LIMS.Utility;
using System.Reflection;

namespace com.Bynonco.LIMS.BLL.Business.Customer.Factory
{
    public class CustomerFactory
    {
        private static IDictCodeBLL __dictCodeBLL;
        private static DictCode __customerDictCode;
        private static readonly string __NAMESPACE = "com.Bynonco.LIMS.BLL.Business";
        private static readonly string __ASSEMBLY = "com.Bynonco.LIMS.BLL";
        static CustomerFactory()
        {
            __dictCodeBLL = BLLFactory.CreateDictCodeBLL();
        }
        private static ICutomer __customer;
        public static ICutomer GetCustomer()
        {
            string customerName = "";
            if (__customerDictCode == null)
            {
                __customerDictCode = __dictCodeBLL.GetDictCodeByCode("System", "SchoolName");
            }
            if (__customerDictCode != null && !string.IsNullOrWhiteSpace(__customerDictCode.Value))
            {
                customerName = __customerDictCode.Value.Trim().ToUpper();
            }
            if (__customer == null)
            {
                __customer = new DefaultCustomer();
                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    object obj = null;
                    try
                    {
                        var typeName = ShortcutStringUtility.GetFirstPYLetter(customerName.Trim()).ToUpper() + "Customer";
                        obj = Assembly.Load(__ASSEMBLY).CreateInstance(__NAMESPACE + "." + typeName);
                    }
                    catch { }
                    if (obj != null)
                    {
                        __customer = (ICutomer)obj;
                    }
                }
            }
            return __customer;
        }
    }
}
