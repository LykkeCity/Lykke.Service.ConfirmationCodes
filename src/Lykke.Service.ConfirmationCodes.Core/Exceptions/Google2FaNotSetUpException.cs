using System;

namespace Lykke.Service.ConfirmationCodes.Core.Exceptions
{
    public class Google2FaNotSetUpException : Exception
    {
        public string ClientId { get; }
        
        public Google2FaNotSetUpException(string clientId, string message)
            : base(message)
        {
            ClientId = clientId;
        }

        public Google2FaNotSetUpException(string message)
            : base(message)
        {
        }

        public Google2FaNotSetUpException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
