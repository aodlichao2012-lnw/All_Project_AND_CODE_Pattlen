
using Jose;
using Microsoft.AspNetCore.WebSockets.Server;
using Model_Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Sockets;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.Web.WebSockets;
using PuppeteerSharp;

namespace ais_web3.Controllers
{
    [ApiController]
    public class FrmStatusController : Controller
    {
 
        //[HttpGet]
        //public string Index()
        //{

        //    return "";
        //}
        //[HttpGet]
        //public void FrmStatus_Load(string id = "")
        //{
        //    session_ID = id;

        //    string json = null;
        //    try
        //    {
        //        if (HttpContext.Request.Cookies["Agen" + session_ID] != null)
        //            if (HttpContext.Request.Cookies["Agen" + session_ID].Value != null)
        //            {
        //                Agenids = HttpContext.Request.Cookies["Agen" + session_ID].Value;
        //                Module2.Agent_Id = Agenids;
        //            }
        //        WebScoket();

        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLog.instance./*Log*/("Error ที่ FrmStatus_Load : " + ex.Message.ToString());
        //        //Module2.Agent_Id = "";
        //    }
        //}



  
    //public async void WebScoket()
    //    {
    //        _httpListener = new HttpListener();

    //        foreach (var prefix in _httpListener.Prefixes)
    //        {
    //            if (prefix == "http://localhost:44389/FrmStatus/WebScoket/")
    //            {
    //                continue;
    //            }
    //            else
    //            {
    //                _httpListener.Prefixes.Add($@"http://localhost:44389/FrmStatus/StartAsync/");
    //            }
    //        }
    //        _httpListener.Start();
    //        await StartAsync();
    //    }

    //    private async Task AcceptWebSocketAsync(HttpListenerContext context)
    //    {
    //        if (context.Request.IsWebSocketRequest)
    //        {
    //            WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);

    //            using (WebSocket webSocket = webSocketContext.WebSocket)
    //            {
    //                byte[] receiveBuffer = new byte[1024];
    //                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

    //                while (!result.CloseStatus.HasValue)
    //                {
    //                    // Handle received data
    //                    string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
    //                    Console.WriteLine($"Received data: {receivedData}");
    //                    string status = Get_Project(receivedData);

    //                    // Send a response
    //                    byte[] responseBuffer = Encoding.UTF8.GetBytes(status);
    //                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

    //                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
    //                }
    //            }
    //        }
    //    }
    //    public async Task StartAsync()
    //    {
         

    //        while (true)
    //        {
    //            HttpListenerContext context = await _httpListener.GetContextAsync();
    //            if (context.Request.IsWebSocketRequest)
    //            {
    //                await AcceptWebSocketAsync(context);
    //            }
    //            else
    //            {
    //                context.Response.StatusCode = 400;
    //                context.Response.Close();
    //            }
    //        }
    //    }
    }

    public class ChatHub : Hub
    {
        private HttpContextBase httpContext  = new HttpContextWrapper(HttpContext.Current);
        //public ChatHub(HttpContextBase httpContext_)
        //{
        //    httpContext = httpContext_;
        //}
        //System.Timers.Timer Timer { get; set; }
        //ChatHub(System.Timers.Timer timer)
        //{
        //    Timer = timer;
        //    int interval = 5000;

        //    // Create a timer with the specified interval
        //    Timer = new System.Timers.Timer(interval);

        //    // Hook up the Elapsed event for the timer
        //    Timer.Elapsed += OnTimedEvent;

        //    // Enable the timer
        //    Timer.Enabled = true;
        //}
        public void RequestData(string id)
        {
            // ทำการ query ข้อมูลจากฐานข้อมูลโดยใช้ ID ที่ได้รับ
            var data = GetDataFromDatabase(id.Split(';')[0], id.Split(';')[1]);

            // ส่งข้อมูลกลับไปยังทุกคนที่เชื่อมต่อ
            Clients.Caller.ReceiveData(data);
        }

