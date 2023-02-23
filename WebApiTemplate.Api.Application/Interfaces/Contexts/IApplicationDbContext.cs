using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiTemplate.Api.Application.Interfaces.Contexts
{
    public interface IApplicationDbContext
    {
        IDbConnection Connection { get; }

        bool HasChanges { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
