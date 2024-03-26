using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions;
using Domain.Entities.Auditorium.Commands;
using Domain.Entities.ShowTimes.Commands;
using Domain.Entities.ShowTimes.Queries;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowTimesController : ControllerBase
{
    private readonly IMoviesProvider _moviesService;
    private readonly IMediator _mediator;

    public ShowTimesController(
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

    [HttpPost]
    public async Task<IActionResult> CreateShowTime(
        string movieId,
        DateTime dateTime,
        int auditoriumId
        )
    {
        var showTimeCommand = new ArrangeNewShowCommand(
            movieId.ToString(),
            dateTime,
            auditoriumId
            );

        await _mediator.Send(showTimeCommand);
        return Ok();
    }

    [HttpPatch("{showtimeId}/reserve")]
    public async Task<IActionResult> ReserveSeats(
        Guid showtimeId,
        [FromBody] IEnumerable<Seat> seats)
    {
        var reserveSeatsCommand = new ReserveSeatsCommand(
            showtimeId,
            seats);

        var result = await _mediator.Send(reserveSeatsCommand);
        
        return Ok(result);
    }
}
