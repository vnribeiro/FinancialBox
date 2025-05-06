# FinancialBox - Financial Goal Management System

## 📖 About the Project
**FinancialBox** is a financial goal management system inspired by the "Caixinhas" feature of Nubank. It allows users to efficiently create, manage, and track financial objectives while providing robust features for transaction management, data validation, and reporting.

This project is being developed as part of a training program, focusing on building a scalable, well-architected, and maintainable application.

---

## 🚀 Features
### Box Management
- Create, Update, Delete, List, and View financial goals.
- Simulate Caixa growth based on monthly contributions and returns.
- Upload a cover image for each Caixa.
- Precalculate Caixa total value during new transactions using database transactions.

### Transaction Management
- Create, Delete, List, and View transactions.
- Validate data for precision (up to 2 decimal places) and prevent negative values.
- Supported transaction types: Deposit and Withdraw.

### Reports
- Generate detailed reports on Caixa evolution over time.

---

## 🛠️ Technologies and Patterns
This project is designed with a focus on **Clean Architecture** and **best practices** for modern software development.

### Stack
- **ASP.NET Core API**
- **Entity Framework Core** (SQL Server or SQLite)

### Advanced Features
- **Fluent Validation** for robust data validation.
- **Repository Pattern** to abstract data access.
- **Unit of Work** for transaction management.
- **Middleware** for centralized exception handling.
- **InputModel, ViewModel, DTOs** for data transfer and separation of concerns.
- **IEntityTypeConfiguration** for separate entity mappings.
- **Domain Events** to handle business logic decoupling.
- **CQRS Pattern** for command and query segregation.
- **Hosted Services** for background tasks.
- **Unit Testing** for ensuring reliability.

---

## 📝 Business Rules
1. Transactions:
   - Must be precise up to two decimal places.
   - Cannot have negative values.
   - Types: Deposit / Withdraw.

2. Caixa:
   - Status options: In Progress, Completed, Canceled, Paused.
   - Tracks related transactions and supports optional deadlines and monthly contribution goals.

---

## 📦 Entities and Data
### Financial Goal (Caixa)
| Property                 | Type         | Description                                         |
|--------------------------|--------------|-----------------------------------------------------|
| `Id`                    | `Guid`   | Unique identifier.                                 |
| `Title`                 | `string`     | Title of the financial goal.                      |
| `TargetAmount`          | `decimal`    | Target amount to be saved.                        |
| `Deadline`              | `datetime?`  | Optional deadline for the goal.                   |
| `IdealMonthlyContribution` | `decimal?` | Suggested monthly contribution.                   |
| `Status`                | `enum`       | Current status (In Progress, Completed, etc.).    |
| `Transactions`          | `Collection` | Associated transactions.                          |
| `CreatedAt`             | `datetime`   | Creation date.                                     |
| `IsDeleted`             | `bool`       | Soft delete flag.                                 |

### Transaction
| Property         | Type         | Description                                         |
|------------------|--------------|-----------------------------------------------------|
| `Id`            | `Guid`   | Unique identifier.                                 |
| `Amount`        | `decimal`    | Transaction amount (positive for deposits).        |
| `Type`          | `enum`       | Transaction type (Deposit, Withdraw).             |
| `TransactionDate` | `datetime`  | Date of the transaction (can be in the past).      |
| `CreatedAt`     | `datetime`   | Creation date.                                     |
| `IsDeleted`     | `bool`       | Soft delete flag.                                  |

---

## 🌐 How to Run the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/financialbox.git
   cd financialbox
