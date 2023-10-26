using AdaPos.Models.Webservice.Required.KADS.Customer;
using AdaPos.Models.Webservice.Respond.KADS.Customer;
using AdaPos.Resources_String.Local;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AdaPos.Class
{
    public class cServiceKADS
    {
        #region Service Api(3) Search Customer

        /// <summary>
        /// ค้นหาข้อมูลลูกค้า
        /// </summary>
        /// <param name="ptPhoneNumber"></param>
        /// <param name="ptCardID"></param>
        /// <returns></returns>
        public static cmlResCstKAD C_PRCtServiceCstSearch(string ptPhoneNumber = "", string ptCardID="")
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlResCstKAD oCst = new cmlResCstKAD();
            cSP oSP = new cSP();
            cLog oLog = new cLog();
            string tFunc = "";
            string tUrl = "";
            
            try
            {
                if (C_CHKbCheckParameter("C_PRCtServiceCstSearch") == false) return oCst;

                //Set request parameter
                if (!string.IsNullOrEmpty(ptPhoneNumber))
                {
                    tFunc = "?$filter=PhoneSearch eq '" + ptPhoneNumber + "'&$expand=PrivilegePointSet";
                }
                else
                {
                    tFunc = "?$filter=TaxID eq '" + ptCardID + "'&$expand=PrivilegePointSet";
                }

                //set url
                tUrl = cVB.tVB_ApiCstSch + tFunc;

                oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Set URL : " + tUrl, cVB.bVB_AlwPrnLog); // log Monitor
                oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Set Authorization : " + cVB.tVB_ApiCstSch_Auth, cVB.bVB_AlwPrnLog); // log Monitor

                oCall = new cClientService();
                oCall = new cClientService("Authorization", cVB.tVB_ApiCstSch_Auth);
                oRep = new HttpResponseMessage();

                // call service
                oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service start...", cVB.bVB_AlwPrnLog); // log Monitor
                try
                {
                    oRep = oCall.C_GEToInvoke(tUrl);
                    oRep.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException oEx)
                {
                    oSP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + "[" +oEx.Message.ToString()+"]", 2);
                    oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service Error/" + oEx.Message.ToString()); //log Error
                    return oCst=null;
                }
                catch (Exception oEx)
                {
                    oSP.SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2);
                    oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service Error/" + oEx.Message.ToString()); //log Error
                    return oCst=null;
                }
                oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service end...", cVB.bVB_AlwPrnLog); // log Monitor

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Response : " + tJSonRes, cVB.bVB_AlwPrnLog); // log Monitor
                    
                    oCst = JsonConvert.DeserializeObject<cmlResCstKAD>(tJSonRes);
                }

                oCall.C_PRCxCloseConn();
                return oCst;
            }
            catch (Exception oEx)
            {
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2);
                oLog.C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : " + oEx.Message);
                return oCst = null;
            }
            finally
            {
                oCall = null;
                oRep = null;
                oLog = null;
                oSP = null;
            }
            //return oCst;
        }

        /// <summary>
        /// ส่ง Create Customer
        /// </summary>
        /// <param name="poResKunnr"></param>
        /// <returns></returns>
        public static cmlResCreateCustomerCode C_PRCoBUGroup6(cmlResKunnr poResKunnr)
        {
            cmlResCreateCustomerCode oCstResp;
            cmlReqCustomerCreate oReqCst = new cmlReqCustomerCreate();
            try
            {
                oReqCst.CustomerCode = poResKunnr.CustomerCode;
                oReqCst.Title = poResKunnr.Title;
                oReqCst.FirstName = poResKunnr.FirstName;
                oReqCst.LastName = poResKunnr.LastName;
                oReqCst.Titlee = poResKunnr.Titlee;
                oReqCst.Namee = poResKunnr.Namee;
                oReqCst.Surnamee = poResKunnr.Surnamee;
                oReqCst.Addr = poResKunnr.Addr;
                oReqCst.Soi = poResKunnr.Soi;
                oReqCst.Street = poResKunnr.Street;
                oReqCst.District = poResKunnr.District;
                oReqCst.City = poResKunnr.City;
                oReqCst.Province = poResKunnr.Province;
                oReqCst.Gender = poResKunnr.Gender;
                oReqCst.Birth = poResKunnr.Birth;
                oReqCst.Email = poResKunnr.Email;
                oReqCst.Mobile = poResKunnr.Mobile;
                oReqCst.Remark = poResKunnr.Remark;
                //oReqCst.PhoneNo = poResKunnr.Mobile;        //*Arm 63-08-20
                oReqCst.PhoneNo = poResKunnr.PhoneNo;        //*Arm 63-08-31
                //oReqCst.PhoneSearch = poResKunnr.Mobile;    //*Arm 63-08-20
                oReqCst.PhoneSearch = poResKunnr.PhoneSearch;    //*Arm 63-08-31
                oReqCst.Membership = poResKunnr.Membership; //*Arm 63-08-20
                oReqCst.Point = poResKunnr.Point;           //*Arm 63-08-31
                oReqCst.BUGroup = poResKunnr.BUGroup;       //*Arm 63-08-31
                oReqCst.TaxID = poResKunnr.TaxID;           //*Arm 63-08-31
                oReqCst.KubotaID = poResKunnr.KubotaID;     //*Arm 63-08-31

                string tJsonCall = JsonConvert.SerializeObject(oReqCst);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCoBUGroup6 : Set JSON Request/" + tJsonCall, cVB.bVB_AlwPrnLog);

                new cLog().C_WRTxLog("cServiceKADS", "C_PRCoBUGroup6 : Search customer/C_PRCtCreateCustomer start.", cVB.bVB_AlwPrnLog); // log Monitor
                string tJSonRet = C_PRCtCreateCustomer(tJsonCall);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCoBUGroup6 : Search customer/C_PRCtCreateCustomer end.", cVB.bVB_AlwPrnLog); // log Monitor

                if (!string.IsNullOrEmpty(tJSonRet)) //*Arm 63-08-19
                {
                    oCstResp  = new cmlResCreateCustomerCode();
                    oCstResp = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRet);
                    if (oCstResp != null && oCstResp.d != null)
                    {
                        //if (oCstResp.d.BUGroup == "ZAR1")
                        //{

                        //}
                        //else
                        //{
                        //    switch (cVB.nVB_Language)
                        //    {
                        //        case 1:     // TH
                        //            new cSP().SP_SHWxMsg(new ResourceManager(typeof(resPopup_TH)).GetString("tMsgPrcCreateCstCode") + " (BUGroup : " + oCstResp.d.BUGroup + ")", 3);
                        //            break;

                        //        default:    // EN
                        //            new cSP().SP_SHWxMsg(new ResourceManager(typeof(resPopup_EN)).GetString("tMsgPrcCreateCstCode") + " (BUGroup : " + oCstResp.d.BUGroup + ")", 3);
                        //            break;
                        //    }
                        //    oCstResp = null;
                        //}
                    }
                    else
                    {
                        oCstResp = null;
                    }
                }
                else
                {
                    oCstResp = null; //*Arm 63-08-20
                }

                new cLog().C_WRTxLog("cServiceKADS", "C_PRCoBUGroup6/call service end...", cVB.bVB_AlwPrnLog);

            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCoBUGroup6 Error/" + oEx.Message);
                oCstResp = null;
            }
            return oCstResp;
        }

        /// <summary>
        /// สร้างลูกค้าใหม่ กรณี ZAR6
        /// </summary>
        /// <param name="ptReqMsg"></param>
        /// <returns></returns>
        public static string C_PRCtCreateCustomer(string ptReqMsg)
        {
            NetworkCredential oCredentials;
            HttpWebRequest oReq;
            HttpWebResponse oResp;
            string tResult = "";
            string tErr = "";
            try
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer start...", cVB.bVB_AlwPrnLog); //log monitor

                if (C_CHKbCheckParameter("C_PRCxCreateCustomer") == false) return tResult;

                if (string.IsNullOrEmpty(cVB.tVB_Token_CreateCstCode) || cVB.oVB_Cookie_CreateCstCode == null)
                {
                    //ถ้า Token หรือ Cookie ไม่มีค่า ให้ส่งขอ Token ใหม่
                    
                    string tReplace = cVB.tVB_ApiGetToken_Auth.Replace("Basic", "");
                    byte[] data = System.Convert.FromBase64String(tReplace);
                    string tAuthen = System.Text.ASCIIEncoding.ASCII.GetString(data);
                    string[] aAuth = tAuthen.Split(':');
                    string tUsername = aAuth[0].ToString();
                    string tPassword = aAuth[1].ToString();

                    // Setup network credentials object to be used for requests to Gateway server
                    oCredentials = new System.Net.NetworkCredential(tUsername, tPassword);

                    // Create Gateway objects
                    oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiGetToken);
                    // Add custom header request to fetch the CSRF token
                    oReq.Credentials = oCredentials;
                    oReq.Method = "GET";
                    oReq.Headers.Add("X-CSRF-Token", "Fetch");

                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/url : "+ cVB.tVB_ApiGetToken, cVB.bVB_AlwPrnLog); 
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/method : GET", cVB.bVB_AlwPrnLog); 
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/user : " + tUsername, cVB.bVB_AlwPrnLog); 
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/password : " + tPassword, cVB.bVB_AlwPrnLog); 
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/Authorization : " + cVB.tVB_ApiGetToken_Auth, cVB.bVB_AlwPrnLog); 
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/X-CSRF-Token : Fetch" , cVB.bVB_AlwPrnLog);

                    // Setup cookie jar to capture cookies coming back from Gateway server. These cookies are needed along with the CSRF token for modifying requests.
                    cVB.oVB_Cookie_CreateCstCode = new CookieContainer();
                    oReq.CookieContainer = cVB.oVB_Cookie_CreateCstCode;

                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/call api start...", cVB.bVB_AlwPrnLog); //log monitor
                    try
                    {
                        oResp = (HttpWebResponse)oReq.GetResponse();
                    }
                    catch (System.Net.WebException oEx)
                    {
                        // Add your error handling here
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine +"[" + oEx.Message.ToString() + "]", 2);
                        new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/call api Error : " + oEx.Message);
                        return tResult;
                    }
                    catch (Exception oEx)
                    {
                        // Add your error handling here
                        new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2);
                        new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/call api Error : " + oEx.Message);
                        return tResult;
                    }
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer : service get token/call api end...", cVB.bVB_AlwPrnLog); //log monitor

                    // Assign values from response to class variables.
                    cVB.tVB_Token_CreateCstCode = oResp.Headers.Get("X-CSRF-Token");
                    new cLog().C_WRTxLog("cServiceKADS", "oC_PRCxCreateCustomer : service get token/Response X-CSRF-Token : " + cVB.tVB_Token_CreateCstCode, cVB.bVB_AlwPrnLog);
                    
                }

                if (!string.IsNullOrEmpty(cVB.tVB_Token_CreateCstCode))
                {
                    new cLog().C_WRTxLog("cServiceKADS", "oC_PRCxCreateCustomer : process create customer/start..." + cVB.tVB_Token_CreateCstCode, cVB.bVB_AlwPrnLog);
                    tResult = C_PRCtServiceCreateCustomer(cVB.oVB_Cookie_CreateCstCode, cVB.tVB_Token_CreateCstCode, ptReqMsg, out tErr);
                    new cLog().C_WRTxLog("cServiceKADS", "oC_PRCxCreateCustomer : process create customer/end... " + cVB.tVB_Token_CreateCstCode, cVB.bVB_AlwPrnLog);
                    
                    //*Arm 63-08-09
                    if(!string.IsNullOrEmpty(tErr))
                    {
                        new cSP().SP_SHWxMsg("[" + tErr + "]" + Environment.NewLine + cVB.oVB_GBResource.GetString("tMsgCantCreateCst"), 2);
                    }
                    //+++++++++++++
                }
                else
                {
                    new cLog().C_WRTxLog("cServiceKADS", "oC_PRCxCreateCustomer : can not call service Create Customer because service get token Response X-CSRF-Token is empty !!! ");
                }
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer end...", cVB.bVB_AlwPrnLog); //log monitor
            }
            catch (Exception oEx)
            {
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + " (" + oEx.Message.ToString() + ")", 2);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCxCreateCustomer Error:" + oEx.Message);
                return tResult;
            }
            finally
            {
                oCredentials = null;
                oResp = null;
                oReq = null;
                ptReqMsg = null;
            }
            return tResult;
        }

        /// <summary>
        /// Call api สำหรับสร้างลูกค้า
        /// </summary>
        /// <param name="poCookieJar"></param>
        /// <param name="ptToken"></param>
        /// <param name="ptBody"></param>
        /// <returns></returns>
        public static string C_PRCtServiceCreateCustomer(CookieContainer poCookieJar, string ptToken, string ptBody, out string ptErr)
        {
            string tMessage = "";
            string tCookie = "";

            HttpWebRequest oReq;
            NetworkCredential oCredentials;
            CookieCollection oCookies;

            try
            {
                ptErr = "";
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer start...", cVB.bVB_AlwPrnLog);
                if (C_CHKbCheckParameter("C_PRCtServiceCstSearch") == false) return tMessage;

                string tReplace = cVB.tVB_ApiCstSch_Auth.Replace("Basic", ""); //*Arm 63-08-09
                byte[] data = System.Convert.FromBase64String(tReplace);
                string tAuthen = System.Text.ASCIIEncoding.ASCII.GetString(data);
                string[] aAuth = tAuthen.Split(':');
                string tUsername = aAuth[0].ToString();
                string tPassword = aAuth[1].ToString();

                // Setup network credentials object to be used for requests to Gateway server
                oCredentials = new System.Net.NetworkCredential(tUsername, tPassword);

                // Create Gateway objects
                oReq = (HttpWebRequest)HttpWebRequest.Create(cVB.tVB_ApiCstSch); //*Arm 63-08-09
                oReq.Credentials = oCredentials;
                oReq.Method = "POST";
                oReq.ContentType = "application/json";
                oReq.Headers.Add("X-CSRF-Token", ptToken);
                oReq.Accept = "application/json";

                //Uri oUri = new Uri(cVB.tVB_APIKADS_ReqToken_Url);
                Uri oUri = new Uri(cVB.tVB_ApiGetToken); //*Arm 63-08-09

                oCookies = poCookieJar.GetCookies(oUri);
                foreach (Cookie cookie in oCookies)
                {
                    tCookie = tCookie + ";" + cookie.ToString();
                }
                oReq.Headers.Add("Cookie", tCookie.Substring(1));

                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/url : " + cVB.tVB_ApiCstSch, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/method : POS", cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/user : " + tUsername, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/password : " + tPassword, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/Authorization : " + cVB.tVB_ApiCstSch_Auth, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/X-CSRF-Token : " + ptToken, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/Cookie : " + tCookie, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/request : " + ptBody, cVB.bVB_AlwPrnLog);

                var oData = Encoding.UTF8.GetBytes(ptBody);
                using (var oStream = oReq.GetRequestStream())
                {
                    oStream.Write(oData, 0, oData.Length);
                }

                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/call api start... ", cVB.bVB_AlwPrnLog);
                try
                {
                    using (HttpWebResponse oResp = (HttpWebResponse)oReq.GetResponse())
                    {
                        using (StreamReader oRd = new StreamReader(oResp.GetResponseStream()))
                        {
                            //*Arm 63-08-20
                            try
                            {
                                string tRetType = "";
                                tRetType = oResp.Headers.Get("ret-type");
                                if (tRetType == "E")
                                {
                                    ptErr = oResp.Headers.Get("ret-message");
                                }
                                else
                                {
                                    tMessage = oRd.ReadToEnd();
                                }
                            }
                            catch(Exception oEx)
                            {
                                tMessage = oRd.ReadToEnd();
                            }
                            //++++++++++++++
                            oRd.Close();
                        }
                        oResp.Close();
                    }
                }
                catch (System.Net.WebException oEx)
                {
                    // Add your error handling here
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + " (" + oEx.Message.ToString() + ")", 2);
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/Error : " + oEx.Message);
                    return tMessage;
                }
                catch (Exception oEx)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") , 2);
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/Error : " + oEx.Message);
                    return tMessage;
                }
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/call api end... ", cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : service create Customer/response : "+ tMessage, cVB.bVB_AlwPrnLog);
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer start...", cVB.bVB_AlwPrnLog);
            }
            catch (Exception oEx)
            {
                //new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + " (" + oEx.Message.ToString() + ")", 2);
                new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"),2); //*Arm 63-08-19 
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtServiceCreateCustomer : Error " + oEx.Message);
                ptErr = "";
                return tMessage;
            }
            finally
            {
                oReq = null;
                oCredentials = null;
                oCookies = null;
                poCookieJar = null;
            }
            return tMessage;
        }

        /// <summary>
        /// เก็บข้อมูล Privilege ของลูกค้าลงตาราง TTmpCstSearch
        /// </summary>
        /// <param name="poDbTbl"></param>
        public static void C_INTxInsertPrivilege2Tmp(DataTable poDbTbl)
        {
            StringBuilder oSql = new StringBuilder();
            cDatabase oDb = new cDatabase();
            try
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog); // log Monitor
                oSql.Clear();
                oSql.AppendLine("IF NOT EXISTS(SELECT name FROM sys.tables WHERE OBJECT_ID = object_id('TTmpCstSearch'))");
                oSql.AppendLine("BEGIN ");
                oSql.AppendLine("    CREATE TABLE[dbo].[TTmpCstSearch](");
                oSql.AppendLine("        [KubotaId][varchar](50) NULL,");
                oSql.AppendLine("        [MatNo] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyPrt] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyUse] [varchar] (50) NULL,");
                oSql.AppendLine("        [QtyBal] [varchar] (50) NULL,");
                oSql.AppendLine("        [MatUnit] [varchar] (50) NULL");
                oSql.AppendLine("    ) ON[PRIMARY]");
                oSql.AppendLine("END");
                oSql.AppendLine("TRUNCATE TABLE TTmpCstSearch");
                oDb.C_SETxDataQuery(oSql.ToString());
                
                using (SqlBulkCopy oBulkCopy = new SqlBulkCopy(cVB.tVB_ConStr, SqlBulkCopyOptions.Default))
                {
                    foreach (DataColumn oColName in poDbTbl.Columns)
                    {
                        oBulkCopy.ColumnMappings.Add(oColName.ColumnName, oColName.ColumnName);
                    }
                    oBulkCopy.BatchSize = 100;
                    oBulkCopy.DestinationTableName = "dbo.TTmpCstSearch";

                    try
                    {
                        oBulkCopy.WriteToServer(poDbTbl);
                    }
                    catch (Exception oEx)
                    {
                        new cLog().C_WRTxLog("cServiceKADS", "C_INTxInsertPrivilege2Tmp/BulkCopy dbo.TTmpCstSearch Error : " + oEx.Message.ToString());
                    }
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_INTxInsertPrivilege2Tmp : " + oEx.Message);
            }
        }

        #endregion Service Api(3) Search Customer

        public static bool C_CHKbCheckParameter(string ptService)
        {
            try
            {
                switch(ptService)
                {
                    case "C_PRCtServiceCstSearch":
                        //Check url
                        if(string.IsNullOrEmpty(cVB.tVB_ApiCstSch))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlCstSchNotDefine"), 3);
                            return false;
                        }
                        //check Authorization
                        if (string.IsNullOrEmpty(cVB.tVB_ApiCstSch_Auth))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAuthNotDefine"), 3);
                            return false;
                        }
                        break;

                    case "C_PRCxCreateCustomer":
                        //Check url
                        if (string.IsNullOrEmpty(cVB.tVB_ApiGetToken))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgUrlGetTokenNotDefine"), 3);
                            return false;
                        }
                        //check Authorization
                        if (string.IsNullOrEmpty(cVB.tVB_ApiGetToken_Auth))
                        {
                            new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgAuthNotDefine"), 3);
                            return false;
                        }
                        break;
                }
                return true;
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_CHKbCheckParameter/CheckParameter(" + ptService + ") : " + oEx.Message);
                return false;
            }
        }

        public static bool C_GETxHDCst(string ptPhone, string ptCardID)
        {
            cmlResCstKAD oResCst;
            DataTable odtPrivil;
            string tReqParam = "";
            bool bStaResult = false;
            try
            {
                if (string.IsNullOrEmpty(ptPhone) && string.IsNullOrEmpty(ptCardID))
                {
                    //ไม่ได้กรอกข้อมูล
                    new cLog().C_WRTxLog("wCstSearch", "ocmSearch_Click : No enter data for search.", cVB.bVB_AlwPrnLog);
                    return false;
                }
                else
                {
                    new cLog().C_WRTxLog("cSale", "C_GETxHDCst : C_PRCtServiceCstSearch start... ", cVB.bVB_AlwPrnLog);
                    //oResCst = cServiceKADS.C_PRCtServiceCstSearch(tReqParam);   //ค้นหาลูกค้า
                    oResCst = cServiceKADS.C_PRCtServiceCstSearch(ptPhone, ptCardID);   //ค้นหาลูกค้า
                    new cLog().C_WRTxLog("cSale", "C_GETxHDCst : C_PRCtServiceCstSearch end... ", cVB.bVB_AlwPrnLog);

                    if (oResCst != null && oResCst.d != null && oResCst.d.results != null && oResCst.d.results.Count > 0)
                    {
                        //มีข้อมูล
                        foreach (cmlResKunnr oResKunnr in oResCst.d.results)
                        {
                            //loop เช้ค CustomerCode ตรงกัน
                            if (oResKunnr.CustomerCode == cVB.tVB_CstCode)
                            {
                                if (oResKunnr.PrivilegePointSet != null && oResKunnr.PrivilegePointSet.results != null && oResKunnr.PrivilegePointSet.results.Count > 0)
                                {
                                    //บันทึก Privilege ลง Table Temp
                                    string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oResKunnr.PrivilegePointSet.results);
                                    odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                                    new cLog().C_WRTxLog("cSale", "C_GETxHDCst : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                                    cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                                    new cLog().C_WRTxLog("cSale", "C_GETxHDCst : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                                }
                                cVB.nVB_CstPoint = oResKunnr.Point;
                                cVB.nVB_CstPiontB4Used = oResKunnr.Point;
                                cVB.tVB_CstName = oResKunnr.Title + " " + oResKunnr.FirstName + " " + oResKunnr.LastName;
                                cVB.tVB_KubotaID = oResKunnr.KubotaID;
                                cVB.tVB_CstCode = oResKunnr.CustomerCode;
                                cVB.tVB_CstTel = oResKunnr.PhoneNo;
                                cVB.tVB_CstCardID = oResKunnr.TaxID;

                                if (string.IsNullOrEmpty(oResKunnr.Membership))
                                {
                                    cVB.tVB_MemCode = "";
                                    cVB.bVB_Flag = false;
                                }
                                else
                                {
                                    cVB.tVB_MemCode = oResKunnr.Membership;
                                    cVB.bVB_Flag = true;
                                }

                                cVB.oVB_Sale.W_SETxTextCst(); //show ข้อมูลลูกค้า

                                new cLog().C_WRTxLog("cSale", "C_GETxHDCst : C_DATxInsHDCst start...", cVB.bVB_AlwPrnLog);
                                cSale.C_DATxInsHDCst(cVB.tVB_CstCode); //Insert ข้อมูลลูกค้า
                                bStaResult = true;
                                break;
                            }
                            else
                            {
                                new cLog().C_WRTxLog("cSale", "C_GETxHDCst : Customer infomation not found.", cVB.bVB_AlwPrnLog);
                            }
                        }//foreach
                    }
                    else
                    {
                        new cLog().C_WRTxLog("cSale", "C_GETxHDCst : Customer infomation not found.", cVB.bVB_AlwPrnLog);
                    }
                }
            }
            catch(Exception oEx)
            {
                new cLog().C_WRTxLog("cSale", "C_GETxHDCst : Error/"+ oEx.Message);
            }
            return bStaResult;
        }

        public static bool C_GETxHDCst(string ptCstCode)
        {
            cClientService oCall;
            HttpResponseMessage oRep;
            cmlResCreateCustomerCode oResCst;
            DataTable odtPrivil;
            string tFunc = "";
            string tUrl = "";
            bool bStaResult = false;
            try
            {
                if (C_CHKbCheckParameter("C_PRCtServiceCstSearch") == false) return false;
                oResCst = new cmlResCreateCustomerCode();

                //Set request parameter
                if (!string.IsNullOrEmpty(ptCstCode))
                {
                    tFunc = "('"+ ptCstCode + "')?$expand=PrivilegePointSet";
                }

                //set url
                tUrl = cVB.tVB_ApiCstSch + tFunc;

                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Set URL : " + tUrl, cVB.bVB_AlwPrnLog); // log Monitor
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Set Authorization : " + cVB.tVB_ApiCstSch_Auth, cVB.bVB_AlwPrnLog); // log Monitor

                oCall = new cClientService();
                oCall = new cClientService("Authorization", cVB.tVB_ApiCstSch_Auth);
                oRep = new HttpResponseMessage();

                // call service
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service start...", cVB.bVB_AlwPrnLog); // log Monitor
                try
                {
                    oRep = oCall.C_GEToInvoke(tUrl);
                    oRep.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException oEx)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS") + Environment.NewLine + "[" + oEx.Message.ToString() + "]", 2);
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service Error/" + oEx.Message.ToString()); //log Error
                    return false;
                }
                catch (Exception oEx)
                {
                    new cSP().SP_SHWxMsg(cVB.oVB_GBResource.GetString("tMsgCantServiceKADS"), 2);
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service Error/" + oEx.Message.ToString()); //log Error
                    return false;
                }
                new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/call service end...", cVB.bVB_AlwPrnLog); // log Monitor

                if (oRep.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string tJSonRes = oRep.Content.ReadAsStringAsync().Result;
                    new cLog().C_WRTxLog("cServiceKADS", "C_PRCtCstSearch : Search customer/Response : " + tJSonRes, cVB.bVB_AlwPrnLog); // log Monitor

                    oResCst = JsonConvert.DeserializeObject<cmlResCreateCustomerCode>(tJSonRes);
                }
                oCall.C_PRCxCloseConn();

                // insert data
                if (oResCst != null && oResCst.d !=null)
                {
                    cVB.nVB_CstPoint = oResCst.d.Point;
                    cVB.nVB_CstPiontB4Used = oResCst.d.Point;
                    cVB.tVB_CstName = oResCst.d.Title + " " + oResCst.d.FirstName + " " + oResCst.d.LastName;
                    cVB.tVB_KubotaID = oResCst.d.KubotaID;
                    cVB.tVB_CstCode = oResCst.d.CustomerCode;
                    cVB.tVB_CstTel = oResCst.d.PhoneNo;
                    cVB.tVB_CstCardID = oResCst.d.TaxID;

                    if (string.IsNullOrEmpty(oResCst.d.Membership))
                    {
                        cVB.tVB_MemCode = "";
                        cVB.bVB_Flag = false;
                    }
                    else
                    {
                        cVB.tVB_MemCode = oResCst.d.Membership;
                        cVB.bVB_Flag = true;
                    }

                    cVB.oVB_Sale.W_SETxTextCst(); //show ข้อมูลลูกค้า
                    new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : C_DATxInsHDCst start...", cVB.bVB_AlwPrnLog);
                    cSale.C_DATxInsHDCst(cVB.tVB_CstCode); //Insert ข้อมูลลูกค้า
                    new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : C_DATxInsHDCst end...", cVB.bVB_AlwPrnLog);

                    //Privilege
                    if (oResCst.d.PrivilegePointSet != null && oResCst.d.PrivilegePointSet.results != null && oResCst.d.PrivilegePointSet.results.Count > 0)
                    {
                        //บันทึก Privilege ลง Table Temp
                        string tJsonPrivilReq = Newtonsoft.Json.JsonConvert.SerializeObject(oResCst.d.PrivilegePointSet.results);
                        odtPrivil = JsonConvert.DeserializeObject<DataTable>(tJsonPrivilReq);

                        new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : C_INTxInsertPrivilege2Tmp start...", cVB.bVB_AlwPrnLog);
                        cServiceKADS.C_INTxInsertPrivilege2Tmp(odtPrivil);
                        new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : C_INTxInsertPrivilege2Tmp end...", cVB.bVB_AlwPrnLog);
                    }

                    bStaResult = true;
                }
                else
                {
                    new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : Customer infomation not found.", cVB.bVB_AlwPrnLog);
                }
            }
            catch (Exception oEx)
            {
                new cLog().C_WRTxLog("cServiceKADS", "C_GETxHDCst : Error/" + oEx.Message);
            }
            finally
            {
                oCall = null;
                oRep = null;
                oResCst = null;
                ptCstCode = "";
                tUrl = "";
                tFunc = "";
                odtPrivil = null;
            }
            return bStaResult;
        }
    }
}
