using API2PSSale.Class;
using API2PSSale.Class.Standard;
using API2PSSale.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using API2PSSale.Models.PdtStkBal;
using System.Threading;

namespace API2PSSale.Controllers
{
    /// <summary>
    /// Controller Service Check stock
    /// v2/Service/Data/CHKPdtStkBal
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/Service")]
    public class cServiceChkPdtStkBalController : ApiController
    {
        [Route("Data/CHKPdtStkBal")]
        [HttpPost]
        public cmlRespone POST_CHKPdtStkBal(cmlPdtStk poPdtStk)
        {
            cmlRespone oRes;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlPdtFail oPdtFail;
            try
            {
                if (poPdtStk == null)
                {
                    oRes = new cmlRespone();
                    oRes.rtCode = new cMS().tMS_RespCode700;
                    oRes.rtDesc = new cMS().tMS_RespDesc700;
                }

                if (poPdtStk.aoDt == null)
                {
                    oRes = new cmlRespone();
                    oRes.rtCode = new cMS().tMS_RespCode700;
                    oRes.rtDesc = new cMS().tMS_RespDesc700;
                }

                //A:หาคลังตาม Shop ที่อยู่ภายใต้ Merchant เดี่ยวกันกับ Pos
                oSql = new StringBuilder();
                oSql.AppendLine("SELECT SHP.FTWahCode,PS.FTPosCode FROM TCNMShop SHP ");
                oSql.AppendLine("INNER JOIN TVDMPosShop PS ON SHP.FTShpCode = PS.FTShpCode ");
                oSql.AppendLine("WHERE SHP.FTShpType = '4' AND GETDATE() BETWEEN SHP.FDShpSaleStart AND SHP.FDShpSaleStop ");
                oSql.AppendLine("AND SHP.FTMerCode = '" + poPdtStk.tMerchant + "'");
                oSql.AppendLine("AND SHP.FTBchCode = '" + poPdtStk.tBachCode + "'");
                oDatabase = new cDatabase();
                List<cmlWah> aoWah = oDatabase.C_GETaDataQuery<cmlWah>(oSql.ToString());

                if (aoWah == null)
                {
                    oRes = new cmlRespone();
                    return oRes;
                }

                List<cmlPdtFail> aoPdtFail = new List<cmlPdtFail>();
                foreach (var oItem in aoWah)
                {
                    oPdtFail = new cmlPdtFail();
                    oSql = new StringBuilder();
                    oSql.AppendLine("SELECT * FROM TCNTPdtStkBal WHERE FTWahCode = '" + oItem.FTWahCode + "'");
                    List<cmlTCNTPdtStkBal> aoPdt = oDatabase.C_GETaDataQuery<cmlTCNTPdtStkBal>(oSql.ToString());
                    List<cmlDT> aoDt = new List<cmlDT>();
                    foreach (var oDt in poPdtStk.aoDt)
                    {
                        decimal tQty = aoPdt.Where(x => x.FTPdtCode == oDt.tFTPdtCode).Select(x => x.FCStkQty).FirstOrDefault();

                        if (oDt.tFCStkQty > tQty)
                        {
                            aoDt.Add(new cmlDT
                            {
                                tFTPdtCode = oDt.tFTPdtCode,
                                tFCStkQty = tQty
                            });
                        }
                    }

                    oPdtFail.tFTPosCode = oItem.FTPosCode;
                    oPdtFail.aoDt = aoDt;
                    aoPdtFail.Add(oPdtFail);

                    if (aoDt.Count == 0)
                    {
                        oRes = new cmlRespone();
                        oRes.rtCode = new cMS().tMS_RespCode001;
                        oRes.rtDesc = new cMS().tMS_RespDesc001;
                        return oRes;
                    }

                    oPdtFail = null;
                    oSql = null;
                }
                oRes = new cmlRespone();
                oRes.rtCode = new cMS().tMS_RespCode001;
                oRes.rtDesc = new cMS().tMS_RespDesc001;
                oRes.aoPdtFail = aoPdtFail;
                aoPdtFail = null;
                return oRes;
            }
            catch (Exception oEx)
            {
                oRes = new cmlRespone();
                oRes.rtCode = new cMS().tMS_RespCode001;
                oRes.rtDesc = new cMS().tMS_RespDesc001;
                return oRes;
            }
        }

      
    }
}
