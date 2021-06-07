namespace BankingApi.Enums
{
    public enum TransactionResult
    {
        Ok,
        SameAccountError,
        NotEnoughFundsError,
        WrongRecipientCredentialsError,
        WrongSenderCredentialsError, 
        WrongAmountError,
        UnexpectedError
    }
}