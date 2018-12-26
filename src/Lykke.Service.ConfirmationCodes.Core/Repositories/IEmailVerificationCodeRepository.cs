using System;
using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface IEmailVerificationCodeRepository
    {
        Task<IEmailVerificationCode> CreateAsync(string email, string partnerId, bool generateRealCode);

        Task<IEmailVerificationPriorityCode> CreatePriorityAsync(string email, string partnerId, DateTime expirationDt);

        /// <summary>
        /// Returns the latest generated code
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="partnerId">Partner Id</param>
        /// <returns></returns>
        Task<IEmailVerificationCode> GetActualCode(string email, string partnerId);

        Task<bool> CheckAsync(string email, string partnerId, string codeToCheck);

        Task DeleteCodesByEmailAsync(string email, string partnerId);
    }
}
