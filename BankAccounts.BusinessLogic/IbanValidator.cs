namespace BankAccounts.BusinessLogic
{
    internal static class IbanValidator
    {
        public static void ValidateIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentNullException(
                    nameof(iban),
                    "The IBAN of an account cannot be null /or empty /or whitespaces only.");
            }

            for (int i = 0; i < iban.Length; i++)
            {
                char c = iban[i];
                if (!char.IsLetterOrDigit(c))
                {
                    throw new FormatException(
                        $"The IBAN of an account can contain only letters and digits. Found invalid character '{c}'.");
                }
            }
        }
    }
}
