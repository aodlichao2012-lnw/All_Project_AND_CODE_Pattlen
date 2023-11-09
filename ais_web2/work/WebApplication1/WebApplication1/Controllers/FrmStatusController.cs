
using Jose;
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

namespace ais_web3.Controllers
{
    public class FrmStatusController : Controller
    {
        string session_ID = string.Empty;
        private Module2 module = new Module2();
        string type_db = string.Empty;
        string user_name = string.Empty;
        public FrmStatusController()
        {
             this.StartAsync();
        }
        string Agenids = string.Empty;
        [HttpGet]
        public string Index()
        {
   
            return "";
        }
        [HttpGet]
        public string FrmStatus_Load(string id ="")
        {
            session_ID = id;

            string json = null;
            try
            {
                if (HttpContext.Request.Cookies["Agen" + session_ID] != null)
                    if (HttpContext.Request.Cookies["Agen" + session_ID].Value != null)
                    {
                        Agenids = HttpContext.Request.Cookies["Agen" + session_ID].Value;
                        Module2.Agent_Id = Agenids;
                    }
            }
            catch (Exception ex)
            {
                //WriteLog.instance./*Log*/("Error ที่ FrmStatus_Load : " + ex.Message.ToString());
                //Module2.Agent_Id = "";
            }

        
                json = Get_Project(session_ID);

          
            return json;
        }
        public  string Get_Project(string id)
        {
            try
            {
                session_ID = id;
                if(HttpContext.Request.Cookies["Agen" + session_ID] != null)
                {
                    Agenids = HttpContext.Request.Cookies["Agen" + session_ID].Value;
                    string SQL = "";
                    SQL = "select CNFG_STATUS_CODE.DESCRIPTION  as DESCRIPTION  from CNFG_AGENT_INFO,CNFG_STATUS_CODE  where AGENT_ID = :AGENT_ID AND CNFG_AGENT_INFO.LOGON_EXT= CNFG_STATUS_CODE.STATUS_ID AND ROWNUM = 1";
                    // Conn.Open(SQL, Conn)
                    DataTable dt2 = null;
                    module = new Module2(session_ID);
                    module.Comman_Static2(SQL, new string[] { Agenids }, new string[] { ":AGENT_ID" }, ref dt2);
                    if (dt2 == null)
                    {
                        return "Unknow";
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        if (HttpContext.Request.Cookies["Tel" + session_ID] == null)
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (HttpContext.Request.Cookies["Tel" + session_ID] == null && HttpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("1/1/0001 12:00:00"))
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (HttpContext.Request.Cookies["Tel" + session_ID] != null && HttpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("1/1/0001 12:00:00")) {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (HttpContext.Request.Cookies["Tel" + session_ID] != null && HttpContext.Request.Cookies["Tel" + session_ID].Expires == Convert.ToDateTime("2000/01/01 00:00:00"))
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                        else if (HttpContext.Request.Cookies["Tel" + session_ID] != null && HttpContext.Request.Cookies["Tel" + session_ID].Expires != Convert.ToDateTime("2000/01/01 00:00:00"))
                        {

                            return "Busy";
                        }
                        else
                        {
                            return dt2.Rows[0]["DESCRIPTION"].ToString();
                        }
                      
                    }
                    return "Unknow";
                }
                else
                {
                    return "Not Login";
                }
            
              
            
            }
            catch (Exception ex)
            {
                return "Unknow";
            }
        }

        #region Socket
        private HttpListener _httpListener;
        private async Task AcceptWebSocketAsync(HttpListenerContext context)
        {
            if (context.Request.IsWebSocketRequest)
            {
                WebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);

                using (WebSocket webSocket = webSocketContext.WebSocket)
                {
                    byte[] receiveBuffer = new byte[1024];

                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    while (!result.CloseStatus.HasValue)
                    {
                        // Handle received data
                        string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                        Console.WriteLine($"Received data: {receivedData}");
                      string status =  Get_Project(receivedData);
                        // Send a response

                        byte[] responseBuffer = Encoding.UTF8.GetBytes(status);
                        await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer), WebSocketMessageType.Text, true, CancellationToken.None);

                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    }
                }
            }
        }
        #endregion
        #region เริ่ม Socket
        public async Task StartAsync()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add("https://localhost:44389/Status");
            _httpListener.Start();

            while (true)
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    await AcceptWebSocketAsync(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }
        #endregion
    }
}