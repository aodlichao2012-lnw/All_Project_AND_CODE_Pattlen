using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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
                oC_CNSP.SP_CLExMemory();
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
