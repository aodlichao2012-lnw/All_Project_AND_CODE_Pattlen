using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
        #if DEBUG

            cService oService = new cService();
            oService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

        #else            
            cService oService = new cService();
            ServiceBase.Run(oService);
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        #endif

            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new cService()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
