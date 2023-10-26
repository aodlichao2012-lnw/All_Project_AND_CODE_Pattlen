using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Request.Base;
using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     File manage.
    /// </summary>
    [RoutePrefix(cCS.tCS_APIVer + "/FileManage")]
    public class cFileManageController : ApiController
    {
        /// <summary>
        /// Create URL for download file.
        /// </summary>
        /// <param name="ptPathFile"></param>
        /// <returns></returns>
        [Route("CreateURL")]
        [HttpPost]
        public cmlResItem<string> POST_FLEtCreateUrlFile([FromBody] string ptPathFile)
        {
            string tPathDwn = "";
            FileInfo oFile;
            cmlResItem<string> aoResult;
            cMS oMsg =new cMS();
            string tPathFile = "";
            try
            {
                aoResult = new cmlResItem<string>();

                if (ptPathFile == null)
                {
                    aoResult.roItem = tPathDwn;
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
                tPathFile = ptPathFile;

                oFile = new FileInfo(tPathFile);
                if (oFile.Exists)
                {
                    tPathDwn = AppDomain.CurrentDomain.BaseDirectory + "FileSend";
                    if (!Directory.Exists(tPathDwn)) Directory.CreateDirectory(tPathDwn);
                    if (!File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        File.Copy(tPathFile, tPathDwn + @"\" + oFile.Name);
                    }
                    else
                    {
                        //*Arm 63-02-26

                        // ถ้ามีไฟล์อยู่ ให้ลบไฟล์เดิมทิ้งก่อน แล้ว Copy ไฟล์ใหม่มาวาง
                        File.Delete(Path.Combine(tPathDwn, oFile.Name));
                        File.Copy(tPathFile, tPathDwn + @"\" + oFile.Name);

                        //++++++++++++++++
                    }

                    if (File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        tPathDwn = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + @"/FileSend/" + oFile.Name;
                        
                        aoResult.roItem = tPathDwn;
                        aoResult.rtCode = oMsg.tMS_RespCode001;
                        aoResult.rtDesc = oMsg.tMS_RespDesc001;
                        return aoResult;
                    }
                    else
                    {
                        tPathDwn = "";
                        aoResult.roItem = tPathDwn;
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                else
                {
                    tPathDwn = "";
                    aoResult.roItem = tPathDwn;
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
            }
            catch (Exception oEx)
            {
                tPathDwn = "";
                aoResult = new cmlResItem<string>();
                aoResult.roItem = tPathDwn;
                aoResult.rtCode = oMsg.tMS_RespCode900;
                aoResult.rtDesc = oMsg.tMS_RespDesc900 + " : " + oEx.Message.ToString();
                return aoResult;
            }
        }
    }
}
