using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Settings;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public ConfirmationCodesServiceSettings ConfirmationCodesServiceSettings { get; set; }
        public ClientAccountServiceClientSettings ClientAccountServiceClient { get; set; }
        public SmsNotifications SmsNotifications { get; set; }
    }
}
