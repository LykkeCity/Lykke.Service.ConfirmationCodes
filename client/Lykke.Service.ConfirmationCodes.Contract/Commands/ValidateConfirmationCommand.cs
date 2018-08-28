using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Contract.Commands
{
    [MessagePackObject(true)]
    public class ValidateConfirmationCommand
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
        public string Type { set; get; }
        public string Confirmation { set; get; }
    }
}
