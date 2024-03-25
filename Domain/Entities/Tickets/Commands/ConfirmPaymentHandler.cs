using Domain.Abstractions;
using MediatR;

namespace Domain.Entities.Tickets.Commands;

public class ConfirmPaymentHandler
    : IRequestHandler<ConfirmPaymentCommand>
{
    private readonly ITicketRepository _ticketRepository;

    public ConfirmPaymentHandler(
        ITicketRepository ticketRepository
        )
    {
        _ticketRepository = ticketRepository;
    }

    public async Task Handle(
        ConfirmPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var ticket 
            = await _ticketRepository.GetById(request.TicketId, cancellationToken);

        if(ticket == null)
        {
            //some logic here
            return;
        }

        ticket.ConfirmPayment(request.PaymentId);

        await _ticketRepository.Update(ticket, cancellationToken);
    }
}
