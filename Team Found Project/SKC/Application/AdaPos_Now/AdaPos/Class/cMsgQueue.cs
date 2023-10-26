using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    class cMsgQueue
    {
        public List<cmlTCNTMsgQueue> C_GETaMsgQueue()
        {
            List<cmlTCNTMsgQueue> aoMsgQueue = new List<cmlTCNTMsgQueue>();
            StringBuilder oSql;
            //int nNotRead;
            try
            {
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 10 MSQ.FNMsgID,MSQ.FTMsgQName,MSQ.FTMsgQData,");
                oSql.AppendLine("MSQ.FTMsgStaActive,MSQ.FTMsgStaPrc,MSQ.FTMsgRemark,");
                oSql.AppendLine("MSQ.FDCreateOn,MSQ.FTCreateBy");
                oSql.AppendLine("FROM TCNTMsgQueue MSQ");
                oSql.AppendLine("WHERE MSQ.FTMsgStaPrc != '1' ");
                oSql.AppendLine("AND MSQ.FTMsgQName = 'PS_QMember" + cVB.tVB_ShpCode+"' "); //*Arm 62-10-30
                oSql.AppendLine("ORDER BY MSQ.FDCreateOn DESC");
            aoMsgQueue = new cDatabase().C_GETaDataQuery<cmlTCNTMsgQueue>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgQueue", "C_GETaMsgQueue : " + oEx.Message); }
            finally
            {
                oSql = null;
            }
            return aoMsgQueue;
        }

        public int C_GETnMaxSeq()
        {
            StringBuilder oSql;
            int nSeq = 0;
            try
            {
                
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT");
                oSql.AppendLine("COUNT(FTMsgStaPrc) AS nRow");
                oSql.AppendLine("FROM TCNTMsgQueue WITH(NOLOCK)");
                oSql.AppendLine("WHERE FTMsgStaActive = '1' "); //*Arm 62-11-12
                //oSql.AppendLine("WHERE FTMsgStaPrc = '' "); //*Arm 62-10-30
                oSql.AppendLine("AND FTMsgQName = 'PS_QMember" + cVB.tVB_ShpCode + "'"); //*Arm 62-10-30
                nSeq = new cDatabase().C_GEToDataQuery<int>(oSql.ToString());
                
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgQueue", "C_GETnMaxSeq : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
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
                oSql.AppendLine("UPDATE TCNTMsgQueue SET");
                oSql.AppendLine("FTMsgStaActive = '2' ");
                oSql.AppendLine("WHERE FTMsgQName = 'PS_QMember" + cVB.tVB_ShpCode + "'"); //*Arm 62-10-30

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgQueue", "C_UPDxReadMsg : " + oEx.Message); }
        }

        /// <summary>
        /// Update Read Message
        /// </summary>
        public void C_UPDxPrcMsg()
        {
            StringBuilder oSql;
            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("UPDATE TCNTMsgQueue SET ");
                oSql.AppendLine("FTMsgStaPrc = '1' " );
                oSql.AppendLine("WHERE FNMsgID = '" + cVB.tVB_QMemMsgID + "'");

                new cDatabase().C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cMsgQueue", "C_UPDxPrcMsg : " + oEx.Message); }
        }

    }
}
