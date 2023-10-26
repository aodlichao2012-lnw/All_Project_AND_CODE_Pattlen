using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Xml;

//using System.Windows.Forms;

namespace IVRCall_Dll_Agen
{
   public class ServerListener
    {
        public static HttpListenerContext context;
        public static string contReq = "";
        private HttpListener listener;
        private bool run;
        private bool firstRun = true;
        public string[] prefixes { get; set; }      

        //private const string prefixes = "http://127.0.0.1:5555/LogIn/";

        MTReqBuilder myMTReqBuilder = new MTReqBuilder();
        ContentData myContent = new ContentData();

        public delegate void dlgshowAppend(string args);
        public static event dlgshowAppend AppendToRichEdit;
        //--------Test -----------
       public int Ok = 0;
       public int Err = 0;

        public void Start(){

            try{
                  
                listener = new HttpListener();
                if (!HttpListener.IsSupported)
                {
                    Console.WriteLine("Connect Opent Port 5555");
                    return;
                } 

                if (firstRun) {

                    listener.Prefixes.Clear();
                    foreach (string s in prefixes){
                        listener.Prefixes.Add(s);
                    }                    
                   // listener.Prefixes.Add(prefixes);
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
                Err = Err + 1;
              //  Console.WriteLine("Request  error:  " + ex);
               // MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listen(){

            while (run){
                IAsyncResult result = listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                result.AsyncWaitHandle.WaitOne();                
            }
            listener.Close();
        }

        private void ListenerCallback(IAsyncResult result){

            try{
                    HttpListener listeners = (HttpListener)result.AsyncState;
                     context = listeners.EndGetContext(result);        
                    HttpListenerRequest request = context.Request;                 

                     contReq = string.Empty;
                    ContentData contReply = new ContentData();
                    string replyMsg = string.Empty;
                           

                using (HttpListenerResponse response = context.Response){

                    try{
                            using (StreamReader r = new StreamReader(request.InputStream)){
                                contReq = r.ReadToEnd();
                                r.Close();
                            }
                    }
                    catch (Exception ex)
                    {
                        Err = Err + 1;
                      //  Thread.CurrentThread.Abort();   
                       // Console.WriteLine("Request  error:  " + ex.Message);
                        //return;
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

                    try {

                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(replyMsg);
                        response.ContentType = "text/xml";
                        context.Response.ContentLength64 = replyMsg.Length;

                        using (Stream output = context.Response.OutputStream) {
                            output.Write(buffer, 0, buffer.Length);
                            output.Flush();
                            output.Close();
                            Ok = Ok+ 1;
                        }
                    } catch (Exception ex){
                       // Thread.CurrentThread.Abort();   
                       // Console.WriteLine("Request  error:  " + ex);
                    }
                    response.Close();                 
                }
            }catch (Exception ex)
            {
                Err = Err + 1;
                // Failed to deserialize request.
                //
                //     FailRequest(context);  
               // return;
            }finally {
                //Thread.Sleep(new TimeSpan(0, 0, 5)); 
                //Application.DoEvents(); 
                Thread.CurrentThread.Abort();           
            }           
            listen();
        }

        public void Stop(){
            run = false;           
        }

        public  ContentData GetContent(string szData){

            if ((szData.IndexOf("TRANS_ID") > -1) || (szData.IndexOf("ONLINESTATUS") > -1)){
                myContent = myMTReqBuilder.GetPaser(szData);
            }else{
                return myContent;
            }
            return myContent;
        }


    }
}
    


