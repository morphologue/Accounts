using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken ct);
    }
}
