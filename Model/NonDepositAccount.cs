namespace Homework12.Model
{
    internal class NonDepositAccount : Account
    {
        public NonDepositAccount(int number, decimal sum, int clientId, AccountType accountType)
        {
            Number = number;
            Sum = sum;
            ClientId = clientId;
            AccountType = accountType;
        }
    }
}
