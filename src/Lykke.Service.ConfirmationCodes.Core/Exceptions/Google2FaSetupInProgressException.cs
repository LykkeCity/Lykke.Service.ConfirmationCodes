using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaSetupInProgressException : Exception
    {
        public string ClientId { get; }

        public Google2FaSetupInProgressException(string clientId, string message)
            : base(message)
        {
            ClientId = clientId;
        }

        public Google2FaSetupInProgressException(string message)
            : base(message)
        {
        }

        public Google2FaSetupInProgressException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
