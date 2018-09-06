using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.GoogleAuthenticator;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    [UsedImplicitly]
    public class Google2FaService : IGoogle2FaService
    {
        private readonly IGoogle2FaRepository _google2FaRepository;
        private readonly int _qrSideLength = 100;
        private readonly string _appName;

        public Google2FaService(
            IGoogle2FaRepository google2FaRepository)
        {
            _google2FaRepository = google2FaRepository;
            _appName = "Lykke";
        }

        public async Task<bool> ClientHasEnabledAsync(string clientId)
        {
            var entry = await _google2FaRepository.GetAsync(clientId);

            return entry != null && entry.IsActive;
        }

        public async Task<bool> ClientHasPendingAsync(string clientId)
        {
            var entry = await _google2FaRepository.GetAsync(clientId);

            return entry != null && !entry.IsActive;
        }

        public async Task ActivateAsync(string clientId)
        {
            var entity = await _google2FaRepository.GetAsync(clientId);
            
            if(entity != null && entity.IsActive)
                throw new InvalidOperationException($"Cannot activate existing & active record for user {clientId}");
            
            await _google2FaRepository.UpdateAsync(clientId, true);
        }

        public async Task<string> CreateAsync(string clientId)
        {
            var entity = await _google2FaRepository.GetAsync(clientId);
            
            if(entity != null && entity.IsActive)
                throw new InvalidOperationException($"Cannot create a new record for {clientId}, there already is an active record");

            var newSecret = Guid.NewGuid() + "_" + clientId;

            await _google2FaRepository.InsertOrUpdateAsync(clientId, newSecret);
            
            var setupInfo = new TwoFactorAuthenticator().GenerateSetupCode(_appName, clientId, newSecret, _qrSideLength, _qrSideLength, true);

            return setupInfo.ManualEntryKey;
        }

        public async Task<bool> CheckCodeAsync(string clientId, string code, bool isActivationCheck = false)
        {
            var entity = await _google2FaRepository.GetAsync(clientId);
            
            if(entity == null || (!entity.IsActive && !isActivationCheck))
                throw new InvalidOperationException($"Cannot check code for {clientId} until their record exists and is active");

            return CheckTwoFactorAuthenticator(entity.Secret, code);
        }
        
        private static bool CheckTwoFactorAuthenticator(string secret, string code)
        {
            return new TwoFactorAuthenticator().ValidateTwoFactorPIN(secret, code);
        }
    }
}
