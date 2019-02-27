using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaAlreadySetException : Exception
    {
        public string ClientId { get; }

        public Google2FaAlreadySetException(string clientId, string message)
            : base(message)
        {
            ClientId = clientId;
        }

        public Google2FaAlreadySetException(string message)
            : base(message)
        {
        }

        public Google2FaAlreadySetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
