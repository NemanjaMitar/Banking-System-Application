
using System;
namespace BankingSystem.Model
{
    public class DinarskiAccount : AccountBase
    {
        public DinarskiAccount() : base() { }

        public DinarskiAccount(Guid customerId, decimal balance = 0)
            : base(customerId, balance) { }

        public override Currency GetCurrency() => Currency.RSD;
        public override bool CanReceiveInternationalTransfer() => false;
    }
}