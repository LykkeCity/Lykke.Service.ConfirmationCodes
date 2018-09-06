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
        public string GetCode(int length)
        {
            if (length < 4) length = 4;
            if (length > 16) length = 16;
            var codeLength = 0;
            var code = string.Empty;
            while (codeLength < length)
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    var buffer = new byte[4];

                    rng.GetBytes(buffer);
                    var seed = BitConverter.ToInt32(buffer, 0);
                    code += new Random(seed).Next(1, 9).ToString();
                }
                codeLength++;
            }
            return code;
        }
    }
}
