namespace Lykke.Service.ConfirmationCodes.Client.Models.Request
{
    public class VerifySetupGoogle2FaRequest
    {
        public string ClientId { set; get; }
        public string Code { set; get; }
    }
}
