namespace BankAccounts.BusinessLogic
{
    internal static class TransactionNumberGenerator
    {
        private static long counter = 0;

        public static long GenerateNew()
        {
            return ++counter;
        }

    }
}
