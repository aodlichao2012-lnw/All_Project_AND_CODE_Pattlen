using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Test1.Models { 
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            Clients.Others.receive(message);
        }
    }
}