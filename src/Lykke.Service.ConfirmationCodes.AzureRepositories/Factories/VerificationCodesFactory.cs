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

        public EmailVerificationCodeEntity CreateEmailVerificationCode(string email, string partnerId = null, bool generateRealCode = true, int codeLength = 6)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var randomCode = _randomValueGenerator.GetCode(codeLength).ToString();

            var entity = new EmailVerificationCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = generateRealCode ? randomCode : "0000",
                CreationDateTime = creationDt
            };

            return entity;
        }

        public EmailVerificationPriorityCodeEntity CreateEmailVerificationPriorityCode(string email, string partnerId,
            DateTime expirationDt, int codeLength)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var randomCode = _randomValueGenerator.GetCode(codeLength).ToString();

            var entity = new EmailVerificationPriorityCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = randomCode,
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
            return entity;
        }

        public SmsVerificationCodeEntity CreateSmsVerificationCode(string phone, string partnerId = null, bool generateRealCode = true, int codeLength = 6)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var randomCode = _randomValueGenerator.GetCode(codeLength).ToString();
            return new SmsVerificationCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = generateRealCode ? randomCode : "0000",
                CreationDateTime = creationDt
            };
        }

        public SmsVerificationPriorityCodeEntity CreateSmsVerificationPriorityCode(string phone, string partnerId,
            DateTime expirationDt, int codeLength = 6)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var randomCode = _randomValueGenerator.GetCode(codeLength).ToString();
            return new SmsVerificationPriorityCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = randomCode,
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
        }
    }
}
