using System.Threading.Tasks;
using BankingApi.DTOs;
using BankingApi.Enums;

namespace BankingApi.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResult> CreateTransaction(CreateTransactionDto createTransactionDto, string userName);
    }
}