using System;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.ClientAccount.Client;
using Lykke.Service.ConfirmationCodes.Contract.Models;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Messages;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    internal class ConfirmationCodesService : IConfirmationCodesService
    {
        private readonly ISmsVerificationCodeRepository _smsVerificationCodeRepository;
        private readonly ISmsCommandProducer _smsCommandProducer;        
        private readonly ISupportToolsSettings _supportToolsSettings;
        private readonly ICallTimeLimitsService _callTimeLimitsService;
        private readonly IClientAccountClient _clientAccountService;
        private readonly IDeploymentSettings _deploymentSettings;

        public ConfirmationCodesService(ISmsVerificationCodeRepository smsVerificationCodeRepository,
            ISmsCommandProducer smsCommandProducer,
            ICallTimeLimitsService callTimeLimitsService,
            IClientAccountClient clientAccountService,
            IDeploymentSettings deploymentSettings,
            ISupportToolsSettings supportToolsSettings)
        {
            _smsVerificationCodeRepository = smsVerificationCodeRepository;
            _smsCommandProducer = smsCommandProducer;            

            _callTimeLimitsService = callTimeLimitsService;
            _clientAccountService = clientAccountService;
            _deploymentSettings = deploymentSettings;            
            _supportToolsSettings = supportToolsSettings;
        }

        public async Task<SmsRequestResult> RequestSmsCode(SmsCodeRequest request)
        {
            var callLimitsResult = await _callTimeLimitsService.ProcessCallLimitsAsync(request.ClientId, request.Operation);

            if (callLimitsResult.Status != CallLimitStatus.Allowed)
            {
                return SmsRequestResult.FailedResult(callLimitsResult.Status);
            }

            var smsSettings = await _clientAccountService.GetSmsAsync(request.ClientId);

            //if there were some precedent calls in prev. 5 mins - we should try another SMS provider
            var useAlternativeProvider = callLimitsResult.CallDates.Any()
                ? !smsSettings.UseAlternativeProvider
                : smsSettings.UseAlternativeProvider;

            await _clientAccountService.SetSmsAsync(request.ClientId, useAlternativeProvider);

            ISmsVerificationCode smsCode;
            //todo: refactor if
            if (request.IsPriority)
            {
                var expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);
                smsCode = await _smsVerificationCodeRepository.CreatePriorityAsync(request.PartnerId, request.PhoneNumber, expDate);
            }
            else
            {
                smsCode = await _smsVerificationCodeRepository.CreateAsync(request.PartnerId, request.PhoneNumber,
                    _deploymentSettings.IsProduction);
            }

            await _smsCommandProducer.SendSms(
                request.PartnerId,
                request.PhoneNumber,
                new SmsConfirmationData
                {
                    ConfirmationCode = smsCode.Code
                },
                smsSettings.UseAlternativeProvider,
                request.Reason,
                request.OuterRequestId);

            return SmsRequestResult.SuccessResult(smsCode.Code);
        }

        public async Task<string> RequestSmsCode(string partnerId, string phoneNumber, bool isPriority, string reason, string outerRequestId)
        {
            ISmsVerificationCode smsCode;
            //todo: refactor if
            if (isPriority)
            {
                var expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);
                smsCode = await _smsVerificationCodeRepository.CreatePriorityAsync(partnerId, phoneNumber, expDate);
            }
            else
            {
                smsCode = await _smsVerificationCodeRepository.CreateAsync(partnerId, phoneNumber,
                    _deploymentSettings.IsProduction);
            }

            await _smsCommandProducer.SendSms(
                partnerId,
                phoneNumber,
                new SmsConfirmationData
                {
                    ConfirmationCode = smsCode.Code
                },
                false,
                reason,
                outerRequestId);

            return smsCode.Code;
        }

        public async Task<bool> CheckAsync(string partnerId, string mobilePhone, string code)
        {
            return await _smsVerificationCodeRepository.CheckAsync(partnerId, mobilePhone, code);
        }
        
        public async Task<SmsCheckResult> CheckSmsAsync(string clientId, string mobilePhone, string code, string operation)
        {
            var smsCheck = new SmsCheckResult();
            
            var callLimitsResult = await _callTimeLimitsService.ProcessCallLimitsAsync(clientId, operation, false);
    
            if (callLimitsResult.Status != CallLimitStatus.Allowed)
            {
                smsCheck.Status = callLimitsResult.Status;
                return smsCheck;
            }
            
            smsCheck.Result = await _smsVerificationCodeRepository.CheckAsync(null, mobilePhone, code);
            return smsCheck;
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
