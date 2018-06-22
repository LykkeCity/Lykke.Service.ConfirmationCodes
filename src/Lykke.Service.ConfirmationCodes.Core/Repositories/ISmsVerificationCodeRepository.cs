using System;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface ISmsVerificationCodeRepository
    {
        Task<ISmsVerificationCode> CreateAsync(string partnerId, string phoneNum, bool generateRealCode);

        Task<ISmsVerificationCode> CreatePriorityAsync(string partnerId, string phoneNum, DateTime expirationDt);

        /// <summary>
        /// Returns the latest generated code
        /// </summary>
        /// <param name="phoneNum">Phone number</param>
        /// <returns></returns>
        Task<ISmsVerificationCode> GetActualCode(string partnerId, string phoneNum);

        Task<bool> CheckAsync(string partnerId, string phoneNum, string codeToCheck);

        Task DeleteCodesByPhoneNumAsync(string partnerId, string phoneNum);
    }
}
