using MediatR;

namespace Domain.Entities.Tickets.Commands;

public record PaymentConfirmedCommand(
    Guid TicketId,
    Guid PaymentId
    )

    : IRequest;
