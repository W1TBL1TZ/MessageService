using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageService.Interfaces;
using MessageService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementController : ControllerBase
    {

        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpPost("/process-messages-from-queue")]
        public async Task<IActionResult> ProcessValidMessagesFromQueue()
        {
            await _measurementService.ProcessValidMessagesFromQueue();
            return Ok();
        }

        [HttpGet("/measurements-created-between-dates")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Measurement>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMeasurementsCreatedBetweenDates([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start > end)
            {
                return BadRequest();
            }
            var result = await _measurementService.GetMeasurementsCreatedBetweenDates(start, end);
            return Ok(result);
        }

        // This was just added for me to test adding the records to the table and not clogging it up with a bunch of duplicate data
        [HttpDelete("/clear-table")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearTable()
        {
            await _measurementService.ClearTable();
            return Ok();
        }
    }
}
