using Microsoft.AspNet.SignalR;
using Model_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Models
{
    public class ChatHub : Hub
    {
 
        //public void SendMessage(string message)
        //{
        //    // ส่งข้อความไปยังลูกค้าทั้งหมดที่เชื่อมต่อกับ hub นี้
        //    foreach (var client in Clients.All)
        //    {
        //        string messaghe = Module2.Instance.Get_Project(message);
        //        client.Invoke("ReceiveMessage", message);
        //    }
        //}
    }
}