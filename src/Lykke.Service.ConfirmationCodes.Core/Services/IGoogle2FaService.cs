using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface IGoogle2FaService
    {
        Task<bool> ClientHasEnabledAsync(string clientId);
        Task<bool> ClientHasPendingAsync(string clientId);
        Task ActivateAsync(string clientId);
        Task<string> CreateAsync(string clientId);
        Task<bool> CheckCodeAsync(string clientId, string code, bool isActivationCheck = false);
        Task RemoveAsync(string clientId);
    }
}
