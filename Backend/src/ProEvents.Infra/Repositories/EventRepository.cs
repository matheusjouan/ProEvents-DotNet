using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Model;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;
using ProEvents.Infra.Pagination;

namespace ProEvents.Infra.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly AppDbContext _context;
        public EventRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PageList<Event>> GetAllEventsAsync(int userId, PageParams pageParams, bool includeSpeaker = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Batches)
                .Include(e => e.SocialNetworks);

            if (includeSpeaker)
            {
                query = query.Include(e => e.EventsSpeakers)
                    .ThenInclude(es => es.Speaker);
            }

            query = query.AsNoTracking()
                .Where(u => u.UserId == userId)
                .Where(e => e.Thema.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderBy(e => e.Id);

            return await PageList<Event>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        public async Task<Event> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(e => e.Batches)
                .Include(e => e.SocialNetworks);

            if (includeSpeaker)
            {
                query = query.Include(e => e.EventsSpeakers)
                    .ThenInclude(ep => ep.Speaker);
            }

            query = query.OrderBy(e => e.Id)
                .Where(evt => evt.Id == eventId)
                .Where(u => u.UserId == userId);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}