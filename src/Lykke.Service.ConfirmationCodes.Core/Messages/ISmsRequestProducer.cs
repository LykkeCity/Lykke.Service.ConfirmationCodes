using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Messages
{
    public interface ISmsRequestProducer
    {
        Task SendSmsAsync<T>(string partnerId, string phoneNumber, T msgData, bool useAlternativeProvider);
    }
}
