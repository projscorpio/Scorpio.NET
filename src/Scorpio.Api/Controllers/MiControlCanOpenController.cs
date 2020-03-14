using Matty.Framework;
using Microsoft.AspNetCore.Mvc;
using Scorpio.CanOpenEds;
using Scorpio.CanOpenEds.DTO;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.Threading.Tasks;

namespace Scorpio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MiControlCanOpenController : ControllerBase
    {
        private readonly ICanOpenObjectRepository _miControlCanOpenObjectRepository;
        private readonly IEventBus _eventBus;

        public MiControlCanOpenController(ICanOpenObjectRepository openObjectRepository, IEventBus eventBus)
        {
            _miControlCanOpenObjectRepository = openObjectRepository;
            _eventBus = eventBus;
        }

        [HttpGet("tree")]
        [ProducesResponseType(typeof(ServiceResult<TreeDTO>), 200)]
        [ProducesResponseType(typeof(ServiceResult<TreeDTO>), 400)]
        public async Task<IActionResult> GetTree()
        {
            var result = new ServiceResult<TreeDTO>();

            try
            {
                result.Data = await _miControlCanOpenObjectRepository.GetTreeAsync();
            }
            catch(Exception ex) 
            {
                result.AddErrorMessage(ex.Message);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{index}/{subIndex}")]
        [ProducesResponseType(typeof(ServiceResult<CanOpenObjectResponseDTO>), 200)]
        [ProducesResponseType(typeof(ServiceResult<CanOpenObjectResponseDTO>), 400)]
        [ProducesResponseType(typeof(ServiceResult<CanOpenObjectResponseDTO>), 404)]
        public async Task<IActionResult> GetCanObject(string index, string subIndex)
        {
            var result = new ServiceResult<CanOpenObjectResponseDTO>();

            try
            {
                result.Data = await _miControlCanOpenObjectRepository.GetCanOpenObjectAsync(index, subIndex);
                if (result.Data is null) return NotFound(result);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("publish")]
        [ProducesResponseType(typeof(ServiceResult<object>), 200)]
        [ProducesResponseType(typeof(ServiceResult<object>), 400)]
        public IActionResult SendCanOpenObject(SendCanOpenObjectParam canOpenObjectParam)
        {
            var result = new ServiceResult<object>();

            try
            {
                _eventBus.Publish(new SendCanOpenObjectCommand
                {
                    Index = canOpenObjectParam.Index,
                    SubIndex = canOpenObjectParam.SubIndex,
                    DataType = canOpenObjectParam.DataType,
                    Value = canOpenObjectParam.Value
                });

                result.AddSuccessMessage("Successfully published");
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
                return BadRequest(result);
            }
        }
    }
}
