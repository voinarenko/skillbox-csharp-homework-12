namespace Homework12.Model
{
    internal class DepositAccount : Account
    {
        public DepositAccount(int number, decimal sum, int clientId, AccountType accountType)
        {
            Number = number;
            Sum = sum;
            ClientId = clientId;
            AccountType = accountType;
        }
    }
}
