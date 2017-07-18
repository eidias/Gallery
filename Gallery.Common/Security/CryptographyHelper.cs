using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Common.Security
{
    public class CryptographyHelper
    {
        public static string ProtectString(string text, byte[] entropy = null)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            byte[] cipherBytes = ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(cipherBytes);
        }

        public static string UnProtectString(string text, byte[] entropy = null)
        {
            byte[] cipherBytes = Convert.FromBase64String(text);
            byte[] bytes = ProtectedData.Unprotect(cipherBytes, entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
