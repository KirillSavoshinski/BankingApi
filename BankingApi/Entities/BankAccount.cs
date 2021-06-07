using System.Collections.Generic;

namespace BankingApi.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public Customer Customer { get; set; } 
        public decimal Balance { get; set; }
        public string AccountNumber { get; set; }
        public ICollection<Transaction> Income { get; set; }
        public ICollection<Transaction> Outcome { get; set; }
    }
}