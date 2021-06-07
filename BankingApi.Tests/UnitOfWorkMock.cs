using BankingApi.Interfaces;
using Moq; 

namespace BankingApi.Tests
{
    public class UnitOfWorkMock
    {
        public Mock<IUnitOfWork> UnitOfWorkMocked { get; set; } = new();
    }
}