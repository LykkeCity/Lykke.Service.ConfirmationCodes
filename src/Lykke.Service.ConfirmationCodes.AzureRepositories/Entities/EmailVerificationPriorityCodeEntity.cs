using System;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class EmailVerificationPriorityCodeEntity : EmailVerificationCodeEntity, IEmailVerificationPriorityCode
    {
        public DateTime ExpirationDate { get; set; }

        public static EmailVerificationPriorityCodeEntity Create(string email, DateTime creationDt,
            DateTime expirationDt, string partnerId = null)
        {
            return new EmailVerificationPriorityCodeEntity
            {
                RowKey = GenerateRowKey(creationDt),
                PartitionKey = GeneratePartitionKey(email, partnerId),
                Code = GenerateRandomCode(),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
        }
    }
}
