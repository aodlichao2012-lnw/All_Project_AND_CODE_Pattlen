using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cMsgRemind
    {
        public cMsgRemind()
        {

        }

        /// <summary>
        /// Get Msg Remind
        /// </summary>
        /// <returns></returns>
        public List<cmlTCNTMsgRemind> C_GETaMsgRemind()
        {
            List<cmlTCNTMsgRemind> aoMsgRemind = new List<cmlTCNTMsgRemind>();
            StringBuilder oSql;
            int nNotRead;
            try
            {
                oSql = new StringBuilder();

                oSql.Clear();
                //oSql.AppendLine("SELECT COUNT(FNMsgID) AS nRow FROM TCNTMsgRemind WHERE FTMsgStaRead != '1'");
                oSql.AppendLine("SELECT COUNT(FNMsgID) AS nRow FROM TCNTMsgRemind WHERE ISNULL(FTMsgStaRead,'') <> '1'");   //*Em 62-09-14
                nNotRead = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
                if (nNotRead > 0)
                {
                    oSql.Clear();
                    oSql.AppendLine("SELECT SYL.FTSynName, FNMsgType, FTMsgData, FDCreateOn");
                    oSql.AppendLine("FROM TCNTMsgRemind MSG");
                    oSql.AppendLine("INNER JOIN TCNTMsgRemind_L MSL ON MSL.FNMsgID = MSG.FNMsgID AND MSL.FNLngID = " + cVB.nVB_Language);
                    //oSql.AppendLine("INNER JOIN TSysSyncData SYN ON SYN.FTSynTable = MSG.FTMsgGroup");
                    //oSql.AppendLine("INNER JOIN TSysSyncData_L SYL ON SYL.FNSynSeqNo = SYN.FNSynSeqNo AND SYL.FNLngID = " + cVB.nVB_Language);
                    oSql.AppendLine("LEFT JOIN TSysSyncData SYN ON SYN.FTSynTable = MSG.FTMsgGroup");   //*Em 62-09-30
                    oSql.AppendLine("LEFT JOIN TSysSyncData_L SYL ON SYL.FNSynSeqNo = SYN.FNSynSeqNo AND SYL.FNLngID = " + cVB.nVB_Language);   //*Em 62-09-30
                    //oSql.AppendLine("WHERE MSG.FTMsgStaRead != '1'");
                    oSql.AppendLine("WHERE ISNULL(MSG.FTMsgStaRead,'') <> '1'");    //*Em 62-09-17
                    oSql.AppendLine("ORDER BY MSG.FDCreateOn DESC");
                } else {
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 10 SYL.FTSynName, FNMsgType, FTMsgData, FDCreateOn");
                    oSql.AppendLine("FROM TCNTMsgRemind MSG");
                    oSql.AppendLine("INNER JOIN TCNTMsgRemind_L MSL ON MSL.FNMsgID = MSG.FNMsgID AND MSL.FNLngID = " + cVB.nVB_Language);
                    //oSql.AppendLine("INNER JOIN TSysSyncData SYN ON SYN.FTSynTable = MSG.FTMsgGroup");
                    //oSql.AppendLine("INNER JOIN TSysSyncData_L SYL ON SYL.FNSynSeqNo = SYN.FNSynSeqNo AND SYL.FNLngID = " + cVB.nVB_Language);
                    oSql.AppendLine("LEFT JOIN TSysSyncData SYN ON SYN.FTSynTable = MSG.FTMsgGroup");  //*Em 62-09-30
                    oSql.AppendLine("LEFT JOIN TSysSyncData_L SYL ON SYL.FNSynSeqNo = SYN.FNSynSeqNo AND SYL.FNLngID = " + cVB.nVB_Language);  //*Em 62-09-30
                    oSql.AppendLine("ORDER BY FDCreateOn DESC");
                }
                aoMsgRemind = new cDatabase().C_GETaDataQuery<cmlTCNTMsgRemind>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgRemind", "C_GETaMsgRemind : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
            return aoMsgRemind;
        }

        /// <summary>
        /// Insert TCNTMsgRemind
        /// </summary>
        public void C_INSxMsgRemind(cmlTCNTMsgRemind poMsg, cmlMsgLng poMsgL)
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("INSERT TCNTMsgRemind WITH(ROWLOCK)");
                oSql.AppendLine("(");
                oSql.AppendLine("   FDCreateOn, FNMsgSeq, FNMsgType,");
                oSql.AppendLine("   FTMsgDocRef, FTMsgGroup, FTCreateBy");
                oSql.AppendLine(")");
                oSql.AppendLine("VALUES");
                oSql.AppendLine("(");
                oSql.AppendLine("   '" + poMsg.FDCreateOn.ToString("yyyy-MM-dd HH:mm:ss") + "', " + poMsg.FNMsgSeq + ", " + poMsg.FNMsgType + ", ");
                oSql.AppendLine("   '" + poMsg.FTMsgDocRef + "', '" + poMsg.FTMsgGroup + "', '" + poMsg.FTCreateBy + "'");
                oSql.AppendLine(");");
                oSql.AppendLine();
                oSql.AppendLine("INSERT TCNTMsgRemind_L WITH(ROWLOCK)");
                oSql.AppendLine("SELECT TOP(1) FNMsgID, 1, '" + poMsgL.ptNameTH + "', '" + poMsgL.ptRmkTH + "'");
                oSql.AppendLine("FROM TCNTMsgRemind WITH(NOLOCK)");
                oSql.AppendLine("ORDER BY FNMsgID DESC");
                oSql.AppendLine();
                oSql.AppendLine("INSERT TCNTMsgRemind_L WITH(ROWLOCK)");
                oSql.AppendLine("SELECT TOP(1) FNMsgID, 2, '" + poMsgL.ptNameEN + "', '" + poMsgL.ptRmkEN + "'");
                oSql.AppendLine("FROM TCNTMsgRemind WITH(NOLOCK)");
                oSql.AppendLine("ORDER BY FNMsgID DESC");
                oSql.AppendLine();

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgRemind", "C_INSxMsgRemind : " + oEx.Message); }
            finally
            {
                oSql = null;
                poMsg = null;
                new cSP().SP_CLExMemory();
            }
        }

        /// <summary>
        /// Get Max Seq
        /// </summary>
        /// <returns></returns>
        public int C_GETnMaxSeq()
        {
            StringBuilder oSql;
            int nSeq = 0;

            try
            {
                //[Pong][2019-01-28][Comment code]
                //oSql = new StringBuilder();
                //oSql.AppendLine("SELECT ISNULL(MAX(FNMsgSeq), 0)");
                //oSql.AppendLine("FROM TCNTMsgRemind WITH(NOLOCK)");

                oSql = new StringBuilder();
                oSql.AppendLine("SELECT");
                oSql.AppendLine("COUNT(ISNULL(FTMsgStaRead, '0')) AS nRow");
                oSql.AppendLine("FROM TCNTMsgRemind WITH(NOLOCK)");
                //oSql.AppendLine("WHERE FTMsgStaRead <> '1'");
                oSql.AppendLine("WHERE ISNULL(FTMsgStaRead,'') <> '1'");   //*Em 62-09-14
                nSeq = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgRemind", "C_GETnMaxSeq : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return nSeq;
        }

        /// <summary>
        /// Update Read Message
        /// </summary>
        public void C_UPDxReadMsg()
        {
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TCNTMsgRemind SET");
                oSql.AppendLine("FTMsgStaRead = '1',");
                oSql.AppendLine("FNMsgSeq = NULL");

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgRemind", "C_UPDxReadMsg : " + oEx.Message); }
        }
    }
}
