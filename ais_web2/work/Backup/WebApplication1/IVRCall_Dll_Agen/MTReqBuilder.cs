using System;
using System.Collections.Generic;

using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace IVRCall_Dll_Agen
{
    class MTReqBuilder{

        En_De_Coder endeCoder =new  En_De_Coder();// = new En_De_Coder();
        ContentData myContent = new ContentData();


        public string Reply_Cuccess(ContentData sData_param)
        {

            string sParame;
            sParame = "HTTP/1.1 200 OK\r\n";// &vbCrLf;
            sParame += "Host: 202.183.231.173\r\n";
            sParame += "Connection: close\r\n";
            sParame += "Content-type:application/x-www-form-urlencoded\r\n\r\n";
           
            sParame += @"<?xml version=""1.0"" encoding=""UTF-8""?>" + "\r\n";
            sParame += "<XML>\r\n";
            sParame += "<STATUS>OK</STATUS>\r\n";
            sParame += "<DETAIL></DETAIL>\r\n";// &vbCrLf;
            sParame += "<PHONE_NO></PHONE_NO>\r\n";// &vbCrLf;
          //  sParame += "<PHONE_NO>" + endeCoder.Encode(sData_param.phoneNo) + "</PHONE_NO>\r\n";// &vbCrLf;
            sParame += "</XML>";

            return sParame;
        }

        public string Reply_UnCuccess()
        {

            string sParame;
            sParame = "HTTP/1.1 200 OK\r\n";// &vbCrLf;
            sParame += "Host: 202.183.231.173\r\n";
            sParame += "Connection: close\r\n";
            sParame += "Content-type:application/x-www-form-urlencoded\r\n\r\n";

            sParame+= @"<?xml version=""1.0"" encoding=""UTF-8""?>" + "\r\n";
            sParame += "<XML>\r\n";
            sParame += "<STATUS>ERROR</STATUS>\r\n";
            sParame += "<DETAIL></DETAIL>\r\n";// &vbCrLf;
            sParame += "<PHONE_NO></PHONE_NO>\r\n";// &vbCrLf;
            sParame += "</XML>";

            return sParame;
        }

        public string SetPaser(ContentData sData_param)
        {

            //  endeCoder = new En_De_Coder();
            //"<XML><TRANS_ID>20090313030249205</TRANS_ID><PHONE_NO>63006C006500530</PHONE_NO>
            // <AIC_ID>00650056003900300031</AIC_ID><AGEN_ID>00650056003900300031</AGEN_ID><IP_TERMINAL>00650056003900300031</IP_TERMINAL>
            //<CALLSTATUS>00650056003900300031</CALLSTATUS><ONLINESTATUS>00650056003900300031</ONLINESTATUS>
            //</XML>";

            string sParame;
            sParame = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + "\r\n";
            sParame += "<XML>";
            sParame += "<TRANS_ID>" + Gen_trandid() + "</TRANS_ID>";
            sParame += "<PHONE_NO>" + endeCoder.Encode(IsDBNull(sData_param.phoneNo)) + "</PHONE_NO>";
            sParame += "<AIC_ID>" + endeCoder.Encode(IsDBNull(sData_param.aicID)) + "</AIC_ID>";//Command  ProcName
            sParame += "<AGEN_ID>" + endeCoder.Encode(IsDBNull(sData_param.agenID)) + "</AGEN_ID>";//Command 
            sParame += "<IP_TERMINAL>" + endeCoder.Encode(IsDBNull(sData_param.ipTerminal)) + "</IP_TERMINAL>";//Command 
            sParame += "<CALLSTATUS>" + endeCoder.Encode(IsDBNull(sData_param.callStatus)) + "</CALLSTATUS>";//Command 
            sParame += "<ONLINESTATUS>" + endeCoder.Encode(IsDBNull(sData_param.onlineStatus)) + "</ONLINESTATUS>";//Command 
            sParame += "<CMD>" + endeCoder.Encode(IsDBNull(sData_param.cmd)) + "</CMD>";//Command 
            sParame += "<CHANNEL>" + endeCoder.Encode(IsDBNull(sData_param.channelNo)) + "</CHANNEL>";//Command 
            sParame += "</XML>";
            return sParame;
        }

        private string IsDBNull(object data)
        {

            if (data == null)
            {
                return "";
            }
            else
            {
                return data.ToString();
            }

        }
        public ContentData GetPaser(string sData_param)
        {
            myContent = new ContentData();
            Hashtable sArrData = new Hashtable();
            XmlDocument doc = new XmlDocument();
            if (sData_param.IndexOf("?>", 1) <= 0)
            {
               // return myContent;
                myContent.contdata = false;// Not have content data
                return myContent;
            }
            string sData_cut = sData_param.Substring(sData_param.IndexOf("?>", 1) + 2, (sData_param.Length - (sData_param.IndexOf("?>", 1) + 2))).Trim();
            doc.Load(new StringReader(sData_cut.Trim()));
            XmlTextReader XmlReader = new XmlTextReader(new StringReader(sData_cut));
            XmlNodeList bookList = doc.GetElementsByTagName("XML");

            foreach (XmlNode node in bookList)
            {
                myContent.tranID = node.ChildNodes.Item(0).InnerText;
                myContent.phoneNo = endeCoder.Decode(node.ChildNodes.Item(1).InnerText);
                myContent.aicID = endeCoder.Decode(node.ChildNodes.Item(2).InnerText);
                myContent.agenID = endeCoder.Decode(node.ChildNodes.Item(3).InnerText);
                myContent.ipTerminal = endeCoder.Decode(node.ChildNodes.Item(4).InnerText);
                myContent.callStatus = endeCoder.Decode(node.ChildNodes.Item(5).InnerText);
                myContent.onlineStatus = endeCoder.Decode(node.ChildNodes.Item(6).InnerText);
                myContent.cmd = endeCoder.Decode(node.ChildNodes.Item(7).InnerText);
                myContent.channelNo = endeCoder.Decode(node.ChildNodes.Item(8).InnerText);   
                            
            }
            myContent.contdata = true;//have content data
            XmlReader.Close();
            return myContent;
        }
        public ContentData GetReplyPaser(string sData_param)
        {
            myContent = new ContentData();
            Hashtable sArrData = new Hashtable();
            XmlDocument doc = new XmlDocument();
            if (sData_param.IndexOf("?>", 1) <= 0)
            {
                // return myContent;
                myContent.contdata = false;// Not have content data
                return myContent;
            }
            string sData_cut = sData_param.Substring(sData_param.IndexOf("?>", 1) + 2, (sData_param.Length - (sData_param.IndexOf("?>", 1) + 2))).Trim();
            doc.Load(new StringReader(sData_cut.Trim()));
            XmlTextReader XmlReader = new XmlTextReader(new StringReader(sData_cut));
            XmlNodeList bookList = doc.GetElementsByTagName("XML");

            foreach (XmlNode node in bookList)
            {
                myContent.status = node.ChildNodes.Item(0).InnerText;
                myContent.detail = node.ChildNodes.Item(1).InnerText;
                myContent.phoneNo = endeCoder.Decode(node.ChildNodes.Item(2).InnerText);

            }
            myContent.contdata = true;//have content data
            XmlReader.Close();
            return myContent;
        }

        private string Gen_trandid()
        {

            string sID = "";
            DateTime sTime = DateTime.Now;

            sID = sTime.ToString("yyyyMMddhhmmss");
            return sID;
        }
       
    }
}
