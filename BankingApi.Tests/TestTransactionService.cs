using System.Threading.Tasks;
using BankingApi.DTOs;
using BankingApi.Entities;
using BankingApi.Enums;
using BankingApi.Interfaces;
using BankingApi.Services;
using Moq;
using Xunit;

namespace BankingApi.Tests
{
    public class TestTransactionService : IClassFixture<UnitOfWorkMock>
    {
        private readonly ITransactionService _transactionService;
        private readonly UnitOfWorkMock _unitOfWorkMoq;
        private const string Name = "Arisha Barron1";

        public TestTransactionService(UnitOfWorkMock unitOfWorkMoq)
        {
            _unitOfWorkMoq = unitOfWorkMoq;
            _transactionService = new TransactionService(_unitOfWorkMoq.UnitOfWorkMocked.Object);
        }

        [Fact]
        public async Task WrongAmountError_Create_Transaction_Test()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                SenderAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                RecipientAccountNumber = "52e368f3-3d14-4a9d-aba4-f675f550ee58",
                Amount = -10
            };

            // Act
            var result = await _transactionService.CreateTransaction(createTransactionDto, Name);

            // Assert
            Assert.Equal(TransactionResult.WrongAmountError, result);
        }
        
        [Fact]
        public async Task SameAccountError_Create_Transaction_Test()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                SenderAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                RecipientAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                Amount = 10
            };

            // Act
            var result = await _transactionService.CreateTransaction(createTransactionDto, Name);

            // Assert
            Assert.Equal(TransactionResult.SameAccountError, result);
        }

        [Fact]
        public async Task NotEnoughFundsError_Transaction_Test()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                SenderAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                RecipientAccountNumber = "52e368f3-3d14-4a9d-aba4-f675f550ee58",
                Amount = 10
            };  
            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.SenderAccountNumber, Name))
                .ReturnsAsync(() => new BankAccount {Balance = 5M});

            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.RecipientAccountNumber, null))
                .ReturnsAsync(() => new BankAccount {Balance = 0M});

            // Act
            var result = await _transactionService.CreateTransaction(createTransactionDto, Name);

            // Assert
            Assert.Equal(TransactionResult.NotEnoughFundsError, result);
        }
        
        [Fact]
        public async Task WrongRecipientCredentialsError_Transaction_Test()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                SenderAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                RecipientAccountNumber = "52e368f3-3d14-4a9d-aba4-f675f550ee58",
                Amount = 10
            };
            const string name = "Arisha Barron1";
            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.SenderAccountNumber, name))
                .ReturnsAsync(() => new BankAccount {Balance = 5M});

            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.RecipientAccountNumber, null))
                .ReturnsAsync(() => null);

            // Act
            var result = await _transactionService.CreateTransaction(createTransactionDto, name);

            // Assert
            Assert.Equal(TransactionResult.WrongRecipientCredentialsError, result);
        }
        
        [Fact]
        public async Task WrongSenderCredentialsError_Transaction_Test()
        {
            // Arrange
            var createTransactionDto = new CreateTransactionDto
            {
                SenderAccountNumber = "43dce66f-2117-40de-b400-752b469a6518",
                RecipientAccountNumber = "52e368f3-3d14-4a9d-aba4-f675f550ee58",
                Amount = 10
            };
            const string name = "Arisha Barron1";
            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.SenderAccountNumber, name))
                .ReturnsAsync(() => null);

            _unitOfWorkMoq.UnitOfWorkMocked.Setup(x =>
                    x.BankAccountRepository.FindBankAccount(createTransactionDto.RecipientAccountNumber, null))
                .ReturnsAsync(() => new BankAccount {Balance = 5M});

            // Act
            var result = await _transactionService.CreateTransaction(createTransactionDto, name);

            // Assert
            Assert.Equal(TransactionResult.WrongSenderCredentialsError, result);
        }
    }
}