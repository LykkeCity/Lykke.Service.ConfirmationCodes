using Lykke.Service.ConfirmationCodes.Contract.Models;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public class SmsCheckResult
    {
        public CallLimitStatus Status { get; set; }
        public bool Result { get; set; }
    }
}
