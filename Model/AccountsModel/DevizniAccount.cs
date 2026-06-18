using System;

namespace BankingSystem.Model
{
    /*
     * Devizni acc - dete apstraktne klase Accbase
     * Sadrzi stranu valutu (EUR, USD)
     * Moze da placa Internacionalno
     * 
    */
    public class DevizniAccount : AccountBase
    {
        // Property Strane valute
        public Currency ForeignCurrency { get; set; }
        public override Currency GetCurrency() => ForeignCurrency;
        public override bool CanReceiveInternationalTransfer() => true;

        // ==============| Konstruktori |====================
        public DevizniAccount() : base() { }

        public DevizniAccount(Guid customerId, Currency currency, decimal balance = 0)
            : base(customerId, balance)
        {
            if (currency == Currency.RSD)
                throw new ArgumentException("Devizni račun ne može biti u RSD.");

            ForeignCurrency = currency;
        }
    }
}