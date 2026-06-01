# Banking System

A desktop banking management application built with **WPF**, **MVVM**, and **Entity Framework 6** for managing customers and their bank accounts.

## Features

- Customer management
  - Add new customers
  - View all customers in a dedicated table
- Account management
  - Add bank accounts for a selected customer
  - Support for multiple accounts per customer
  - Delete accounts
  - View account details
- Master-detail UI
  - Left table shows customers
  - Right table shows accounts for the selected customer
- Account model
  - Each account belongs to exactly one customer
  - A customer can own multiple accounts
  - Automatic GUID generation for accounts
  - Automatic account number generation
- Currency handling
  - Dinarski accounts (`RSD`)
  - Devizni accounts (foreign currency support)

## Technologies

- C#
- WPF
- MVVM pattern
- Entity Framework 6
- SQL Server LocalDB

## Project Structure

```text
BankingSystem/
├── Model/
│   ├── Customer.cs
│   ├── AccountBase.cs
│   ├── DinarskiAccount.cs
│   ├── DevizniAccount.cs
│   ├── IAccount.cs
│   ├── ECurrency.cs
│   └── BankContext.cs
├── ViewModel/
│   ├── MainWindowViewModel.cs
│   ├── AccountsWindowViewModel.cs
│   └── RelayCommand.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── AccountsWindow.xaml
├── AccountsWindow.xaml.cs
└── App.config
```

## Data Model

### Customer
Represents a bank customer.

**Fields:**
- `Id`
- `FullName`
- `Accounts`

### AccountBase
Abstract base class for all accounts.

**Fields:**
- `AccountNumber`
- `AccountGuid`
- `Balance`
- `CustomerId`
- `Customer`
- `CurrencyDisplay`

### Account Types
- `DinarskiAccount` — local currency account (`RSD`)
- `DevizniAccount` — foreign currency account (`EUR`, `USD`, etc.)

## Relationship Rules

- One customer can have multiple accounts.
- One account belongs to exactly one customer.
- Accounts are filtered in the UI based on the selected customer.

## How It Works

1. Add a customer.
2. Select the customer in the customers table.
3. Add one or more accounts for that customer.
4. View account details or delete accounts as needed.

## Database

The application uses **Entity Framework 6** with **SQL Server LocalDB**.

Example connection string in `App.config`:

```xml
<connectionStrings>
  <add name="BankingSystemDb"
       connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankingSystemDb;Integrated Security=True;MultipleActiveResultSets=True"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## Notes

- If the model changes during development, the database may need to be recreated or migrated.
- For development, automatic database recreation can be enabled with an EF initializer.
- For production-like workflow, Code First Migrations are recommended.

## Possible Improvements

- Edit customer information
- Edit existing accounts
- Support transactions and transfers
- Validation for duplicate customers or invalid balances
- Search and filtering
- Better styling and themes
- Full CRUD for customers and accounts

## Author

Student project for practicing:
- Object-oriented design
- MVVM architecture
- Entity relationships in EF6
- WPF UI development
