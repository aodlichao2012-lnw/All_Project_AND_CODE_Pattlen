using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdaLinkSKC.Class
{
    class cSecurity
    {
        /// <summary>
        /// Encrypt data with Triple DES.
        /// </summary>
        /// 
        /// <param name="ptPlaintext">Data to encrypt.</param>
        /// <param name="ptKey">Key.</param>
        /// 
        /// <returns>
        /// String data encrypted.
        /// </returns>
        public string C_DATtTripleDESEncryptData(string ptPlaintext, string ptKey)
        {
            TripleDESCryptoServiceProvider oTripleDESSrvPvd;
            MemoryStream oMemStm;
            CryptoStream oEncStm;
            byte[] anPlaintextBytes;
            string tStrEnc;

            try
            {
                // Convert the plaintext string to a byte array.
                oTripleDESSrvPvd = new TripleDESCryptoServiceProvider();
                anPlaintextBytes = Encoding.Unicode.GetBytes(ptPlaintext);

                // Initialize the crypto provider.
                oTripleDESSrvPvd.Key = C_DATaSHA1EncryptData(ptKey, (int)Math.Floor(oTripleDESSrvPvd.KeySize / 8.0));
                oTripleDESSrvPvd.IV = C_DATaSHA1EncryptData("", (int)Math.Floor(oTripleDESSrvPvd.BlockSize / 8.0));

                // Create the stream.
                oMemStm = new MemoryStream();

                // Create the encoder to write to the stream.
                oEncStm = new CryptoStream(oMemStm, oTripleDESSrvPvd.CreateEncryptor(), CryptoStreamMode.Write);

                // Use the crypto stream to write the byte array to the stream.
                oEncStm.Write(anPlaintextBytes, 0, anPlaintextBytes.Length);
                oEncStm.FlushFinalBlock();

                // Convert the encrypted stream to a printable string.
                tStrEnc = Convert.ToBase64String(oMemStm.ToArray());
                return tStrEnc;
            }
            catch (Exception)
            {

            }
            finally
            {
                oTripleDESSrvPvd = null;
                oMemStm = null;
                oEncStm = null;
                anPlaintextBytes = null;
                tStrEnc = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return "";
        }

        /// <summary>
        /// Decrypt data with Triple DES.
        /// </summary>
        /// 
        /// <param name="ptEncryptedData">Data encrypted to decrypt.</param>
        /// <param name="ptKey">Key.</param>
        /// 
        /// <returns>
        /// String data decrypted.
        /// </returns>
        public string C_DATtTripleDESDecryptData(string ptEncryptedData, string ptKey)
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
                oTripleDESSrvPvd.Key = C_DATaSHA1EncryptData(ptKey, (int)Math.Floor(oTripleDESSrvPvd.KeySize / 8.0));
                oTripleDESSrvPvd.IV = C_DATaSHA1EncryptData("", (int)Math.Floor(oTripleDESSrvPvd.BlockSize / 8.0));

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

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return "";
        }

        /// <summary>
        /// Encrypt data with SHA1.
        /// </summary>
        /// 
        /// <param name="ptPlaintext">Data to encrypt.</param>
        /// <param name="pnHashSize">Hash size.</param>
        /// 
        /// <returns>
        /// Byte data encrypted.
        /// </returns>
        public byte[] C_DATaSHA1EncryptData(string ptPlaintext, int pnHashSize)
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

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return null;
        }
    }
}
