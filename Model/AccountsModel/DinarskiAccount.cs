using System;

namespace BankingSystem.Model
{
    //=====================| Izvedena klasa za Dinarski racun | ========================
    // Karakteristike:
    /// <summary>
    /// - Valuta koja se koristi je dinar
    /// - Ne moze da se koristi za medjunarodne transakcije
    /// - Implementira sve osnovne funkcionalnosti IAccount Interfejsa, odnosno AccountBase klase
    /// </summary>
    public class DinarskiAccount : AccountBase
    {
        public DinarskiAccount() : base() { }
        public DinarskiAccount(Guid customerId, long accountNumber, decimal balance = 0): base(customerId, accountNumber, balance) { }

        public override Currency GetCurrency() => Currency.RSD;
        public override bool CanReceiveInternationalTransfer() => false;
    }
}