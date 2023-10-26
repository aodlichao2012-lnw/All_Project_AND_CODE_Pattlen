using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cClientService
    {
        //Install-Package Microsoft.Net.Http -Version 2.0.20710.0
        //private string tC_Uri = "http://wwww.google.com"; // เรียกไปยัง google
        private string tC_Uri = cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"; //*Net 63-04-01 ยกมาจาก baseline
        private HttpClient oC_Client;

        public cClientService()
        {
            oC_Client = new HttpClient();
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// ส่ง Header ของ Alipay
        /// </summary>
        /// <param name="ptHeaderField">Header Field</param>
        /// <param name="ptHeaderValue">Header Value</param>
        public cClientService(string ptHeaderField, string ptHeaderValue)
        {
            oC_Client = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(ptHeaderField, ptHeaderValue);
        }

        /// <summary>
        /// ส่ง Header มากกว่า 1 //Net 63-05-22
        /// </summary>
        /// <param name="paoHeaders"></param>
        public cClientService(List<KeyValuePair<string, string>> paoHeaders)
        {
            oC_Client = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (KeyValuePair<string, string> oHeader in paoHeaders)
            {
                if (String.IsNullOrEmpty(oHeader.Key) == false)
                    oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(oHeader.Key, oHeader.Value);
            }
        }

        public string C_GETtInvoke(string ptFunc)
        {
            oC_Client.Timeout = TimeSpan.FromSeconds(120);
            return oC_Client.GetStringAsync(ptFunc).Result;
        }

        public HttpResponseMessage C_POSToInvoke(string ptFunc, string ptJson)
        {
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            return oC_Client.PostAsync(ptFunc, new StringContent(ptJson, Encoding.UTF8, "application/json")).Result;
        }



        public HttpResponseMessage C_POSToInvoke(string ptFunc)
        {
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            return oC_Client.PostAsync(ptFunc, new StringContent("", Encoding.UTF8, "application/json")).Result;
        }
    }
}
