using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Receive;
using MQReceivePrc.Models.Stock.StockCard;
using MQReceivePrc.Models.Stock.Stockbalance;
using MQReceivePrc.Models.Webservice.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cStock
    {
        public bool C_PRCbGetStkCrd(cmlRcvDocApv poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlPdtStkCrd oStkCrd;
            StringBuilder oSql;
            cDatabase oDB;
            cSP oSP = new cSP();
            int nCmdTime = 60;
            try
            {
                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {

                    if (poData == null) return false;

                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT FNStkCrdID,FTBchCode,FDStkDate,FTStkDocNo,FTWahCode,");
                    oSql.AppendLine("FTPdtCode,FTStkType,FCStkQty,FCStkSetPrice,FCStkCostIn,");
                    oSql.AppendLine("FCStkCostEx,FDCreateOn,FTCreateBy");
                    oSql.AppendLine("FROM TCNTPdtStkCrd WITH(NOLOCK) ");
                    oSql.AppendLine("WHERE FTBchCode = '" + poData.ptBchCode + "'");
                    oSql.AppendLine("AND FTStkDocNo = '" + poData.ptDocNo + "'");

                    oDB = new cDatabase();

                    oStkCrd = new cmlPdtStkCrd();
                    oStkCrd.aoTCNTPdtStkCrd = new List<cmlTCNTPdtStkCrd>();

                    oStkCrd.aoTCNTPdtStkCrd = oDB.C_GETaDataQuery<cmlTCNTPdtStkCrd>(poData.ptConnStr, oSql.ToString(), nCmdTime);


                    //Send API2PSSale
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/PdtStock/Data/UplPdtStkCrd";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oStkCrd);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cStock", "C_PRCbGetStkCrd : " + oEx.Message);
                            }

                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResResult oRes = JsonConvert.DeserializeObject<cmlResResult>(tJSonRes);

                                if (oRes.rtCode == "001")
                                {

                                }
                                else
                                {
                                    new cLog().C_WRTxLog("cSale", "C_PRCbGetStkCrd/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbGetStkCrd");
                return false;
            }
        }

        public bool C_PRCbDownloadStkCrd(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlPdtStkCrd oStkCrd;
            cDataReader<cmlTCNTPdtStkCrd> aoStkCrd;
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect = 0;
            SqlTransaction oTransaction;
            SqlConnection oConn;
            cSP oSP = new cSP();
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;

                oStkCrd = JsonConvert.DeserializeObject<cmlPdtStkCrd>(poData.ptData);

                oSql = new StringBuilder();
                oDB = new cDatabase();
                // Create Temp
                #region Create Temp
                //TCNTPdtStkCrdBch
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTPdtStkCrdBchTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTPdtStkCrdBchTmp FROM TCNTPdtStkCrdBch with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtStkCrdBchTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtStkCrdBch' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TCNTPdtStkCrdBchTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTPdtStkCrdBchTmp FROM TCNTPdtStkCrdBch with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TCNTPdtStkCrdBchTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion Create Temp

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB
                if (oStkCrd.aoTCNTPdtStkCrd != null)
                {
                    aoStkCrd = new cDataReader<cmlTCNTPdtStkCrd>(oStkCrd.aoTCNTPdtStkCrd);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoStkCrd.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TCNTPdtStkCrdBchTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoStkCrd);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE PSC ");
                oSql.AppendLine("   FROM TCNTPdtStkCrdBch PSC WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TCNTPdtStkCrdBchTmp TMPPSC WITH(NOLOCK) ON PSC.FTBchCode = TMPPSC.FTBchCode AND PSC.FDStkDate = TMPPSC.FDStkDate");// แก้
                oSql.AppendLine("   AND PSC.FTStkDocNo = TMPPSC.FTStkDocNo AND PSC.FTWahCode = TMPPSC.FTWahCode");
                oSql.AppendLine("   AND PSC.FTPdtCode = TMPPSC.FTPdtCode AND PSC.FTStkType = TMPPSC.FTStkType");
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTPdtStkCrdBch");
                oSql.AppendLine("   SELECT FTBchCode,FDStkDate,FTStkDocNo,FTWahCode,FTPdtCode,");
                oSql.AppendLine("   FTStkType,FCStkQty,FCStkSetPrice,FCStkCostIn,FCStkCostEx,");
                oSql.AppendLine("   FDCreateOn,FTCreateBy");
                oSql.AppendLine("   FROM TCNTPdtStkCrdBchTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbDownloadStkCrd");
                return false;
            }
        }


        public bool C_PRCbGetStkBal(cmlRcvDocApv poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlPdtStkBal oStkBal;
            StringBuilder oSql;
            cDatabase oDB;
            cSP oSP = new cSP();
            int nCmdTime = 60;
            try
            {
                //*Arm 63-03-31 Check StaUseCentralized
                if (cVB.bVB_StaUseCentralized == false) // ไม่ใช้งานระบบ Centralized
                {

                    if (poData == null) return false;

                    oSql = new StringBuilder();
                    // Sale, SALEPOSVD, TNFBRANCH, TNFWAREHOSE, TNFWAREHOSEOUT, TNFWAREHOSEIN, TNFWAREHOSEVD, ADJUSTSTOCK, PURCHASECN, PURCHASEINV
                    switch (poData.ptDocType)
                    {
                        case "SALE":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TPSTSalHD HD WITH(NOLOCK) ON PSB.FTWahCode = HD.FTWahCode ");
                            oSql.AppendLine("INNER JOIN TPSTSalDT DT WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTXshDocNo = DT.FTXshDocNo AND PSB.FTPdtCode = DT.FTPdtCode AND DT.FTXsdStaPdt != 4 ");
                            oSql.AppendLine("WHERE HD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND HD.FTXshDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "SALEPOSVD":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TVDTSalHD VDHD WITH(NOLOCK) ON PSB.FTWahCode = VDHD.FTWahCode ");
                            oSql.AppendLine("INNER JOIN TVDTSalDT VDDT WITH(NOLOCK) ON VDHD.FTBchCode = VDDT.FTBchCode AND VDHD.FTXshDocNo = VDDT.FTXshDocNo AND PSB.FTPdtCode = VDDT.FTPdtCode AND VDDT.FTXsdStaPdt != 4");
                            oSql.AppendLine("WHERE VDHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND VDHD.FTXshDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "TNFBRANCH":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTbxHD TbxHD WITH(NOLOCK) ON PSB.FTWahCode = TbxHD.FTXthWhFrm ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTbxDT TbxDT WITH(NOLOCK) ON TbxHD.FTBchCode = TbxDT.FTBchCode AND TbxHD.FTXthDocNo = TbxDT.FTXthDocNo AND PSB.FTPdtCode = TbxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TbxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TbxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            oSql.AppendLine("UNION ");
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTbxHD TbxHD WITH(NOLOCK) ON PSB.FTWahCode = TbxHD.FTXthWhTo  ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTbxDT TbxDT WITH(NOLOCK) ON TbxHD.FTBchCode = TbxDT.FTBchCode AND TbxHD.FTXthDocNo = TbxDT.FTXthDocNo AND PSB.FTPdtCode = TbxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TbxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TbxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "TNFWAREHOSE":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwxHD TWxHD ON PSB.FTWahCode = TWxHD.FTXthWhFrm ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwxDT TWxDT ON TWxHD.FTBchCode = TWxDT.FTBchCode AND TWxHD.FTXthDocNo = TWxDT.FTXthDocNo AND PSB.FTPdtCode = TWxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            oSql.AppendLine("UNION ");
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwxHD TWxHD ON PSB.FTWahCode = TWxHD.FTXthWhTo ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwxDT TWxDT ON TWxHD.FTBchCode = TWxDT.FTBchCode AND TWxHD.FTXthDocNo = TWxDT.FTXthDocNo AND PSB.FTPdtCode = TWxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "TNFWAREHOSEOUT":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwoHD TWoHD ON PSB.FTWahCode = TWoHD.FTXthWhFrm ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwoDT TWoDT ON TWoHD.FTBchCode = TWoDT.FTBchCode AND TWoHD.FTXthDocNo = TWoDT.FTXthDocNo AND PSB.FTPdtCode = TWoDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWoHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWoHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            oSql.AppendLine("UNION ");
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwoHD TWoHD ON PSB.FTWahCode = TWoHD.FTXthWhTo ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwoDT TWoDT ON TWoHD.FTBchCode = TWoDT.FTBchCode AND TWoHD.FTXthDocNo = TWoDT.FTXthDocNo AND PSB.FTPdtCode = TWoDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWoHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWoHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "TNFWAREHOSEIN":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwiHD TWiHD ON PSB.FTWahCode = TWiHD.FTXthWhFrm ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwiDT TWiDT ON TWiHD.FTBchCode = TWiDT.FTBchCode AND TWiHD.FTXthDocNo = TWiDT.FTXthDocNo AND PSB.FTPdtCode = TWiDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWiHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWiHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            oSql.AppendLine("UNION ");
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwiHD TWiHD ON PSB.FTWahCode = TWiHD.FTXthWhTo ");
                            oSql.AppendLine("INNER JOIN TCNTPdtTwiDT TWiDT ON TWiHD.FTBchCode = TWiDT.FTBchCode AND TWiHD.FTXthDocNo = TWiDT.FTXthDocNo AND PSB.FTPdtCode = TWiDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TWiHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TWiHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "TNFWAREHOSEVD":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TVDTPdtTwxHD TwxHD WITH(NOLOCK) ON PSB.FTWahCode = TwxHD.FTXthWhFrm ");
                            oSql.AppendLine("INNER JOIN TVDTPdtTwxDT TwxDT WITH(NOLOCK) ON TwxHD.FTBchCode = TwxDT.FTBchCode AND TwxHD.FTXthDocNo = TwxDT.FTXthDocNo AND PSB.FTPdtCode = TwxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TwxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TwxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            oSql.AppendLine("UNION ");
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TVDTPdtTwxHD TwxHD WITH(NOLOCK) ON PSB.FTWahCode = TwxHD.FTXthWhTo ");
                            oSql.AppendLine("INNER JOIN TVDTPdtTwxDT TwxDT WITH(NOLOCK) ON TwxHD.FTBchCode = TwxDT.FTBchCode AND TwxHD.FTXthDocNo = TwxDT.FTXthDocNo AND PSB.FTPdtCode = TwxDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TwxHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TwxHD.FTXthDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "ADJUSTSTOCK":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TCNTPdtAdjStkHD ADJHD WITH(NOLOCK) ON PSB.FTWahCode = ADJHD.FTAjhWhTo ");
                            oSql.AppendLine("INNER JOIN TCNTPdtAdjStkDT ADJDT WITH(NOLOCK) ON ADJHD.FTBchCode = ADJDT.FTBchCode AND ADJHD.FTAjhDocNo = ADJDT.FTAjhDocNo AND PSB.FTPdtCode = ADJDT.FTPdtCode ");
                            oSql.AppendLine("WHERE ADJHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND ADJHD.FTAjhDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "PURCHASECN":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TAPTPcHD TPcHD WITH(NOLOCK) ON PSB.FTWahCode = TPcHD.FTWahCode ");
                            oSql.AppendLine("INNER JOIN TAPTPcDT TPcDT WITH(NOLOCK) ON TPcHD.FTBchCode = TPcDT.FTBchCode AND TPcHD.FTXphDocNo = TPcDT.FTXphDocNo AND PSB.FTPdtCode = TPcDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TPcHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TPcHD.FTXphDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        case "PURCHASEINV":
                            oSql.AppendLine("SELECT DISTINCT PSB.FTBchCode,PSB.FTWahCode,PSB.FTPdtCode,PSB.FCStkQty,PSB.FDLastUpdOn,PSB.FTLastUpdBy,PSB.FDCreateOn,PSB.FTCreateBy ");
                            oSql.AppendLine("FROM TCNTPdtStkBal PSB WITH(NOLOCK) ");
                            oSql.AppendLine("INNER JOIN TAPTPiHD TPiHD WITH(NOLOCK) ON PSB.FTWahCode = TPiHD.FTWahCode ");
                            oSql.AppendLine("INNER JOIN TAPTPiDT TPiDT WITH(NOLOCK) ON TPiHD.FTBchCode = TPiDT.FTBchCode AND TPiHD.FTXphDocNo = TPiDT.FTXphDocNo AND PSB.FTPdtCode = TPiDT.FTPdtCode ");
                            oSql.AppendLine("WHERE TPiHD.FTBchCode = '" + poData.ptBchCode + "' ");
                            oSql.AppendLine("AND TPiHD.FTXphDocNo = '" + poData.ptDocNo + "' ");
                            break;

                        default:
                            return false;
                    }

                    oDB = new cDatabase();

                    oStkBal = new cmlPdtStkBal();
                    oStkBal.aoTCNTPdtStkBal = new List<cmlTCNTPdtStkBal>();

                    oStkBal.aoTCNTPdtStkBal = oDB.C_GETaDataQuery<cmlTCNTPdtStkBal>(poData.ptConnStr, oSql.ToString(), nCmdTime);

                    //Send API2PSSale
                    if (oSP.SP_CHKbIsHQBch(poData.ptConnStr, (int)poShopDB.nCommandTimeOut) == false)
                    {
                        string tAPIUrl = "";
                        string tUrlFunc = "/PdtStock/Data/UplPdtStkBal";
                        string tAPIHeader = "";
                        string tXKey = "";
                        string tBchHQ = "";
                        tBchHQ = oSP.SP_GETtBchHQ(poData.ptConnStr, (int)poShopDB.nCommandTimeOut);
                        tAPIUrl = oSP.SP_GETtUrlAPI(poData.ptConnStr, (int)poShopDB.nCommandTimeOut, tBchHQ, 5, ref tXKey, ref tAPIHeader);

                        if (!string.IsNullOrEmpty(tAPIUrl))
                        {
                            string tJSonCall = JsonConvert.SerializeObject(oStkBal);
                            cClientService oCall = new cClientService();
                            oCall = new cClientService(tAPIHeader, tXKey);
                            HttpResponseMessage oRep = new HttpResponseMessage();
                            try
                            {
                                oRep = oCall.C_POSToInvoke(tAPIUrl + tUrlFunc, tJSonCall);
                            }
                            catch (Exception oEx)
                            {
                                new cLog().C_WRTxLog("cStock", "C_PRCbGetStkBal : " + oEx.Message);
                            }

                            if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                                cmlResResult oRes = JsonConvert.DeserializeObject<cmlResResult>(tJSonRes);
                                if (oRes.rtCode == "001")
                                {

                                }
                                else
                                {
                                    new cLog().C_WRTxLog("cSale", "C_PRCbGetStkBal/ToHQ : " + oRes.rtMsg);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbGetStkBal");
                return false;
            }
        }

        public bool C_PRCbDownloadStkBal(cmlRcvDataUpload poData, cmlShopDB poShopDB, ref string ptErrMsg)
        {
            cmlPdtStkBal oStkBal;
            cDataReader<cmlTCNTPdtStkBal> aoStkBal;
            StringBuilder oSql;
            cDatabase oDB;
            int nRowAffect = 0;
            SqlTransaction oTransaction;
            SqlConnection oConn;
            cSP oSP = new cSP();
            try
            {
                if (poData == null) return false;
                if (string.IsNullOrEmpty(poData.ptData)) return false;

                oStkBal = JsonConvert.DeserializeObject<cmlPdtStkBal>(poData.ptData);

                oSql = new StringBuilder();
                oDB = new cDatabase();
                // Create Temp
                #region Create Temp
                //TCNTPdtStkBalBch
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TCNTPdtStkBalBchTmp'))");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("	    SELECT TOP 0 * INTO TCNTPdtStkBalBchTmp FROM TCNTPdtStkBalBch with(nolock)");
                oSql.AppendLine("   END");
                oSql.AppendLine("ELSE");
                oSql.AppendLine("   BEGIN");
                oSql.AppendLine("       IF ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtStkBalBchTmp' ),0) <> ISNULL((SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TCNTPdtStkBalBch' ),0)");
                oSql.AppendLine("       BEGIN");
                oSql.AppendLine("   	    DROP TABLE TCNTPdtStkBalBchTmp");
                oSql.AppendLine("   	    SELECT TOP 0 * INTO TCNTPdtStkBalBchTmp FROM TCNTPdtStkBalBch with(nolock)");
                oSql.AppendLine("       END");
                oSql.AppendLine("   END");
                oSql.AppendLine("TRUNCATE TABLE TCNTPdtStkBalBchTmp");
                oSql.AppendLine("");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);
                #endregion Create Temp

                oConn = new SqlConnection(poData.ptConnStr);
                oConn.Open();

                oTransaction = oConn.BeginTransaction();

                //insert to DB 
                if (oStkBal.aoTCNTPdtStkBal != null)
                {
                    aoStkBal = new cDataReader<cmlTCNTPdtStkBal>(oStkBal.aoTCNTPdtStkBal);

                    using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(oConn, SqlBulkCopyOptions.Default, oTransaction))
                    {
                        foreach (string tColName in aoStkBal.ColumnNames)
                        {
                            oBulkCopy.ColumnMappings.Add(tColName, tColName);
                        }

                        oBulkCopy.BatchSize = 100;
                        oBulkCopy.DestinationTableName = "dbo.TCNTPdtStkBalBchTmp";

                        try
                        {
                            oBulkCopy.WriteToServer(aoStkBal);
                        }
                        catch (Exception oEx)
                        {
                            oTransaction.Rollback();
                            ptErrMsg = oEx.Message.ToString();
                            return false;
                        }
                    }
                }

                oTransaction.Commit();

                oSql = new StringBuilder();
                oSql.AppendLine("BEGIN TRY");
                oSql.AppendLine("BEGIN TRANSACTION");
                oSql.AppendLine("   DELETE PSB ");
                oSql.AppendLine("   FROM TCNTPdtStkBalBch PSB WITH(ROWLOCK)");
                oSql.AppendLine("   INNER JOIN TCNTPdtStkBalBchTmp TMPPSB WITH(NOLOCK) ON PSB.FTBchCode = TMPPSB.FTBchCode AND PSB.FTWahCode = TMPPSB.FTWahCode AND PSB.FTPdtCode = TMPPSB.FTPdtCode ");
                
                oSql.AppendLine();
                oSql.AppendLine("   INSERT INTO TCNTPdtStkBalBch");
                oSql.AppendLine("   SELECT * FROM TCNTPdtStkBalBchTmp WITH(NOLOCK) ");
                oSql.AppendLine();
                oSql.AppendLine("   COMMIT TRANSACTION");
                oSql.AppendLine("END TRY");
                oSql.AppendLine("BEGIN CATCH");
                oSql.AppendLine("   IF(@@TRANCOUNT > 0)");
                oSql.AppendLine("       ROLLBACK TRAN;");
                oSql.AppendLine("   THROW;");
                oSql.AppendLine("END CATCH");
                oDB.C_DATbExecuteNonQuery(poData.ptConnStr, oSql.ToString(), (int)poShopDB.nCommandTimeOut, out nRowAffect);

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbDownloadStkBal");
                return false;
            }
        }

    }
}
