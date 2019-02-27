using System;
using Lykke.Service.ConfirmationCodes.Services;

namespace Lykke.Service.ConfirmationCodes.Settings
{
    public class SupportToolsSettings : ISupportToolsSettings
    {
        public int PriorityCodeExpirationInterval { get; set; }
        public TimeSpan RepeatCallInverval { get; set; }
        public int CallsLimit { get; set; }
    }
}
