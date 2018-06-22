using System;

namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public interface IEmailVerificationPriorityCode : IEmailVerificationCode
    {
        DateTime ExpirationDate { get; set; }
    }
}
