namespace BankAccounts.BusinessLogic
{
    public partial class BankStatement
    {
        internal BankStatement(
            Account account,
            DateTime fromDate,
            DateTime toDate,
            decimal startAmount,
            Transaction[] transactionsWithinPeriod)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            if (startAmount < 0)
            {
                throw new ArgumentException(
                    "The start amount cannot be a negative value",
                    nameof(startAmount));
            }

            if (transactionsWithinPeriod is null)
            {
                throw new ArgumentNullException(nameof(transactionsWithinPeriod));
            }

            Account = account;
            FromDate = fromDate;
            ToDate = toDate;
            StartAmount = startAmount;
            
            Items = new BankStatementItem[transactionsWithinPeriod.Length];

            decimal finalAmount = startAmount;
            for (int i = 0; i < transactionsWithinPeriod.Length; i++)
            {
                Transaction transaction = transactionsWithinPeriod[i];

                if (transaction.SourceAccount is null)
                {
                    throw new ArgumentException(
                        "Found a transaction without source account",
                        nameof(transactionsWithinPeriod));
                }

                if (!string.Equals(transaction.SourceAccount.Iban, Account.Iban, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException(
                        $"Found a transaction that reference anotner source account. Expected to reference account IBAN '{Account.Iban}' but found '{transaction.SourceAccount.Iban}'",
                        nameof(transactionsWithinPeriod));
                }

                Items[i] = new BankStatementItem(transaction);

                switch (transaction.TransactionType)
                {
                    case TransactionType.MoneyIn:
                        finalAmount += transaction.TransactionAmount;
                        break;

                    case TransactionType.MoneyOut:
                        finalAmount -= transaction.TransactionAmount;
                        break;
                }
            }

            FinalAmount = finalAmount;
        }

        public Account Account { get; }

        public decimal StartAmount { get; }

        public decimal FinalAmount { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public BankStatementItem[] Items { get; }
    }
}
