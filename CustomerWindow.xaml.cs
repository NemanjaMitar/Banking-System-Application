using System.Windows;
using BankingSystem.Model;

namespace BankingSystem
{
    public partial class CustomerWindow : Window
    {
        private readonly Customer _editing;   // null = add mode, set = edit mode

        public Customer NewCustomer { get; private set; }

        // ADD mode
        public CustomerWindow()
        {
            InitializeComponent();
        }

        // EDIT mode — prefill from existing customer
        public CustomerWindow(Customer existing)
        {
            InitializeComponent();
            _editing = existing;

            if (existing != null)
            {
                Title = "Edit Customer";
                txtFirstName.Text = existing.FirstName;
                txtLastName.Text = existing.LastName;
                dpDateOfBirth.SelectedDate = existing.DateOfBirth;
                txtEmail.Text = existing.Email;
            }
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

            if (_editing != null)
            {
                // EDIT: write changes back onto the existing entity
                _editing.FirstName = firstName;
                _editing.LastName = lastName;
                _editing.DateOfBirth = dpDateOfBirth.SelectedDate;
                _editing.Email = txtEmail.Text?.Trim();
                NewCustomer = _editing;
            }
            else
            {
                // ADD: create a new entity
                NewCustomer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dpDateOfBirth.SelectedDate,
                    Email = txtEmail.Text?.Trim()
                };
            }

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