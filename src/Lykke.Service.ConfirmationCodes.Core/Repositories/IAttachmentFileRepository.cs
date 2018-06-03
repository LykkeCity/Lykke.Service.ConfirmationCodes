using System.IO;
using System.Threading.Tasks;

namespace Lykke.Service.ConfirmationCodes.Core.Repositories
{
    public interface IAttachmentFileRepository
    {
        Task<string> InsertAttachment(Stream stream);
        Task<Stream> GetAttachment(string fileId);
    }
}
