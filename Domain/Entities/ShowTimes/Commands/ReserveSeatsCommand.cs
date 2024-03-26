using Domain.ValueObjects;
using MediatR;

namespace Domain.Entities.ShowTimes.Commands;

public record ReserveSeatsCommand(
    Guid ShowTimeId,
    IEnumerable<Seat> Seats) 
    : IRequest<ReserveSeatsCommandData>;


public record ReserveSeatsCommandData(
    Guid reservationId,
    IEnumerable<Seat> seats,
    MovieEntity Movie,
    int AuditoriumId
    );
