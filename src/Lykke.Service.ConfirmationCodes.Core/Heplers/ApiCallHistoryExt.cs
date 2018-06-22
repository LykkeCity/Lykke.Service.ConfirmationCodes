using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.ConfirmationCodes.Core.Heplers
{
    public static class ApiCallHistoryExt
    {
        public static bool IsCallEnabled(this IReadOnlyCollection<DateTime> history, TimeSpan period, int callLimit)
        {
            return history.Count < callLimit || DateTime.UtcNow - history.Last() > period;
        }
    }
}
