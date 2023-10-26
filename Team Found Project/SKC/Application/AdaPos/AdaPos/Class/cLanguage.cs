using AdaPos.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaPos.Class
{
    public class cLanguage
    {
        public cLanguage()
        {

        }

        /// <summary>
        /// Get language
        /// </summary>
        /// <param name="pnLngID"></param>
        /// <returns></returns>
        public List<cmlTSysLanguage> C_GETaLanguage()
        {
            StringBuilder oSql;
            List<cmlTSysLanguage> aoLanguage = new List<cmlTSysLanguage>();

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNLngID, FTLngNameEng, FTLngShortName FROM TSysLanguage");
                oSql.AppendLine("WHERE FTLngStaUse = '1'");

                aoLanguage = new cDatabase().C_GETaDataQuery<cmlTSysLanguage>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cLanguage", "C_GETaLanguage " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return aoLanguage;
        }

        /// <summary>
        /// Get language by ID
        /// </summary>
        /// <returns></returns>
        public cmlTSysLanguage C_GEToLanguage()
        {
            StringBuilder oSql;
            cmlTSysLanguage oLanguage = null;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT TOP 1 FNLngID, FTLngNameEng, FTLngShortName FROM TSysLanguage");
                oSql.AppendLine("WHERE FTLngStaUse = '1'");
                oSql.AppendLine("ORDER BY FNLngID");

                oLanguage = new cDatabase().C_GEToDataQuery<cmlTSysLanguage>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cLanguage", "C_GEToLanguage " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return oLanguage;
        }

        /// <summary>
        /// Get language by ID
        /// </summary>
        /// <param name="pnLngID"></param>
        /// <returns></returns>
        public cmlTSysLanguage C_GEToLanguage(int pnLngID)
        {
            StringBuilder oSql;
            cmlTSysLanguage oLanguage = null;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT FNLngID, FTLngNameEng, FTLngShortName FROM TSysLanguage");
                oSql.AppendLine("WHERE FTLngStaUse = '1'");
                oSql.AppendLine("AND FNLngID = " + pnLngID);

                oLanguage = new cDatabase().C_GEToDataQuery<cmlTSysLanguage>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cLanguage", "C_GEToLanguage " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
            }

            return oLanguage;
        }
    }
}
