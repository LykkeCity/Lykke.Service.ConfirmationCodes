using System;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class SmsVerificationPriorityCodeEntity : SmsVerificationCodeEntity, ISmsVerificationPriorityCode
    {
        public DateTime ExpirationDate { get; set; }
    }
}
