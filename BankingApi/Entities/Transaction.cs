using System;

namespace BankingApi.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public BankAccount SenderAccount { get; set; }
        public BankAccount RecipientAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}