using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTemplate.Api.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;

        Task CommitAsync(CancellationToken cancellationToken);
    }
}
