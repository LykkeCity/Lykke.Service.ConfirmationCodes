using System;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface IEmailVerificationCodeFactory
    {
        EmailVerificationCodeEntity CreateEmailVerificationCode(string email, string partnerId = null, bool generateRealCode = true);
        EmailVerificationPriorityCodeEntity CreateEmailVerificationPriorityCode(string email, string partnerId, DateTime expirationDt);
    }
}
