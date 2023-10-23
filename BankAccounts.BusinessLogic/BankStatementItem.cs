namespace BankAccounts.BusinessLogic
{
    public class BankStatementItem
    {
        internal BankStatementItem(Transaction transaction)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            Timestamp = transaction.Timestamp;
            TransactionType = transaction.TransactionType;
            Counterparty = $"{transaction.CounterpartyAccount.Owner}({transaction.CounterpartyAccount.Iban})";
            AmountBefore = transaction.SourceAccountAmountBefore;
            TransactionAmout = transaction.TransactionAmount;
            AmountAfter = transaction.SourceAccountAmountAfter;
        }

        public DateTime Timestamp { get; }

        public TransactionType TransactionType { get; }

        public string Counterparty { get; }

        public decimal AmountBefore { get; }

        public decimal TransactionAmout { get; }

        public decimal AmountAfter { get; }
    }
}
