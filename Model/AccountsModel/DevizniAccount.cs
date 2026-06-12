using System;

namespace BankingSystem.Model
{
    public class DevizniAccount : AccountBase
    {
        public Currency ForeignCurrency { get; set; }

        public DevizniAccount() : base() { }

        public DevizniAccount(Guid customerId, Currency currency, decimal balance = 0)
            : base(customerId, balance)
        {
            if (currency == Currency.RSD)
                throw new ArgumentException("Devizni račun ne može biti u RSD.");

            ForeignCurrency = currency;
        }

        public override Currency GetCurrency() => ForeignCurrency;
        public override bool CanReceiveInternationalTransfer() => true;
    }
}