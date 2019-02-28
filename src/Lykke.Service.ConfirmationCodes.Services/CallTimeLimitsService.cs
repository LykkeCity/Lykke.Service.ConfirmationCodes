using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Contract.Models;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    public class CallTimeLimitsService : ICallTimeLimitsService
    {
        private readonly ICallTimeLimitsRepository _callTimeLimitsRepository;
        private const string DefaultOperationName = "CallTimeLimitsService.ProcessCall";
        private readonly TimeSpan _repeatEnabledTimeSpan;
        private readonly int _callsLimit;

        public CallTimeLimitsService(
            ICallTimeLimitsRepository callTimeLimitsRepository,
            TimeSpan repeatEnabledTimeSpan,
            int callsLimit
            )
        {
            _callTimeLimitsRepository = callTimeLimitsRepository;
            _repeatEnabledTimeSpan = repeatEnabledTimeSpan;
            _callsLimit = callsLimit;
        }
        
        public async Task<CallLimitsResult> ProcessCallLimitsAsync(string clientId, string operation = null, bool checkRepeat = true, bool increaseCount = true)
        {
            operation = operation ?? DefaultOperationName;
            
            IReadOnlyCollection<DateTime> callDates = await _callTimeLimitsRepository.GetCallHistoryAsync(operation, clientId);
            
            if (callDates.Any())
            {
                if (callDates.Count >= _callsLimit)
                    return CallLimitsResult.Failed(CallLimitStatus.LimitExceed);
                
                if (checkRepeat && DateTime.UtcNow - callDates.Last() < _repeatEnabledTimeSpan)
                    return CallLimitsResult.Failed(CallLimitStatus.CallTimeout);
            }

            if (increaseCount)
                await _callTimeLimitsRepository.InsertRecordAsync(operation, clientId);
            
            return CallLimitsResult.Success(callDates);
        }

        public Task<int> GetCallsCountAsync(string method, string clientId)
        {
            return _callTimeLimitsRepository.GetCallsCountAsync(method, clientId);
        }

        public Task ClearCallsHistoryAsync(string method, string clietnId)
        {
            return _callTimeLimitsRepository.ClearCallsHistoryAsync(method, clietnId);
        }
    }
}
