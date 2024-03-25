using Domain.Abstractions;
using MediatR;

namespace Domain.Entities.ShowTimes.Queries;

public class GetShowTimesHandler : IRequestHandler<GetShowTimesQuery, IEnumerable<ShowTimeEntity>>
{
    private readonly IShowTimeRepository _showTimeRepository;

    public GetShowTimesHandler(
        IShowTimeRepository showTimeRepository
        )
    {
        _showTimeRepository = showTimeRepository;
    }

    public Task<IEnumerable<ShowTimeEntity>> Handle(GetShowTimesQuery request, CancellationToken cancellationToken)
    {
        return _showTimeRepository.GetShowTimesAsync(
            request.From,
            request.To,
            request.AuditoriumId,
            cancellationToken
            );
    }
}
