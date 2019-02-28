using Autofac;

namespace Lykke.Service.ConfirmationCodes.Services
{
    public class AutofacServicesModule : Module
    {
        private readonly IDeploymentSettings _deploymentSettings;
        private readonly ISupportToolsSettings _supportToolsSettings;
        private readonly int _google2FaMaxTries;

        public AutofacServicesModule(
            IDeploymentSettings deploymentSettings,
            ISupportToolsSettings supportToolsSettings,
            int google2FaMaxTries)
        {
            _supportToolsSettings = supportToolsSettings;
            _deploymentSettings = deploymentSettings;
            _google2FaMaxTries = google2FaMaxTries;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_deploymentSettings);
            builder.RegisterInstance(_supportToolsSettings);

            builder.RegisterType<ConfirmationCodesService>().AsImplementedInterfaces();
            builder.RegisterType<EmailConfirmationService>().AsImplementedInterfaces();
            builder.RegisterType<Google2FaService>().AsImplementedInterfaces();
            builder.RegisterType<Google2FaBlacklistService>()
                .AsImplementedInterfaces()
                .WithParameter("maxTries", _google2FaMaxTries);
            
            builder.RegisterType<CallTimeLimitsService>()
                .WithParameter(TypedParameter.From(_supportToolsSettings.RepeatCallInverval))
                .WithParameter(TypedParameter.From(_supportToolsSettings.CallsLimit))
                .AsImplementedInterfaces();
        }
    }
}
