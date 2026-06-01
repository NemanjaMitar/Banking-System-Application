using System;

namespace BankingSystem.Model
{
    public class DevizniAccount : AccountBase
    {
        public Currency ForeignCurrency { get; set; }

        public DevizniAccount() : base()
        {
        }

        public DevizniAccount(int customerId, long accountNumber, Currency currency, decimal balance = 0)
            : base(customerId, accountNumber, balance)
        {
            if (currency == Currency.RSD)
                throw new ArgumentException("Devizni račun ne može biti u RSD.");

            ForeignCurrency = currency;
        }

        public override Currency GetCurrency() => ForeignCurrency;
        public override bool CanReceiveInternationalTransfer() => true;
    }
}