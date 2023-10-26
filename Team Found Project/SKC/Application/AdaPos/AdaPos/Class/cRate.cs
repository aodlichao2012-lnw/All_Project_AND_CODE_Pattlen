using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cRate
    {
        public cRate()
        {

        }

        /// <summary>
        /// ดึงสกุลเงินที่ใช้งาน
        /// </summary>
        public void C_GETxCurrencyLocal()
        {
            List<cmlTFNMRate> aoRate;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FTRteCode, FCRteRate, FTRteType FROM TFNMRate ");
                oSql.AppendLine("WHERE FTRteStaLocal = '1' AND FTRteStaUse = '1' ");

                aoRate = new cDatabase().C_GETaDataQuery<cmlTFNMRate>(oSql.ToString());

                if (aoRate.Count > 0)
                {
                    cVB.tVB_RateCode = aoRate[0].FTRteCode;
                    cVB.cVB_Rate = (decimal)aoRate[0].FCRteRate.SP_CHKcDoubleNull();
                    cVB.tVB_RateType = aoRate[0].FTRteType;
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cRate", "C_GETxCurrencyLocal : " + oEx.Message); }
            finally
            {
                aoRate = null;
                oSql = null;
            }
        }
    }
}
