using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.Tickets.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(
        IMediator mediator
        )
    {
        _mediator = mediator;
    }

    [HttpPost("{reservationId}/confirm")]
    public async Task<IActionResult> Confirm(
        Guid reservationId,
        [FromBody] Guid paymentId
        )
    {
        await _mediator.Send(
            new ConfirmPaymentCommand(
                reservationId,
                paymentId));

        return Ok();
    }
}
