using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface IGoogle2FaBlacklistService
    {
        Task ClientSucceededAsync(string clientId);
        Task<bool> IsClientBlockedAsync(string clientId);
        Task ClientFailedAsync(string clientId);
        Task ResetAsyncAsync(string clientId);
    }
}
