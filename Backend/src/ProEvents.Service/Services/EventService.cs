using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProEvents.Domain.Model;
using ProEvents.Infra.Interface;
using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventService(IEventRepository eventRepository,
        IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<PageList<EventDTO>> GetAllEventsAsync(int userId, PageParams pageParams, bool includeSpeaker = false)
        {
            try
            {
                var events = await _eventRepository.GetAllEventsAsync(userId, pageParams);

                var result = _mapper.Map<PageList<EventDTO>>(events);
                result.CurrentPage = events.CurrentPage;
                result.TotalPages = events.TotalPages;
                result.PageSize = events.PageSize;
                result.TotalCount = events.TotalCount;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDTO> GetEventByIdAsync(int userId, int eventId, bool includeSpeaker = false)
        {
            try
            {
                var evt = await _eventRepository.GetEventByIdAsync(userId, eventId);
                if (evt == null) return null;

                return _mapper.Map<EventDTO>(evt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDTO> AddEvent(int userId, EventDTO modelDto)
        {
            try
            {
                var model = _mapper.Map<Event>(modelDto);
                model.UserId = userId;
                await _eventRepository.Add(model);

                var eventCreated = await _eventRepository.GetEventByIdAsync(userId, model.Id);
                return _mapper.Map<EventDTO>(eventCreated);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDTO> UpdateEvent(int userId, int eventId, EventDTO modelDto)
        {
            try
            {
                var evt = await _eventRepository.GetEventByIdAsync(userId, eventId);
                if (evt == null)
                    return null;

                modelDto.Id = evt.Id;
                modelDto.UserId = userId;
                _mapper.Map(modelDto, evt);

                await _eventRepository.Update(evt);
                var eventUpdated = await _eventRepository.GetEventByIdAsync(userId, evt.Id);

                return _mapper.Map<EventDTO>(eventUpdated);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteEvent(int userId, int eventId)
        {
            try
            {
                var eventExists = await _eventRepository.GetEventByIdAsync(userId, eventId);
                if (eventExists == null)
                    throw new Exception("Event not found");

                await _eventRepository.Delete(eventExists);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteImage(string imageName, string path)
        {
            var imgPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @$"Resources/{path}",
                imageName
            );

            if (System.IO.File.Exists(imgPath))
                System.IO.File.Delete(imgPath);
        }

        public async Task<string> SaveImage(IFormFile imgFile, string path)
        {
            // Definindo o nome do Arquivo
            string imgName = new String(
                Path.GetFileNameWithoutExtension(imgFile.FileName)
                .Take(10) // pega os 10 primeiros caracteres
                .ToArray()
                ).Replace(" ", "-"); // Caso tiver espaço, substitui por "-"

            imgName = $"{imgName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imgFile.FileName)}";

            var imgPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                @$"Resources/{path}",
                imgName
            );

            using (var fileStream = new FileStream(imgPath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);
            }

            return imgName;
        }
    }
}