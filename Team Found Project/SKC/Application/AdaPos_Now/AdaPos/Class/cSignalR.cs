using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cSignalR
    {
        #region Variable

        private HubConnection oC_Connection { get; set; }

        #endregion End Variable

        #region Constructor

        public cSignalR()
        {

        }

        #endregion End Constructor

        /// <summary>
        /// Connect SignalR
        /// </summary>
        public async Task C_CONoSignalRAI()
        {
            if (string.IsNullOrEmpty(cVB.tVB_SgnRPosSrv)) return;   //*Em 62-01-07  WaterPark
            try
            {
                oC_Connection = new HubConnection(cVB.tVB_SgnRPosSrv);
                //cVB.oVB_HubProxyAI = oC_Connection.CreateHubProxy("cChatHub");

                await oC_Connection.Start();
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cSignalR", "C_CONoSignalRAI : " + oEx.Message); }
        }
    }
}
