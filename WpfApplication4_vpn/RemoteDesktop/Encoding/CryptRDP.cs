using System;
using System.Security.Cryptography;

namespace RemoteDesktop.Encoding
{
    public class CryptRDP
    {

        public static string Protect(string data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                //  only by the same current user.
                byte[] secret = System.Text.Encoding.Unicode.GetBytes(data);
                var pr = ProtectedData.Protect(secret, new byte[0], DataProtectionScope.CurrentUser);
                return GetByteChain(pr);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public static string Unprotect(string data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                byte[] secret = GetByteChain(data);
                byte[] up = ProtectedData.Unprotect(secret, new byte[0], DataProtectionScope.CurrentUser);
                return System.Text.Encoding.Unicode.GetString(up);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private static string GetByteChain(byte[] bytes)
        {
            if (bytes == null) return String.Empty;
            string val = null;
            foreach (var item in bytes)
            {
                val += item.ToString("X2");
            }
            return val;
        }

        private static byte[] GetByteChain(string bytes)
        {
            if (bytes == null) return new byte[0];
            byte[] val = new byte[bytes.Length / 2];
            int j = 0;
            for (int i = 0; i < val.Length; i++)
            {
                val[i] = byte.Parse(bytes[j].ToString() + bytes[j+1].ToString(), System.Globalization.NumberStyles.HexNumber);
                j += 2;
            }
            return val;
        }
    }
}
