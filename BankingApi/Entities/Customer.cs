using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BankingApi.Entities
{
    public class Customer: IdentityUser<int>
    {
        public ICollection<BankAccount> BankAccounts { get; set; }
    }
}