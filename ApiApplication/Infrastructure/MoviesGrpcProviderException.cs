using System.Collections.Generic;
using ApiApplication.Domain.Exceptions;

namespace ApiApplication.Infrastructure
{
    public class MoviesGrpcProviderException : DomainException
    {
        public MoviesGrpcProviderException(List<object> exceptions)
        {
            Exceptions = exceptions;
        }

        public List<object> Exceptions { get; }
    }
}
