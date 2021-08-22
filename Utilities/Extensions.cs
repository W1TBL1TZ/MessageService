using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace MessageService.Utilities
{
    // Same sentiment here as with Utilities. These types of classes are great candidates for nuget packages.
    public static class Extensions
    {

        public static IEnumerable<T> ExecuteQuery<T>(this CloudTable table, TableQuery<T> query) where T : ITableEntity, new()
        {
            List<T> result = new List<T>();

            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> tableQueryResult = table.ExecuteQuerySegmentedAsync(query, continuationToken).Result;

                result.AddRange(tableQueryResult.Results);

                continuationToken = tableQueryResult.ContinuationToken;
            } while (continuationToken != null);

            return result;
        }
    }
}
