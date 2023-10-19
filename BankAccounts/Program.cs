using BankAccounts.BusinessLogic;

namespace BankAccounts
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Account accountJohnDoe = new Account("John Doe", "ACCJHN1", 100);
            /*
             * John Doe: start from 100$
             * -------------------------------------------------------------------
             * Date       | Operation                | Initial | Amount  | Total
             * -------------------------------------------------------------------
             * 2020-10-01 | Receive from Rob.Stevens | 100     | 10      | 110
             * 2020-10-02 | Receive from Rob.Stevens | 110     | 50      | 160
             * 2020-10-03 | Payment to Rob.Stevens   | 160     | 30      | 130
             * -------------------------------------------------------------------
             * TOTAL                                                       130
             */

            Account accountRobertStevens = new Account("Robert Stevens", "ACCROB1", 300);
            /*
             * Robert Stevens: start from 300$
             * -------------------------------------------------------------------
             * Date       | Operation            | Initial | Amount  | Total
             * -------------------------------------------------------------------
             * 2020-10-01 | Payment to J.Doe     | 300     | 10      | 290
             * 2020-10-02 | Payment to J.Doe     | 290     | 50      | 240
             * 2020-10-03 | Receive from J.Doe   | 240     | 30      | 270
             * -------------------------------------------------------------------
             * TOTAL                                                   270
             */

            TransactionManager.ProcessTransaction(
                payingAccount: accountRobertStevens,
                receivingAccount: accountJohnDoe,
                amount: 10,
                new DateTime(2020, 10, 1));

            TransactionManager.ProcessTransaction(
                payingAccount: accountRobertStevens,
                receivingAccount: accountJohnDoe,
                amount: 50,
                new DateTime(2020, 10, 2));

            TransactionManager.ProcessTransaction(
                payingAccount: accountJohnDoe,
                receivingAccount: accountRobertStevens,
                amount: 30,
                new DateTime(2020, 10, 3));

            BankStatement statementJD = accountJohnDoe.GenerateBankStatement(
                fromDate: new DateTime(2020, 10, 1),
                toDate: new DateTime(2020, 10, 2));

            BankStatementPrinter.Print(statementJD);

            Console.WriteLine();
            Console.WriteLine();

            BankStatement statementRS = accountRobertStevens.GenerateBankStatement(
                fromDate: new DateTime(2020, 10, 1),
                toDate: new DateTime(2020, 10, 2));

            BankStatementPrinter.Print(statementRS);

            Console.WriteLine();
            Console.WriteLine();

            // no match
            BankStatement statementRS2 = accountRobertStevens.GenerateBankStatement(
               fromDate: new DateTime(2020, 10, 4),
               toDate: new DateTime(2020, 10, 5));

            BankStatementPrinter.Print(statementRS2);
        }
    }
}