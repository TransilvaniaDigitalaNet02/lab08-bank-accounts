using BankAccounts.BusinessLogic;

namespace BankAccounts.Tests
{
    public class AccountTests
    {
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Theory]
        public void Constructor_WhenOwnerIsNullOrWhitespaces_ShouldThrowArgumentNullException(string personName)
        {
            const string iban = "IBAN1234";
            try
            {
                Account account = new Account(personName, iban);
                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException argumentNullException &&
                            string.Equals(
                                argumentNullException.ParamName,
                                nameof(personName),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [Theory]
        public void Constructor_WhenIbanIsNullOrWhitespaces_ShouldThrowArgumentNullException(string iban)
        {
            const string owner = "Owner Abc";
            try
            {
                Account account = new Account(owner, iban);
                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException argumentNullException &&
                            string.Equals(
                                argumentNullException.ParamName,
                                nameof(iban),
                                StringComparison.OrdinalIgnoreCase));
            }
        }

        [InlineData("IBAN&%^*&")]
        [InlineData("IBAN IBAN")]
        [InlineData(".-][';?}{|")]
        [Theory]
        public void Constructor_WhenIbanContainsInvalidContent_ShouldThrowFormatException(string iban)
        {
            const string owner = "Owner Abc";
            try
            {
                Account account = new Account(owner, iban);
                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is FormatException);
            }
        }

        [Fact]
        public void Constructor_WhenInitialDepositIsNegative_ShouldThrowFormatException()
        {
            const string owner = "Owner Abc";
            const string iban = "IBAN123";
            const decimal initialDeposit = -100M;
            try
            {
                Account account = new Account(owner, iban, initialDeposit);
                Assert.Fail("Ctor should fail with null or whitespaces only owner.");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentException argumentException &&
                            string.Equals(
                                argumentException.ParamName, 
                                nameof(initialDeposit), 
                                StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}