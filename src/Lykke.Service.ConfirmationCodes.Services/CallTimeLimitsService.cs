using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    public class CallTimeLimitsService : ICallTimeLimitsService
    {
        private readonly ICallTimeLimitsRepository _callTimeLimitsRepository;
        private const string DefaultOperationName = "CallTimeLimitsService.ProcessCall";
        private readonly TimeSpan _repeatEnabledTimeSpan = TimeSpan.FromMinutes(1);
        private int _callLimit = 3;

        public CallTimeLimitsService(
            ICallTimeLimitsRepository callTimeLimitsRepository
            )
        {
            _callTimeLimitsRepository = callTimeLimitsRepository;
        }
        
        public async Task<CallLimitsResult> ProcessCallLimitsAsync(string clientId, string operation = null, bool checkRepeat = true)
        {
            operation = operation ?? DefaultOperationName;
            
            IReadOnlyCollection<DateTime> callDates = await _callTimeLimitsRepository.GetCallHistoryAsync(operation, clientId);
            
            if (callDates.Any())
            {
                if (callDates.Count >= _callLimit)
                {
                    return CallLimitsResult.Failed(CallLimitStatus.LimitExceed);
                }
                
                if (checkRepeat && DateTime.UtcNow - callDates.Last() < _repeatEnabledTimeSpan)
                {
                    return CallLimitsResult.Failed(CallLimitStatus.CallTimeout);
                }
            }

            await _callTimeLimitsRepository.InsertRecordAsync(operation, clientId);
            
            return CallLimitsResult.Success(callDates);
        }
    }
}
