using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaNoSetupInProgressException : Exception
    {
        public string ClientId { get; }

        public Google2FaNoSetupInProgressException(string clinetId, string message)
            : base(message)
        {
            ClientId = clinetId;
        }

        public Google2FaNoSetupInProgressException(string message)
            : base(message)
        {
        }

        public Google2FaNoSetupInProgressException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
