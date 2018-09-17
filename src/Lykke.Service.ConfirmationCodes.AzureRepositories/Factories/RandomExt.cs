using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public static class RandomExt
    {
        public static long NextLong(this Random self, long min, long max)
        {
            var buf = new byte[sizeof(ulong)];
            self.NextBytes(buf);
            ulong n = BitConverter.ToUInt64(buf, 0);
            double normalised = n / (ulong.MaxValue + 1.0);
            double range = (double)max - min;
            return (long)(normalised * range) + min;
        }
    }
}
