using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public static class cCstScreen
    {
        public static List<string> Blank()
        {
            try
            {

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cCstScreen", "C_GETaListMedia " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return null;
        }
        public static List<cmlAdMsgMedia> C_GETaListAdMsg()
        {
            cDatabase oDB;
            StringBuilder oSql;
            try
            {
                oDB = new cDatabase();
                oSql = new StringBuilder();
                oSql.AppendLine($"SELECT MSG.FTAdvCode AS tAdvCode, MSG.FTAdvType AS tAdvType, ");
                oSql.AppendLine($"MSGL.FTAdvName AS tAdvName, MSGL.FTAdvMsg AS tAdvMsg, ");
                oSql.AppendLine($"MOB.FNMedType AS nMedType, MOB.FTMedFileType AS tMedFileType, ISNULL(MOB.FTMedKey,IOB.FTImgKey) AS tMedKey, ");
                oSql.AppendLine($"ISNULL(MOB.FTMedPath,IOB.FTImgObj) AS tFTMedPath ");
                oSql.AppendLine($"FROM TCNMPosAds POS WITH(NOLOCK) ");
                oSql.AppendLine($"FULL OUTER JOIN TCNMAdMsg MSG WITH(NOLOCK) ON MSG.FTAdvCode = POS.FTAdvCode");
                oSql.AppendLine($" AND '{DateTime.Now:yyyy-MM-dd HH:mm:ss}' >= MSG.FDAdvStart ");
                oSql.AppendLine($" AND '{DateTime.Now:yyyy-MM-dd HH:mm:ss}' <= MSG.FDAdvStop ");
                oSql.AppendLine($"AND MSG.FTAdvStaUse='1'");
                oSql.AppendLine($"INNER JOIN TCNMAdMsg_L MSGL WITH(NOLOCK) ON MSG.FTAdvCode = MSGL.FTAdvCode AND MSGL.FNLngID = {cVB.nVB_Language}");
                oSql.AppendLine($"LEFT JOIN TCNMMediaObj MOB WITH(NOLOCK) ON MSG.FTAdvCode = MOB.FTMedRefID");
                oSql.AppendLine($"LEFT JOIN TCNMImgObj IOB WITH(NOLOCK) ON MSG.FTAdvCode = IOB.FTImgRefID AND FTImgTable='TCNMAdMsg'");
                oSql.AppendLine($"WHERE (POS.FTBchCode='{cVB.tVB_BchCode}' OR ISNULL(POS.FTBchCode,'')='')");
                oSql.AppendLine($"AND (POS.FTPosCode='{cVB.tVB_PosCode}' OR ISNULL(POS.FTPosCode,'')='')");
                oSql.AppendLine("ORDER BY ISNULL(POS.FNPsdSeq,99),MSG.FNAdvSeqNo,MOB.FNMedSeq,IOB.FNImgSeq");

                return oDB.C_GETaDataQuery<cmlAdMsgMedia>(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cCstScreen", "C_GETaListMedia " + oEx.Message);
            }
            finally
            {
                //new cSP().SP_CLExMemory();
            }
            return null;
        }
    }
}
