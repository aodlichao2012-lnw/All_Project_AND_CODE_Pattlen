using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AdaPos.Class
{
    public class cLog
    {
        private cSP oC_CNSP;

        #region Constructor

        public cLog()
        {
            try
            {
                oC_CNSP = new cSP();
            }
            catch (Exception oEx) { C_WRTxLog("cLog", "cLog " + oEx.Message); }
        }

        #endregion End Constructor

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="ptForm"></param>
        /// <param name="ptFunction"></param>
        public void C_WRTxLog(string ptForm, string ptFunction,bool pbAlwPrn=true) //*Net 63-06-06 เปิด ปิด การพิมพ์ log
        {
            string tPath;

            try
            {
                if (pbAlwPrn == false) return; //*Net 63-06-06 เปิด ปิด การพิมพ์ log
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Log"))
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\Log");

                tPath = System.Windows.Forms.Application.StartupPath + @"\Log\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!File.Exists(tPath))
                    File.Create(tPath).Dispose();

                using (StreamWriter oOutputFile = new StreamWriter(tPath, true))
                {
                    //oOutputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Form (" + ptForm + ") > " + ptFunction);
                    //*Em 63-05-26
                    //string tVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    string tVersion = Application.ProductVersion;   //*Em 63-08-13
                    //string tMsg = tVersion + " : " + DateTime.Now.ToString("HH:mm:ss") + " : Form (" + ptForm + ") > " + ptFunction;
                    string tMsg = tVersion + " : " + DateTime.Now.ToString("HH:mm:ss.fff") + " : Form (" + ptForm + ") > " + ptFunction;
                    oOutputFile.WriteLine(tMsg);
                    //++++++++++++
                    oOutputFile.Dispose();
                }
            }
            catch (Exception oEx) { Debug.WriteLine(oEx.ToString()); }
            finally
            {
                tPath = null;
                ptForm = null;
                ptFunction = null;
                //oC_CNSP.SP_CLExMemory(); //*Net 63-07-30 ปิดตาม Moshi
            }
        }

        public void C_CLRxDataLog()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder(); 
            try
            {
                oSql.AppendLine("DELETE FROM TCNTTmpLogChg WITH(ROWLOCK)");
                oSql.AppendLine("WHERE FTLogStaPrc = '2'");
                oSql.AppendLine("AND CONVERT(VARCHAR(10),FDCreateOn,121) < CONVERT(VARCHAR(10),DATEADD(DAY,-"+ cVB.nVB_TimeClearLog +",GETDATE()),121)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE MSGL");
                oSql.AppendLine("FROM TCNTMsgRemind_L MSGL WITH(ROWLOCK)");
                oSql.AppendLine("INNER JOIN TCNTMsgRemind MSG WITH(NOLOCK) ON MSG.FNMsgID = MSGL.FNMsgID");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),MSG.FDCreateOn,121) < CONVERT(VARCHAR(10),DATEADD(DAY,-" + cVB.nVB_TimeClearLog + ",GETDATE()),121)");
                oSql.AppendLine("");
                oSql.AppendLine("DELETE FROM TCNTMsgRemind WITH(ROWLOCK)");
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDCreateOn,121) < CONVERT(VARCHAR(10),DATEADD(DAY,-" + cVB.nVB_TimeClearLog + ",GETDATE()),121)");
                oDB.C_SETxDataQuery(oSql.ToString());

            }
            catch (Exception oEx) { C_WRTxLog("cLog", "C_CLRxDataLog " + oEx.Message); }
        }
    }
}
