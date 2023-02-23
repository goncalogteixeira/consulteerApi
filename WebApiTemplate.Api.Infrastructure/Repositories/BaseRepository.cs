using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Common;
using WebApiTemplate.Api.Application.Extensions;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Infrastructure.DbContexts;
using WebApiTemplate.Api.Infrastructure.Extensions;
using Z.EntityFramework.Plus;

namespace WebApiTemplate.Api.Infrastructure.Repositories
{
    // TODO: this still has to be tested when the CRUD operations are created.
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BaseRepository(
            ApplicationDbContext dbContext
            , IMapper mapper
            )
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public ValueTask<T> GetByIdAsync(int id)
        {
            return _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync(bool track = false)
        {
            return track ? await _dbContext.Set<T>().ToListAsync()
                         : await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public Task<List<T>> GetAsync(bool track = false, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = track
                                  ? _dbContext.Set<T>()
                                  : _dbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes.SafeAny())
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToListAsync();
        }

        public Task<List<T>> GetWithQueryAsync(IQueryable<T> query, params SortInfo<T>[] orderBy)
        {
            IQueryable<T> sortedQuery = query;

            if (orderBy.SafeAny())
            {
                foreach (SortInfo<T> o in orderBy)
                {
                    sortedQuery = sortedQuery.SortBy(o.Ascending, o.Key);
                }
            }

            return sortedQuery.ToListAsync();
        }

        public Task<List<U>> GetProjectionAsync<U>(Expression<Func<T, bool>> filter = null, params SortInfo<T>[] orderBy) where U : class
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy.SafeAny())
            {
                foreach (SortInfo<T> o in orderBy)
                {
                    query = query.SortBy(o.Ascending, o.Key);
                }
            }

            return query.ProjectTo<U>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public Task<List<U>> GetProjectionWithQueryAsync<U>(IQueryable<T> query, params SortInfo<T>[] orderBy) where U : class
        {
            IQueryable<T> sortedQuery = query;

            if (orderBy.SafeAny())
            {
                foreach (SortInfo<T> o in orderBy)
                {
                    sortedQuery = sortedQuery.SortBy(o.Ascending, o.Key);
                }
            }

            return sortedQuery
                .ProjectTo<U>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<PagedResponse<U>> GetPagedProjectionAsync<U>(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null, params SortInfo<T>[] orderBy) where U : class
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return GetPagedProjectionWithQueryAsync<U>(pageNumber, pageSize, query, orderBy);
        }

        public async Task<PagedResponse<U>> GetPagedProjectionWithQueryAsync<U>(int pageNumber, int pageSize, IQueryable<T> query, params SortInfo<T>[] orderBy) where U : class
        {
            int count = 0;
            List<U> data = new();

            if (pageSize > 0)
            {
                count = query.DeferredCount().FutureValue();

                IQueryable<T> sortedQuery = query;

                if (orderBy.SafeAny())
                {
                    foreach (SortInfo<T> o in orderBy)
                    {
                        sortedQuery = sortedQuery.SortBy(o.Ascending, o.Key);
                    }
                }

                data = await sortedQuery
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ProjectTo<U>(_mapper.ConfigurationProvider)
                        .Future()
                        .ToListAsync();
            }

            return new PagedResponse<U>(data, pageNumber, pageSize, count);
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);

            return entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public Task BulkInsertAsync(IEnumerable<T> entities)
        {
            return _dbContext.BulkInsertAsync(entities);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).CurrentValues.SetValues(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities.ToArray());
        }

        public Task BulkUpdateAsync(IEnumerable<T> entities)
        {
            return _dbContext.BulkUpdateAsync(entities);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public Task BulkDeleteAsync(IEnumerable<T> entities)
        {
            return _dbContext.BulkDeleteAsync(entities);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.CountAsync();
        }
    }
}
