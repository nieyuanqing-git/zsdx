using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace com.Bynonco.LIMS.BLL.Assist
{
    public class HttpRequestAssitant
    {

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static string PerformHttpGet(string url, bool requireHttp200)
        {
            string responseBody = null;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 5000;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (!requireHttp200 || response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                using (StreamReader responseReader = new StreamReader(responseStream))
                                {
                                    responseBody = responseReader.ReadToEnd();
                                }
                            }
                        }
                    }
                }

            }
            catch (WebException e)
            {
                throw e;
                //using (var responseReader = new StreamReader(e.Response.GetResponseStream()))
                //{
                //    responseBody = responseReader.ReadToEnd();
                //}
                //return responseBody;
            }

            return responseBody;
        }

        public static string PerformHttpPost(string url, string jsonData, bool requireHttp200)
        {
            string responseBody = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml";
            var datas = Encoding.UTF8.GetBytes(jsonData);
            request.ContentLength = datas.Length;

            using (var requestWriter = request.GetRequestStream())
            {
                requestWriter.Write(datas, 0, datas.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var responseReader = new StreamReader(responseStream))
                        {
                            responseBody = responseReader.ReadToEnd();
                        }
                    }
                }
            }

            return responseBody;
        }


        public static string WebHttpPost(string url, System.Collections.Specialized.NameValueCollection datas)
        {
            using (var webClient = new WebClient())
            {
                var result = webClient.UploadValues(url, "POST", datas);
                return System.Text.Encoding.UTF8.GetString(result);
            }
        }
    }
}