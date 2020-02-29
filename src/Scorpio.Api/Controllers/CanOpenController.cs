using Matty.Framework;
using Microsoft.AspNetCore.Mvc;
using Scorpio.CanOpenEds;
using Scorpio.CanOpenEds.DTO;
using System;
using System.Threading.Tasks;

namespace Scorpio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CanOpenController : ControllerBase
    {
        private readonly ICanOpenObjectRepository _canOpenObjectRepository;

        public CanOpenController(ICanOpenObjectRepository canOpenObjectRepository)
        {
            _canOpenObjectRepository = canOpenObjectRepository;
        }

        [HttpGet("tree")]
        [ProducesResponseType(typeof(ServiceResult<TreeDTO>), 200)]
        [ProducesResponseType(typeof(ServiceResult<TreeDTO>), 400)]
        public async Task<IActionResult> GetTree()
        {
            var result = new ServiceResult<TreeDTO>();

            try
            {
                result.Data = await _canOpenObjectRepository.GetTreeAsync();
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
                result.Data = await _canOpenObjectRepository.GetCanOpenObjectAsync(index, subIndex);
                if (result.Data is null) return NotFound(result);
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
