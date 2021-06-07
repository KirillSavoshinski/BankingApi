using BankingApi.Entities;

namespace BankingApi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Customer user);
    }
}