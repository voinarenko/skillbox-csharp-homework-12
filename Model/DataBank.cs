using Homework12.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Homework12.Model
{
    public enum AccountType
    {
        [Description("Недепозитный")]
        NonDeposit,
        [Description("Депозитный")]
        Deposit
    }
    public static class DataBank
    {
        private static string? _infoMessage;

        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns>список клиентов</returns>
        public static List<Client> GetAllClients()
        {
            using var db = new ApplicationContext();
            if (db.Clients == null) return new List<Client>();
            var result = db.Clients.ToList();
            return result;
        }

        /// <summary>
        /// Получение списка счетов
        /// </summary>
        /// <returns>список счетов</returns>
        public static List<Account> GetAllAccounts()
        {
            using var db = new ApplicationContext();
            if (db.Accounts == null) return new List<Account>();
            var result = db.Accounts.ToList();
            return result;
        }

        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        /// <param name="nameFirst">имя клиента</param>
        /// <param name="nameLast">фамилия клиента</param>
        /// <returns>результат выполнения</returns>
        public static void AddClient(string nameFirst, string nameLast)
        {
            using var db = new ApplicationContext();

            // проверка существования записи
            var clientExists = db.Clients != null && db.Clients.Any(c => c.NameFirst == nameFirst && c.NameLast == nameLast);
            if (clientExists) return;
            var newClient = new Client { NameFirst = nameFirst, NameLast = nameLast };
            db.Clients?.Add(newClient);
            db.SaveChanges();
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="client">клиент</param>
        /// <returns>результат выполнения</returns>
        public static void DeleteClient(Client client)
        {
            using var db = new ApplicationContext();

            db.Clients?.Remove(client);
            db.SaveChanges();
        }

        /// <summary>
        /// Открытие нового счёта
        /// </summary>
        /// <param name="client">клиент</param>
        /// <param name="type">тип счёта</param>
        /// <returns>результат выполнения</returns>
        public static string OpenAccount(Client client, AccountType type)
        {
            using var db = new ApplicationContext();
            var generated = GenerateAccountNumber(); 
            var existingAccounts = GetAllAccountsByClientId(client.Id);
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
                _infoMessage = "Счёт существует!";
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
        /// <param name="sum">сумма</param>
        /// <param name="clientId">ID клиента</param>
        /// <param name="accountType">тип счёта</param>
        /// <returns></returns>
        public static Account? SelectAccountType(int number, decimal sum, int clientId, AccountType accountType)
        {
            Account? account = accountType switch
            {
                AccountType.NonDeposit => new NonDepositAccount(number, sum, clientId, accountType),
                AccountType.Deposit => new DepositAccount(number, sum, clientId, accountType),
                _ => null
            };

            return account;
        }

        /// <summary>
        /// Закрытие счёта
        /// </summary>
        /// <param name="account">счёт</param>
        /// <returns>результат выполнения</returns>
        public static void CloseAccount(Account account)
        {
            using var db = new ApplicationContext();

            db.Accounts?.Remove(account);
            db.SaveChanges();
        }

        /// <summary>
        /// Пополнение счёта
        /// </summary>
        /// <param name="oldAccount">счёт</param>
        /// <param name="newSum">сумма</param>
        /// <returns>результат выполнения</returns>
        public static void FundAccount(Account? oldAccount, decimal newSum)
        {
            using var db = new ApplicationContext();

            var account = db.Accounts?.FirstOrDefault(a => oldAccount != null && a.Number == oldAccount.Number);
            if (account == null) return;
            account.Sum += newSum;
            db.SaveChanges();
        }

        /// <summary>
        /// Перевод средств
        /// </summary>
        /// <param name="accountFrom">счёт-отправитель</param>
        /// <param name="accountTo">счёт получатель</param>
        /// <param name="newSum">сумма</param>
        /// <returns>результат выполнения</returns>
        public static void TransferFunds(Account? accountFrom, Account? accountTo, decimal newSum)
        {
            using var db = new ApplicationContext();

            var account = db.Accounts?.FirstOrDefault(a => a.Number == accountFrom!.Number);
            var targetAccount = db.Accounts?.FirstOrDefault(b => b.Number == accountTo!.Number);
            if (account == null) return;
            account.Sum -= newSum;
            if (targetAccount == null) return;
            targetAccount.Sum += newSum;
            db.SaveChanges();
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

        /// <summary>
        /// Получение списка счетов по id клиента
        /// </summary>
        /// <param name="id">id клиента</param>
        /// <returns>список счетов клиента</returns>
        public static List<Account> GetAllAccountsByClientId(int id)
        {
            using var db = new ApplicationContext();
            {
                List<Account> accounts = (from account in GetAllAccounts() where account.ClientId == id select account).ToList();
                return accounts;
            }
        }
    }
}
