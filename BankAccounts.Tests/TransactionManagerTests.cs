using BankAccounts.BusinessLogic;

namespace BankAccounts.Tests
{
    public class TransactionManagerTests
    {
        [Fact]
        public void ProcessTransaction_WhenPayingAccountIsNull_ShouldThrowArgumentNullException()
        {
            Account payingAccount = null;
            Account receivingAccount = new Account("Receiver Owner", "IBANRCV");

            try
            {
                TransactionManager.ProcessTransaction(payingAccount, receivingAccount, 100, DateTime.Now);

                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException argumentException &&
                            string.Equals(
                                argumentException.ParamName,
                                nameof(payingAccount),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ProcessTransaction_WhenReceivingAccountIsNull_ShouldThrowArgumentNullException()
        {
            Account payingAccount = new Account("Paying Owner", "IBANPAY");
            Account receivingAccount = null;

            try
            {
                TransactionManager.ProcessTransaction(payingAccount, receivingAccount, 100, DateTime.Now);

                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException argumentException &&
                            string.Equals(
                                argumentException.ParamName,
                                nameof(receivingAccount),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ProcessTransaction_WhenAmountIsNegative_ShouldThrowArgumentNullException()
        {
            Account payingAccount = new Account("Paying Owner", "IBANPAY");
            Account receivingAccount = new Account("Receiver Owner", "IBANRCV");
            decimal amount = -100;

            try
            {
                TransactionManager.ProcessTransaction(payingAccount, receivingAccount, amount, DateTime.Now);

                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException argumentException &&
                            string.Equals(
                                argumentException.ParamName,
                                nameof(amount),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ProcessTransaction_WhenAmountIsGreaterThanPayingBalance_ShouldThrowArgumentNullException()
        {
            Account payingAccount = new Account("Paying Owner", "IBANPAY", 100);
            Account receivingAccount = new Account("Receiver Owner", "IBANRCV");
            decimal amount = 200;

            try
            {
                TransactionManager.ProcessTransaction(payingAccount, receivingAccount, amount, DateTime.Now);

                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException argumentException &&
                            string.Equals(
                                argumentException.ParamName,
                                nameof(amount),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [Fact]
        public void ProcessTransaction_WhenMultipleTransactionsBetweenSameAccounts_BalanceIsCalculatedCorrectly()
        {
            Account payingAccount = new Account("Paying Owner", "IBANPAY", 100);
            Account receivingAccount = new Account("Receiver Owner", "IBANRCV", 100);
            decimal amount = 100;

            DateTime startDate = new DateTime(2000, 1, 1);
            int noOfTransactions = 10;
            for (int i = 0; i < noOfTransactions; i++)
            {
                startDate.AddDays(i);

                TransactionManager.ProcessTransaction(payingAccount, receivingAccount, amount, startDate);

                // swap paying and receiving
                Account temp = payingAccount;
                payingAccount = receivingAccount;
                receivingAccount = temp;
            }

            // all transactions are recorded
            Assert.True(payingAccount.AllTransactions.Length == noOfTransactions);
            Assert.True(receivingAccount.AllTransactions.Length == noOfTransactions);

            // initial deposit is not changed
            Assert.True(payingAccount.InitialDeposit == 100);
            Assert.True(receivingAccount.InitialDeposit == 100);

            // current account remains unchanged
            // because all transactions have a swapped counterpart
            Assert.True(payingAccount.CurrentAmount == 100);
            Assert.True(receivingAccount.CurrentAmount == 100);
        }
    }
}
