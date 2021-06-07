namespace BankingApi.DTOs
{
    public class CreateTransactionDto
    {
        public string SenderAccountNumber { get; set; }
        public string RecipientAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}