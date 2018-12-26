using System;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface ISmsVerificationCodeFactory
    {
        SmsVerificationCodeEntity CreateSmsVerificationCode(string phone, string partnerId = null, bool generateRealCode = true);
        SmsVerificationPriorityCodeEntity CreateSmsVerificationPriorityCode(string phoneNum, string partnerId, DateTime expirationDt);
    }
}
