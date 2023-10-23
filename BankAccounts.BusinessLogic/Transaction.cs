using BankAccounts.BusinessLogic.Validation;

namespace BankAccounts.BusinessLogic
{
    public class Transaction
    {
        internal Transaction(
            long number,
            DateTime timestamp,
            TransactionType transactionType,
            Account sourceAccount,
            Account counterpartyAccount,
            decimal transactionAmount)
        {
            if (transactionAmount < 0)
            {
                throw new ArgumentException(
                    "The transaction amount must be a positive number",
                    nameof(transactionAmount));
            }

            TransactionValidator.ValidateTransactionType(transactionType);

            Number = number; ;
            Timestamp = timestamp;
            TransactionType = transactionType;
            SourceAccount = sourceAccount;
            CounterpartyAccount = counterpartyAccount;
            TransactionAmount = transactionAmount;
            SourceAccountAmountBefore = sourceAccount.CurrentAmount;
            
            checked
            {
                switch (transactionType)
                {
                    case TransactionType.MoneyIn:
                        SourceAccountAmountAfter = sourceAccount.CurrentAmount + transactionAmount;
                        break;

                    case TransactionType.MoneyOut:
                        SourceAccountAmountAfter = sourceAccount.CurrentAmount - transactionAmount;
                        break;
                }
            }
        }

        public DateTime Timestamp { get; }

        public long Number { get; }

        public TransactionType TransactionType { get; }

        public Account SourceAccount { get; }

        public Account CounterpartyAccount { get; }

        public decimal TransactionAmount { get; }

        public decimal SourceAccountAmountBefore { get; }

        public decimal SourceAccountAmountAfter { get; }
    }
}
