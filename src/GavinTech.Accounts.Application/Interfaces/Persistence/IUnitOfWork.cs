using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        void EnableChangeTracking();
        Task SaveChangesAsync(CancellationToken ct);
    }
}
