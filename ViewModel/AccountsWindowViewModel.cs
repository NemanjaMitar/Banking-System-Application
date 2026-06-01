using BankingSystem.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BankingSystem.ViewModel
{
    public class AccountsWindowViewModel : INotifyPropertyChanged
    {
        private readonly BankContext _context;

        public ObservableCollection<Customer> Customers { get; set; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private decimal _initialBalance;
        public decimal InitialBalance
        {
            get => _initialBalance;
            set
            {
                _initialBalance = value;
                OnPropertyChanged();
            }
        }

        public AccountsWindowViewModel()
        {
            _context = new BankContext();
            Customers = new ObservableCollection<Customer>(_context.Customers.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}