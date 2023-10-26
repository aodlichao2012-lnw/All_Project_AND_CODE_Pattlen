using MQReceivePrc.Class.Standard;
using MQReceivePrc.Models.Config;
using MQReceivePrc.Models.Pos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQReceivePrc.Class
{
    public class cFixDoc
    {
        public bool C_PRCbFixDoc(cmlCheckListSalDoc poParam, cmlShopDB poShopDB, out string ptErrMsg)
        {
            string tMsg;
            string tConnStr;
            string tBchCode;
            string tPosCode;
            string tQResponse; //*Net 63-09-02 Q ที่จะส่งรายซ่อมกลับ
            cmlCheckListSalDoc oResFixDoc;
            List<cmlListSalDoc> aoFixDoc;
            try
            {
                ptErrMsg = "";
                if (poParam == null || poParam.paData == null || poParam.paData.Count == 0)
                {
                    ptErrMsg = $"List DocNo is Empty";
                    return true;
                }

                tBchCode = poParam.paData[0].ptFTBchCode;
                tPosCode = poParam.paData[0].ptFTPosCode;

                tConnStr = new cDatabase().C_GETtConnectString(cVB.oVB_ShopDB.tServer, cVB.oVB_ShopDB.tUser, cVB.oVB_ShopDB.tPassword, cVB.oVB_ShopDB.tDatabase, (int)cVB.oVB_ShopDB.nConnectTimeOut, cVB.oVB_ShopDB.tAuthenMode);
                aoFixDoc = C_GEToFixDoc(tConnStr, poParam.paData, (int)poShopDB.nCommandTimeOut);
                if (aoFixDoc == null || aoFixDoc.Count == 0)
                {
                    ptErrMsg = $"no Fix Doc.";
                    //return true; //Net 63-07-24 ถ้าไม่มีบิลซ่อมให้ส่ง list เป็น 0
                    aoFixDoc = new List<cmlListSalDoc>();
                }
                oResFixDoc = new cmlCheckListSalDoc();
                oResFixDoc.ptFunction = "Upload Doc No by Shift";
                oResFixDoc.ptSource = "HQ.MQRcvProcess";
                //oResFixDoc.ptDest = "POS.AdaStoreFront";
                oResFixDoc.ptFilter = poParam.paData[0].ptFTBchCode;
                oResFixDoc.paData = aoFixDoc;
                ptErrMsg = $"Bch{poParam.paData[0].ptFTBchCode} Pos{poParam.paData[0].ptFTPosCode} Fix {aoFixDoc.Count} Doc.";

                //*Net 63-08-26 ถ้าส่งมาจาก PosTools ให้กลับอีก Queue
                if (poParam.ptSource == "POS.PosTools")
                {
                    oResFixDoc.ptDest = "POS.PosTools";
                    tQResponse = $"PS_QPos{poParam.paData[0].ptFTBchCode}{poParam.paData[0].ptFTPosCode}";
                }
                else
                {
                    oResFixDoc.ptDest = "POS.AdaStoreFront";
                    tQResponse = $"PS_QFixDoc{poParam.paData[0].ptFTBchCode}{poParam.paData[0].ptFTPosCode}";
                }

                tMsg = JsonConvert.SerializeObject(oResFixDoc);
                //cFunction.C_PRCxMQPublish($"PS_QFixDoc{poParam.paData[0].ptFTBchCode}{poParam.paData[0].ptFTPosCode}", tMsg,out ptErrMsg);
                cFunction.C_PRCxMQPublish(tQResponse, tMsg, out ptErrMsg);

                return true;
            }
            catch (Exception oEx)
            {
                ptErrMsg = oEx.Message.ToString();
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_PRCbFixDoc");
            }
            finally
            {
                new cSP().SP_CLExMemory();
            }
            return false;
        }

        public List<cmlListSalDoc> C_GEToFixDoc(string ptConnStr, List<cmlListSalDoc> poPosDoc, int pnTimeOut)
        {
            StringBuilder oSql;
            cDatabase oDB;
            List<cmlListSalDoc> aoSrvDoc;
            List<cmlListSalDoc> aoPosDoc;
            try
            {
                oSql = new StringBuilder();
                oDB = new cDatabase();
                aoPosDoc = poPosDoc.ToList();
                oSql.Clear();
                oSql.AppendLine($"SELECT FTBchCode AS ptFTBchCode,FTShfCode AS ptFTShfCode,FTPosCode AS ptFTPosCode");
                oSql.AppendLine($",FDXshDocDate AS ptFDXshDocDate,FTXshDocNo AS ptFTBdhDocNo");
                oSql.AppendLine($"FROM TPSTSalHD HD WITH(NOLOCK)");
                oSql.AppendLine($"WHERE HD.FTBchCode='{aoPosDoc[0].ptFTBchCode}' AND HD.FTPosCode='{aoPosDoc[0].ptFTPosCode}'");
                oSql.AppendLine($" AND HD.FTShfCode='{aoPosDoc[0].ptFTShfCode}' AND HD.FTXshStaDoc='1'");
                aoSrvDoc = oDB.C_GETaDataQuery<cmlListSalDoc>(ptConnStr, oSql.ToString(), pnTimeOut);
                foreach (cmlListSalDoc oSrvDoc in aoSrvDoc)
                {
                    cmlListSalDoc oDoc = aoPosDoc.Where(oPosDoc => oPosDoc.ptFTBdhDocNo == oSrvDoc.ptFTBdhDocNo).FirstOrDefault();
                    if (oDoc != null)
                    {
                        int nIndex = aoPosDoc.IndexOf(oDoc);
                        if (nIndex >= 0)
                        {
                            aoPosDoc.RemoveAt(nIndex);
                        }
                    }
                }
                return aoPosDoc;
            }
            catch (Exception oEx)
            {
                cFunction.C_LOGxKeepLogErr(oEx.Message.ToString(), "C_GEToFixDoc");
            }
            finally
            {
                oSql = null;
                oDB = null;
            }
            return new List<cmlListSalDoc>();
        }
    }
}
