using AdaPos.Models.Database;
using AdaPos.Models.Other;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cPdt
    {
        public cPdt()
        {

        }

        /// <summary>
        /// Get Product Group
        /// </summary>
        public List<cmlTCNMPdtGrp> C_GETaPdtGroup(int pnStartGrpRow)
        {
            List<cmlTCNMPdtGrp> aoPdtGrp = new List<cmlTCNMPdtGrp>(); ;
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT GRP.FTPgpChain, FTPgpChainName, GRPIMG.FTImgObj");
                oSql.AppendLine("FROM TCNMPdtGrp GRP");
                oSql.AppendLine("INNER JOIN TCNMPdtGrp_L GPL ON GRP.FTPgpChain = GPL.FTPgpChain");
                oSql.AppendLine("AND GPL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMImgPdt GRPIMG ON GRP.FTPgpChain = GRPIMG.FTImgRefID AND FTImgTable = 'TCNMPdtGrp'");
                //oSql.AppendLine("ORDER BY GRP.FTPgpChain DESC ");   //*Arm 62-11-15
                aoPdtGrp = new cDatabase().C_GETaDataQuery<cmlTCNMPdtGrp>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdtGroup : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoPdtGrp;
        }
        //*Net 63-01-13 เพิ่มการรับค่า PdtGroup จากตาราง TCNMPdtTouchGrp
        public List<cmlTCNMPdtTouchGrp> C_GETaPdtTouchGroup(int pnStartGrpRow)
        {
            List<cmlTCNMPdtTouchGrp> aoPdtTouchGrp = new List<cmlTCNMPdtTouchGrp>(); 
            StringBuilder oSql;

            try
            {
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT PTGP.FTTcgCode, PTGPL.FTTcgName, GRPIMG.FTImgObj");
                oSql.AppendLine("FROM TCNMPdtTouchGrp PTGP");
                oSql.AppendLine("INNER JOIN TCNMPdtTouchGrp_L PTGPL ON PTGP.FTTcgCode = PTGPL.FTTcgCode");
                oSql.AppendLine("AND PTGPL.FNLngID = " + cVB.nVB_Language);
                oSql.AppendLine("LEFT JOIN TCNMImgPdt GRPIMG ON PTGP.FTTcgCode = GRPIMG.FTImgRefID AND GRPIMG.FTImgTable = 'TCNMPdtTouchGrp'");

                //oSql.AppendLine("ORDER BY GRP.FTPgpChain DESC ");   //*Arm 62-11-15
                aoPdtTouchGrp = new cDatabase().C_GETaDataQuery<cmlTCNMPdtTouchGrp>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdtTouchGroup : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoPdtTouchGrp;
        }
        public List<cmlPdtDetail> C_GETaPdt(string ptValue)
        {
            List<cmlPdtDetail> aoPdt = new List<cmlPdtDetail>();
            StringBuilder oSql;
            SqlParameter[] aoSqlParam;
            DataTable oDbTblPdt;
            bool bStaPrc;
            try
            {
                //oSql = new StringBuilder();
                ////Zen 63-03-19
                //oSql.AppendLine("SELECT FTPdtCode AS tPdtCode,");
                //oSql.AppendLine("FTBarCode AS tBarcode,");
                //oSql.AppendLine("FTPdtName AS tPdtName,");
                //oSql.AppendLine("FTPunCode AS tPunCode,");
                //oSql.AppendLine("FTPunName AS tUnitName,");
                //oSql.AppendLine("FCPdtUnitFact AS cUnitFactor,");
                //oSql.AppendLine("FCpdtPrice AS cPdtPrice,");
                //oSql.AppendLine("0 nRowCount,");
                //oSql.AppendLine("'' AS tPicPath,");
                //oSql.AppendLine("FTPdtSaleType AS tSaleType,");
                //oSql.AppendLine("FTPdtStaAlwDis AS tStaAlwDis ");
                //oSql.AppendLine("FROM( ");
                //oSql.AppendLine("SELECT DISTINCT TOP 5 PDT.FTPdtCode,PBR.FTBarCode, ");
                //oSql.AppendLine("ISNULL(PDL.FTPdtName,(SELECT TOP 1 FTPdtName FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = PDT.FTPdtCode)) AS FTPdtName, ");
                //oSql.AppendLine("ISNULL(PUL.FTPunName,(SELECT TOP 1 FTPunName FROM TCNMPdtUnit_L WITH(NOLOCK) WHERE FTPunCode = PPS.FTPunCode)) AS FTPunName,PDL.FNLngID, ");
                ////oSql.AppendLine("PUL.FTPunCode, PPS.FCPdtUnitFact,PDTPri.FCpdtPrice, PDT.FTPdtSaleType,PDT.FTPdtStaAlwDis ");
                ////oSql.AppendLine("PUL.FTPunCode, PPS.FCPdtUnitFact,ISNULL(PDTPri.FCpdtPrice,ISNULL(PDTPri.FCpdtPrice,0)) AS FCPdtPrice, PDT.FTPdtSaleType,PDT.FTPdtStaAlwDis");  //*Em 63-04-16
                //oSql.AppendLine("PUL.FTPunCode, PPS.FCPdtUnitFact,ISNULL(GrpPri.FCpdtPrice,ISNULL(PDTPri.FCpdtPrice,0)) AS FCPdtPrice, PDT.FTPdtSaleType,PDT.FTPdtStaAlwDis"); //*Arm 63-04-16
                //oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK) ");
                //oSql.AppendLine("LEFT JOIN TCNMPdt_L PDL WITH(NOLOCK) ON PDT.FTPdtCode = PDL.FTPdtCode ");
                //oSql.AppendLine("INNER JOIN TCNMPdtPackSize PPS WITH(NOLOCK) ON PDT.FTPdtCode = PPS.FTPdtCode ");
                //oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUL WITH(NOLOCK) ON PPS.FTPunCode = PUL.FTPunCode AND PDL.FNLngID = PUL.FNLngID ");
                //oSql.AppendLine("INNER JOIN TCNMPdtBar PBR WITH(NOLOCK) ON PDT.FTPdtCode = PBR.FTPdtCode AND PBR.FTBarStaUse = '1' AND PBR.FTBarStaAlwSale = '1' ");
                //oSql.AppendLine("AND PPS.FTPunCode = PBR.FTPunCode AND PBR.FTBarStaUse = '1' ");
                ////oSql.AppendLine("INNER JOIN  TPSTPdtPrice PDTPri WITH(NOLOCK) ON PDT.FTPdtCode = PDTPri.FTPdtCode ");
                ////oSql.AppendLine("AND PPS.FTPunCode = PDTPri.FTPunCode ");

                ////*Em 63-04-16
                //oSql.AppendLine("LEFT JOIN  TPSTPdtPrice PDTPri WITH(NOLOCK) ON PDT.FTPdtCode = PDTPri.FTPdtCode ");
                //oSql.AppendLine("	AND PPS.FTPunCode = PDTPri.FTPunCode AND (ISNULL(PDTPri.FTPplCode,'') = '')");
                //oSql.AppendLine("LEFT JOIN  TPSTPdtPrice GrpPri WITH(NOLOCK) ON PDT.FTPdtCode = GrpPri.FTPdtCode");
                //oSql.AppendLine("	AND PPS.FTPunCode = GrpPri.FTPunCode AND (ISNULL(GrpPri.FTPplCode,'') = '" + cVB.tVB_PriceGroup + "')");
                ////+++++++++++++++++++++++

                //oSql.AppendLine("WHERE (PDT.FTPdtForSystem = '1' OR (PDT.FTPdtForSystem = '4' AND PDT.FTPdtType = '2' AND PDT.FTPdtSaleType = '2')) ");
                //oSql.AppendLine("AND (PDT.FTPdtCode = '" + ptValue + "' OR PBR.FTBarCode = '" + ptValue + "')  ");
                ////oSql.AppendLine("AND (ISNULL(PDTPri.FTPplCode,'') = '"+ cVB.tVB_PriceGroup +"')");  //*Em 63-03-27
                //oSql.AppendLine(")PDT   ");
                //aoPdt = new cDatabase().C_GETaDataQuery<cmlPdtDetail>(oSql.ToString());
                //*Em 63-03-09
                new cLog().C_WRTxLog("cPdt", "C_GETaPdt : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                //new cLog().C_WRTxLog("cPdt", "C_GETaPdt : call STP_GETaPdtScan Start"); //*Arm 63-05-21
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptPdtValue", SqlDbType.VarChar, 20){ Value = ptValue},
                    new SqlParameter ("@ptPplCode", SqlDbType.VarChar, 20){ Value = cVB.tVB_PriceGroup == null?"":cVB.tVB_PriceGroup},
                    new SqlParameter ("@FNResult", SqlDbType.Int) {
                        Direction = ParameterDirection.Output }
                };
                oDbTblPdt = new DataTable();

                bStaPrc = new cDatabase().C_DATbExecuteQueryStoreProcedure(cVB.oVB_Config, "STP_GETaPdtScan", ref aoSqlParam, ref oDbTblPdt);
                if (bStaPrc)
                {
                    foreach (DataRow oRow in oDbTblPdt.Rows)
                    {
                        cmlPdtDetail oPdt = new cmlPdtDetail();
                        oPdt.tPdtCode = oRow.Field<string>("tPdtCode");
                        oPdt.tBarcode = oRow.Field<string>("tBarcode");
                        oPdt.tPdtName = oRow.Field<string>("tPdtName");
                        oPdt.tPunCode = oRow.Field<string>("tPunCode");
                        oPdt.tUnitName = oRow.Field<string>("tUnitName");
                        oPdt.cUnitFactor = oRow.Field<decimal>("cUnitFactor");
                        oPdt.cPdtPrice = oRow.Field<decimal>("cPdtPrice");
                        oPdt.nRowCount = oRow.Field<int>("nRowCount");
                        oPdt.tPicPath = oRow.Field<string>("tPicPath");
                        oPdt.tSaleType = oRow.Field<string>("tSaleType");
                        oPdt.tStaAlwDis = oRow.Field<string>("tStaAlwDis");
                        oPdt.tPgpChain = oRow.Field<string>("tPgpChain"); // *Arm 63-06-19
                        oPdt.tStkControl = oRow.Field<string>("tStkControl");   //*Em 63-08-14
                        aoPdt.Add(oPdt);
                    }
                }
                //+++++++++++++
                //new cLog().C_WRTxLog("cPdt", "C_GETaPdt : call STP_GETaPdtScan End"); //*Arm 63-05-21
                new cLog().C_WRTxLog("cPdt", "C_GETaPdt : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdt : " + oEx.Message); }
            finally
            {
                oSql = null;
                aoSqlParam = null;
                oDbTblPdt = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }

            return aoPdt;
        }

        public decimal C_GETaPrice(List<cmlPdtDetail> aoPdt)
        {
            //List<cmlPdtDetail> aoPdt = new List<cmlPdtDetail>();
            string tPrice = "";
            cDatabase oDatabase;
            bool bStaPrc = false;
            DataTable oDbTbl;
            decimal cPrice = 0;
            try
            {


                oDatabase = new cDatabase();

                using (SqlConnection conn = oDatabase.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("dbo.STP_GEToPdtPrice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // set up the parameters
                    cmd.Parameters.Add("@ptPdtCode", SqlDbType.VarChar, 25);
                    cmd.Parameters.Add("@ptPunCode", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@ptCstPplCode", SqlDbType.VarChar, 20);
                    cmd.Parameters.Add("@ptBchPplCode", SqlDbType.VarChar, 20);
                    cmd.Parameters.Add("@pdSaleDate", SqlDbType.Date);
                    cmd.Parameters.Add("@FCResult", SqlDbType.Float).Direction = ParameterDirection.Output;

                    // set parameter values
                    cmd.Parameters["@ptPdtCode"].Value = aoPdt[0].tPdtCode;
                    cmd.Parameters["@ptPunCode"].Value = aoPdt[0].tPunCode;
                    cmd.Parameters["@ptCstPplCode"].Value = cVB.tVB_PriceGroup;
                    cmd.Parameters["@ptBchPplCode"].Value = cVB.tVB_BchPriceGroup;
                    cmd.Parameters["@pdSaleDate"].Value = cVB.tVB_SaleDate;

                    // open connection and execute stored procedure

                    cmd.ExecuteNonQuery();

                    // read output value from @FCResult
                    cPrice = Convert.ToInt32(cmd.Parameters["@FCResult"].Value);
                    conn.Close();
                }

            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdt : " + oEx.Message); }
            finally
            {
                oDatabase = null;
                oDbTbl = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return cPrice;
        }
        /// <summary>
        /// Get Product Sale
        /// </summary>
        /// <param name="ptPgpChain">FTPgpChain</param>
        /// <param name="ptValue">Text Search / Scan</param>
        /// <param name="pnMode">0:Show, 1:Scan, 2:Search</param>
        /// <param name="pnSchBy">0:Code, 1:Name, 2:Barcode, 3:Unit, 4:Price</param>
        /// <param name="pnStartRow"></param>
        /// <param name="pbImage"></param>
        /// <param name="pnSort">1:Ascending, 2:Descending</param>
        /// <returns></returns>
        public List<cmlPdtDetail> C_GETaPdtSale(string ptValue, int pnMode, int pnSchBy, int pnStartRow, string ptPgpChain = "", string ptViewPdt = "",int pnSort = 1)
        {
            List<cmlPdtDetail> aoPdt = new List<cmlPdtDetail>();
            StringBuilder oSql;

            try
            {
                new cLog().C_WRTxLog("cPdt", "C_GETaPdtSale : Start", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
                oSql = new StringBuilder();

                //*Arm 63-08-28 Comment Code
                ////*Em 63-02-26  แก้ไขให้ไปใช้ view
                //oSql.AppendLine("SELECT DISTINCT PDT.FTPdtCode AS tPdtCode,");
                //oSql.AppendLine("FTBarCode AS tBarcode,");
                //oSql.AppendLine("FTPdtName AS tPdtName,");
                //oSql.AppendLine("FTPunName AS tUnitName,");
                //oSql.AppendLine("FCPdtUnitFact AS cUnitFactor,");
                ////oSql.AppendLine("(CASE WHEN ISNULL(FTPplCodeCst,'') = '"+ (string.IsNullOrEmpty(cVB.tVB_PriceGroup)?"":cVB.tVB_PriceGroup) + "' THEN FCPdtPriceCst ELSE FCPdtPrice END) AS cPdtPrice,");
                ////oSql.AppendLine("(CASE WHEN ISNULL(FTPplCodeCst,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "' AND ISNULL(FTPplCodeCst,'') <> ''  THEN FCPdtPriceCst ELSE FCPdtPrice END) AS cPdtPrice,");      //*Em 63-03-03
                //oSql.AppendLine("(CASE WHEN ISNULL(PriCst.FTPplCode,'') <> ''  THEN PriCst.FCPdtPrice ELSE PDT.FCPdtPrice END) AS cPdtPrice,"); //*Em 63-04-07
                //oSql.AppendLine("COUNT(*) OVER (PARTITION BY 1) nRowCount,");
                ////oSql.AppendLine("FTImgObj AS tPicPat,");
                //oSql.AppendLine("FTPdtPicPath AS tPicPath,");   //*Em 63-04-22
                //oSql.AppendLine("FTPdtSaleType AS tSaleType,");
                //oSql.AppendLine("FTPgpChain AS tPgpChain,"); //*Arm 63-06-17
                //oSql.AppendLine("FTPdtStaAlwDis AS tStaAlwDis,");
                //oSql.AppendLine("FTPdtStkControl AS tStkControl");    //*Em 63-08-14
                ////oSql.AppendLine("FROM VCN_ProductSale PDT WITH(NOLOCK)");
                //oSql.AppendLine("FROM TPSMPdt PDT WITH(NOLOCK)");   //*Em 63-04-22
                //oSql.AppendLine("LEFT JOIN TPSTPdtPrice PriCst WITH(NOLOCK) ON PDT.FTPdtCode = PriCst.FTPdtCode AND PDT.FTPunCode = PriCst.FTPunCode ");    //*Em 63-04-07
                //oSql.AppendLine("	AND ISNULL(PriCst.FTPplCode,'') <> '' AND PriCst.FTPriType = '1' AND ISNULL(PriCst.FTPplCode,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "'");    //*Em 63-04-07
                ////oSql.AppendLine("WHERE FNLngID =" + cVB.nVB_Language);
                ////if (!string.IsNullOrEmpty(cVB.tVB_PriceGroup))
                ////{
                ////    oSql.AppendLine("AND FTPplCodeCst = '"+ cVB.tVB_PriceGroup + "' AND ISNULL(FTPplCodePdt,'') = ''");
                ////}
                ////else
                ////{
                ////    oSql.AppendLine("AND ISNULL(FTPplCodePdt,'') = ''");
                ////}

                ////if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                ////{
                ////    oSql.AppendLine("AND (FTBchCode = '" + cVB.tVB_BchCode + "' OR ISNULL(FTBchCode,'') = '')");
                ////    oSql.AppendLine("AND ISNULL(FTMerCode,'') ='' AND ISNULL(FTShpCode,'') = ''");
                ////}
                ////else
                ////{
                ////    oSql.AppendLine("AND (ISNULL(FTBchCode,'') = '" + cVB.tVB_BchCode + "' OR ISNULL(FTBchCode,'') = '')");
                ////    oSql.AppendLine("AND ((ISNULL(FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(FTShpCode,'') ='') ");
                ////    oSql.AppendLine("OR (ISNULL(FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(FTShpCode,'') ='" + cVB.tVB_ShpCode + "'))");
                ////}

                //oSql.AppendLine("WHERE 1=1");   //*Em 63-04-22
                //if (!string.IsNullOrEmpty(ptPgpChain))
                //    oSql.AppendLine("AND FTTcgCode = '" + ptPgpChain + "'");   //*Net 63-01-13 เปลี่ยนจากตาราง PdtGrp เป็น PdtTouchGrp

                //if (pnMode == 1)        // Mode Scan
                //    oSql.AppendLine("AND (FTBarCode = '" + ptValue + "' OR PDT.FTPdtCode = '" + ptValue + "')");       // Scan Barcode
                //else    // Mode Search & Show
                //{
                //    if (pnMode == 2)    // Mode Search
                //    {
                //        switch (pnSchBy)
                //        {
                //            case 0:     // Search By : Code
                //                oSql.AppendLine("AND PDT.FTPdtCode LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 1:     // Search By : Name
                //                ptValue = ptValue.Replace(" ", "%");    //*Em 63-08-11
                //                oSql.AppendLine("AND FTPdtName LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 2:     // Search By : Barcode
                //                oSql.AppendLine("AND FTBarCode LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 3:     // Search By : Price
                //                oSql.AppendLine("AND PDT.FCPdtPrice LIKE '%" + ptValue + "%'");  // Search
                //                break;
                //        }
                //    }

                //    if (string.Equals(ptViewPdt, "0"))   // Image
                //        oSql.AppendLine("AND FCPdtUnitFact = 1");           // หน่วยเล็กสุด
                //}
                ////++++++++++++++++++++++++++++++

                //////oSql.AppendLine("ORDER BY PDT.FTPdtCode ");

                ////*Em 62-06-27
                //switch (pnSort)
                //{
                //    case 1:
                //        oSql.AppendLine("ORDER BY tPdtName ASC ");
                //        break;
                //    case 2:
                //        oSql.AppendLine("ORDER BY tPdtName DESC ");
                //        break;
                //}
                ////+++++++++++++
                //oSql.AppendLine("OFFSET " + pnStartRow + " ROWS");
                ////oSql.AppendLine("FETCH NEXT 50 ROWS ONLY;");
                ////oSql.AppendLine("FETCH NEXT  " + cVB.nVB_MaxData + "  ROWS ONLY;"); //*Arm 62-10-16
                //oSql.AppendLine("FETCH NEXT  " + cVB.nVB_PdtPerPage + "  ROWS ONLY;"); //*Arm 62-11-15
                //*Arm 63-08-28 Comment Code End


                //*Arm 63-08-28
                oSql.AppendLine("SELECT tPdtCode, tBarcode, tPdtName, tUnitName, cUnitFactor, ");
                oSql.AppendLine("cPdtPrice, COUNT(*) OVER(PARTITION BY 1) nRowCount, tPicPath, tSaleType, tPgpChain, ");
                oSql.AppendLine("tStaAlwDis, tStkControl, tTcgCode ");
                oSql.AppendLine("FROM ( ");
                oSql.AppendLine("    SELECT DISTINCT TOP " + cVB.nVB_BrwTop + " PDT.FTPdtCode AS tPdtCode, ");
                oSql.AppendLine("       FTBarCode AS tBarcode, ");
                oSql.AppendLine("       FTPdtName AS tPdtName, ");
                oSql.AppendLine("       FTPunName AS tUnitName, ");
                oSql.AppendLine("       FCPdtUnitFact AS cUnitFactor, ");
                oSql.AppendLine("       (CASE WHEN ISNULL(PriCst.FTPplCode, '') <> ''  THEN PriCst.FCPdtPrice ELSE PDT.FCPdtPrice END) AS cPdtPrice, ");
                oSql.AppendLine("       FTPdtPicPath AS tPicPath, ");
                oSql.AppendLine("       FTPdtSaleType AS tSaleType, ");
                oSql.AppendLine("       FTPgpChain AS tPgpChain, ");
                oSql.AppendLine("       FTPdtStaAlwDis AS tStaAlwDis, ");
                oSql.AppendLine("       FTPdtStkControl AS tStkControl, ");
                oSql.AppendLine("       FTTcgCode AS tTcgCode ");
                oSql.AppendLine("   FROM TPSMPdt PDT WITH(NOLOCK) ");
                oSql.AppendLine("   LEFT JOIN TPSTPdtPrice PriCst WITH(NOLOCK) ON PDT.FTPdtCode = PriCst.FTPdtCode AND PDT.FTPunCode = PriCst.FTPunCode ");
                oSql.AppendLine("       AND ISNULL(PriCst.FTPplCode,'') <> '' AND PriCst.FTPriType = '1' AND ISNULL(PriCst.FTPplCode,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "'");
                //*Em 63-09-17
                oSql.AppendLine("WHERE 1=1");   //*Em 63-04-22
                if (!string.IsNullOrEmpty(ptPgpChain))
                    oSql.AppendLine("AND FTTcgCode = '" + ptPgpChain + "'");   //*Net 63-01-13 เปลี่ยนจากตาราง PdtGrp เป็น PdtTouchGrp

                if (pnMode == 1)        // Mode Scan
                    oSql.AppendLine("AND (FTBarCode = '" + ptValue + "' OR PDT.FTPdtCode = '" + ptValue + "')");       // Scan Barcode
                else    // Mode Search & Show
                {
                    if (pnMode == 2)    // Mode Search
                    {
                        switch (pnSchBy)
                        {
                            case 0:     // Search By : Code
                                oSql.AppendLine("AND PDT.FTPdtCode LIKE '%" + ptValue + "%'");  // Search
                                break;

                            case 1:     // Search By : Name
                                ptValue = ptValue.Replace(" ", "%");    //*Em 63-08-11
                                oSql.AppendLine("AND FTPdtName LIKE '%" + ptValue + "%'");  // Search
                                break;

                            case 2:     // Search By : Barcode
                                oSql.AppendLine("AND FTBarCode LIKE '%" + ptValue + "%'");  // Search
                                break;

                            case 3:     // Search By : Price
                                oSql.AppendLine("AND PDT.FCPdtPrice LIKE '%" + ptValue + "%'");  // Search
                                break;
                        }
                    }

                    if (string.Equals(ptViewPdt, "0"))   // Image
                        oSql.AppendLine("AND FCPdtUnitFact = 1");           // หน่วยเล็กสุด
                }
                //+++++++++++++++++
                oSql.AppendLine(") P");
                //oSql.AppendLine("WHERE 1=1");

                //if (!string.IsNullOrEmpty(ptPgpChain))
                //{
                //    oSql.AppendLine("AND tTcgCode = '" + ptPgpChain + "'");
                //}

                //if (pnMode == 1) // Mode Scan
                //{
                //    oSql.AppendLine("AND (tBarcode = '" + ptValue + "' OR tPdtCode = '" + ptValue + "')"); // Scan Barcode
                //}
                //else    // Mode Search & Show
                //{
                //    if (pnMode == 2) // Mode Search
                //    {
                //        switch (pnSchBy)
                //        {
                //            case 0:     // Search By : Code
                //                oSql.AppendLine("AND tPdtCode LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 1:     // Search By : Name
                //                ptValue = ptValue.Replace(" ", "%");    //*Em 63-08-11
                //                oSql.AppendLine("AND tPdtName LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 2:     // Search By : Barcode
                //                oSql.AppendLine("AND tBarcode LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 3:     // Search By : Price
                //                oSql.AppendLine("AND cPdtPrice LIKE '%" + ptValue + "%'");  // Search
                //                break;
                //        }
                //    }
                //    if (string.Equals(ptViewPdt, "0")) // Image
                //        oSql.AppendLine("AND cUnitFactor = 1"); // หน่วยเล็กสุด
                //}
                switch (pnSort)
                {
                    case 1:
                        oSql.AppendLine("ORDER BY tPdtName ASC ");
                        break;
                    case 2:
                        oSql.AppendLine("ORDER BY tPdtName DESC ");
                        break;
                }
                oSql.AppendLine("OFFSET " + pnStartRow + " ROWS");
                oSql.AppendLine("FETCH NEXT  " + cVB.nVB_PdtPerPage + "  ROWS ONLY;");
                //+++++++++++++

                aoPdt = new cDatabase().C_GETaDataQuery<cmlPdtDetail>(oSql.ToString());

                new cLog().C_WRTxLog("cPdt", "C_GETaPdtSale : End", cVB.bVB_AlwPrnLog); //*Arm 63-05-21
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdtSale : " + oEx.Message); }
            finally
            {
                oSql = null;
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
            return aoPdt;
        }

        public void C_PRCxPreparePdtSale()
        {
            cDatabase oDB = new cDatabase();
            int nResult;

            try
            {
                using (SqlConnection conn = oDB.C_CONoDatabase(cVB.oVB_Config))
                using (SqlCommand cmd = new SqlCommand("STP_PRCxPreparePdtSale", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;    //*Em 63-06-02 //*Arm 63-08-28 ปรับจาก 90 เป็น 300 วินาที
 
                    // set up the parameters
                    cmd.Parameters.Add("@ptBchCode", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@ptMerCode", SqlDbType.VarChar, 10);
                    cmd.Parameters.Add("@ptShpCode", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@ptPplCode", SqlDbType.VarChar, 20);
                    cmd.Parameters.Add("@pnLang", SqlDbType.Int);
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.Parameters["@ptBchCode"].Value = cVB.tVB_BchCode;
                    //cmd.Parameters["@ptMerCode"].Value = cVB.tVB_Merchart;
                    //cmd.Parameters["@ptShpCode"].Value = cVB.tVB_ShpCode;
                    cmd.Parameters["@ptMerCode"].Value = cVB.tVB_Merchart == null ? "" : cVB.tVB_Merchart; //*Net 63-08-18
                    cmd.Parameters["@ptShpCode"].Value = cVB.tVB_ShpCode == null ? "" : cVB.tVB_ShpCode; //*Net 63-08-18
                    //cmd.Parameters["@ptPplCode"].Value = cVB.tVB_BchPriceGroup;
                    cmd.Parameters["@ptPplCode"].Value = cVB.tVB_PriceGroup == null ? "" : cVB.tVB_PriceGroup;  //*Em 63-08-17S
                    cmd.Parameters["@pnLang"].Value = cVB.nVB_Language;
                    cmd.ExecuteNonQuery();

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    new cLog().C_WRTxLog("cPdt", "C_PRCxPreparePdtSale : Result =" + nResult, cVB.bVB_AlwPrnLog);
                    conn.Close();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_PRCxPreparePdtSale : " + oEx.Message); }
            finally
            {
                //new cSP().SP_CLExMemory(); //*Net 63-07-30 ปิด Clear Mem
            }
        }

        //*Net 63-07-30 ยกมาจาก Moshi
        public void C_PRCxClearPdtPrice4Pdt()
        {
            cDatabase oDB = new cDatabase();
            StringBuilder oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("DELETE PDT ");
                oSql.AppendLine("FROM TCNTPdtPrice4PDT PDT WITH(NOLOCK)");
                oSql.AppendLine("WHERE NOT EXISTS");
                oSql.AppendLine("   (SELECT FTPdtCode FROM");
                oSql.AppendLine("	    (SELECT DISTINCT FTPdtCode,FTPunCode,ISNULL(FTPplCode,'') FTPplCode,FTPghDocType,FTPghStaAdj,MAX(FDPghDStart + ' ' + FTPghTStart) AS FDPghDStart ");
                oSql.AppendLine("	    FROM TCNTPdtPrice4PDT WITH(NOLOCK)");
                oSql.AppendLine("       WHERE (FTPghTStart LIKE '00:%' AND FTPghTStop LIKE '23:%')");   //*Em 63-06-24
                oSql.AppendLine("       AND CONVERT(VARCHAR(10),FDPghDStart,121) <= CONVERT(VARCHAR(10),GETDATE(),121)");       //*Em 63-09-01
                oSql.AppendLine("	    GROUP BY FTPdtCode,FTPunCode,ISNULL(FTPplCode,''),FTPghDocType,FTPghStaAdj) TMP");
                oSql.AppendLine("	WHERE FTPdtCode = PDT.FTPdtCode AND FTPunCode = PDT.FTPunCode AND FTPplCode = ISNULL(FTPplCode,'')");
                oSql.AppendLine("	AND FTPghDocType = PDT.FTPghDocType AND FTPghStaAdj = PDT.FTPghStaAdj ");
                oSql.AppendLine("	AND CONVERT(VARCHAR(19),FDPghDStart,121) = CONVERT(VARCHAR(19),(PDT.FDPghDStart + ' ' + PDT.FTPghTStart),121))");
                oSql.AppendLine("AND (FTPghTStart LIKE '00:%' AND FTPghTStop LIKE '23:%')");       //*Em 63-06-24
                oSql.AppendLine("AND CONVERT(VARCHAR(10),FDPghDStart,121) <= CONVERT(VARCHAR(10),GETDATE(),121)");       //*Em 63-09-01
                oSql.AppendLine("");
                oSql.AppendLine("DELETE FROM TCNTPdtPrice4PDT"); //*Em 63-06-24
                oSql.AppendLine("WHERE CONVERT(VARCHAR(10),FDPghDStop,121) < CONVERT(VARCHAR(10),GETDATE(),121)"); //*Em 63-06-24
                oDB.C_SETxDataQuery(oSql.ToString());
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cPdt", "C_PRCxClearPdtPrice4Pdt : " + oEx.Message);
            }
            finally
            {
                oDB = null;
                oSql = null;
                //new cSP().SP_CLExMemory();
            }
        }
    }
}
