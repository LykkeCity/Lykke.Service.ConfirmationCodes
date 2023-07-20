using System.Threading.Tasks;
using AzureStorage.Queue;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Dto;
using Lykke.Service.ConfirmationCodes.Core.Messages;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Messages
{
    public class SmsCommandProducer : ISmsCommandProducer
    {
        private readonly IQueueExt _queueExt;

        public SmsCommandProducer(IQueueExt queueExt)
        {
            _queueExt = queueExt;

            _queueExt.RegisterTypes(
                QueueType.Create("SmsConfirmMessage", typeof(SendSmsData<SmsConfirmationData>))
            );
            _queueExt.RegisterTypes(
                QueueType.Create("SimpleSmsMessage", typeof(SendSmsData<string>))
            );
        }

        public Task SendSms<T>(string partnerId, string phoneNumber, T msgData,
            bool useAlternativeProvider, string reason, string outerRequestId)
        {
            var msg = new SendSmsData<T>
            {
                PartnerId = partnerId,
                MessageData = msgData,
                PhoneNumber = phoneNumber,
                UseAlternativeProvider = useAlternativeProvider,
                Reason = reason,
                OuterRequestId = outerRequestId
            };

            return _queueExt.PutMessageAsync(msg);
        }
    }
}
