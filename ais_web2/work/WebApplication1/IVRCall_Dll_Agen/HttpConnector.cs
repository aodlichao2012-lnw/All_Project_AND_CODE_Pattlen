using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.IO;
//using System.Windows.Forms;

namespace IVRCall_Dll_Agen
{
    class HttpConnector{

        string urlConecAgen       { get; set; }  
        string portCon              { get; set; }         
        string proxy                   { get; set; }
        string method                { get; set; }
        Int32  conTimeOut          { get; set; }
        Boolean keepAlive          { get; set; }
        string contentType          { get; set; }
        Boolean AutoRedirect      { get; set; }
        // Authen *****
        string username             { get; set; }
        string password             { get; set; }
        string proxyAddress        { get; set; }
        string proxyPort              { get; set; }    

        public HttpConnector() {
           
            this.proxy          = null;
            this.method         = "POST";  
            this.keepAlive      = false;
            this.conTimeOut     = 50000;
            this.contentType    = "text/xml;charset=utf-8";
            this.AutoRedirect   = false;
            Config myConfig = new Config();
            this.portCon = Config.myPortCon.ToString();
            this.urlConecAgen = Config.myUrlConetServer.ToString();
        }      
        //Send Content to all Operator 
        public string SendContent(string sOperaid,string url, string sXML_send){

            string sRespont         = string.Empty;
            HttpWebRequest request  = null;

            request = (HttpWebRequest)WebRequest.Create(urlConecAgen.Replace("{IP}", url).Replace("{PORT}",portCon));
            request.ContentType = contentType;
            request.ContentLength = sXML_send.Length;
            request.Timeout = conTimeOut;
            request.Method = method;
            request.AllowAutoRedirect = false;
            request.Proxy = null;            
            request.KeepAlive = keepAlive;
            
            try {

                using (StreamWriter SW = new StreamWriter(request.GetRequestStream())){                    
                    SW.Write(sXML_send);
                    SW.Close();
                }

            }catch (Exception e) {
                sRespont = string.Format("StreamWriter send content Error: {0}", e.Message);// "Respont Send Request Error: " + e.Message ;
                return sRespont;
            }

            try {

                using ( HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    using (StreamReader SR = new StreamReader(response.GetResponseStream())){

                        sRespont = SR.ReadToEnd();
                        response.Close();
                        SR.Close();//Application.DoEvents();
                    }                
                }
                
            } catch (Exception e) {
                //sRespont = e.Message + "  ::Err_Respont_Send_SMS_response";
                sRespont = string.Format("StreamReader send content Error: {0}", e.Message);
                return sRespont;
            }
            return sRespont;
        }
        public string Encode(string str){
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }
        public string Decode(string str){
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
    }
}
