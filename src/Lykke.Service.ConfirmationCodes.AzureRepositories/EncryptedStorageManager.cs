using System;
using AzureStorage;
using AzureStorage.Tables.Decorators;
using Lykke.AzureStorage.Cryptography;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories
{
    public class EncryptedStorageManager
    {
        private const string InitKey = "init";

        private readonly INoSQLTableStorage<EncryptionInitModel> _storage;

        public EncryptedStorageManager(INoSQLTableStorage<EncryptionInitModel> storage)
        {
            _storage = storage;
        }

        public bool HasKey => Serializer != null;
        public ICryptographicSerializer Serializer { get; private set; }

        public bool TrySetKey(string key, out string error)
        {
            error = null;

            if (HasKey)
            {
                error = "Key is already installed.";
                return false;
            }

            ICryptographicSerializer serializer;
            try
            {
                serializer = new AesSerializer(key);
            }
            catch (Exception ex)
            {
                error = $"Wrong key format. {ex.Message}";
                return false;
            }
            var encryptedStorage = EncryptedTableStorageDecorator<EncryptionInitModel>.Create(_storage, serializer);

            if (WasEncryptionSet())
            {
                try
                {
                    var existingValue = encryptedStorage.GetDataAsync(InitKey, InitKey).GetAwaiter().GetResult();
                    if (existingValue.Data == InitKey)
                    {
                        Serializer = serializer;
                        return true;
                    }
                    else
                    {
                        error = "The specified key is incorrect.";
                        return false;
                    }
                }
                catch (System.Security.Cryptography.CryptographicException)
                {
                    error = "The specified key is incorrect.";
                    return false;
                }
            }
            else
            {
                // this is a new and the only one key
                encryptedStorage.InsertAsync(new EncryptionInitModel { PartitionKey = InitKey, RowKey = InitKey, Data = InitKey }).GetAwaiter().GetResult();
                Serializer = serializer;
                return true;
            }
        }

        public bool WasEncryptionSet()
        {
            return _storage.GetDataAsync(InitKey, InitKey).GetAwaiter().GetResult() != null;
        }
    }
}
