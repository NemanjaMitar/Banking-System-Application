using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// =====================| Klasa klijenta koji poseduje vise racuna |=======================
// Ime, Prezime, Datum rodjenja, Email
// ID

namespace BankingSystem.Model
{
    public class Customer : INotifyPropertyChanged
    {
        #region Polja, get, set
        private Guid _id;
        private string _firstName;
        private string _lastName;
        private DateTime? _dateOfBirth;
        private string _email;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // we generate the Guid in code
        public Guid Id
        {
            get => _id;
            set { if (_id != value) { _id = value; OnPropertyChanged(nameof(Id)); } }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    OnPropertyChanged(nameof(FullName)); // keep computed prop in sync
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                    OnPropertyChanged(nameof(Age));
                }
            }
        }

        public string Email
        {
            get => _email;
            set { if (_email != value) { _email = value; OnPropertyChanged(nameof(Email)); } }
        }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        [NotMapped]
        public int Age
        {
            get
            {
                if (_dateOfBirth == null) return 0;
                var dob = _dateOfBirth.Value;
                var today = DateTime.Today;
                int age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--; // birthday not reached yet this year
                return age;
            }
        }
        #endregion

        // Racuni koje poseduje korisnik
        public virtual ICollection<AccountBase> Accounts { get; set; } = new List<AccountBase>();
        // Konstruktor
        public Customer()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() => FullName;


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}