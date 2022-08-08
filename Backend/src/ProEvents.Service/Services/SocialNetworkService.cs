using AutoMapper;
using ProEvents.Domain.Model;
using ProEvents.Infra.Interface;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services
{
    public class SocialNetworkService : ISocialNetworkService
    {
        private readonly ISocialNetworkRepository _socialNetworkRepository;
        private readonly IMapper _mapper;

        public SocialNetworkService(ISocialNetworkRepository socialNetworkRepository, IMapper mapper)
        {
            _socialNetworkRepository = socialNetworkRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteByEvent(int eventId, int socialNetworkId)
        {
            try
            {
                var socialNetwork = await _socialNetworkRepository.GetSocialNetworkEventByIdAsync(eventId, socialNetworkId);
                if (socialNetwork == null)
                    throw new Exception("SocialNetwork of EventId does not exist");

                await _socialNetworkRepository.Delete(socialNetwork);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteBySpeaker(int speakerId, int socialNetworkId)
        {
            try
            {
                var socialNetwork = await _socialNetworkRepository.GetSocialNetworkSpeakerByIdAsync(speakerId, socialNetworkId);
                if (socialNetwork == null)
                    throw new Exception("SocialNetwork of SpeakerId does not exist");

                await _socialNetworkRepository.Delete(socialNetwork);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<SocialNetworkDTO>> GetAllByEventIdAsync(int eventId)
        {
            try
            {
                var socialNetworks = await _socialNetworkRepository.GetAllByEventIdAsync(eventId);
                if (socialNetworks == null)
                    return null;

                return _mapper.Map<IEnumerable<SocialNetworkDTO>>(socialNetworks);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<SocialNetworkDTO>> GetAllBySpeakerIdAsync(int speakerId)
        {
            try
            {
                var socialNetworks = await _socialNetworkRepository.GetAllBySpeakerIdAsync(speakerId);
                if (socialNetworks == null)
                    return null;

                return _mapper.Map<IEnumerable<SocialNetworkDTO>>(socialNetworks);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<SocialNetworkDTO> GetSocialNetworkEventByIdsAsync(int eventId, int socialNetworkId)
        {
            try
            {
                var socialNetwork = await _socialNetworkRepository.GetSocialNetworkEventByIdAsync(eventId, socialNetworkId);
                if (socialNetwork == null)
                    return null;

                return _mapper.Map<SocialNetworkDTO>(socialNetwork);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<SocialNetworkDTO> GetSocialNetworkSpeakerByIdsAsync(int speakerId, int socialNetworkId)
        {
            try
            {
                var socialNetwork = await _socialNetworkRepository.GetSocialNetworkSpeakerByIdAsync(speakerId, socialNetworkId);
                if (socialNetwork == null)
                    return null;

                return _mapper.Map<SocialNetworkDTO>(socialNetwork);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<SocialNetworkDTO>> SaveByEvent(int eventId, SocialNetworkDTO[] models)
        {
            try
            {
                var socialNetworks = await _socialNetworkRepository.GetAllByEventIdAsync(eventId);
                if (socialNetworks == null)
                    return null;

                foreach (var model in models)
                {
                    // Se não possuir o SN, será criado
                    if (model.Id == 0)
                        await AddSocialNetWork(eventId, model, true);


                    // Se possuir o SN será alterado
                    else
                    {
                        var socialNetwork = socialNetworks.FirstOrDefault(x => x.Id == model.Id);
                        model.EventId = eventId;

                        _mapper.Map(model, socialNetwork);
                        await _socialNetworkRepository.Update(socialNetwork);
                    }
                }

                var socialNetworksUpdates = await _socialNetworkRepository.GetAllByEventIdAsync(eventId);
                return _mapper.Map<IEnumerable<SocialNetworkDTO>>(socialNetworksUpdates);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<SocialNetworkDTO>> SaveBySpeaker(int speakerId, SocialNetworkDTO[] models)
        {
            try
            {
                var socialNetworks = await _socialNetworkRepository.GetAllBySpeakerIdAsync(speakerId);
                if (socialNetworks == null)
                    return null;

                foreach (var model in models)
                {
                    // Se não possuir o SN, será criado
                    if (model.Id == 0)
                        await AddSocialNetWork(speakerId, model, false);

                    // Se possuir o SN será alterado
                    else
                    {
                        var socialNetwork = socialNetworks.FirstOrDefault(x => x.Id == model.Id);
                        model.EventId = speakerId;

                        _mapper.Map(model, socialNetwork);
                        await _socialNetworkRepository.Update(socialNetwork);
                    }
                }

                var socialNetworksUpdates = await _socialNetworkRepository.GetAllBySpeakerIdAsync(speakerId);
                return _mapper.Map<IEnumerable<SocialNetworkDTO>>(socialNetworksUpdates);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task AddSocialNetWork(int id, SocialNetworkDTO model, bool isEvent)
        {
            try
            {
                var socialNetwork = _mapper.Map<SocialNetwork>(model);

                if (isEvent)
                {
                    socialNetwork.EventId = id;
                    socialNetwork.SpeakerId = null;
                }
                else
                {
                    socialNetwork.SpeakerId = id;
                    socialNetwork.EventId = null;
                }

                await _socialNetworkRepository.Add(socialNetwork);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}