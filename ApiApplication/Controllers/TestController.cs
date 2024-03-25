using System;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Entities.Auditorium.Commands;
using Domain.Entities.ShowTimes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IMoviesProvider _moviesService;
    private readonly IMediator _mediator;

    public TestController(
        IMoviesProvider moviesService, 
        IMediator mediator
        )
    {
        _moviesService = moviesService;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<object> Get()
    {
        return await _moviesService.GetAll();
    }

    [HttpGet]
    public async Task<object> Test()
    {
        var auditoriumId = 1;

        var showTimeCommand = new ArrangeNewShowCommand(
                        "ID-test1",
                        DateTime.UtcNow.AddDays(2),
                        auditoriumId
                        );

        await _mediator.Send(showTimeCommand);

        var shows = await _mediator.Send(new GetShowTimesQuery(
            null,
            null,
            null
            ));

        return shows;
    }
}
