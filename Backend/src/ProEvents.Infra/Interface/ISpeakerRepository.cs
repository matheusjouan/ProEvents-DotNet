using ProEvents.Domain.Model;
using ProEvents.Infra.Pagination;

namespace ProEvents.Infra.Interface
{
    public interface ISpeakerRepository : IBaseRepository<Speaker>
    {
        Task<PageList<Speaker>> GetAllSpeakersAsync(PageParams pageParams, bool includeEvents = false);
        Task<Speaker> GetSpeakerByUserIdAsync(int userId, bool includeEvents = false);
    }
}