using AutoMapper;
using ProEvents.Domain.Model;
using ProEvents.Infra.Interface;
using ProEvents.Service.DTOs;
using ProEvents.Service.Interfaces;

namespace ProEvents.Service.Services
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMapper _mapper;

        public BatchService(IBatchRepository batchRepository, IMapper mapper)
        {
            _batchRepository = batchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BatchDTO>> SaveBatches(int eventId, IEnumerable<BatchDTO> modelsDto)
        {
            try
            {
                // Busca todos os lotes a partir de um Evento
                var batches = await _batchRepository.GetBactchesByEventIdAsync(eventId);

                foreach (var item in modelsDto)
                {
                    // Se não possuir o lote, será criado
                    if (item.Id == 0)
                    {
                        var batch = _mapper.Map<Batch>(item);
                        batch.EventId = eventId;

                        await _batchRepository.Add(batch);
                    }

                    // Se possuir o lote será alterado
                    else
                    {
                        var batch = batches.FirstOrDefault(b => b.Id == item.Id);
                        item.EventId = eventId;

                        _mapper.Map(item, batch);
                        await _batchRepository.Update(batch);
                    }
                }

                var batchesUpdated = await _batchRepository.GetBactchesByEventIdAsync(eventId);
                return _mapper.Map<IEnumerable<BatchDTO>>(batchesUpdated);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<BatchDTO> GetBatchByEventIdBatchIdAsync(int eventId, int batchId)
        {
            try
            {
                var batch = await _batchRepository.GetBatchByIdsAsync(eventId, batchId);
                if (batch == null) return null;

                return _mapper.Map<BatchDTO>(batch);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<IEnumerable<BatchDTO>> GetBatchesByEventIdAsync(int eventId)
        {
            try
            {
                var batches = await _batchRepository.GetBactchesByEventIdAsync(eventId);
                if (batches == null) return null;

                return _mapper.Map<IEnumerable<BatchDTO>>(batches);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<bool> DeleteBatch(int eventId, int batchId)
        {
            try
            {
                var batch = await _batchRepository.GetBatchByIdsAsync(eventId, batchId);
                if (batch == null) throw new Exception("Batch not found");

                await _batchRepository.Delete(batch);
                return true;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}