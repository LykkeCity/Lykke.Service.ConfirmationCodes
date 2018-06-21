using System;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class EmailVerificationPriorityCodeEntity : EmailVerificationCodeEntity, IEmailVerificationPriorityCode
    {
        public DateTime ExpirationDate { get; set; }
    }
}
