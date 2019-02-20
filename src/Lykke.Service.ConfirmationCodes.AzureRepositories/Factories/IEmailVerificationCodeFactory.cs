using System;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface IEmailVerificationCodeFactory
    {
        EmailVerificationCodeEntity CreateEmailVerificationCode(string email, string partnerId = null, bool generateRealCode = true, int codeLength = 4);
        EmailVerificationPriorityCodeEntity CreateEmailVerificationPriorityCode(string email, string partnerId, DateTime expirationDt, int codeLength = 4);
    }
}
