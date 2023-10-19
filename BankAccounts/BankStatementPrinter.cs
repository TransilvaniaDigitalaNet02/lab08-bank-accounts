using BankAccounts.BusinessLogic;

namespace BankAccounts
{
    internal static class BankStatementPrinter
    {
        public static void Print(BankStatement statement)
        {
            if (statement is null)
            {
                Console.WriteLine("Nothing to print, the bank statement is null");
                return;
            }

            string timestampLabel = nameof(BankStatement.BankStatementItem.Timestamp);
            const int timestampLabelWidth = 10;

            string transactionTypeLabel = nameof(BankStatement.BankStatementItem.TransactionType);
            const int transactionTypeLabelWidth = 15;

            string counterpartyLabel = nameof(BankStatement.BankStatementItem.Counterparty);
            const int counterpartyLabelWidth = 25;

            string amountBeforeLabel = nameof(BankStatement.BankStatementItem.AmountBefore);
            const int amountBeforeLabelWidth = 18;

            string transactionAmoutLabel = nameof(BankStatement.BankStatementItem.TransactionAmout);
            const int transactionAmoutLabelWidth = 18;

            string amountAfterLabel = nameof(BankStatement.BankStatementItem.AmountAfter);
            const int amountAfterLabelWidth = 18;

            string tableHeader = $"{timestampLabel,timestampLabelWidth} | " +
                                 $"{transactionTypeLabel,transactionTypeLabelWidth} | " +
                                 $"{counterpartyLabel,counterpartyLabelWidth} | " +
                                 $"{amountBeforeLabel,amountBeforeLabelWidth} | " +
                                 $"{transactionAmoutLabel,transactionAmoutLabelWidth} | " +
                                 $"{amountAfterLabel, amountAfterLabelWidth} ";

            string separator = new string(
                '-',
                timestampLabelWidth + 
                transactionTypeLabelWidth + 
                counterpartyLabelWidth + 
                amountBeforeLabelWidth +
                transactionAmoutLabelWidth +
                amountAfterLabelWidth + 
                3 * 5 + 1);

            Console.WriteLine($"Bank statement for {statement.Account.Owner} - IBAN: {statement.Account.Iban}");
            Console.WriteLine(separator);
            Console.WriteLine($"Start amount: {statement.StartAmount}");
            Console.WriteLine(separator);
            Console.WriteLine(tableHeader);
            Console.WriteLine(separator);

            foreach (BankStatement.BankStatementItem item in statement.Items)
            {
                Console.WriteLine($"{item.Timestamp, timestampLabelWidth:yyyy-MM-dd} | " +
                                  $"{item.TransactionType, transactionTypeLabelWidth} | " +
                                  $"{item.Counterparty, counterpartyLabelWidth} | " +
                                  $"{item.AmountBefore, amountBeforeLabelWidth} | " +
                                  $"{item.TransactionAmout, transactionAmoutLabelWidth} | " +
                                  $"{item.AmountAfter, amountAfterLabelWidth} ");
            }

            Console.WriteLine(separator);
            Console.WriteLine($"Final amount: {statement.FinalAmount}");
        }
    }
}
