using JetBrains.Annotations;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ConfirmationCodesServiceSettings
    {
        public DbSettings Db { get; set; }
        public DeploymentSettings DeploymentSettings { get; set; }
        public SupportToolsSettings SupportToolsSettings { get; set; }
        public int Google2FaConfirmationMaxTries { get; set; }
    }
}
