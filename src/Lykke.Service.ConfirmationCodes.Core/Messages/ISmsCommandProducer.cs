using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Messages
{
    public interface ISmsCommandProducer
    {
        Task SendSms<T>(string partnerId, string phoneNumber, T msgData, bool useAlternativeProvider, string reason, string outerRequestId);
    }
}
