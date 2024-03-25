using Domain.Abstractions;
using Domain.Entities;
using Grpc.Core;
using ProtoDefinitions;

namespace Infrastructure.MoviesProvider;

public class MoviesGrpcProvider : IMoviesProvider
{
    private readonly MoviesApi.MoviesApiClient _moviesApiClient;

    public MoviesGrpcProvider(
        MoviesApi.MoviesApiClient moviesApiClient
        )
    {
        _moviesApiClient = moviesApiClient;
    }

    public async Task<MovieEntity> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await Execute(
                _moviesApiClient.GetByIdAsync(
                    new IdRequest { Id = id },
                    cancellationToken: cancellationToken)
                );

        var data = result.Data.Unpack<showResponse>();
        return MapToDomainModel(data);
    }

    public async Task<List<MovieEntity>> GetWithFilter(string searchFilter)
    {
        var result = await Execute(
                _moviesApiClient.SearchAsync(new SearchRequest { Text = searchFilter }
            ));

        var data = result.Data.Unpack<showListResponse>();

        return data.Shows.Select(MapToDomainModel).ToList();
    }

    public async Task<List<MovieEntity>> GetAll()
    {
        var result = await Execute(
                _moviesApiClient.GetAllAsync(new Empty())
            );

        var data = result.Data.Unpack<showListResponse>();

        return data.Shows.Select(MapToDomainModel).ToList();
    }

    private async Task<responseModel> Execute(AsyncUnaryCall<responseModel> call)
    {
        var result = await call;

        if (!result.Success)
        {
            var exceptions = result.Exceptions
                .Select(e => (object)new
                {
                    e.Message,
                    e.StatusCode
                })
                .ToList();

            throw new MoviesGrpcProviderException(exceptions);
        }

        return result;
    }
    private MovieEntity MapToDomainModel(showResponse show)
    {
        return new MovieEntity
        {
            Id = show.Id,
            Rank = show.Rank,
            Title = show.Title,
            FullTitle = show.FullTitle,
            Year = show.Year,
            Image = show.Image,
            Crew = show.Crew,
            ImDbRating = show.ImDbRating,
            ImDbRatingCount = show.ImDbRatingCount
        };
    }
}
