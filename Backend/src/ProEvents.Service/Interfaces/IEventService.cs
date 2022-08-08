using Microsoft.AspNetCore.Http;
using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;


namespace ProEvents.Service.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> AddEvent(int userId, EventDTO model);
        Task<EventDTO> UpdateEvent(int userId, int eventId, EventDTO model);
        Task<bool> DeleteEvent(int userId, int eventId);
        Task<PageList<EventDTO>> GetAllEventsAsync(int userId, PageParams pageParams, bool includeSpeaker = false);
        Task<EventDTO> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false);
        Task<string> SaveImage(IFormFile fileName, string path);
        void DeleteImage(string imageName, string path);
    }
}