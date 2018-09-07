using Lykke.AzureStorage.Tables.Decorators;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class EncryptionInitModel: TableEntity
    {
        [Encrypt]
        public string Data { get; set; }
    }
}
