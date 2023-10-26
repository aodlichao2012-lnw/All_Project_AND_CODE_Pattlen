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
                new cSP().SP_CLExMemory();
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
                new cSP().SP_CLExMemory();
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
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptPdtValue", SqlDbType.VarChar, 20){ Value = ptValue},
                    new SqlParameter ("@ptPplCode", SqlDbType.VarChar, 20){ Value = cVB.tVB_PriceGroup},
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
                        aoPdt.Add(oPdt);
                    }
                }
                //+++++++++++++
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdt : " + oEx.Message); }
            finally
            {
                oSql = null;
                aoSqlParam = null;
                oDbTblPdt = null;
                new cSP().SP_CLExMemory();
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
                    cmd.Parameters.Add("@pdSaleDate", SqlDbType.DateTime);
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
                new cSP().SP_CLExMemory();
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
                oSql = new StringBuilder();
                //if (!string.IsNullOrEmpty(cVB.tVB_PriceGroup))
                //{
                //    oSql.AppendLine("SELECT DISTINCT PDT.FTPdtCode AS tPdtCode,  ");
                //    oSql.AppendLine("PBR.FTBarCode AS tBarcode, ");
                //    oSql.AppendLine("ISNULL(PDL.FTPdtName,(SELECT TOP 1 FTPdtName FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = PDT.FTPdtCode)) AS tPdtName,");
                //    oSql.AppendLine("ISNULL(PUL.FTPunName,(SELECT TOP 1 FTPunName FROM TCNMPdtUnit_L WITH(NOLOCK) WHERE FTPunCode = PPS.FTPunCode)) AS tUnitName,");    //*Em 62-06-26
                //    oSql.AppendLine("PPS.FCPdtUnitFact AS cUnitFactor, ");
                //    oSql.AppendLine("ISNULL(TmpPriceCst.FCPgdPriceRet,TmpPricePdt.FCPgdPriceRet) AS cPdtPrice,");
                //    oSql.AppendLine("Count(*) OVER(PARTITION BY 1 ) AS nRowCount,");    //*Arm  62-10-16
                //    oSql.AppendLine("IMG.FTImgObj AS tPicPath,");    //*Em 62-06-26
                //    oSql.AppendLine("PDT.FTPdtSaleType AS tSaleType,");  //*Em 62-09-23
                //    oSql.AppendLine("PDT.FTPdtStaAlwDis AS tStaAlwDis");  //*Em 62-10-04
                //    oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                //    if (string.IsNullOrEmpty(cVB.tVB_ShpCode))  //*Em 62-08-07
                //    {
                //        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PSB WITH(NOLOCK) ON PSB.FTPdtCode = PDT.FTPdtCode");
                //    }
                //    else
                //    {
                //        oSql.AppendLine("INNER JOIN TCNMPdtSpcBch PSB WITH(NOLOCK) ON PSB.FTPdtCode = PDT.FTPdtCode ");
                //    }
                //    oSql.AppendLine("LEFT JOIN TCNMPdt_L PDL WITH(NOLOCK) ON PDT.FTPdtCode = PDL.FTPdtCode ");
                //    oSql.AppendLine("	AND PDL.FNLngID = " + cVB.nVB_Language);
                //    //oSql.AppendLine("LEFT JOIN TCNMPdtGrp PGP WITH(NOLOCK) ON ISNULL(PDT.FTPgpChain,'') = ISNULL(PGP.FTPgpChain,'')");  //*Em 62-03-12  AdaPos5.0
                //    oSql.AppendLine("LEFT JOIN TCNMPdtTouchGrp PTGP WITH(NOLOCK) ON ISNULL(PDT.FTTcgCode,'') = ISNULL(PTGP.FTTcgCode,'')");  //*Net 63-01-13 เปลี่ยนจากตาราง PdtGrp เป็น PdtTouchGrp
                //    oSql.AppendLine("INNER JOIN TCNMPdtPackSize PPS WITH(NOLOCK) ON PDT.FTPdtCode = PPS.FTPdtCode");
                //    oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUL WITH(NOLOCK) ON PPS.FTPunCode = PUL.FTPunCode ");
                //    oSql.AppendLine("   AND PUL.FNLngID = " + cVB.nVB_Language);
                //    oSql.AppendLine("INNER JOIN TCNMPdtBar PBR WITH(NOLOCK) ON PDT.FTPdtCode = PBR.FTPdtCode AND PBR.FTBarStaUse = '1' AND PBR.FTBarStaAlwSale = '1'");  //*Em 62-08-07
                //    oSql.AppendLine("   AND PPS.FTPunCode = PBR.FTPunCode AND PBR.FTBarStaUse = '1'");
                //    oSql.AppendLine("INNER JOIN (SELECT FTPdtCode, FTPunCode, FCPgdPriceRet, SEQ ");
                //    oSql.AppendLine("            FROM (");
                //    oSql.AppendLine("               SELECT P4P.FTPdtCode,P4P.FTPunCode,P4P.FCPgdPriceRet,");
                //    oSql.AppendLine("                   RANK() OVER(PARTITION BY P4P.FTPdtCode,P4P.FTPunCode");
                //    oSql.AppendLine("                   ORDER BY P4P.FDLastUpdOn DESC) SEQ");   //*Em 62-05-22  AdaFC
                //    oSql.AppendLine("               FROM TCNTPdtPrice4Pdt P4P WITH(NOLOCK)");
                //    //oSql.AppendLine("               WHERE CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStart,121) + ' '+ FTPghTStart) <= GETDATE()");
                //    oSql.AppendLine("               WHERE (GETDATE() BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStart,121) + ' '+ FTPghTStart) AND CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStop,121) + ' '+ FTPghTStop))");    //*Em 62-09-17
                //    oSql.AppendLine("               AND FTPghDocType = '1'");    //*Em 62-09-17
                //    oSql.AppendLine("           ) PdtPrice4Pdt");
                //    oSql.AppendLine("           WHERE PdtPrice4Pdt.SEQ = 1) TmpPricePdt");
                //    oSql.AppendLine("   ON PDT.FTPdtCode = TmpPricePdt.FTPdtCode");
                //    oSql.AppendLine("   AND PPS.FTPunCode = TmpPricePdt.FTPunCode");
                //    oSql.AppendLine("LEFT JOIN (SELECT FTPdtCode, FTPunCode, FCPgdPriceRet, SEQ ");
                //    oSql.AppendLine("            FROM (");
                //    oSql.AppendLine("               SELECT P4C.FTPdtCode,P4C.FTPunCode,P4C.FCPgdPriceRet,");
                //    oSql.AppendLine("                   RANK() OVER(PARTITION BY P4C.FTPdtCode,P4C.FTPunCode");
                //    oSql.AppendLine("                   ORDER BY P4C.FDLastUpdOn DESC) SEQ");   //*Em 62-05-22  AdaFC
                //    oSql.AppendLine("               FROM TCNTPdtPrice4CST P4C WITH(NOLOCK)");
                //    oSql.AppendLine("               WHERE CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStart,121) + ' '+ FTPghTStart) <= GETDATE()");
                //    oSql.AppendLine("               AND P4C.FTPplCode = '"+ cVB.tVB_PriceGroup +"'");
                //    oSql.AppendLine("           ) PdtPrice4Cst");
                //    oSql.AppendLine("           WHERE PdtPrice4Cst.SEQ = 1) TmpPriceCst");
                //    oSql.AppendLine("   ON PDT.FTPdtCode = TmpPriceCst.FTPdtCode");
                //    oSql.AppendLine("   AND PPS.FTPunCode = TmpPriceCst.FTPunCode");
                //    oSql.AppendLine("LEFT JOIN TCNMImgPdt IMG WITH(NOLOCK) ON PDT.FTPdtCode = IMG.FTImgRefID AND IMG.FTImgKey = 'master'"); //*Em 62-06-26
                //    //oSql.AppendLine("WHERE PDT.FTPdtForSystem = '1'");                  // สินค้าสำหรับ Pos
                //    oSql.AppendLine("WHERE (PDT.FTPdtForSystem = '1' OR (PDT.FTPdtForSystem = '4' AND PDT.FTPdtType = '2' AND PDT.FTPdtSaleType = '2'))");    //*Em 62-09-23
                //}
                //else
                //{
                //    oSql.AppendLine("SELECT DISTINCT PDT.FTPdtCode AS tPdtCode,  ");
                //    oSql.AppendLine("PBR.FTBarCode AS tBarcode, ");
                //    oSql.AppendLine("ISNULL(PDL.FTPdtName,(SELECT TOP 1 FTPdtName FROM TCNMPdt_L WITH(NOLOCK) WHERE FTPdtCode = PDT.FTPdtCode)) AS tPdtName,");
                //    //oSql.AppendLine("PUL.FTPunName AS tUnitName,");
                //    oSql.AppendLine("ISNULL(PUL.FTPunName,(SELECT TOP 1 FTPunName FROM TCNMPdtUnit_L WITH(NOLOCK) WHERE FTPunCode = PPS.FTPunCode)) AS tUnitName,");    //*Em 62-06-26
                //    oSql.AppendLine("PPS.FCPdtUnitFact AS cUnitFactor, ");
                //    oSql.AppendLine("TmpPricePdt.FCPgdPriceRet AS cPdtPrice,");
                //    oSql.AppendLine("Count(*) OVER(PARTITION BY 1 ) AS nRowCount,");    //*Arm  62-10-16
                //    oSql.AppendLine("IMG.FTImgObj AS tPicPath,");    //*Em 62-06-26
                //    oSql.AppendLine("PDT.FTPdtSaleType AS tSaleType,");  //*Em 62-09-23
                //    oSql.AppendLine("PDT.FTPdtStaAlwDis AS tStaAlwDis");  //*Em 62-10-04
                //    oSql.AppendLine("FROM TCNMPdt PDT WITH(NOLOCK)");
                //    if (string.IsNullOrEmpty(cVB.tVB_ShpCode))  //*Em 62-08-07
                //    {
                //        oSql.AppendLine("LEFT JOIN TCNMPdtSpcBch PSB WITH(NOLOCK) ON PSB.FTPdtCode = PDT.FTPdtCode");
                //    }
                //    else
                //    {
                //        oSql.AppendLine("INNER JOIN TCNMPdtSpcBch PSB WITH(NOLOCK) ON PSB.FTPdtCode = PDT.FTPdtCode ");
                //    }
                //    oSql.AppendLine("LEFT JOIN TCNMPdt_L PDL WITH(NOLOCK) ON PDT.FTPdtCode = PDL.FTPdtCode ");
                //    oSql.AppendLine("	AND PDL.FNLngID = " + cVB.nVB_Language);
                //    //oSql.AppendLine("INNER JOIN TCNMPdtGrp PGP WITH(NOLOCK) ON PDT.FTPgpChain = PGP.FTPgpChain");
                //    //oSql.AppendLine("LEFT JOIN TCNMPdtGrp PGP WITH(NOLOCK) ON ISNULL(PDT.FTPgpChain,'') = ISNULL(PGP.FTPgpChain,'')");  //*Em 62-03-12  AdaPos5.0
                //    oSql.AppendLine("LEFT JOIN TCNMPdtTouchGrp PTGP WITH(NOLOCK) ON ISNULL(PDT.FTTcgCode,'') = ISNULL(PTGP.FTTcgCode,'')");  //*Net 63-01-13 เปลี่ยนจากตาราง PdtGrp เป็น PdtTouchGrp
                //    oSql.AppendLine("INNER JOIN TCNMPdtPackSize PPS WITH(NOLOCK) ON PDT.FTPdtCode = PPS.FTPdtCode");
                //    oSql.AppendLine("LEFT JOIN TCNMPdtUnit_L PUL WITH(NOLOCK) ON PPS.FTPunCode = PUL.FTPunCode ");
                //    oSql.AppendLine("   AND PUL.FNLngID = " + cVB.nVB_Language);
                //    oSql.AppendLine("INNER JOIN TCNMPdtBar PBR WITH(NOLOCK) ON PDT.FTPdtCode = PBR.FTPdtCode AND PBR.FTBarStaUse = '1' AND PBR.FTBarStaAlwSale = '1'"); //*Em 62-08-07
                //    oSql.AppendLine("   AND PPS.FTPunCode = PBR.FTPunCode AND PBR.FTBarStaUse = '1'");
                //    oSql.AppendLine("INNER JOIN (SELECT FTPdtCode, FTPunCode, FCPgdPriceRet, SEQ ");
                //    oSql.AppendLine("            FROM (");
                //    oSql.AppendLine("               SELECT P4P.FTPdtCode,P4P.FTPunCode,P4P.FCPgdPriceRet,");
                //    oSql.AppendLine("                   RANK() OVER(PARTITION BY P4P.FTPdtCode,P4P.FTPunCode");
                //    //oSql.AppendLine("                   ORDER BY P4P.FDPghDStart DESC) SEQ");
                //    oSql.AppendLine("                   ORDER BY P4P.FDLastUpdOn DESC) SEQ");   //*Em 62-05-22  AdaFC
                //    oSql.AppendLine("               FROM TCNTPdtPrice4Pdt P4P WITH(NOLOCK)");
                //    //oSql.AppendLine("               WHERE CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStart,121) + ' '+ FTPghTStart) <= GETDATE()");
                //    oSql.AppendLine("               WHERE (GETDATE() BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStart,121) + ' '+ FTPghTStart) AND CONVERT(DATETIME, CONVERT(VARCHAR(10),FDPghDStop,121) + ' '+ FTPghTStop))");    //*Em 62-09-17
                //    oSql.AppendLine("               AND FTPghDocType = '1'");    //*Em 62-09-17
                //    oSql.AppendLine("           ) PdtPrice4Pdt");
                //    oSql.AppendLine("           WHERE PdtPrice4Pdt.SEQ = 1) TmpPricePdt");
                //    oSql.AppendLine("   ON PDT.FTPdtCode = TmpPricePdt.FTPdtCode");
                //    oSql.AppendLine("   AND PPS.FTPunCode = TmpPricePdt.FTPunCode");
                //    oSql.AppendLine("LEFT JOIN TCNMImgPdt IMG WITH(NOLOCK) ON PDT.FTPdtCode = IMG.FTImgRefID AND IMG.FTImgKey = 'master' AND IMG.FNImgSeq = '1'"); //*Em 62-06-26 , *Arm 62-11-15 เพิ่ม AND IMG.FNImgSeq = '1'
                //    //oSql.AppendLine("WHERE PDT.FTPdtForSystem = '1'");                  // สินค้าสำหรับ Pos
                //    oSql.AppendLine("WHERE (PDT.FTPdtForSystem = '1' OR (PDT.FTPdtForSystem = '4' AND PDT.FTPdtType = '2' AND PDT.FTPdtSaleType = '2'))");    //*Em 62-09-23
                //}

                //if (string.IsNullOrEmpty(cVB.tVB_ShpCode))  //*Em 62-08-07
                //{
                //    oSql.AppendLine("AND (PSB.FTBchCode = '" + cVB.tVB_BchCode + "' OR ISNULL(PSB.FTBchCode,'') = '')");
                //    oSql.AppendLine("AND ISNULL(PSB.FTMerCode,'') ='' AND ISNULL(PSB.FTShpCode,'') = ''");
                //}
                //else
                //{
                //    oSql.AppendLine("AND (ISNULL(PSB.FTBchCode,'') = '" + cVB.tVB_BchCode + "' OR ISNULL(PSB.FTBchCode,'') = '')");
                //    oSql.AppendLine("AND ((ISNULL(PSB.FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(PSB.FTShpCode,'') ='') ");
                //    oSql.AppendLine("OR (ISNULL(PSB.FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(PSB.FTShpCode,'') ='" + cVB.tVB_ShpCode + "'))");
                //}

                //if (!string.IsNullOrEmpty(ptPgpChain))
                //    //oSql.AppendLine("AND PDT.FTPgpChain = '" + ptPgpChain + "'");   // แสดงตามกลุ่มสินค้า
                //oSql.AppendLine("AND PDT.FTTcgCode = '" + ptPgpChain + "'");   //*Net 63-01-13 เปลี่ยนจากตาราง PdtGrp เป็น PdtTouchGrp

                //if (pnMode == 1)        // Mode Scan
                //    oSql.AppendLine("AND PBR.FTBarCode = '" + ptValue + "'");       // Scan Barcode
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
                //                oSql.AppendLine("AND PDL.FTPdtName LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 2:     // Search By : Barcode
                //                oSql.AppendLine("AND PBR.FTBarCode LIKE '%" + ptValue + "%'");  // Search
                //                break;

                //            case 3:     // Search By : Price
                //                oSql.AppendLine("AND TmpPricePdt.FCPgdPriceRet LIKE '%" + ptValue + "%'");  // Search
                //                break;
                //        }
                //    }

                //    if(string.Equals(ptViewPdt, "0"))   // Image
                //        oSql.AppendLine("AND PPS.FCPdtUnitFact = 1");           // หน่วยเล็กสุด
                //}

                //*Em 63-02-26  แก้ไขให้ไปใช้ view
                oSql.AppendLine("SELECT DISTINCT PDT.FTPdtCode AS tPdtCode,");
                oSql.AppendLine("FTBarCode AS tBarcode,");
                oSql.AppendLine("FTPdtName AS tPdtName,");
                oSql.AppendLine("FTPunName AS tUnitName,");
                oSql.AppendLine("FCPdtUnitFact AS cUnitFactor,");
                //oSql.AppendLine("(CASE WHEN ISNULL(FTPplCodeCst,'') = '"+ (string.IsNullOrEmpty(cVB.tVB_PriceGroup)?"":cVB.tVB_PriceGroup) + "' THEN FCPdtPriceCst ELSE FCPdtPrice END) AS cPdtPrice,");
                //oSql.AppendLine("(CASE WHEN ISNULL(FTPplCodeCst,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "' AND ISNULL(FTPplCodeCst,'') <> ''  THEN FCPdtPriceCst ELSE FCPdtPrice END) AS cPdtPrice,");      //*Em 63-03-03
                oSql.AppendLine("(CASE WHEN ISNULL(PriCst.FTPplCode,'') <> ''  THEN PriCst.FCPdtPrice ELSE PDT.FCPdtPrice END) AS cPdtPrice,"); //*Em 63-04-07
                oSql.AppendLine("COUNT(*) OVER (PARTITION BY 1) nRowCount,");
                //oSql.AppendLine("FTImgObj AS tPicPat,");
                oSql.AppendLine("FTPdtPicPath AS tPicPath,");   //*Em 63-04-22
                oSql.AppendLine("FTPdtSaleType AS tSaleType,");
                oSql.AppendLine("FTPdtStaAlwDis AS tStaAlwDis");
                //oSql.AppendLine("FROM VCN_ProductSale PDT WITH(NOLOCK)");
                oSql.AppendLine("FROM TPSMPdt PDT WITH(NOLOCK)");   //*Em 63-04-22
                oSql.AppendLine("LEFT JOIN TPSTPdtPrice PriCst WITH(NOLOCK) ON PDT.FTPdtCode = PriCst.FTPdtCode AND PDT.FTPunCode = PriCst.FTPunCode ");    //*Em 63-04-07
                oSql.AppendLine("	AND ISNULL(PriCst.FTPplCode,'') <> '' AND PriCst.FTPriType = '1' AND ISNULL(PriCst.FTPplCode,'') = '" + (string.IsNullOrEmpty(cVB.tVB_PriceGroup) ? "" : cVB.tVB_PriceGroup) + "'");    //*Em 63-04-07
                //oSql.AppendLine("WHERE FNLngID =" + cVB.nVB_Language);
                //if (!string.IsNullOrEmpty(cVB.tVB_PriceGroup))
                //{
                //    oSql.AppendLine("AND FTPplCodeCst = '"+ cVB.tVB_PriceGroup + "' AND ISNULL(FTPplCodePdt,'') = ''");
                //}
                //else
                //{
                //    oSql.AppendLine("AND ISNULL(FTPplCodePdt,'') = ''");
                //}

                //if (string.IsNullOrEmpty(cVB.tVB_ShpCode))
                //{
                //    oSql.AppendLine("AND (FTBchCode = '" + cVB.tVB_BchCode + "' OR ISNULL(FTBchCode,'') = '')");
                //    oSql.AppendLine("AND ISNULL(FTMerCode,'') ='' AND ISNULL(FTShpCode,'') = ''");
                //}
                //else
                //{
                //    oSql.AppendLine("AND (ISNULL(FTBchCode,'') = '" + cVB.tVB_BchCode + "' OR ISNULL(FTBchCode,'') = '')");
                //    oSql.AppendLine("AND ((ISNULL(FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(FTShpCode,'') ='') ");
                //    oSql.AppendLine("OR (ISNULL(FTMerCode,'') = '" + cVB.tVB_Merchart + "' AND ISNULL(FTShpCode,'') ='" + cVB.tVB_ShpCode + "'))");
                //}

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
                //++++++++++++++++++++++++++++++

                ////oSql.AppendLine("ORDER BY PDT.FTPdtCode ");

                //*Em 62-06-27
                switch (pnSort)
                {
                    case 1:
                        oSql.AppendLine("ORDER BY tPdtName ASC ");
                        break;
                    case 2:
                        oSql.AppendLine("ORDER BY tPdtName DESC ");
                        break;
                }
                //+++++++++++++
                oSql.AppendLine("OFFSET " + pnStartRow + " ROWS");
                //oSql.AppendLine("FETCH NEXT 50 ROWS ONLY;");
                //oSql.AppendLine("FETCH NEXT  " + cVB.nVB_MaxData + "  ROWS ONLY;"); //*Arm 62-10-16
                oSql.AppendLine("FETCH NEXT  " + cVB.nVB_PdtPerPage + "  ROWS ONLY;"); //*Arm 62-11-15
                aoPdt = new cDatabase().C_GETaDataQuery<cmlPdtDetail>(oSql.ToString());
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_GETaPdtSale : " + oEx.Message); }
            finally
            {
                oSql = null;
                new cSP().SP_CLExMemory();
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

                    // set up the parameters
                    cmd.Parameters.Add("@ptBchCode", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@ptMerCode", SqlDbType.VarChar, 10);
                    cmd.Parameters.Add("@ptShpCode", SqlDbType.VarChar, 5);
                    cmd.Parameters.Add("@ptPplCode", SqlDbType.VarChar, 20);
                    cmd.Parameters.Add("@pnLang", SqlDbType.Int);
                    cmd.Parameters.Add("@FNResult", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.Parameters["@ptBchCode"].Value = cVB.tVB_BchCode;
                    cmd.Parameters["@ptMerCode"].Value = cVB.tVB_Merchart;
                    cmd.Parameters["@ptShpCode"].Value = cVB.tVB_ShpCode;
                    cmd.Parameters["@ptPplCode"].Value = cVB.tVB_BchPriceGroup;
                    cmd.Parameters["@pnLang"].Value = cVB.nVB_Language;
                    cmd.ExecuteNonQuery();

                    nResult = Convert.ToInt32(cmd.Parameters["@FNResult"].Value);
                    conn.Close();
                }
            }
            catch (Exception oEx) { new cLog().C_WRTxLog("cPdt", "C_PRCxPreparePdtSale : " + oEx.Message); }
            finally
            {
                new cSP().SP_CLExMemory();
            }
        }
    }
}
