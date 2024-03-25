using MediatR;

namespace Domain.Entities.Auditorium.Commands;

public record ArrangeNewShowCommand(
    string MovieExternalId,
    DateTime SessionDate,
    int AuditoriumId
    ) : IRequest
{

}
