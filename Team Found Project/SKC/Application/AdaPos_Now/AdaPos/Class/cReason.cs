using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cReason
    {
        public cReason()
        {

        }

        /// <summary>
        /// Get reason
        /// </summary>
        /// <returns></returns>
        public List<cmlTCNMRsn> C_GETaReason(string ptRsgCode, string ptRsnText, int pnMode = 0, int pnModeSch = 0)
        {
            List<cmlTCNMRsn> aoRsn = new List<cmlTCNMRsn>();
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT RSNL.FTRsnCode, ISNULL(RSNL.FTRsnName,(SELECT TOP 1 FTRsnName FROM TCNMRsn_L WITH(NOLOCK) WHERE FTRsnCode = RSN.FTRsnCode)) AS FTRsnName");
                oSql.AppendLine("FROM TCNMRsn RSN WITH(NOLOCK)");   //*Em 63-05-15
                oSql.AppendLine("LEFT JOIN TCNMRsn_L RSNL WITH(NOLOCK) ON RSN.FTRsnCode = RSNL.FTRsnCode");    //*Em 63-05-15
                oSql.AppendLine("WHERE RSN.FTRsgCode = '" + ptRsgCode + "'");
                oSql.AppendLine("AND RSNL.FNLngID = " + cVB.nVB_Language);
                if (pnModeSch == 1)
                {
                    switch (pnMode)
                    {
                        case 0:
                            oSql.AppendLine("");
                            break;
                        case 1:     // Search By : Code
                            oSql.AppendLine("AND RSNL.FTRsnCode Like '%" + ptRsnText + "%'");
                            break;

                        case 2:     // Search By : Name
                            oSql.AppendLine("AND RSNL.FTRsnName Like '%" + ptRsnText + "%'");
                            break;
                    }
                }
                else if (pnModeSch == 2)
                {
                    switch (pnMode)
                    {
                        case 0:
                            oSql.AppendLine("");
                            break;
                        case 1:     // Search By : Code
                            oSql.AppendLine("AND RSNL.FTRsnCode = '" + ptRsnText + "'");
                            break;

                        case 2:     // Search By : Name
                            oSql.AppendLine("AND RSNL.FTRsnName = '" + ptRsnText + "'");
                            break;
                    }
                }
                aoRsn = new cDatabase().C_GETaDataQuery<cmlTCNMRsn>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cDepositWithdraw", "C_GETaReason : " + oEx.Message); }
            finally
            {
                ptRsgCode = null;
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoRsn;
        }
    }
}
