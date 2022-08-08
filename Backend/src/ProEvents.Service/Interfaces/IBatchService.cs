using ProEvents.Service.DTOs;

namespace ProEvents.Service.Interfaces
{
    public interface IBatchService
    {
        Task<IEnumerable<BatchDTO>> SaveBatches(int eventId, IEnumerable<BatchDTO> modelsDto);
        Task<bool> DeleteBatch(int eventId, int batchId);
        Task<IEnumerable<BatchDTO>> GetBatchesByEventIdAsync(int eventId);
        Task<BatchDTO> GetBatchByEventIdBatchIdAsync(int eventId, int batchId);

    }
}