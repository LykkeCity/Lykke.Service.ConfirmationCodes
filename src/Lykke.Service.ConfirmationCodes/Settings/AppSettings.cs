using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Settings;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public ConfirmationCodesServiceSettings ConfirmationCodesService { get; set; }
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; }
        public SmsNotifications SmsNotifications { get; set; }
        public SagasRabbitMqSettings SagasRabbitMq { set; get; }
    }
}