        public void Send(string id , string Agen)
        {
            // ทำการ query ข้อมูลจากฐานข้อมูลโดยใช้ ID ที่ได้รับ
            var data = GetDataFromDatabase(id ,Agen);

            // ส่งข้อมูลกลับไปยังทุกคนที่เชื่อมต่อ
            Clients.Caller.ReceiveData(data);
        }

        private string GetDataFromDatabase(string id , string Agen)
        {
            // ทำการ query ข้อมูลจากฐานข้อมูลโดยใช้ ID ที่ได้รับ
            // จำลองกระบวนการดึงข้อมูลจากฐานข้อมูลที่นี่
            // ตัวอย่าง: ให้ data เป็นข้อมูลที่ได้จากการ query
            var data = Get_Project(id , Agen);
            return data;
        }

        private HttpListener _httpListener;
        string session_ID = string.Empty;
        private Module2 module = new Module2();
        string type_db = string.Empty;
        string user_name = string.Empty;

        string Agenids = string.Empty;
        public string Get_Project(string id, string Agen)
        {
            try
            {

                    Agenids = Agen;
                    string SQL = "";
                    SQL = "select CNFG_STATUS_CODE.DESCRIPTION  as DESCRIPTION  from CNFG_AGENT_INFO,CNFG_STATUS_CODE  where AGENT_ID = :AGENT_ID AND CNFG_AGENT_INFO.LOGON_EXT= CNFG_STATUS_CODE.STATUS_ID AND ROWNUM = 1";
                    // Conn.Open(SQL, Conn)
                    DataTable dt2 = null;
                    module = new Module2(id);
                    dt2 = module.Comman_Static2(SQL, new string[] { Agenids }, new string[] { ":AGENT_ID" }, dt2);
                    if (dt2 == null)
                    {
                        return "Unknow";
                    }
                    if (dt2.Rows.Count > 0)
                    {

                        if (httpContext.Request.Cookies["Tel" + session_ID] == null)
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (httpContext.Request.Cookies["Tel" + session_ID] == null && httpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("1/1/0001 12:00:00"))
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (httpContext.Request.Cookies["Tel" + session_ID] != null && httpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("1/1/0001 12:00:00"))
                        {
                          

                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (httpContext.Request.Cookies["Tel" + session_ID] != null && httpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("2000/01/01 00:00:00"))
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        } 
                        else if (httpContext.Request.Cookies["Tel" + session_ID] != null && httpContext.Request.Cookies["Tel" + session_ID].Expires != Convert.ToDateTime("2000/01/01 00:00:00"))
                        {
                            return "Busy";
                          
                        }else if(httpContext.Request.Cookies["Tel" + session_ID] != null)
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }

                    else
                    {
                        return "Not Login";
                    }

                    }
           
                else
                {
                    return "Not Login";
                }

                return "Unknow";

            }
            catch (Exception ex)
            {
                return "Unknow";
            }
        }
    }

    //public class Websocket2 : WebSocketHandler
    //{

    //    private static WebSocketCollection clients = new WebSocketCollection();

    //    public Websocket2()
    //    {
    //        clients.Add(this);
    //    }

    //    public override void OnOpen()
    //    {
    //        clients.Broadcast("New client joined the chat!");
    //    }

    //    public override void OnMessage(string message)
    //    {
    //        string status = Get_Project(message);
    //        clients.Broadcast(status);
    //    }

    //    public override void OnClose()
    //    {
    //        clients.Broadcast("Client left the chat!");
    //        clients.Remove(this);
    //    }
    //}
    //public class SendSocket : IHttpHandler
    //{
    //    public bool IsReusable
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    public void ProcessRequest(HttpContext context)
    //    {
    //        if (context.IsWebSocketRequest)
    //        {
    //            context.AcceptWebSocketRequest(new Websocket2());
    //        }
    //    }
    //}
}

