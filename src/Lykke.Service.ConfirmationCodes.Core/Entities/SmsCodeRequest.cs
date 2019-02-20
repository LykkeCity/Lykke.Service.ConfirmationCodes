namespace Lykke.Service.ConfirmationCodes.Core.Entities
{
    public class SmsCodeRequest
    {
        public string ClientId { get; set; }
        public string PartnerId { get; set; }
        public string PhoneNumber { get; set; }
        public string Operation { get; set; }
        public bool IsPriority { get; set; }
    }
}
