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
    }
}
