using MessageService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageService.Interfaces
{
    public interface IQueueService
    {
        Task<IEnumerable<Measurement>> GetMeasurementsFromQueue();
    }
}
