
using System;
namespace BankingSystem.Model
{
    /*
        * Dinarski acc - dete apstraktne klase Accbase
        * Sadrzi  valutu dinar ne moze imati neku stranu
        * Ne Moze da placa Internacionalno 
        * 
    */
    public class DinarskiAccount : AccountBase
    {
        public DinarskiAccount() : base() { }

        public DinarskiAccount(Guid customerId, decimal balance = 0)
            : base(customerId, balance) { }

        public override Currency GetCurrency() => Currency.RSD;
        public override bool CanReceiveInternationalTransfer() => false;
    }
}