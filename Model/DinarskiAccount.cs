namespace BankingSystem.Model
{
    public class DinarskiAccount : AccountBase
    {
        public DinarskiAccount() : base()
        {
        }

        public DinarskiAccount(int customerId, long accountNumber, decimal balance = 0)
            : base(customerId, accountNumber, balance)
        {
        }

        public override Currency GetCurrency() => Currency.RSD;
        public override bool CanReceiveInternationalTransfer() => false;
    }
}