using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

using UnityEngine;

    /// <summary>
    /// Class provides static method to serialize and encrypt data to XML
    /// 
    /// Works on Android, iOS, Standalone.
    /// 
    /// Does not encrypt data on WP8
    /// </summary>
    public static class EncryptedXmlSerializer
    {
        private static readonly string PrivateKey = "PkkrNm4nREd5VSsyfDxhb1FPV3RDVTApKUdv";

        #region API

        /// <summary>
        /// Reads and decrypts file at specified path
        /// </summary>
        /// <param name="path">Patht to file</param>
        /// <typeparam name="T">Type of the serialized object</typeparam>
        /// <returns>Decrypted deserialized object or null if file does not exist</returns>

    #endregion

    #region encrypt_decrypt
    public static string EncryptData(string toEncrypt)
        {
#if UNITY_WP8
            return toEncrypt;
#else
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = CreateRijndaelManaged();
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
#endif
        }

        public static string DecryptData(string toDecrypt)
        {
#if UNITY_WP8
            return toDecrypt;
#else
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = CreateRijndaelManaged();
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
#endif
        }

#if !UNITY_WP8
        private static RijndaelManaged CreateRijndaelManaged()
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(PrivateKey);
            var result = new RijndaelManaged();

            var newKeysArray = new byte[16];
            Array.Copy(keyArray, 0, newKeysArray, 0, 16);

            result.Key = newKeysArray;
            result.Mode = CipherMode.ECB;
            result.Padding = PaddingMode.PKCS7;
            return result;
        }
#endif
        #endregion
    }