using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ConfirmationCodes.Client 
{
    /// <summary>
    /// Settings for configure service client
    /// </summary>
    public class ConfirmationCodesServiceClientSettings 
    {
        /// <summary>
        /// Service url
        /// </summary>
        [HttpCheck("/api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
