using MessageService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageService.Interfaces
{
    public interface IMeasurementService
    {
        Task<IEnumerable<Measurement>> GetMeasurementsCreatedBetweenDates(DateTime start, DateTime end);

        Task ProcessValidMessagesFromQueue();

        Task ClearTable();
    }
}
