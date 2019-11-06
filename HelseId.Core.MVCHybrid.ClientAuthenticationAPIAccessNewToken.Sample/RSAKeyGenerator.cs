using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample
{
    public class RSAKeyGenerator
    {
        private const string KeyName = "HelseId_DCR_Key";
        public const int Size = 4096;
        public const string JwsAlgorithmName = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.RsaSha512;

        /// <summary>
        /// Creates a new RSA key pair, and returns the key as Xml formatted string
        /// If a key allready exists it will be deleted
        /// </summary>
        /// <param name="includePrivateParameters">If true the private parameters will be included in the xml formatted key</param>
        /// <returns></returns>
        public static string CreateNewKey(bool includePrivateParameters)
        {
            CngKey cngKey;

            try
            {
                cngKey = CngKey.Open(KeyName);
                cngKey.Dispose();
                DeleteKey();
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Unable to open CngKey - assuming that the key does not exist");
                Debug.WriteLine($"{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

            try
            {
                var creationParameters = new CngKeyCreationParameters()
                {
                    ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                    Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
                    KeyCreationOptions = CngKeyCreationOptions.OverwriteExistingKey,
                    KeyUsage = CngKeyUsages.Signing,

                    Parameters =
                    {
                        new CngProperty("Length", BitConverter.GetBytes(Size), CngPropertyOptions.None),
                    }
                };

                Debug.WriteLine("Creating new CngKey");
                cngKey = CngKey.Create(CngAlgorithm.Rsa, KeyName, creationParameters);

                using (cngKey)
                using (RSA rsa = new RSACng(cngKey))
                {
                    Debug.WriteLine("Creating new CngKey");
                    return rsa.ToXmlString(includePrivateParameters);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception("An error occurred"); //do not rethrow - hide the underlying error
            }

        }

        public static RSA GetRsa()
        {
            try
            {
                Debug.WriteLine("Trying to open existing CngKey");
                var cngKey = CngKey.Open(KeyName);

                using (cngKey)
                using (RSA rsa = new RSACng(cngKey))
                {
                    return rsa;
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception("An exception occurred while opening a CngKey");
            }
        }

        public static RSAParameters GetRsaParameters()
        {
            try
            {
                Debug.WriteLine("Trying to open existing CngKey");
                var cngKey = CngKey.Open(KeyName);

                using (cngKey)
                using (RSA rsa = new RSACng(cngKey))
                {
                    return rsa.ExportParameters(true);
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception("An exception occurred while opening a CngKey");
            }
        }


        public static string GetPublicKeyAsXml()
        {
            try
            {
                Debug.WriteLine("Trying to open existing CngKey");
                var cngKey = CngKey.Open(KeyName);

                using (cngKey)
                using (RSA rsa = new RSACng(cngKey))
                {
                    return rsa.ToXmlString(false);
                }
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw new Exception("An exception occurred while opening a CngKey");
            }

        }

        public static bool KeyExists()
        {
            try
            {
                var key = CngKey.Open(KeyName);
                key.Dispose();
                return true;
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                return false;
            }
        }

        public static void DeleteKey()
        {
            try
            {
                var key = CngKey.Open(KeyName);
                key.Delete();
                //Delete closes the handle to the key - no need to dispose
            }
            catch (CryptographicException e)
            {
                Debug.WriteLine("Unable to delete CngKey.");
                Debug.WriteLine($"Unable to open CngKey.{Environment.NewLine}{e.Message}{Environment.NewLine}{e.StackTrace}");
                throw;
            }
        }

    }
}
