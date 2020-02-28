namespace com.Bynonco.LIMS.BLL.Business
{
    /// <summary> 北京大学医学部 </summary>
    public class BJDXYXBCustomer : DefaultCustomer
    {
        private readonly string __CODE = "BJDXYXB";

        public override string GetHomeIndex(string xpath)
        {
            return __CODE + "Index";
        }
    }
}