using Autofac;
using AzureStorage.Queue;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;
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
        private const string TableNameEmailVerificationMock = "MockMails";
        private const string TableNameSmsVerificationMock = "MockSms";
        private const string TableNameApiCalls = "ApiSuccessfulCalls";
        public const string TableEmailAttachmentsMock = "EmailAttachmentsMock";

        private readonly IReloadingManager<SmsNotifications> _smsNotificationsSettings;
        private readonly ILog _log;
        private readonly IReloadingManager<string> _personalDataConnString;
        private readonly IReloadingManager<string> _logsConnString;


        public AutofacRepositoriesModule(
            IReloadingManager<SmsNotifications> smsNotificationsSettings,
            IReloadingManager<string> personalDataConnString, 
            IReloadingManager<string> logsConnString, 
            ILog log)
        {
            _logsConnString = logsConnString;
            _personalDataConnString = personalDataConnString;
            _log = log;
            _smsNotificationsSettings = smsNotificationsSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ISmsCommandProducer>(y => 
                new SmsCommandProducer(AzureQueueExt.Create(
                    _smsNotificationsSettings.ConnectionString(x => x.AzureQueue.ConnectionString),
                    _smsNotificationsSettings.CurrentValue.AzureQueue.QueueName)));

            builder.Register<ISmsVerificationCodeRepository>(y => new SmsVerificationCodeRepository(
                AzureTableStorage<SmsVerificationCodeEntity>.Create(_personalDataConnString, TableNameSmsVerificationCodes, _log),
                AzureTableStorage<SmsVerificationPriorityCodeEntity>.Create(_personalDataConnString, TableNameSmsVerificationPriorityCodes, _log))
            );

            builder.Register<IEmailVerificationCodeRepository>(y => new EmailVerificationCodeRepository(
                AzureTableStorage<EmailVerificationCodeEntity>.Create(_personalDataConnString, TableNameEmailVerificationCodes, _log),
                AzureTableStorage<EmailVerificationPriorityCodeEntity>.Create(_personalDataConnString, TableNameEmailVerificationPriorityCodes, _log))
            );

            builder.Register<ICallTimeLimitsRepository>(y =>
                new CallTimeLimitsRepository(
                    AzureTableStorage<ApiCallHistoryRecord>.Create(_logsConnString, TableNameApiCalls, _log)));
        }
    }
}
