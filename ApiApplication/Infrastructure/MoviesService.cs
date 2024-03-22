using System.Threading.Tasks;
using ApiApplication.Domain.Movies.Abstractions;
using ProtoDefinitions;

namespace ApiApplication.Infrastructure;

public class MoviesService : IMoviesService
{
    private readonly MoviesApi.MoviesApiClient _moviesApiClient;

    public MoviesService(
        MoviesApi.MoviesApiClient moviesApiClient
        )
    {
        _moviesApiClient = moviesApiClient;
    }

    public async Task<object> GetAll()
    {
        return await _moviesApiClient.GetAllAsync(new Empty());
    }
}
