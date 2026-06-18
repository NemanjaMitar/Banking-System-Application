using BankingSystem.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;

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
            string email = txtEmail.Text?.Trim();
            DateTime? dob = dpDateOfBirth.SelectedDate;

            if (!ValidateInput(firstName, lastName, email, dob, out string error))
            {
                MessageBox.Show(error, "Invalid input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_editing != null)
            {
                _editing.FirstName = firstName;
                _editing.LastName = lastName;
                _editing.DateOfBirth = dob;
                _editing.Email = email;
                NewCustomer = _editing;
            }
            else
            {
                NewCustomer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dob,
                    Email = email
                };
            }

            DialogResult = true;
            Close();
        }

        private static bool ValidateInput(string firstName, string lastName,
            string email, DateTime? dob, out string error)
        {
            // Ime / prezime: obavezno + duzina
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                error = "First and last name are required.";
                return false;
            }

            if (firstName.Length < 2 || firstName.Length > 50)
            {
                error = "First name must be between 2 and 50 characters.";
                return false;
            }

            if (lastName.Length < 2 || lastName.Length > 50)
            {
                error = "Last name must be between 2 and 50 characters.";
                return false;
            }

            // Slova, razmak, crtica, apostrof (npr. "Anne-Marie", "O'Brien")
            const string namePattern = @"^[\p{L}][\p{L} '\-]*$";
            if (!Regex.IsMatch(firstName, namePattern))
            {
                error = "First name contains invalid characters.";
                return false;
            }

            if (!Regex.IsMatch(lastName, namePattern))
            {
                error = "Last name contains invalid characters.";
                return false;
            }

            // Email: obavezan + format
            if (string.IsNullOrWhiteSpace(email))
            {
                error = "Email is required.";
                return false;
            }

            const string emailPattern =
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                error = "Email is not in a valid format.";
                return false;
            }

            // Datum rodjenja: obavezan, ne u buducnosti, razuman opseg
            if (dob == null)
            {
                error = "Date of birth is required.";
                return false;
            }

            if (dob.Value.Date > DateTime.Today.AddYears(-18))
            {
                error = "Customer must be at least 18 years old.";
                return false;
            }

            if (dob.Value.Date > DateTime.Today)
            {
                error = "Date of birth cannot be in the future.";
                return false;
            }

            if (dob.Value.Date < DateTime.Today.AddYears(-120))
            {
                error = "Date of birth is not valid.";
                return false;
            }

            error = null;
            return true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}