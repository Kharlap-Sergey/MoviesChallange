using Domain.Exceptions;

namespace Infrastructure.MoviesProvider
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
