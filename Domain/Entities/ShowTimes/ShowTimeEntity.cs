using Domain.Core;
using Domain.Entities.Auditorium;
using Domain.Entities.Tickets;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities.ShowTimes;

public class ShowTimeEntity : Entity<Guid>
{
    public DateTime SessionDate { get; private set; }

    public string MovieId { get; private set; }

    public int AuditoriumId { get; private set; }

    private readonly List<TicketEntity> _tickets = new();

    public IReadOnlyCollection<TicketEntity> Tickets => _tickets;
    
    public ShowTimeEntity()
    {
        
    }

    public ShowTimeEntity(
        DateTime date,
        string movieId,
        int auditoriumId,
        Guid? id = null
        )
    {
        SessionDate = date;
        MovieId = movieId;
        AuditoriumId = auditoriumId;
        Id = id ?? Guid.NewGuid();
    }


    public Guid Reserve(
        IEnumerable<Seat> seats,
        AuditoriumEntity auditorium
        )
    {
        //ensure all seats are contiguous
        if (!auditorium.IsSeatsContiguous(seats))
        {
            throw new InvalidOperationDomainException("Seats are not contiguous");
        };

        var unavailableSeats = new HashSet<Seat>(
                _tickets
                    .Where(ticket => !ticket.IsExpired())
                    .SelectMany(ticket => ticket.Seats)
                );

        //ensure all seats are available
        foreach (var seat in seats)
        {
            if (unavailableSeats.Contains(seat))
            {
                throw new InvalidOperationDomainException("Seat is not available");
            }
        }

        var reservation = new TicketEntity(seats, this);

        _tickets.Add(reservation);

        AddDomainEvent(
            new TicketReservedDomainEvent(
                reservation,
                TicketEntity.DefaultExpiresIn)
            );
        
        return reservation.Id;
    }
}
