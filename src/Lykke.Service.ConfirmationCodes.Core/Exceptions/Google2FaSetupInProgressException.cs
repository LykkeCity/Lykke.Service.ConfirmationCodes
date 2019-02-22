using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaSetupInProgressException : Exception
    {
        public string ClientId { get; }

        public Google2FaSetupInProgressException(string clinetId, string message)
            : base(message)
        {
            ClientId = clinetId;
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
