using System;
using Microsoft.AspNetCore.Mvc;
using Scorpio.Api.DataAccess;
using Scorpio.Api.Models;
using Scorpio.Api.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Matty.Framework;
using Scorpio.Api.Validation;

namespace Scorpio.Api.Controllers
{
    public class SensorDataController : CrudController<ISensorDataRepository, SensorData>
    {
        public SensorDataController(ISensorDataRepository dataRepository) : base(dataRepository)
        {
        }

        [HttpGet("sensorKey/{sensorKey}")]
        [ProducesResponseType(typeof(List<SensorData>), 200)]
        public async Task<IActionResult> GetBySensorKey(string sensorKey)
        {
            var result = await Repository.GetManyFiltered(x => x.SensorKey == sensorKey);
            return Ok(result);
        }

        [HttpGet("sensorKey/{sensorKey}/latest")]
        [ProducesResponseType(typeof(SensorData), 200)]
        public async Task<IActionResult> GetLatestBySensorKey(string sensorKey)
        {
            var result = await Repository.GetLatestFiltered(x => x.SensorKey == sensorKey) ?? new SensorData();
            return Ok(result);
        }

        [HttpGet("sensorKey/{sensorKey}/paged")]
        [ProducesResponseType(typeof(List<SensorData>), 200)]
        public async Task<IActionResult> GetBySensorKeyPaged(string sensorKey, [FromQuery] PageParam pageParam)
        {
            var results = await Repository.GetManyFilteredAndPaged(x => x.SensorKey == sensorKey, pageParam);
            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SensorData), 201)]
        public override Task<ServiceResult<SensorData>> Add(SensorData entity)
        {
            SensorDataValidatorExecutor.Execute(entity, true);

            // If no date, add current one
            if (entity.TimeStamp == new DateTime()) entity.TimeStamp = DateTime.UtcNow;

            return base.Add(entity);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SensorData), 200)]
        public override Task<ServiceResult<SensorData>> Update(string id, SensorData entity)
        {
            SensorDataValidatorExecutor.Execute(entity, true);
            if (string.IsNullOrWhiteSpace(entity.Id)) entity.Id = id;
            return base.Update(id, entity);
        }

        [HttpDelete("many/{sensorKey}")]
        public async Task<IActionResult> RemoveRange(string sensorKey, DateTime? from, DateTime? to)
        {
            var result = new ServiceResult<long>();

            if (to.HasValue && from.HasValue && to < from)
                throw new ArgumentException("Date 'from' cannot be higher than 'to'");

            var deletedCount = await Repository.RemoveRange(sensorKey, from, to);

            result.Data = deletedCount;
            result.AddSuccessMessage(deletedCount > 0
                ? $"Deleted {deletedCount} entries"
                : "No data matching criteria to delete");

            return Ok(result);
        }
    }
}
