using JetBrains.Annotations;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    [UsedImplicitly]
    public class CacheSettings
    {
        public string RedisConfiguration { get; set; }
    }
}
