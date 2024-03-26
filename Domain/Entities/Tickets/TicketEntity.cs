using Domain.Core;
using Domain.Entities.ShowTimes;
using Domain.Events;
using Domain.Exceptions;
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

    public TicketEntity(
        Guid? id = null,
        IEnumerable<Seat>? seats = null,
        ShowTimeEntity? showtime = null,
        TicketStatus? status = null,
        DateTime? createdTime = null,
        Guid? paymentId = null
        )
    {
        if(id.HasValue)
            Id = id.Value;
        if(seats != null)
            Seats = seats;
        if(showtime != null)
            Showtime = showtime;
        if(status != null)
            Status = status;
        if(createdTime.HasValue)
            CreatedTime = createdTime.Value;
        if(paymentId.HasValue)
            PaymentId = paymentId.Value;
    }

    public TicketEntity(IEnumerable<Seat> seats, ShowTimeEntity showtimeEntity)
    {
        Id = Guid.NewGuid();
        Status = TicketStatus.Pending;
        CreatedTime = DateTime.UtcNow;
        Seats = seats;
        Showtime = showtimeEntity;
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
            throw new InvalidOperationDomainException("Only Pending tickets could be expired");
        }

        if (!IsExpired())
            return;

        Status = TicketStatus.Expired;
        AddDomainEvent(new TicketExpiredDomainEvent(Id, Seats));
    }

    public void ConfirmPayment(Guid paymentId)
    {
        if (Status != TicketStatus.Pending)
        {
            throw new InvalidOperationDomainException("Only Pending tickets could be paid");
        }

        PaymentId = paymentId;
        Status = TicketStatus.Paid;
        AddDomainEvent(new TicketPaymentConfirmedDomainEvent(this, paymentId));
    }
}
