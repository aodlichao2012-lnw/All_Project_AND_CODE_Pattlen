using API2PSSale.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSSale.Class.Online
{
    public class cOnline
    {
        /// <summary>
        /// Class online
        /// </summary>
        public bool C_CHKbOnline(out string ptErrCode, out string ptErrDesc)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            string tResult;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 ISNULL(FTCmpCode,'') AS FTCmpCode FROM TCNMComp WITH (NOLOCK)");
                tResult = oDatabase.C_GETtSQLScalarString(oSql.ToString());
                if (tResult == "")
                {
                    ptErrCode = oMsg.tMS_RespCode905;
                    ptErrDesc = oMsg.tMS_RespDesc905;
                    return false;
                }
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception)
            {
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
    }
}