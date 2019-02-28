using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Services
{
    public interface ICallTimeLimitsService
    {
        Task<CallLimitsResult> ProcessCallLimitsAsync(string clientId, string operation = null, bool checkRepeat = true, bool increaseCount = true);
        Task<int> GetCallsCountAsync(string method, string clientId);
        Task ClearCallsHistoryAsync(string method, string clietnId);
    }
}
