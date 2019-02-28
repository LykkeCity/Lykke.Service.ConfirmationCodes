using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface ICallTimeLimitsRepository
    {
        Task InsertRecordAsync(string method, string clientId);
        Task<IReadOnlyCollection<DateTime>> GetCallHistoryAsync(string method, string clientId);
        Task<int> GetCallsCountAsync(string method, string clientId);
        Task ClearCallsHistoryAsync(string method, string clietnId);
    }
}
