using Domain.Core;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities.Auditorium;

public class AuditoriumEntity : Entity<int>
{
    private readonly List<Seat> _seats;
    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

    public AuditoriumEntity()
    {
        
    }
    public AuditoriumEntity(int id, List<Seat> seats)
    {
        Id = id;
        _seats = seats;
    }

    public AuditoriumEntity(
        List<Seat> seats
        )
    {
        _seats = seats;
    }

    public void ArrangeNewShow(
        DateTime start,
        MovieEntity movie
        )
    {
        //todo: check if auditorium is available

        if (start < DateTime.UtcNow)
        {
            throw new InvalidOperationDomainException("Start time must be in the future");
        }

        AddDomainEvent(
            new NewAuditoriumReservationDomainEvent<MovieEntity>(Id, start, movie)
            );
    }

    public bool IsSeatsContiguous(IEnumerable<Seat> seats)
    {
        var sortedSeats = seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber).ToList();
        for (var i = 1; i < sortedSeats.Count; i++)
        {
            if (sortedSeats[i].Row != sortedSeats[i - 1].Row)
            {
                return false;
            }

            if (sortedSeats[i].SeatNumber != sortedSeats[i - 1].SeatNumber + 1)
            {
                return false;
            }
        }

        return true;
    }
}
