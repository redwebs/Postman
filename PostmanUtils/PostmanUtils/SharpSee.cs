using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace PostmanUtils
{
    public static class SharpSee
    {
        #region File

        public static Tuple<bool, string> ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Tuple<bool, string>(false, $"File not found: {filePath}");
            }
            var sr = File.OpenText(filePath);
            var fileText = sr.ReadToEnd();
            sr.Close();
            
            return new Tuple<bool, string>(true, fileText);
        }

        public static Tuple<bool, string> StringToFile(StringBuilder saveData, string filePath)
        {
            return StringToFile(saveData.ToString(), filePath);
        }

        public static Tuple<bool, string> StringToFile(string saveData, string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new Tuple<bool, string>(false, "Path is null or empty.");
            }

            try
            {
                // create a writer and open the file
                TextWriter tw = new StreamWriter(filePath, false);

                // write a line of text to the file
                tw.WriteLine(saveData);

                // close the stream
                tw.Close();
                return new Tuple<bool, string>(true, "Text was saved to file.");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, $"Could not save file, Exception: {ex.Message}");
            }
        }

        #endregion


        #region Web


        public static string DateTimeStr(string prefix, string extension)
        {
            const string format = "yyMMdd_HHmmss";
            var dt = DateTime.Now;
            return (prefix + "_" + dt.ToString(format) + "." + extension);
        }

        public static string DateStr(string prefix, string extension)
        {
            const string format = "yyyy-MM-dd";
            var dt = DateTime.Now;
            return (prefix + dt.ToString(format) + "." + extension);
        }


        public static string GetInnerExceptionMsgs(Exception ex)
        {
            var statusMsg = string.Empty;

            if (!ex.Message.Contains("See the inner exception for details"))
            {
                statusMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    statusMsg += ex.InnerException.Message;
                }
            }
            else
            {
                if (ex.InnerException != null)
                {
                    if (!ex.InnerException.Message.Contains("See the inner exception for details"))
                    {
                        statusMsg = ex.InnerException.Message;
                    }
                    else
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            statusMsg += ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            statusMsg += ex.InnerException.Message;
                        }

                    }
                }
            }
            return statusMsg;
        }

        /// <summary>
        /// Encrypt a string using Triple DES encryption and additional SHA1 hash. 
        /// </summary>
        /// <param name="clearText">UTF8 Clear text to be encrypted</param>
        /// <param name="key">Encryption key used by both methods</param>
        /// <returns>Encrypted base 64 string</returns>
        /// 
        public static string EncryptTextDual(string clearText, string key)
        {
            byte[] key24Len = new byte[24];

            var toEncryptArray = Encoding.UTF8.GetBytes(clearText);

            var sha1Provider = new SHA512CryptoServiceProvider();
            var keyArray = sha1Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            sha1Provider.Clear();

            for (int idx = 0; idx < 24; idx++)
            {
                key24Len[idx] = keyArray[idx];
            }

            var tripleDesProvider = new TripleDESCryptoServiceProvider { };
            tripleDesProvider.Key = key24Len;
            tripleDesProvider.Mode = CipherMode.ECB;
            tripleDesProvider.Padding = PaddingMode.PKCS7;

            var iCryptoTransform = tripleDesProvider.CreateEncryptor();
            var resultArray = iCryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tripleDesProvider.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypt a Base 64 string using Triple DES encryption method and SHA1 hash. 
        /// </summary>
        /// <param name="cipherText">Encrypted text</param>
        /// <param name="key">Encryption key used by both methods</param>
        /// <returns>Clear text UTF8 string</returns>
        /// 
        public static Tuple<string, bool> DecryptTextDual(string cipherText, string key)
        {
            try
            {
                byte[] key24Len = new byte[24];

                var toDecryptArray = Convert.FromBase64String(cipherText);

                var sha1Provider = new SHA512CryptoServiceProvider();
                var keyArray = sha1Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                sha1Provider.Clear();

                for (int idx = 0; idx < 24; idx++)
                {
                    key24Len[idx] = keyArray[idx];
                }

                var tripleDesProvider = new TripleDESCryptoServiceProvider { };
                tripleDesProvider.Key = key24Len;
                tripleDesProvider.Mode = CipherMode.ECB;
                tripleDesProvider.Padding = PaddingMode.PKCS7;

                var iCryptoTransform = tripleDesProvider.CreateDecryptor();
                var resultArray = iCryptoTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                tripleDesProvider.Clear();
                return new Tuple<string, bool>(Encoding.UTF8.GetString(resultArray), true);
            }
            catch (Exception ex)
            {
                return new Tuple<string, bool>(ex.Message, false);
            }
        }

        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length];
            var charArray = str.ToCharArray();
            for (int idx = 0; idx < str.Length; idx++)
            {
                bytes[idx] = (byte)charArray[idx];
            }
            return bytes;
        }
    }
    #endregion
}
