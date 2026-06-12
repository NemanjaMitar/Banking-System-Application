using System;

namespace BankingSystem.Model
{
    //=====================| Izvedena klasa za Devizni racun | ========================
    // Karakteristike:
    /// <summary>
    /// - Valuta koja se koristi moze biti dolar ili euro
    /// - Moze da se koristi za medjunarodne transakcije
    /// - Implementira sve osnovne funkcionalnosti IAccount Interfejsa, odnosno AccountBase klase
    /// </summary>
    public class DevizniAccount : AccountBase
    {
        public Currency ForeignCurrency { get; set; }

        public DevizniAccount() : base() { }

        public DevizniAccount(Guid customerId, long accountNumber, Currency currency, decimal balance = 0) : base(customerId, accountNumber, balance)
        {

            // Eventualna provera inicijalnog stanja, ili ovde ili pri validaciji
            // ...

            if (currency == Currency.RSD)
            {
                throw new ArgumentException("Devizni račun ne može biti u RSD.");
            }

            ForeignCurrency = currency;
        }

        public override Currency GetCurrency() => ForeignCurrency;
        public override bool CanReceiveInternationalTransfer() => true;
    }
}