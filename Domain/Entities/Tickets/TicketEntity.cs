using Domain.Core;
using Domain.Entities.ShowTimes;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities.Tickets;

public class TicketEntity : Entity<Guid>
{
    public static TimeSpan DefaultExpiresIn = TimeSpan.FromMinutes(10);

    public TicketStatus Status { get; private set; }

    public ShowTimeEntity Showtime { get; private set; }

    public IEnumerable<Seat> Seats { get; private set; }

    public DateTime CreatedTime { get; private set; }

    public Guid? PaymentId { get; private set; }

    public TicketEntity()
    {

    }

    public TicketEntity(IEnumerable<Seat> seats, ShowTimeEntity showtimeEntity)
    {
        Id = Guid.NewGuid();
        Status = TicketStatus.Pending;
        CreatedTime = DateTime.UtcNow;
        Seats = seats;
        Showtime = showtimeEntity;
    }

    public bool IsValid()
    {
        if (Status != TicketStatus.Pending && Status != TicketStatus.Paid)
        {
            return false;
        }

        return !IsExpired();
    }

    public bool IsExpired()
    {
        if (Status == TicketStatus.Pending)
        {
            return CreatedTime.Add(DefaultExpiresIn) < DateTime.UtcNow;
        }

        return Status == TicketStatus.Expired;
    }

    public void Expire()
    {
        if (Status != TicketStatus.Pending)
        {
            throw new InvalidOperationException("Only Pending tickets could be expired");
        }

        Status = TicketStatus.Expired;

    }

    public void ConfirmPayment(Guid paymentId)
    {
        if (Status != TicketStatus.Pending)
        {
            throw new InvalidOperationException("Only Pending tickets could be paid");
        }

        PaymentId = paymentId;
        Status = TicketStatus.Paid;
        AddDomainEvent(new TicketPaymentConfirmedDomainEvent(this, paymentId));
    }
}
