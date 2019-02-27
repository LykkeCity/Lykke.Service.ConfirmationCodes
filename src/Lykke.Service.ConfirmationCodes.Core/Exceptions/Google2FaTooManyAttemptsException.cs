using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaTooManyAttemptsException : Exception
    {
        public string ClientId { get; }

        public Google2FaTooManyAttemptsException(string clientId, string message)
            : base(message)
        {
            ClientId = clientId;
        }

        public Google2FaTooManyAttemptsException(string message)
            : base(message)
        {
        }

        public Google2FaTooManyAttemptsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
