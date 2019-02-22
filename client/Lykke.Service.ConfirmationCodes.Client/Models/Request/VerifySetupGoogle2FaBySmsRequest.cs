namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    public class VerifySetupGoogle2FaBySmsRequest
    {
        public string ClientId { set; get; }
        public string Phone { set; get; }
        public string SmsCode { set; get; }
    }
}
