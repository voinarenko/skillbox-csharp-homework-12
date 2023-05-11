using System.Collections.Generic;

namespace Homework12.Model;

public class Client
{
    public string? Name { get; set; }
    public List<Account>? Accounts { get; set; }
}

public abstract class Account
{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
}

public class DepositAccount : Account
{
    // Additional properties specific to deposit account
}

public class RegularAccount : Account
{
    // Additional properties specific to regular account
}
