using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    [UsedImplicitly]
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string ClientPersonalInfoConnString { get; set; }
        
        [AzureTableCheck]
        public string Google2FaConnString { get; set; }
        
        [AzureTableCheck]
        public string CallLimitsConnString { get; set; }
    }
}
