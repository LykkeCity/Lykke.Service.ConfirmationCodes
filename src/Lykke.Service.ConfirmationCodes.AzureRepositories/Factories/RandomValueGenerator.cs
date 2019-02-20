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

        public string GetCode(int codeLength, bool isReal)
        {
            string defaultCode = new string('0', codeLength);

            if (!isReal) 
                return defaultCode;
            
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[4];

                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);

                return new Random(seed).Next((int)Math.Pow(10, codeLength) - 1).ToString(defaultCode);
            }
        }
    }
}
