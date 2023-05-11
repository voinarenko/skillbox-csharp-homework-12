using Homework12.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Homework12.ViewModel;

public abstract class AccountViewModel<T> where T : Account
{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public void PutMoney(decimal amount)
    {
        Balance += amount;
    }
}

public class DepositAccountViewModel : AccountViewModel<DepositAccount>
{
    // Additional properties specific to deposit account view model
}

public class RegularAccountViewModel : AccountViewModel<RegularAccount>
{
    // Additional properties specific to regular account view model
}

public class ClientViewModel
{
    public string Name { get; set; }
    public ObservableCollection<AccountViewModel<Account>> Accounts { get; set; }

    public void TransferMoney<T>(AccountViewModel<T> sourceAccount, AccountViewModel<T> destinationAccount, decimal amount) where T : Account
    {
        if (sourceAccount.Balance >= amount)
        {
            sourceAccount.Balance -= amount;
            destinationAccount.Balance += amount;
        }
        else
        {
            // Handle insufficient funds
        }
    }
}


public class BankingViewModel
{
    private const string DataFilePath = "data.json";

    public ObservableCollection<ClientViewModel> Clients { get; set; }

    public void PutMoneyOnAccount<T>(T accountViewModel, decimal amount) where T : AccountViewModel<Account>
    {
        accountViewModel.Balance += amount;
    }

    public void TransferMoney<T>(T sourceAccountViewModel, T destinationAccountViewModel, decimal amount) where T : AccountViewModel<Account>
    {
        if (sourceAccountViewModel.Balance >= amount)
        {
            sourceAccountViewModel.Balance -= amount;
            destinationAccountViewModel.Balance += amount;
        }
        else
        {
            // Handle insufficient funds
        }
    }

    public void LoadData()
    {
        if (File.Exists(DataFilePath))
        {
            var data = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText(DataFilePath));
            Clients = new ObservableCollection<ClientViewModel>(data.Select(c => new ClientViewModel
            {
                Name = c.Name,
                Accounts = new ObservableCollection<AccountViewModel<Account>>(c.Accounts.Select(a => a switch
                {
                    DepositAccount => new DepositAccountViewModel { AccountNumber = a.AccountNumber, Balance = a.Balance },
                    RegularAccount => new RegularAccountViewModel { AccountNumber = a.AccountNumber, Balance = a.Balance },
                    _ => throw new System.NotImplementedException()
                }))
            }));
        }
        else
        {
            Clients = new ObservableCollection<ClientViewModel>();
        }
    }

    public void SaveData()
    {
        var data = Clients.Select(c => new Client
        {
            Name = c.Name,
            Accounts = c.Accounts.Select(a => a switch
            {
                DepositAccountViewModel => new DepositAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
                RegularAccountViewModel => new RegularAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
                _ => throw new System.NotImplementedException()
            }).ToList()
        }).ToList();

        File.WriteAllText(DataFilePath, JsonConvert.SerializeObject(data));
    }
}
