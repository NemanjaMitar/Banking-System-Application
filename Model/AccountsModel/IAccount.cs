using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Model
{

    //=================| INTERFEJS koji implementira svaki tip racuna u banci|====================
    public interface IAccount
    {
        // Dodavanje novca na racun
        void Deposit(decimal amount);
        // Skidanje novca sa racuna
        void Withdraw(decimal amount);
        // Vracanje trenutnog stanja
        decimal GetBalance();
        // Vracanje broja racuna
        long GetAccountNumber();
        // Vracanje valute koja se koristi za transakcije
        Currency GetCurrency();
        // Vracanje mogucnosti transfera izmedju dva racuna razlicitog tipa
        bool CanReceiveInternationalTransfer();
    }
}
