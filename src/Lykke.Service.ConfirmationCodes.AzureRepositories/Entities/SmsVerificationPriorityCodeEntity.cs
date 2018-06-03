using System;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class SmsVerificationPriorityCodeEntity : SmsVerificationCodeEntity, ISmsVerificationPriorityCode
    {
        public DateTime ExpirationDate { get; set; }

        public static SmsVerificationPriorityCodeEntity Create(string partnerId, string phone, DateTime creationDt,
            DateTime expirationDt)
        {
            return new SmsVerificationPriorityCodeEntity
            {
                RowKey = GenerateRowKey(creationDt),
                PartitionKey = GeneratePartitionKey(partnerId, phone),
                Code = GenerateRandomCode(),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
        }
    }
}
