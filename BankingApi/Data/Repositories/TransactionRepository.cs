using System.Threading.Tasks;
using BankingApi.Entities;
using BankingApi.Interfaces;

namespace BankingApi.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreateTransaction(BankAccount senderAccount, BankAccount recipientAccount,
            decimal amount)
        {
            senderAccount.Balance -= amount;
            recipientAccount.Balance += amount;
            await _context.Transactions.AddAsync(new Transaction
            {
                Amount = amount,
                SenderAccount = senderAccount,
                RecipientAccount = recipientAccount
            });
        }
    }
}