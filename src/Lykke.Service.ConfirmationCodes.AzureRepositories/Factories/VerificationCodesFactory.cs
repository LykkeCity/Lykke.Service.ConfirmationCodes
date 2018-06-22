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

        public EmailVerificationCodeEntity CreateEmailVerificationCode(string email, string partnerId = null, bool generateRealCode = true)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            var entity = new EmailVerificationCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = generateRealCode ? _randomValueGenerator.GetInt(1, 9999).ToString("0000") : "0000",
                CreationDateTime = creationDt
            };

            return entity;
        }

        public EmailVerificationPriorityCodeEntity CreateEmailVerificationPriorityCode(string email, string partnerId,
            DateTime expirationDt)
        {
            var creationDt = _dateTimeProvider.GetDateTime();

            var entity = new EmailVerificationPriorityCodeEntity
            {
                RowKey = EmailVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = EmailVerificationCodeEntity.GeneratePartitionKey(email, partnerId),
                Code = _randomValueGenerator.GetInt(1, 9999).ToString("0000"),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
            return entity;
        }

        public SmsVerificationCodeEntity CreateSmsVerificationCode(string phone, string partnerId = null, bool generateRealCode = true)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            return new SmsVerificationCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = generateRealCode ? _randomValueGenerator.GetInt(1, 9999).ToString("0000") : "0000",
                CreationDateTime = creationDt
            };
        }

        public SmsVerificationPriorityCodeEntity CreateSmsVerificationPriorityCode(string phone, string partnerId,
            DateTime expirationDt)
        {
            var creationDt = _dateTimeProvider.GetDateTime();
            return new SmsVerificationPriorityCodeEntity
            {
                RowKey = SmsVerificationCodeEntity.GenerateRowKey(creationDt),
                PartitionKey = SmsVerificationCodeEntity.GeneratePartitionKey(partnerId, phone),
                Code = _randomValueGenerator.GetInt(1, 9999).ToString("0000"),
                CreationDateTime = creationDt,
                ExpirationDate = expirationDt
            };
        }
    }
}
