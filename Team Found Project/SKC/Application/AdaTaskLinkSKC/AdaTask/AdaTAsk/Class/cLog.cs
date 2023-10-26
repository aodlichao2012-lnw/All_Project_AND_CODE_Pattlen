using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaTask.Class
{
    public class cLog
    {

        #region Constructor

        public cLog()
        {

        }

        #endregion End Constructor

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="ptForm"></param>
        /// <param name="ptFunction"></param>
        public void C_WRTxLog(string ptForm, string ptFunction)
        {
            string tPath;

            try
            {
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Log"))
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\Log");

                tPath = System.Windows.Forms.Application.StartupPath + @"\Log\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!File.Exists(tPath))
                    File.Create(tPath).Dispose();

                using (StreamWriter oOutputFile = new StreamWriter(tPath, true))
                {
                    oOutputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Form (" + ptForm + ") > " + ptFunction);
                    oOutputFile.Dispose();
                }
            }
            catch (Exception oEx) { Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
                ptForm = null;
                ptFunction = null;
                GC.Collect();
            }
        }
    }
}
