using MediatR;

namespace Domain.Entities.Tickets.Commands;

public record ConfirmPaymentCommand(
    Guid TicketId,
    Guid PaymentId
    )

    : IRequest;
