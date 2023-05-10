namespace Homework12.Model;

public interface IAccount
{
    // Положить деньги на счёт
    void Put(decimal sum);
    // Снять деньги со счёта
    decimal Withdraw(decimal sum);
}