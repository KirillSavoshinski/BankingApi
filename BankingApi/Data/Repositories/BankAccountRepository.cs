using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankingApi.DTOs;
using BankingApi.Entities;
using BankingApi.Enums;
using BankingApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Data.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BankAccountRepository(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }

        public async Task<BankAccountDto> CreateBankAccount(CreateBankAccountDto createAccountDto, Customer customer)
        {
            var bankAccount = new BankAccount
            {
                Customer = customer,
                Balance = createAccountDto.InitialDeposit,
                AccountNumber = Guid.NewGuid().ToString()
            };

            await _context.BankAccounts.AddAsync(bankAccount);
            return _mapper.Map<BankAccountDto>(bankAccount);
        }

        public async Task<BankAccount> FindBankAccount(string accountNumber, string userName = null)
        {
            var bankAccount = await _context.BankAccounts.Include(c => c.Customer)
                .FirstOrDefaultAsync(n => n.AccountNumber == accountNumber);

            if (bankAccount == null) return null;

            if (userName != null && bankAccount.Customer.UserName != userName) return null;

            return bankAccount;
        }

        public async Task<IEnumerable<TransferHistoryDto>> GetHistory(string accountNumber, string userName)
        {
            var bankAccount = await _context.BankAccounts.Include(c => c.Customer)
                .Include(i => i.Income)
                .ThenInclude(i => i.RecipientAccount)
                .Include(i => i.Income)
                .ThenInclude(i => i.SenderAccount)
                .Include(o => o.Outcome)
                .ThenInclude(i => i.RecipientAccount)
                .Include(i => i.Outcome)
                .ThenInclude(i => i.SenderAccount)
                .FirstOrDefaultAsync(n => n.AccountNumber == accountNumber);

            var history = new List<TransferHistoryDto>();

            if (bankAccount == null || bankAccount.Customer.UserName != userName) return null;

            history.AddRange(bankAccount.Income.Select(transaction => new TransferHistoryDto
            {
                SenderAccountNumber = transaction.SenderAccount.AccountNumber,
                RecipientAccountNumber = transaction.RecipientAccount.AccountNumber,
                Amount = transaction.Amount,
                TypeTransaction = TypeTransaction.Income, 
                UserName = bankAccount.Customer.UserName
            }));
            
            history.AddRange(bankAccount.Outcome.Select(transaction => new TransferHistoryDto
            {
                SenderAccountNumber = transaction.SenderAccount.AccountNumber,
                RecipientAccountNumber = transaction.RecipientAccount.AccountNumber,
                Amount = transaction.Amount,
                TypeTransaction = TypeTransaction.Income, 
                UserName = bankAccount.Customer.UserName
            }));

            return history;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}