using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Settings
{
    public class AzureQueueSettings
    {
        [AzureQueueCheck]
        public string ConnectionString { get; set; }

        public string QueueName { get; set; }
    }
}
