using System.Security.Cryptography;
using Common;

namespace Lykke.Service.ConfirmationCodes.Core.Heplers
{
    public static class StringExtensions
    {
        public static string GetSha256Hash(this string src)
        {
            if (string.IsNullOrEmpty(src))
                return string.Empty;

            var hash = SHA256.Create().ComputeHash(src.ToUtf8Bytes());
            return hash.ToHexString().ToLower();
        }
    }
}
