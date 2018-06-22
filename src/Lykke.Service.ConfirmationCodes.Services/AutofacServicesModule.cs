using Autofac;

namespace Lykke.Service.ConfirmationCodes.Services
{
    public class AutofacServicesModule : Module
    {
        private readonly IDeploymentSettings _deploymentSettings;
        private readonly ISupportToolsSettings _supportToolsSettings;

        public AutofacServicesModule(IDeploymentSettings deploymentSettings, ISupportToolsSettings supportToolsSettings)
        {
            _supportToolsSettings = supportToolsSettings;
            _deploymentSettings = deploymentSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_deploymentSettings);
            builder.RegisterInstance(_supportToolsSettings);

            builder.RegisterType<ConfirmationCodesService>().AsImplementedInterfaces();
            builder.RegisterType<EmailConfirmationService>().AsImplementedInterfaces();
        }
    }
}
