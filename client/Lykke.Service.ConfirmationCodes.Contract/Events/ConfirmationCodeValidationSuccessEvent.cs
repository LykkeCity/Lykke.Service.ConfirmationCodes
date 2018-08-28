using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Contract.Events
{
    [MessagePackObject(true)]
    public class ConfirmationCodeValidationSuccessEvent
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
    }
}
