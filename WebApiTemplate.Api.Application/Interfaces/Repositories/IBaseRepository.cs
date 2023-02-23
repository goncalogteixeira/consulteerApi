using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Common;

namespace WebApiTemplate.Api.Application.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }

        ValueTask<T> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync(bool track = false);

        Task<List<T>> GetAsync(bool track = false, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);

        Task<List<T>> GetWithQueryAsync(IQueryable<T> query, params SortInfo<T>[] orderBy);

        Task<List<U>> GetProjectionAsync<U>(Expression<Func<T, bool>> filter = null, params SortInfo<T>[] orderBy) where U : class;

        Task<List<U>> GetProjectionWithQueryAsync<U>(IQueryable<T> query, params SortInfo<T>[] orderBy) where U : class;

        Task<PagedResponse<U>> GetPagedProjectionAsync<U>(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null, params SortInfo<T>[] orderBy) where U : class;

        Task<PagedResponse<U>> GetPagedProjectionWithQueryAsync<U>(int pageNumber, int pageSize, IQueryable<T> query, params SortInfo<T>[] orderBy) where U : class;

        T Add(T entity);

        void AddRange(IEnumerable<T> entities);

        Task BulkInsertAsync(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        Task BulkUpdateAsync(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        Task BulkDeleteAsync(IEnumerable<T> entities);

        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
