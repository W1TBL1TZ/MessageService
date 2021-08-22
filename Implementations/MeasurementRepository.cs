using MessageService.Interfaces;
using MessageService.Models;
using MessageService.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService.Implementations
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly IConfiguration _configuration;
        private readonly CloudTable validMessgesTable;
        public MeasurementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            var storageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString(Constants.SAS));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            validMessgesTable = cloudTableClient.GetTableReference(Constants.VALIDMESSAGESTABLENAME);
        }

        public async Task ClearTable()
        {
            try
            {
                var query = new TableQuery<Measurement>();
                TableContinuationToken token = null;

                do
                {
                    var result = await validMessgesTable.ExecuteQuerySegmentedAsync(query, token);
                    foreach (var row in result)
                    {
                        var op = TableOperation.Delete(row);
                        await validMessgesTable.ExecuteAsync(op);
                    }
                    token = result.ContinuationToken;
                } while (token != null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveValidMessages(IEnumerable<Measurement> measurements)
        {
            TableBatchOperation batchInsert = new TableBatchOperation();
            foreach (var measurement in measurements)
            {
                // I have actually not worked directly with Cloud tables before, so would be quite curious to talk to you about production scenarios regarding 
                // how you decide on your partition and row key. For the purpose of this test, I just threw them into one bucket.
                // Given the DDD approach that I took with the model, I would probably work the partition and row key determination in there as well in a real scenario
                measurement.PartitionKey = "Test";
                measurement.RowKey = measurement.MeasurementId;
                batchInsert.Insert(measurement);
            }
            var result = await validMessgesTable.ExecuteBatchAsync(batchInsert);

            return true;
        }

        public async Task<IEnumerable<Measurement>> GetMeasurementsCreatedBetweenDates(DateTime start, DateTime end)
        {
            // As mentioned above, with a lack of direct experience with cloud tables, I am not quite sure what best practice would be in terms of retrieval.
            // In a regular database scenario, I would probably create a stored procedure with params for the start and end date.
            var rangeQuery = new TableQuery<Measurement>().Where(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterConditionForDate("CreatedAt", QueryComparisons.GreaterThan, start),
            TableOperators.And,
            TableQuery.GenerateFilterConditionForDate("CreatedAt", QueryComparisons.LessThan, end)));

            var entities = validMessgesTable.ExecuteQuery(rangeQuery).ToList();

            return entities;
        }
    }
}
