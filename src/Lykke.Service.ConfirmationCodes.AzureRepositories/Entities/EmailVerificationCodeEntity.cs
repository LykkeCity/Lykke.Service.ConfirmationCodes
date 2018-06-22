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
    }
}
