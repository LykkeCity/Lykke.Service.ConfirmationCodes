using System;
using Common;
using Lykke.Service.ConfirmationCodes.Core.Entities;
using Lykke.Service.ConfirmationCodes.Core.Heplers;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ConfirmationCodes.AzureRepositories.Entities
{
    public class SmsVerificationCodeEntity : TableEntity, ISmsVerificationCode
    {
        private static int codeLeghth = 4;

        public string Id => RowKey;
        public string Phone => PartitionKey;
        public string Code { get; set; }
        public DateTime CreationDateTime { get; set; }

        public static string GenerateRowKey(DateTime creationDt)
        {
            return IdGenerator.GenerateDateTimeIdNewFirst(creationDt);
        }

        public static string GeneratePartitionKey(string partnerId, string phoneNumber)
        {
            var phoneNumberE164 = phoneNumber.ToE164Number();
            if (phoneNumberE164 == null)
                throw new ArgumentException("phoneNumber");

            return string.IsNullOrWhiteSpace(partnerId)
                ? phoneNumberE164.GetSha256Hash()
                : $"{partnerId}_{phoneNumberE164}".GetSha256Hash();
        }

        protected static string GenerateRandomCode()
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);
            return rand.Next(9999).ToString(new string('0', codeLeghth));
        }

        public static SmsVerificationCodeEntity Create(string partnerId, string phone, DateTime creationDt, bool generateRealCode)
        {
            return new SmsVerificationCodeEntity
            {
                RowKey = GenerateRowKey(creationDt),
                PartitionKey = GeneratePartitionKey(partnerId, phone),
                Code = generateRealCode ? GenerateRandomCode() : "0000",
                CreationDateTime = creationDt
            };
        }
    }
}
