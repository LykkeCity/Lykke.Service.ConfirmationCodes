using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface IConfirmationCodesService
    {
        Task<string> RequestSmsCode(string partnerId, string clientId, string phoneNumber, bool isPriority = false, int expirationInterval = 0);
        Task<string> RequestSmsCode(string partnerId, string phoneNumber, bool isPriority = false, int expirationInterval = 0);
        Task<bool> CheckAsync(string partnerId, string mobilePhone, string code);
        Task<ISmsVerificationPriorityCode> GetPriorityCode(string partnerId, string mobilePhone);
        Task DeleteCodes(string partnerId, string phoneNum);
    }
}
