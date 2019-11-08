using System;
using System.Threading.Tasks;
using AzureStorage;
using JetBrains.Annotations;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Repositories
{
    [UsedImplicitly]
    public class Google2FaRepository : IGoogle2FaRepository
    {
        private readonly INoSQLTableStorage<Google2FaSecretEntity> _tableStorage;

        public Google2FaRepository(INoSQLTableStorage<Google2FaSecretEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IGoogle2FaSecret> GetAsync(string clientId)
        {
            return
                await _tableStorage.GetDataAsync(
                    Google2FaSecretEntity.GeneratePartitionKey(),
                    clientId);
        }

        public async Task<IGoogle2FaSecret> UpdateAsync(string clientId, bool isActive)
        {
            var entity = await _tableStorage.GetDataAsync(
                Google2FaSecretEntity.GeneratePartitionKey(),
                clientId);

            if(entity == null)
                throw new InvalidOperationException();

            entity.IsActive = isActive;

            await _tableStorage.InsertOrMergeAsync(entity);

            return entity;
        }

        public async Task<IGoogle2FaSecret> InsertOrUpdateAsync(string clientId, string secret)
        {
            var entity = Google2FaSecretEntity.Create(clientId, secret);

            await _tableStorage.InsertOrMergeAsync(entity);

            return entity;
        }

        public Task RemoveAsync(string clientId)
        {
            return _tableStorage.DeleteIfExistAsync(Google2FaSecretEntity.GeneratePartitionKey(), clientId);
        }
    }
}
