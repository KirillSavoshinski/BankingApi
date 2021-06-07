using System;
using System.Threading.Tasks;
using BankingApi.DTOs;
using BankingApi.Enums;
using BankingApi.Interfaces;

namespace BankingApi.Services
{
    public class TransactionService: ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork; 

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        
        public async Task<TransactionResult> CreateTransaction(CreateTransactionDto createTransactionDto, string userName)
        {
            if (createTransactionDto.RecipientAccountNumber == createTransactionDto.SenderAccountNumber)
            {
                return TransactionResult.SameAccountError;
            }
            
            if (createTransactionDto.Amount < 0M) return TransactionResult.WrongAmountError;
            
            var senderAccount = 
                await _unitOfWork.BankAccountRepository.FindBankAccount(createTransactionDto.SenderAccountNumber, userName);
            var recipientAccount =
                await _unitOfWork.BankAccountRepository.FindBankAccount(createTransactionDto.RecipientAccountNumber);

            if (senderAccount == null) return TransactionResult.WrongSenderCredentialsError;
            if (recipientAccount == null) return TransactionResult.WrongRecipientCredentialsError;

            if (senderAccount.Balance - createTransactionDto.Amount < 0M) return TransactionResult.NotEnoughFundsError;

            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.TransactionRepository.CreateTransaction(senderAccount, recipientAccount,
                    createTransactionDto.Amount);
                await _unitOfWork.Complete();
                await transaction.CommitAsync();
                return TransactionResult.Ok;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return TransactionResult.UnexpectedError;
            }  
        }
    }
}