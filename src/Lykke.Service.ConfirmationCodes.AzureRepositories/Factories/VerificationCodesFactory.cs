using System;
using Lykke.Service.ConfirmationCodes.AzureRepositories.Entities;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Factories
{
    internal class VerificationCodesFactory : IEmailVerificationCodeFactory, ISmsVerificationCodeFactory
    {
        private readonly IRandomValueGenerator _randomValueGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VerificationCodesFactory(IRandomValueGenerator randomValueGenerator, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _randomValueGenerator = randomValueGenerator;
        }

        public EmailVerificationCodeEntity CreateEmailVerificationCode(string email, string partnerId = null, 
            bool generateRealCode = true, int codeLength = 4)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var entity = new EmailVerificationCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = _randomValueGenerator.GetCode(codeLength, generateRealCode),
                CreationDateTime = creationDt
            };

            return entity;
        }

        public EmailVerificationPriorityCodeEntity CreateEmailVerificationPriorityCode(string email, string partnerId,
            DateTime expirationDt, int codeLength = 4)
        {
            var creationDt = _dateTimeProvider.GetDateTime();

            var entity = new EmailVerificationPriorityCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = _randomValueGenerator.GetCode(codeLength, true),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
            return entity;
        }

        public SmsVerificationCodeEntity CreateSmsVerificationCode(string phone, string partnerId = null, 
            bool generateRealCode = true, int codeLength = 4)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            return new SmsVerificationCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = _randomValueGenerator.GetCode(codeLength, generateRealCode),
                CreationDateTime = creationDt
            };
        }

        public SmsVerificationPriorityCodeEntity CreateSmsVerificationPriorityCode(string phone, string partnerId,
            DateTime expirationDt, int codeLength = 4)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            return new SmsVerificationPriorityCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = _randomValueGenerator.GetCode(codeLength, true),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
        }
    }
}
