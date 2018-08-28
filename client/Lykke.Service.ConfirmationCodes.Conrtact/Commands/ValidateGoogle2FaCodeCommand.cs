using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Conrtact.Commands
{
    [MessagePackObject(true)]
    public class ValidateGoogle2FaCodeCommand
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
        public string Code { set; get; }
    }
}
