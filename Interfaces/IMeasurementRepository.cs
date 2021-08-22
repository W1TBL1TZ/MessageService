using MessageService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageService.Interfaces
{
    public interface IMeasurementRepository
    {
        Task<bool> SaveValidMessages(IEnumerable<Measurement> measurements);
        Task<IEnumerable<Measurement>> GetMeasurementsCreatedBetweenDates(DateTime start, DateTime end);

        Task ClearTable();
    }
}
