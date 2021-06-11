namespace Business.Constants
{
    public static class Messages
    {        
        public static string AccountCreated => "Account created.";
        public static string AccountsListed => "Accounts listed.";
        public static string TransactionCreated => "Transaction created.";
        public static string TransactionsListed => "Transactions listed.";
        public static string AccountNotFound => "Account number not exist.";
        public static string AccountAlreadyExist => "Account number should be unique.";
        public static string BadPrecision => "Precision should be limited to 2.";
        public static string InsufficientBalance => "Sender account has no available fund to finish the transaction.";
        public static string InvalidAccount => "Account number is invalid.";
        public static string InvalidAmount => "Amount must be a positive value.";
        public static string InvalidBalance => "Balance can not be negative.";
        public static string InvalidCurrency => "Currency code can only contain 'TRY', 'USD', 'EUR'.";
        public static string MismatchedCurrencies => "Currency codes not match.";
    }
}
