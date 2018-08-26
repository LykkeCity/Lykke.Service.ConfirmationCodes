using AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Decorators;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class Google2FaSecretEntity : TableEntity, IGoogle2FaSecret
    {
        public string ClientId => RowKey;
        [Encrypt]
        public string Secret { get; set; }
        public bool IsActive { get; set; }

        public static string GeneratePartitionKey()
        {
            return "part";
        }

        public static Google2FaSecretEntity Create(string clientId, string secret)
        {
            return new Google2FaSecretEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = clientId,
                Secret = secret,
                IsActive = false
            };
        }
    }
}
