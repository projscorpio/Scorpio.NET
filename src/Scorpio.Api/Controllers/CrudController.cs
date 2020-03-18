using Matty.Framework;
using Microsoft.AspNetCore.Mvc;
using Scorpio.Api.DataAccess;
using Scorpio.Api.Models;
using Scorpio.Api.Paging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Scorpio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class CrudController<TRepository, TEntity> : ControllerBase
        where TRepository : IGenericRepository<TEntity, string>
        where TEntity : EntityBase
    {
        protected readonly TRepository Repository;

        protected CrudController(TRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var result = await Repository.GetAllAsync();
            return result;
        }

        [HttpGet("paged")]
        public virtual async Task<PagedList<TEntity>> GetPaged([FromQuery] PageParam pageParam)
        {
            var result = await Repository.GetPaged(pageParam);
            return result;
        }

        [HttpGet("{id}")]
        public virtual async Task<TEntity> GetById(string id)
        {
            var result = await Repository.GetByIdAsync(id);

            if (result is null)
            {
                NotFound();
            }

            return result;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public virtual async Task<ServiceResult<TEntity>> Add(TEntity entity)
        {
            var response = new ServiceResult<TEntity>(User);

            try
            {
                response.Data = await Repository.CreateAsync(entity);
                response.AddSuccessMessage($"Successfully created {entity}");
                CreatedAtAction(nameof(Add), response);
            }
            catch (FormatException ex)
            {
                var msg = $"Supplied invalid id: {ex.Message} Please don't use any ID here - it will be created automatically.";
                response.AddErrorMessage(msg);
                BadRequest(response);
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                BadRequest(response);
            }

            return response;
        }

        [HttpDelete("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public virtual async Task<ServiceResult<TEntity>> Delete(string id)
        {
            var response = new ServiceResult<TEntity>(User);
            var existing = await Repository.GetByIdAsync(id);
            if (existing is null)
            {
                NotFound();
                return response;
            }
            
            try
            {
                await Repository.DeleteAsync(existing);
                response.AddSuccessMessage($"Successfully deleted {existing}");
                CreatedAtAction(nameof(Add), response);
            }
            catch (FormatException ex)
            {
                response.AddErrorMessage($"Supplied invalid id: {ex.Message}");
                BadRequest(response);
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                BadRequest(response);
            }

            return response;
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public virtual async Task<ServiceResult<TEntity>> Update(string id, TEntity entity)
        {
            var response = new ServiceResult<TEntity>(User);
            var existing = await Repository.GetByIdAsync(id);
            if (existing is null)
            {
                NotFound();
            }

            try
            {
                response.Data = await Repository.UpdateAsync(entity);
                response.AddSuccessMessage($"Successfully updated entity: {entity}");
                Ok(response);
            }
            catch (FormatException ex)
            {
                response.AddErrorMessage($"Supplied invalid id: {ex.Message}");
                BadRequest(response);
            }
            catch (Exception ex)
            {
                response.AddErrorMessage(ex.Message);
                BadRequest(response);
            }

            return response;
        }
    }
}