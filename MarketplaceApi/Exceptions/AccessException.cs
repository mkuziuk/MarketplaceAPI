using System;

namespace MarketplaceApi.Exceptions
{
    public class AccessException : Exception
    {
        public AccessException()
        { }

        public AccessException(string message)
            : base(message)
        { }

        public AccessException(string message, Exception inner) 
            : base(message, inner)
        { }
    }
}