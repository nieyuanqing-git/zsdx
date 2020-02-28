using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.august.DataLink;
using com.Bynonco.LIMS.BLL;
using com.Bynonco.LIMS.DAL;
using System.Data;
using System.Data.SqlClient;

namespace com.Bynonco.LIMS.BLL
{
    public class GenerateBusinessIdBLL
    {
        /// <summary>
        /// 产生相关业务编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int Generate(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("出错,代码为空");
            IList<IDataParameter> lstDataParameter = new List<IDataParameter>()
                {
                    DataParameterFactory.CreateDataParameter("code",code,ParameterDirection.Input),
                    DataParameterFactory.CreateDataParameter("returnValue",null,ParameterDirection.ReturnValue),
                };
            var reuslt = ProcedureAdapter.ExecuteProcedure("ProGenerateBusinessId", Configuration.DefaultConnectionString.Name, lstDataParameter);
            return (int)reuslt.ReturnValue;
        }
    }
}
