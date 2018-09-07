namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public interface IGoogle2FaSecret
    {
        string ClientId { get; }
        string Secret { get; }
        bool IsActive { get; }
    }
}
