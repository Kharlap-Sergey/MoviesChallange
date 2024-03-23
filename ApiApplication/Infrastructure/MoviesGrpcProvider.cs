using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApplication.Domain.Movies;
using ApiApplication.Domain.Movies.Abstractions;
using Grpc.Core;
using ProtoDefinitions;

namespace ApiApplication.Infrastructure;

public class MoviesGrpcProvider : IMoviesProvider
{
    private readonly MoviesApi.MoviesApiClient _moviesApiClient;

    public MoviesGrpcProvider(
        MoviesApi.MoviesApiClient moviesApiClient
        )
    {
        _moviesApiClient = moviesApiClient;
    }

    public async Task<MovieModel> GetById(string id)
    {
        var result =  await Execute(
                _moviesApiClient.GetByIdAsync(new IdRequest { Id = id }
            ));

        var data = result.Data.Unpack<showResponse>();
        return MapToDomainModel(data);
    }

    public async Task<List<MovieModel>> GetWithFilter(string searchFilter)
    {
        var result =  await Execute(
                _moviesApiClient.SearchAsync(new SearchRequest { Text = searchFilter }
            ));

        var data = result.Data.Unpack<showListResponse>();

        return data.Shows.Select(MapToDomainModel).ToList();
    }

    public async Task<List<MovieModel>> GetAll()
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
    private MovieModel MapToDomainModel(showResponse show)
    {
        return new MovieModel
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
