using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Security
{
    public class CryptographyProvider
    {
        readonly byte[] key;
        readonly byte[] iv;

        public CryptographyProvider(X509Certificate2 certificate, byte[] keyExchange)
            : this(certificate.PrivateKey, keyExchange)
        {
        }

        public CryptographyProvider(AsymmetricAlgorithm asymmetricAlgorithm, byte[] keyExchange)
        {
            RSAPKCS1KeyExchangeDeformatter keyExchangeDeformatter = new RSAPKCS1KeyExchangeDeformatter(asymmetricAlgorithm);
            byte[] rgbData = keyExchangeDeformatter.DecryptKeyExchange(keyExchange);
            key = rgbData.Take(32).ToArray();
            iv = rgbData.Skip(32).Take(16).ToArray();
        }

        public MemoryStream Encrypt(byte[] input)
        {
            MemoryStream outputStream = new MemoryStream();
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                Encrypt(inputStream, outputStream);
                return outputStream;
            }
        }

        public void Encrypt(Stream inputStream, Stream outputStream)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor(key, iv);
                Transform(inputStream, outputStream, cryptoTransform);
            }
        }

        public MemoryStream Decrypt(byte[] input)
        {
            MemoryStream outputStream = new MemoryStream();
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                Decrypt(inputStream, outputStream);
                return outputStream;
            }
        }

        public void Decrypt(Stream inputStream, Stream outputStream)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                ICryptoTransform cryptoTransform = aesManaged.CreateDecryptor(key, iv);
                Transform(inputStream, outputStream, cryptoTransform);
            }
        }

        void Transform(Stream inputStream, Stream outputStream, ICryptoTransform cryptoTransform)
        {
            using (CryptoStream cryptoStream = new CryptoStream(outputStream, cryptoTransform, CryptoStreamMode.Write))
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead <= 0)
                    {
                        break;
                    }
                    cryptoStream.Write(buffer, 0, bytesRead);
                }
            }
        }

        public static string CreateKeyExchange(string thumbprint)
        {
            X509Certificate2 certificate = GetCertificateByThumbprint(thumbprint, false, StoreLocation.LocalMachine);
            byte[] keyExchange = CreateKeyExchange(certificate);
            return Convert.ToBase64String(keyExchange);
        }

        public static byte[] CreateKeyExchange(X509Certificate2 certificate)
        {
            PublicKey publicKey = certificate.PublicKey;
            RSAPKCS1KeyExchangeFormatter keyExchangeFormatter = new RSAPKCS1KeyExchangeFormatter(publicKey.Key);
            using (AesManaged aesManaged = new AesManaged())
            {
                //Aes default implementation has a 32 byte key and a 16 byte initialization vector.
                byte[] rgbData = aesManaged.Key.Concat(aesManaged.IV).ToArray();
                return keyExchangeFormatter.CreateKeyExchange(rgbData, aesManaged.GetType());
            }
        }

        public static X509Certificate2 GetCertificateByThumbprint(string thumbprint, bool validOnly, StoreLocation storeLocation)
        {
            return FindCertificates(X509FindType.FindByThumbprint, thumbprint, validOnly, storeLocation).SingleOrDefault();
        }

        public static IEnumerable<X509Certificate2> FindCertificates(X509FindType findType, object findValue, bool validOnly, StoreLocation storeLocation)
        {
            X509Store store = new X509Store(StoreName.My, storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificateCollection = store.Certificates.Find(findType, findValue, validOnly);
                foreach (X509Certificate2 certificate in certificateCollection)
                {
                    yield return certificate;
                }
            }
            finally
            {
                store.Close();
            }
        }
    }
}
