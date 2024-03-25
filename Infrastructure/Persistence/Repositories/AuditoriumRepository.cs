using Domain.Abstractions;
using Domain.Entities.Auditorium;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AuditoriumRepository
    : BaseRepository, IAuditoriumRepository
{
    private readonly AppDbContext context;

    public AuditoriumRepository(
        IMediator mediator,
        AppDbContext context
        )
        : base(mediator)
    {
        this.context = context;
    }

    public Task<AuditoriumEntity?> GetById(int id, CancellationToken token)
    {
        return context.Auditoriums
                .Include(x => x.Seats)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public async Task Update(AuditoriumEntity auditorium, CancellationToken token)
    {
        context.Auditoriums.Update(auditorium);
        await context.SaveChangesAsync(token);

        await DispatchDomainEventsAsync(auditorium, token);
    }
}
