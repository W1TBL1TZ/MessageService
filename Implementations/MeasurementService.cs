using MessageService.Interfaces;
using MessageService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Implementations
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IQueueService _queueService;
        private readonly IMeasurementRepository _measurementRepository;
        public MeasurementService(IQueueService queueService, IMeasurementRepository measurementRepository)
        {
            _queueService = queueService;
            _measurementRepository = measurementRepository;
        }
        public async Task<IEnumerable<Measurement>> GetMeasurementsCreatedBetweenDates(DateTime start, DateTime end)
        {
            return await _measurementRepository.GetMeasurementsCreatedBetweenDates(start, end);
        }

        public async Task ProcessValidMessagesFromQueue()
        {
            var messagesToProcess = await _queueService.GetMeasurementsFromQueue();
            var validMessages = messagesToProcess.Where(m => m.IsValid);

            if (validMessages.Any())
            {
                await _measurementRepository.SaveValidMessages(validMessages);
            }
        }

        public async Task ClearTable()
        {
            await _measurementRepository.ClearTable();
        }
    }
}
