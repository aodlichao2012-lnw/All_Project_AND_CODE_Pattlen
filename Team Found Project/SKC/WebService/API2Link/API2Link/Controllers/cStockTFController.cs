using API2Link.Class.Standard;
using API2Link.Models.Webservice.StockTransfer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace API2Link.Controllers
{
    public class cStockTFController : ApiController
    {
        //[Route("StockTransfer")]
        
        //[HttpPost]
        //public cmlResStkTF SKC_StockTransfer(cmlReqStkTF oReqStk)
        //{
        //    cmlResStkTF oResStk = new cmlResStkTF();
        //    if (SP_GETtHeader(Request))
        //    {
                
        //        cRabbitMQ oRabbitMQ = new cRabbitMQ();
        //        cmlReqStkTFToBK oModel;
        //        cmlData oModelData;
        //        string tMes = "";
        //        string tMesToMQ = "";


        //        //HttpHeadAttribute oResp = new HttpHeadAttribute();
        //        //string tXKey = oResp.HttpMethods.GetEnumerator("X-Key");

        //        try
        //        {

        //            if (string.IsNullOrEmpty(oReqStk.MatDocNo))
        //            {
        //                oResStk.ErrCode = "701";
        //                oResStk.ErrDesc = "validate parameter model false.";
        //                return oResStk;
        //            }

        //            tMes = JsonConvert.SerializeObject(oReqStk);
        //            oModelData = new cmlData();
        //            oModelData.ptFilter = tMes;
        //            oModelData.ptDateFrm = DateTime.Now.ToString("yyyyMMdd");
        //            oModelData.ptDateTo = DateTime.Now.ToString("yyyyMMdd");

        //            oModel = new cmlReqStkTFToBK();
        //            oModel.ptFunction = "STKTF";
        //            oModel.ptSource = "AdaLink";
        //            oModel.ptDest = "BQ Process";
        //            oModel.ptData = oModelData;
        //            tMesToMQ = JsonConvert.SerializeObject(oModel);
        //            if (oRabbitMQ.C_GETbLoadConfigMQ())
        //            {
        //                if (oRabbitMQ.C_PRCxMQPublish(cVB.oVB_RabbitMQ.tMQListQueue, tMesToMQ))
        //                {
        //                    oResStk.ErrCode = "001";
        //                    oResStk.ErrDesc = "success";
        //                }
        //                else
        //                {
        //                    oResStk.ErrCode = "905";
        //                    oResStk.ErrDesc = "cannot connect database.";
        //                    new cLog().C_PRCxLog("SKC_StockTransfer", "Rabbit MQ Publish Error");
        //                }
        //            }
        //            else
        //            {
        //                new cLog().C_PRCxLog("SKC_StockTransfer", "Get Config Rabbit MQ Error");
        //            }
        //        }
        //        catch (Exception oEx)
        //        {
        //            new cLog().C_PRCxLog("SKC_StockTransfer", oEx.Message.ToString());
        //        }
        //        return oResStk;
        //    }
        //    else
        //    {
        //        throw new HttpResponseException(HttpStatusCode.Forbidden);
        //    }
        //    return oResStk;
        //}

        //public static bool SP_GETtHeader(HttpRequestMessage poRequest)
        //{
        //    bool bStatus = false;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(poRequest.Headers.GetValues("X-Key").FirstOrDefault()))
        //        {
        //            string tXKey = poRequest.Headers.GetValues("X-Key").FirstOrDefault();
        //            if (tXKey == "524aa1ebc230770ed5553bf0a50008449e734fd3f1010ac5c56b5b3003ff8e39")
        //            {
        //                bStatus = true;
        //            }
        //        }
        //        return bStatus;
        //    }
        //    catch (Exception oEx) { throw new HttpResponseException(HttpStatusCode.Forbidden); }
            
            
            
        //}
    }
}
