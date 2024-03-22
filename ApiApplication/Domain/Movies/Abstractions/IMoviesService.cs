using System.Threading.Tasks;

namespace ApiApplication.Domain.Movies.Abstractions;

public interface IMoviesService
{
    public Task<object> GetAll();
}
