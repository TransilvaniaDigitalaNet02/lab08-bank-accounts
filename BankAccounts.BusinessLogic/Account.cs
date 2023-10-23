using BankAccounts.BusinessLogic.Utilities;
using BankAccounts.BusinessLogic.Validation;

namespace BankAccounts.BusinessLogic
{
    public class Account
    {
        public Account(
            string owner,
            string iban,
            decimal initialDeposit = 0M) 
        {
            PersonValidator.ValidatePersonName(owner);
            IbanValidator.ValidateIban(iban);

            if (initialDeposit < 0)
            {
                throw new ArgumentException(
                    "Initial deposit must be greater than or equal to zero.",
                    nameof(initialDeposit));
            }

            Owner = owner;
            Iban = iban;
            InitialDeposit = initialDeposit;
            CurrentAmount = initialDeposit;
            AllTransactions = new Transaction[0];
        }

        public string Owner { get; }

        public string Iban { get; }

        public decimal InitialDeposit { get; }

        public decimal CurrentAmount { get; private set; }

        public Transaction[] AllTransactions { get; private set; }

        internal void ProcessPayment(Transaction transaction)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction.TransactionType != TransactionType.MoneyOut)
            {
                throw new ArgumentException(
                    $"Invalid transaction type for payment. Expecting {TransactionType.MoneyOut}, but found {transaction.TransactionType}.",
                    nameof(transaction));
            }

            if (!string.Equals(Iban, transaction.SourceAccount.Iban, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"Invalid transaction source account. Expecting to be current IBAN '{Iban}', but found '{transaction.SourceAccount.Iban}'.",
                    nameof(transaction));
            }

            if (CurrentAmount != transaction.SourceAccountAmountBefore)
            {
                throw new ArgumentException(
                    $"Unable to process {TransactionType.MoneyOut} transaction; CurrentAmount {CurrentAmount} is different than the transaction calculated source amount before {transaction.SourceAccountAmountBefore}.",
                    nameof(transaction));
            }

            if (CurrentAmount < transaction.TransactionAmount)
            {
                throw new ArgumentException(
                    $"Unable to process {TransactionType.MoneyOut} transaction; CurrentAmount {CurrentAmount} is less than transaction amount {transaction.TransactionAmount}.",
                    nameof(transaction));
            }

            checked
            {
                if (CurrentAmount - transaction.TransactionAmount != transaction.SourceAccountAmountAfter)
                {
                    throw new ArgumentException(
                        $"Unable to process {TransactionType.MoneyOut} transaction; Final amout {transaction.SourceAccountAmountAfter} not matching current account amount of {CurrentAmount} - transaction amout of {transaction.TransactionAmount}.",
                        nameof(transaction));
                }
            }

            AllTransactions = TransactionArrayHelper.AddOne(AllTransactions, transaction);
            CurrentAmount = transaction.SourceAccountAmountAfter;
        }

        internal void ProcessCollection(Transaction transaction)
        {
            if (transaction is null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction.TransactionType != TransactionType.MoneyIn)
            {
                throw new ArgumentException(
                    $"Invalid transaction type for payment. Expecting {TransactionType.MoneyIn}, but found {transaction.TransactionType}.",
                    nameof(transaction));
            }

            if (!string.Equals(Iban, transaction.SourceAccount.Iban, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"Invalid transaction source account. Expecting to be current IBAN '{Iban}', but found '{transaction.SourceAccount.Iban}'.",
                    nameof(transaction));
            }

            if (CurrentAmount != transaction.SourceAccountAmountBefore)
            {
                throw new ArgumentException(
                    $"Unable to process {TransactionType.MoneyIn} transaction; CurrentAmount {CurrentAmount} is different than the transaction calculated source amount before {transaction.SourceAccountAmountBefore}.",
                    nameof(transaction));
            }

            checked
            {
                if (CurrentAmount + transaction.TransactionAmount != transaction.SourceAccountAmountAfter)
                {
                    throw new ArgumentException(
                        $"Unable to process {TransactionType.MoneyIn} transaction; Final amout {transaction.SourceAccountAmountAfter} not matching current account amount of {CurrentAmount} + transaction amout of {transaction.TransactionAmount}.",
                        nameof(transaction));
                }
            }

            AllTransactions = TransactionArrayHelper.AddOne(AllTransactions, transaction);
            CurrentAmount = transaction.SourceAccountAmountAfter;
        }

        public BankStatement GenerateBankStatement(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
            {
                throw new ArgumentException($"From date {fromDate:yyyy-MM-dd} must be before to date {toDate:yyyy-MM-dd}");
            }

            // again because we don't use lists and LINQ yet, we do a couple of manual iterations
            Transaction[] matches = new Transaction[0];
            
            // count the number of matches and the amount before the range
            for (int i = 0; i < AllTransactions.Length; i++)
            {
                Transaction trx = AllTransactions[i];
                if (trx.Timestamp >= fromDate && trx.Timestamp <= toDate) 
                {
                    matches = TransactionArrayHelper.AddOne(matches, trx);
                }
                else if (trx.Timestamp > toDate)
                {
                    break;
                }
            }

            decimal startAmount = 0;
            if (matches.Length > 0) // we have matches
            {
                // start amount is the amount before the first transaction
                startAmount = matches[0].SourceAccountAmountBefore;
            }
            else // we don't have matches
            {
                if (AllTransactions.Length > 0) // the account has transactions
                {
                    if (fromDate >= AllTransactions[0].Timestamp) // we are looking after account was open
                    {
                        if (fromDate > AllTransactions[AllTransactions.Length - 1].Timestamp) // but also after the last transaction
                        {
                            // the amount after the last transaction
                            startAmount = AllTransactions[AllTransactions.Length - 1].SourceAccountAmountAfter;
                        }
                        else
                        {
                            // the initial deposit
                            startAmount = InitialDeposit;
                        }
                    }
                }
            }

            return new BankStatement(
                account: this,
                fromDate: fromDate,
                toDate: toDate,
                startAmount: startAmount,
                transactionsWithinPeriod: matches);
        }
    }
}