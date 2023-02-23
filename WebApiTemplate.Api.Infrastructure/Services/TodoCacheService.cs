using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Common.CacheKeys;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Extensions;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Application.Interfaces.Services;
using WebApiTemplate.Api.Domain.Entities;

namespace WebApiTemplate.Api.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        readonly ICacheService _cacheService;
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public TodoService(ICacheService cacheService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TodoDTO>> GetAllTodosAsync()
        {
            return (List<TodoDTO>)await GetTodosAsync();
        }

        public async Task<TodoDTO> GetTodoAsync(int id)
        {
            // get settings from cache
            var cacheKey = TodoCacheKeys.GetDetailsKey(id);

            // check if is in cache
            var todo = await _cacheService.GetAsync<TodoDTO>(cacheKey);

            // get from database
            if (todo == null)
            {
                todo = _mapper.Map<TodoDTO>(await _unitOfWork.GetRepository<Todo>().GetByIdAsync(id));

                // add to cache
                await _cacheService.SetAsync(cacheKey, todo);
            }

            return todo;
        }

        public async Task FlushCache()
        {
            await _cacheService.RemoveByPrefixAsync(TodoCacheKeys.Prefix);
        }

        #region Private Methods

        private async Task<IEnumerable<TodoDTO>> GetTodosAsync()
        {
            // get settings from cache
            var cacheKey = TodoCacheKeys.GetAllKey();

            // check if is in cache
            var resources = await _cacheService.GetAsync<IEnumerable<TodoDTO>>(cacheKey);

            // get from database
            if (!resources.SafeAny())
            {
                resources = _mapper.Map<IEnumerable<TodoDTO>>(await _unitOfWork.GetRepository<Todo>().GetAllAsync());

                // add to cache
                await _cacheService.SetAsync(cacheKey, resources);
            }

            return resources;
        }

        #endregion
    }
}


