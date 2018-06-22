using System;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Factories;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Repositories
{
    public class EmailVerificationCodeRepository : IEmailVerificationCodeRepository
    {
        private readonly INoSQLTableStorage<EmailVerificationCodeEntity> _tableStorage;
        private readonly INoSQLTableStorage<EmailVerificationPriorityCodeEntity> _priorityCodesStorage;
        private readonly IEmailVerificationCodeFactory _emailVerificationCodeFactory;

        public EmailVerificationCodeRepository(
            INoSQLTableStorage<EmailVerificationCodeEntity> tableStorage,
            INoSQLTableStorage<EmailVerificationPriorityCodeEntity> priorityCodesStorage,
            IEmailVerificationCodeFactory emailVerificationCodeFactory
            )
        {
            _emailVerificationCodeFactory = emailVerificationCodeFactory;
            _tableStorage = tableStorage;
            _priorityCodesStorage = priorityCodesStorage;
        }

        public async Task<IEmailVerificationCode> CreateAsync(string email, string partnerId, bool generateRealCode)
        {
            var entity = _emailVerificationCodeFactory.CreateEmailVerificationCode(email, partnerId, generateRealCode);
            await _tableStorage.InsertAsync(entity);
            return entity;
        }

        public async Task<IEmailVerificationPriorityCode> CreatePriorityAsync(string email, string partnerId,
            DateTime expirationDt)
        {
            var entity = _emailVerificationCodeFactory.CreateEmailVerificationPriorityCode(email, partnerId, expirationDt);
            await _priorityCodesStorage.InsertAsync(entity);
            return entity;
        }

        public async Task<IEmailVerificationCode> GetActualCode(string email, string partnerId)
        {
            var priorityCode =
                await _priorityCodesStorage.GetTopRecordAsync(
                    EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId));

            if (priorityCode == null || priorityCode.ExpirationDate < DateTime.UtcNow)
            {
                return await _tableStorage.GetTopRecordAsync(
                    EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId));
            }

            return priorityCode;
        }

        public async Task<bool> CheckAsync(string email, string partnerId, string codeToCheck)
        {
            var actualCode = await GetActualCode(email, partnerId);
            return actualCode != null && actualCode.Code == codeToCheck;
        }

        public Task DeleteCodesByEmailAsync(string email, string partnerId)
        {
            return Task.WhenAll(
                DeleteCodesInRepository(email, partnerId, _tableStorage),
                DeleteCodesInRepository(email, partnerId, _priorityCodesStorage)
            );
        }

        private static async Task DeleteCodesInRepository<T>(string email, string partnerId,
            INoSQLTableStorage<T> storage) where T : ITableEntity, new()
        {
            var batchOperation = new TableBatchOperation();
            var entitiesToDelete =
                (await storage.GetDataAsync(EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId)))
                .ToArray();
            if (entitiesToDelete.Any())
            {
                foreach (var e in entitiesToDelete)
                    batchOperation.Delete(e);
                await storage.DoBatchAsync(batchOperation);
            }
        }
    }
}
