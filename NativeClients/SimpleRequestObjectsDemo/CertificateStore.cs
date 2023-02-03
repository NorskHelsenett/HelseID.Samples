using System;
using System.Security.Cryptography.X509Certificates;

namespace HelseId.RequestObjectsDemo
{
    static class CertificateStore
    {
        public static X509Certificate2 GetCertificateByThumbprint(
            string thumbprint,
            StoreName store = StoreName.My,
            StoreLocation location = StoreLocation.LocalMachine)
        {
            if (string.IsNullOrEmpty(thumbprint))
            {
                throw new ArgumentOutOfRangeException(nameof(thumbprint));
            }

            using (var x509Store = new X509Store(store, location))
            {
                x509Store.Open(OpenFlags.ReadOnly);

                var certificatesInStore = x509Store.Certificates;
                var certificates = certificatesInStore.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificates.Count < 1)
                {
                    throw new Exception($"Did not find any Certificates with the thumbprint: {thumbprint}");
                }

                if (certificates.Count > 1)
                {
                    throw new Exception($"Found {certificates.Count} certificates with thumbprint: {thumbprint}");
                }

                return certificates[0];
            }
        }
    }
}