using BankingApi.Enums;

namespace BankingApi.DTOs
{
    public class TransferHistoryDto
    { 
        public string SenderAccountNumber { get; set; }
        public string RecipientAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public TypeTransaction TypeTransaction { get; set; }
        public string UserName { get; set; }
    }
}