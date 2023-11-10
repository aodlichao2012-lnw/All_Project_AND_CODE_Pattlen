using Microsoft.AspNet.SignalR;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using System.Web.Services.Description;




    [assembly: OwinStartup("StartupConfiguration", typeof(DotNetNuke.Modules.VideoChat.Startup))]
    namespace DotNetNuke.Modules.VideoChat
    {
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                // SignalR Hub Startup
                var hubConfiguration = new HubConfiguration();
                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;
                hubConfiguration.EnableJSONP = false;

                app.MapSignalR(hubConfiguration);


            }
        }
    }
