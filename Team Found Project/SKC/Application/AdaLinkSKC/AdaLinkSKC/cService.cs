using AdaLinkSKC.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC
{
    public partial class cService : ServiceBase
    {
        public cService()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            this.OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            new cMain().C_PRCxMQProcess();
        }

        protected override void OnStop()
        {
        }
    }
}
