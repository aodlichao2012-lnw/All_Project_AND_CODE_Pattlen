using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace IVRCall_Dll_Agen
{
   public abstract class IVRCall_Dll_Agen{

        string[] prefixes;
        public  String sMess = "";
        delegate void SetTextCallback(string text);
        MTReqBuilder myMTBuilder = new MTReqBuilder();
        ContentData myContentData = new ContentData();
        HttpConnector myHttpConnect = new HttpConnector();
      

        public IVRCall_Dll_Agen(){

            ServerListener myServer = new ServerListener();
            prefixes = new string[] { "http://127.0.0.1:5555/LogIn/", "http://" + GetIP() + ":5555/LogIn/" };
            myServer.prefixes = prefixes;
            myServer.Start();
            ServerListener.AppendToRichEdit += new ServerListener.dlgshowAppend(this.SetText);
        }
        private String GetIP(){
            String IPStr = "";
            String strHostName = Dns.GetHostName();
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName); // Find host by name

            foreach (IPAddress ipaddress in iphostentry.AddressList){
                IPStr = ipaddress.ToString();
                return IPStr;
            }
            return IPStr;
        }
        public void SetText(string str){
            internalcallback(str);
        }
       public abstract void internalcallback(string str);


       public string GetPhone(string agenID, string ipTerminal)
       {
           myContentData.agenID = agenID;
           myContentData.aicID = "";
           myContentData.callStatus = "";
           myContentData.channelNo = "";
           myContentData.ipTerminal = ipTerminal;
           myContentData.onlineStatus = "";
           myContentData.phoneNo = "";
           myContentData.tranID = "";
           myContentData.cmd = "";

           myContentData.cmd = "Phone";
           myContentData = myMTBuilder.GetReplyPaser(myHttpConnect.SendContent("4",Config.myUrlConetServer, myMTBuilder.SetPaser(myContentData)));
           // myContentData = myMTBuilder.GetReplyPaser(myHttpConnect.SendContent("4", myMTBuilder.SetPaser(myContentData)));
           return myContentData.phoneNo.ToString();           
       }

        // private void SetText(string text)
        //{
        //    if (this.textBox1.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetText);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        this.textBox1.Text += text + Environment.NewLine;
        //    }
        //}
    }
}
