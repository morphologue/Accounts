using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
