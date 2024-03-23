using System.Threading.Tasks;
using Domain.Movies.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMoviesProvider _moviesService;

    public TestController(
        IMoviesProvider moviesService
        )
    {
        _moviesService = moviesService;
    }

    [HttpGet]
    public async Task<object> Get()
    {
        return await _moviesService.GetAll();
    }
}
