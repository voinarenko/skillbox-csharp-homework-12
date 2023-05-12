using Homework12.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Documents;

namespace Homework12.Model;

public interface IAccount
{
    /// <summary>
    /// Положить деньги на счёт
    /// </summary>
    /// <param name="amount">сумма</param>
    public void PutMoney(decimal amount);

}

public abstract class Account<out T> where T : IAccount
{
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }

    protected Account(int account, decimal balance)
    {
        AccountNumber = account;
        Balance = balance;
    }

    public void PutMoney(decimal amount)
    {
        Balance += amount;
    }
}

public class DepositAccount : Account<IAccount>
{
    public DepositAccount(int account, decimal balance) : base(account, balance)
    {
    }
}

public class RegularAccount : Account<IAccount>
{
    public RegularAccount(int account, decimal balance) : base(account, balance)
    {
    }
}

public class Client
{
    public string? Name { get; set; }
    public ObservableCollection<Account<IAccount>>? Accounts { get; set; }

    public Client(string? name)
    {
        Name = name;
    }
}

public enum AccountType
{
    [Description("Недепозитный")]
    Regular,
    [Description("Депозитный")]
    Deposit
}
public class Bank
{
    //private const string DataFilePath = "data.json";

    public static ObservableCollection<Client>? Clients { get; set; }

    public void PutMoneyOnAccount<T>(T account, decimal amount) where T : Account<IAccount>
    {
        account.Balance += amount;
    }

    public void TransferMoney<T>(T sourceAccount, T destinationAccount, decimal amount) where T : Account<IAccount>
    {
        if (sourceAccount.Balance >= amount)
        {
            sourceAccount.Balance -= amount;
            destinationAccount.Balance += amount;
        }
        else
        {
            const string message = "Недостаточно средств!";
            PopUpWindow(message);
        }
    }


    /// <summary>
    /// Открытие нового счёта
    /// </summary>
    /// <param name="client">клиент</param>
    /// <param name="type">тип счёта</param>
    /// <returns>результат выполнения</returns>
    public static string OpenAccount(Client client, AccountType type)
    {
        var message = "Счёт существует!";
        var generated = GenerateAccountNumber(); 
        var existingAccounts = GetAllAccounts();
        var exists = false;

        // проверка существования записи и генерация нового номера
            for (; ; )
            {
                var accountExists = db.Accounts != null && db.Accounts.Any(a => a.Number == generated);
                if (!accountExists) break;
                generated = GenerateAccountNumber();
            }
            
            // проверка на существующий счёт указанного типа
            foreach (var a in existingAccounts.Where(a => a.TypeAcc == type))
            {
                exists = true;
            }

            // открытие нового счёта
            if (exists)
            {
                
            }
            else
            {
                var newAccount = SelectAccountType(generated, 0, client.Id, type);
                if (newAccount != null) db.Accounts?.Add(newAccount);
                db.SaveChanges();
                _infoMessage = "Счёт открыт!";
            }

            return _infoMessage;
        }

    /// <summary>
    /// Присвоение параметра "тип счёта", в зависимости от выбора
    /// </summary>
    /// <param name="number">номер счёта</param>
    /// <param name="amount">сумма</param>
    /// <param name="accountType">тип счёта</param>
    /// <returns></returns>
    public static Account<IAccount>? SelectAccountType(int number, decimal amount, AccountType accountType)
    {
        Account<IAccount>? account = accountType switch
        {
            AccountType.Regular => new Account<RegularAccount>(number, amount),
            AccountType.Deposit => new Account<DepositAccount>(number, amount),
            _ => null
        };

        return account;
    }

    /// <summary>
    /// Получение списка счетов
    /// </summary>
    /// <returns>список счетов</returns>
    public static IEnumerable<Account<IAccount>> GetAllAccounts()
    {
        if (Client.Accounts != null)
        {
            var accounts = Client.Accounts.ToList();
            if (Clients != null)
                foreach (var c in Clients)
                {
                    accounts.Add(c.Accounts.ToList());
                }
        }
    }

    /// <summary>
    /// Информационное окно
    /// </summary>
    /// <param name="message">сообщение</param>
    private void PopUpWindow(string message)
    {
        var infoWindow = new InfoWindow(message);
        infoWindow.ShowDialog();
    }

    /// <summary>
    /// Генерация номера счёта случайным образом
    /// </summary>
    /// <returns>номер счёта</returns>
    private static int GenerateAccountNumber()
    {
        const int min = 1000;
        const int max = 9999;
        var rdm = new Random();
        return rdm.Next(min, max);
    }

    #region Загрузка и сохранение данных

    //public void LoadData()
    //{
    //    if (File.Exists(DataFilePath))
    //    {
    //        var data = JsonConvert.DeserializeObject<List<Client>>(File.ReadAllText(DataFilePath));
    //        Clients = new ObservableCollection<Client>(data.Select(c => new ClientViewModel
    //        {
    //            Name = c.Name,
    //            Accounts = new ObservableCollection<Account<IAccount>>(c.Accounts.Select(a =>
    //                a switch
    //            {
    //                DepositAccount => new DepositAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
    //                RegularAccount => new RegularAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
    //                _ => throw new System.NotImplementedException()
    //            }))
    //        }));
    //    }
    //    else
    //    {
    //        Clients = new ObservableCollection<Client>();
    //    }
    //}

    //public void SaveData()
    //{
    //    var data = Clients.Select(c => new Client
    //    {
    //        Name = c.Name,
    //        Accounts = c.Accounts.Select(a => a switch
    //        {
    //            DepositAccount => new DepositAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
    //            RegularAccount => new RegularAccount { AccountNumber = a.AccountNumber, Balance = a.Balance },
    //            _ => throw new System.NotImplementedException()
    //        }).ToList()
    //    }).ToList();

    //    File.WriteAllText(DataFilePath, JsonConvert.SerializeObject(data));
    //}

    #endregion
}

