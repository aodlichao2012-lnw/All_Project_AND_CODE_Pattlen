using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class
{
    class cClientService
    {
        private HttpClient oC_Client;

        public cClientService()
        { 
            oC_Client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public cClientService(string ptHeaderField, string ptHeaderValue)
        {
            oC_Client = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            oC_Client.DefaultRequestHeaders.Add("x-csrf-token", "fetch");
            oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(ptHeaderField, ptHeaderValue);
        }

        public cClientService(List<KeyValuePair<string, string>> paValue)
        {
            oC_Client = new HttpClient();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            oC_Client.Timeout = TimeSpan.FromSeconds(50);
            oC_Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            foreach (KeyValuePair<string, string> aValue in paValue)
            {
                oC_Client.DefaultRequestHeaders.TryAddWithoutValidation(aValue.Key, aValue.Value);
                
            }
        }

        public string C_GETtInvoke(string ptFunc)
        {
            oC_Client.BaseAddress = new Uri(ptFunc);    //*Em 63-05-24
            oC_Client.Timeout = TimeSpan.FromSeconds(120);
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

        public HttpResponseMessage C_GEToInvoke(string ptFunc)
        {
            oC_Client.BaseAddress = new Uri(ptFunc);
            oC_Client.Timeout = TimeSpan.FromSeconds(120);
            return oC_Client.GetAsync(ptFunc).Result;
        }

        public void C_PRCxCloseConn()
        {
            if (oC_Client != null) oC_Client.Dispose();
        }
    }
}

