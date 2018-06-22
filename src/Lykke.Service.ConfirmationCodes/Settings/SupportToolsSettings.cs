using Lykke.Service.ConfirmationCodes.Services;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    public class SupportToolsSettings : ISupportToolsSettings
    {
        public int PriorityCodeExpirationInterval { get; set; }
    }
}
