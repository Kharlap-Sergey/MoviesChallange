using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Infrastructure.MoviesProvider;

public class MoviesProviderCacheDecorator : IMoviesProvider
{
    private readonly IMoviesProvider _moviesProvider;
    private readonly IDistributedCache _cache;
    private readonly IOptions<MoviesProviderCacheDurationOptions> _options;

    public MoviesProviderCacheDecorator(
        IMoviesProvider moviesProvider,
        IDistributedCache cache,
        IOptions<MoviesProviderCacheDurationOptions> options
        )
    {
        _moviesProvider = moviesProvider;
        _cache = cache;
        _options = options;
    }
    public Task<List<MovieEntity>> GetAll()
    {
        string key = $"{typeof(MoviesProviderCacheDecorator).FullName}.{nameof(GetAll)}";

        return _cache.GetOrSet(
            key,
            async () => await _moviesProvider.GetAll(),
            _options.Value.GetAll);
    }

    public Task<MovieEntity> GetById(string id, CancellationToken cancellationToken)
    {
        string key = $"{typeof(MoviesProviderCacheDecorator).FullName}.{nameof(GetById)}_{id}";

        return _cache.GetOrSet(
            key,
            async () => await _moviesProvider.GetById(id, cancellationToken),
            _options.Value.GetById);
    }

    public Task<List<MovieEntity>> GetWithFilter(string searchFilter)
    {
        string key = $"{typeof(MoviesProviderCacheDecorator).FullName}.{nameof(GetWithFilter)}_{searchFilter}";

        return _cache.GetOrSet(
            key,
            async () => await _moviesProvider.GetWithFilter(searchFilter),
            _options.Value.GetWithFilter);
    }
}
