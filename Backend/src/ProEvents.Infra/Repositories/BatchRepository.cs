using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Model;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;

namespace ProEvents.Infra.Repositories
{
    public class BatchRepository : BaseRepository<Batch>, IBatchRepository
    {

        private readonly AppDbContext _context;
        public BatchRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Batch>> GetBactchesByEventIdAsync(int eventId)
        {
            IQueryable<Batch> query = _context.Batches;

            query = query.AsNoTracking()
                .Where(x => x.EventId == eventId);

            return await query.ToListAsync();
        }

        public async Task<Batch> GetBatchByIdsAsync(int eventId, int batchId)
        {
            IQueryable<Batch> query = _context.Batches;

            query = query.AsNoTracking()
                .Where(x => x.EventId == eventId && x.Id == batchId);

            return await query.FirstOrDefaultAsync();

        }
    }
}