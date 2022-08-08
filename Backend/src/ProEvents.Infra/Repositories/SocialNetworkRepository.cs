using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Model;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;

namespace ProEvents.Infra.Repositories
{
    public class SocialNetworkRepository : BaseRepository<SocialNetwork>, ISocialNetworkRepository
    {

        private readonly AppDbContext _context;
        public SocialNetworkRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SocialNetwork>> GetAllByEventIdAsync(int eventId)
        {
            IQueryable<SocialNetwork> query = _context.SocialNetworks;

            query = query.AsNoTracking()
                .Where(sn => sn.EventId == eventId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<SocialNetwork>> GetAllBySpeakerIdAsync(int speakerId)
        {
            IQueryable<SocialNetwork> query = _context.SocialNetworks;

            query = query.AsNoTracking()
                .Where(sn => sn.SpeakerId == speakerId);

            return await query.ToListAsync();
        }

        public async Task<SocialNetwork> GetSocialNetworkEventByIdAsync(int eventId, int id)
        {
            IQueryable<SocialNetwork> query = _context.SocialNetworks;

            query = query.AsNoTracking()
                .Where(sn => sn.EventId == eventId &&
                       sn.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<SocialNetwork> GetSocialNetworkSpeakerByIdAsync(int speakerId, int id)
        {
            IQueryable<SocialNetwork> query = _context.SocialNetworks;

            query = query.AsNoTracking()
                .Where(sn => sn.SpeakerId == speakerId &&
                       sn.Id == id);

            return await query.FirstOrDefaultAsync();
        }
    }
}