using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Model
{
    public abstract class AccountBase : INotifyPropertyChanged, IAccount
    {
        private decimal balance;
        private Guid accountGuid;
        private Guid customerId;
        private string iban;
        private Country country = Country.RS; // domestic default

        #region Properties, INotifyPropertyChanged

        [Key]
        [StringLength(34)]
        public string Iban
        {
            get => iban;
            set { if (iban != value) { iban = value; OnPropertyChanged(nameof(Iban)); } }
        }

        public Guid AccountGuid
        {
            get => accountGuid;
            set { if (accountGuid != value) { accountGuid = value; OnPropertyChanged(nameof(AccountGuid)); } }
        }

        public decimal Balance
        {
            get => balance;
            set { if (balance != value) { balance = value; OnPropertyChanged(nameof(Balance)); } }
        }

        [ForeignKey(nameof(Customer))]
        public Guid CustomerId
        {
            get => customerId;
            set { if (customerId != value) { customerId = value; OnPropertyChanged(nameof(CustomerId)); } }
        }

        public virtual Country Country
        {
            get => country;
            set { if (country != value) { country = value; OnPropertyChanged(nameof(Country)); } }
        }
        #endregion

        public virtual Customer Customer { get; set; }

        //========================| KONSTRUKTORI |===========================
        protected AccountBase()
        {
            AccountGuid = Guid.NewGuid();
        }

        protected AccountBase(Guid customerId, decimal balance = 0)
        {
            AccountGuid = Guid.NewGuid();
            CustomerId = customerId;
            Balance = balance;
        }

        #region IAccount
        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            Balance += amount;
        }

        public virtual void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
        }

        public decimal GetBalance() => Balance;
        public string GetIban() => Iban;

        public abstract Currency GetCurrency();
        public abstract bool CanReceiveInternationalTransfer();
        #endregion

        [NotMapped]
        public string CurrencyDisplay
        {
            get
            {
                switch (GetCurrency())
                {
                    case Currency.RSD: return "RSD - Dinar";
                    case Currency.EUR: return "EUR - Euro";
                    case Currency.USD: return "USD - US Dollar";
                    default: return GetCurrency().ToString();
                }
            }
        }

        #region INotifyPropertyChangedEvent
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}