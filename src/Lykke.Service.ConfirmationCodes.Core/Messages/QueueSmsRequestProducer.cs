using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Messages
{
    public class QueueSmsRequestProducer : ISmsRequestProducer
    {
        private readonly ISmsCommandProducer _smsCommandProducer;

        public QueueSmsRequestProducer(ISmsCommandProducer smsCommandProducer)
        {
            _smsCommandProducer = smsCommandProducer;
        }

        public async Task SendSmsAsync<T>(string partnerId, string phoneNumber, T msgData, bool useAlternativeProvider)
        {
            await _smsCommandProducer.ProduceSendSmsCommand(partnerId, phoneNumber, msgData, useAlternativeProvider);
        }
    }

    public interface ISmsCommandProducer
    {
        Task ProduceSendSmsCommand<T>(string partnerId, string phoneNumber, T msgData, bool useAlternativeProvider);
    }
}
