using System;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    public interface ISmsVerificationCodeFactory
    {
        SmsVerificationCodeEntity CreateSmsVerificationCode(string phone, string partnerId = null, bool generateRealCode = true, int codeLength = 4);
        SmsVerificationPriorityCodeEntity CreateSmsVerificationPriorityCode(string phoneNum, string partnerId, DateTime expirationDt, int codeLength = 4);
    }
}
