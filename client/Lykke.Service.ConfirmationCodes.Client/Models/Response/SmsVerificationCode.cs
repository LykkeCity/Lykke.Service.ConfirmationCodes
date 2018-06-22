using System;
using Newtonsoft.Json;

namespace Lykke.Service.ConfirmationCodes.Client.Models.Response
{
    /// <summary>
    /// </summary>
    public class SmsVerificationCode
    {
        /// <summary>
        /// </summary>
        public SmsVerificationCode(string code = default(string), DateTime? created = default(DateTime?), DateTime? expires = default(DateTime?))
        {
            Code = code;
            Created = created;
            Expires = expires;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Created")]
        public DateTime? Created { get; private set; }
      
        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Expires")]
        public DateTime? Expires { get; private set; }
    }
}
