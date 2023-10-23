namespace BankAccounts.BusinessLogic.Validation
{
    internal static class TransactionValidator
    {
        public static void ValidateTransactionType(TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.MoneyIn:
                case TransactionType.MoneyOut:
                    break;

                default:
                    throw new ArgumentException(
                            $"Unknown transaction type '{transactionType}'",
                            nameof(transactionType));
            }
        }
    }
}
