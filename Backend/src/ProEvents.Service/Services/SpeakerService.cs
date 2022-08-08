using AutoMapper;
using ProEvents.Domain.Model;
using ProEvents.Infra.Interface;
using ProEvents.Infra.Pagination;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services
{
    public class SpeakerService : ISpeakerService
    {
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;

        public SpeakerService(ISpeakerRepository speakerRepository, IMapper mapper)
        {
            _speakerRepository = speakerRepository;
            _mapper = mapper;
        }

        public async Task<SpeakerDTO> AddSpeaker(int userId, SpeakerAddDTO model)
        {
            try
            {
                var speaker = _mapper.Map<Speaker>(model);
                speaker.UserId = userId;

                await _speakerRepository.Add(speaker);

                var newSpeaker = await _speakerRepository.GetSpeakerByUserIdAsync(userId, false);
                return _mapper.Map<SpeakerDTO>(newSpeaker);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<SpeakerDTO>> GetAllSpeakersAsync(PageParams pageParams, bool includeEvent = false)
        {
            try
            {
                var speakers = await _speakerRepository.GetAllSpeakersAsync(pageParams);
                if (speakers == null)
                    return null;

                var result = _mapper.Map<PageList<SpeakerDTO>>(speakers);
                result.CurrentPage = speakers.CurrentPage;
                result.TotalPages = speakers.TotalPages;
                result.TotalCount = speakers.TotalCount;
                result.PageSize = speakers.PageSize;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SpeakerDTO> GetSpeakerByUserIdAsync(int userId, bool includeEvent = false)
        {
            try
            {
                var speaker = await _speakerRepository.GetSpeakerByUserIdAsync(userId);
                if (speaker == null)
                    return null;

                return _mapper.Map<SpeakerDTO>(speaker);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SpeakerDTO> UpdateSpeaker(int userId, SpeakerUpdateDTO model)
        {
            try
            {
                var speaker = await _speakerRepository.GetSpeakerByUserIdAsync(userId, false);
                if (speaker == null)
                    return null;

                model.Id = speaker.Id;
                model.UserId = userId;

                _mapper.Map(model, speaker);

                await _speakerRepository.Update(speaker);
                var speakerUpdated = await _speakerRepository.GetSpeakerByUserIdAsync(userId);

                return _mapper.Map<SpeakerDTO>(speakerUpdated);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}