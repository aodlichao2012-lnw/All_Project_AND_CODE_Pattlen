using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cClientService
    {
        //Install-Package Microsoft.Net.Http -Version 2.0.20710.0
        //private string tC_Uri = "http://wwww.google.com"; // เรียกไปยัง google
        //private string tC_Uri = cVB.tVB_API2PSMaster + "/CheckOnline/IsOnline"; //*Net 63-04-01 ยกมาจาก baseline
        private HttpClient oC_Client;

        public cClientService()
        {
            new cLog().C_WRTxLog("cClientService", "cClientService : Start ", cVB.bVB_AlwPrnLog); //*Net 63-07-30 เพิ่มเปิดปิดตาม Option
            oC_Client = new HttpClient();
            ////ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            //oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            new cLog().C_WRTxLog("cClientService", "cClientService : End ", cVB.bVB_AlwPrnLog); //*Net 63-07-30 เพิ่มเปิดปิดตาม Option
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
            //oC_Client.BaseAddress = new Uri(tC_Uri);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(ptHeaderField, ptHeaderValue);
        }

        public string C_GETtInvoke(string ptFunc, int nTimeOut = 120)
        {
            oC_Client.BaseAddress = new Uri(ptFunc);    //*Em 63-05-24
            //oC_Client.Timeout = TimeSpan.FromSeconds(120);
            oC_Client.Timeout = TimeSpan.FromSeconds(nTimeOut); //*Arm 63-08-06
            return oC_Client.GetStringAsync(ptFunc).Result;
        }

        public HttpResponseMessage C_POSToInvoke(string ptFunc, string ptJson)
        {
            oC_Client.BaseAddress = new Uri(ptFunc);    //*Em 63-05-24
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            return oC_Client.PostAsync(ptFunc, new StringContent(ptJson, Encoding.UTF8, "application/json")).Result;
        }

      

        public HttpResponseMessage C_POSToInvoke(string ptFunc)
        {
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            return oC_Client.PostAsync(ptFunc, new StringContent("", Encoding.UTF8, "application/json")).Result;
        }

        /// <summary>
        /// (*Arm 63-06-12)
        /// </summary>
        /// <param name="ptFunc"></param>
        /// <returns></returns>
        public HttpResponseMessage C_GEToInvoke(string ptFunc)
        {
            oC_Client.BaseAddress = new Uri(ptFunc);
            oC_Client.Timeout = TimeSpan.FromSeconds(120);
            return oC_Client.GetAsync(ptFunc).Result;
        }

        /// <summary>
        /// '*Em 63-07-17
        /// </summary>
        public void C_PRCxCloseConn()
        {
            if (oC_Client != null) oC_Client.Dispose();
        }
    }
}
