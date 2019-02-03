using System;

namespace RemoteDesktop.Encoding
{
    /// <summary>
    /// 
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// To Base64 String
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Convertet <see cref="string"/> to Base64.</returns>
        public static string EncodeBase64(string text)
        {
            if (text == null) return null;

            byte[] textAsBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(textAsBytes);
        }

        /// <summary>
        /// From Base64 String
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        /// <exception cref="System.FormatException"></exception>
        public static string DecodeBase64(string encodedText)
        {
            if (encodedText == null) return null;

            byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
            return System.Text.Encoding.UTF8.GetString(textAsBytes);
        }
    }
}
