using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.FinancialGoals;
using FinancialBox.Domain.Features.Users.Events;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Domain.Features.Users;

public class User : BaseEntity, IAggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;

    public Guid RoleId { get; private set; } = RoleConstants.UserRoleId;
    public Role Role { get; private set; } = null!;

    public ICollection<FinancialGoal> FinancialGoals { get; private set; } = [];

    protected User() {}

    public User(string firstName, string lastName, Email email, Password password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public static User Register(string firstName, string lastName, Email email, Password password)
    {
        var user = new User(firstName, lastName, email, password);
        user.AddDomainEvent(new UserRegisteredEvent(user.Id, user.Email));
        return user;
    }

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdatePassword(Password newPassword)
    {
        Password = newPassword;
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail;
    }

    public void SetRole(Guid roleId)
    {
        RoleId = roleId;
    }

    public bool HasRole(Guid roleId)
    {
        return RoleId == roleId;
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}
