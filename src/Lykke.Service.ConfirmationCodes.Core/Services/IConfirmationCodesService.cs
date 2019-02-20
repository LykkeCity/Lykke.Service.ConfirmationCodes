using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface IConfirmationCodesService
    {
        Task<SmsRequestResult> RequestSmsCode(SmsCodeRequest request);
        Task<string> RequestSmsCode(string partnerId, string phoneNumber, bool isPriority = false);
        Task<bool> CheckAsync(string partnerId, string mobilePhone, string code);
        Task<SmsCheckResult> CheckSmsAsync(string clientId, string mobilePhone, string code, string operation);
        Task<ISmsVerificationPriorityCode> GetPriorityCode(string partnerId, string mobilePhone);
        Task DeleteCodes(string partnerId, string phoneNum);
    }
}
