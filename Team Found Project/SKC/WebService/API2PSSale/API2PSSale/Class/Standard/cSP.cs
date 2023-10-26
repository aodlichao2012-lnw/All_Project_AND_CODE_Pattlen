using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.ModelBinding;

namespace API2PSSale.Class.Standard
{
    /// <summary>
    /// Class Function
    /// </summary>
    public class cSP
    {
        /// <summary>
        /// Check API Key 
        /// </summary>
        /// <param name="poHttpContext"></param>
        /// <param name="ptErrAPI"></param>
        /// <returns></returns>
        public bool SP_CHKbKeyApi(HttpContext poHttpContext, out string ptErrAPI)
        {
            NameValueCollection oReqHeaders;
            string tClientAPIKey, tConfigAPIKey;
            cDatabase oDatabase;
            StringBuilder oSQL;
            var oCache = MemoryCache.Default;
            var oCM_cachePolicty = new CacheItemPolicy();
            try
            {
                oDatabase = new cDatabase();
                oSQL = new StringBuilder();
                tConfigAPIKey = "";

                oCM_cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);

                oReqHeaders = poHttpContext.Request.Headers;
                tClientAPIKey = oReqHeaders.Get("X-Api-Key");

                // Get Config API Key
                if (oCache.Get("cache_API") == null)
                {
                    oSQL.AppendLine("SELECT ISNULL(FTSysStaUsrValue,'') AS FTSysStaUsrValue ");
                    oSQL.AppendLine("FROM  TSysConfig WITH (NOLOCK) WHERE FTSysKey='POS' AND FTSysSeq='1' AND FTSysCode='tCN_AgnKeyAPI'");
                    tConfigAPIKey = oDatabase.C_GETtSQLScalarString(oSQL.ToString());

                    if (tConfigAPIKey == "-1")
                    {
                        ptErrAPI = tConfigAPIKey;
                        return false;
                    }
                    oCache.Set("cache_API", tConfigAPIKey, oCM_cachePolicty);
                }
                else
                {
                    tConfigAPIKey = (string)oCache.Get("cache_API");
                }

                if (tConfigAPIKey == "")
                {
                    ptErrAPI = "";
                    return false;
                }

                if (tClientAPIKey != tConfigAPIKey)
                {
                    ptErrAPI = "";
                    return false;
                }

                ptErrAPI = "";
                return true;
            }
            catch (Exception)
            {
                ptErrAPI = "";
                return false;
            }
            finally
            {
                oSQL = null;
                oCache = null;
                oCM_cachePolicty = null;
                oDatabase = null;
                oReqHeaders = null;
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Net 63-06-15 Arm 63-07-31 ยกมาจาก Moshi
        /// Check API Key from Config
        /// </summary>
        /// <param name="poHttpContext"></param>
        /// <param name="ptErrAPI"></param>
        /// <returns></returns>
        public bool SP_CHKbKeyApiConfig(HttpContext poHttpContext, out string ptErrAPI)
        {
            NameValueCollection oReqHeaders;
            string tClientAPIKey, tConfigAPIKey;
            StringBuilder oSQL;
            var oCache = MemoryCache.Default;
            var oCM_cachePolicty = new CacheItemPolicy();
            var oAppSetting = ConfigurationManager.AppSettings;
            try
            {
                oSQL = new StringBuilder();
                tConfigAPIKey = "";

                oCM_cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);

                oReqHeaders = poHttpContext.Request.Headers;
                tClientAPIKey = oReqHeaders.Get("X-Api-Key");

                // Get Config API Key
                if (oCache.Get("cache_APIConfig") == null)
                {
                    tConfigAPIKey = cSP.SP_DATtTripleDESDecryptData(oAppSetting.Get("Access"), cCS.tCS_SHA1Key2);

                    if (tConfigAPIKey == "-1")
                    {
                        ptErrAPI = tConfigAPIKey;
                        return false;
                    }
                    oCache.Set("cache_APIConfig", tConfigAPIKey, oCM_cachePolicty);
                }
                else
                {
                    tConfigAPIKey = (string)oCache.Get("cache_APIConfig");
                }

                if (tConfigAPIKey == "")
                {
                    ptErrAPI = "";
                    return false;
                }

                if (tClientAPIKey != tConfigAPIKey)
                {
                    ptErrAPI = "";
                    return false;
                }

                ptErrAPI = "";
                return true;
            }
            catch (Exception)
            {
                ptErrAPI = "";
                return false;
            }
            finally
            {
                oSQL = null;
                oCache = null;
                oCM_cachePolicty = null;
                oReqHeaders = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Check model parameter
        /// </summary>
        /// <param name="ptModelErr"></param>
        /// <param name="poModelState"></param>
        /// <returns></returns>
        public bool SP_CHKbParaModel(ref string ptModelErr, ModelStateDictionary poModelState)
        {
            try
            {
                if (poModelState.IsValid)
                {
                    // Validate pass.
                    return true;
                }
                else
                {
                    // Validate false.
                    IEnumerable<string> atErrList = from oState in poModelState.Values
                                                    from oError in oState.Errors
                                                    where !string.IsNullOrEmpty(oError.ErrorMessage)
                                                    select oError.ErrorMessage;

                    ptModelErr = string.Join("|", atErrList);
                }
            }
            catch (Exception) { }
            finally
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
            return false;
        }

        /// <summary>
        /// Convert Full path image to base 64 
        /// </summary>
        /// <param name="ptPath"></param>
        /// <returns>string</returns>
        public string SP_GETtPath2Base64(string ptPath)
        {
            string tBase64;
            try
            {
                using (Image oImg = Image.FromFile(ptPath))
                {
                    using (MemoryStream oMem = new MemoryStream())
                    {
                        oImg.Save(oMem, oImg.RawFormat);
                        byte[] anBytes = oMem.ToArray();
                        tBase64 = Convert.ToBase64String(anBytes);
                    }
                }
                return tBase64;
            }
            catch (Exception) { return ""; }
            finally
            {
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// AES128 Decrypt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptEncryptedText"></param>
        /// <param name="ptKey"></param>
        /// <param name="ptIV"></param>
        /// <returns></returns>
        public T SP_DAToAES128Decrypt<T>(string ptEncryptedText, string ptKey, string ptIV)
        {
            try
            {
                ptEncryptedText.Replace(@"\", "");

                var tEncryptedBytes = Convert.FromBase64String(ptEncryptedText);
                var oRijndaelManaged = SP_GEToRijndaelManaged(ptKey, ptIV);
                if ((oRijndaelManaged != null))
                {
                    var aDecrypt = SP_CALaASE128Decrypt(tEncryptedBytes, SP_GEToRijndaelManaged(ptKey, ptIV));
                    if ((aDecrypt != null))
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(aDecrypt));
                }
            }
            catch (Exception ex)
            {
            }

            return default(T);
        }

        /// <summary>
        /// CAL ASE128 Decrypt
        /// </summary>
        /// <param name="paEncryptedData"></param>
        /// <param name="poRijndaelManaged"></param>
        /// <returns></returns>
        private static byte[] SP_CALaASE128Decrypt(byte[] paEncryptedData, RijndaelManaged poRijndaelManaged)
        {
            try
            {
                return poRijndaelManaged.CreateDecryptor().TransformFinalBlock(paEncryptedData, 0, paEncryptedData.Length);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Rijndael
        /// </summary>
        /// <param name="ptSecretKey"></param>
        /// <param name="ptSecretIV"></param>
        /// <returns></returns>
        private static RijndaelManaged SP_GEToRijndaelManaged(string ptSecretKey, string ptSecretIV)
        {
            byte[] aKeyBytes;
            byte[] aIVBytes;
            byte[] aSecretKeyBytes;
            byte[] aSecretIVBytes;
            RijndaelManaged oRijndaelManaged;
            try
            {
                aKeyBytes = new byte[16];
                aSecretKeyBytes = Encoding.UTF8.GetBytes(ptSecretKey);
                Array.Copy(aSecretKeyBytes, aKeyBytes, Math.Min(aKeyBytes.Length, aSecretKeyBytes.Length));

                aIVBytes = new byte[16];
                aSecretIVBytes = Encoding.UTF8.GetBytes(ptSecretIV);
                Array.Copy(aSecretIVBytes, aIVBytes, Math.Min(aIVBytes.Length, aSecretIVBytes.Length));

                oRijndaelManaged = new RijndaelManaged();
                oRijndaelManaged.Mode = CipherMode.CBC;
                // oRijndaelManaged.Padding = PaddingMode.Zeros
                oRijndaelManaged.Padding = PaddingMode.PKCS7;
                oRijndaelManaged.KeySize = 128;
                oRijndaelManaged.BlockSize = 128;
                oRijndaelManaged.Key = aKeyBytes;
                oRijndaelManaged.IV = aIVBytes;
                return oRijndaelManaged;
            }
            catch (Exception ex) { }
            finally { }
            return null/* TODO Change to default(_) if this is not a reference type */;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptEncryptedText"></param>
        /// <param name="ptKey"></param>
        /// <returns></returns>
        public static string SP_DATtAES128Decrypt(string ptEncryptedText, string ptKey)
        {
            try
            {
                var tEncryptedBytes = Convert.FromBase64String(ptEncryptedText);
                var oRijndaelManaged = SP_GEToRijndaelManaged(ptKey);
                if ((oRijndaelManaged != null))
                {
                    var aDecrypt = SP_CALaASE128Decrypt(tEncryptedBytes, SP_GEToRijndaelManaged(ptKey));
                    if ((aDecrypt != null))
                        return Encoding.UTF8.GetString(aDecrypt);
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        private static RijndaelManaged SP_GEToRijndaelManaged(string ptSecretKey)
        {
            byte[] aKeyBytes;
            byte[] aSecretKeyBytes;
            RijndaelManaged oRijndaelManaged;

            try
            {
                aKeyBytes = new byte[16];
                aSecretKeyBytes = Encoding.UTF8.GetBytes(ptSecretKey);
                Array.Copy(aSecretKeyBytes, aKeyBytes, Math.Min(aKeyBytes.Length, aSecretKeyBytes.Length));

                oRijndaelManaged = new RijndaelManaged();
                oRijndaelManaged.Mode = CipherMode.CBC;
                oRijndaelManaged.Padding = PaddingMode.PKCS7;
                oRijndaelManaged.KeySize = 128;
                oRijndaelManaged.BlockSize = 128;
                oRijndaelManaged.Key = aKeyBytes;
                oRijndaelManaged.IV = aKeyBytes;

                return oRijndaelManaged;
            }
            catch (Exception ex)
            {
            }

            return null/* TODO Change to default(_) if this is not a reference type */;
        }

        public static string SP_DATtTripleDESDecryptData(string ptEncryptedData, string ptKey)
        {
            TripleDESCryptoServiceProvider oTripleDESSrvPvd;
            MemoryStream oMemStm;
            CryptoStream oDecStm;
            byte[] anEncryptedBytes;
            string tStrDec;

            try
            {
                // Initialize the crypto provider.
                oTripleDESSrvPvd = new TripleDESCryptoServiceProvider();
                oTripleDESSrvPvd.Key = SP_DATaSHA1EncryptData(ptKey, (int)Math.Floor(oTripleDESSrvPvd.KeySize / 8.0));
                oTripleDESSrvPvd.IV = SP_DATaSHA1EncryptData("", (int)Math.Floor(oTripleDESSrvPvd.BlockSize / 8.0));

                // Convert the encrypted text string to a byte array.
                anEncryptedBytes = Convert.FromBase64String(ptEncryptedData);

                // Create the stream.
                oMemStm = new MemoryStream();

                // Create the encoder to write to the stream.
                oDecStm = new CryptoStream(oMemStm, oTripleDESSrvPvd.CreateDecryptor(), CryptoStreamMode.Write);

                // Use the crypto stream to write the byte array to the stream.
                oDecStm.Write(anEncryptedBytes, 0, anEncryptedBytes.Length);
                oDecStm.FlushFinalBlock();

                // Convert the encrypted stream to a printable string.
                tStrDec = Encoding.Unicode.GetString(oMemStm.ToArray());
                return tStrDec;
            }
            catch (Exception)
            {

            }
            finally
            {
                oTripleDESSrvPvd = null;
                oMemStm = null;
                oDecStm = null;
                anEncryptedBytes = null;
                tStrDec = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptPlaintext"></param>
        /// <param name="pnHashSize"></param>
        /// <returns></returns>
        public static byte[] SP_DATaSHA1EncryptData(string ptPlaintext, int pnHashSize)
        {
            SHA1CryptoServiceProvider oSHA1SrvPvd;
            byte[] anPlaintextBytes, anHash, anTmpHash;

            try
            {
                // Hash the key.
                oSHA1SrvPvd = new SHA1CryptoServiceProvider();
                anPlaintextBytes = Encoding.Unicode.GetBytes(ptPlaintext);
                anHash = oSHA1SrvPvd.ComputeHash(anPlaintextBytes);

                // Truncate or pad the hash.
                anTmpHash = anHash;
                anHash = new byte[pnHashSize];
                Array.Copy(anTmpHash, anHash, Math.Min(anHash.Length, anTmpHash.Length));

                return anHash;
            }
            catch (Exception)
            {

            }
            finally
            {
                oSHA1SrvPvd = null;
                anPlaintextBytes = null;
                anHash = null;
                anTmpHash = null;

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //GC.Collect();
            }

            return null;
        }
    }
}