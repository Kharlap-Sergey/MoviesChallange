using MediatR;

namespace Domain.Entities.ShowTimes.Queries;

public record GetShowTimesQuery(
    DateTime? From,
    DateTime? To,
    int? AuditoriumId) : IRequest<IEnumerable<ShowTimeEntity>>;