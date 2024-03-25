using Domain.Entities;

namespace Domain.Abstractions;

public interface IMoviesProvider
{
    public Task<List<MovieEntity>> GetAll();

    public Task<MovieEntity> GetById(string id, CancellationToken cancellationToken);

    public Task<List<MovieEntity>> GetWithFilter(string searchFilter);
}
