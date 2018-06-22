using System;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public interface ISmsVerificationPriorityCode : ISmsVerificationCode
    {
        DateTime ExpirationDate { get; set; }
    }
}
