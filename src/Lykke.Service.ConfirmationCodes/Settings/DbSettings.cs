using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string ClientPersonalInfoConnString { get; set; }
    }
}
