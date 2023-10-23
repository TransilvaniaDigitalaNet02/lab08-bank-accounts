namespace BankAccounts.BusinessLogic.Validation
{
    internal static class PersonValidator
    {
        public static void ValidatePersonName(string personName)
        {
            if (string.IsNullOrWhiteSpace(personName))
            {
                throw new ArgumentNullException(
                    nameof(personName),
                    "A person name cannot be null /or empty /or whitespaces only");
            }
        }
    }
}
