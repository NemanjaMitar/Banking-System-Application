using BankingSystem.Model;
using System.Windows;

namespace BankingSystem
{
    public partial class AccountsWindow : Window
    {
        private readonly Customer _customer;

        public AccountBase NewAccount { get; set; }
        public decimal InitialBalance { get; set; }

        public AccountsWindow(Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            DataContext = this;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

            // Validiraj podatke koje je korisnik uneo
            NewAccount = new DinarskiAccount(_customer.Id, 0, InitialBalance);
            DialogResult = true;
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}