using BankingSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BankingSystem
{
    public partial class AccountsWindow : Window, INotifyPropertyChanged
    {
        private readonly Customer _customer;
        private readonly AccountBase _editing;   // null = add, set = edit

        public AccountBase NewAccount { get; private set; }

        public ObservableCollection<Country> CountryOptions { get; } =
            new ObservableCollection<Country>(
                Enum.GetValues(typeof(Country)).Cast<Country>());

        public ObservableCollection<Currency> CurrencyOptions { get; } =
            new ObservableCollection<Currency>(
                Enum.GetValues(typeof(Currency)).Cast<Currency>());

        private Country _selectedCountry = Country.RS;
        public Country SelectedCountry
        {
            get => _selectedCountry;
            set { if (_selectedCountry != value) { _selectedCountry = value; OnPropertyChanged(); } }
        }

        private decimal _initialBalance;
        public decimal InitialBalance
        {
            get => _initialBalance;
            set { _initialBalance = value; OnPropertyChanged(); }
        }

        private Currency _selectedCurrency = Currency.RSD;
        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                if (_selectedCurrency != value)
                {
                    _selectedCurrency = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanReceiveInternationalTransfer));
                }
            }
        }

        public bool CanReceiveInternationalTransfer => SelectedCurrency != Currency.RSD;

        // ADD mode
        public AccountsWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            DataContext = this;
        }

        // EDIT mode
        public AccountsWindow(Customer customer, AccountBase existing)
        {
            InitializeComponent();
            _customer = customer;
            _editing = existing;
            DataContext = this;

            if (existing != null)
            {
                Title = "Edit Account";
                InitialBalance = existing.Balance;
                SelectedCurrency = existing.GetCurrency();
                SelectedCountry = existing.Country;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InitialBalance < 0)
            {
                MessageBox.Show("Initial balance cannot be negative.",
                    "Invalid input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_editing != null)
            {
                _editing.Balance = InitialBalance;
                NewAccount = _editing;
            }
            else
            {
                NewAccount = CreateAccount(SelectedCurrency, _customer.Id, InitialBalance);
                if (NewAccount == null)
                    return;
                NewAccount.Country = SelectedCountry;   // ← country now applied
            }

            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private AccountBase CreateAccount(Currency currency, Guid customerId, decimal balance)
        {
            switch (currency)
            {
                case Currency.RSD:
                    return new DinarskiAccount(customerId, balance);          // ← no stray 0
                case Currency.USD:
                case Currency.EUR:
                    return new DevizniAccount(customerId, currency, balance); // ← no stray 0
                default:
                    MessageBox.Show($"Accounts in {currency} are not supported yet.",
                        "Not implemented", MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}