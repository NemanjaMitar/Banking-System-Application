using BankingSystem.Model;
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
        private readonly BankContext _context;
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
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand DetailsCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand AddCustomerCommand { get; }
        public MainWindowViewModel()
        {
            _context = new BankContext();

            Customers = new ObservableCollection<Customer>(
                _context.Customers.Include(c => c.Accounts).ToList());

            FilteredAccounts = new ObservableCollection<AccountBase>();

            if (Customers.Any())
            {
                SelectedCustomer = Customers.First();
            }

            AddCommand = new RelayCommand(OnAdd);
            DetailsCommand = new RelayCommand(OnDetails);
            DeleteCommand = new RelayCommand(OnDelete);
            AddCustomerCommand = new RelayCommand(OnAddCustomer);
        }

        private void LoadAccountsForSelectedCustomer()
        {
            FilteredAccounts.Clear();

            if (SelectedCustomer == null)
                return;

            var accounts = _context.Accounts
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == SelectedCustomer.Id)
                .ToList();

            foreach (var account in accounts)
            {
                FilteredAccounts.Add(account);
            }
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
                newAccount.AccountNumber = GenerateAccountNumber();

                _context.Accounts.Add(newAccount);
                _context.SaveChanges();

                LoadAccountsForSelectedCustomer();
            }
        }

        private void OnDetails()
        {
            if (SelectedAccount == null) return;

            MessageBox.Show(
                $"Account number: {SelectedAccount.AccountNumber}\n" +
                $"GUID: {SelectedAccount.AccountGuid}\n" +
                $"Balance: {SelectedAccount.Balance}\n" +
                $"Customer: {SelectedCustomer?.FullName}\n" +
                $"Currency: {SelectedAccount.GetCurrency()}\n" +
                $"International transfers: {SelectedAccount.CanReceiveInternationalTransfer()}");
        }

        private void OnDelete()
        {
            if (SelectedAccount == null) return;

            var res = MessageBox.Show("Are you sure?", "Deleting selected account", MessageBoxButton.YesNo);

            if (res == MessageBoxResult.Yes)
            {
                _context.Accounts.Remove(SelectedAccount);
                _context.SaveChanges();
                LoadAccountsForSelectedCustomer();
            }
        }

        private long GenerateAccountNumber()
        {
            if (!_context.Accounts.Any())
                return 1000000001;

            return _context.Accounts.Max(a => a.AccountNumber) + 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OnAddCustomer()
        {
            CustomerWindow window = new CustomerWindow();

            if (window.ShowDialog() == true && window.NewCustomer != null)
            {
                Customer newCustomer = window.NewCustomer;

                _context.Customers.Add(newCustomer);
                _context.SaveChanges();

                Customers.Add(newCustomer);

                if (SelectedCustomer == null)
                    SelectedCustomer = newCustomer;
            }
        }
    }
}