using AdaPos.Models.Database;
using AdaPos.Models.Other;
using AdaPos.Models.Webservice.Required.Stock;
using AdaPos.Models.Webservice.Respond.Stock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public static class cStock
    {
        //API Error Code
        //001	success.
        //700	all parameter is null.
        //701	validate parameter model false.
        //706	product retail price not allow less than 0.
        //800	data not found.
        //905	cannot connect database.

        //WebService Error Code
        //200	OK
        //202	Acepted
        //401	Unauthorized
        //403	Forbidden
        //408	Request Timeout
        //500	Internal Server Error
        //504	Gateway Timeout
        //525	SSL Handshake Failed

        /// <summary>
        /// ตรวจสอบสินค้าว่ามีใน Stock หรือไม่
        /// </summary>
        /// <param name="poPdtOrder"></param>
        /// <returns></returns>
        public static bool C_CHKbSalStock(ref cmlPdtOrder poPdtOrder)
        {
            cmlResChkStock oResPDTStk;
            try
            {
                if(String.IsNullOrEmpty(cVB.tVB_APIKADS)) return true;

                oResPDTStk = new cmlResChkStock();
                oResPDTStk = C_GEToStocksPDT(poPdtOrder.tBarcode, cVB.tVB_BchRefID, cVB.tVB_ShpCode, cVB.tVB_WahCode);
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return false;
        }

        public static cmlResChkStock C_GEToStocksPDT(string ptPdtCode, string ptBchCode, string ptShopCode, string ptWahCode)
        {
            cmlReqChkStock oPDTChk;
            cmlResChkStock oPDTStock;
            string tJsonRes;
            string tJsonReq;
            try
            {
                oPDTChk = new cmlReqChkStock();
                oPDTStock = new cmlResChkStock();

                oPDTChk.tPartNo = ptPdtCode;
                oPDTChk.tSaleOrg = ptBchCode;
                oPDTChk.tPlantCode = ptShopCode;
                oPDTChk.tStorage = ptWahCode;
                tJsonReq = JsonConvert.SerializeObject(oPDTChk);

                //Call API Check Stock

                tJsonRes = "{\"ErrorCode\":\"001\",\"MatNo\":\"8852122016689\",\"MatQty\":10,\"MatUnit\":\"EA\",\"BinLoc\":\"0100\",\"Inters\":[{\"MatNo\":\"8852122724294\",\"Binlocation\":\"100\",\"MatQty\":5},{\"MatNo\":\"8852122012810\",\"Binlocation\":\"100\",\"MatQty\":10}]}";
                oPDTStock = JsonConvert.DeserializeObject<cmlResChkStock>(tJsonRes);
                return oPDTStock;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cStock", MethodBase.GetCurrentMethod().Name + " : " + oEx.Message);
            }
            finally
            {
                oPDTChk = null;
                oPDTStock = null;
                new cSP().SP_CLExMemory();
            }
            return null;
        }
    }
}
