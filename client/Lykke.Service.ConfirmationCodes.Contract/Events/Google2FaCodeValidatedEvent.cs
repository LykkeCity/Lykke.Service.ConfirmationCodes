using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Contract.Events
{
    [MessagePackObject(true)]
    public class Google2FaCodeValidatedEvent
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
        public Google2FaValidationResult ValidationResult { set; get; }
    }

    public enum Google2FaValidationResult
    {
        Ok, Fail, NotSetUp
    }
}
