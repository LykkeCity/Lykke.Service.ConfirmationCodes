using System;

namespace Lykke.Service.ConfirmationCodes.Services
{
    public interface ISupportToolsSettings
    {
        int PriorityCodeExpirationInterval { get; set; }
        TimeSpan RepeatCallInverval { get; set; }
        int CallsLimit { get; set; }
    }
}
