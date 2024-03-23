namespace Domain.Movies.Abstractions;

public interface IMoviesProvider
{
    public Task<List<MovieModel>> GetAll();

    public Task<MovieModel> GetById(string id);

    public Task<List<MovieModel>> GetWithFilter(string searchFilter);
}
