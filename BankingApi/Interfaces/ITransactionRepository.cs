using System.Threading.Tasks;
using BankingApi.Entities;

namespace BankingApi.Interfaces
{
    public interface ITransactionRepository
    {
        Task CreateTransaction(BankAccount senderAccount, BankAccount recipientAccount,
            decimal amount);
    }
}