using BankingSystem.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BankingSystem.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly BankContext context;
        private static readonly Random _rng = new Random();

        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<AccountBase> FilteredAccounts { get; set; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    OnPropertyChanged();
                    LoadAccountsForSelectedCustomer();
                }
            }
        }

        private AccountBase _selectedAccount;
        public AccountBase SelectedAccount
        {
            get => _selectedAccount;
            set { _selectedAccount = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand DetailsCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand AddCustomerCommand { get; }

        public RelayCommand UpdateAccountCommand { get; }
        public RelayCommand UpdateCustomerCommand { get; }
        public RelayCommand DeleteCustomerCommand { get; }

        public MainWindowViewModel()
        {

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;   // designer: skip DB access, no connection string needed
            context = new BankContext();

            Customers = new ObservableCollection<Customer>(
                context.Customers.Include(c => c.Accounts).ToList());

            FilteredAccounts = new ObservableCollection<AccountBase>();

            if (Customers.Any())
                SelectedCustomer = Customers.First();

            AddCommand = new RelayCommand(OnAdd);
            DetailsCommand = new RelayCommand(OnDetails);
            DeleteCommand = new RelayCommand(OnDelete);
            AddCustomerCommand = new RelayCommand(OnAddCustomer);
            UpdateAccountCommand = new RelayCommand(OnUpdateAccount);
            UpdateCustomerCommand = new RelayCommand(OnUpdateCustomer);
            DeleteCustomerCommand = new RelayCommand(OnDeleteCustomer);
        }

        private void OnUpdateAccount()
        {
            if (SelectedAccount == null) { MessageBox.Show("Select an account first."); return; }

            // reuse the account dialog as an editor, prefilled
            var window = new AccountsWindow(SelectedCustomer, SelectedAccount);
            if (window.ShowDialog() == true)
            {
                // SelectedAccount was edited in-place by the dialog
                context.Entry(SelectedAccount).State = EntityState.Modified;
                context.SaveChanges();
                LoadAccountsForSelectedCustomer();
            }
        }

        private void OnUpdateCustomer()
        {
            if (SelectedCustomer == null) { MessageBox.Show("Select a customer first."); return; }

            var window = new CustomerWindow(SelectedCustomer);   // editing overload
            if (window.ShowDialog() == true)
            {
                context.Entry(SelectedCustomer).State = EntityState.Modified;
                context.SaveChanges();
                // refresh the displayed row
                int i = Customers.IndexOf(SelectedCustomer);
                Customers[i] = SelectedCustomer;
            }
        }

        private void OnDeleteCustomer()
        {
            if (SelectedCustomer == null) return;

            var res = MessageBox.Show(
                $"Delete {SelectedCustomer.FullName} and ALL their accounts?",
                "Delete customer", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res != MessageBoxResult.Yes) return;

            // remove dependent accounts first (unless cascade delete is configured)
            var accounts = context.Accounts
                .Where(a => a.CustomerId == SelectedCustomer.Id).ToList();
            context.Accounts.RemoveRange(accounts);
            context.Customers.Remove(SelectedCustomer);
            context.SaveChanges();

            Customers.Remove(SelectedCustomer);
            SelectedCustomer = Customers.FirstOrDefault();
        }
        private void LoadAccountsForSelectedCustomer()
        {
            FilteredAccounts.Clear();

            if (SelectedCustomer == null)
                return;

            var accounts = context.Accounts
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == SelectedCustomer.Id)
                .ToList();

            foreach (var account in accounts)
                FilteredAccounts.Add(account);
        }

        private void OnAdd()
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            AccountsWindow window = new AccountsWindow(SelectedCustomer);

            if (window.ShowDialog() == true && window.NewAccount != null)
            {
                AccountBase newAccount = window.NewAccount;
                newAccount.Iban = GenerateIban(newAccount.Country);

                context.Accounts.Add(newAccount);
                context.SaveChanges();

                LoadAccountsForSelectedCustomer();
            }
        }

        // Builds a fresh 13-digit account seed, generates the IBAN, retries on collision.
        private string GenerateIban(Country country)
        {
            // account-number length per country
            int digits = country == Country.DE ? 10 : 13;
            long max = (long)Math.Pow(10, digits);
            long min = (long)Math.Pow(10, digits - 1);

            string iban;
            do
            {
                long seed = (long)(_rng.NextDouble() * (max - min)) + min;
                iban = IbanGenerator.Generate(country, "260", seed);
            }
            while (context.Accounts.Any(a => a.Iban == iban));

            return iban;
        }

        private void OnDetails()
        {
            if (SelectedAccount == null) return;

            MessageBox.Show(
                $"IBAN: {SelectedAccount.Iban}\n" +
                $"GUID: {SelectedAccount.AccountGuid}\n" +
                $"Balance: {SelectedAccount.Balance}\n" +
                $"Customer: {SelectedCustomer?.FullName}\n" +
                $"Country: {SelectedAccount.Country}\n" +
                $"Currency: {SelectedAccount.GetCurrency()}\n" +
                $"International transfers: {SelectedAccount.CanReceiveInternationalTransfer()}");
        }

        private void OnDelete()
        {
            if (SelectedAccount == null) return;

            var res = MessageBox.Show("Are you sure?", "Deleting selected account",
                                      MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                context.Accounts.Remove(SelectedAccount);
                context.SaveChanges();
                LoadAccountsForSelectedCustomer();
            }
        }

        private void OnAddCustomer()
        {
            CustomerWindow window = new CustomerWindow();

            if (window.ShowDialog() == true && window.NewCustomer != null)
            {
                Customer newCustomer = window.NewCustomer;

                context.Customers.Add(newCustomer);
                context.SaveChanges();

                Customers.Add(newCustomer);

                if (SelectedCustomer == null)
                    SelectedCustomer = newCustomer;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}