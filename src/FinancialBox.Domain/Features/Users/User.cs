using FinancialBox.Domain.Common;
using FinancialBox.Domain.Features.Users.ValueObjects;

namespace FinancialBox.Domain.Features.Users;

public class User : AggregateRoot
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public bool IsEmailConfirmed { get; private set; } = false;

    public ICollection<Role> Roles { get; private set; } = [];

    protected User() {}

    private User(string firstName, string lastName, Email email, Password password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public static User Create(string firstName, string lastName, Email email, Password password)
        => new(firstName, lastName, email, password);

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

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
    }

    public void AddRole(Role role)
    {
        if (Roles.All(r => r.Id != role.Id))
        {
            Roles.Add(role);
        }
    }

    public bool HasRole(Guid roleId)
    {
        return Roles.Any(r => r.Id == roleId);
    }

    public void RemoveRole(Guid roleId)
    {
        var role = Roles.FirstOrDefault(r => r.Id == roleId);

        if (role is not null)
        {
            Roles.Remove(role);
        }
    }

    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}
