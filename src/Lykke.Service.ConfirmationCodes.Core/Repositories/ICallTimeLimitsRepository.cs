using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface ICallTimeLimitsRepository
    {
        Task InsertRecordAsync(string method, string clientId);
        Task<IReadOnlyCollection<DateTime>> GetCallHistoryAsync(string method, string clientId, TimeSpan period);
        Task<int> GetCallsCount(string method, string clientId);
        Task ClearCallsHistory(string method, string clietnId);
    }
}
