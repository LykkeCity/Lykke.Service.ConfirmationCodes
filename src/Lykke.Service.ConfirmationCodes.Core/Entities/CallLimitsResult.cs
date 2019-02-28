using System;
using System.Collections.Generic;
using Lykke.Service.ConfirmationCodes.Contract.Models;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public class CallLimitsResult
    {
        public CallLimitStatus Status { get; set; }
        public IReadOnlyCollection<DateTime> CallDates { get; set; } = Array.Empty<DateTime>();


        public static CallLimitsResult Success(IReadOnlyCollection<DateTime> callDates)
        {
            return new CallLimitsResult
            {
                Status = CallLimitStatus.Allowed,
                CallDates = callDates
            };
        }
        
        public static CallLimitsResult Failed(CallLimitStatus status)
        {
            return new CallLimitsResult
            {
                Status = status
            };
        }
    }
}
