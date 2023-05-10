namespace Homework12.Model
{
    public class NonDepositAccount : Account
    {
        public NonDepositAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }
 
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый недепозитный счёт! Id счёта: {Id}", Sum));
        }
    }
}
