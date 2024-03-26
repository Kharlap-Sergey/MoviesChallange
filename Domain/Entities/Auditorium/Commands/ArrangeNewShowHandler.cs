using Domain.Abstractions;
using MediatR;

namespace Domain.Entities.Auditorium.Commands;

public class ArrangeNewShowHandler
    : IRequestHandler<ArrangeNewShowCommand>
{
    private readonly IAuditoriumRepository _auditoriumRepository;
    private readonly IMoviesProvider _moviesProvider;

    public ArrangeNewShowHandler(
        IAuditoriumRepository auditoriumRepository,
        IMoviesProvider moviesProvider
        )
    {
        _auditoriumRepository = auditoriumRepository;
        _moviesProvider = moviesProvider;
    }

    public async Task Handle(
        ArrangeNewShowCommand request,
        CancellationToken cancellationToken)
    {

        var auditorium
            = await _auditoriumRepository.GetById(request.AuditoriumId, cancellationToken);

        var movie = await _moviesProvider.GetById(request.MovieExternalId, cancellationToken);

        auditorium.ArrangeNewShow(
            request.SessionDate,
            movie
            );

        await _auditoriumRepository.Update(auditorium, cancellationToken);

        return;
    }
}
