using com.Bynonco.LIMS.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.Bynonco.LIMS.BLL
{
    public class OAuthBLL : IOAuthBLL
    {
        private IDictCodeTypeBLL __dictCodeTypeBLL;
        public OAuthBLL()
        {
            __dictCodeTypeBLL = BLLFactory.CreateDictCodeTypeBLL();
        }
        public void GetOAuthExpirationSettings(out int authorizationCodeExpiration, out int tokenExpiration)
        {
            authorizationCodeExpiration = 600;
            tokenExpiration = 600;
            var dictCodeType = __dictCodeTypeBLL.GetFirstOrDefaultEntityByExpression(string.Format("Code=OAuth"));
            if (dictCodeType != null && dictCodeType.DictCodes != null && dictCodeType.DictCodes.Count > 0)
            {
                int second = -1;
                var dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "TOKEN_EXPIRATION");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value) && int.TryParse(dictCode.Value.Trim(), out second) && second > 0)
                {
                    tokenExpiration = second;
                }
                dictCode = dictCodeType.DictCodes.FirstOrDefault(p => p.Code == "CODE_EXPIRATION");
                if (dictCode != null && !string.IsNullOrEmpty(dictCode.Value) && int.TryParse(dictCode.Value.Trim(), out second) && second > 0)
                {
                    authorizationCodeExpiration = second;
                }
            }
        }
    }
}
