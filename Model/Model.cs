using System.Collections.Generic;
using Homework12.Model;
using Homework12.View;

namespace Homework12.Model;

internal interface IAccount
{
    // Положить деньги на счёт
    void Put(decimal sum);
    // Снять деньги со счёта
    decimal Withdraw(decimal sum);
}

public class Client
{
    public string? Name { get; set; }
    public List<Account>? Accounts { get; set; }
}

public abstract class Account : IAccount
{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public virtual void Put(decimal sum)
    {
        Balance += sum;
    }

    public virtual decimal Withdraw(decimal sum)
    {
        decimal result = 0;
        if (Balance >= sum)
        {
            Balance -= sum;
            result = sum;
        }
        else
        {
            const string message = "Недостаточно средств!";
            var infoWindow = new InfoWindow(message);
            infoWindow.ShowDialog();
        }
        return result;
    }
}

public class DepositAccount : Account
{
    // Additional properties specific to deposit account
}

public class RegularAccount : Account
{
    // Additional properties specific to regular account
}
