using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Scorpio.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Scorpio.Api.DataAccess
{
    public class SensorDataRepository : MongoRepository<SensorData>, ISensorDataRepository
    {
        public SensorDataRepository(IOptions<MongoDbConfiguration> options) : base(options)
        {
        }

        public async Task<long> RemoveRange(string sensorKey, DateTime? from, DateTime? to)
        {
            FilterDefinition<SensorData> filter = Builders<SensorData>.Filter.Eq("SensorKey", sensorKey);

            // Specified either from od to date
            // If 'to' date is not specified, default to now,
            // that means we wont delete future entries if no 'to' date is specifeid explicitly
            if (from.HasValue || to.HasValue)
            {
                var dateFrom = from ?? new DateTime();
                var dateTo = to ?? DateTime.UtcNow;

                var dateFilters = Builders<SensorData>.Filter.Lte("TimeStamp", new BsonDateTime(dateTo))
                  & Builders<SensorData>.Filter.Gte("TimeStamp", new BsonDateTime(dateFrom));

                filter &= dateFilters;
            }

            var result = await Collection.DeleteManyAsync(filter);
            return result.DeletedCount;
        }

        public override async Task<SensorData> CreateAsync(SensorData entity)
        {
            var currentDocs = await GetManyFiltered(x => x.SensorKey == entity.SensorKey);
            var currentCount = currentDocs.Count();
            var allowedCount = Options.Value.SensorDataSamplesToKeep;

            if (currentCount >= allowedCount)
            {
                // Remove oldest ones
                var docToRemoveIds = currentDocs
                    .OrderBy(x => x.TimeStamp)
                    .Take(currentCount - allowedCount + 1)
                    .Select(x => x.Id);

                await Collection.DeleteManyAsync(x => docToRemoveIds.Contains(x.Id));
            }

            return await base.CreateAsync(entity);
        }
    }
}
