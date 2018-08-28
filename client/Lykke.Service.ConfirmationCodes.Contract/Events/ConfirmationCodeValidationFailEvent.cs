using MessagePack;

namespace Lykke.Service.ConfirmationCodes.Contract.Events
{
    [MessagePackObject(true)]
    public class ConfirmationCodeValidationFailEvent
    {
        public string Id { set; get; }
        public string ClientId { set; get; }
        public ValidationFailReason Reason {set; get; }
    }
    
    public enum ValidationFailReason
    {
        InvalidConfirmation,
        SecondFactorNotSetUp
    }
}
