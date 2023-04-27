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
        public static string AddClient(string nameFirst, string nameLast)
        {
            var result = "Клиент уже существует!";
            using var db = new ApplicationContext();

            // проверка существования записи
            var clientExists = db.Clients != null && db.Clients.Any(c => c.NameFirst == nameFirst && c.NameLast == nameLast);
            if (clientExists) return result;
            var newClient = new Client { NameFirst = nameFirst, NameLast = nameLast };
            db.Clients?.Add(newClient);
            db.SaveChanges();
            result = "Запись добавлена!";

            return result;
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="client">клиент</param>
        /// <returns>результат выполнения</returns>
        public static string DeleteClient(Client client)
        {
            var result = "Удаление невозможно";
            using var db = new ApplicationContext();

            db.Clients?.Remove(client);
            db.SaveChanges();
            result = $"Клиент {client.NameFirst} {client.NameLast} удален!";
            
            return result;
        }

        /// <summary>
        /// Открытие нового счёта
        /// </summary>
        /// <param name="client">клиент</param>
        /// <returns>результат выполнения</returns>
        public static string OpenAccount(Client client)
        {
            const string result = "Счёт открыт!";
            using var db = new ApplicationContext();
            var generated = GenerateAccountNumber();

            // проверка существования записи и генерация нового номера
            for (; ; )
            {
                var accountExists = db.Accounts != null && db.Accounts.Any(a => a.Id == generated);
                if (!accountExists) break;
                generated = GenerateAccountNumber();
            }
            
            // открытие нового счёта
            var newAccount = new Account { Id = generated, ClientId = client.Id };
            db.Accounts?.Add(newAccount);
            db.SaveChanges();

            return result;
        }

        /// <summary>
        /// Закрытие счёта
        /// </summary>
        /// <param name="account">счёт</param>
        /// <returns>результат выполнения</returns>
        public static string CloseAccount(Account account)
        {
            var result = "Удаление невозможно!";
            using var db = new ApplicationContext();

            db.Accounts?.Remove(account);
            db.SaveChanges();
            result = $"Счёт N{account.Id} удален!";
            
            return result;
        }

        /// <summary>
        /// Пополнение счёта
        /// </summary>
        /// <param name="oldAccount">счёт</param>
        /// <param name="newSum">сумма</param>
        /// <returns>результат выполнения</returns>
        public static string FundAccount(Account oldAccount, decimal newSum)
        {
            var result = "Пополнение невозможно!";
            using var db = new ApplicationContext();

            var account = db.Accounts?.FirstOrDefault(a => a.Id == oldAccount.Id);
            if (account == null) return result;
            account.Sum += newSum;
            db.SaveChanges();
            result = $"Счёт N{account.Id} пополнен на {newSum} монет!";

            return result;
        }


        /// <summary>
        /// Перевод средств
        /// </summary>
        /// <param name="accountFrom">счёт-отправитель</param>
        /// <param name="accountTo">счёт получатель</param>
        /// <param name="newSum">сумма</param>
        /// <returns>результат выполнения</returns>
        public static string TransferFunds(Account accountFrom, Account accountTo, decimal newSum)
        {
            var result = "Пополнение невозможно!";
            using var db = new ApplicationContext();

            var account = db.Accounts?.FirstOrDefault(a => a.Id == accountFrom.Id);
            var targetAccount = db.Accounts?.FirstOrDefault(b => b.Id == accountTo.Id);
            if (account == null) return result;
            account.Sum -= newSum;
            if (targetAccount == null) return result;
            targetAccount.Sum += newSum;
            db.SaveChanges();
            result = $"Со счёта N{account.Id} переведено {newSum} монет на счёт N{targetAccount.Id}!";

            return result;
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
    }
}
