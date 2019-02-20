using Lykke.Service.ConfirmationCodes.Contract.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Response
{
    /// <summary>
    /// Sms confirmation response
    /// </summary>
    public class SmsConfirmationResponse
    {
        /// <summary>
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CallLimitStatus Status { get; set; }
    }
}
