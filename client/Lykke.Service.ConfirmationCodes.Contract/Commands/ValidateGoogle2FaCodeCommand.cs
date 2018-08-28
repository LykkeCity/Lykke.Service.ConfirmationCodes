using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Contract.Commands
{
    [MessagePackObject(true)]
    public class ValidateGoogle2FaCodeCommand
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
        public string Code { set; get; }
    }
}
