using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Model;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;
using ProEvents.Infra.Pagination;

namespace ProEvents.Infra.Repositories
{
    public class SpeakerRepository : BaseRepository<Speaker>, ISpeakerRepository
    {
        private readonly AppDbContext _context;
        public SpeakerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PageList<Speaker>> GetAllSpeakersAsync(PageParams pageParams, bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(u => u.User)
                .Include(s => s.SocialNetworks);

            if (includeEvents)
            {
                query = query.Include(s => s.EventsSpeakers)
                    .ThenInclude(es => es.EventId);
            }

            query = query
                .Where(s => s.Resume.ToLower().Contains(pageParams.Term.ToLower()) ||
                       s.User.FirstName.ToLower().Contains(pageParams.Term.ToLower()) ||
                       s.User.LastName.ToLower().Contains(pageParams.Term.ToLower()))
                .Where(s => s.User.UserType == Domain.Enum.UserType.Speaker)
                .OrderBy(s => s.Id);

            return await PageList<Speaker>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
        }
        public async Task<Speaker> GetSpeakerByUserIdAsync(int userId, bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(s => s.User)
                .Include(s => s.SocialNetworks);

            if (includeEvents)
            {
                query = query.Include(s => s.EventsSpeakers)
                    .ThenInclude(es => es.EventId);
            }

            query = query.OrderBy(s => s.Id).Where(s => s.UserId == userId);
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}