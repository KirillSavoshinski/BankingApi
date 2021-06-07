using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApi.DTOs;
using BankingApi.Entities;

namespace BankingApi.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<BankAccountDto> CreateBankAccount(CreateBankAccountDto createAccountDto, Customer customer);
        Task<BankAccount> FindBankAccount(string accountNumber, string userName = null);
        Task<IEnumerable<TransferHistoryDto>> GetHistory(string accountNumber, string userName);
        Task<bool> Save();
    }
}