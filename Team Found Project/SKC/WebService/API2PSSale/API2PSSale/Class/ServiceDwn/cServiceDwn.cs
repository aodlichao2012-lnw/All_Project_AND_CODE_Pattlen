using API2PSSale.Class.Standard;
using API2PSSale.Models.WebService.Request;
using API2PSSale.Models.WebService.Response.SalDT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace API2PSSale.Class.ServiceDwn
{
    /// <summary>
    /// Class service download
    /// </summary>
    public class cServiceDwn
    {
        /// <summary>
        /// Function Sale detail
        /// </summary>
        /// <param name="poPara"></param>
        /// <param name="poResult"></param>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <returns></returns>
        public bool C_GETbSalDT(cmlReqSalDT poPara, out List<cmlResTPSTSalDT> poResult,out string ptErrCode,out string ptErrDesc)
        {
            cSP oSP = new cSP();
            cMS oMsg = new cMS();
            StringBuilder oSql;
            DataTable oTbl;
            SqlParameter[] aoSqlParam;
            cDatabase oDatabase;
            try
            {
                oSql = new StringBuilder();
                oTbl = new DataTable();
                oDatabase = new cDatabase();
                poResult = new List<cmlResTPSTSalDT>();

                oSql.Clear();
                oSql.AppendLine("SELECT ISNULL(FTXshDocNo,'') AS FTXshDocNo, FTXshRefInt FROM TPSTSalHD WITH (NOLOCK)");
                oSql.AppendLine("WHERE 1=1");
                oSql.AppendLine("AND FTBchCode='" + poPara.ptBchCode + "'");
                oSql.AppendLine("AND FTXshDocNo='" + poPara.ptXshDocNo + "'");
                //oSql.AppendLine("AND FTShpCode='" + poPara.ptShpCode + "'");  //*Em 62-09-17  ไม่ต้อง where shop
/*                oSql.AppendLine("AND ISNULL(FTXshRefInt,'') = '' ");*/  //*Em 62-09-17
                oTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());  //*BOY 62-11-15

                if (oTbl != null)
                {
                    if(!string.IsNullOrEmpty(oTbl.Rows[0].Field<string>("FTXshRefInt")))
                    {
                        ptErrCode = oMsg.tMS_RespCode802;
                        ptErrDesc = oMsg.tMS_RespDesc802;
                        return false;
                    }
                    aoSqlParam = new SqlParameter[]
                    {
                    new SqlParameter ("@ptDocNo", SqlDbType.NVarChar,20){ Value = oTbl.Rows[0].Field<string>("FTXshDocNo")},
                    new SqlParameter ("@ptBchCode", SqlDbType.NVarChar,20){ Value = poPara.ptBchCode },
                    new SqlParameter ("@ptShpCode", SqlDbType.NVarChar,20){ Value = poPara.ptShpCode },
                    };

                    oTbl = oDatabase.C_GEToQueryStoreDataTbl("STP_GEToTPSTSalDT", aoSqlParam);
                    if (oTbl.Rows.Count > 0 && oTbl != null)
                    {
                        var oItem = from DataRow oRow in oTbl.Rows
                                    select new cmlResTPSTSalDT()
                                    {
                                        FTBchCode = oRow["FTBchCode"] == DBNull.Value ? "" : (string)oRow["FTBchCode"],
                                        FTXshDocNo = oRow["FTXshDocNo"] == DBNull.Value ? "" : (string)oRow["FTXshDocNo"],
                                        FNXsdSeqNo = oRow["FNXsdSeqNo"] == DBNull.Value ? 0 : (int)oRow["FNXsdSeqNo"],
                                        FTPdtCode = oRow["FTPdtCode"] == DBNull.Value ? "" : (string)oRow["FTPdtCode"],
                                        FTXsdPdtName = oRow["FTXsdPdtName"] == DBNull.Value ? "" : (string)oRow["FTXsdPdtName"],
                                        FTPunCode = oRow["FTPunCode"] == DBNull.Value ? "" : (string)oRow["FTPunCode"],
                                        FTPunName = oRow["FTPunName"] == DBNull.Value ? "" : (string)oRow["FTPunName"],
                                        FCXsdFactor = oRow["FCXsdFactor"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdFactor"]),
                                        FTXsdBarCode = oRow["FTXsdBarCode"] == DBNull.Value ? "" : (string)oRow["FTXsdBarCode"],
                                        FTSrnCode = oRow["FTSrnCode"] == DBNull.Value ? "" : (string)oRow["FTSrnCode"],
                                        FTXsdVatType = oRow["FTXsdVatType"] == DBNull.Value ? "" : (string)oRow["FTXsdVatType"],
                                        FTVatCode = oRow["FTVatCode"] == DBNull.Value ? "" : (string)oRow["FTVatCode"],
                                        FCXsdVatRate = oRow["FCXsdVatRate"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdVatRate"]),
                                        FTXsdSaleType = oRow["FTXsdSaleType"] == DBNull.Value ? "" : (string)oRow["FTXsdSaleType"],
                                        FCXsdSalePrice = oRow["FCXsdSalePrice"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdSalePrice"]),
                                        FCXsdQty = oRow["FCXsdQty"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdQty"]),
                                        FCXsdQtyAll = oRow["FCXsdQtyAll"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdQtyAll"]),
                                        FCXsdSetPrice = oRow["FCXsdSetPrice"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdSetPrice"]),
                                        FCXsdAmtB4DisChg = oRow["FCXsdAmtB4DisChg"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdAmtB4DisChg"]),
                                        FTXsdDisChgTxt = oRow["FTXsdDisChgTxt"] == DBNull.Value ? "" : (string)oRow["FTXsdDisChgTxt"],
                                        FCXsdDis = oRow["FCXsdDis"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdDis"]),
                                        FCXsdChg = oRow["FCXsdChg"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdChg"]),
                                        FCXsdNet = oRow["FCXsdNet"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdNet"]),
                                        FCXsdNetAfHD = oRow["FCXsdNetAfHD"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdNetAfHD"]),
                                        FCXsdVat = oRow["FCXsdVat"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdVat"]),
                                        FCXsdVatable = oRow["FCXsdVatable"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdVatable"]),
                                        FCXsdWhtAmt = oRow["FCXsdWhtAmt"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdWhtAmt"]),
                                        FTXsdWhtCode = oRow["FTXsdWhtCode"] == DBNull.Value ? "" : (string)oRow["FTXsdWhtCode"],
                                        FCXsdWhtRate = oRow["FCXsdWhtRate"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdWhtRate"]),
                                        FCXsdCostIn = oRow["FCXsdWhtRate"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdWhtRate"]),
                                        FCXsdCostEx = oRow["FCXsdCostEx"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdCostEx"]),
                                        FTXsdStaPdt = oRow["FTXsdStaPdt"] == DBNull.Value ? "" : (string)oRow["FTXsdStaPdt"],
                                        FCXsdQtyLef = oRow["FCXsdQtyLef"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdQtyLef"]),
                                        FCXsdQtyRfn = oRow["FCXsdQtyRfn"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdQtyRfn"]),
                                        FTXsdStaPrcStk = oRow["FTXsdStaPrcStk"] == DBNull.Value ? "" : (string)oRow["FTXsdStaPrcStk"],
                                        FTXsdStaAlwDis = oRow["FTXsdStaAlwDis"] == DBNull.Value ? "" : (string)oRow["FTXsdStaAlwDis"],
                                        FNXsdPdtLevel = oRow["FNXsdPdtLevel"] == DBNull.Value ? 0 : (int)oRow["FNXsdPdtLevel"],
                                        FTXsdPdtParent = oRow["FTXsdPdtParent"] == DBNull.Value ? "" : (string)oRow["FTXsdPdtParent"],
                                        FCXsdQtySet = oRow["FCXsdQtySet"] == DBNull.Value ? 0.00 : Convert.ToDouble((decimal)oRow["FCXsdQtySet"]),
                                        FTPdtStaSet = oRow["FTPdtStaSet"] == DBNull.Value ? "" : (string)oRow["FTPdtStaSet"],
                                        FTXsdRmk = oRow["FTXsdRmk"] == DBNull.Value ? "" : (string)oRow["FTXsdRmk"],
                                        FDLastUpdOn = oRow["FDLastUpdOn"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["FDLastUpdOn"],
                                        FTLastUpdBy = oRow["FTLastUpdBy"] == DBNull.Value ? "" : (string)oRow["FTLastUpdBy"],
                                        FDCreateOn = oRow["FDCreateOn"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["FDCreateOn"],
                                        FTCreateBy = oRow["FTCreateBy"] == DBNull.Value ? "" : (string)oRow["FTCreateBy"],
                                    };
                        poResult = oItem.ToList();
                    }
                    ptErrCode = "";
                    ptErrDesc = "";
                    return true;
                }
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
            }
            catch (Exception)
            {
                poResult = new List<cmlResTPSTSalDT>();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oTbl = null;
                aoSqlParam = null;
                oDatabase = null;
                oSP = null;
                oMsg = null;
                oSql = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }
    }
}