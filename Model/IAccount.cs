using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model
{
    public interface IAccount
    {
        void Deposit(decimal amount);
        void Withdraw(decimal amount);
        decimal GetBalance();
        long GetAccountNumber();
        Currency GetCurrency();
        bool CanReceiveInternationalTransfer();
    }
}
