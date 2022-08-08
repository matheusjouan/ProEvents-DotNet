using ProEvents.Domain.Model;
using ProEvents.Infra.Pagination;

namespace ProEvents.Infra.Interface
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<PageList<Event>> GetAllEventsAsync(int userId, PageParams pageParams, bool includeSpeaker = false);
        Task<Event> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false);
    }
}