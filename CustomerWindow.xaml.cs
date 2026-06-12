using System.Windows;
using BankingSystem.Model;

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
            string firstName = txtFirstName.Text?.Trim();
            string lastName = txtLastName.Text?.Trim();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("First and last name are required.");
                return;
            }

            NewCustomer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dpDateOfBirth.SelectedDate,
                Email = txtEmail.Text?.Trim()
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