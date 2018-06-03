using System;
using Common;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class EmailVerificationCodeEntity : TableEntity, IEmailVerificationCode
    {
        public string Id => RowKey;
        public string Code { get; set; }
        public DateTime CreationDateTime { get; set; }

        public static string GenerateRowKey(DateTime creationDt)
        {
            return IdGenerator.GenerateDateTimeIdNewFirst(creationDt);
        }

        public static string GeneratePartitionKey(string email, string partnerId)
        {
            return (string.IsNullOrEmpty(partnerId) ? $"{email}" : $"{email}_{partnerId}").SanitizeEmail();
        }

        protected static string GenerateRandomCode()
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);
            return rand.Next(9999).ToString("0000");
        }

        public static EmailVerificationCodeEntity Create(string email, DateTime creationDt, bool generateRealCode,
            string partnerId = null)
        {
            return new EmailVerificationCodeEntity
            {
                RowKey = GenerateRowKey(creationDt),
                PartitionKey = GeneratePartitionKey(email, partnerId),
                Code = generateRealCode ? GenerateRandomCode() : "0000",
                CreationDateTime = creationDt
            };
        }
    }
}
