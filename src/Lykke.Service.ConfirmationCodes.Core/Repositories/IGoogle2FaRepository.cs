using System.Threading.Tasks;
using Lykke.Service.ConfirmationCodes.Core.Entities;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface IGoogle2FaRepository
    {
        Task<IGoogle2FaSecret> GetAsync(string clientId);
        Task<IGoogle2FaSecret> UpdateAsync(string clientId, bool isActive);
        Task<IGoogle2FaSecret> InsertOrUpdateAsync(string clientId, string secret);
    }
}
