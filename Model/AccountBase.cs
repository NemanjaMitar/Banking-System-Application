using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Model
{
    public abstract class AccountBase : INotifyPropertyChanged, IAccount
    {
        private long _accountNumber;
        private decimal _balance;
        private Guid _accountGuid;
        private int _customerId;

        [Key]
        public long AccountNumber
        {
            get => _accountNumber;
            set
            {
                if (_accountNumber != value)
                {
                    _accountNumber = value;
                    OnPropertyChanged(nameof(AccountNumber));
                }
            }
        }

        public Guid AccountGuid
        {
            get => _accountGuid;
            set
            {
                if (_accountGuid != value)
                {
                    _accountGuid = value;
                    OnPropertyChanged(nameof(AccountGuid));
                }
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        [ForeignKey(nameof(Customer))]
        public int CustomerId
        {
            get => _customerId;
            set
            {
                if (_customerId != value)
                {
                    _customerId = value;
                    OnPropertyChanged(nameof(CustomerId));
                }
            }
        }

        public virtual Customer Customer { get; set; }

        protected AccountBase()
        {
            AccountGuid = Guid.NewGuid();
        }

        protected AccountBase(int customerId, long accountNumber, decimal balance = 0)
        {
            AccountGuid = Guid.NewGuid();
            CustomerId = customerId;
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
        public decimal GetBalance() => Balance;
        public long GetAccountNumber() => AccountNumber;
        public abstract Currency GetCurrency();
        public abstract bool CanReceiveInternationalTransfer();
    }
}