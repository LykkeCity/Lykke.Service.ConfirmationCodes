using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Heplers;
using Lykke.Service.ConfirmationCodes.Core.Messages;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    internal class ConfirmationCodesService : IConfirmationCodesService
    {
        private readonly ISmsVerificationCodeRepository _smsVerificationCodeRepository;
        private readonly ISmsRequestProducer _smsRequestProducer;        
        private readonly ISupportToolsSettings _supportToolsSettings;
        private readonly ICallTimeLimitsRepository _callTimeLimitsRepository;
        private readonly IClientAccountClient _clientAccountService;
        private readonly IDeploymentSettings _deploymentSettings;
        
        private const string GetMethodName = "ConfirmationCodesService.RequestSmsCode";
        private readonly TimeSpan _repeatCallsTimeSpan = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _repeatEnabledTimeSpan = TimeSpan.FromSeconds(45);
        private int _callLimit = 2;

        public ConfirmationCodesService(ISmsVerificationCodeRepository smsVerificationCodeRepository,
            ISmsRequestProducer smsRequestProducer,
            ICallTimeLimitsRepository callTimeLimitsRepository,
            IClientAccountClient clientAccountService,
            IDeploymentSettings deploymentSettings, 
            ISupportToolsSettings supportToolsSettings)
        {
            _smsVerificationCodeRepository = smsVerificationCodeRepository;
            _smsRequestProducer = smsRequestProducer;            

            _callTimeLimitsRepository = callTimeLimitsRepository;
            _clientAccountService = clientAccountService;
            _deploymentSettings = deploymentSettings;            
            _supportToolsSettings = supportToolsSettings;
        }

        public async Task<string> RequestSmsCode(string partnerId, string clientId, string phoneNumber, bool isPriority = false, int expirationInterval = 0, int codeLength = 6)
        {
            DateTime expDate;

            var callHistory =
                await _callTimeLimitsRepository.GetCallHistoryAsync(GetMethodName, clientId, _repeatCallsTimeSpan);

            if (!callHistory.Any() || callHistory.IsCallEnabled(_repeatEnabledTimeSpan, _callLimit))
            {
                await _callTimeLimitsRepository.InsertRecordAsync(GetMethodName, clientId);

                var smsSettings = await _clientAccountService.GetSmsAsync(clientId);

                //if there were some precedent calls in prev. 5 mins - we should try another SMS provider
                var useAlternativeProvider = callHistory.Any()
                    ? !smsSettings.UseAlternativeProvider
                    : smsSettings.UseAlternativeProvider;

                await _clientAccountService.SetSmsAsync(clientId, useAlternativeProvider);

                ISmsVerificationCode smsCode;
                //todo: refactor if
                if (isPriority)
                {
                    if (expirationInterval != 0 && expirationInterval <= _supportToolsSettings.PriorityCodeExpirationInterval)
                        expDate = DateTime.UtcNow.AddSeconds(expirationInterval);
                    else
                        expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);

                    smsCode = await _smsVerificationCodeRepository.CreatePriorityAsync(partnerId, phoneNumber, expDate, codeLength);
                }
                else
                {
                    smsCode = await _smsVerificationCodeRepository.CreateAsync(partnerId, phoneNumber,
                        _deploymentSettings.IsProduction, codeLength);
                }

                await _smsRequestProducer.SendSmsAsync(partnerId, phoneNumber,
                        new SmsConfirmationData { ConfirmationCode = smsCode.Code },
                        smsSettings.UseAlternativeProvider);
                return smsCode.Code;
            }

            return null;
        }

        public async Task<string> RequestSmsCode(string partnerId, string phoneNumber, bool isPriority = false, int expirationInterval = 0, int codeLength = 6)
        {
            ISmsVerificationCode smsCode;
            DateTime expDate;
            //todo: refactor if
            if (isPriority)
            {
                if (expirationInterval != 0 && expirationInterval <= _supportToolsSettings.PriorityCodeExpirationInterval)
                    expDate = DateTime.UtcNow.AddSeconds(expirationInterval);
                else
                    expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);

                smsCode = await _smsVerificationCodeRepository.CreatePriorityAsync(partnerId, phoneNumber, expDate, codeLength);
            }
            else
            {
                smsCode = await _smsVerificationCodeRepository.CreateAsync(partnerId, phoneNumber,
                    _deploymentSettings.IsProduction, codeLength);
            }

            await _smsRequestProducer.SendSmsAsync(partnerId, phoneNumber,
                new SmsConfirmationData {ConfirmationCode = smsCode.Code}, false);

            return smsCode.Code;
        }

        public async Task<bool> CheckAsync(string partnerId, string mobilePhone, string code)
        {
            return await _smsVerificationCodeRepository.CheckAsync(partnerId, mobilePhone, code);
        }

        public async Task<ISmsVerificationPriorityCode> GetPriorityCode(string partnerId, string mobilePhone)
        {
            var code = await _smsVerificationCodeRepository.GetActualCode(partnerId, mobilePhone);

            return code as ISmsVerificationPriorityCode;
        }

        public Task DeleteCodes(string partnerId, string phoneNum)
        {
            return _smsVerificationCodeRepository.DeleteCodesByPhoneNumAsync(partnerId, phoneNum);
        }
    }
}
