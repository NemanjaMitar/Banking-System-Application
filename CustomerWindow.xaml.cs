using BankingSystem.Model;
using System.Windows;

namespace BankingSystem
{
    public partial class CustomerWindow : Window
    {
        public Customer NewCustomer { get; private set; }

        public CustomerWindow()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text?.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Customer name is required.");
                return;
            }

            NewCustomer = new Customer
            {
                FullName = fullName
            };

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