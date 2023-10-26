using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml;

namespace IVRCall_Dll_Agen
{
    class ServerListener_
    {
        private HttpListener listener;
        private bool run;
        private bool firstRun = true;
        public string prefixes { get; set; }

        //private const string prefixes = "http://127.0.0.1:5555/LogIn/";

        MTReqBuilder myMTReqBuilder = new MTReqBuilder();
        ContentData myContent = new ContentData();

        public delegate void dlgshowAppend(string args);
        public static event dlgshowAppend AppendToRichEdit;

        public void Start()
        {

            try
            {
                //using (HttpListener listener = new HttpListener()){                      
                listener = new HttpListener();
                if (firstRun)
                {
                    listener.Prefixes.Add(prefixes);
                }

                firstRun = false;
                listener.Start();
                run = true;
                Thread thread = new Thread(new ThreadStart(listen));
                thread.Start();
                //  }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listen()
        {
            while (run)
            {
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                result.AsyncWaitHandle.WaitOne();
            }

            listener.Close();
        }

        private void ListenerCallback(IAsyncResult result)
        {


            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;
            //HttpListenerResponse response = context.Response;

            string contReq = string.Empty;
            ContentData contReply = new ContentData();
            string replyMsg = string.Empty;

            try
            {

                using (HttpListenerResponse response = context.Response)
                {

                    using (StreamReader r = new StreamReader(request.InputStream))
                    {
                        contReq = r.ReadToEnd();
                    }

                    contReply = GetContent(contReq);

                    if (contReply.phoneNo.CompareTo("") != 0)
                    {
                        AppendToRichEdit(contReply.phoneNo.ToString());
                        replyMsg = myMTReqBuilder.Reply_Cuccess(myContent);
                    }
                    else
                    {
                        AppendToRichEdit("Number is Null");
                        replyMsg = myMTReqBuilder.Reply_UnCuccess();
                    }

                    try
                    {

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(replyMsg);
                        response.ContentType = "text/xml";
                        context.Response.ContentLength64 = replyMsg.Length;

                        using (Stream output = context.Response.OutputStream)
                        {
                            output.Write(buffer, 0, buffer.Length);
                            output.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                      //  Console.WriteLine("Request  error:  " + ex);
                    }

                }
            }
            catch (Exception ex)
            {
                // Failed to deserialize request.
                //
                //     FailRequest(context);
                return;
            }
            Thread.CurrentThread.Abort();
            listen();
        }

        public void Stop()
        {
            run = false;
        }

        public ContentData GetContent(string szData)
        {

            if ((szData.IndexOf("TRANS_ID") > -1) || (szData.IndexOf("ONLINESTATUS") > -1))
            {
                // string replyMsg = "";
                //  Boolean status = false;
                myContent = myMTReqBuilder.GetPaser(szData);
            }
            else
            {
                return myContent;
            }

            return myContent;
        }


    }
}
    


