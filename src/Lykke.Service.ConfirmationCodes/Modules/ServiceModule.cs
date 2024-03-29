﻿using Autofac;
using Lykke.Messages.Email;
using Lykke.Sdk;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ConfirmationCodes.AzureRepositories;
using Lykke.Service.ConfirmationCodes.Core.Messages;
using Lykke.Service.ConfirmationCodes.Services;
using Lykke.Service.ConfirmationCodes.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.ConfirmationCodes.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;
        
        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterModule(new AutofacRepositoriesModule(
                _appSettings.Nested(x => x.SmsNotifications),
                _appSettings.Nested(x => x.ConfirmationCodesService.Db.ClientPersonalInfoConnString),
                _appSettings.Nested(x => x.ConfirmationCodesService.Db.Google2FaConnString),
                _appSettings.Nested(x => x.ConfirmationCodesService.Db.LogsConnString)
            ));

            builder.RegisterModule(new AutofacServicesModule(
                _appSettings.CurrentValue.ConfirmationCodesService.DeploymentSettings,
                _appSettings.CurrentValue.ConfirmationCodesService.SupportToolsSettings,
                _appSettings.CurrentValue.ConfirmationCodesService.Google2FaConfirmationMaxTries));

            builder.RegisterEmailSenderViaAzureQueueMessageProducer(_appSettings.Nested(x => x.ConfirmationCodesService.Db.ClientPersonalInfoConnString));
            builder.RegisterLykkeServiceClient(_appSettings.CurrentValue.ClientAccountServiceClient.ServiceUrl);

            builder.RegisterInstance(_appSettings.CurrentValue.ConfirmationCodesService).AsSelf();
        }
    }
}
