using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingApi.Interfaces
{
    public interface IUnitOfWork
    {
        ITransactionRepository TransactionRepository { get; }
        IBankAccountRepository BankAccountRepository { get; }
        Task<bool> Complete();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}