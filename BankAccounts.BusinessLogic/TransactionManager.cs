namespace BankAccounts.BusinessLogic
{
    public static class TransactionManager
    {
        public static void ProcessTransaction(
            Account payingAccount,
            Account receivingAccount,
            decimal amount,
            DateTime timestamp)
        {
            if (payingAccount is null)
            {
                throw new ArgumentNullException(nameof(payingAccount));
            }

            if (receivingAccount is null)
            {
                throw new ArgumentNullException(nameof(receivingAccount));
            }

            if (payingAccount.CurrentAmount < amount)
            {
                throw new ArgumentException($"Cannot pay {amount} from account {payingAccount.Iban} because of insufficient credit",
                    nameof(amount));
            }

            long trxNumber = TransactionNumberGenerator.GenerateNew();

            // paying account processes the payment
            payingAccount.ProcessPayment(new Transaction(
                number: trxNumber,
                timestamp: timestamp,
                transactionType: TransactionType.MoneyOut,
                sourceAccount: payingAccount,
                counterpartyAccount: receivingAccount,
                transactionAmount: amount));

            // receiving account processes the collection
            receivingAccount.ProcessCollection(new Transaction(
                number: trxNumber,
                timestamp: timestamp,
                transactionType: TransactionType.MoneyIn,
                sourceAccount: receivingAccount,
                counterpartyAccount: payingAccount,
                transactionAmount: amount));
        }
    }
}
