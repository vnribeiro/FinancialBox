using FinancialBox.BuildingBlocks.Common;
using FinancialBox.Domain.FinancialGoals;

namespace FinancialBox.Domain.Users;

public class User : BaseEntity, IAggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public ICollection<FinancialGoal> FinancialGoals { get; private set; } = new List<FinancialGoal>();

    protected User() {}

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string newEmail)
    {
        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}