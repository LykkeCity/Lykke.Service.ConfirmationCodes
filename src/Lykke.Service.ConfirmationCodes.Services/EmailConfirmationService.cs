using System;
using System.Threading.Tasks;
using Lykke.Messages.Email;
using Lykke.Messages.Email.MessageData;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Repositories;
using Lykke.Service.ConfirmationCodes.Core.Services;

namespace Lykke.Service.ConfirmationCodes.Services
{
    internal class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IEmailVerificationCodeRepository _emailVerificationCodeRepository;
        private readonly IEmailSender _emailSender;
        private readonly ISupportToolsSettings _supportToolsSettings;
        private readonly IDeploymentSettings _deploymentSettings;

        public EmailConfirmationService(IEmailVerificationCodeRepository emailVerificationCodeRepository,
            IEmailSender emailSender,
            IDeploymentSettings deploymentSettings,
            ISupportToolsSettings supportToolsSettings)
        {
            _deploymentSettings = deploymentSettings;
            _emailVerificationCodeRepository = emailVerificationCodeRepository;
            _emailSender = emailSender;
            _supportToolsSettings = supportToolsSettings;
        }

        public async Task<string> SendConfirmEmail(string email, string partnerId, bool isPriority = false, int codeLength = 6)
        {
            IEmailVerificationCode emailCode;
            if (isPriority)
            {
                var expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);
                emailCode = await _emailVerificationCodeRepository.CreatePriorityAsync(email, partnerId, expDate, codeLength);
            }
            else
            {
                emailCode = await _emailVerificationCodeRepository.CreateAsync(email, partnerId, _deploymentSettings.IsProduction, codeLength);
            }

            var msgData = new EmailComfirmationData
            {
                ConfirmationCode = emailCode.Code,
                Year = DateTime.UtcNow.Year.ToString()
            };

            await _emailSender.SendEmailAsync(partnerId, email, msgData);

            return emailCode.Code;
        }

        public async Task<string> SendConfirmCypEmail(string email, string partnerId, bool createPriorityCode = false, int codeLength = 6)
        {
            IEmailVerificationCode emailCode;
            if (createPriorityCode)
            {
                var expDate = DateTime.UtcNow.AddSeconds(_supportToolsSettings.PriorityCodeExpirationInterval);
                emailCode = await _emailVerificationCodeRepository.CreatePriorityAsync(email, partnerId, expDate, codeLength);
            }
            else
            {
                emailCode = await _emailVerificationCodeRepository.CreateAsync(email, partnerId, _deploymentSettings.IsProduction, codeLength);
            }

            var msgData = new EmailComfirmationCypData
            {
                ConfirmationCode = emailCode.Code,
                Year = DateTime.UtcNow.Year.ToString()
            };

            await _emailSender.SendEmailAsync(partnerId, email, msgData);

            return emailCode.Code;
        }

        public async Task<bool> CheckAsync(string email, string partnerId, string code)
        {
            return await _emailVerificationCodeRepository.CheckAsync(email, partnerId, code);
        }

        public async Task<IEmailVerificationPriorityCode> GetPriorityCode(string email, string partnerId)
        {
            var code = await _emailVerificationCodeRepository.GetActualCode(email, partnerId);

            return code as IEmailVerificationPriorityCode;
        }

        public Task DeleteCodes(string email, string partnerId)
        {
            return _emailVerificationCodeRepository.DeleteCodesByEmailAsync(email, partnerId);
        }
    }
}
