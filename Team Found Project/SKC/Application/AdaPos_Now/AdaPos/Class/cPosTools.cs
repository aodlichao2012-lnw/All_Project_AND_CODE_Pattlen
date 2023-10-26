using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public static class cPosTools
    {
        public static void C_SETxStartPosTools()
        {
            string tPath;
            List<string> atPath;
            try
            {
                //C_SETxStopPosTools();
                if (C_CHKbPosToolsRunning()) return;
                atPath = AppDomain.CurrentDomain.BaseDirectory.Split('\\').ToList();
                atPath.RemoveAt(atPath.Count - 2);
                tPath = string.Join("\\", atPath);
                tPath = Directory.GetFiles(tPath, "AdaPosTools.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (String.IsNullOrEmpty(tPath)) return;
                Process.Start(tPath);
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPosTools", "C_SETxStartPosTools : " + oEx.Message);

            }
        }
        public static bool C_CHKbPosToolsRunning()
        {
            Process[] aoProcess;
            try
            {
                aoProcess= Process.GetProcessesByName("AdaPosTools");
                if (aoProcess == null || aoProcess.Length == 0) 
                    return false;


            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPosTools", "C_CHKbPosToolsRunning : " + oEx.Message);

            }
            return true;
        }
        public static void C_SETxStopPosTools()
        {
            Process[] aoProcess;
            try
            {
                aoProcess = Process.GetProcessesByName("AdaPosTools");
                if (aoProcess == null || aoProcess.Length == 0) return;

                foreach (Process oPrc in aoProcess)
                {
                    oPrc.Kill();
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPosTools", "C_SETxStopPosTools : " + oEx.Message);

            }
        }
    }
}
