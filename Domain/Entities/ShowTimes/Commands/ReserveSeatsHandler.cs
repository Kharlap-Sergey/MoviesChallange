using Domain.Abstractions;
using MediatR;

namespace Domain.Entities.ShowTimes.Commands;
public class ReserveSeatsHandler : IRequestHandler<ReserveSeatsCommand, ReserveSeatsCommandData>
{
    private readonly IShowTimeRepository _showTimeRepository;
    private readonly IAuditoriumRepository _auditoriumRepository;
    private readonly IMoviesProvider _moviesProvider;

    public ReserveSeatsHandler(
        IShowTimeRepository showTimeRepository,
        IAuditoriumRepository auditoriumRepository,
        IMoviesProvider moviesProvider)
    {
        _showTimeRepository = showTimeRepository;
        _auditoriumRepository = auditoriumRepository;
        _moviesProvider = moviesProvider;
    }

    public async Task<ReserveSeatsCommandData> Handle(
        ReserveSeatsCommand request,
        CancellationToken cancellationToken)
    {
        var showTime = await _showTimeRepository.GetShowTimeByIdAsync(
            request.ShowTimeId,
            cancellationToken
            );

        var auditorium = await _auditoriumRepository
            .GetById(
                showTime.AuditoriumId,
                cancellationToken);

        var reservation = showTime.Reserve(
            request.Seats,
            auditorium!
            );

        var movie = await _moviesProvider.GetById(showTime.MovieId, cancellationToken);
        await _showTimeRepository.UpdateShowTimeAsync(showTime, cancellationToken);
        
        return new ReserveSeatsCommandData(
            reservation,
            request.Seats,
            movie,
            showTime.AuditoriumId
            );
    }
}

