namespace BankAccounts.BusinessLogic.Utilities
{
    internal static class TransactionArrayHelper
    {
        internal static Transaction[] AddOne(Transaction[] existingTransactions, Transaction transactionToAdd)
        {
            if (existingTransactions is null)
            {
                throw new ArgumentNullException(nameof(existingTransactions));
            }

            if (transactionToAdd is null)
            {
                throw new ArgumentNullException(nameof(transactionToAdd));
            }

            // instead of this we could have used lists
            Transaction[] result = new Transaction[existingTransactions.Length + 1];
            for (int i = 0; i < existingTransactions.Length; i++)
            {
                result[i] = existingTransactions[i];
            }

            // last index from the end
            result[result.Length - 1] = transactionToAdd;

            return result;
        }
    }
}
