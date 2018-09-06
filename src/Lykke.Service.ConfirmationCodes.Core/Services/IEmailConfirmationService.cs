using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface IEmailConfirmationService
    {
        Task<string> SendConfirmEmail(string email, string partnerId, bool isPriority, int expirationInterval = 0, int codeLength = 6);
        //todo: consider methods combining for Cyp and Non-cyp
        Task<string> SendConfirmCypEmail(string email, string partnerId,bool isPriority, int expirationInterval = 0, int codeLength = 6);
        Task<bool> CheckAsync(string email, string partnerId, string code);
        Task<IEmailVerificationPriorityCode> GetPriorityCode(string email, string partnerId);
        Task DeleteCodes(string email, string partnerId);
    }
}
