using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MQAdaLink.Class
{
    public class cEncryptDecrypt
    {
        private byte[] aC_EncIV;
        private byte[] aC_EncKey;

        #region Constructor

        public cEncryptDecrypt(string ptMode)   // 1: Normal, 2: Password
        {
            try
            {
                if (string.Equals(ptMode, "1"))   // Key มาตรฐาน
                {
                    aC_EncKey = Encoding.UTF8.GetBytes("BJHhj?AW=c7-4#vF");
                    aC_EncIV = Encoding.UTF8.GetBytes("bh_B4Hu?L4@CV6pb");
                }
                else    // Password
                {
                    aC_EncKey = Encoding.UTF8.GetBytes("5YpPTypXtwMML$u@");
                    aC_EncIV = Encoding.UTF8.GetBytes("zNhQ$D%arP6U8waL");
                }
            }
            catch { }
        }

        #endregion

        #region Function / Method

        /// <summary>
        /// เข้ารหัส
        /// </summary>
        /// <param name="ptValue">ค่าที่ต้องการเข้ารหัส</param>
        /// <param name="paKey">Encoding.UTF8.GetBytes(tC_EncKey)</param>
        /// <param name="paIV">Encoding.UTF8.GetBytes(tC_EncIV)</param>
        /// <param name="pnKeysize"></param>
        /// <param name="pnBlocksize"></param>
        /// <param name="poCipher"></param>
        /// <param name="poPadding"></param>
        /// <returns></returns>
        public string C_CALtEncrypt(string ptValue, int pnKeysize = 128, int pnBlocksize = 128,
                                           CipherMode poCipher = CipherMode.CBC, PaddingMode poPadding = PaddingMode.PKCS7)
        {
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.BlockSize = pnBlocksize;
            oAes.KeySize = pnKeysize;
            oAes.Mode = poCipher;
            oAes.Padding = poPadding;

            byte[] oSrc = Encoding.UTF8.GetBytes(ptValue);

            using (ICryptoTransform oEncrypt = oAes.CreateEncryptor(aC_EncKey, aC_EncIV))
            {
                byte[] oDest = oEncrypt.TransformFinalBlock(oSrc, 0, oSrc.Length);
                oEncrypt.Dispose();

                return Convert.ToBase64String(oDest);
            }
        }

        /// <summary>
        /// ถอดรหัส
        /// </summary>
        /// <param name="ptValue">ค่าที่ต้องการถอดรหัส</param>
        /// <param name="paKey">Encoding.UTF8.GetBytes(tC_EncKey)</param>
        /// <param name="paIV">Encoding.UTF8.GetBytes(tC_EncIV)</param>
        /// <param name="pnKeysize"></param>
        /// <param name="pnBlocksize"></param>
        /// <param name="poCipher"></param>
        /// <param name="poPadding"></param>
        /// <returns></returns>
        public string C_CALtDecrypt(string ptValue, int pnKeysize = 128, int pnBlocksize = 128,
                                    CipherMode poCipher = CipherMode.CBC, PaddingMode poPadding = PaddingMode.PKCS7)
        {
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.BlockSize = pnBlocksize;
            oAes.KeySize = pnKeysize;
            oAes.Mode = poCipher;
            oAes.Padding = poPadding;

            byte[] aSrc = Convert.FromBase64String(ptValue);

            using (ICryptoTransform oDecrypt = oAes.CreateDecryptor(aC_EncKey, aC_EncIV))
            {
                byte[] aDest = oDecrypt.TransformFinalBlock(aSrc, 0, aSrc.Length);
                oDecrypt.Dispose();

                return Encoding.UTF8.GetString(aDest); //Padding is invalid and cannot be removed. 
            }
        }

        public string C_CALtDecrypt(string ptValue, string ptKey, int pnKeysize = 128, int pnBlocksize = 128,
                                    CipherMode poCipher = CipherMode.CBC, PaddingMode poPadding = PaddingMode.PKCS7)
        {
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.BlockSize = pnBlocksize;
            oAes.KeySize = pnKeysize;
            oAes.Mode = poCipher;
            oAes.Padding = poPadding;

            byte[] aSrc = Convert.FromBase64String(ptValue);
            byte[] aKey = new byte[16];
            byte[] aKeyTmp = Encoding.UTF8.GetBytes(ptKey);

            Array.Copy(aKeyTmp, 0, aKey, 0, Math.Min(aKeyTmp.Length, aKey.Length)); // ทำการ Copy ข้อมูล Key ไปใส่ให้ครบ 16 byte

            using (ICryptoTransform oDecrypt = oAes.CreateDecryptor(aKey, aKey)) // Key กับ IV เป็นตัวเดียวกัน
            {
                byte[] aDest = oDecrypt.TransformFinalBlock(aSrc, 0, aSrc.Length);
                oDecrypt.Dispose();

                return Encoding.UTF8.GetString(aDest);
            }
        }

        public string C_CALtEncrypt(string ptValue, string ptKey, int pnKeysize = 128, int pnBlocksize = 128,
                                    CipherMode poCipher = CipherMode.CBC, PaddingMode poPadding = PaddingMode.PKCS7)
        {
            AesCryptoServiceProvider oAes = new AesCryptoServiceProvider();
            oAes.BlockSize = pnBlocksize;
            oAes.KeySize = pnKeysize;
            oAes.Mode = poCipher;
            oAes.Padding = poPadding;

            byte[] aSrc = Encoding.UTF8.GetBytes(ptValue);
            byte[] aKey = new byte[16];
            byte[] aKeyTmp = Encoding.UTF8.GetBytes(ptKey);

            Array.Copy(aKeyTmp, 0, aKey, 0, Math.Min(aKeyTmp.Length, aKey.Length)); // ทำการ Copy ข้อมูล Key ไปใส่ให้ครบ 16 byte

            using (ICryptoTransform oEncrypt = oAes.CreateEncryptor(aKey, aKey))
            {
                byte[] oDest = oEncrypt.TransformFinalBlock(aSrc, 0, aSrc.Length);
                oEncrypt.Dispose();

                return Convert.ToBase64String(oDest);
            }
        }
        #endregion
    }
}
