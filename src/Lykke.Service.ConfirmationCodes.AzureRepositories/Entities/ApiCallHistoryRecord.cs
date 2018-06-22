using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class ApiCallHistoryRecord : TableEntity
    {
        public static string GeneratePartitionKey(string method, string clientId)
        {
            return $"{method}_{clientId}";
        }

        public static ApiCallHistoryRecord Create(string method, string clientId)
        {
            return new ApiCallHistoryRecord
            {
                PartitionKey = GeneratePartitionKey(method, clientId),
                DateTime = DateTime.UtcNow
            };
        }

        public DateTime DateTime { get; set; }
    }
}
