using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cWebService
    {
        private string tC_Uri = "http://wwww.google.com"; // เรียกไปยัง google
        private HttpClient oC_Client;

        public cWebService()
        {
            oC_Client = new HttpClient();
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// ส่ง Header ของ Alipay
        /// </summary>
        /// <param name="ptHeaderField">Header Field</param>
        /// <param name="ptHeaderValue">Header Value</param>
        public cWebService(string ptHeaderField, string ptHeaderValue)
        {
            oC_Client = new HttpClient();
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(ptHeaderField, ptHeaderValue);
        }

        public string C_GETtInvoke(string ptFunc)
        {
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
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
