using Lykke.Service.ConfirmationCodes.Services;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    public class DeploymentSettings : IDeploymentSettings
    {
        public bool IsProduction { get; set; }
    }
}
