using Domain.Abstractions;
using Domain.Entities.ShowTimes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ShowTimeRepository : BaseRepository, IShowTimeRepository
{
    private readonly AppDbContext _context;

    public ShowTimeRepository(
        IMediator mediator,
        AppDbContext context
        )
        : base(mediator)
    {
        _context = context;
    }

    public async Task AddShowTimeAsync(
        ShowTimeEntity ShowTimeEntity,
        CancellationToken cancellationToken
        )
    {
        _context.Showtimes.Add(ShowTimeEntity);

        await _context.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(ShowTimeEntity, cancellationToken);
    }

    public async Task<ShowTimeEntity> GetShowTimeByIdAsync(Guid showTimeId, CancellationToken cancellationToken)
    {
        return await _context.Showtimes
            .Include(st => st.Tickets)
            .FirstOrDefaultAsync(st => st.Id == showTimeId);
    }

    public async Task<IEnumerable<ShowTimeEntity>> GetShowTimesAsync(
        DateTime? from = null,
        DateTime? to = null,
        int? auditoriumId = null,
        CancellationToken cancellationToken = default
        )
    {
        var query = _context.Showtimes.AsQueryable();
        if (from.HasValue)
        {
            query = query.Where(x => x.SessionDate >= from);
        }
        if (to.HasValue)
        {
            query = query.Where(x => x.SessionDate <= to);
        }
        if (auditoriumId.HasValue)
        {
            query = query.Where(x => x.AuditoriumId == auditoriumId);
        }

        var shows = await query.AsNoTracking().ToListAsync(cancellationToken);

        return shows;
    }

    public async Task UpdateShowTimeAsync(ShowTimeEntity ShowTimeEntity, CancellationToken cancellationToken)
    {
        _context.Showtimes.Update(ShowTimeEntity);

        await _context.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(ShowTimeEntity, cancellationToken);
    }
}
