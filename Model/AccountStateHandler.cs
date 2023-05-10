namespace Homework12.Model
{
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);
 
    public class AccountEventArgs
    {
        // Сообщение
        public string Message { get; private set;}
        // Сумма, на которую изменился счёт
        public decimal Sum { get; private set;}
 
        public AccountEventArgs(string message, decimal sum)
        {
            Message = message;
            Sum = sum;
        }
    }
}
