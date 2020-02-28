using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Xml;
using System.Security.Cryptography;
using System.Text;

namespace SSOLab.App1.WebApp
{
    public partial class SSOController : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Params["isSubmit"]) && Request.Params["isSubmit"] == "1")
            {
                try
                {
                    string ssoKey = "XD0cEmXD0IcmYD0gBmYE0OdnYE1jHnZE1USnZF3y3GYpm93Gjp2s2GSog32FfoDm2FZoaG2FZnmhpkVBlJXkWB1eGkWCqA2lWC7k2lXCw1dlXDMqolZr805InrQk4Ixq";
                    string recvInfo = Request.Params["sso_userinfo"];
                    string keyDes = ssoKey.Substring(ssoKey.Length / 2 - 1, 8);
                    string userInfo = DESDecrypt(Request.Params["sso_userinfo"], ssoKey.Substring(ssoKey.Length / 2 - 1, 8));
                    Response.Write(userInfo);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(userInfo);

                    if (xmlDoc.SelectSingleNode("/userinfo/islogin").InnerText == "true")
                    {
                        Session["Label"] = xmlDoc.SelectSingleNode("/userinfo/username").InnerText;
                        Session["UserName"] = xmlDoc.SelectSingleNode("/userinfo/displayname").InnerText;
                        Session["UserOrgName"] = xmlDoc.SelectSingleNode("/userinfo/dptname").InnerText;
                        Response.Redirect("/Account/GFKJDXSSNCheck", true);
                    }
                    else
                    {
                        string returnUrl = GetHostUrl() + "/SSOController.aspx";
                        Response.Redirect(Request.Params["sso_signinurl"] + "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl));
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
        }


        public static string DESDecrypt(string text, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);

            byte[] inputBuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = des.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Encoding.GetEncoding("UTF-8").GetString(outputBuffer);
        }

        public static string GetHostUrl()
        {
            return string.Format("{0}://{1}:{2}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Host,
                HttpContext.Current.Request.Url.Port);
        }
    }
}
