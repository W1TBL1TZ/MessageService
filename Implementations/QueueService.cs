using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using MessageService.Interfaces;
using MessageService.Models;
using MessageService.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Implementations
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;
        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<Measurement>> GetMeasurementsFromQueue()
        {
            QueueClient queue = new QueueClient(_configuration.GetConnectionString(Constants.SAS), Constants.DEVICEMEASUREMENTQUEUENAME);

            var messages = await RetrieveNextMessageAsync(queue);
            var result = new List<Measurement>();

            foreach (var message in messages)
            {
                result.Add(message.Deserialize<Measurement>());
            }

            return result;
        }

        private async Task<IEnumerable<string>> RetrieveNextMessageAsync(QueueClient theQueue)
        {
            // Again I am not sure here about the implementation of how many to take off the queue, but I beleive they need to be deleted once they are retrieved.
            // I opted to not delete so I could keep testing over and over. I assume the implementation overall would be similar to Service bus or Event grid.
            // I just decided to grab around 100 messages here for testing.
            var result = new List<string>();
            if (await theQueue.ExistsAsync())
            {
                QueueProperties properties = await theQueue.GetPropertiesAsync();

                if (properties.ApproximateMessagesCount > 0)
                {
                    while (result.Count < 100)
                    {
                        var retrievedMessages = await theQueue.ReceiveMessagesAsync(Math.Min(properties.ApproximateMessagesCount, 32));
                        result.AddRange(retrievedMessages.Value.Select(m => m.Body.ToString()));
                    }
                }
            }

            return result;
        }
    }
}
