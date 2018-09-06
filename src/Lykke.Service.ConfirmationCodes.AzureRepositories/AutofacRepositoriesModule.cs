using System;
using Autofac;
using AzureStorage;
using AzureStorage.Queue;
using AzureStorage.Tables;
using AzureStorage.Tables.Decorators;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Factories;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Messages;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Repositories;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Settings;
using Lykke.Service.ConfirmationCodes.Core.Messages;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.SettingsReader;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories
{
    public class AutofacRepositoriesModule : Module
    {
        private const string TableNameEmailVerificationCodes = "EmailVerificationCodes";
        private const string TableNameSmsVerificationCodes = "SmsVerificationCodes";
        private const string TableNameSmsVerificationPriorityCodes = "SmsVerificationPriorityCodes";
        private const string TableNameEmailVerificationPriorityCodes = "EmailVerificationPriorityCodes";
        private const string TableNameGoogle2Fa = "Google2Fa";
        private const string TableNameApiCalls = "ApiSuccessfulCalls";

        private readonly IReloadingManager<SmsNotifications> _smsNotificationsSettings;
        
        private readonly IReloadingManager<string> _personalDataConnString;
        private readonly IReloadingManager<string> _google2faConnString;
        private readonly IReloadingManager<string> _logsConnString;

        public AutofacRepositoriesModule(
            IReloadingManager<SmsNotifications> smsNotificationsSettings,
            IReloadingManager<string> personalDataConnString, 
            IReloadingManager<string> google2faConnString, 
            IReloadingManager<string> logsConnString)
        {
            _logsConnString = logsConnString;
            
            _personalDataConnString = personalDataConnString;
            _google2faConnString = google2faConnString;
            _smsNotificationsSettings = smsNotificationsSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ISmsCommandProducer>(y =>
                new SmsCommandProducer(AzureQueueExt.Create(
                    _smsNotificationsSettings.ConnectionString(x => x.AzureQueue.ConnectionString),
                    _smsNotificationsSettings.CurrentValue.AzureQueue.QueueName)));

            builder.RegisterType<VerificationCodesFactory>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RandomValueGenerator>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DateTimeProvider>().AsImplementedInterfaces().SingleInstance();

            builder.Register(x =>
                AzureTableStorage<SmsVerificationCodeEntity>.Create(_personalDataConnString,
                    TableNameSmsVerificationCodes, x.Resolve<ILogFactory>()
                )).AsImplementedInterfaces().SingleInstance();
            builder.Register(x =>
                AzureTableStorage<SmsVerificationPriorityCodeEntity>.Create(_personalDataConnString,
                    TableNameSmsVerificationPriorityCodes, x.Resolve<ILogFactory>())).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SmsVerificationCodeRepository>().AsImplementedInterfaces().SingleInstance();

            builder.Register(x =>
                AzureTableStorage<EmailVerificationCodeEntity>.Create(_personalDataConnString,
                    TableNameEmailVerificationCodes, x.Resolve<ILogFactory>())).AsImplementedInterfaces().SingleInstance();
            builder.Register(x =>
                AzureTableStorage<EmailVerificationPriorityCodeEntity>.Create(_personalDataConnString,
                    TableNameEmailVerificationPriorityCodes, x.Resolve<ILogFactory>())).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EmailVerificationCodeRepository>().AsImplementedInterfaces().SingleInstance();

            builder.Register<ICallTimeLimitsRepository>(y =>
                new CallTimeLimitsRepository(
                    AzureTableStorage<ApiCallHistoryRecord>.Create(_logsConnString, TableNameApiCalls, y.Resolve<ILogFactory>())));
            
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EncryptionKey")))
            {
                builder
                    .Register(x =>
                    {
                        var manager = new EncryptedStorageManager(AzureTableStorage<EncryptionInitModel>.Create(
                            _google2faConnString,
                            TableNameGoogle2Fa,
                            x.Resolve<ILogFactory>()));
                        
                        if (!manager.TrySetKey(Environment.GetEnvironmentVariable("EncryptionKey"), out string error))
                        {
                            var exception = new InvalidOperationException("EncryptionKey is not set");
                            x.Resolve<ILogFactory>().CreateLog(this).WriteFatalError("SetEncryptionKey", error, exception);
                            throw exception;
                        }
    
                        return manager;
                    })
                    .As<EncryptedStorageManager>()
                    .AutoActivate();
            }
            else
            {
                if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                    throw new Exception("Need to set EncryptionKey in Production environment");
                    
                builder
                    .Register(x => new EncryptedStorageManager(AzureTableStorage<EncryptionInitModel>.Create(
                    _google2faConnString,
                    TableNameGoogle2Fa,
                    x.Resolve<ILogFactory>())))
                    .As<EncryptedStorageManager>()
                    .AutoActivate();
            }
            
            builder
                .Register(
                    x => EncryptedTableStorageDecorator<Google2FaSecretEntity>.Create(
                        AzureTableStorage<Google2FaSecretEntity>.Create(
                            _google2faConnString,
                            TableNameGoogle2Fa,
                            x.Resolve<ILogFactory>()),
                        x.Resolve<EncryptedStorageManager>().Serializer))
                .As<INoSQLTableStorage<Google2FaSecretEntity>>()
                .SingleInstance();
            
            builder
                .RegisterType<Google2FaRepository>()
                .As<IGoogle2FaRepository>()
                .SingleInstance();
        }
    }
}
