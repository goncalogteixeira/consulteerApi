using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Infrastructure.DbContexts;

namespace WebApiTemplate.Api.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private bool disposed;
        private Dictionary<Type, object> repositories;
        private readonly IMapper _mapper;

        public UnitOfWork(
            ApplicationDbContext dbContext
            , IMapper mapper
            )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the specified repository for the <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="hasCustomRepository"><c>True</c> if providing custom repositry</param>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
        public IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            // what's the best way to support custom reposity?
            if (hasCustomRepository)
            {
                var customRepo = _dbContext.GetService<IBaseRepository<TEntity>>();

                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);

            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new BaseRepository<TEntity>(_dbContext, _mapper);
            }

            return (IBaseRepository<TEntity>)repositories[type];
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return _dbContext.BulkSaveChangesAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected async virtual ValueTask DisposeAsync(bool disposing)
        {
            if (!disposed && disposing)
            {
                //dispose managed resources
                await _dbContext.DisposeAsync();

                //dispose unmanaged resources
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }

            //dispose unmanaged resources
            disposed = true;
        }
    }
}
