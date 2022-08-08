using ProEvents.Domain.Model;

namespace ProEvents.Infra.Interface
{
    public interface IBatchRepository : IBaseRepository<Batch>
    {
        Task<IEnumerable<Batch>> GetBactchesByEventIdAsync(int eventId);
        Task<Batch> GetBatchByIdsAsync(int eventId, int batchId);
    }
}