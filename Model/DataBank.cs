using Homework12.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework12.Model
{
    public static class DataBank
    {
        /// <summary>
        /// Получение списка клиентов
        /// </summary>
        /// <returns>список клиентов</returns>
        public static List<Client> GetAllClients()
        {
            using var db = new ApplicationContext();
            if (db.Clients != null)
            {
                var result = db.Clients.ToList();
                return result;
            }
            return new List<Client>();
        }

        /// <summary>
        /// Получение списка счетов
        /// </summary>
        /// <returns>список счетов</returns>
        public static List<Account> GetAllAccounts()
        {
            using var db = new ApplicationContext();
            if (db.Accounts != null)
            {
                var result = db.Accounts.ToList();
                return result;
            }
            return new List<Account>();
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
        /// <returns>результат выполнения</returns>
        public static void OpenAccount(Client client)
        {
            using var db = new ApplicationContext();
            var generated = GenerateAccountNumber();

            // проверка существования записи и генерация нового номера
            for (; ; )
            {
                var accountExists = db.Accounts != null && db.Accounts.Any(a => a.Number == generated);
                if (!accountExists) break;
                generated = GenerateAccountNumber();
            }
            
            // открытие нового счёта
            var newAccount = new Account { Number = generated, Sum = 0, ClientId = client.Id };
            db.Accounts?.Add(newAccount);
            db.SaveChanges();
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
