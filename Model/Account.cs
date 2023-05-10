namespace Homework12.Model
{
    public abstract class Account : IAccount
    {
        #region События

        //Событие, возникающее при выводе денег
        protected internal event AccountStateHandler Withdrawn = null!;
        // Событие возникающее при добавлении на счёт
        protected internal event AccountStateHandler Added = null!;
        // Событие возникающее при открытии счёта
        protected internal event AccountStateHandler Opened = null!;
        // Событие возникающее при закрытии счёта
        protected internal event AccountStateHandler Closed = null!;
        // Событие возникающее при начислении процентов
        protected internal event AccountStateHandler Calculated = null!;

        #endregion

        #region Переменные
        
        private static int _counter = 0;
        protected int Days = 0; // время с момента открытия счёта

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sum">сумма</param>
        /// <param name="percentage">процент</param>
        protected Account(decimal sum, int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++_counter;
        }
 
        /// <summary>
        /// Текущая сумма на счету
        /// </summary>
        public decimal Sum { get; private set; }

        /// <summary>
        /// Процент начислений
        /// </summary>
        public int Percentage { get; }

        /// <summary>
        /// Уникальный идентификатор счёта
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Вызов событий
        /// </summary>
        /// <param name="e"></param>
        /// <param name="handler"></param>
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            handler?.Invoke(this, e);
        }

        #region Вызов отдельных событий. Для каждого события определяется свой виртуальный метод

        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawn(AccountEventArgs e)
        {
            CallEvent(e, Withdrawn);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Метод внесения средств на счёт
        /// </summary>
        /// <param name="sum">сумма</param>
        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("На счет поступило " + sum, sum));
        }

        /// <summary>
        /// Метод снятия со счёта 
        /// </summary>
        /// <param name="sum">сумма снятия</param>
        /// <returns>сколько снято со счёта</returns>
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnWithdrawn(new AccountEventArgs($"Сумма {sum} снята со счёта {Id}", sum));
            }
            else
            {
                OnWithdrawn(new AccountEventArgs($"Недостаточно денег на счёте {Id}", 0));
            }
            return result;
        }

        /// <summary>
        /// Метод открытия счета
        /// </summary>
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {Id}", Sum));
        }

        /// <summary>
        /// Метод закрытия счета
        /// </summary>
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Счёт {Id} закрыт.  Итоговая сумма: {Sum}", Sum));
        }
 
        /// <summary>
        /// Метод увеличения дней
        /// </summary>
        protected internal void IncrementDays()
        {
            Days++;
        }

        /// <summary>
        /// Метод начисления процентов
        /// </summary>
        protected internal virtual void Calculate()
        {
            var increment = Sum * Percentage / 100;
            Sum += increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}", increment));
        }

        #endregion

    }
}
