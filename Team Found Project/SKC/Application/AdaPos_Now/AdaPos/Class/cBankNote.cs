using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaPos.Class
{
    public class cBankNote
    {
        public cBankNote()
        {

        }

        /// <summary>
        /// Get banknote
        /// </summary>
        /// <returns></returns>
        public List<cmlTFNMBankNote> C_GETaBanknote()
        {
            StringBuilder oSql;
            List<cmlTFNMBankNote> aoBanknote = new List<cmlTFNMBankNote>();

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT BKN.FTBntCode, BKNL.FTBntName, BKN.FCBntRateAmt");
                oSql.AppendLine("FROM TFNMBankNote BKN WITH(NOLOCK)");
                oSql.AppendLine("INNER JOIN TFNMBankNote_L BKNL WITH(NOLOCK) ON BKNL.FTRteCode = BKN.FTRteCode ");
                oSql.AppendLine("   AND BKNL.FTBntCode = BKN.FTBntCode AND BKNL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("WHERE FTBntStaShw = '1'");
                oSql.AppendLine("AND BKN.FTRteCode = '" + cVB.tVB_RteCode + "'");

                aoBanknote = new cDatabase().C_GETaDataQuery<cmlTFNMBankNote>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cBankNote", "C_GETaBanknote " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปรับตาม Moshi
            }

            return aoBanknote;
        }
    }
}
