using System;
using System.Security.Cryptography;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    internal class RandomValueGenerator : IRandomValueGenerator
    {
        public int GetInt(int min, int max)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[4];

                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);

                return new Random(seed).Next(min, max);
            }
        }
        public long GetCode(int length)
        {
            if (length < 4) length = 4;
            if (length > 16) length = 16;
            var codeLength = 0;
            var codemask = string.Empty;
            while (codeLength < length)
            {
                codemask += "9";
                codeLength++;
            }
            var max = Convert.ToInt64(codemask);
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[sizeof(ulong)];
                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);
                return new Random(seed).NextLong(1, max);
            }
        }
    }
}
